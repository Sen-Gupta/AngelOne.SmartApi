using AngelOne.SmartApi.Clients;
using AngelOne.SmartApi.Clients.Enums;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            // Use GetRequiredService to ensure that the service is available
            var marketDataClient = serviceProvider.GetRequiredService<MarketDataClient>();

            // Login
            var profile = await marketDataClient.GetProfile();   
            Console.WriteLine($"Profile: {profile?.Name}");

            //Quotes
            var quoteRequest = new QuoteRequest();
            quoteRequest.Mode = Modes.FULL.ToString();
            quoteRequest.ExchangeNameTokens.Add("NSE", new List<string> { "3045"});

            var quoteResult = await marketDataClient.GetQuotes(quoteRequest);
            if(quoteResult.HasFailedQuotes)
            {
                Console.WriteLine($"Failed Quotes: {quoteResult.FailedQuotes.Count}");
            }
            else
            {
                Console.WriteLine($"Quotes: {quoteResult.Quotes.Count}");
            }   

            return 0; // or another exit code if needed
        }
    }
}
