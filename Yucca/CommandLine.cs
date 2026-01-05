#nullable enable
using System;
using System.Collections.Generic;

namespace Yucca;

internal static class CommandLine
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
            if (a.StartsWith("--") || a.StartsWith("-"))
            {
                var key = a.TrimStart('-');
                string value = string.Empty;

                // support --key=value
                var eqIndex = key.IndexOf('=');
                if (eqIndex >= 0)
                {
                    value = key[(eqIndex + 1)..];
                    key = key[..eqIndex];
                }
                else
                {
                    // support --key value
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        value = args[i + 1];
                        i++; // consume value
                    }
                }

                dict[key] = value;
            }
            else
            {
                // treat as stray positional value; map to 'name' if not already present
                if (!dict.ContainsKey("name") && !string.IsNullOrWhiteSpace(a))
                    dict["name"] = a;
            }
        }

        return dict;
    }
}