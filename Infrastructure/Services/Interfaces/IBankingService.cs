using Infrastructure.Dtos;

namespace Infrastructure.Services.Interfaces
{
    public interface IBankingService
    {
        public Task<AccountDetailsResponseDto> GetBalanceAsync(Guid accountId);

        public Task AddMoneyAsync(AddMoneyRequestDto requestDto);

        public Task RemoveMoneyAsync(RemoveMoneyRequestDto requestDto);
    }
}
