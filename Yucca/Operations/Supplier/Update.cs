using System;
using System.Threading.Tasks;

namespace Yucca.Operations.Supplier;

public class Update(SupplierOps supplierOps) : IYuccaOperation
{
    private readonly SupplierOps _supplierOps = supplierOps;

    public static string RegisterCommand() => "supplier update";

    public async Task Execute(string[] parameters)
    {
        for (int i = 2; i < parameters.Length; i++)
        {
            if (parameters[i] == "--help" || parameters[i] == "-h")
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

        var named = CommandLine.ParseNamedArgs(parameters, 2);
        var id = CommandLine.Get(named, "id");
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Supplier id is required. Use --id \"Supplier ID\".");
            Console.ResetColor();
            return;
        }

        await _supplierOps.UpdateSupplier(
            id!,
            CommandLine.Get(named, "name"),
            CommandLine.Get(named, "address1"),
            CommandLine.Get(named, "address2"),
            CommandLine.Get(named, "city"),
            CommandLine.Get(named, "state"),
            CommandLine.Get(named, "postcode"),
            CommandLine.Get(named, "country-code"),
            CommandLine.Get(named, "phone"),
            CommandLine.Get(named, "email"),
            CommandLine.Get(named, "website"),
            CommandLine.Get(named, "tax"));
    }
}
