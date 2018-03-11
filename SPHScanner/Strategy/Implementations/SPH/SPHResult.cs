using System;
using SPHScanner.Strategy;

namespace SPHScanner
{
    public class SPHResult : IScanResult
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

        public SPHResult()
        {
        }

        public void Dump()
        {
            Console.WriteLine($"SPH: {Date}   {Symbol,15}   {Price}");
        }
    }
}
