using Microsoft.EntityFrameworkCore;

namespace Factured_PruebaTecnica_API.Entities
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Task1> Tasks { get; set; }
    }
}