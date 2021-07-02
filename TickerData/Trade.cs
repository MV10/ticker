using System;

namespace TickerData
{
    /// <summary>
    /// Represents a single trade action for a given stock.
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// Effective date of the trade action.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        /// <summary>
        /// The trade action taken.
        /// </summary>
        public TradeType Action { get; set; } = TradeType.Buy;
        
        /// <summary>
        /// The cash value of the trade action. This is not the gain or loss, it
        /// is typically Price * Shares (excluding special cases like receiving
        /// a gift, which has 0 basis).
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// The effective share price related to this trade.
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// The number shares transferred by this trade.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Free-form memo.
        /// </summary>
        public string Note { get; set; } = string.Empty;
    }
}
