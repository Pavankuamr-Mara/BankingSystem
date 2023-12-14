using Domain.Models;

namespace Infrastructure.Dtos
{
    public class AccountOverviewDto
    {
        public Guid AccountNumber { get; set; }

        public static AccountOverviewDto MapFromDomainAccount(Account domainAccount)
        {
            return new AccountOverviewDto()
            {
                AccountNumber = domainAccount.Id
            };
        }
    }
}
