using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExchangeSharp;

namespace SPHScanner
{
    class Program
    {
        const int TIMEFRAME_H1 = (60 * 60);

        static void Main(string[] args)
        {
            Console.WriteLine("SPH Scanner 1.0 (c) 2018 Erwin Beckers");
            Console.WriteLine("");

            ScanExchange("bittrex", new ExchangeBittrexAPI()); 

            ScanExchange("binance", new ExchangeBinanceAPI()); 


            Console.WriteLine("--- done ---");
            Console.ReadLine();
        }

        private static void ScanExchange(string exchange, ExchangeAPI api)
        {
            Console.WriteLine($"Loading symbols from {exchange}");
            Task.Run(async () =>
            {
                var strategy = new SPHStrategy();
                var allSymbols = (await api.GetSymbolsAsync()).OrderBy(e => e).ToList();
                var symbols = new List<string>();
                foreach(var symbol in allSymbols)
                {
                    if (symbol.StartsWith("BTC-") || symbol.EndsWith("BTC")) symbols.Add(symbol);
                }
                Console.WriteLine($"got {symbols.Count} btc pairs from {exchange}");
                Console.WriteLine("scanning all btc pairs for SPH's....");

                var idx = 1;
                foreach (var symbol in symbols)
                {
                    Debug.WriteLine($"{idx}/{symbols.Count} scanning {symbol}");
                    idx++;

                    var candles = (await api.GetCandlesAsync(symbol, TIMEFRAME_H1, DateTime.Now.AddMonths(-1))).ToList();
                    var sphs = strategy.Find(symbol, candles);
                    foreach (var sph in sphs)
                    {
                        Console.WriteLine($"SPH: {sph.Date}   {symbol}   {sph.Price}");
                    }
                }
            })
            .Wait();
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
