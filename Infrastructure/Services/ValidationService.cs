using Domain.Models;
using Infrastructure.AppSettingsModel;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class ValidationService(IOptions<ValidationSettings> options) : IValidationService
    {
        private const string ERRROR_ACCOUNT_ID_NOT_FOUND = "Account Id not found.";
        private const string ERRROR__USER_NOT_FOUND = "User Id not found.";
        private const string ERRROR_MAX_DEPOSIT_REACHED = "The maximum deposit amount per transaction is $10,000.";
        private const string ERRROR_MINIMUM_BALANCE = "The minimum balance amount should be $100 or more.";
        private const string ERRROR_WITHDRAWAL_LIMIT_REACHED = "The maximum withdrawal limit is reached.";
        private const string ERRROR_NEGATIVE_VALUE = "Amount must be non-zero and positive integer value.";

        private readonly ValidationSettings _validationSettings = options.Value;

        public Account EnsureAccountExist(IQueryable<Account> domainAccounts, Guid find)
        {
            return domainAccounts.SingleOrDefault(a => a.Id == find)
                ?? throw new RecordNotFoundException(ERRROR_ACCOUNT_ID_NOT_FOUND);
        }

        public User EnsureUserExist(IQueryable<User> domainUsers, Guid find)
        {
            return domainUsers.SingleOrDefault(u => u.Id == find)
                ?? throw new RecordNotFoundException(ERRROR__USER_NOT_FOUND);
        }

        public void ValidateDepositAmount(int amount)
        {
            CheckForZeroOrNegativeAmount(amount);
            if (amount > _validationSettings.MaxDeposit)
            {
                throw new ExceedingDepositLimitException(ERRROR_MAX_DEPOSIT_REACHED);
            }
        }

        public void ValidateWithdrawAmount(Account domainAccount, int amount)
        {
            CheckForZeroOrNegativeAmount(amount);
            if (domainAccount.Balance - amount < _validationSettings.MinimunBalance)
            {
                throw new DeceedingBalanceLimitException(ERRROR_MINIMUM_BALANCE);
            }

            var maxWithdrawal = GetMaximumWithdrawalLimit(domainAccount.Balance);
            if (maxWithdrawal < amount)
            {
                throw new DeceedingBalanceLimitException(ERRROR_WITHDRAWAL_LIMIT_REACHED);
            }
        }

        private static void CheckForZeroOrNegativeAmount(int amount)
        {
            if (amount <= 0)
            {
                throw new ZeroOrNegativeAmountException(ERRROR_NEGATIVE_VALUE);
            }
        }

        private int GetMaximumWithdrawalLimit(int balance)
        {
            return balance * _validationSettings.MaximumWithdrawalPercentage / 100;
        }
    }
}
