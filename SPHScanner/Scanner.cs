using System;
using System.Linq;
using Scanner.Entities;
using SPHScanner.Strategy;

namespace SPHScanner
{
    public class Scanner
    {
        private ExchangeTypes _exchangeType;
        public Scanner(ExchangeTypes exchangeType)
        {
            _exchangeType = exchangeType;
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
                var factory = new CandleFactory();
                var candles = factory.Get(_exchangeType, symbol);

                // scan for SPHs
                var results = strategy.Scan(symbol, candles);

                // show SPHs found
                foreach (var result in results)
                {
                    result.Dump();
                }
            }
            Console.WriteLine("");
        }
    }
}
