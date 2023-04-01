using DAL.Entity;
using DAL.Entity.BillingAddresses;
using DAL.Entity.Copies;
using DAL.Entity.Developers;
using DAL.Entity.Games;
using DAL.Entity.GameType;
using DAL.Entity.Genres;
using DAL.Entity.Images;
using DAL.Entity.Mails;
using DAL.Entity.Orders;
using DAL.Entity.Platforms;
using DAL.Entity.Publishers;
using DAL.Entity.Regions;
using DAL.Entity.SoldCopies;
using DAL.Entity.Tags;
using DAL.Repositories;

namespace UnitsOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
	{
		IRepository<Order> Orders { get; }
		IRepository<User> Users { get; }
		IRepository<Game> Games { get; }
		IRepository<Tag> Tags{ get; }
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

		void DeleteDB();
	}
}
