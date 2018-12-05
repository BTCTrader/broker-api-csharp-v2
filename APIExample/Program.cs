using System;
using APIClient;
using APIClient.ApiClientV2;
using APIClient.Helpers;
using APIClient.Models;
using Microsoft.Extensions.Configuration;

namespace APIExampleV2
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var username = configuration["username"];
            var password = configuration["password"];
            var resourceUrl = configuration["resourceUrl"];
            var authenticationUrl = configuration["authenticationUrl"];

            var authentication = new Authentication(username, password, authenticationUrl);

            var tokenResponse = authentication.GetTokenResponse().Result;

            if (tokenResponse == null)
            {
                Console.WriteLine("Could not get token response");
                Console.Read();
                return;
            }

            if (tokenResponse.IsError)
            {
                Console.WriteLine($"tokenResponse got an error : {tokenResponse.ErrorDescription}");
                Console.Read();
                return;
            }

            Console.WriteLine($"Token expires in {tokenResponse.ExpiresIn / 60} minutes");

            //for refresh token you can use it like this
            //tokenResponse = authentication.GetTokenResponse(tokenResponse.RefreshToken).Result;


            var apiClient = new ApiClientV2(resourceUrl, tokenResponse);

            // Print the ticker to the console
            var tickerList = apiClient.GetTicker("BTCTRY");
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
            var orderbook = apiClient.GetOrderBook("BTCTRY");

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
            var trades = apiClient.GetLastTrades("BTCTRY", 10);

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
            var ohlc = apiClient.GetDailyOhlc("BTCTRY", 7);

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
                Quantity = 1m,
                NewOrderClientId = "test",
                OrderMethod = OrderMethod.Limit,
                OrderType = OrderType.Sell,
                Price = 100m,
                PairSymbol = "ETHTRY"
            };

            //Create New Order
            var orderOutput = apiClient.CreateOrder(limitSellOrder);

            if (orderOutput.Result != null && orderOutput.Result.Success)
            {
                Console.WriteLine(orderOutput.Result.Data.ToString());
            }

            var openOrders = apiClient.GetOpenOrders();

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
            var balances = apiClient.GetBalances();

            if (balances.Result != null && balances.Result.Success)
            {
                foreach (var balance in balances.Result.Data)
                {
                    Console.WriteLine(balance.ToString());
                }
            }

            // Cancel order
            var cancelOrder = apiClient.CancelOrder(orderId);

            if (cancelOrder.Result)
            {
                Console.WriteLine($"Successfully canceled order {orderId}");
            }
            else
            {
                Console.WriteLine("Could not cancel order");
            }

            var userTrades = apiClient.GetUserTrades(new[] { "buy,sell" }, new[] { "try,btc" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

            if (userTrades.Result.Success)
            {
                foreach (var userTrade in userTrades.Result.Data)
                {
                    Console.WriteLine(userTrade);
                }
            }

            var userFiatTransactions = apiClient.GetUserFiatTransactions(new[] { "deposit", "withdrawal" }, new[] { "try" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

            if (userFiatTransactions.Result.Success)
            {
                foreach (var userFiatTransaction in userFiatTransactions.Result.Data)
                {
                    Console.WriteLine(userFiatTransaction);
                }
            }

            var userCryptoTransactions = apiClient.GetUserCryptoTransactions(new[] { "deposit", "withdrawal" }, new[] { "btc","eth","xrp" }, DateTime.UtcNow.AddDays(-30).ToUnixTime(), DateTime.UtcNow.ToUnixTime());

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
