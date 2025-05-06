using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.CSharp.Library.DemoAPIClient
{
    /// <summary>
    /// Registration Service for the concrete implementation
    /// </summary>
    public static class ApiClientRegistrationService
    {
        /// <summary>
        /// Adds the API client services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="apikey">The apikey.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddIRPApiClientServices(this IServiceCollection services, string apikey, string baseUrl)
        {
            {
                services.AddApiService<IPersonService, PersonService>("personApi", baseUrl, apikey);

                return services;
            }
        }

        /// <summary>
        /// Adds the API service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the t implementation.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="clientName">Name of the client.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="apiKey">The API key.</param>
        public static void AddApiService<TInterface, TImplementation>(this IServiceCollection services, 
            string clientName, string baseUrl, string apiKey)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            services.AddScoped<TInterface>(provider =>
            {
                HttpClient client = provider.GetRequiredService<IHttpClientFactory>().CreateClient(clientName);
                client.BaseAddress = new Uri(baseUrl);

                ILogger<TImplementation> logger = provider.GetRequiredService<ILogger<TImplementation>>();
                return (TInterface)Activator.CreateInstance(typeof(TImplementation), apiKey, client, logger)!;
            });
        }
    }
}
