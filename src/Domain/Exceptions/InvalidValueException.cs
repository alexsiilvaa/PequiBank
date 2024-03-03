namespace PequiBank.Domain.Exceptions;

public sealed class InvalidValueException(string message) : Exception(message)
{
}