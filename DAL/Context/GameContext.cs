using DAL.Entity;
using DAL.Entity.Cheques;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
		public DbSet<Platform> Platforms { get; set; }
		public DbSet<Developer> Publishers { get; set; }
		public DbSet<Copy> Copies{ get; set; }
		public DbSet<Cheque> Cheques{ get; set; }
		public DbSet<Genre> Genres { get; set; }

	}
}