namespace ComponentSelector.Domain.Exceptions;

public class ItemNotFoundException(string message) : Exception(message)
{
}
