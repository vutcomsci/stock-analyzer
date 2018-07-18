using StockAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalyzer.Helper
{
    internal static class StockHelper
    {
        internal static List<IGrouping<DateTime,StockItem>> TimeFrame(this List<StockItem> source, Func<StockItem, DateTime> grouping)
        {
            return source.GroupBy(grouping).AsEnumerable().ToList();
        }

        internal static double Low(this IGrouping<DateTime, StockItem> source) {
            return source.Min(t=>t.Current);
        }

        internal static double Open(this IGrouping<DateTime, StockItem> source)
        {
            return source.OrderBy(o => o.Current).First().Current;
        }

        internal static double Close(this IGrouping<DateTime, StockItem> source)
        {
            return source.OrderBy(o => o.Current).Last().Current;
        }

        internal static double High(this IGrouping<DateTime, StockItem> source)
        {
            return source.Max(m => m.Current);
        }
    }
}
