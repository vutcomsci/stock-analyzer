using StockAnalyzer.Helper;
using StockAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StockAnalyzer
{
    public static class Program
    {
        static List<StockItem> _items = new List<StockItem>();
        delegate List<IGrouping<DateTime, StockItem>> TimeFrame(Func<StockItem, DateTime> grouping);

        public static async Task Main(string[] args)
        {

            await FileOpen("Files/a/stock-a.csv");

            Console.WriteLine($"Stock Items Count : {_items.Count}");
            Console.WriteLine($"Start Date : {_items.OrderBy(o => o.Date).First().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Last Date : {_items.OrderBy(o => o.Date).Last().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine("-----");

            Func<StockItem, DateTime> grouping = (g) => g.Date.Date;
            //  x => SqlFunctions.DateDiff("dd", startDate, x.CreateDate))
            var x = _items.GroupBy(g => g.Date.Date);
            foreach (var item in _items.TimeFrame(grouping).Select(s=>new {s.Key,StockItems=s }).ToList())
            {
                Console.WriteLine();
                //var currents = item.StockItems.Low();
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

    }
}
