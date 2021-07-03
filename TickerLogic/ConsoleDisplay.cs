using System;
using System.IO;
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

            Console.WriteLine("Shares       Symbol  Name                                      AvgAge");
            Dashes(79);

            foreach(var s in acct.Stocks.OrderBy(s => s.Key).ToDictionary(k => k.Key, k => k.Value).Values.ToList())
            {
                decimal shares = 0;
                int avgAge = 0;
                int buys = 0;

                foreach(var t in s.Trades)
                {
                    shares += t.Action switch
                    {
                        TradeType.Buy => t.Shares,
                        TradeType.Sell => t.Shares * -1,
                        TradeType.Divdend_Reinvestment => t.Shares,
                        _ => 0
                    };

                    if(t.Action == TradeType.Buy || t.Action == TradeType.Divdend_Reinvestment)
                    {
                        avgAge += (DateTime.Now - t.Timestamp).Days;
                        buys++;
                    }
                }

                avgAge = avgAge / buys;

                var name = (s.Name.Length > 40) ? s.Name.Substring(0, 40) : s.Name.PadRight(40);

                if(shares != 0)
                    Console.WriteLine($"{shares,11:0.000000}  {s.Symbol,-5} - {name}  {avgAge,4}");
            }
        }

        public static void ListAccounts(Config config)
        {
            Console.WriteLine($"All ticker account files stored at the configured location:");
            Dashes(79);
            var list = Directory.GetFiles(Path.Join(config.DataPath), "*.tikr");
            foreach (var file in list) Console.WriteLine(file);
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

            Console.WriteLine($"{stock.Symbol} - {stock.Name}\n");

            Console.WriteLine("Date        Trd Shares        Cost Basis   Total Amt   Age ");
            Dashes(79);

            foreach(var t in stock.Trades.OrderBy(t => t.Timestamp).ToList())
            {
                var action = t.Action switch
                {
                    TradeType.Buy => "BUY",
                    TradeType.Sell => "SEL",
                    TradeType.Divdend_Reinvestment => "DIV",
                    _ => "TODO"
                };

                var days = (DateTime.Now - t.Timestamp).Days;

                Console.WriteLine($"{t.Timestamp:yyyy-MM-dd}  {action} {t.Shares,11:0.000000} @ {t.Price,10:c} = {t.Amount,10:c}  {days,4}");
            }
        }

        private static void Dashes(int count)
            => Console.WriteLine(new string('-', count));
    }
}
