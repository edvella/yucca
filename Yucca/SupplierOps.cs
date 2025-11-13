using System;
using System.Linq;
using Yucca.Inventory;
using Yucca.Volatile;

namespace Yucca;

public class SupplierOps
{
    private static ISupplierList supplierList = new InMemorySupplierList();

    public static void AddSupplier(string name)
    {
        var supplier = new Supplier
        {
            Name = name
        };

        supplierList.Save(supplier).GetAwaiter().GetResult();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Supplier '{name}' added successfully.");
        Console.ResetColor();

        ListSuppliers();
    }

    public static void ListSuppliers()
    {
        var suppliers = supplierList.FilterByName("").GetAwaiter().GetResult();

        if (suppliers.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Suppliers:");
            Console.ResetColor();

            const int idWidth = 38;
            const int nameWidth = 40;
            string topBorder = $"┌{new string('─', idWidth)}┬{new string('─', nameWidth)}┐";
            string midBorder = $"├{new string('─', idWidth)}┼{new string('─', nameWidth)}┤";
            string bottomBorder = $"└{new string('─', idWidth)}┴{new string('─', nameWidth)}┘";

            Console.WriteLine(topBorder);
            Console.WriteLine($"│ {"ID".PadRight(idWidth - 1)}│ {"Name".PadRight(nameWidth - 1)}│");
            Console.WriteLine(midBorder);

            foreach (var supplier in suppliers)
            {
                string idStr = supplier.Id?.ToString() ?? "";
                string nameStr = supplier.Name ?? "";
                Console.WriteLine($"│ {idStr.PadRight(idWidth - 1)}│ {nameStr.PadRight(nameWidth - 1)}│");
            }

            Console.WriteLine(bottomBorder);
        }
        else
        {
            Console.WriteLine("No suppliers found.");
        }
    }
}