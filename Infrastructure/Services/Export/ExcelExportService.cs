using OfficeOpenXml;

namespace StockFlow.API.Infrastructure.Services.Export
{
    public static class ExcelExportService
    {
        public static byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName = "Report")
        {
            ExcelPackage.License.SetNonCommercialPersonal("StockFlow Developer");

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();

            // Header
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            // Rows
            int row = 2;

            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value =
                        properties[col].GetValue(item);
                }

                row++;
            }

            return package.GetAsByteArray();
        }
    }
}