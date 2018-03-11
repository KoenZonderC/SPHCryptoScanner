using System;
namespace SPHScanner
{
    public class SPH
    {
        /// <summary>
        /// Gets or sets the symbol name
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal Price { get; set; }

        public SPH()
        {
        }
    }
}
