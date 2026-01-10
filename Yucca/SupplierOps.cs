using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yucca.Inventory;
using Yucca.Output;

namespace Yucca;

public class SupplierOps
{
    private readonly ISupplierList _supplierList;

    public SupplierOps(ISupplierList supplierList)
    {
        _supplierList = supplierList;
    }

    public async Task AddSupplier(Supplier supplier)
    {
        if (supplier == null) throw new ArgumentNullException(nameof(supplier));

        await _supplierList.Save(supplier);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Supplier '{supplier.Name}' added successfully.");
        Console.ResetColor();

        await ListSuppliers(OutputFormat.Table);
    }

    public async Task RemoveSupplier(string id)
    {
        var existing = await GetSupplier(id);

        await _supplierList.Remove(id);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Supplier '{existing.Name}' (id: {id}) removed successfully.");
        Console.ResetColor();

        await ListSuppliers(OutputFormat.Table);
    }

    public async Task ListSuppliers(OutputFormat format = OutputFormat.Table)
    {
        var suppliers = await _supplierList.FilterByName("");

        if (!suppliers.Any())
        {
            Console.WriteLine("No suppliers found.");
            return;
        }

        switch (format)
        {
            case OutputFormat.Json:
                PrintAsJson(suppliers);
                break;
            case OutputFormat.Csv:
                PrintAsCsv(suppliers);
                break;
            case OutputFormat.Table:
            default:
                PrintAsTable(suppliers);
                break;
        }
    }

    private static void PrintAsTable(IEnumerable<Supplier> suppliers)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Suppliers:");
        Console.ResetColor();

        const int idWidth = 6;
        const int nameWidth = 30;
        const int cityWidth = 20;
        const int countryWidth = 13;
        const int phoneWidth = 20;
        const int websiteWidth = 30;

        string topBorder = $"┌{new string('─', idWidth)}┬{new string('─', nameWidth)}┬{new string('─', cityWidth)}┬{new string('─', countryWidth)}┬{new string('─', phoneWidth)}┬{new string('─', websiteWidth)}┐";
        string midBorder = $"├{new string('─', idWidth)}┼{new string('─', nameWidth)}┼{new string('─', cityWidth)}┼{new string('─', countryWidth)}┼{new string('─', phoneWidth)}┼{new string('─', websiteWidth)}┤";
        string bottomBorder = $"└{new string('─', idWidth)}┴{new string('─', nameWidth)}┴{new string('─', cityWidth)}┴{new string('─', countryWidth)}┴{new string('─', phoneWidth)}┴{new string('─', websiteWidth)}┘";

        Console.WriteLine(topBorder);
        Console.WriteLine($"│ {"ID".PadRight(idWidth - 1)}│ {"Name".PadRight(nameWidth - 1)}│ {"City".PadRight(cityWidth - 1)}│ {"Country Code".PadRight(countryWidth - 1)}│ {"Phone".PadRight(phoneWidth - 1)}│ {"Website".PadRight(websiteWidth - 1)}│");
        Console.WriteLine(midBorder);

        foreach (var supplier in suppliers)
        {
            string idStr = supplier.Id?.ToString() ?? "";
            string nameStr = supplier.Name ?? "";
            string cityStr = supplier.City ?? "";
            string countryStr = supplier.Country?.IsoCode ?? "";
            string phoneStr = supplier.ContactPhone ?? "";
            string websiteStr = supplier.Website ?? "";

            Console.WriteLine($"│ {idStr.PadRight(idWidth - 1)}│ {nameStr.PadRight(nameWidth - 1)}│ {cityStr.PadRight(cityWidth - 1)}│ {countryStr.PadRight(countryWidth - 1)}│ {phoneStr.PadRight(phoneWidth - 1)}│ {websiteStr.PadRight(websiteWidth - 1)}│");
        }

        Console.WriteLine(bottomBorder);
    }

    private static void PrintAsJson(IEnumerable<Supplier> suppliers)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(suppliers, options);
        Console.WriteLine(json);
    }

    private static void PrintAsCsv(IEnumerable<Supplier> suppliers)
    {
        string csv = CsvExporter.GenerateSupplierCsv(suppliers);
        Console.WriteLine(csv);
    }

    public async Task ViewSupplier(string id)
    {
        var supplier = await GetSupplier(id);
        if (supplier == null) return;

        Console.WriteLine($"ID: {supplier.Id}");
        Console.WriteLine($"Name: {supplier.Name}");
        Console.WriteLine($"Address Line 1: {supplier.AddressLine1}");
        Console.WriteLine($"Address Line 2: {supplier.AddressLine2}");
        Console.WriteLine($"City: {supplier.City}");
        Console.WriteLine($"State: {supplier.State}");
        Console.WriteLine($"Post Code: {supplier.PostCode}");
        Console.WriteLine($"Country: {supplier.Country}");
        Console.WriteLine($"Contact Phone: {supplier.ContactPhone}");
        Console.WriteLine($"Email: {supplier.Email}");
        Console.WriteLine($"Website: {supplier.Website}");
        Console.WriteLine($"Tax Number: {supplier.TaxNumber}");
    }

    private async Task<Supplier> GetSupplier(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Supplier id is required.");
            Console.ResetColor();
            return null;
        }

        var supplier = await _supplierList.Get(id);
        if (supplier == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Supplier with id '{id}' not found.");
            Console.ResetColor();
            return null;
        }

        return supplier;
    }

    internal async Task UpdateSupplier(
        string id,
        string name,
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string postCode,
        string countrycode,
        string contactPhone,
        string email,
        string website,
        string taxNumber
        )
    {
        var existing = await GetSupplier(id);
        if (existing == null) return;

        if (!string.IsNullOrEmpty(name)) existing.Name = name;
        if (!string.IsNullOrEmpty(addressLine1)) existing.AddressLine1 = addressLine1;
        if (!string.IsNullOrEmpty(addressLine2)) existing.AddressLine2 = addressLine2;
        if (!string.IsNullOrEmpty(city)) existing.City = city;
        if (!string.IsNullOrEmpty(state)) existing.State = state;
        if (!string.IsNullOrEmpty(postCode)) existing.PostCode = postCode;
        if (!string.IsNullOrEmpty(countrycode)) existing.Country = new Country { IsoCode = countrycode };
        if (!string.IsNullOrEmpty(contactPhone)) existing.ContactPhone = contactPhone;
        if (!string.IsNullOrEmpty(email)) existing.Email = email;
        if (!string.IsNullOrEmpty(website)) existing.Website = website;
        if (!string.IsNullOrEmpty(taxNumber)) existing.TaxNumber = taxNumber;

        await _supplierList.Save(existing);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Supplier '{existing.Name}' updated successfully.");
        Console.ResetColor();

        await ListSuppliers();
    }

    public async Task ExportSuppliersAsCsv(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File path is required for export.");
            Console.ResetColor();
            return;
        }

        var suppliers = await _supplierList.FilterByName("");

        if (!suppliers.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No suppliers found to export.");
            Console.ResetColor();
            return;
        }

        try
        {
            var csvContent = CsvExporter.GenerateSupplierCsv(suppliers);
            await File.WriteAllTextAsync(filePath, csvContent);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully exported {suppliers.Count()} supplier(s) to '{filePath}'.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error exporting suppliers: {ex.Message}");
            Console.ResetColor();
        }
    }
}