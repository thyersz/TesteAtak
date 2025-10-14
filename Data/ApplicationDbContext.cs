using Microsoft.EntityFrameworkCore;
using TesteAtak.Models;

namespace TesteAtak.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
    }
}
