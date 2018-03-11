using System;
using Microsoft.EntityFrameworkCore;

namespace Scanner.Entities
{
    public class PriceDbContext: DbContext
    {
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Candle> Candles { get; set; }

        public PriceHistoryContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Exchange>()
                   .HasKey(e => e.ExchangeId);

            builder.Entity<Candle>()
                   .HasKey(p => new { p.Close, p.ExchangeId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }
    }
}
