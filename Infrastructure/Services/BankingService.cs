using DAL;
using Infrastructure.Dtos;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class BankingService(IUnitOfWork unitOfWork, IValidationService validationService) : IBankingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidationService _validationService = validationService;

        public async Task<AccountDetailsResponseDto> GetBalanceAsync(Guid accountId)
        {
            var domainAccounts = await _unitOfWork.AccountRepository.GetAllAsync();
            var viewAccount = _validationService.EnsureAccountExist(domainAccounts, accountId);
            return AccountDetailsResponseDto.MapFromDomainAccount(viewAccount);
        }

        public async Task AddMoneyAsync(AddMoneyRequestDto requestDto)
        {
            var domainAccounts = await _unitOfWork.AccountRepository.GetAllAsync();
            var account = _validationService.EnsureAccountExist(domainAccounts, requestDto.AccountId);
            _validationService.ValidateDepositAmount(requestDto.Amount);
            account.AddMoney(requestDto.Amount);
            _unitOfWork.AccountRepository.Update(account);
            _unitOfWork.Save();
        }

        public async Task RemoveMoneyAsync(RemoveMoneyRequestDto requestDto)
        {
            var domainAccounts = await _unitOfWork.AccountRepository.GetAllAsync();
            var account = _validationService.EnsureAccountExist(domainAccounts, requestDto.AccountId);
            _validationService.ValidateWithdrawAmount(account, requestDto.Amount);
            account.RemoveMoney(requestDto.Amount);
            _unitOfWork.AccountRepository.Update(account);
            _unitOfWork.Save();
        }
    }
}
