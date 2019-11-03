using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace demo.WebSockets
{

    public class PriceItem
    {
        public string Currency { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
    public class PriceEngine
    {
        private static PriceEngine _instance;
        private Random random = new Random();
        public static PriceEngine Instance()
        {
            if (_instance == null)
            {
                _instance = new PriceEngine();
                _instance.initPrices();
            }
            return _instance;
        }

        Dictionary<string,PriceItem> Items = new Dictionary<string, PriceItem>();
        private  PriceEngine()
        {
            Items = new Dictionary<string, PriceItem>();
            Items.Add("EUR", new PriceItem() { Currency="EUR",Bid= 7.442092M, Ask= 7.486878M });
        }

        public void initPrices()
        {
            var prices = @"211311020190111201914
036AUD001       4,600415       4,614258       4,628101
124CAD001       5,067129       5,082376       5,097623
203CZK001       0,291721       0,292599       0,293477
208DKK001       0,996237       0,999235       1,002233
348HUF100       2,258738       2,265535       2,272332
392JPY100       6,161182       6,179721       6,198260
578NOK001       0,724905       0,727086       0,729267
752SEK001       0,691117       0,693197       0,695277
756CHF001       6,753872       6,774195       6,794518
826GBP001       8,637525       8,663516       8,689507
840USD001       6,672727       6,692805       6,712883
977BAM001       3,805081       3,816531       3,827981
978EUR001       7,442092       7,464485       7,486878
985PLN001       1,747011       1,752268       1,757525";

            var lines = prices.Split("\r\n");
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace (line) && line.Length>= "036AUD001       4,600415       4,614258       4,628101".Length )
                {
                    var l = line.Replace(",", ".");
                    var koef = 1;
                    Int32.TryParse(l.Substring(6, 3),out koef);
                    var bid = 1M;
                    Decimal.TryParse(l.Substring(9, 15) , NumberStyles.Currency, CultureInfo.InvariantCulture, out bid);
                    var ask = 1M;
                    Decimal.TryParse(l.Substring(39, 15), NumberStyles.Currency, CultureInfo.InvariantCulture, out ask);
                    var p = new PriceItem()
                    {
                        Currency = l.Substring(3, 3),
                        Bid = bid / koef,
                        Ask = ask / koef
                    };
                    Items[p.Currency] = p;
                    //Console.WriteLine(line);
                }

                

            }
            Console.WriteLine(JsonConvert.SerializeObject(Items));
        }


        public string getQuotasMessage(String message)
        {
            decimal percentage = Convert.ToDecimal (random.NextDouble() * 200-100);
            if (decimal.TryParse(message, out percentage))
            {
                percentage = map(percentage, 5, 70, -100, 100);
            }
            return getQuotasMessage(percentage);
        }

        decimal map(decimal x, decimal in_min, decimal in_max, decimal out_min, decimal out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        public string getQuotasMessage(decimal percentage)
        {
            var sorted = from v in Items.Values orderby v.Currency select v;
            var calculated = new List<PriceItem>();
            foreach (var item in sorted )
            {
                var delta = item.Bid - item.Ask;
                calculated.Add(
                    new PriceItem()
                    {
                        Currency = item.Currency ,
                        Bid = Math.Round(item.Bid - delta * percentage / 100.0M,6),
                        Ask = Math.Round(item.Ask- delta * percentage / 100.0M,6),
                    }
                    );
            }
            return JsonConvert.SerializeObject(calculated, Formatting.Indented );
        }
    }
}
