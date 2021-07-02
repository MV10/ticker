using System;
using System.IO;
using System.Threading.Tasks;
using TickerData;

namespace TickerLogic
{
    public static class ConsolePrompt
    {
        public static async Task<Account> AddAccount(Config config)
        {
            Console.WriteLine("Creating a new account:");
            var acct = new Account();

            while (true)
            {
                Console.Write("The Account ID should be a short, easy-to-type filename.\n> ");
                acct.Id = Console.ReadLine();
                var pathname = Path.Join(config.DataPath, $"{acct.Id}.json");
                if (File.Exists(pathname) && ConsoleInput.GetOneKey("That account already exists. Overwrite?", "YN").Equals("N")) continue;
                break;
            }

            Console.Write("You can provide a longer, more descriptive Name for this account.\n> ");
            acct.Name = Console.ReadLine();

            Console.WriteLine("\nAccount created.");
            await config.WriteAccount(acct);

            return acct;
        }

        public static void AddCashTransaction(Account acct)
        {
            Console.WriteLine("Adding a cash transaction:");
            var cash = new Cash();

            cash.Timestamp = ConsoleInput.GetDate("Effective date (mm/dd/yyyy) or Enter for today?");

            cash.Amount = ConsoleInput.GetDecimal("Amount (negative is a withdrawal or buy transaction)?");

            Console.Write("Description of this transaction?\n> ");
            cash.Description = Console.ReadLine().Trim();

            acct.CashTransactions.Add(cash);
            Console.WriteLine("Cash transaction added.");
        }

        public static void BuyStock(Account acct, string symbol)
        {
            if (!acct.Stocks.TryGetValue(symbol, out var stock))
            {
                Console.WriteLine($"Adding stock symbol {symbol}:");
                stock = new Stock { Symbol = symbol };
                Console.Write("What is the full name of the stock?\n> ");
                stock.Name = Console.ReadLine();
                acct.Stocks.Add(symbol, stock);
                Console.WriteLine("Stock added.\n");
            }

            Console.WriteLine($"BUY {stock.Symbol} - {stock.Name}:");
            var trade = GetTrade(TradeType.Buy);

            stock.Trades.Add(trade);
            Console.WriteLine("BUY transaction added.");

            if(ConsoleInput.GetOneKey("Create a cash debit for this amount?", "YN").Equals("Y"))
            {
                var cash = new Cash
                {
                    Timestamp = trade.Timestamp,
                    Amount = trade.Amount * -1,
                    Description = $"Bought {stock.Symbol} - {stock.Name}: {trade.Shares} at {trade.Price:c}"
                };
                acct.CashTransactions.Add(cash);
                Console.WriteLine("Cash transaction added.");
            }
        }

        public static void SellStock(Account acct, string symbol)
        {
            if (!acct.Stocks.TryGetValue(symbol, out var stock))
                throw new Exception($"The account does not have any transaction history for symbol {symbol}");

            Console.WriteLine($"SELL {stock.Symbol} - {stock.Name}:");
            var trade = GetTrade(TradeType.Sell);

            stock.Trades.Add(trade);
            Console.WriteLine("SELL transaction added.");

            if (ConsoleInput.GetOneKey("Create a cash credit for this amount?", "YN").Equals("Y"))
            {
                var cash = new Cash
                {
                    Timestamp = trade.Timestamp,
                    Amount = trade.Amount,
                    Description = $"Sold {stock.Symbol} - {stock.Name}: {trade.Shares} at {trade.Price:c}"
                };
                acct.CashTransactions.Add(cash);
                Console.WriteLine("Cash transaction added.");
            }
        }

        public static void DividendReinvestment(Account acct, string symbol)
        {
            if (!acct.Stocks.TryGetValue(symbol, out var stock))
                throw new Exception($"The account does not have any transaction history for symbol {symbol}");

            Console.WriteLine($"DIVIDEND REINVESTMENT BUY {stock.Symbol} - {stock.Name}:");
            var trade = GetTrade(TradeType.Divdend_Reinvestment);

            stock.Trades.Add(trade);
            Console.WriteLine("DIV-BUY transaction added.");
        }

        private static Trade GetTrade(TradeType action)
        {
            var trade = new Trade 
            { 
                Action = action,
                Timestamp = ConsoleInput.GetDate("Effective date (mm/dd/yyyy) or Enter for today?"),
                Amount = ConsoleInput.GetDecimal("Cash value of the transaction?"),
                Price = ConsoleInput.GetDecimal("Share price?"),
                Shares = ConsoleInput.GetDecimal("Number of shares?")
            };

            Console.Write("Optional note for this transaction?\n> ");
            trade.Note = Console.ReadLine();

            return trade;
        }
    }
}
