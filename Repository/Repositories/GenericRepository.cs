using DAL.Repositories.Interfaces;
using EFCoreInMemoryDbDemo;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity>(AppDbContext context) : IGenericRepository<TEntity> where TEntity : class
    {
        internal AppDbContext context = context;
        internal DbSet<TEntity> dbSet = context.Set<TEntity>();

        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(dbSet.ToList().AsQueryable());
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
