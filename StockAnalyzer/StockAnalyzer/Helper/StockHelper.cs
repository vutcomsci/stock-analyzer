using StockAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalyzer.Helper
{
    internal static class StockHelper
    {
        internal static List<IGrouping<T, StockItem>> Test<T>(List<StockItem> source,Func<StockItem, T> grouping)
        {
            return source.GroupBy(grouping).AsEnumerable().ToList();
        }

        /*Extension
        internal static List<IGrouping<T,StockItem>> TimeFrame<T>(this List<StockItem> source, Func<StockItem, T> grouping)
        {
            return source.GroupBy(grouping).AsEnumerable().ToList();
        }*/

        internal static double Low<T>(this IGrouping<T, StockItem> source) {
            return source.Min(t=>t.Current);
        }

        internal static double Open<T>(this IGrouping<T, StockItem> source)
        {
            return source.OrderBy(o => o.Current).First().Current;
        }

        internal static double Close<T>(this IGrouping<T, StockItem> source)
        {
            return source.OrderBy(o => o.Current).Last().Current;
        }

        internal static double High<T>(this IGrouping<T, StockItem> source)
        {
            return source.Max(m => m.Current);
        }
    }
}
