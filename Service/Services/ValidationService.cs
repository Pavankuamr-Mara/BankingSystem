using Data.Models;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class ValidationService : IValidationService
    {
        private const int MIN_BALANCE_AMOUNT = 100;
        private const int MAX_DEPOSIT_AMOUNT = 10000;
        private const int MAX_WITHDRAW_AMOUNT_PERCENTAGE = 90;

        public Account EnsureAccountExist(IQueryable<Account> domainAccounts, Guid find)
        {
            return domainAccounts.SingleOrDefault(a => a.Id == find)
                ?? throw new RecordNotFoundException($"Account Id '{find}' not found.");
        }

        public User EnsureUserExist(IQueryable<User> domainUsers, Guid find)
        {
            return domainUsers.SingleOrDefault(u => u.Id == find)
                ?? throw new RecordNotFoundException($"User Id '{find}' not found.");
        }

        public void ValidateDepositAmount(int amount)
        {
            CheckForZeroOrNegativeAmount(amount);
            if (amount > MAX_DEPOSIT_AMOUNT)
            {
                throw new ExceedingDepositLimitException("The maximum deposit amount per transaction is $10,000.");
            }
        }

        public void ValidateWithdrawAmount(Account domainAccount, int amount)
        {
            CheckForZeroOrNegativeAmount(amount);
            if (domainAccount.Balance - amount < MIN_BALANCE_AMOUNT)
            {
                throw new DeceedingBalanceLimitException("The minimum balance amount should be $100 or more.");
            }

            var maxWithdrawal = domainAccount.Balance * MAX_WITHDRAW_AMOUNT_PERCENTAGE / 100;
            if (maxWithdrawal < amount)
            {
                throw new DeceedingBalanceLimitException($"The maximum withdrawal limit ${maxWithdrawal} is reached.");
            }
        }

        private static void CheckForZeroOrNegativeAmount(int amount)
        {
            if (amount <= 0)
            {
                throw new ZeroOrNegativeAmountException("Amount must be non-zero and positive integer value.");
            }
        }


    }
}
