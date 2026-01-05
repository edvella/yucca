using System;
using System.Threading.Tasks;

namespace Yucca.Operations.Supplier;

internal class Export(SupplierOps supplierOps) : IYuccaOperation
{
    private readonly SupplierOps _supplierOps = supplierOps;

    public static string RegisterCommand() => "supplier export";

    public async Task Execute(string[] parameters)
    {
        var named = CommandLine.ParseNamedArgs(parameters, 2);
        var filePath = CommandLine.Get(named, "file");

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File path is required. Use: yucca supplier export --file <path>");
            Console.ResetColor();
            return;
        }

        await _supplierOps.ExportSuppliersAsCsv(filePath);
    }
}
