namespace APIClient.Models
{
    public class Trades
    {
        public string Pair { get; set; }

        public string PairNormalized { get; set; }

        public long Date { get; set; }

        public string Tid { get; set; }

        public string Price { get; set; }

        public string Amount { get; set; }

        public override string ToString()
        {
            return $"Pair: {Pair} \n PairNormalized: {PairNormalized} \n Date: {Date} \n Tid: {Tid} \n Price: {Price} \n Amount: {Amount}";
        }
    }
}