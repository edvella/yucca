#nullable enable
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Yucca.Inventory;
using Yucca.Operations;
using Yucca.Operations.App;
using Yucca.Operations.Supplier;
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

            builder.Services.AddKeyedTransient<IYuccaOperation, Help>(Help.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, About>(About.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, List>(List.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, View>(View.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, Add>(Add.RegisterCommand());

            if (args.Length > 0)
            {
                var command = args[0];
                if (args.Length >= 2 && !args[1].StartsWith('-')) command += " " + args[1];

                var operation = builder.Services.BuildServiceProvider().GetKeyedService<IYuccaOperation>(command);

                if (operation != null)
                    await operation.Execute(args);
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

                    var named = CommandLine.ParseNamedArgs(args, 2);
                    var id = CommandLine.Get(named, "id");
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Supplier id is required. Use --id \"Supplier ID\".");
                        Console.ResetColor();
                        return;
                    }

                    await supplierOps.UpdateSupplier(
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
                else if (args.Length >= 2 && args[0] == "supplier" && args[1] == "export")
                {
                    var named = CommandLine.ParseNamedArgs(args, 2);
                    var filePath = CommandLine.Get(named, "file");

                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("File path is required. Use: yucca supplier export --file <path>");
                        Console.ResetColor();
                        return;
                    }

                    await supplierOps.ExportSuppliersAsCsv(filePath);
                }
                else
                    Console.WriteLine("No valid command provided. Use 'yucca help' to display information about the application.");
            }
            else
                Console.WriteLine("No valid command provided. Use 'yucca help' to display information about the application.");
        }
    }
}
