using System;

namespace TickerData
{
    public class Cash
    {
        /// <summary>
        /// Effective date of the transaction.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Cash balance increase or decrease (both cash transfers and holdings activities).
        /// </summary>
        public decimal Amount { get; set; } = 0;

        /// <summary>
        /// Free-form memo.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
