using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExchangeSharp;
using Scanner.Entities;

namespace SPHScanner
{
    public class CandleFactory
    {
        const int TIMEFRAME_H1 = (60 * 60);

        /// <summary>
        /// Returns the candles for a specific exchange & symbol.
        /// When present candles are returned from the database
        /// and if needed database is updated with new candles from the exchange
        /// </summary>
        /// <returns>Candles for the exchange / symbol</returns>
        /// <param name="exchangeType">Exchange.</param>
        /// <param name="symbolName">Symbol.</param>
        public List<Candle> Get(ExchangeTypes exchangeType, string symbolName)
        {
            // open database
            using (var db = new PriceDbContext())
            {
                // get exchange from database
                var exchange = db.Exchanges.FirstOrDefault(e => e.Name == exchangeType);
                if (exchange == null)
                {
                    Debug.WriteLine($"Add {exchangeType}");
                    // new exchange.. add record in database
                    exchange = new Exchange()
                    {
                        Name = exchangeType
                    };
                    db.Exchanges.Add(exchange);
                    db.SaveChanges();
                }

                // get symbol from database
                var symbol = db.Symbols.FirstOrDefault(e => e.Name == symbolName && e.ExchangeId == exchange.ExchangeId);
                if (symbol == null)
                {
                    Debug.WriteLine($"Add {symbolName}  on {exchangeType}");
                    // new symbol.. add record in database
                    symbol = new Symbol()
                    {
                        Name = symbolName,
                        ExchangeId = exchange.ExchangeId,
                        LastUpdate = new DateTime(2018, 1, 1),
                        LastCandle = new DateTime(2018, 1, 1)
                    };
                    db.Symbols.Add(symbol);
                    db.SaveChanges();
                }

                // Check if we need to get new candles from the exchange
                var now = DateTime.Now;
                if (now.Day != symbol.LastUpdate.Day || now.Month != symbol.LastUpdate.Month || now.Year != symbol.LastUpdate.Year)
                {
                    // yes... then get all (new) candles from the exchange
                    Debug.WriteLine($"update {symbol.Name}  Last Update:{symbol.LastUpdate} ");
                    
                    var api = ExchangeFactory.Create(exchangeType);
                    var newCandles = api.GetCandles(symbol.Name, TIMEFRAME_H1, symbol.LastCandle).ToList();

                    // add new candles to the database for next time
                    foreach (var candle in newCandles)
                    {
                        // only add new candles
                        if (candle.Timestamp > symbol.LastCandle)
                        {
                            symbol.LastCandle = candle.Timestamp;

                            db.Candles.Add(new Candle()
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

                    // set lastupdate for this symbol
                    symbol.LastUpdate = now;
                    Debug.WriteLine($"  set lastupdate {symbol.Name}  to :{symbol.LastUpdate} ");
                    db.SaveChanges();
                }


                // return candles for this symbol;
                var startDate = DateTime.Now.AddMonths(-2);
                var candles = db.Candles
                                .Where(e => e.SymbolId == symbol.SymbolId && e.Date >= startDate)
                                .OrderBy(e => e.Date)
                                .ToList();
                return candles;
            }
        }
    }
}
