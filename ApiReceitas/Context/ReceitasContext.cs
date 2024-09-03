using ApiReceitas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiReceitas.Context
{
    public class ReceitasContext : DbContext
    {
        public ReceitasContext(DbContextOptions<ReceitasContext> options) : base(options)
        { }
        public DbSet<Receitas> Receitas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Receitas>()
                .Property(r => r.Ingredientes)
                .HasConversion(
                    v => string.Join(";", v), 
                    v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList() 
                );
        }
    }
}
