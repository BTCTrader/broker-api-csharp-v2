using System;
using APIClient.ApiClientV1;
using APIClient.Helpers;
using APIClient.Models;
using Microsoft.Extensions.Configuration;

namespace APIExampleV1
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var publicKey = configuration["publicKey"];
            var privateKey = configuration["privateKey"];
            var resourceUrl = configuration["resourceUrl"];

            var apiClientV1 = new ApiClientV1(publicKey, privateKey, resourceUrl);

            var tickerList = apiClientV1.GetTicker("BTCTRY");
            if (tickerList.Result.Success)
            {
                foreach (var ticker in tickerList.Result.Data)
                {
                    Console.WriteLine(ticker.ToString());
                }
            }
            else
            {
                Console.WriteLine(tickerList.Result.ToString());
            }

            // Print the best bid price and amount to the console
            var orderbook = apiClientV1.GetOrderBook("BTCTRY");

            if (orderbook.Result.Success)
            {
                var bestBidPrice = orderbook.Result.Data.Bids[0][0];
                var bestBidAmount = orderbook.Result.Data.Bids[0][1];
                Console.WriteLine("Best bid price:" + bestBidPrice);
                Console.WriteLine("Best bid amount:" + bestBidAmount);
            }
            else
            {
                Console.WriteLine(orderbook.Result.ToString());
            }

            // Print the last 10 trades in the market to the console.
            var trades = apiClientV1.GetLastTrades("BTCTRY", 10);

            if (trades.Result.Success)
            {
                Console.WriteLine("Last 10 trades in the market");
                foreach (var trade in trades.Result.Data)
                {
                    Console.WriteLine(trade.ToString());
                }
            }
            else
            {
                Console.WriteLine(trades.Result.ToString());
            }

            // Print the last 7 days' OHLC to the console
            var ohlc = apiClientV1.GetDailyOhlc("BTCTRY", 7);

            if (ohlc.Result.Success)
            {
                foreach (var dailyOhlc in ohlc.Result.Data)
                {
                    Console.WriteLine(dailyOhlc);
                }
            }
            else
            {
                Console.WriteLine(ohlc.Result.ToString());
            }

            var limitSellOrder = new OrderInput
            {
                Quantity = 0.001m,
                NewOrderClientId = "test",
                OrderMethod = OrderMethod.Limit,
                OrderType = OrderType.Sell,
                Price = 21000m,
                PairSymbol = "BTCTRY"
            };

            var limitBuyOrder = new OrderInput
            {
                Quantity = 0.001m,
                NewOrderClientId = "test",
                OrderMethod = OrderMethod.Limit,
                OrderType = OrderType.Buy,
                Price = 20000m,
                PairSymbol = "BTCTRY"
            };

            //Create New Order
            var orderOutput = apiClientV1.CreateOrder(limitBuyOrder);

            if (orderOutput.Result == null)
            {
                Console.WriteLine("Could not get response from server");
                Console.Read();
                return;
            }

            Console.WriteLine(!orderOutput.Result.Success
                ? $"Code:{orderOutput.Result.Code} , Message: {orderOutput.Result.Message}"
                : orderOutput.Result.Data.ToString());


            var openOrders = apiClientV1.GetOpenOrders();

            long orderId = 0;
            if (openOrders.Result != null && openOrders.Result.Success)
            {
                foreach (var askOrder in openOrders.Result.Data.Asks)
                {
                    Console.WriteLine(askOrder);
                }

                orderId = openOrders.Result.Data.Asks[0].Id;
                foreach (var bidOrder in openOrders.Result.Data.Bids)
                {
                    Console.WriteLine(bidOrder);
                }
            }

            //Get Balances for each currency
            var balances = apiClientV1.GetBalances();

            if (balances.Result != null && balances.Result.Success)
            {
                foreach (var balance in balances.Result.Data)
                {
                    Console.WriteLine(balance.ToString());
                }
            }

            // Cancel order
            var cancelOrder = apiClientV1.CancelOrder(orderId);

            if (cancelOrder.Result)
            {
                Console.WriteLine($"Successfully canceled order {orderId}");
            }
            else
            {
                Console.WriteLine("Could not cancel order");
            }


            var userTrades = apiClientV1.GetUserTrades(new[] { "buy,sell" }, new[] { "try,btc" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

            if (userTrades.Result.Success)
            {
                foreach (var userTrade in userTrades.Result.Data)
                {
                    Console.WriteLine(userTrade);
                }
            }

            var userFiatTransactions = apiClientV1.GetUserFiatTransactions(new[] { "deposit", "withdrawal" }, new[] { "try" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

            if (userFiatTransactions.Result.Success)
            {
                foreach (var userFiatTransaction in userFiatTransactions.Result.Data)
                {
                    Console.WriteLine(userFiatTransaction);
                }
            }

            var userCryptoTransactions = apiClientV1.GetUserCryptoTransactions(new[] { "deposit", "withdrawal" }, new[] { "btc","eth","xrp" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

            if (userCryptoTransactions.Result.Success)
            {
                foreach (var userCryptoTransaction in userCryptoTransactions.Result.Data)
                {
                    Console.WriteLine(userCryptoTransaction);
                }
            }

            Console.Read();
        }
    }
}
