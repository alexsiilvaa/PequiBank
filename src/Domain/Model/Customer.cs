namespace PequiBank.Domain.Model;

public sealed class Customer(int id, int creditLimit, int balance)
{
    public int Id { get; private set; } = id;
    public int CreditLimit { get; private set; } = creditLimit;
    public int Balance { get; private set; } = balance;
}