﻿using System.Linq.Expressions;

namespace DAL.Repositories
{
	public interface IRepository<TEntity> : IDisposable
	{
		Task ClearAsync();

		Task DeleteAsync(TEntity entity);

		Task RemoveRangeAsync(params TEntity[] entities);

		Task<TEntity> AddAsync(TEntity entity);

		Task<TEntity> AddAsyncNoSave(TEntity item);

        Task<TEntity> ModifyAsync(TEntity newItem);

        TEntity ModifyNoSave(TEntity item);

        Task<TEntity?> GetAsync(int Id);

		Task<IQueryable<TEntity>> GetAllAsync();

		Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

		Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

		Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

		IAsyncEnumerable<TEntity> GetAll(CancellationToken cancellationToken);

		IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }
}