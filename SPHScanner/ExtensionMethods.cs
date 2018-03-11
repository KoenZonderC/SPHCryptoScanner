using System;
using Scanner.Entities;

namespace SPHScanner
{
    public static class ExtensionMethods
    {
        public static bool IsRedCandle(this Candle candle)
        {
            return candle.Close < candle.Open;
        }

        public static decimal BodyPercentage(this Candle candle)
        {
            var candleBody = Math.Abs(candle.Close - candle.Open);
            var openPrice = candle.Open;
            var percentage = candleBody / openPrice;
            percentage *= 100.0M;
            return percentage;
        }
    }
}
