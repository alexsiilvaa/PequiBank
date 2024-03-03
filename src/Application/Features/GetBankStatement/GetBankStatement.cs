namespace PequiBank.Application.Features.GetBankStatement;

using PequiBank.Application.Repository;
using System.Threading.Tasks;

public sealed class GetBankStatement
    (
        IFinancialTransactionRepository financialTransactionRepository
    ) : IGetBankStatement
{
    public async ValueTask<ResponseGetBankStatement> ExecuteAsync(int customerId, CancellationToken cancellationToken)
    {
        var (customer, transactions) = await financialTransactionRepository.BankStatementAsync(customerId, cancellationToken);

        return new
        (
            Customer: new(customer.Balance, customer.CreditLimit),
            FinancialTransactions: transactions.ConvertAll(t => new FinancialTransaction(t.Value, t.Type, t.Description, t.ExecutedOn.GetValueOrDefault()))
        );
    }
}