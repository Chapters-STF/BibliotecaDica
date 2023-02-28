using BibliotecasDicas.Data;
using BibliotecasDicas.Network;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using BibliotecasDicas.Repository;

namespace BibliotecasDicas
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connSqlLiteString = config.GetConnectionString("sqliteDefault");

            // Dependency Injection
            var services = new ServiceCollection();
            services.AddDbContext<RickMortyContext>(opt => opt.UseSqlite(connSqlLiteString));

            // Refit com token

            // var refitSetting = new RefitSettings()
            //{
            //    AuthorizationHeaderValueGetter = () => Task.FromResult(config.GetSection("api")["movieToken"])
            //};            
            //services.AddRefitClient<RickMortyAPI>(refitSetting)

            services.AddRefitClient<RickMortyAPI>()
                .ConfigureHttpClient(c => {
                    c.BaseAddress = new Uri(config.GetSection("api")["rickmorty"]);                    
                });


            var serviceProvider = services.BuildServiceProvider();

            var rickApi = serviceProvider.GetService<RickMortyAPI>();
            var rickdb = serviceProvider.GetService<RickMortyContext>();
            var runApp = new RickAndMortyRepository(rickApi, rickdb);
            //runApp.RunWithText().Wait();
            runApp.RunSaveDb().Wait();
        }
    }
    
}