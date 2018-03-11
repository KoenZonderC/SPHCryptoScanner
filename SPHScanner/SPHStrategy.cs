using System;
using System.Collections.Generic;
using ExchangeSharp;

namespace SPHScanner
{
    public class SPHStrategy
    {
        public SPHStrategy()
        {
        }

        internal List<SPH> Find(string symbol, List<MarketCandle> candles)
        {
            var result = new List<SPH>();

            for (int i = candles.Count - 1; i > 0; i--)
            {

                // Find Panic....
                var     candleIndex = i;
                decimal totalPanic  = 0;
                decimal candleCount = 0;
                while (candleIndex > 0)
                {
                    var candle = candles[candleIndex];
                    if (!candle.IsRedCandle()) break;
                    totalPanic += candle.BodyPercentage();
                    candleIndex--;
                    candleCount++;
                }
                if (candleCount > 0) 
                {
                    var panicPerCandle =totalPanic /  candleCount;
                    if (panicPerCandle >= 5m)
                    {
                        // we found panic.. Now check for stability
                        var startCandleIndex = i - (int)(candleCount) + 1;
                        var endCandleIndex = i;
                        var startPrice = candles[startCandleIndex].OpenPrice;
                        var panicPrice = candles[endCandleIndex].ClosePrice;
                        if (StabilityFound(candles, startCandleIndex, startPrice))
                        {
                            // Stability found
                            // Now check recovery

                            if (PriceRetracesTo(candles, startPrice, endCandleIndex + 1, (int)(candleCount * 2)))
                            {

                                if (!PriceWentBelow(candles, panicPrice, endCandleIndex))
                                {
                                    var sph = new SPH();
                                    sph.Symbol = candles[startCandleIndex].Name;
                                    sph.Price = candles[endCandleIndex].ClosePrice;
                                    sph.Date = candles[endCandleIndex].Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                                    result.Add(sph);
                                }
                            }
                        }

                    }
                }

            }
            return result;
        }

        private bool PriceWentBelow(List<MarketCandle> candles, decimal panicPrice, int endCandleIndex)
        {
            for (int i = candles.Count-1; i > endCandleIndex; i--)
            {
                var candle = candles[i];
                var minPrice = Math.Min(candle.OpenPrice, candle.ClosePrice);
                if (minPrice < panicPrice) return true;
            }
            return false;
        }

        private bool PriceRetracesTo(List<MarketCandle> candles, decimal price, int startIndex, int maxCandles)
        {
            for (int i = startIndex; i <= startIndex + maxCandles; ++i)
            {
                var candle = candles[i];
                if (candle.ClosePrice >= price) return true;
            }
            return false;
        }

        private bool StabilityFound(List<MarketCandle> candles, int startIndex, decimal averagePrice)
        {
            var priceRangeLow  = (averagePrice / 100.0m) * (100m-3.5m);
            var priceRangeHigh = (averagePrice / 100.0m) * (100m+3.5m);

            var stabilityCandles = 0;
            for (int i = startIndex - 1; i > 0; i--)
            {
                var candle = candles[i];
                var candleBodyLow = Math.Min(candle.OpenPrice, candle.ClosePrice);
                var candleBodyHigh = Math.Max(candle.OpenPrice, candle.ClosePrice);
                if (candleBodyLow >= priceRangeLow && candleBodyHigh <= priceRangeHigh)
                {
                    stabilityCandles++;
                }
                else
                {
                    break;
                }
            }

            return stabilityCandles >= 4;
        }
    }
}
