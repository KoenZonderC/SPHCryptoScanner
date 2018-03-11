using System;
using System.Collections.Generic;
using Scanner.Entities;

namespace SPHScanner.Strategy
{
    public interface IStrategy
    {
        IList<IScanResult> Scan(string symbol, List<Candle> candles);
    }
}
