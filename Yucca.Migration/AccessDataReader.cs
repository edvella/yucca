using System.Data.OleDb;
using Yucca.Inventory;

namespace Yucca.Migration;

public class AccessDataReader
{
    public async Task<List<Supplier>> ReadSuppliersAsync(string path, string password)
    {
        var suppliers = new List<Supplier>();

        try
        {
            var connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Jet OLEDB:Database Password={password}";
            using var connection = new OleDbConnection(connectionString);
            await connection.OpenAsync();

            var query = @"SELECT *
                         FROM Suppliers";

            using var command = new OleDbCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(reader["SuppName"]?.ToString()))
                    {
                        var supplier = new Supplier
                        {
                            Name = reader["SuppName"]?.ToString() ?? string.Empty,
                            AddressLine1 = reader["Addr1"]?.ToString() ?? string.Empty,
                            AddressLine2 = reader["Addr2"]?.ToString() ?? string.Empty,
                            City = reader["City"]?.ToString() ?? string.Empty,
                            State = reader["State"]?.ToString() ?? string.Empty,
                            PostCode = reader["ZIP"]?.ToString() ?? string.Empty,
                            ContactPhone = reader["TelNum"]?.ToString() ?? string.Empty,
                            Email = reader["Email"]?.ToString() ?? string.Empty,
                            Website = reader["Website"]?.ToString() ?? string.Empty,
                            TaxNumber = reader["TaxNum"]?.ToString() ?? string.Empty,
                        };

                        suppliers.Add(supplier);
                    }
                }
                catch (SupplierWithoutName ex)
                {
                    Console.WriteLine($"⚠️  Warning: Skipping supplier (ID: {reader["SuppName"]}) - {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to read from Access database.", ex);
        }

        return suppliers;
    }
}