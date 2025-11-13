using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Yucca.Inventory;

namespace Yucca.Persistence.SQLServer;

public class SqlSupplierList : ISupplierList
{
    private readonly IConfiguration _configuration;

    public SqlSupplierList(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Create(Supplier supplier)
    {
        await using var connection = GetConnection();

        var query = @"
                INSERT INTO Suppliers (Name, AddressLine1, AddressLine2, City, State, PostCode, CountryIsoCode, ContactPhone, Email, Website, TaxNumber)
                VALUES (@Name, @AddressLine1, @AddressLine2, @City, @State, @PostCode, @CountryIsoCode, @ContactPhone, @Email, @Website, @TaxNumber);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

        var parameters = new DynamicParameters(supplier);
        parameters.Add("CountryIsoCode", supplier.Country?.IsoCode);

        int newId = await connection.QuerySingleAsync<int>(query, parameters);
        return newId.ToString();
    }

    public async Task<IEnumerable<Supplier>> FilterByName(string text)
    {
        await using var connection = GetConnection();

        var query = @"SELECT Id, Name, AddressLine1, AddressLine2, City, State, CountryIsoCode, PostCode, ContactPhone, Email, Website, TaxNumber
                        FROM Suppliers
                        WHERE Name LIKE @SearchText
                        ORDER BY Name ASC";

        var rows = await connection.QueryAsync<SupplierDbModel>(query, new { SearchText = $"%{text}%" });

        var suppliers = rows.Select(r => new Supplier
        {
            Id = r.Id,
            Name = r.Name ?? string.Empty,
            AddressLine1 = r.AddressLine1,
            AddressLine2 = r.AddressLine2,
            City = r.City,
            State = r.State,
            PostCode = r.PostCode,
            ContactPhone = r.ContactPhone,
            Email = r.Email,
            Website = r.Website,
            TaxNumber = r.TaxNumber,
            Country = new Country { IsoCode = r.CountryIsoCode ?? string.Empty }
        });

        return suppliers;
    }

    public async Task<Supplier?> Get(string id)
    {
        await using var connection = GetConnection();

        var row = await connection.QueryFirstOrDefaultAsync<SupplierDbModel>(
            "SELECT Id, Name, AddressLine1, AddressLine2, City, State, CountryIsoCode, PostCode, ContactPhone, Email, Website, TaxNumber " +
            "FROM Suppliers " +
            "WHERE ID = @Id",
            new { Id = id });

        if (row == null)
            return null;

        var supplier = new Supplier
        {
            Id = row.Id,
            Name = row.Name ?? string.Empty,
            AddressLine1 = row.AddressLine1,
            AddressLine2 = row.AddressLine2,
            City = row.City,
            State = row.State,
            PostCode = row.PostCode,
            ContactPhone = row.ContactPhone,
            Email = row.Email,
            Website = row.Website,
            TaxNumber = row.TaxNumber,
            Country = new Country { IsoCode = row.CountryIsoCode ?? string.Empty }
        };

        return supplier;
    }

    public async Task Remove(string id)
    {
        await using var connection = GetConnection();
        await connection.ExecuteAsync("DELETE FROM Suppliers WHERE Id = @id", new { Id = id });
    }

    public async Task<string> Update(Supplier supplier)
    {
        await using var connection = GetConnection();

        var query = @"
                UPDATE Suppliers SET Name = @Name, AddressLine1 = @AddressLine1, City = @City, State = @State, PostCode = @PostCode,
                CountryIsoCode = @CountryIsoCode, ContactPhone = @ContactPhone, Email = @Email, Website = @Website, TaxNumber = @TaxNumber WHERE Id = @Id";

        var parameters = new DynamicParameters(supplier);
        parameters.Add("CountryIsoCode", supplier.Country?.IsoCode);

        await connection.ExecuteAsync(query, parameters);
        return supplier.Id;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_configuration
            .GetConnectionString("YuccaDbConnection"));
    }
}
