namespace APIClient.Models
{
    public class UserTrade
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public string NumeratorSymbol { get; set; }
        public string DenominatorSymbol { get; set; }
        public string OrderType { get; set; }
        public long Timestamp { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Price: {Price}, NumeratorSymbol: {NumeratorSymbol}, DenominatorSymbol: {DenominatorSymbol}," +
                   $" OrderType: {OrderType}, Timestamp: {Timestamp}, Amount: {Amount}, Fee: {Fee}, Amount: {Tax}";
        }
    }
}
