using System.Linq.Expressions;

namespace DAL.Repositories
{
	public interface IRepository<TEntity> : IDisposable
	{
		Task ClearAsync();
		Task DeleteAsync(int Id);
		Task DeleteAsync(TEntity Entity);
		Task<TEntity> AddAsync(TEntity Entity);
		Task<TEntity> ModifyAsync(int Id, TEntity NewItem);
		Task<TEntity> GetAsync(int Id);
		IAsyncEnumerable<TEntity> GetAll();
		IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
	}
}
