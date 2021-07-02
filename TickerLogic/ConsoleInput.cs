using System;
using System.IO;

namespace TickerLogic
{
    /// <summary>
    /// Helper functions for basic Console user-input collection.
    /// </summary>
    public static class ConsoleInput
    {
        /// <summary>
        /// Requests a fully-qualified path from the user and validates
        /// </summary>
        public static string GetPath(string prompt, bool allowBlank)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                Console.Write("> ");
                var path = Console.ReadLine().Trim();
                if (allowBlank && string.IsNullOrWhiteSpace(path)) return string.Empty;
                if (Path.IsPathFullyQualified(path))
                {
                    if (Directory.Exists(path)) return path;
                    Console.WriteLine("The path does not exist or is inaccessible. Please try again.");
                }
                else
                {
                    Console.WriteLine("A fully-qualified path is required (drive or server name, and directory).");
                }
            }
        }

        /// <summary>
        /// Returns a single keystroke compared to a list of accepted values
        /// </summary>
        public static string GetOneKey(string prompt, string validKeys)
        {
            Console.WriteLine(prompt);
            Console.Write("> ");
            while (true)
            {
                var k = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                if (validKeys.ToUpper().Contains(k))
                {
                    Console.WriteLine(k);
                    return k;
                }
            }
        }

        public static DateTime GetDate(string prompt)
        {
            Console.WriteLine(prompt);
            while(true)
            {
                Console.Write("> ");
                var date = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(date)) return DateTime.Now;
                if (!DateTime.TryParse(date, out var dt)) continue;
                return dt;
            }
        }

        public static decimal GetDecimal(string prompt)
        {
            Console.WriteLine(prompt);
            while(true)
            {
                Console.Write("> ");
                var amount = Console.ReadLine().Trim();
                if (!decimal.TryParse(amount, out var amt)) continue;
                if (amt == 0) continue;
                return amt;
            }
        }
    }
}
