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


        /// <summary>
        /// Gets or sets the stability period in hours.
        /// </summary>
        /// <value>The stability in hours.</value>
        public int StabilityInHours { get; set; }

        /// <summary>
        /// Gets or sets the panic percentage.
        /// </summary>
        /// <value>The panic percentage.</value>
        public decimal PanicPercentage { get; set; }

        /// <summary>
        /// Gets or sets the panic duration in hours.
        /// </summary>
        /// <value>The panic hours.</value>
        public int PanicHours { get; set; }


        /// <summary>
        /// Gets or sets the recovery hours.
        /// </summary>
        /// <value>The recovery hours.</value>
        public int RecoveryHours { get; set; }

        public SPHResult()
        {
        }

        public void Dump()
        {
            Console.WriteLine($"SPH: {Date}   {Symbol,15} panic:{PanicPercentage:00.00}% in {PanicHours} hours, stability: {StabilityInHours,2} hours, recovery:{RecoveryHours} hours,  price: {Price}");
        }
    }
}
