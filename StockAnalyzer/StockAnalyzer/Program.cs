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
        static string _stock;
        static string _stockName;
        static string _labelTimeframe = TimeFrame.Date.ToString();
        static Func<StockItem, object> _grouping;

        public static async Task Main(string[] args)
        {
            InitArgs(args);
            await FileOpen(_stock);

            Console.WriteLine($"Stock Name : {_stockName}");
            Console.WriteLine($"Stock Items Count : {_items.Count:n0}");
            Console.WriteLine($"Start Date : {_items.OrderBy(o => o.Date).First().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Last Date : {_items.OrderBy(o => o.Date).Last().Date.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Time frame : {_labelTimeframe}");
            Console.WriteLine("-----");

            foreach (var item in _items.TimeFrame(_grouping).Select(s => new { s.Key, StockItems = s }).ToList())
            {
                Console.WriteLine();
                Console.Write($"Date : {StockHelper.PrintDate(item.Key)} -->  ");
                Console.Write($" Open : {item.StockItems.Open():F2}");
                Console.Write($" Close : {item.StockItems.Close():F2}");
                Console.Write($" High : {item.StockItems.High():F2}");
                Console.Write($" Low : {item.StockItems.Low():F2}");
            }
            Console.WriteLine();
        }

        #region File
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
        #endregion

        #region Args
        private static void InitArgs(string[] args)
        {
            try
            {
                int arrIndex;
                if (args.Length == 0)
                {
                    throw new StockArgumentException("Not found argument.");
                }
                else
                {
                    if (string.IsNullOrEmpty(args.Where(w => w.Equals("-s")).FirstOrDefault()))
                    {
                        throw new StockArgumentException("Missing stock argument  -s");
                    }
                    else
                    {
                        arrIndex = Array.IndexOf(args, "-s");
                        ++arrIndex;
                        if (arrIndex > args.Length || args[arrIndex].Contains("-"))
                        {
                            throw new StockArgumentException("Not found stock  -s  \"a\"|\"b\"  ");
                        }
                        else
                        {
                            _stockName = args[arrIndex];
                            _stock = $"Files/stock-{args[arrIndex]}.csv";
                        }
                    }


                    if (string.IsNullOrEmpty(args.Where(w => w.Contains("-t")).FirstOrDefault()))
                    {
                        throw new StockArgumentException("Missing timeframe argument  -t ");
                    }
                    else
                    {
                        arrIndex = Array.IndexOf(args, "-t");
                        ++arrIndex;

                        if (arrIndex > args.Length || args[arrIndex].Contains("-"))
                        {
                            throw new StockArgumentException("Wrong timeframe  -t  \"d\"|\"H\"  ");
                        }
                        else
                        {
                            if (args[arrIndex] == "d") //date
                            {
                                _labelTimeframe = TimeFrame.Date.ToString();
                                _grouping = (g) => new { g.Date.Date };
                            }
                            else if (args[arrIndex] == "H")//hour
                            {
                                _labelTimeframe = TimeFrame.Hour.ToString();
                                _grouping = (g) => new { g.Date.Date, g.Date.Hour };
                            }
                            else
                            {
                                throw new StockArgumentException("Wrong timeframe  -t  \"d\"|\"H\"  ");
                            }
                        }
                    }
                }
            }
            catch (StockArgumentException ex)
            {
                Console.WriteLine($"Error! {ex.Message}");
                Environment.Exit(0);
            }
        }
        #endregion
    }
}
