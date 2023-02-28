using BibliotecasDicas.Data;
using BibliotecasDicas.Network;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.Json;
using Refit;
using BibliotecasDicas.Models.Rick;

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

            // Dependencies
            var services = new ServiceCollection();
            services.AddDbContext<RickMortyContext>(opt => opt.UseSqlite(connSqlLiteString));

            // com token
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
            var runApp = new RunApp(rickApi, rickdb);
            //runApp.RunWithText().Wait();
            runApp.RunSaveDb().Wait();
        }
    }
    public class RunApp
    {
        public RickMortyAPI _api { get; set; }
        public RickMortyContext _context { get; set; }
        public RunApp(RickMortyAPI api, RickMortyContext context)
        {
            _api = api;
            _context = context;
        }

        public async Task RunWithText()
        {
            try
            {
                var characters = await _api.GetCharacters();
                Console.WriteLine(JsonSerializer.Serialize(characters));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task RunSaveDb()
        {
            try
            {
                var dbCharactersId = _context.RickMorty.AsNoTracking().Select(p => p.id).ToList(); 
                var characters = await _api.GetCharacters();

                var uniqueFirst = characters.results.FirstOrDefault(p => !dbCharactersId.Contains(p.id));
                _context.RickMorty.Add(uniqueFirst);
                _context.SaveChanges();

                var dbCharacters = _context.RickMorty.AsNoTracking().ToList();

                Console.WriteLine(JsonSerializer.Serialize(dbCharacters));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}