using System;
using Microsoft.EntityFrameworkCore;

namespace Scanner.Entities
{
    public class PriceDbContext: DbContext
    {
        /// <summary>
        /// table recording all exchanges
        /// </summary>
        public DbSet<Exchange> Exchanges { get; set; }

        /// <summary>
        /// table recording all symbols
        /// </summary>
        public DbSet<Symbol> Symbols { get; set; }

        /// <summary>
        /// table recording all candles
        /// </summary>
        public DbSet<Candle> Candles { get; set; }

        /// <summary>
        /// table recording all sphs
        /// </summary>
        public DbSet<SPH> SPHs { get; set; }

        public PriceDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=datasticks.db");
           // optionsBuilder.UseSqlServer("Server=.\\sql2014;Database=Sph;Trusted_Connection=True;");
        }
    }
}
