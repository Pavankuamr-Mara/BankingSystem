namespace Infrastructure.Dtos
{
    public abstract class TransactionRequestDto
    {
        public Guid AccountId { get; set; }
        public int Amount {  get; set; }
    }
}
