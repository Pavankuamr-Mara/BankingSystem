using DAL;
using Domain.Models;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Moq;

namespace InfrastructureTests.Services
{
    public class BankingServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IValidationService> _validationService;
        private readonly BankingService _service;

        public BankingServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _validationService = new Mock<IValidationService>(MockBehavior.Strict);
            _service = new BankingService(_mockUnitOfWork.Object, _validationService.Object);
        }

        [Fact]
        public async Task GetBalanceAsync_Should_Return_Account_Balance()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var checkBalanceAccountId = accounts[0].Id;
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), checkBalanceAccountId)).Returns(accounts[0]);

            //Act
            var result = await _service.GetBalanceAsync(checkBalanceAccountId);

            //Assert
            Assert.Equal(result.AccountNumber, accounts[0].Id);
            Assert.Equal(result.Balance, accounts[0].Balance);
        }

        [Fact]
        public async Task GetBalanceAsync_Should_Throw_Exception_When_EnsureAccountExist_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var checkBalanceAccountId = accounts[0].Id;
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), checkBalanceAccountId)).Throws<Exception>();

            //Act
            Task act() => _service.GetBalanceAsync(checkBalanceAccountId);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public void AddMoneyAsync_Should_Add_Money()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new AddMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            var initialAccountBalance = accounts[0].Balance;
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Returns(accounts[0]);
            _validationService.Setup(v => v.ValidateDepositAmount(requestDto.Amount));
            _mockUnitOfWork.Setup(u => u.AccountRepository.Update(It.IsAny<Account>()));
            _mockUnitOfWork.Setup(u => u.Save());

            //Act
            var result = _service.AddMoneyAsync(requestDto);

            //Assert
            Assert.True(result.IsCompleted);
            Assert.Equal(accounts[0].Balance, initialAccountBalance + requestDto.Amount);
        }

        [Fact]
        public async Task AddMoneyAsync_Should_Throw_Exception_When_ValidateDepositAmount_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new AddMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Throws<Exception>();

            //Act
            Task act() => _service.AddMoneyAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task AddMoneyAsync_Should_Throw_Exception_When_EnsureAccountExist_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new AddMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Returns(accounts[0]);
            _validationService.Setup(v => v.ValidateDepositAmount(requestDto.Amount)).Throws<Exception>();

            //Act
            Task act() => _service.AddMoneyAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }
        [Fact]
        public void RemoveMoneyAsync_Should_Remove_Money()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new RemoveMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            var initialAccountBalance = accounts[0].Balance;
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Returns(accounts[0]);
            _validationService.Setup(v => v.ValidateWithdrawAmount(accounts[0], requestDto.Amount));
            _mockUnitOfWork.Setup(u => u.AccountRepository.Update(It.IsAny<Account>()));
            _mockUnitOfWork.Setup(u => u.Save());

            //Act
            var result = _service.RemoveMoneyAsync(requestDto);

            //Assert
            Assert.True(result.IsCompleted);
            Assert.Equal(accounts[0].Balance, initialAccountBalance - requestDto.Amount);
        }

        [Fact]
        public async Task RemoveMoneyAsync_Should_Throw_Exception_When_ValidateDepositAmount_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new RemoveMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Throws<Exception>();

            //Act
            Task act() => _service.RemoveMoneyAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public async Task RemoveMoneyAsync_Should_Throw_Exception_When_EnsureAccountExist_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var requestDto = new RemoveMoneyRequestDto()
            {
                AccountId = accounts[0].Id,
                Amount = 100,
            };
            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), requestDto.AccountId)).Returns(accounts[0]);
            _validationService.Setup(v => v.ValidateWithdrawAmount(accounts[0], requestDto.Amount)).Throws<Exception>();

            //Act
            Task act() => _service.RemoveMoneyAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }
    }
}
