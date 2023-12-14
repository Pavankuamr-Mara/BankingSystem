namespace Domain.Models
{
    public class User(string name)
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Name { get; init; } = name;

        public ICollection<Account> Accounts { get; private set; } = new List<Account>();

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

    }
}
