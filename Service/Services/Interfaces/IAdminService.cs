using Infrastructure.Dtos;

namespace Infrastructure.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        public Task<AccountOverviewDto> AddAccountAsync(AddAccountRequestDto requestDto);

        public Task RemoveAccountAsync(Guid accountId);
    }
}
