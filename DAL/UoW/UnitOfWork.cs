using DAL.Context;
using DAL.Entity;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Orders;
using DAL.Repositories;
using UnitsOfWork.Interfaces;

namespace DAL.UoW
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly GameContext gameContext;

		public UnitOfWork()
		{
			gameContext = new GameContext();
		}

		public UnitOfWork(GameContext gameContext)
		{
			this.gameContext = gameContext;
		}
		private IRepository<Order> _orders;
		private IRepository<Game> _games;
		private IRepository<Genre> _genres;
		private IRepository<User> _users;
		private IRepository<Copy> _copies;
		private IRepository<SoldCopy> _soldCopies;
		private IRepository<GameSubscription> _gameSubscriptions;
		private IRepository<BillingAddress> _billingAddresses;
		private IRepository<Image> _images;
		private IRepository<CopyType> _copyTypes;
		private IRepository<Platform> _platforms;
		private IRepository<Developer> _developers;
		private IRepository<Publisher> _publishers;
		private IRepository<Region> _regions;
		public IRepository<Order> Orders
		{
			get
			{
				_orders ??= new Repository<Order>(gameContext);
				return _orders;
			}
		}

		public IRepository<Game> Games
		{
			get
			{
				_games ??= new Repository<Game>(gameContext);
				return _games;
			}
		}
		public IRepository<Genre> Genres
		{
			get
			{
				_genres ??= new Repository<Genre>(gameContext);
				return _genres;
			}
		}
		public IRepository<User> Users
		{
			get
			{
				_users ??= new Repository<User>(gameContext);
				return _users;
			}
		}
		public IRepository<Copy> Copies
		{
			get
			{
				_copies ??= new Repository<Copy>(gameContext);
				return _copies;
			}
		}
		public IRepository<SoldCopy> SoldCopies
		{
			get
			{
				_soldCopies ??= new Repository<SoldCopy>(gameContext);
				return _soldCopies;
			}
		}
		public IRepository<GameSubscription> GameSubscriptions
		{
			get
			{
				_gameSubscriptions ??= new Repository<GameSubscription>(gameContext);
				return _gameSubscriptions;
			}
		}
		public IRepository<BillingAddress> BillingAddresses
		{
			get
			{
				_billingAddresses ??= new Repository<BillingAddress>(gameContext);
				return _billingAddresses;
			}
		}

		public IRepository<Image> Images
		{
			get
			{
				_images ??= new Repository<Image>(gameContext);
				return _images;
			}
		}
		public IRepository<CopyType> CopyTypes
		{
			get
			{
				_copyTypes ??= new Repository<CopyType>(gameContext);
				return _copyTypes;
			}
		}
		public IRepository<Platform> Platforms
		{
			get
			{
				_platforms ??= new Repository<Platform>(gameContext);
				return _platforms;
			}
		}
		public IRepository<Developer> Developers
		{
			get
			{
				_developers ??= new Repository<Developer>(gameContext);
				return _developers;
			}
		}
		public IRepository<Publisher> Publishers
		{
			get
			{
				_publishers ??= new Repository<Publisher>(gameContext);
				return _publishers;
			}
		}
		public IRepository<Region> Regions
		{
			get
			{
				_regions ??= new Repository<Region>(gameContext);
				return _regions;
			}
		}

		public void DeleteDB()
		{
			gameContext.Database.EnsureDeleted();
			gameContext.Database.EnsureCreated();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					gameContext.Dispose();
				}
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
