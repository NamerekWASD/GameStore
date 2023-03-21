using DAL.Entity;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Context
{
	public class GameContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public GameContext() : base()
		{
			Database.EnsureCreated();
		}
		public GameContext(DbContextOptions<GameContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
		public DbSet<Game> Games { get; set; }
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
	}
}