using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
		public async Task DeleteAsync(int Id)
		{
			DbSet.Remove(await DbSet.FindAsync(Id));
			await Context.SaveChangesAsync();
		}
		public async Task DeleteAsync(TEntity Item)
		{
			DbSet.Entry(Item).State = EntityState.Deleted;
			await Context.SaveChangesAsync();
		}
		public async Task RemoveRangeAsync(params TEntity[] entities)
		{
			DbSet.RemoveRange(entities);
			await Context.SaveChangesAsync();
		}
		public async Task<TEntity> AddAsync(TEntity Item)
		{
			var entity = await DbSet.AddAsync(Item);
			await Context.SaveChangesAsync();
			return entity.Entity;
		}
		public async Task<TEntity> ModifyAsync(int Id, TEntity Item)
		{
			Context.Entry<TEntity>(await Context.Set<TEntity>().FindAsync(Id)).CurrentValues.SetValues(Item);
			await Context.SaveChangesAsync();
			return await DbSet.FindAsync(Id);
		}
		public async Task<TEntity> GetAsync(int Id)
		{
			return await DbSet.FindAsync(Id);
		}
		public async IAsyncEnumerable<TEntity> GetAll()
		{
			foreach(var entity in await DbSet.ToListAsync())
			{
				yield return entity;
			}
		}
		public async IAsyncEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
		{
			foreach (var entity in await DbSet.Where(predicate).ToListAsync())
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
