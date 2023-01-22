using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Football.Models;

namespace Football.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Team> Teams { get; set; }

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var countries = new Country[3];
            countries[0] = new Country { ID = System.Guid.NewGuid(), Name = "Италия" };
            countries[1] = new Country{ ID = System.Guid.NewGuid(), Name = "США" };
            countries[2] = new Country { ID = System.Guid.NewGuid(), Name = "РФ" };

            // Имеет смысл с самого начала добавить в качестве команды опцию "Свободный агент", т.к. игрок может не иметь контракта ни с одной из команд. 
            var teams = new Team[1];
            teams[0] = new Team { ID = System.Guid.NewGuid(), Name = "Свободный агент" };  
            modelBuilder.Entity<Country>().HasData(countries);
            modelBuilder.Entity<Team>().HasData(teams);
        }
    }
}
