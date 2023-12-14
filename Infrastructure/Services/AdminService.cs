using DAL;
using Domain.Models;
using Infrastructure.Dtos;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Services
{
    public class AdminService(IUnitOfWork unitOfWork, IValidationService validationService) : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidationService _validationService = validationService;

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var domainUsers = (await _unitOfWork.UserRepository.GetAllAsync())
                .Include(u => u.Accounts)
                .ToList();
            return domainUsers.Select(UserResponseDto.MapFromDomainUser);
        }

        public async Task<AccountOverviewDto> AddAccountAsync(AddAccountRequestDto requestDto)
        {
            var domainUsers = (await _unitOfWork.UserRepository.GetAllAsync()).Include(u => u.Accounts);
            var user = _validationService.EnsureUserExist(domainUsers, requestDto.UserId);
            var account = new Account(0, user.Id);
            _unitOfWork.AccountRepository.Insert(account);
            _unitOfWork.Save();
            return AccountOverviewDto.MapFromDomainAccount(account);
        }

        public async Task RemoveAccountAsync(Guid accountId)
        {
            var domainAccounts = await _unitOfWork.AccountRepository.GetAllAsync();
            var accountToDelete = _validationService.EnsureAccountExist(domainAccounts, accountId);
            _unitOfWork.AccountRepository.Delete(accountToDelete);
            _unitOfWork.Save();
        }
    }
}
