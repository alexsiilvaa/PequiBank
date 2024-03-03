namespace PequiBank.Application.Repository;

using PequiBank.Domain.Model;
using System.Threading.Tasks;

public interface IFinancialTransactionRepository
{
    Task<int> UpdateBalanceAsync(FinancialTransaction financialTransaction, CancellationToken cancellationToken);

    Task CreateAsync(FinancialTransaction financialTransaction, CancellationToken cancellationToken);

    ValueTask<(Customer Customer, List<FinancialTransaction> Transactions)> BankStatementAsync(int customerId, CancellationToken cancellationToken);
}