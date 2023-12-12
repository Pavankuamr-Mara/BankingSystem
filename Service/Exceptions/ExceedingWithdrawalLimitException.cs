namespace Infrastructure.Exceptions
{
    public class ExceedingWithdrawalLimitException(string message) : Exception(message)
    {
    }
}
