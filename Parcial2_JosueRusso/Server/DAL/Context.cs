using Microsoft.EntityFrameworkCore;
using Parcial2_JosueRusso.Shared;

namespace Parcial2_JosueRusso.Server.DAL
{
    public class Context : DbContext
    {
        public DbSet<Entradas> Entradas { get; set; }

        public 

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entradas>()
        }

    }
}
