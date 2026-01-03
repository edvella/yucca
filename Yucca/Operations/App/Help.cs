using System;
using System.Threading.Tasks;

namespace Yucca.Operations.App
{
    public class Help : IYuccaOperation
    {
        public static string RegisterCommand()
        {
            return "help";
        }

        public async Task Execute(string[] parameters)
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
            Console.WriteLine("  supplier export --file <path>   Export suppliers as CSV to specified file path");
            Console.WriteLine("  about           Display information about the application");
        }
    }
}
