using Data.Models;

namespace Data
{
    public static class Data
    {
        public static IEnumerable<User> InitializeData()
        {
            var users = new List<User>
            {
                new ("Xavier", new List<Account>
                {
                    new (00001, 1000),
                    new (00002, 100)
                }),
                new ("Chris", new List<Account>
                {
                    new (00010, 99),
                }),

            };
            return users;
        }
    }
}
