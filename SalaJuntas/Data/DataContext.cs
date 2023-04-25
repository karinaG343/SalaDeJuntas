using Microsoft.EntityFrameworkCore;
using SalaJuntas.Models;

namespace SalaJuntas.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base(option){ }
        public DbSet<Reservaciones> Reservaciones { get; set; }
        public DbSet<Salas> Salas { get; set; }
    }
}
