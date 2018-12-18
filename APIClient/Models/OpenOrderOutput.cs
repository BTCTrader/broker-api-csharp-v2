using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public class OpenOrderOutput
    {
        public IList<Order> Asks { get; set; }

        public IList<Order> Bids { get; set; }
    }

    public class Order
    {
        public long Id { get; set; }

        public string Price { get; set; }

        public string Quantity { get; set; }

        public string Amount { get; set; }

        public string PairSymbol { get; set; }

        public string PairSymbolNormalized { get; set; }

        public string Type { get; set; }

        public string Method { get; set; }

        public string OrderClientId { get; set; }

        public long Time { get; set; }

        public long UpdateTime { get; set; }

        public string Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Price: {Price}, Amount: {Amount}, Quantity: {Quantity}, PairSymbol: {PairSymbol}, PairSymbolNormalized: {PairSymbolNormalized}, Type: {Type}, Method: {Method}, OrderClientId: {OrderClientId}, Time: {Time}, UpdateTime: {UpdateTime}, Status: {Status}";
        }
    }
}
