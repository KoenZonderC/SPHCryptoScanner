using System;
using System.Collections.Generic;
using Scanner.Entities;
using SPHScanner.Strategy;

namespace SPHScanner
{
    public class SPHStrategy : IStrategy
    {
        public SPHStrategy()
        {
        }

        /// <summary>
        /// Search for SPH's in the candle list
        /// </summary>
        /// <returns>List of SPH's found</returns>
        /// <param name="symbol">Symbol.</param>
        /// <param name="candles">Candles list</param>
        public IList<IScanResult> Scan(string symbol, List<Candle> candles)
        {
            var result = new List<IScanResult>();

            for (var i = candles.Count - 1; i > 0; i--)
            {
                // Find panic....
                var candleIndex = i;
                var totalPanic = 0M;
                var candleCount = 0M;
                while (candleIndex > 0)
                {
                    var candle = candles[candleIndex];
                    if (!candle.IsRedCandle()) break;
                    var panic = candle.BodyPercentage();
                    if (panic < 5) break;
                    totalPanic += candle.BodyPercentage();
                    candleIndex--;
                    candleCount++;
                }

                if (candleCount > 0)
                {
                    var panicPerCandle = totalPanic / candleCount;
                    if (panicPerCandle < 5m && candleCount > 1)
                    {
                        // perhaps the start candle is part of the stability phase and not the panic phase.
                        candleCount--;
                        candleIndex++;
                        var candle = candles[candleIndex];
                        var candlePercentage = candle.BodyPercentage();
                        if (candlePercentage < 5m)
                        {
                            totalPanic = totalPanic - candlePercentage;
                            panicPerCandle = totalPanic / candleCount;
                        }
                    }
                    if (panicPerCandle >= 5m)
                    {
                        // we found panic.. 
                        var startCandleIndex = i - (int)(candleCount) + 1;
                        var endCandleIndex = i;

                        var startPrice = candles[startCandleIndex].Open;
                        var panicPrice = candles[endCandleIndex].Close;

                        // Now check for stability before the panic appeared
                        var hours = StabilityInHours(candles, startCandleIndex, startPrice);
                        if (hours >= 2)
                        {
                            // Stability found
                            // Now check if price retraces back to opening price quickly
                            int recoveryInHours = GetRecoveryInHours(candles, startPrice, endCandleIndex + 1);
                            if ( recoveryInHours <  (int)(candleCount * 4))
                            {
                                // found fast retracement, check if SPH is still valid
                                if (!PriceWentBelow(candles, panicPrice, endCandleIndex))
                                {
                                    // SPH is still valid, add it to the result list.
                                    var sph = new SPHResult();
                                    sph.Symbol = symbol;
                                    sph.StabilityInHours = hours;
                                    sph.Price = candles[endCandleIndex].Close;
                                    sph.PanicPercentage = totalPanic;
                                    sph.PanicHours = (int)candleCount;
                                    sph.RecoveryHours = recoveryInHours;
                                    sph.Date = candles[endCandleIndex].Date.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                                    result.Add(sph);
                                }
                            }
                        }

                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Returns if price went below the panic price
        /// </summary>
        /// <returns><c>true</c>, if price went below panic price, <c>false</c> otherwise.</returns>
        /// <param name="candles">list of candles</param>
        /// <param name="panicPrice">Panic price.</param>
        /// <param name="candleIndex">candle to look from.</param>
        private bool PriceWentBelow(List<Candle> candles, decimal panicPrice, int candleIndex)
        {
            for (int i = candles.Count - 1; i > candleIndex+1; i--)
            {
                var candle = candles[i];
                var minPrice = Math.Min(candle.Open, candle.Close);
                if (minPrice < panicPrice) return true;
            }
            return false;
        }

        /// <summary>
        /// returns how long the recovery period takes in hours
        /// </summary>
        /// <returns>The recovery in hours.</returns>
        /// <param name="candles">Candles.</param>
        /// <param name="price">Price.</param>
        /// <param name="startIndex">Start index.</param>
        private int GetRecoveryInHours(List<Candle> candles, decimal price, int startIndex)
        {
            int hrs = 1;
            for (int i = startIndex; i < candles.Count; ++i)
            {
                var candle = candles[i];
                if (candle.Close >= price) return hrs; 
                hrs++;   
            }
            return 10000;
        }


        /// <summary>
        /// Checks if there is a region of stability around the average price in hours
        /// </summary>
        /// <returns><c>true</c>, stability period in hours, <c>false</c> otherwise.</returns>
        /// <param name="candles">Candles list</param>
        /// <param name="startIndex">Start candle</param>
        /// <param name="averagePrice">Average price.</param>
        private int StabilityInHours(List<Candle> candles, int startIndex, decimal averagePrice)
        {
            // allow price to fluctuate +- 3.5% around the average price
            var priceRangeLow = (averagePrice / 100.0m) * (100m - 3.5m);
            var priceRangeHigh = (averagePrice / 100.0m) * (100m + 3.5m);

            var stabilityCandles = 0;
            for (int i = startIndex - 1; i > 0; i--)
            {
                var candle = candles[i];
                var candleBodyLow = Math.Min(candle.Open, candle.Close);
                var candleBodyHigh = Math.Max(candle.Open, candle.Close);
                if (candleBodyLow >= priceRangeLow && candleBodyHigh <= priceRangeHigh)
                {
                    stabilityCandles++;
                }
                else
                {
                    break;
                }
            }

            return stabilityCandles ;
        }
    }
}
