using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecasDicas.Data
{
    // Used to create EF Core code first when there is no webhost
    public class RickMortyContextFactory : IDesignTimeDbContextFactory<RickMortyContext>
    {
        
        public RickMortyContextFactory()
        {
        }
        public RickMortyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RickMortyContext>();
            optionsBuilder.UseSqlite("DataSource=mydatabase.db;");

            return new RickMortyContext(optionsBuilder.Options);
        }
    }
}
