using Microsoft.EntityFrameworkCore;
using ProductosAPI.Models;

namespace ProductosAPI.Data
{
    public class ProductosContext : DbContext
    {
        public ProductosContext(DbContextOptions<ProductosContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }
}
