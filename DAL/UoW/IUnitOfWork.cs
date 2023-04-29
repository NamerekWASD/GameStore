using DAL.Entity;
using DAL.Repositories;

namespace UnitsOfWork.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Order> Orders { get; }
		IRepository<User> Users { get; }
		IRepository<Game> Games { get; }
		IRepository<Tag> Tags { get; }
		IRepository<Genre> Genres { get; }
		IRepository<Copy> Copies { get; }
		IRepository<SoldCopy> SoldCopies { get; }
		IRepository<GameSubscription> GameSubscriptions { get; }
		IRepository<BillingAddress> BillingAddresses { get; }
		IRepository<Image> Images { get; }
		IRepository<CopyType> CopyTypes { get; }
		IRepository<Platform> Platforms { get; }
		IRepository<Developer> Developers { get; }
		IRepository<Publisher> Publishers { get; }
		IRepository<Region> Regions { get; }
		IRepository<ImageType> ImageTypes { get; }

        Task CommitChanges();
        void DeleteDB();
	}
}