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
                                if (col == 1)
                                {
                                    player.playerId = row - 1;
                                    object x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        player.playerName = x.ToString();
                                    }
                                    continue;
                                }
                                if (col == 2)
                                {
                                    if (excelWorksheet.Cells[row, col].Value == null)
                                    {
                                        player.playerEmail = "";
                                    }
                                    else
                                    {
                                        player.playerEmail = excelWorksheet.Cells[row, col].Value.ToString();
                                    }
                                    continue;
                                }
                                if (col == 3)
                                {
                                    player.playerFinalYear = (excelWorksheet.Cells[row, col].Value == null) ? " " : excelWorksheet.Cells[row, col].Value.ToString();
                                    continue;
                                }
                                if (col == 4)
                                {
                                    player.headCoach = (excelWorksheet.Cells[row, col].Value == null) ? " " : excelWorksheet.Cells[row, col].Value.ToString();
                                    continue;
                                }
                                if (col == 5)
                                {
                                    player.playerAttending2024 = Convert.ToBoolean(excelWorksheet.Cells[row, col].Value.ToString());
                                    continue;
                                }
                            }
                            PlayerList.Add(player);
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
