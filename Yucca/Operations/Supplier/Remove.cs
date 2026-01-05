using System;
using System.Threading.Tasks;

namespace Yucca.Operations.Supplier;

internal class Remove(SupplierOps supplierOps) : IYuccaOperation
{
    private readonly SupplierOps _supplierOps = supplierOps;

    public static string RegisterCommand() => "supplier remove";

    public async Task Execute(string[] parameters)
    {
        for (int i = 2; i < parameters.Length; i++)
        {
            if (parameters[i] == "--help" || parameters[i] == "-h")
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

        var named = CommandLine.ParseNamedArgs(parameters, 2);
        var id = CommandLine.Get(named, "id");
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Supplier id is required. Use: yucca supplier remove <id>");
            Console.ResetColor();
            return;
        }

        await _supplierOps.RemoveSupplier(id);
    }
}
