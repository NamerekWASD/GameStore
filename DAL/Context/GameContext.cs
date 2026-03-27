using DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Context
{
	public class GameContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public GameContext() : base() { }

		public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
				return;
            }
            string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "API"));

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("Default");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Game> Games { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<CopyType> CopyTypes { get; set; }
		public DbSet<Region> AvailableRegions { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<GameSubscription> GameSubscriptions { get; set; }
		public DbSet<BillingAddress> BillingAddresses { get; set; }
		public DbSet<Platform> Platforms { get; set; }
		public DbSet<Publisher> Publishers { get; set; }
		public DbSet<Developer> Developers { get; set; }
		public DbSet<Copy> Copies { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<SoldCopy> SoldCopies { get; set; }
		public DbSet<ImageType> ImageTypes { get; set; }
	}
}