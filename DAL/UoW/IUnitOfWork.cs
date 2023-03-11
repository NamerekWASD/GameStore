using DAL.Entity;
using DAL.Entity.Cheques;
using DAL.Entity.Games;
using DAL.Repositories;

namespace UnitsOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
	{
		public IRepository<Cheque> Cheques { get; }
		public IRepository<User> Users { get; }
		public IRepository<Game> Games { get; }
		public IRepository<Genre> Genres { get; }
		public IRepository<Copy> Copies { get; }
		public IRepository<SoldCopy> SoldCopies { get; }

		void DeleteDB();
	}
}
