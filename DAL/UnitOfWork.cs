using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Domain.Models;
using Domain;

namespace DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly AppDbContext _context = new AppDbContext();
        private IGenericRepository<User>? _userRepository;
        private IGenericRepository<Account>? _accountRepository;

        public UnitOfWork()
        {
            var user1 = new User("Xavier");
            UserRepository.Insert(user1);
            var user2 = new User("Chris");
            UserRepository.Insert(user2);

            AccountRepository.Insert(new Account(1000, user1.Id));
            AccountRepository.Insert(new Account(100, user1.Id));
            AccountRepository.Insert(new Account(200, user2.Id));

            Save();
        }

        public IGenericRepository<User> UserRepository
        {
            get
            {
                this._userRepository ??= new GenericRepository<User>(_context);
                return _userRepository;
            }
        }

        public IGenericRepository<Account> AccountRepository
        {
            get
            {
                this._accountRepository ??= new GenericRepository<Account>(_context);
                return _accountRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
