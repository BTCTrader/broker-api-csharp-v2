using System;
using System.Collections.Generic;
using System.Text;

namespace APIClient.Models
{
    public class ReturnModel<T> where T : class
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Code { get; set; }

        public T Data { get; set; }

        public override string ToString()
        {
            return $"Code {Code}: , Message : {Message}";
        }
    }
}
