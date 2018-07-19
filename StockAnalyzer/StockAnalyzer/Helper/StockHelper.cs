using StockAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalyzer.Helper
{
    internal static class StockHelper
    {
        internal static List<IGrouping<object, StockItem>> TimeFrame(this List<StockItem> source, Func<StockItem, object> grouping)
        {
            return source.Where(w => w.Current > 0).GroupBy(grouping).AsEnumerable().ToList();
        }

        internal static double Low<T>(this IGrouping<T, StockItem> source)
        {
            return source.Min(t => t.Current);
        }

        internal static double Open<T>(this IGrouping<T, StockItem> source)
        {
            return source.OrderBy(o => o.Date).First().Current;
        }

        internal static double Close<T>(this IGrouping<T, StockItem> source)
        {
            return source.OrderBy(o => o.Date).Last().Current;
        }

        internal static double High<T>(this IGrouping<T, StockItem> source)
        {
            return source.Max(m => m.Current);
        }

        internal static string PrintDate(object grouping)
        {
            string format = "dd/MM/yyyy";
            DateTime date = (DateTime)grouping.GetType().GetProperty("Date").GetValue(grouping, null);
            int? hour = (int?)grouping.GetType().GetProperty("Hour")?.GetValue(grouping, null);

            if (hour.HasValue) {
                date = date.AddHours(hour.Value);
                format += " HH:mm";
            }

            return date.ToString(format);
        }

    }
}
