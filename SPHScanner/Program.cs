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
            Console.WriteLine("SPH Scanner 1.03 (c) 2018 Erwin Beckers");
            Console.WriteLine("");

            // create database if it doesnt exists yet
            var db = new PriceDbContext();
            db.Database.EnsureCreated();

            // create strategy
            var strategy = new SPHStrategy();

            // scan SPH's on bittrex
            var scanner = new Scanner(ExchangeTypes.Bittrex);
            scanner.Scan(strategy);

            // scan SPH's on binance
            scanner = new Scanner(ExchangeTypes.Binance);
            scanner.Scan(strategy);


            Console.WriteLine("--- done ---");
            Console.ReadLine();
        }
    }
}
