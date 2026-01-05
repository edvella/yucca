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
            builder.Services.AddKeyedTransient<IYuccaOperation, Update>(Update.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, Remove>(Remove.RegisterCommand());
            builder.Services.AddKeyedTransient<IYuccaOperation, Export>(Export.RegisterCommand());

            if (args.Length > 0)
            {
                var command = args[0];
                if (args.Length >= 2 && !args[1].StartsWith('-')) command += " " + args[1];

                var operation = builder.Services.BuildServiceProvider().GetKeyedService<IYuccaOperation>(command);

                if (operation != null)
                {
                    await operation.Execute(args);
                    Environment.Exit(0);
                }                
            }
            
            Console.WriteLine("No valid command provided. Use 'yucca help' to display information about the application.");
            Environment.Exit(1);
        }
    }
}
