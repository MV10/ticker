using System;
using System.Linq;
using TickerData;

namespace TickerLogic
{
    public static class ConsoleDisplay
    {
        public static void HoldingsSummary(Account acct)
        {
            if(acct.Stocks.Count == 0)
            {
                Console.WriteLine("The account does not have any holdings.");
            }

            // TODO show averages like cost and age

            foreach(var s in acct.Stocks.OrderBy(s => s.Key).ToDictionary(k => k.Key, k => k.Value).Values.ToList())
            {
                decimal shares = 0;
                foreach(var t in s.Trades)
                {
                    shares += t.Action switch
                    {
                        TradeType.Buy => t.Shares,
                        TradeType.Sell => t.Shares * -1,
                        TradeType.Divdend_Reinvestment => t.Shares,
                        _ => 0
                    };
                }

                if(shares != 0)
                    Console.WriteLine($"{shares,11:0.000000} {s.Symbol,5} - {s.Name}");
            }
        }

        public static void ListAccounts(Config config)
        {
            Console.WriteLine("TODO: ListAccounts");
        }

        public static void ListCashTransactions(Account acct)
        {
            if(acct.CashTransactions.Count == 0)
            {
                Console.WriteLine("The account does not have any cash transactions.");
                return;
            }

            foreach(var t in acct.CashTransactions.OrderBy(t => t.Timestamp).ToList())
            {
                Console.WriteLine($"{t.Timestamp:yyyy-MM-dd}  {t.Amount,12:c}  {t.Description}");
            }
        }

        public static void SymbolHistory(Account acct, string symbol)
        {
            if (!acct.Stocks.TryGetValue(symbol, out var stock))
            {
                Console.WriteLine($"The account has never held symbol {symbol}");
                return;
            }

            Console.WriteLine($"{stock.Symbol} - {stock.Name}");
            Dashes();

            // TODO improve formatting
            // TODO add share lot aging

            foreach(var t in stock.Trades.OrderBy(t => t.Timestamp).ToList())
            {
                var action = t.Action switch
                {
                    TradeType.Buy => "BUY",
                    TradeType.Sell => "SEL",
                    TradeType.Divdend_Reinvestment => "DIV",
                    _ => "TODO"
                };
                Console.WriteLine($"{t.Timestamp:yyyy-MM-dd}  {action} {t.Shares,11:0.000000} @ {t.Price,10:c} = {t.Amount,10:c}");
            }

            Dashes();
        }

        private static void Dashes()
            => Console.WriteLine($"\n{new string('-', 79)}\n");
    }
}
