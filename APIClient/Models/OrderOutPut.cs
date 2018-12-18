using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public class OrderOutput
    {
        public long Id { get; set; }

        public long Datetime { get; set; }

        public string Type { get; set; }

        public string Method { get; set; }

        public string Price { get; set; }

        public string Amount { get; set; }

        public string Quantity { get; set; }

        public string PairSymbol { get; set; }

        public string PairSymbolNormalized { get; set; }


        public string NewOrderClientId { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}, Datetime {Datetime}, Type {Type}, Method {Method}, Price {Price}, Amount {Amount}, Quantity {Quantity}, PairSymbol {PairSymbol}, PairSymbolNormalized {PairSymbolNormalized}";
        }
    }
}
