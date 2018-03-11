using System;

namespace Scanner.Entities
{
    /// <summary>
    /// Available exchanges
    /// </summary>
    public enum ExchangeTypes : int
    {
        Bittrex = 1,
        Binance = 2,
        Gdax = 3,
        Okex = 4,
        Gemini = 5,
        Kraken = 6,
        Bithumb = 7,
        Bitfinex = 8,
        Poloniex = 9
    }

    public class Exchange
    {
        /// <summary>
        /// Gets or sets the exchange id.
        /// </summary>
        /// <value>The exchange id.</value>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Gets or sets the exchange type.
        /// </summary>
        /// <value>The name.</value>
        public ExchangeTypes Name  { get; set; }
    }
}
