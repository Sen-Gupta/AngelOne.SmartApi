using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Sockets;
using AngelOne.SmartApi.Clients.Sockets.Interface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace AngelOne.SmartApi.Clients
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartApiClients(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            if(configuration == null)
            {
                Console.WriteLine("Configuration is null. There may be an issue loading the JSON file.");
                return services;
            }
            var smartApiSettings = configuration.GetSection("SmartApi").Get<SmartApiSettings>();

            services.AddHttpClient<AngelOneAuthClient>(client =>
            {
                client.BaseAddress = new Uri($"{Constants.Endpoints.BaseUrls.Auth}{Constants.Endpoints.Login}");
                // Configure other HttpClient settings as needed
            });

            services.AddHttpClient<AngelOneTokenClient>(client =>
            {
                client.BaseAddress = new Uri($"{Constants.Endpoints.BaseUrls.Auth}{Constants.Endpoints.Token}");
                // Configure other HttpClient settings as needed
            });

            services.AddHttpClient<SmartApiClient>(client =>
            {
                client.BaseAddress = new Uri(Constants.Endpoints.BaseUrls.API);
                // Configure other HttpClient settings as needed
            });

            services.AddSingleton(smartApiSettings!);

            // Register other services or dependencies needed by Clients
            services.TryAddSingleton(_ => configuration);
            services.TryAddSingleton<TokenManager>();
            services.TryAddScoped<ISmartWebSocket, SmartWebSocket>();
            services.TryAddScoped<TickerService>();
            return services;
        }
    }
}
