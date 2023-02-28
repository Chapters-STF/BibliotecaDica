using BibliotecasDicas.Models.Rick;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecasDicas.Data
{
    public class RickMortyContext : DbContext
    {
        public RickMortyContext(DbContextOptions<RickMortyContext> options) : base(options)
        {}
        public DbSet<Result> RickMorty { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Result>().HasKey(p => p.id);
        }
    }
}
