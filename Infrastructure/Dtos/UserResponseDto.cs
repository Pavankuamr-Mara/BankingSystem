using Domain.Models;

namespace Infrastructure.Dtos
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public Guid[] AccountNumbers { get; set; } = [];

        public static UserResponseDto MapFromDomainUser(User domainUser)
        {
            return new UserResponseDto()
            {
                Id = domainUser.Id,
                Username = domainUser.Name,
                AccountNumbers = domainUser.Accounts.Select(x => x.Id).ToArray()
            };
        }
    }
}
