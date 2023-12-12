namespace Infrastructure.Exceptions
{
    public class ZeroOrNegativeAmountException(string message) : Exception(message)
    {
    }
}
