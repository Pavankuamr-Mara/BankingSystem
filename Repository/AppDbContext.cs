using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInMemoryDbDemo
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AppDb");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
