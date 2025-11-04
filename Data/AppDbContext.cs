using Microsoft.EntityFrameworkCore;

namespace CafezinhoELivrosApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        //Tabelas
        //public DbSet<T> Table {get; set;} = null!;
    }
}
