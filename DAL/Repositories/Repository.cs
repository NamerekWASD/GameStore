using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DAL.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly DbContext Context;
		private readonly DbSet<TEntity> DbSet;
		public Repository(DbContext Context)
		{
			this.Context = Context;
			DbSet = Context.Set<TEntity>();
		}

		public async Task ClearAsync()
		{
			DbSet.RemoveRange(DbSet);
			await Context.SaveChangesAsync();
		}
		public async Task DeleteAsync(TEntity item)
		{
			DbSet.Remove(item);
			await Context.SaveChangesAsync();
		}
		public async Task RemoveRangeAsync(params TEntity[] entities)
		{
			DbSet.RemoveRange(entities);
			await Context.SaveChangesAsync();
		}
		public async Task<TEntity> AddAsync(TEntity item)
		{
			var entity = await DbSet.AddAsync(item);
			await Context.SaveChangesAsync();
			return entity.Entity;
		}
		public async Task<TEntity> ModifyAsync(TEntity item)
		{
			Context.Entry(item).State = EntityState.Modified;
			await Context.SaveChangesAsync();
			return item;
		}
		public async Task<TEntity?> GetAsync(int Id)
		{
			return await DbSet.FindAsync(Id);
		}
		public async Task<IQueryable<TEntity>> GetAllAsync()
		{
			return (await DbSet.ToListAsync()).AsQueryable();
		}
		public async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
		{
			return (await DbSet.ToListAsync(cancellationToken: cancellationToken)).AsQueryable();
		}
		public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return (await DbSet.Where(predicate).ToListAsync()).AsQueryable();
		}
		public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		{
			return (await DbSet.Where(predicate).ToListAsync(cancellationToken: cancellationToken)).AsQueryable();
		}

		public async IAsyncEnumerable<TEntity> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			foreach(var entity in await DbSet.ToListAsync(cancellationToken: cancellationToken))
			{
				yield return entity;
			}
		}
		public async IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			foreach (var entity in await DbSet.Where(predicate).ToListAsync(cancellationToken: cancellationToken))
			{
				yield return entity;
			}
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
