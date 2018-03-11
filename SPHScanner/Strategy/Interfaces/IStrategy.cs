using System;
using System.Collections.Generic;
using Scanner.Entities;

namespace SPHScanner.Strategy
{
    public class IStrategy
    {
        IList<IScanResult> Scan(List<Candle> candles);
    }
}
