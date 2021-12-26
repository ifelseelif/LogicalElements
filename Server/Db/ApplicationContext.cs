using Microsoft.EntityFrameworkCore;
using Server.Db.models;

namespace Server.Db
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Element> Elements { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}