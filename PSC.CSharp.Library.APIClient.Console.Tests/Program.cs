    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PSC.CSharp.Library.DemoAPIClient;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var myApi = host.Services.GetRequiredService<IPersonService>();

            myApi.AddPerson(new PSC.CSharp.Library.DemoAPIClient.Models.Person.PersonModel()
            {
                FirstName = "Enrico",
                LastName = "Rossini"
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddApiClientServices("test", "https://my.url");
                });
    }