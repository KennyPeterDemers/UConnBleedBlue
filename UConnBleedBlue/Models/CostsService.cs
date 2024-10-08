using OfficeOpenXml;

namespace UConnBleedBlue.Models
{
    public class CostsService
    {
        public string Err { get; set; } = "";

        public List<Cost> CostList = new List<Cost>();

        public double TotalCosts { get; set; } = 0.0;
        public CostsService()
        {
            try
            {
                string filePath = Directory.GetCurrentDirectory() + @"\\wwwroot\Data\Costs.xlsx";
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
                            Cost cost = new Cost();
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                if (col == 1)
                                {
                                    cost.CostId = row - 1;
                                    object x = excelWorksheet.Cells[row, col].Value;
                                    if (x != null)
                                    {
                                        cost.Description = x.ToString();
                                    }
                                    continue;
                                }
                                if (col == 2)
                                {
                                    if (excelWorksheet.Cells[row, col].Value == null)
                                    {
                                        cost.Amount = 0.0;
                                    }
                                    else
                                    {
                                        cost.Amount = (double)excelWorksheet.Cells[row, col].Value;
                                    }
                                    break;
                                }
                            }
                            CostList.Add(cost);
                            TotalCosts += cost.Amount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Err =  ex.Message;
            }
        }
    }
}
