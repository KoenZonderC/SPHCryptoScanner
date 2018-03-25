using Scanner.Entities;
using SPHScanner.Strategy;
using System;
using System.Linq;

namespace SPHScanner
{
    public class Scanner
    {
        private ExchangeTypes _exchangeType;
        private PriceDbContext _dbContext;

        public Scanner(PriceDbContext dbContext, ExchangeTypes exchangeType)
        {
            _exchangeType = exchangeType;
            _dbContext = dbContext;
        }

        public void Scan(IStrategy strategy)
        {
            // get symbols for exchange
            // we could get them from the database as well
            // but database could be empty and/or new symbols can be added/removed at any time
            // so we get the current list of symbols from the exchange
            var api = ExchangeFactory.Create(_exchangeType);
            var symbols = api.GetSymbols().OrderBy(e => e).ToList();

            // next for each symbol
            Console.WriteLine($"Scanning {symbols.Count} symbols on {_exchangeType}");
            foreach (var symbol in symbols)
            {
                // get the candles
                try
                {
                    var factory = new CandleFactory(_exchangeType, api, _dbContext);
                    var candles = factory.Get(symbol);

                    // scan for SPHs
                    var results = strategy.Scan(symbol, candles);

                    // show SPHs found
                    foreach (var result in results)
                    {
                        result.Dump();
                    }
                }
                catch (Exception)
                {
                }
            }
            Console.WriteLine("");
        }
    }
}