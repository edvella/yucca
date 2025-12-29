using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Yucca.Inventory;
using Yucca.Output;

namespace YuccaTests;

public class CsvExporterTests
{
    [Fact]
    public void GenerateSupplierCsv_WithEmptyList_ReturnsHeaderOnly()
    {
        // Arrange
        var suppliers = new List<Supplier>();

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        result.Should().Be("Id,Name,AddressLine1,AddressLine2,City,State,PostCode,CountryIso,CountryName,ContactPhone,Email,Website,TaxNumber");
    }

    [Fact]
    public void GenerateSupplierCsv_WithSingleSupplier_GeneratesCorrectCsv()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "123",
                Name = "ACME Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Suite 100",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@acme.com",
                Website = "https://acme.com",
                TaxNumber = "12-3456789"
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines.Should().HaveCount(2);
        lines[0].Should().Be("Id,Name,AddressLine1,AddressLine2,City,State,PostCode,CountryIso,CountryName,ContactPhone,Email,Website,TaxNumber");
        lines[1].Should().Be("123,ACME Corp,123 Main St,Suite 100,Springfield,IL,62701,US,United States,555-1234,contact@acme.com,https://acme.com,12-3456789");
    }

    [Fact]
    public void GenerateSupplierCsv_WithMultipleSuppliers_GeneratesAllRows()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Supplier One",
                AddressLine1 = "Address 1",
                AddressLine2 = "",
                City = "City 1",
                State = "State 1",
                PostCode = "12345",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "111-1111",
                Email = "one@example.com",
                Website = "www.one.com",
                TaxNumber = "TAX001"
            },
            new()
            {
                Id = "2",
                Name = "Supplier Two",
                AddressLine1 = "Address 2",
                AddressLine2 = "",
                City = "City 2",
                State = "State 2",
                PostCode = "54321",
                Country = new Country { IsoCode = "CA", Name = "Canada" },
                ContactPhone = "222-2222",
                Email = "two@example.com",
                Website = "www.two.com",
                TaxNumber = "TAX002"
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines.Should().HaveCount(3);
        lines[1].Should().Contain("Supplier One");
        lines[2].Should().Contain("Supplier Two");
    }

    [Fact]
    public void GenerateSupplierCsv_WithCommaInField_QuotesTheField()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Smith, Jones & Co",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Contain("\"Smith, Jones & Co\"");
    }

    [Fact]
    public void GenerateSupplierCsv_WithQuoteInField_EscapesQuote()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "O'Brien \"The Boss\" Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Contain("\"O'Brien \"\"The Boss\"\" Corp\"");
    }

    [Fact]
    public void GenerateSupplierCsv_WithNewlineInField_QuotesTheField()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Multi\nLine Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().StartWith("1,\"Multi");
        lines[1].Should().Contain("Line Corp\"");
    }

    [Fact]
    public void GenerateSupplierCsv_WithCarriageReturnInField_QuotesTheField()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Multi\rLine Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().StartWith("1,\"Multi");
        lines[1].Should().Contain("Line Corp\"");
    }

    [Fact]
    public void GenerateSupplierCsv_WithNullCountry_HandlesGracefully()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "No Country Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = null,
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Contain("1,No Country Corp,123 Main St,,Springfield,IL,62701,,");
    }

    [Fact]
    public void GenerateSupplierCsv_WithNullCountryProperties_HandlesGracefully()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Partial Country Corp",
                AddressLine1 = "123 Main St",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = null },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Contain("US,");
    }

    [Fact]
    public void GenerateSupplierCsv_WithEmptyStringFields_IncludesEmptyFields()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Minimal Corp",
                AddressLine1 = "",
                AddressLine2 = "",
                City = "",
                State = "",
                PostCode = "",
                Country = new Country { IsoCode = "", Name = "" },
                ContactPhone = "",
                Email = "",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Be("1,Minimal Corp,,,,,,,,,,,");
    }

    [Fact]
    public void GenerateSupplierCsv_WithSpecialCharactersInMultipleFields_QuotesAllAffected()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "1",
                Name = "Multi\"Line, Special Corp",
                AddressLine1 = "123 Main St, Suite 100",
                AddressLine2 = "",
                City = "Springfield",
                State = "IL",
                PostCode = "62701",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = "555-1234",
                Email = "contact@example.com",
                Website = "",
                TaxNumber = ""
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines[1].Should().Contain("\"Multi\"\"Line, Special Corp\"");
        lines[1].Should().Contain("\"123 Main St, Suite 100\"");
    }

    [Fact]
    public void GenerateSupplierCsv_WithLargeSupplierList_GeneratesAllRows()
    {
        // Arrange
        var suppliers = new List<Supplier>();
        for (int i = 1; i <= 100; i++)
        {
            suppliers.Add(new()
            {
                Id = i.ToString(),
                Name = $"Supplier {i}",
                AddressLine1 = $"Address {i}",
                AddressLine2 = "",
                City = $"City {i}",
                State = "ST",
                PostCode = "00000",
                Country = new Country { IsoCode = "US", Name = "United States" },
                ContactPhone = $"555-{i:0000}",
                Email = $"supplier{i}@example.com",
                Website = $"www.supplier{i}.com",
                TaxNumber = $"TAX{i:000}"
            });
        }

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        lines.Should().HaveCount(101); // 1 header + 100 data rows
        lines[0].Should().Be("Id,Name,AddressLine1,AddressLine2,City,State,PostCode,CountryIso,CountryName,ContactPhone,Email,Website,TaxNumber");
        lines[50].Should().Contain("Supplier 50");
        lines[100].Should().Contain("Supplier 100");
    }

    [Fact]
    public void GenerateSupplierCsv_PreservesFieldOrder()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new()
            {
                Id = "TEST-ID",
                Name = "Test Corp",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                City = "Test City",
                State = "TS",
                PostCode = "12345",
                Country = new Country { IsoCode = "XX", Name = "Test Country" },
                ContactPhone = "123-4567",
                Email = "test@test.com",
                Website = "www.test.com",
                TaxNumber = "TAX123"
            }
        };

        // Act
        var result = CsvExporter.GenerateSupplierCsv(suppliers);

        // Assert
        var lines = result.Split("\r\n");
        var dataParts = lines[1].Split(',');
        dataParts[0].Should().Be("TEST-ID");
        dataParts[1].Should().Be("Test Corp");
        dataParts[2].Should().Be("Line 1");
        dataParts[3].Should().Be("Line 2");
        dataParts[4].Should().Be("Test City");
        dataParts[5].Should().Be("TS");
        dataParts[6].Should().Be("12345");
        dataParts[7].Should().Be("XX");
        dataParts[8].Should().Be("Test Country");
        dataParts[9].Should().Be("123-4567");
        dataParts[10].Should().Be("test@test.com");
        dataParts[11].Should().Be("www.test.com");
        dataParts[12].Should().Be("TAX123");
    }
}
