using BibliotecasDicas.Models.Rick;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecasDicas.Network
{
    public interface RickMortyAPI
    {
        [Get("/")]
        Task<RickMortyWrapper> GetCharacters();
        
        [Headers("Authorization: Bearer")]
        [Get("/")]
        Task<RickMortyWrapper> GetCharactersToken();
    }
}
