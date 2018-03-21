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

            //SPH: 2018-03-18 17:00:00          BTC-CLAM panic:05.10% in 1 hours, stability:  2 hours, recovery:1 hours,  price: 0.00039249
            //SPH: 2018-03-18 16:00:00          BTC-DOGE panic:05.13% in 1 hours, stability:  3 hours, recovery:2 hours,  price: 0.00000037
            //SPH: 2018-03-15 06:00:00          BTC-PDC panic:07.74% in 1 hours, stability:  2 hours, recovery:2 hours,  price: 0.00000286
            //SPH: 2018-02-06 08:00:00          NULSBTC panic:09.36 % in 1 hours, stability: 2 hours, recovery: 2 hours,  price: 0.00022659
            Console.WriteLine("SPH Scanner 1.04 (c) 2018 Erwin Beckers");
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
