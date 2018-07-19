using StockAnalyzer.Helper;
using StockAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockAnalyzer
{
    public static class Program
    {
        static List<StockItem> _items = new List<StockItem>();
        delegate List<IGrouping<object, StockItem>> TimeFrameDate(List<StockItem> source, Func<StockItem, object> grouping);

        public static async Task Main(string[] args)
        {
            if(args.Length == 0)
                
            await FileOpen("Files/a/stock-a.csv");

            Console.WriteLine($"Stock Items Count : {_items.Count}");
            Console.WriteLine($"Start Date : {_items.OrderBy(o => o.Date).First().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Last Date : {_items.OrderBy(o => o.Date).Last().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Time frame : Date");
            Console.WriteLine("-----");
           
            TimeFrameDate zzzzz = new TimeFrameDate(StockHelper.Test);
            foreach (var item in zzzzz(_items, ParamTimeFrame()).Select(s => new { s.Key, StockItems = s }).ToList())
            {
                Console.WriteLine();
                Console.Write($"Date Item : {item.Key}");
                Console.Write($" O : {item.StockItems.Open():F2}");
                Console.Write($" H : {item.StockItems.High():F2}");
                Console.Write($" C : {item.StockItems.Close():F2}");
                Console.Write($" L : {item.StockItems.Low():F2}");
            }
            Console.WriteLine();
        }

        private static async Task FileOpen(string path)
        {
            using (var reader = new StreamReader(path))
            {
                string s = "";
                int rowCounting = 0;
                while ((s = await reader.ReadLineAsync()) != null)
                {
                    ++rowCounting;
                    if (rowCounting != 1)
                    {
                        string[] fileds = s.Split(',');
                        StockItem item = new StockItem();
                        item.Date = DateTime.Parse(fileds[0]);
                        item.Current = Convert.ToDouble(fileds[1]);
                        item.Delta = Convert.ToDouble(fileds[2]);
                        item.Bids = Convert.ToDouble(fileds[3]);
                        item.Offers = Convert.ToDouble(fileds[4]);
                        _items.Add(item);
                    }
                }
            }
        }

        private static Func<StockItem, object> ParamTimeFrame()
        {
            Func<StockItem, object> grouping;
            int x = 1;
            switch (x)
            {
                case 1: return grouping = (g) => g.Date.Date; 
                case 2: return grouping = (g) => new { g.Date.Date, g.Date.Hour };
            }
            return grouping = (g) => g.Date.Date; 
        }
    }
}
