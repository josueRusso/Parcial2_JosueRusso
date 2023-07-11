using Microsoft.EntityFrameworkCore;
using Parcial2_JosueRusso.Shared;

namespace Parcial2_JosueRusso.Server.DAL
{
    public class Context : DbContext
    {
        public DbSet<Entradas> Entradas { get; set; }

        public DbSet<Productos> Productos { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Productos>().HasData(new List<Productos>()
            {
                new Productos(){ ProductoId = 1,Descripcion = "Mani",Tipo = 0,Existencia = 50 },
                new Productos(){ ProductoId = 2,Descripcion = "Pistacho",Tipo = 0,Existencia = 600 },
                new Productos(){ ProductoId = 3,Descripcion = "Pasas",Tipo = 0,Existencia = 500 },
                new Productos(){ ProductoId = 4,Descripcion = "Ciruelas",Tipo = 0,Existencia = 700 },
                new Productos(){ ProductoId = 5,Descripcion = "Mixto MPP 0.5 lb", Tipo = 0,Existencia = 0 },
                new Productos(){ ProductoId = 6,Descripcion = "Mixto MPC 0.5 lb", Tipo = 0,Existencia = 0 },
                new Productos(){ ProductoId = 7,Descripcion = "Mixto MPP 0.2 lb", Tipo = 0,Existencia = 0 },
            });
        }

    }
}
