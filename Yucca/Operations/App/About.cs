using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Yucca.Operations.App
{
    public class About : IYuccaOperation
    {
        public static string RegisterCommand()
        {
            return "about";
        }

        public async Task Execute(string[] parameters)
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

        public static Info GetAboutInfo()
        {
            return new Info
            {
                Header = "About Yucca",
                Title = "Yucca Accounts Manager",
                Description = GenerateDescription()
            };
        }

        private static string GenerateDescription()
        {
            string description = "An app for small to medium-sized businesses that need a comprehensive tool for managing inventory, sales, clients, suppliers, and financial operations.";

            description += $"\nVersion {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

            description += "\nDevelopment by Ed, started in 2020.";

            description += "\nBased on the original version by Edward Vella and Bernard Gatt © 2004-2007 Syntax Rebels. All rights reserved.";

            return description;
        }
    }
}
