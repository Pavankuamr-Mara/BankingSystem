using Domain.Models;

namespace Infrastructure.Dtos
{
    public class AccountDetailsResponseDto : AccountOverviewDto
    {
        public int Balance { get; set; }

        public new static AccountDetailsResponseDto MapFromDomainAccount(Account domainAccount)
        {
            return new AccountDetailsResponseDto()
            {
                AccountNumber = domainAccount.Id, 
                Balance = domainAccount.Balance
            };
        }
    }
}
