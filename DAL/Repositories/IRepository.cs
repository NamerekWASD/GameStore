using DAL.Entity.Games;
using System.Linq.Expressions;

namespace DAL.Repositories
{
	public interface IRepository<TEntity> : IDisposable
	{
		Task ClearAsync();
		Task DeleteAsync(int Id);
		Task DeleteAsync(TEntity entity);
		Task RemoveRangeAsync(params TEntity[] entities);
		Task<TEntity> AddAsync(TEntity entity);
		Task<TEntity> ModifyAsync(int Id, TEntity newItem);
		Task<TEntity> GetAsync(int Id);
		IAsyncEnumerable<TEntity> GetAll();
		IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
	}
}
