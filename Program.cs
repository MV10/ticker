using System;
using System.IO;
using System.Threading.Tasks;
using TickerLogic;

namespace ticker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\nticker\n\n(Press CTRL+C to exit at any time.)\n");

            var config = await Config.Read() ?? await Config.Prompt();
            if(config is null)
                ExitWriteLine("\nUnable to read or create configuration file. Aborting.");

            // No arguments -- dump a summary of the current account's holdings
            if(args.Length == 0)
            {
                if(string.IsNullOrWhiteSpace(config.ActiveAccountId))
                    ExitWriteLine("\nNo account is active.\nUse the -? or -help switch for usage information.");

                var acct = await config.GetActiveAccount();
                ConsoleDisplay.HoldingsSummary(acct);
                Environment.Exit(0);
            }

            switch(args[0].ToUpper())
            {
                case "-?":
                case "-HELP":
                    {
                        Console.WriteLine("-?, -help\tlist available switches (this list)");
                        Console.WriteLine("-path [dir]\tshow the storage path; specify [dir] to change it");
                        Console.WriteLine("-acct [id]\tlist accounts; specify [id] to set active account");
                        Console.WriteLine("-add\t\tadd new account (prompts)");
                        Console.WriteLine("-cash\t\tcash transaction (prompts)");
                        Console.WriteLine("[symbol]\tdump transaction history of the symbol");
                        Console.WriteLine("-buy [symbol]\tbuy shares (prompts)");
                        Console.WriteLine("-sell [symbol]\tsell shares (prompts)");
                        Console.WriteLine("-div [symbol]\tbuy divident reinvestment shares (prompts)");
                        Console.WriteLine("(none)\t\tholdings summary of the active account");
                        break;
                    }

                // -path [dir] ... show the storage path; specify [dir] to change it (can include spaces without quotes)
                case "-PATH":
                    {
                        if(args.Length == 1) ExitWriteLine($"Account data files are stored here:\n{config.DataPath}", 0);

                        var location = string.Join(' ', args, 1, args.Length - 1);
                        Console.WriteLine($"Validating: {location}");
                        if (!Path.IsPathFullyQualified(location)) ExitWriteLine("A fully-qualified path is required (drive or server name, and directory).");
                        if (!Directory.Exists(location)) ExitWriteLine("The path does not exist or is inaccessible.");
                        config.DataPath = location;
                        await config.Write();
                        break;
                    }

                // -acct [id] ... list the accounts; specify [id] to set active account (can include spaces without quotes)
                case "-ACCT":
                    {
                        if(args.Length == 1)
                        {
                            ConsoleDisplay.ListAccounts(config);
                            Environment.Exit(0);
                        }

                        var id = string.Join(' ', args, 1, args.Length - 1);
                        var pathname = Path.Join(config.DataPath, $"{id}.tikr");
                        if (!File.Exists(pathname)) ExitWriteLine($"The requested account could not be found:\n{pathname}");
                        config.ActiveAccountId = id;
                        await config.Write();
                        break;
                    }

                // -add ... adds a new account via prompting
                case "-ADD":
                    {
                        if (args.Length > 1) ExitWriteLine($"The {args[0]} switch does not accept arguments.");

                        var acct = await ConsolePrompt.AddAccount(config);
                        if (acct == null) Environment.Exit(0);

                        if(ConsoleInput.GetOneKey("\nMake this account the default? (Y/N)", "YN").Equals("Y"))
                        {
                            config.ActiveAccountId = acct.Id;
                            await config.Write();
                        }
                        break;
                    }

                // -cash ... adds a cash transaction via prompting
                case "-CASH":
                    {
                        if (args.Length > 1) ExitWriteLine($"The {args[0]} switch does not accept arguments.");
                        var acct = await config.GetActiveAccount();
                        ConsolePrompt.AddCashTransaction(acct);
                        await config.WriteAccount(acct);
                        break;
                    }

                // -trans ... dump cash transaction history
                case "-TRANS":
                    {
                        if (args.Length > 1) ExitWriteLine($"The {args[0]} switch does not accept arguments.");
                        var acct = await config.GetActiveAccount();
                        ConsoleDisplay.ListCashTransactions(acct);
                        break;
                    }

                // -buy [symbol] ... buy shares via prompting
                case "-BUY":
                    {
                        if (args.Length != 2) ExitWriteLine($"The {args[0]} switch requires a single [symbol] argument.");
                        var acct = await config.GetActiveAccount();
                        ConsolePrompt.BuyStock(acct, args[1].ToUpper());
                        await config.WriteAccount(acct);
                        break;
                    }

                // -sell [symbol] ... sell shares via prompting
                case "-SELL":
                    {
                        if (args.Length != 2) ExitWriteLine($"The {args[0]} switch requires a single [symbol] argument.");
                        var acct = await config.GetActiveAccount();
                        ConsolePrompt.SellStock(acct, args[1].ToUpper());
                        await config.WriteAccount(acct);
                        break;
                    }

                // -div [symbol] ... add dividend reinvestment shares via prompting
                case "-DIV":
                    {
                        if (args.Length != 2) ExitWriteLine($"The {args[0]} switch requires a single [symbol] argument.");
                        var acct = await config.GetActiveAccount();
                        ConsolePrompt.DividendReinvestment(acct, args[1].ToUpper());
                        await config.WriteAccount(acct);
                        break;
                    }

                // [symbol] ... dump transaction history of the symbol
                default:
                    {
                        if (args[0].StartsWith('-')) ExitWriteLine($"Use -? for help, {args[0]} is not a valid switch.");

                        var acct = await config.GetActiveAccount();
                        ConsoleDisplay.SymbolHistory(acct, args[0].ToUpper());
                        break;
                    }
            }
            Console.WriteLine();
        }

        static void ExitWriteLine(string message, int exitCode = 1)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Environment.Exit(exitCode);
        }
    }
}
