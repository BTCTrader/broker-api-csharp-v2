using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APIClient.Helpers;
using APIClient.Models;
using IdentityModel.Client;

namespace APIClient.ApiClientV2
{
    public class ApiClientV2 : IApiClient
    {
        private readonly string _resourceUrl;
        private readonly TokenResponse _tokenResponse;
        /// <summary>
        /// Use for methods which do not need authentication
        /// </summary>
        /// <param name="resourceUrl"></param>
        public ApiClientV2(string resourceUrl)
        {
            _resourceUrl = resourceUrl;
        }

        /// <summary>
        /// Use for methods which need authentication
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <param name="tokenResponse"></param>
        public ApiClientV2(string resourceUrl, TokenResponse tokenResponse)
        {
            _resourceUrl = resourceUrl;
            _tokenResponse = tokenResponse;
        }

        /// <summary>
        /// Creates new order
        /// </summary>
        /// <param name="orderInput">OrderInput</param>
        /// <returns>OrderOutput</returns>
        public async Task<ReturnModel<OrderOutput>> CreateOrder(OrderInput orderInput)
        {
            var requestUrl = "api/v2/order";

            var response = await SendRequest(HttpVerbs.Post, requestUrl, orderInput, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<OrderOutput>();

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

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>true if successfully canceled otherwise false</returns>
        public async Task<bool> CancelOrder(long id)
        {
            var requestUrl = $"api/v2/order?id={id}";

            var response = await SendRequest(HttpVerbs.Delete, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<string>();

            return returnModel.Success;
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
        /// Gets last trades by pair
        /// </summary>
        /// <param name="pairSymbol">pair symbol</param>
        /// <param name="numberOfTrades">number of returned trades</param>
        /// <returns>List of trades</returns>
        public async Task<ReturnModel<IList<Trades>>> GetLastTrades(string pairSymbol, int numberOfTrades)
        {
            var requestUrl = $"api/v2/trades?pairSymbol={pairSymbol}&last={numberOfTrades}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl);

            var returnModel = response.ToReturnModel<IList<Trades>>();

            return returnModel;
        }

        //public async Task<ReturnModel<IList<UserTrade>>> GetUserTrades()
        //{

        //}

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
        /// Gets user balances
        /// </summary>
        /// <returns>List of currencies balances</returns>
        public async Task<ReturnModel<IList<UserBalance>>> GetBalances()
        {
            var requestUrl = "api/v2/users/balances";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserBalance>>();

            return returnModel;
        }

        /// <summary>
        /// Gets user open orders by pair
        /// </summary>
        /// <param name="pairSymbol">pair symbol</param>
        /// <returns>OpenOrderOutput list of asks and bids</returns>
        public async Task<ReturnModel<OpenOrderOutput>> GetOpenOrders(string pairSymbol = null)
        {
            var requestUrl = $"api/v2/openOrders?pairSymbol={pairSymbol}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<OpenOrderOutput>();

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
        /// Gets access token for user
        /// </summary>
        /// <returns>access token</returns>


        private async Task<HttpResponseMessage> SendRequest(HttpVerbs action, string url, object inputModel = null, bool requiresAuthentication = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_resourceUrl);
                client.Timeout = TimeSpan.FromSeconds(30);

                if (requiresAuthentication)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResponse.AccessToken);
                }
                HttpResponseMessage response = null;
                try
                {
                    switch (action)
                    {
                        case HttpVerbs.Get:
                            response = await client.GetAsync(url);
                            break;
                        case HttpVerbs.Post:
                            var postContent = inputModel.ToHttpContent();
                            response = await client.PostAsync(url, postContent);
                            break;
                        case HttpVerbs.Put:
                            var putContent = inputModel.ToHttpContent();
                            response = await client.PutAsync(url, putContent);
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
                    return response;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine($"Token is unauthorized to do this action: [{action.ToString().ToUpper()}] /{url}. Please check your bearer token in request header.");
                }

                return response;
            }
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
            var requestUrl = $"api/v2/users/transactions/trade?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

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

            var requestUrl = $"api/v2/users/transactions/fiat?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

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

            var requestUrl = $"api/v2/users/transactions/crypto?{typeBuilder}{symbolBuilder}startDate={startDate}&endDate={endDate}";

            var response = await SendRequest(HttpVerbs.Get, requestUrl, requiresAuthentication: true);

            var returnModel = response.ToReturnModel<IList<UserTransaction>>();

            return returnModel;
        }
    }
}
