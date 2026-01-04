using System;
using System.Threading.Tasks;

namespace Yucca.Operations.Supplier
{
    public class View(SupplierOps supplierOps) : IYuccaOperation
    {
        private readonly SupplierOps _supplierOps = supplierOps;

        public static string RegisterCommand() => "supplier view";

        public async Task Execute(string[] parameters)
        {
            var named = CommandLine.ParseNamedArgs(parameters, 2);
            var id = CommandLine.Get(named, "id");

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Supplier ID is required. Use yucca supplier view --id <id>.");
                Console.ResetColor();
                return;
            }

            await _supplierOps.ViewSupplier(id);
        }
    }
}
