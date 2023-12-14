namespace DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<IQueryable<TEntity>> GetAllAsync();

        public void Insert(TEntity entity);

        public void Delete(TEntity entityToDelete);

        public void Update(TEntity entityToUpdate);

    }
}
