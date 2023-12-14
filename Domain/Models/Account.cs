namespace Domain.Models
{
    public class Account(int balance, Guid userId)
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public int  Balance {  get; private set; } = balance;

        public Guid UserId { get; private set; } = userId;

        public User User { get; private set; }

        public void AddMoney(int amount)
        {
            Balance += amount;
        }

        public void RemoveMoney(int amount)
        {
            Balance -= amount;
        }

        public void AttachUser(User user)
        {
            User = user;
        }
    }

}
