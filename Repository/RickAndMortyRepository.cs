using BibliotecasDicas.Data;
using BibliotecasDicas.Network;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BibliotecasDicas.Repository
{
    public class RickAndMortyRepository
    {

        public RickMortyAPI _api { get; set; }
        public RickMortyContext _context { get; set; }
        public RickAndMortyRepository(RickMortyAPI api, RickMortyContext context)
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
