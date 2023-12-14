using Domain.Models;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.Services;

namespace InfrastructureTests.Services
{
    public class ValidationServiceTest
    {
        private readonly ValidationService _service = new ValidationService();

        [Fact]
        public void EnsureUserExist_Should_Return_User_When_Matching_User_Record_Found()
        {
            //Arrange
            var users = new List<User>
            {
                new ("Xavier"),
                new ("Chris")
            }.AsQueryable();

            //Act
            var result = _service.EnsureUserExist(users, users.First().Id);

            //Assert
            result.Should().BeEquivalentTo(users.First());
        }

        [Fact]
        public void EnsureUserExist_Should_Throw_Exception_When_Matching_User_Record_Not_Found()
        {
            //Arrange
            var users = new List<User>
            {
                new ("Xavier"),
                new ("Chris"),
            }.AsQueryable();
            var find = Guid.NewGuid();

            //Act
            void act() => _service.EnsureUserExist(users, find);

            //Assert
            var exception = Assert.Throws<RecordNotFoundException>(act);
            Assert.Equal($"User Id '{find}' not found.", exception.Message);
        }

        [Fact]
        public void EnsureUserExist_Should_Throw_Exception_When_User_Records_Are_0()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var find = Guid.NewGuid();

            //Act
            void act() => _service.EnsureUserExist(users, find);

            //Assert
            var exception = Assert.Throws<RecordNotFoundException>(act);
            Assert.Equal($"User Id '{find}' not found.", exception.Message);
        }

        [Fact]
        public void EnsureAccountExist_Should_Return_Account_When_Matching_Account_Record_Found()
        {
            //Arrange
            var accounts = new List<Account>
            {
                new (1000, Guid.NewGuid()),
                new (100, Guid.NewGuid()),
            }.AsQueryable();

            //Act
            var result = _service.EnsureAccountExist(accounts, accounts.First().Id);

            //Assert
            result.Should().BeEquivalentTo(accounts.First());
        }

        [Fact]
        public void EnsureAccountExist_Should_Throw_Exception_When_Matching_Account_Record_Not_Found()
        {
            //Arrange
            var accounts = new List<Account>
            {
                new (1000, Guid.NewGuid()),
                new (100, Guid.NewGuid()),
            }.AsQueryable();
            var find = Guid.NewGuid();

            //Act
            void act() => _service.EnsureAccountExist(accounts, find);

            //Assert
            var exception = Assert.Throws<RecordNotFoundException>(act);
            Assert.Equal($"Account Id '{find}' not found.", exception.Message);
        }

        [Fact]
        public void EnsureAccountExist_Should_Throw_Exception_When_Account_Records_Are_0()
        {
            //Arrange
            var accounts = new List<Account>().AsQueryable();
            var find = Guid.NewGuid();

            //Act
            void act() => _service.EnsureAccountExist(accounts, find);

            //Assert
            var exception = Assert.Throws<RecordNotFoundException>(act);
            Assert.Equal($"Account Id '{find}' not found.", exception.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void ValidateDepositAmount_Should_Accept_Valid_Amount_For_Deposit(int amount)
        {
            //Act & Assert
            _service.ValidateDepositAmount(amount);
        }

        [Theory]
        [InlineData(100001)]
        public void ValidateDepositAmount_Should_throw_Exception_When_Max_Deposit_Amount_Reached(int amount)
        {
            //Act
            void act() => _service.ValidateDepositAmount(amount);

            //Assert
            var exception = Assert.Throws<ExceedingDepositLimitException>(act);
            Assert.Equal("The maximum deposit amount per transaction is $10,000.", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ValidateDepositAmount_Should_throw_Exception_When_Amount_Is_Zero_Or_Non_Positive(int amount)
        {
            //Act
            void act() => _service.ValidateDepositAmount(amount);

            //Assert
            var exception = Assert.Throws<ZeroOrNegativeAmountException>(act);
            Assert.Equal("Amount must be non-zero and positive integer value.", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ValidateWithdrawAmount_Should_throw_Exception_When_Amount_Is_Zero_Or_Non_Positive(int amount)
        {
            //Arrange
            var account = new Account(200, Guid.NewGuid());

            //Act
            void act() => _service.ValidateWithdrawAmount(account, amount);

            //Assert
            var exception = Assert.Throws<ZeroOrNegativeAmountException>(act);
            Assert.Equal("Amount must be non-zero and positive integer value.", exception.Message);
        }

        [Fact]
        public void ValidateWithdrawAmount_Should_throw_Exception_When_Balance_Would_Be_Less_Than_100()
        {
            //Arrange
            var account = new Account(130, Guid.NewGuid());
            var amount = 31;

            //Act
            void act() => _service.ValidateWithdrawAmount(account, amount);

            //Assert
            var exception = Assert.Throws<DeceedingBalanceLimitException>(act);
            Assert.Equal("The minimum balance amount should be $100 or more.", exception.Message);
        }

        [Fact]
        public void ValidateWithdrawAmount_Should_throw_Exception_When_Withdrwal_Amount_More_Than_90_Percent()
        {
            //Arrange
            var account = new Account(10000, Guid.NewGuid());
            var amount = 9001;
            
            //Act
            void act() => _service.ValidateWithdrawAmount(account, amount);

            //Assert
            var exception = Assert.Throws<DeceedingBalanceLimitException>(act);
            var maxWithdrawal = account.Balance * 90 / 100;
            Assert.Equal($"The maximum withdrawal limit ${maxWithdrawal} is reached.", exception.Message);
        }

        [Theory]
        [InlineData(1000, 900)]
        [InlineData(130, 30)]
        public void ValidateWithdrawAmount_Should_Accept_Valid_Amount(int balance, int amount)
        {
            //Arrange
            var account = new Account(balance, Guid.NewGuid());

            //Act & Assert
            _service.ValidateWithdrawAmount(account, amount);
        }
    }
}