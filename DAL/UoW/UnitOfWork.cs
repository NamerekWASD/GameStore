using DAL.Context;
using DAL.Entity;
using DAL.Entity.Cheques;
using DAL.Entity.Games;
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

		private IRepository<Cheque> _cheques;
		private IRepository<Game> _games;
		private IRepository<Genre> _genres;
		private IRepository<User> _users;
		private IRepository<Copy> _copies;
		private IRepository<SoldCopy> _soldCopies;
		public IRepository<Cheque> Cheques
		{
			get
			{
				_cheques ??= new Repository<Cheque>(gameContext);
				return _cheques;
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
