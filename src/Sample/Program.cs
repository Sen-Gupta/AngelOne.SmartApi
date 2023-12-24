using AngelOne.SmartApi.Clients;
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
            var loginClient = serviceProvider.GetRequiredService<AngelOneLoginClient>();

            // Use the loginClient instance as needed
            await loginClient.Login();
            
            return 0; // or another exit code if needed
        }
    }
}
