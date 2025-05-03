using System;

namespace Yucca
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--about")
            {
                DisplayAboutInfo();
            }
            else
            {
                Console.WriteLine("No valid command provided. Use '--about' to display information about the application.");
            }
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
