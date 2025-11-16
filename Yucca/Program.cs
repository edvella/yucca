using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
                Console.WriteLine("  about           Display information about the application");
            }
            else if (args.Length > 0 && args[0] == "about")
                DisplayAboutInfo();
            else if (args.Length == 2 && args[0] == "supplier" && args[1] == "list")
                await supplierOps.ListSuppliers();
            else if (args.Length >= 3 && args[0] == "supplier" && args[1] == "add")
                await supplierOps.AddSupplier(args[2]);
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
    }
}
