using System.Collections.Generic;

namespace TickerData
{
    /// <summary>
    /// Represents a trading account holding stocks, trade data, and cash transactions.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Short identifier assigned to the account (for convenience). Must be a valid
        /// filename when combined with a ".tikr" extension.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Arbitrary name assigned to the account.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Account holdings keyed on Stock.Symbol.
        /// </summary>
        public Dictionary<string, Stock> Stocks { get; set; } = new Dictionary<string, Stock>();
        
        /// <summary>
        /// Ordered list of Cash withdrawals and deposits.
        /// </summary>
        public List<Cash> CashTransactions { get; set; } = new List<Cash>();
    }
}
