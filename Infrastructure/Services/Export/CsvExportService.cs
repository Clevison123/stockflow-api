using System.Text;

namespace StockFlow.API.Infrastructure.Services.Export
{
    public static class CsvExportService
    {
        public static byte[] ExportToCsv<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            var sb = new StringBuilder();

            // Header
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item)?.ToString() ?? "";
                    return value.Replace(",", " "); // evitar quebrar CSV
                });

                sb.AppendLine(string.Join(",", values));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
