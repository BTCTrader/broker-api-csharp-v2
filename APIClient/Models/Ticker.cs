using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public class Ticker
    {
        public string Pair { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public decimal Last { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Volume { get; set; }
        public decimal Average { get; set; }
        public decimal Daily { get; set; }
        public decimal DailyPercent { get; set; }
        public string DenominatorSymbol { get; set; }
        public string NumeratorSymbol { get; set; }
        public double Timestamp { get; set; }

        public override string ToString()
        {
            return "Pair: " + Pair + "\n" + "Last: " + Last + "\n" + "High: " + High + "\n" +
                   "Low: " + Low + "\n" + "Volume: " + Volume + "\n" +
                   "Bid: " + Bid + "\n" + "Ask: " + Ask + "\n" +
                   "Open: " + Open + "\n" + "Average: " + Average + "\n" + "Daily: " + Daily + "\n" +
                   "DailyPercent: " + DailyPercent + "\n" + "DenominatorSymbol: " + DenominatorSymbol + "\n" +
                   "NumeratorSymbol: " + NumeratorSymbol + "\n" + "Timestamp: " + Timestamp;
        }
    }
}
