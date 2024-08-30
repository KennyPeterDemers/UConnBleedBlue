using OfficeOpenXml;

namespace UConnBleedBlue.Data
{
    public class PlayerService
    {
        public List<Player>? GetPlayers()
        {
            try
            {
                List<Player> players = new List<Player>();
                string filePath = Directory.GetCurrentDirectory() + @"\\wwwroot\players\Players.xlsx";
                FileInfo fileInfo = new FileInfo(filePath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet? excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    if (excelWorksheet != null)
                    {
                        int totalColumns = excelWorksheet.Dimension.End.Column;
                        int totalRows = excelWorksheet.Dimension.End.Row;
                        for (int row = 1; row <= totalRows; row++)
                        {
                            Player player = new Player();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                if (col == 1)
                                {
                                    player.playerId = row;
                                    object x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        player.playerName = x.ToString();
                                    }
                                    continue;
                                }
                                if (col == 2)
                                {
                                    player.playerFinalYear = excelWorksheet.Cells[row, col].Value.ToString();
                                    continue;
                                }
                                if (col == 3)
                                {
                                    player.playerAttending2024 = Convert.ToBoolean(excelWorksheet.Cells[row, col].Value.ToString());
                                    continue;
                                }
                            }
                            players.Add(player);
                        }
                    }
                }
                return players;
            }
            catch
            {
                return null;
            }
        }
    }
}
