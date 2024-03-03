namespace PequiBank.Application.Features.GetBankStatement;

public record FinancialTransaction(int Value, char Type, string Description, DateTimeOffset ExecutedOn);
public record Customer(int Balance, int CreditLimit);
public record ResponseGetBankStatement(Customer Customer, List<FinancialTransaction> FinancialTransactions);