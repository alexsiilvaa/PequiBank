namespace PequiBank.Domain.Model;

using PequiBank.Domain.Exceptions;
using System.Text.Json.Serialization;

public sealed class FinancialTransaction
{
    [JsonConstructor]
    public FinancialTransaction(int customerId, char type, int value, string description, DateTime executedon)
    {
        CustomerId = customerId;
        Type = type;
        Value = value;
        Description = description;
        ExecutedOn = executedon;
    }

    public FinancialTransaction(int customerId, string type, int value, string description)
    {
        if (type is not "c" and not "d")
        {
            throw new InvalidFinancialTransactionTypeException();
        }

        if (value <= 0)
        {
            throw new InvalidValueException("Negative value or value equal to Zero is not allowed.");
        }

        if (description is null or { Length: 0 or > 10 })
        {
            throw new InvalidValueException("Null values or values longer than 10 characters are not allowed.");
        }

        CustomerId = customerId;
        Type = type[0];
        Value = value;
        Description = description;
    }

    public int CustomerId { get; private set; }
    public char Type { get; private set; }
    public int Value { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset? ExecutedOn { get; private set; }
}