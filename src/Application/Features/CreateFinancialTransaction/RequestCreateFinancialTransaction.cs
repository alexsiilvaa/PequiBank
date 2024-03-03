namespace PequiBank.Application.Features.CreateFinancialTransaction;

public record RequestCreateFinancialTransaction(int CustomerId, int Value, string Type, string Description);