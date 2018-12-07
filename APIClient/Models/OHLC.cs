using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public class OHLC
    {
        public string PairSymbol { get; set; }
        public string PairSymbolNormalized { get; set; }
        public long Time { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
        public string Average { get; set; }
        public string DailyChangeAmount { get; set; }
        public string DailyChangePercentage { get; set; }

        public override string ToString()
        {
            var result = "PairSymbol: " + PairSymbol;
            result += "\n PairSymbolNormalized: " + PairSymbolNormalized;
            result += "\n Time: " + Time;
            result += "\n" + "Open: " + Open;
            result += "\n" + "High: " + High;
            result += "\n" + "Low: " + Low;
            result += "\n" + "Volume: " + Volume;
            result += "\n" + "Average: " + Average;
            result += "\n" + "DailyChangeAmount: " + DailyChangeAmount;
            result += "\n" + "DailyChangePercentage: " + DailyChangePercentage;
            return result;
        }
    }
}
