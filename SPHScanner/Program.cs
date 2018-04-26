using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExchangeSharp;
using Scanner.Entities;

namespace SPHScanner
{
    class Program
    {
        const int TIMEFRAME_H1 = (60 * 60);

        static void Main(string[] args)
        {
            Console.WriteLine("SPH Scanner 1.05 (c) 2018 Erwin Beckers");
            Console.WriteLine("");

            // create database if it doesnt exists yet
            using (var db = new PriceDbContext())
            {
                db.Database.EnsureCreated();

                // create strategy
                var strategy = new SPHStrategy();

                // scan SPH's on bitfinex
                var scanner = new Scanner(db, ExchangeTypes.Bitfinex);
                scanner.Scan(strategy);

                // scan SPH's on kraken
                scanner = new Scanner(db, ExchangeTypes.Kraken);
                scanner.Scan(strategy);

                // scan SPH's on bittrex
                scanner = new Scanner(db, ExchangeTypes.Bittrex);
                scanner.Scan(strategy);

                // scan SPH's on binance
                scanner = new Scanner(db, ExchangeTypes.Binance);
                scanner.Scan(strategy);
            }

            Console.WriteLine("--- done ---");
            Console.ReadLine();
        }
    }
}
