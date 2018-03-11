using System;
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
            Console.WriteLine("Loading symbols from bittrex");

           Task.Run(async () =>
           {
               var strategy = new SPHStrategy();
               var api = new ExchangeBittrexAPI();
                var symbols = (await api.GetSymbolsAsync()).Where(e => e.StartsWith("BTC-")).OrderBy(e=>e).ToList();
               Console.WriteLine($"got {symbols.Count} btc pairs from bittrex");
               Console.WriteLine("scanning all btc pairs for SPH's....");

               var idx = 1;
               foreach (var symbol in symbols)
                {
                   Debug.WriteLine($"{idx}/{symbols.Count} scanning {symbol}");
                   idx++;
                   var candles = await api.GetCandlesAsync(symbol, TIMEFRAME_H1, DateTime.Now.AddMonths(-1));
                   var sphs = strategy.Find(symbol, candles.ToList());
                   foreach(var sph in sphs)
                    {
                        Console.WriteLine($"SPH: {sph.Date}   {symbol}   {sph.Price}");    
                    }
               }
           })
           .Wait();


            Console.WriteLine("--- done ---");
            Console.ReadLine();
        }
    }
}
