using OfficeOpenXml;

namespace UConnBleedBlue.Models
{
    public class PlayersService
    {
        public string SelectedPlayerFinalYear { get; set; } = "";
        public string HeadCoach { get; set; } = "";
        public string ImageSource { get; set; } = "";
        
        public List<Player> PlayerList = new List<Player>();

        public string Err { get; set; } = "";
        public PlayersService()
        {
            try
            {
                string filePath = Directory.GetCurrentDirectory() + @"\\wwwroot\Data\Players.xlsx";
                FileInfo fileInfo = new FileInfo(filePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet? excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    if (excelWorksheet != null)
                    {
                        int totalColumns = excelWorksheet.Dimension.End.Column;
                        int totalRows = excelWorksheet.Dimension.End.Row;
                        for (int row = 2; row <= totalRows; row++)
                        {
                            Player player = new Player();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                // Name
                                if (col == 1)
                                {
                                    player.Id = row - 1;
                                    object x = excelWorksheet.Cells[row, col].Value;
                                    if ((x != null) && (string.IsNullOrEmpty((string)x) == false))
                                    {
                                        player.Name = (string)x;
                                    }
                                    else
                                    {
                                        player.Name = "Unknown Player";
                                    }
                                    continue;
                                }
                                // Email
                                if (col == 2)
                                {
                                    if (excelWorksheet.Cells[row, col].Value == null)
                                    {
                                        player.Email = "";
                                    }
                                    else
                                    {
                                        player.Email = (string)excelWorksheet.Cells[row, col].Value;
                                    }
                                    continue;
                                }
                                // Final Year
                                if (col == 3)
                                {
                                    var cell = excelWorksheet.Cells[row, col];
                                    player.FinalYear = string.IsNullOrWhiteSpace(cell.Text) ? "" : cell.Text.Trim();
                                    if(player.FinalYear == "2025")
                                    {
                                        player.FinalYear = "";
                                    }
                                    continue;
                                }
                                // Head Coach
                                if (col == 4)
                                {
                                    if (excelWorksheet.Cells[row, col].Value == null)
                                    {
                                        player.HeadCoach = "";
                                    }
                                    else
                                    {
                                        player.HeadCoach = (string)excelWorksheet.Cells[row, col].Value;
                                    }
                                    continue;
                                }
                                // Attending Tailgate`  `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `   `
                                if (col == 5)
                                {
                                    player.AttendingTailgate = (excelWorksheet.Cells[row, col].Value == null) ? false : Convert.ToBoolean(excelWorksheet.Cells[row, col].Value);
                                    continue;
                                }
                                // Donation Amount
                                if (col == 6)
                                {
                                    player.Donation = (excelWorksheet.Cells[row, col].Value == null) ? 0 : Convert.ToDecimal(excelWorksheet.Cells[row, col].Value);
                                    continue;
                                }
                                // Extra Tickets Needed
                                if (col == 7)
                                {
                                    player.ExtraTickets = (excelWorksheet.Cells[row, col].Value == null) ? 0 : Convert.ToInt32(excelWorksheet.Cells[row, col].Value);
                                    continue;
                                }
                            }
                            // I have some names that were not players but should get emails
                            if(player.HeadCoach != "MAIL-ONLY")
                            {
                                PlayerList.Add(player);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
            }
        }
    }
}
