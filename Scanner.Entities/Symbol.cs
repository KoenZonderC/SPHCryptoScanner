using System;
namespace Scanner.Entities
{
    public class Symbol
    {
        /// <summary>
        /// Gets or sets the symbol id.
        /// </summary>
        /// <value>The symbol id.</value>
        public int SymbolId { get; set; }

        /// <summary>
        /// Gets or sets the exchange id.
        /// </summary>
        /// <value>The exchange id.</value>
        public int ExchangeId { get; set; }
        public virtual Exchange Exchange {get;set;}

        /// <summary>
        /// Gets or sets the symbol name.
        /// </summary>
        /// <value>The symbol name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the datetime when this symbol was last refreshed/udpated.
        /// </summary>
        /// <value>The last update datetime.</value>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the datetime of the latest candle in the database.
        /// </summary>
        /// <value>The datetime of the latest candle.</value>
        public DateTime LastCandle { get; set; }

        public Symbol()
        {
        }
    }
}
