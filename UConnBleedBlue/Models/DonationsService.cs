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
                string filePath = Directory.GetCurrentDirectory() + @"\\wwwroot\Data\Football Alumni Donations.xlsx";
                FileInfo fileInfo = new FileInfo(filePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    object x;
                    int id = 1;
                    ExcelWorksheet? excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    if (excelWorksheet != null)
                    {
                        int totalColumns = excelWorksheet.Dimension.End.Column;
                        int totalRows = excelWorksheet.Dimension.End.Row;
                        for (int row = 2; row <= totalRows; row++)
                        {
                            Donation donation = new Donation();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                if (col == 1)
                                {
                                    donation.PlayerId = id++;
                                    x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        donation.PlayerName = (string)x;
                                        if (donation.PlayerName == "Total Vision Deland")
                                        {
                                            donation.PlayerName = "Ryan Timko";
                                        }
                                        else if (donation.PlayerName == "Carmen Ammirato")
                                        {
                                            donation.PlayerName = "Carmen and Marlene Ammirato";
                                        }
                                    }
                                    continue;
                                }
                                if (col == 3)
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
                            else
                            {
                                id--;
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
