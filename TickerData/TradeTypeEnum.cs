
namespace TickerData
{
    public enum TradeType
    {
        /// <summary>
        /// A purchase transaction of stock shares.
        /// </summary>
        Buy = 0,

        /// <summary>
        /// A sale transaction of stock shares.
        /// </summary>
        Sell = 1,

        /// <summary>
        /// Shares purchased with shareholder dividend payments.
        /// </summary>
        Divdend_Reinvestment = 2,

        /// <summary>
        /// Shares given as a gift transfer for tax purposes.
        /// </summary>
        Send_Gift = 3,

        /// <summary>
        /// Shares received as a gift for tax purposes.
        /// </summary>
        Receive_Gift = 4,

        /// <summary>
        /// A change in the number of shares held as the result of a stock split.
        /// </summary>
        Split = 5,

        /// <summary>
        /// A change in the number of shares held as the result of a reverse stock split.
        /// </summary>
        Reverse_Split = 6
    }
}
