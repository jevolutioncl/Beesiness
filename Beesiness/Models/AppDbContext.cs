using Microsoft.EntityFrameworkCore;

namespace Beesiness.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
    :   base(options)
        {
        }
        public DbSet<Colmena> tblColmenas { get; set; }
        public DbSet<Enfermedad> tblEnfermedades { get; set; }
        public DbSet<EnfermedadColmena> tblEnfermedadColmena { get; set; }
        public DbSet<Tratamiento> tblTratamientos { get; set; }
        public DbSet<Tarea> tblTareas { get; set; }
        public DbSet<TareaColmena> tblTareaColmena { get; set; }
        public DbSet<Alertas> tblAlertas { get; set; }
        public DbSet<InfoSensores> tblInfoSensores { get; set; }
        public DbSet<Nota> tblNotas { get; set; }
        public DbSet<Polinizacion> tblPolinizaciones { get; set; }
        public DbSet<ColmenaPolinizacion> tblColmenaPolinizacion { get; set; }
        public DbSet<TipoFlor> tblTipoFlor { get; set; }
        public DbSet<Produccion> tblProducciones { get; set; }
        public DbSet<Usuario> tblUsuarios { get; set; }
        public DbSet<Rol> tblRoles { get; set; }
        public DbSet<Inspeccion> tblInspecciones { get; set; }
        public DbSet<UsuarioTemporal> tblUsuariosTemporales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Beesiness;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ColmenaPolinizacion>()
                .HasKey(cp => new { cp.IdPolinizacion, cp.IdColmena });
        }


    }
}