#nullable enable
using System;
using System.Collections.Generic;

namespace Yucca;

public static class CommandLine
{

    public static string? Get(Dictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var v) ? v : null;
    }

    public static Dictionary<string, string> ParseNamedArgs(string[] args, int startIndex)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (int i = startIndex; i < args.Length; i++)
        {
            var a = args[i];
            if (a.StartsWith("--") || a.StartsWith('-'))
            {
                var key = a.TrimStart('-');

                if (i + 1 < args.Length && !args[i + 1].StartsWith('-'))
                {
                    dict[key] = args[i + 1];
                    i++;
                }
            }
        }

        return dict;
    }

    public static void ShowSuccess(string message)
    {
        ShowConsoleMessage(ConsoleColor.Green, message);
    }

    public static void ShowError(string message)
    {
        ShowConsoleMessage(ConsoleColor.Red, message);
    }

    public static void ShowWarning(string message)
    {
        ShowConsoleMessage(ConsoleColor.Yellow, message);
    }

    private static void ShowConsoleMessage(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}