using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using APIClient.Helpers;
using APIClient.Models;

namespace APIClient.ApiClientV1
{
    public class ApiClientV1 : IApiClient
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _resourceUrl;

        public ApiClientV1(string publicKey, string privateKey, string resourceUrl)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;
            _resourceUrl = resourceUrl;
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
        }

        /// <summary>
        /// Cancels order with given order Id
        /// </summary>
        /// <returns>True if order was cancelled, false otherwise</returns>
        public async Task<bool> CancelOrder(long id)
        {
            var requestUrl = $"api/v1/order?id={id}";

            var response = await SendRequest(HttpVerbs.Delete, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<string>();

            return returnModel.Success;
        }


        /// <summary>
        /// Creates given Order. Requires authentication.
        /// </summary>
        /// <param name="orderInput">Order to be created</param>
        /// <returns>An object of OrderOutPut for the created order information</returns>
        public async Task<ReturnModel<OrderOutput>> CreateOrder(OrderInput orderInput)
        {
            const string requestUrl = "api/v1/order";

            var response = await SendRequest(HttpVerbs.Post, requestUrl, orderInput, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<OrderOutput>();

            return returnModel;
        }

        /// <summary>
        /// Get the authenticated account's balances
        /// </summary>
        /// <returns>A list of type UserBalance for each currency. Null if account balance cannot be retreived </returns>
        public async Task<ReturnModel<IList<UserBalance>>> GetBalances()
        {
            const string requestUrl = "api/v1/users/balances";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserBalance>>();

            return returnModel;
        }

        /// <summary>
        /// Gets the daily open, high, low, close, average etc. data in the market 
        /// </summary>
        /// <param name="pairSymbol">pair symbol</param>
        /// <param name="last">The number of days to request</param>
        /// <returns>The OHLC data for the last given number of days</returns>
        public async Task<ReturnModel<IList<OHLC>>> GetDailyOhlc(string pairSymbol, int last)
        {
            var requestUrl = $"api/v2/ohlc?pairSymbol={pairSymbol}&last={last}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<IList<OHLC>>();

            return returnModel;
        }

        public async Task<ReturnModel<IList<UserTrade>>> GetUserTrades(string[] types, string[] symbols, long startDate, long endDate)
        {
            var typeBuilder = new StringBuilder();
            foreach (var type in types)
            {
                typeBuilder.Append($"type={type}&");
            }

            var symbolBuilder = new StringBuilder();

            foreach (var symbol in symbols)
            {
                symbolBuilder.Append($"symbol={symbol}&");
            }
            var requestUrl = $"api/v1/users/transactions/trade?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl,requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserTrade>>();

            return returnModel;
        }

        public async Task<ReturnModel<IList<UserTransaction>>> GetUserFiatTransactions(string[] types, string[] symbols, long startDate, long endDate)
        {
            var typeBuilder = new StringBuilder();
            foreach (var type in types)
            {
                typeBuilder.Append($"type={type}&");
            }

            var symbolBuilder = new StringBuilder();

            foreach (var symbol in symbols)
            {
                symbolBuilder.Append($"symbol={symbol}&");
            }

            var requestUrl = $"api/v1/users/transactions/fiat?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl,requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserTransaction>>();

            return returnModel;
        }

        public async Task<ReturnModel<IList<UserTransaction>>> GetUserCryptoTransactions(string[] types, string[] symbols, long startDate, long endDate)
        {
            var typeBuilder = new StringBuilder();
            foreach (var type in types)
            {
                typeBuilder.Append($"type={type}&");
            }

            var symbolBuilder = new StringBuilder();

            foreach (var symbol in symbols)
            {
                symbolBuilder.Append($"symbol={symbol}&");
            }

            var requestUrl = $"api/v1/users/transactions/crypto?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl,requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserTransaction>>();

            return returnModel;
        }
        /// <summary>
        /// Get the last trades in the market by pairsymbol.
        /// </summary>
        /// <param name="pairSymbol">a pair symbol ex. BTCTRY</param>
        /// <param name="numberOfTrades">The number of trades that will be requested.</param>
        /// <returns>The requested number of last trades in the market.</returns>
        public async Task<ReturnModel<IList<Trades>>> GetLastTrades(string pairSymbol, int numberOfTrades)
        {
            var requestUrl = $"api/v1/trades?pairSymbol={pairSymbol}&last={numberOfTrades}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<IList<Trades>>();

            return returnModel;
        }

        /// <summary>
        /// Get all open orders of the user
        /// </summary>
        /// <returns>Users open orders listed. Null if there was an error</returns>
        public async Task<ReturnModel<OpenOrderOutput>> GetOpenOrders(string pairSymbol = null)
        {
            var requestUrl = $"api/v1/openOrders?pairSymbol={pairSymbol}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<OpenOrderOutput>();

            return returnModel;
        }

        /// <summary>
        /// Gets orderbook by pair
        /// </summary>
        /// <param name="pairSymbol">pair symbol</param>
        /// <param name="limit">number of returned orders for buy/sell</param>
        /// <returns>OrderBook</returns>
        public async Task<ReturnModel<OrderBook>> GetOrderBook(string pairSymbol, int limit = 30)
        {
            var requestUrl = $"api/v2/orderBook?pairSymbol={pairSymbol}&limit={limit}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<OrderBook>();

            return returnModel;
        }

        /// <summary>
        /// Gets all pairs ticker values 
        /// </summary>
        /// <returns>List of ticker values</returns>
        public async Task<ReturnModel<IList<Ticker>>> GetTicker()
        {
            var requestUrl = "api/v2/ticker";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<IList<Ticker>>();

            return returnModel;
        }

        /// <summary>
        /// Gets ticker by pair
        /// </summary>
        /// <param name="pairSymbol">pair symbol</param>
        /// <returns>Ticker values</returns>
        public async Task<ReturnModel<IList<Ticker>>> GetTicker(string pairSymbol)
        {
            var requestUrl = $"api/v2/ticker?pairSymbol={pairSymbol}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<IList<Ticker>>();

            return returnModel;
        }

        /// <summary>
        /// Generate a signature by giving timestamp
        /// </summary>
        /// <param name="stamp">long tickes for current time</param>
        /// <returns>string segnature</returns>
        private string GetSignature(long stamp)
        {
            string signature = null;
            try
            {
                var data = $"{_publicKey}{stamp}";
                using (var hmac = new HMACSHA256(Convert.FromBase64String(_privateKey)))
                {
                    var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                    signature = Convert.ToBase64String(signatureBytes);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception occured in GetSignature method. The likely cause is a private or public key in wrong format. Exception:" + e.Message);
            }

            return signature;
        }

        private static long GetStamp()
        {
            var stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return stamp;
        }

        /// <summary>
        /// Handles the response received from the application. You can change this method to have custom error handling in your app.
        /// </summary>
        /// <returns>Returns false if there were no errors. True if request failed.</returns>
        private static bool RequestSucceeded(HttpResponseMessage response)
        {
            var result = true;
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received error. Status code: " + response.StatusCode + ". Error message: " +
                                response.ReasonPhrase);
                result = false;
            }
            else
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                if (obj is JObject && obj["error"] != null)
                {
                    Debug.WriteLine("Received error. Status code: " + (obj["error"]["code"].ToString() as string) +
                                    ". Error message: " + (obj["error"]["message"].ToString() as string));
                    result = false;
                }
            }

            return result;
        }

        private async Task<HttpResponseMessage> SendRequest(HttpVerbs action, string url, object inputModel = null, bool requiresAuthentication = false)
        {
            HttpResponseMessage response = null;

            using (var client = new HttpClient { BaseAddress = new Uri(_resourceUrl), Timeout = TimeSpan.FromSeconds(30) })
            {
                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Add("X-PCK", _publicKey);
                    var stamp = GetStamp();
                    client.DefaultRequestHeaders.Add("X-Stamp", stamp.ToString(CultureInfo.InvariantCulture));
                    var signature = GetSignature(stamp);
                    client.DefaultRequestHeaders.Add("X-Signature", signature);
                }

                try
                {
                    switch (action)
                    {
                        case HttpVerbs.Post:
                            var postContent = inputModel.ToHttpContent();
                            response = await client.PostAsync(url, postContent);
                            break;
                        case HttpVerbs.Get:
                            response = await client.GetAsync(url);
                            break;
                        case HttpVerbs.Delete:
                            response = await client.DeleteAsync(url);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    response = null;
                }

                if (response == null)
                {
                    Console.WriteLine("Cannot get response from server.");
                    return null;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Token is unauthorized to do this action: [{action.ToString().ToUpper()}] /{url}. Please check your bearer token in request header.");
                }

                return response;
            }
        }
    }
}
