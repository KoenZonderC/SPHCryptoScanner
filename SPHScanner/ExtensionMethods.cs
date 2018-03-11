using System;
using ExchangeSharp;

namespace SPHScanner
{
    public static class ExtensionMethods
    { 
        public static bool IsRedCandle(this MarketCandle candle)
        {
            return candle.ClosePrice < candle.OpenPrice;
        } 

        public static decimal BodyPercentage(this MarketCandle candle){
            var candleBody =  Math.Abs(candle.ClosePrice - candle.OpenPrice);
            var openPrice = candle.OpenPrice;
            var percentage = candleBody / openPrice;
            percentage *= 100.0M;
            return percentage;
        }
    }
}
