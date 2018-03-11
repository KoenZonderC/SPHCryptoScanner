using System;
namespace Scanner.Entities
{
    public class Candle
    {
        /// <summary>
        /// Gets or sets the candle id.
        /// </summary>
        /// <value>The candle id.</value>
        public int CandleId { get; set; }

        /// <summary>
        /// Gets or sets the symbol id.
        /// </summary>
        /// <value>The symbol id.</value>
        public int SymbolId { get; set; }

        /// <summary>
        /// Gets or sets the candle date and time.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the open price.
        /// </summary>
        /// <value>The open price.</value>
        public decimal Open { get; set; }

        /// <summary>
        /// Gets or sets the close price.
        /// </summary>
        /// <value>The close price.</value>
        public decimal Close { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        /// <value>The high price.</value>
        public decimal High { get; set; }

        /// <summary>
        /// Gets or sets the low price
        /// </summary>
        /// <value>The low price.</value>
        public decimal Low { get; set; }


        public virtual Symbol Symbol { get; set; }

        public Candle()
        {
        }
    }
}
