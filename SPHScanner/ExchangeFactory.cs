using System;
using ExchangeSharp;
using Scanner.Entities;

namespace SPHScanner
{
    public static class ExchangeFactory
    {
        /// <summary>
        /// Returns the correct exchange api for the specified exchange.
        /// </summary>
        /// <returns>api for communicating with the exchange.</returns>
        /// <param name="exchangeType">The exchange.</param>
        public static ExchangeAPI Create(ExchangeTypes exchangeType)
        {
            switch (exchangeType)
            {
                case ExchangeTypes.Binance:
                    return new ExchangeBinanceAPI();

                case ExchangeTypes.Bittrex:
                    return new ExchangeBittrexAPI();

                case ExchangeTypes.Gdax:
                    return new ExchangeGdaxAPI();

                case ExchangeTypes.Okex:
                    return new ExchangeOkexAPI();

                case ExchangeTypes.Gemini:
                    return new ExchangeGeminiAPI();

                case ExchangeTypes.Kraken:
                    return new ExchangeKrakenAPI();

                case ExchangeTypes.Bithumb:
                    return new ExchangeBithumbAPI();

                case ExchangeTypes.Bitfinex:
                    return new ExchangeBitfinexAPI();

                case ExchangeTypes.Poloniex:
                    return new ExchangePoloniexAPI();
            }
            return null;
        }
    }
}
