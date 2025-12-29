using Yucca.Inventory;

namespace Yucca.Output
{
    public class CsvExporter
    {
        public static string GenerateSupplierCsv(List<Supplier> suppliers)
        {
            var csvLines = new List<string>
                {
                    "Id,Name,AddressLine1,AddressLine2,City,State,PostCode,CountryIso,CountryName,ContactPhone,Email,Website,TaxNumber"
                };

            foreach (var s in suppliers)
            {
                string countryIso = s.Country?.IsoCode ?? string.Empty;
                string countryName = s.Country?.Name ?? string.Empty;

                string[] fields = [
                    EscapeCsv(s.Id),
                        EscapeCsv(s.Name),
                        EscapeCsv(s.AddressLine1),
                        EscapeCsv(s.AddressLine2),
                        EscapeCsv(s.City),
                        EscapeCsv(s.State),
                        EscapeCsv(s.PostCode),
                        EscapeCsv(countryIso),
                        EscapeCsv(countryName),
                        EscapeCsv(s.ContactPhone),
                        EscapeCsv(s.Email),
                        EscapeCsv(s.Website),
                        EscapeCsv(s.TaxNumber)
                ];

                csvLines.Add(string.Join(',', fields));
            }

            return string.Join("\r\n", csvLines);
        }

        private static string EscapeCsv(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var needsQuotes = input.Contains(',') || input.Contains('\"') || input.Contains('\n') || input.Contains('\r');
            var escaped = input.Replace("\"", "\"\"");
            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }
    }
}