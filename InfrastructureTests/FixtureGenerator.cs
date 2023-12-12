using Data.Models;

namespace InfrastructureTests
{
    internal static class FixtureGenerator
    {
        internal static List<User> GetTestUsersWithAccounts()
        {
            var users = new List<User>
            {
                new ("Xavier"),
                new ("Chris")
            };

            var account1 = new Account(1000, users[0].Id);
            var account2 = new Account(100, users[0].Id);
            var account3 = new Account(200, users[1].Id);

            users[0].AddAccount(account1);
            users[0].AddAccount(account2);
            users[1].AddAccount(account3);

            return users;
        }
    }
}
