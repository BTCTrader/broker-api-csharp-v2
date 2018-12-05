using System.Collections.Generic;
using System.Threading.Tasks;
using APIClient.Models;

namespace APIClient
{
    public interface IApiClient
    {
        Task<ReturnModel<OrderBook>> GetOrderBook(string pairSymbol, int limit = 30);

        Task<ReturnModel<IList<Trades>>> GetLastTrades(string pairSymbol, int numberOfTrades);

        Task<ReturnModel<OrderOutput>> CreateOrder(OrderInput orderInput);

        Task<bool> CancelOrder(long id);

        Task<ReturnModel<IList<Ticker>>> GetTicker(string pairSymbol);

        Task<ReturnModel<IList<Ticker>>> GetTicker();

        Task<ReturnModel<IList<UserBalance>>> GetBalances();

        Task<ReturnModel<OpenOrderOutput>> GetOpenOrders(string pairSymbol = null);

        Task<ReturnModel<IList<OHLC>>> GetDailyOhlc(string pairSymbol, int last);

        Task<ReturnModel<IList<UserTrade>>> GetUserTrades(string[] type, string[] symbol, long startDate, long endDate);

        Task<ReturnModel<IList<UserTransaction>>> GetUserFiatTransactions(string[] type, string[] symbol, long startDate, long endDate);

        Task<ReturnModel<IList<UserTransaction>>> GetUserCryptoTransactions(string[] type, string[] symbol, long startDate, long endDate);
    }
}
