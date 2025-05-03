using System;
using System.Reflection;

namespace Yucca;

public static class About
{
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
