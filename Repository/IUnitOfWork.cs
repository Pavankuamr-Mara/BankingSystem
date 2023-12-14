using DAL.Repositories.Interfaces;
using Domain.Models;

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
