using ExchangeSharp;
using Scanner.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SPHScanner
{
    public class CandleFactory
    {
        private const int TIMEFRAME_H1 = (60 * 60);
        private ExchangeAPI _api;
        private PriceDbContext _dbContext;
        private ExchangeTypes _exchangeType;

        public CandleFactory(ExchangeTypes exchangeType, ExchangeAPI api, PriceDbContext dbContext)
        {
            _api = api;
            _dbContext = dbContext;
            _exchangeType = exchangeType;
        }

        /// <summary>
        /// Returns the candles for a specific exchange & symbol.
        /// When present candles are returned from the database
        /// and if needed database is updated with new candles from the exchange
        /// </summary>
        /// <returns>Candles for the exchange / symbol</returns>
        /// <param name="exchangeType">Exchange.</param>
        /// <param name="symbolName">Symbol.</param>
        public List<Candle> Get(string symbolName)
        {
            // get exchange from database
            var exchange = _dbContext.Exchanges.FirstOrDefault(e => e.Name == _exchangeType);
            if (exchange == null)
            {
                Debug.WriteLine($"Add {_exchangeType}");
                // new exchange.. add record in database
                exchange = new Exchange()
                {
                    Name = _exchangeType
                };
                _dbContext.Exchanges.Add(exchange);
                _dbContext.SaveChanges();
            }

            // get symbol from database
            var symbol = _dbContext.Symbols.FirstOrDefault(e => e.Name == symbolName && e.ExchangeId == exchange.ExchangeId);
            if (symbol == null)
            {
                Debug.WriteLine($"Add {symbolName}  on {_exchangeType}");
                // new symbol.. add record in database
                symbol = new Symbol()
                {
                    Name = symbolName,
                    ExchangeId = exchange.ExchangeId,
                    LastUpdate = new DateTime(2017, 1, 1),
                    LastCandle = new DateTime(2017, 1, 1)
                };
                _dbContext.Symbols.Add(symbol);
                _dbContext.SaveChanges();
            }

            // Check if we need to get new candles from the exchange
            var now = DateTime.Now;
            if (now.Day != symbol.LastUpdate.Day || now.Month != symbol.LastUpdate.Month || now.Year != symbol.LastUpdate.Year)
            {
                // yes... then get all (new) candles from the exchange
                Debug.WriteLine($"update {symbol.Name}  Last Update:{symbol.LastUpdate} ");

                for (int i = 0; i < 2; ++i)
                {
                    var newCandles = _api.GetCandles(symbol.Name, TIMEFRAME_H1, symbol.LastCandle).ToList();
                    // add new candles to the database for next time
                    foreach (var candle in newCandles)
                    {
                        // only add new candles
                        if (candle.Timestamp > symbol.LastCandle)
                        {
                            symbol.LastCandle = candle.Timestamp;

                            _dbContext.Candles.Add(new Candle()
                            {
                                SymbolId = symbol.SymbolId,
                                Date = candle.Timestamp,
                                Open = candle.OpenPrice,
                                Close = candle.ClosePrice,
                                Low = candle.LowPrice,
                                High = candle.HighPrice
                            });
                        }
                    }
                    var ts = DateTime.Now - symbol.LastCandle;
                    if (ts.TotalDays <= 1) break;
                }

                // set lastupdate for this symbol
                symbol.LastUpdate = now;

                Debug.WriteLine($"  set lastupdate {symbol.Name}  to :{symbol.LastUpdate} ");
                _dbContext.SaveChanges();
            }

            // return candles for this symbol;
            var startDate = DateTime.Now.AddMonths(-1);
            var candles = _dbContext.Candles
                            .Where(e => e.SymbolId == symbol.SymbolId && e.Date >= startDate)
                            .OrderBy(e => e.Date)
                            .ToList();
            return candles;
        }
    }
}