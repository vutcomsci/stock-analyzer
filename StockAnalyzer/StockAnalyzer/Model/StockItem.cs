using System;
using System.Collections.Generic;
using System.Text;

namespace StockAnalyzer.Model
{
    internal class StockItem
    {
        internal DateTime Date { get; set; }
        internal double Current { get; set; }
        internal double Delta { get; set; }
        internal double Bids { get; set; }
        internal double Offers { get; set; }
    }
}
