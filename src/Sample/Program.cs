using AngelOne.SmartApi.Clients;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Sockets;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace AngelOne.SmartApi.Client.Sample
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            //Registering the smart api clients
            var serviceProvider = new ServiceCollection()
           .AddSmartApiClients()  // Register services using AddSmartApi extension method
           .BuildServiceProvider();

            // use Auth Client for explicit logout
            var authClient = serviceProvider.GetRequiredService<AngelOneAuthClient>();

            // Use GetRequiredService to ensure that the service is available
            var marketDataClient = serviceProvider.GetRequiredService<MarketDataClient>();

            var tickerService = serviceProvider.GetRequiredService<TickerService>();

            // Login
            //var profile = await marketDataClient.GetProfile();   
            //Console.WriteLine($"Profile: {profile?.Name}");

            //Quotes
            var quoteRequest = new QuoteRequest();
            quoteRequest.Mode = Constants.Modes.FULL;
            quoteRequest.ExchangeNameTokens.Add("NSE", new List<string> { "3045" });

            //var quoteResult = await marketDataClient.GetQuotes(quoteRequest);


            //Candle Request
            var candleRequest = new CandleRequest();
            candleRequest.SymbolToken = "3045";
            candleRequest.Exchange = Constants.Exchanges.NSE;
            candleRequest.Interval = Constants.CANDLE_INTERVAL.ONE_DAY;
            candleRequest.FromDate = DateTime.Now.Date.AddDays(-7).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            candleRequest.ToDate = DateTime.Now.Date.AddDays(-2).ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            //var candleResponse = await marketDataClient.GetCandle(candleRequest);

            //RMS Limit
            var rmsLimit = await marketDataClient.GetRMSLimit();

            try
            {
                await tickerService.Connect();
                //Print to console each ping
                tickerService.OnTickPong += (message) =>
                {
                    Console.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} received Pong message: {message}");
                };
                tickerService.OnTickLtp += (message) =>
                {
                    Console.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} received LTP message: {message.Token}={message.LastTradedPrice}");
                };
                tickerService.OnTick += (message) =>
                {
                    Console.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} received LTP message: {message.Token}={message.TotalSellQuantity}");
                };
                tickerService.OnTickQuote += (message) =>
                {
                    Console.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} received LTP message: {message.Token}={message.VolTraded}");
                };
                var subscribeRequest = new SubscribeRequest();
                subscribeRequest.Tokens.Mode = Constants.TickerModes.Codes[Constants.TickerModes.FULL];
                subscribeRequest.Tokens.TokenList.Add(new Token { ExchangeType = Constants.Exchanges.Codes[Constants.Exchanges.NSE], Tokens = new List<string> { "3045", "11460" } });
                tickerService.Subscribe(subscribeRequest);
            }
            catch (Exception ex)
            {

            }

            // we need to keep running the progr
            while (true)
            {
                // do something
            }

            //Logout
            //var result = await authClient.Logout();

            //return 0; // or another exit code if needed
        }
    }
}
