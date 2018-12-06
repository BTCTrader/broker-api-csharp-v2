namespace APIClient.Models
{
    public class UserTransaction
    {
        public long Id { get; set; }
        public string BalanceType { get; set; }
        public string CurrencySymbol { get; set; }
        public long Timestamp { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, BalanceType: {BalanceType}, CurrencySymbol: {CurrencySymbol}, " +
                   $"Timestamp: {Timestamp}, Amount: {Amount}, Fee: {Fee}, Amount: {Tax}";
        }
    }
}
