namespace Infrastructure.Exceptions
{
    public class ExceedingDepositLimitException(string message) : Exception(message)
    {
    }
}
