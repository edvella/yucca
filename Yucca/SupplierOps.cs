using System;
using System.Linq;
using System.Threading.Tasks;
using Yucca.Inventory;

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

        await ListSuppliers();
    }

    public async Task ListSuppliers()
    {
        var suppliers = await _supplierList.FilterByName("");

        if (suppliers.Any())
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
        else
        {
            Console.WriteLine("No suppliers found.");
        }
    }
}