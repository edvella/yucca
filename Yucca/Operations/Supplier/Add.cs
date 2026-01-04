using System;
using System.Threading.Tasks;
using Yucca.Inventory;

namespace Yucca.Operations.Supplier
{
    public class Add(SupplierOps supplierOps) : IYuccaOperation
    {
        private readonly SupplierOps _supplierOps = supplierOps;

        public static string RegisterCommand() => "supplier add";

        public async Task Execute(string[] parameters)
        {
            for (int i = 2; i < parameters.Length; i++)
            {
                if (parameters[i] == "--help" || parameters[i] == "-h")
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
                    Console.WriteLine("  yucca supplier add --name \"ACME Ltd\"");
                    Console.WriteLine("  yucca supplier add --name \"ACME Ltd\" --city \"New York\" --country-code US --phone \"0123456789\"");
                    return;
                }
            }

            var named = CommandLine.ParseNamedArgs(parameters, 2);
            var name = CommandLine.Get(named, "name");
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Supplier name is required. Use --name \"Supplier Name\".");
                Console.ResetColor();
                return;
            }

            var supplier = new Inventory.Supplier
            {
                Name = name!,
                AddressLine1 = CommandLine.Get(named, "address1"),
                AddressLine2 = CommandLine.Get(named, "address2"),
                City = CommandLine.Get(named, "city"),
                State = CommandLine.Get(named, "state"),
                PostCode = CommandLine.Get(named, "postcode"),
                ContactPhone = CommandLine.Get(named, "phone"),
                Email = CommandLine.Get(named, "email"),
                Website = CommandLine.Get(named, "website"),
                TaxNumber = CommandLine.Get(named, "tax")
            };

            var countryIso = CommandLine.Get(named, "country-code");
            if (!string.IsNullOrWhiteSpace(countryIso))
                supplier.Country = new Country { IsoCode = countryIso };

            await _supplierOps.AddSupplier(supplier);
        }
    }
}
