using Blazorise.Extensions;
using OfficeOpenXml;
using System.Numerics;

namespace UConnBleedBlue.Models
{
    public class DonationsService
    {
        public int NumberOfPeopleDonating {  get; set; }
        public double TotalDonations { get; set; } = 0.0;

        public List<Donation> DonationList = new List<Donation>();

        string err = "";
        public DonationsService()
        {
            try
            {

                Dictionary<string, string> names = new Dictionary<string, string>();
                string filePath = Directory.GetCurrentDirectory() + @"\\wwwroot\Data\Donations.xlsx";
                FileInfo fileInfo = new FileInfo(filePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    object x;
                    ExcelWorksheet? excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    if (excelWorksheet != null)
                    {
                        int totalColumns = excelWorksheet.Dimension.End.Column;
                        int totalRows = excelWorksheet.Dimension.End.Row;
                        for (int row = 3; row <= totalRows; row++)
                        {
                            Donation donation = new Donation();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                if (col == 1)
                                {
                                    donation.PlayerId = row - 2;
                                    x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        donation.PlayerName = x.ToString();
                                    }
                                    continue;
                                }
                                if (col == 4)
                                {
                                    x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        donation.Amount = (double)x;
                                        TotalDonations += donation.Amount;
                                    }
                                    continue;
                                }
                            }
                            bool doubleEntry = false;
                            foreach (Donation d in DonationList)
                            {
                                if (d.PlayerName == donation.PlayerName)
                                {
                                    doubleEntry = true;
                                    break;
                                }
                            }
                            if (doubleEntry == false)
                            {
                                DonationList.Add(donation);
                            }
                        }
                    }
                }
                NumberOfPeopleDonating = DonationList.Count;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
        }
    }
}
