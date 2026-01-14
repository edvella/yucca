using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yucca.Inventory;
using Yucca.Output;

namespace Yucca;

public class SupplierOps(ISupplierList supplierList)
{
    private readonly ISupplierList _supplierList = supplierList;

    public async Task AddSupplier(Supplier supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);

        await _supplierList.Save(supplier);

        CommandLine.ShowSuccess($"Supplier '{supplier.Name}' added successfully.");

        await ListSuppliers(OutputFormat.Table);
    }

    public async Task RemoveSupplier(string id)
    {
        var existing = await GetSupplier(id);

        await _supplierList.Remove(id);

        CommandLine.ShowSuccess($"Supplier '{existing.Name}' (id: {id}) removed successfully.");

        await ListSuppliers(OutputFormat.Table);
    }

    public async Task ListSuppliers(OutputFormat format = OutputFormat.Table)
    {
        var suppliers = await _supplierList.FilterByName("");

        if (!suppliers.Any())
        {
            CommandLine.ShowWarning("No suppliers found.");
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
            CommandLine.ShowError("Supplier id is required.");
            return null;
        }

        var supplier = await _supplierList.Get(id);
        if (supplier == null)
        {
            CommandLine.ShowError($"Supplier with id '{id}' not found.");
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

        CommandLine.ShowSuccess($"Supplier '{existing.Name}' updated successfully.");

        await ListSuppliers();
    }

    public async Task ExportSuppliersAsCsv(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            CommandLine.ShowError("File path is required for export.");
            return;
        }

        var suppliers = await _supplierList.FilterByName("");

        if (!suppliers.Any())
        {
            CommandLine.ShowWarning("No suppliers found to export.");
            return;
        }

        try
        {
            var csvContent = CsvExporter.GenerateSupplierCsv(suppliers);
            await File.WriteAllTextAsync(filePath, csvContent);

            CommandLine.ShowSuccess($"Successfully exported {suppliers.Count()} supplier(s) to '{filePath}'.");
        }
        catch (Exception ex)
        {
            CommandLine.ShowError($"Error exporting suppliers: {ex.Message}");
        }
    }
}