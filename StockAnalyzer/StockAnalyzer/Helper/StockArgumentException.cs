using System;
using System.Collections.Generic;
using System.Text;

namespace StockAnalyzer.Helper
{
    internal class StockArgumentException : Exception
    {
        public override string Message { get; }
        public StockArgumentException(string message)
        {
            Message = message;
        }
    }
}
