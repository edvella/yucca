#nullable enable
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yucca.Inventory;
using Yucca.Persistence.SQLServer;

namespace Yucca
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<ISupplierList, SqlSupplierList>();
            builder.Services.AddTransient<SupplierOps>();

            var supplierOps = builder.Services.BuildServiceProvider().GetRequiredService<SupplierOps>();

            if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  yucca [command] [options]");
                Console.WriteLine();
                Console.WriteLine("Commands:");
                Console.WriteLine("  supplier list   List all suppliers");
                Console.WriteLine("  supplier view --id <id> View full supplier details");
                Console.WriteLine("  supplier add    Add a new supplier (supports named parameters)");
                Console.WriteLine("                 Example: yucca supplier add --name \"ACME Ltd\" --city \"New York\" --country US --phone \"0123456789\"");
                Console.WriteLine("  supplier update Update supplier details using same parameters as add command. Requires ID parameter.");
                Console.WriteLine("  supplier remove Remove a supplier by id");
                Console.WriteLine("  about           Display information about the application");
            }
            else if (args.Length > 0 && args[0] == "about")
                DisplayAboutInfo();
            else if (args.Length == 2 && args[0] == "supplier" && args[1] == "list")
                await supplierOps.ListSuppliers();
            else if (args.Length >= 3 && args[0] == "supplier" && args[1] == "view")
            {
                var named = ParseNamedArgs(args, 2);
                var id = Get(named, "id");

                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Supplier ID is required. Use yucca supplier view --id <id>.");
                    Console.ResetColor();
                    return;
                }

                await supplierOps.ViewSupplier(id);
            }
            else if (args.Length >= 3 && args[0] == "supplier" && args[1] == "add")
            {
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i] == "--help" || args[i] == "-h")
                    {
                        Console.WriteLine("Usage: yucca supplier add [options]");
                        Console.WriteLine();
                        Console.WriteLine("Required:");
                        Console.WriteLine("  --name \"Supplier Name\"        The supplier name (required)");
                        Console.WriteLine();
                        Console.WriteLine("Optional fields (named):");
                        Console.WriteLine("  --address1 \"Address line 1\"");
                        Console.WriteLine("  --address2 \"Address line 2\"");
                        Console.WriteLine("  --city \"City\"");
                        Console.WriteLine("  --state \"State\"");
                        Console.WriteLine("  --postcode \"Postal/ZIP code\"");
                        Console.WriteLine("  --country-code <ISO>   Country ISO code (e.g. US)");
                        Console.WriteLine("  --phone \"Contact phone\"");
                        Console.WriteLine("  --email \"Email address\"");
                        Console.WriteLine("  --website \"Website URL\"");
                        Console.WriteLine("  --tax \"Tax number\"");
                        Console.WriteLine();
                        Console.WriteLine("Examples:");
                        Console.WriteLine("  dotnet run -- supplier add --name \"ACME Ltd\"");
                        Console.WriteLine("  dotnet run -- supplier add --name \"ACME Ltd\" --city \"New York\" --country-code US --phone \"0123456789\"");
                        return;
                    }
                }

                var named = ParseNamedArgs(args, 2);
                var name = Get(named, "name");
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Supplier name is required. Use --name \"Supplier Name\".");
                    Console.ResetColor();
                    return;
                }

                var supplier = new Supplier
                {
                    Name = name!,
                    AddressLine1 = Get(named, "address1"),
                    AddressLine2 = Get(named, "address2"),
                    City = Get(named, "city"),
                    State = Get(named, "state"),
                    PostCode = Get(named, "postcode"),
                    ContactPhone = Get(named, "phone"),
                    Email = Get(named, "email"),
                    Website = Get(named, "website"),
                    TaxNumber = Get(named, "tax")
                };

                var countryIso = Get(named, "country-code");
                if (!string.IsNullOrWhiteSpace(countryIso))
                    supplier.Country = new Country { IsoCode = countryIso };

                await supplierOps.AddSupplier(supplier);
            }
            else if (args.Length >= 3 && args[0] == "supplier" && args[1] == "update")
            {
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i] == "--help" || args[i] == "-h")
                    {
                        Console.WriteLine("Usage: yucca supplier update [options]");
                        Console.WriteLine();
                        Console.WriteLine("Required:");
                        Console.WriteLine("  --id \"Supplier ID\"        The ID of the existing supplier");
                        Console.WriteLine();
                        Console.WriteLine("Optional fields (named):");
                        Console.WriteLine("  --name \"Supplier Name\"        The supplier name");
                        Console.WriteLine("  --address1 \"Address line 1\"");
                        Console.WriteLine("  --address2 \"Address line 2\"");
                        Console.WriteLine("  --city \"City\"");
                        Console.WriteLine("  --state \"State\"");
                        Console.WriteLine("  --postcode \"Postal/ZIP code\"");
                        Console.WriteLine("  --country-code <ISO>   Country ISO code (e.g. US)");
                        Console.WriteLine("  --phone \"Contact phone\"");
                        Console.WriteLine("  --email \"Email address\"");
                        Console.WriteLine("  --website \"Website URL\"");
                        Console.WriteLine("  --tax \"Tax number\"");
                        Console.WriteLine();                        
                        return;
                    }
                }

                var named = ParseNamedArgs(args, 2);
                var id = Get(named, "id");
                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Supplier id is required. Use --id \"Supplier ID\".");
                    Console.ResetColor();
                    return;
                }

                await supplierOps.UpdateSupplier(
                    id!,
                    Get(named, "name"),
                    Get(named, "address1"),
                    Get(named, "address2"),
                    Get(named, "city"),
                    Get(named, "state"),
                    Get(named, "postcode"),
                    Get(named, "country-code"),
                    Get(named, "phone"),
                    Get(named, "email"),
                    Get(named, "website"),
                    Get(named, "tax"));
            }
            else if (args.Length >= 3 && args[0] == "supplier" && args[1] == "remove")
            {
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i] == "--help" || args[i] == "-h")
                    {
                        Console.WriteLine("Usage: yucca supplier remove <id>");
                        Console.WriteLine();
                        Console.WriteLine("Remove a supplier by its id.");
                        Console.WriteLine();
                        Console.WriteLine("Examples:");
                        Console.WriteLine("  dotnet run -- supplier remove 6f1a3b2c");
                        return;
                    }
                }

                string? id = args[2];
                if (string.IsNullOrWhiteSpace(id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Supplier id is required. Use: yucca supplier remove <id>");
                    Console.ResetColor();
                    return;
                }

                await supplierOps.RemoveSupplier(id);
            }
            else
                Console.WriteLine("No valid command provided. Use '--help' to display information about the application.");
        }

        private static void DisplayAboutInfo()
        {
            var about = About.GetAboutInfo();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine(about.Title);
            Console.WriteLine("========================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(about.Description);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.ResetColor();
        }

        private static Dictionary<string, string> ParseNamedArgs(string[] args, int startIndex)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = startIndex; i < args.Length; i++)
            {
                var a = args[i];
                if (a.StartsWith("--") || a.StartsWith("-"))
                {
                    var key = a.TrimStart('-');
                    string value = string.Empty;

                    // support --key=value
                    var eqIndex = key.IndexOf('=');
                    if (eqIndex >= 0)
                    {
                        value = key[(eqIndex + 1)..];
                        key = key[..eqIndex];
                    }
                    else
                    {
                        // support --key value
                        if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                        {
                            value = args[i + 1];
                            i++; // consume value
                        }
                    }

                    dict[key] = value;
                }
                else
                {
                    // treat as stray positional value; map to 'name' if not already present
                    if (!dict.ContainsKey("name") && !string.IsNullOrWhiteSpace(a))
                        dict["name"] = a;
                }
            }

            return dict;
        }

        private static string? Get(Dictionary<string, string> dict, string key)
        {
            return dict.TryGetValue(key, out var v) ? v : null;
        }
    }
}
