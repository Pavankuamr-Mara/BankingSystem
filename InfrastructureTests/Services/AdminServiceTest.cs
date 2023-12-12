using DAL;
using Data.Models;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Moq;

namespace InfrastructureTests.Services
{

    public class AdminServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IValidationService> _validationService;
        private readonly AdminService _service;

        public AdminServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _validationService = new Mock<IValidationService>(MockBehavior.Strict);
            _service = new AdminService(_mockUnitOfWork.Object, _validationService.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_All_Users_With_Account_Overview()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            _mockUnitOfWork.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(users.AsQueryable());

            //Act
            var result = (await _service.GetAllUsersAsync()).ToList();

            //Assert
            Assert.Equal(result.Count, users.Count);
            Assert.Equal(result[0].Id, users[0].Id);
            Assert.Equal(result[0].Username, users[0].Name);
            Assert.Equal(result[0].AccountNumbers, users[0].Accounts.Select(a => a.Id).ToArray());
            Assert.Equal(result[1].Id, users[1].Id);
            Assert.Equal(result[1].Username, users[1].Name);
            Assert.Equal(result[1].AccountNumbers, users[1].Accounts.Select(a => a.Id).ToArray());
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_Empty_List_When_There_Are_No_Users()
        {
            //Arrange
            var users = new List<User>();
            _mockUnitOfWork.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(users.AsQueryable());

            //Act
            var result = (await _service.GetAllUsersAsync()).ToList();

            //Assert
            Assert.Equal(result.Count, users.Count);
        }

        [Fact]
        public async Task AddAccountAsync_Should_Add_New_Account()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var requestDto = new AddAccountRequestDto()
            {
                UserId = users[0].Id,
            };
            _mockUnitOfWork.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(users.AsQueryable());
            _validationService.Setup(v => v.EnsureUserExist(users.AsQueryable(), requestDto.UserId)).Returns(users[0]);
            _mockUnitOfWork.Setup(u => u.AccountRepository.Insert(It.IsAny<Account>()));
            _mockUnitOfWork.Setup(u => u.Save());
            //Act
            var result = await _service.AddAccountAsync(requestDto);

            //Assert
            result.AccountNumber = users[0].Accounts.Last().Id;
        }

        [Fact]
        public async Task AddAccountAsync_Should_Throw_Exception_When_EnsureUserExist_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var requestDto = new AddAccountRequestDto()
            {
                UserId = users[0].Id,
            };
            _mockUnitOfWork.Setup(u => u.UserRepository.GetAllAsync()).ReturnsAsync(users.AsQueryable());
            _validationService.Setup(v => v.EnsureUserExist(users.AsQueryable(), requestDto.UserId)).Throws<Exception>();

            //Act
            Task act() => _service.AddAccountAsync(requestDto);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        [Fact]
        public void RemoveAccountAsync_Should_Delete_Account()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var deleteAccountId = accounts[0].Id;

            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), deleteAccountId)).Returns(accounts[0]);
            _mockUnitOfWork.Setup(u => u.AccountRepository.Delete(accounts[0]));
            _mockUnitOfWork.Setup(u => u.Save());

            //Act
            var result = _service.RemoveAccountAsync(deleteAccountId);

            //Assert
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public async Task RemoveAccountAsync_Throw_Exception_When_EnsureAccountExist_Fails()
        {
            //Arrange
            var users = FixtureGenerator.GetTestUsersWithAccounts();
            var accounts = users.SelectMany(u => u.Accounts).ToList();
            var deleteAccountId = accounts[0].Id;

            _mockUnitOfWork.Setup(u => u.AccountRepository.GetAllAsync()).ReturnsAsync(accounts.AsQueryable());
            _validationService.Setup(v => v.EnsureAccountExist(accounts.AsQueryable(), deleteAccountId)).Throws<Exception>();

            //Act
            Task act() => _service.RemoveAccountAsync(deleteAccountId);

            //Assert
            await Assert.ThrowsAsync<Exception>(act);
        }
    }
}
