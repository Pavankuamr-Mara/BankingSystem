using DAL.Repositories.Interfaces;
using Data.Models;

namespace DAL
{
    public interface IUnitOfWork
    {
        public IGenericRepository<User> UserRepository { get; }

        public IGenericRepository<Account> AccountRepository { get; }

        public void Save();

        public void Dispose();
    }
}
