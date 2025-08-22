// DonationsService.cs
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.Globalization;
using System.Numerics;

namespace UConnBleedBlue.Models
{
    public class DonationsService
    {
        private readonly IWebHostEnvironment _env;
        private bool _loaded;

        public List<Donation> DonationList { get; } = new();
        public double TotalDonations => DonationList.Where(d => d.Amount > 0).Sum(d => d.Amount);
        public int NumberOfPeopleDonating => DonationList.Count(d => d.Amount > 0);
        public string? Error { get; private set; }

        public DonationsService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task EnsureLoadedAsync(
            string fileName = "players.xlsx",
            int nameCol = 1,
            int finalYearCol = 3,
            int amountCol = 6,
            int headerRows = 1)
        {
            if (_loaded) return Task.CompletedTask;

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var path = Path.Combine(_env.WebRootPath, "Data", fileName);
                if (!File.Exists(path))
                {
                    Error = $"File not found: {path}";
                    _loaded = true; // prevent retry loop on every render
                    return Task.CompletedTask;
                }

                using var pkg = new ExcelPackage(new FileInfo(path));
                var ws = pkg.Workbook.Worksheets.FirstOrDefault();
                if (ws == null || ws.Dimension == null)
                {
                    Error = "No worksheet or empty sheet.";
                    _loaded = true;
                    return Task.CompletedTask;
                }

                DonationList.Clear();
                int id = 1;
                int lastRow = ws.Dimension.End.Row;

                for (int row = headerRows + 1; row <= lastRow; row++)
                {
                    // Use .Text for a safe string (Excel may store numbers as numeric)
                    var name = ws.Cells[row, nameCol].Text?.Trim();
                    var finalYear = ws.Cells[row, finalYearCol].Text?.Trim();

                    var amtValue = ws.Cells[row, amountCol].Value; // raw object
                    var amtText = ws.Cells[row, amountCol].Text;  // formatted text

                    // Skip empty rows
                    if (string.IsNullOrWhiteSpace(name) && amtValue is null && string.IsNullOrWhiteSpace(amtText))
                        continue;

                    DonationList.Add(new Donation
                    {
                        PlayerId = id++,
                        PlayerName = name,
                        FinalYear = finalYear,
                        Amount = ToDouble(amtValue, amtText)
                    });
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                _loaded = true;
            }

            return Task.CompletedTask;
        }

        private static double ToDouble(object? v, string? fallbackText)
        {
            if (v == null)
            {
                // Sometimes EPPlus leaves .Value null but .Text has "$1,234.00"
                if (!string.IsNullOrWhiteSpace(fallbackText))
                {
                    if (double.TryParse(fallbackText, NumberStyles.Any, CultureInfo.CurrentCulture, out var ft)) return ft;
                    if (double.TryParse(fallbackText, NumberStyles.Any, CultureInfo.InvariantCulture, out ft)) return ft;
                }
                return 0;
            }

            if (v is double d) return d;
            if (v is decimal m) return (double)m;
            if (v is int i) return i;
            if (v is long l) return l;

            var s = v.ToString();
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out var x)) return x;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out x)) return x;
            return 0;
        }
    }
}