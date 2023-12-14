using Domain.Models;

namespace Infrastructure.Services.Interfaces
{
    public interface IValidationService
    {
        public Account EnsureAccountExist(IQueryable<Account> domainAccounts, Guid find);

        public User EnsureUserExist(IQueryable<User> domainUsers, Guid find);

        public void ValidateDepositAmount(int amount);

        public void ValidateWithdrawAmount(Account domainAccount, int amount);
    }
}
