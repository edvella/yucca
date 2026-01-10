using System;
using System.Threading.Tasks;
using Yucca.Output;

namespace Yucca.Operations.Supplier;

public class List(SupplierOps supplierOps) : IYuccaOperation
{
    private readonly SupplierOps _supplierOps = supplierOps;

    public static string RegisterCommand() => "supplier list";

    public async Task Execute(string[] parameters)
    {
        var args = CommandLine.ParseNamedArgs(parameters, 1);
        var formatStr = CommandLine.Get(args, "format") ?? "table";

        if (!Enum.TryParse<OutputFormat>(formatStr, ignoreCase: true, out var format))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid format '{formatStr}'. Supported formats: table, json, csv");
            Console.ResetColor();
            return;
        }

        await _supplierOps.ListSuppliers(format);
    }
}
