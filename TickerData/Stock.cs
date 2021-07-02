using System.Collections.Generic;

namespace TickerData
{
    /// <summary>
    /// Represents a stock position's history of trade activity.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// The symbol representing the stock.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The name of the stock.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ordered list of trading activity for this stock.
        /// </summary>
        public List<Trade> Trades { get; set; } = new List<Trade>();
    }
}
