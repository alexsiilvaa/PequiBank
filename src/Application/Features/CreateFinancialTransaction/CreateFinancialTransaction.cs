namespace PequiBank.Application.Features.CreateFinancialTransaction;

using PequiBank.Application.Repository;
using PequiBank.Domain.Model;
using System.Threading.Channels;
using System.Threading.Tasks;

public sealed class CreateFinancialTransaction
    (
        ICustomerRepository customerRepository,
        IFinancialTransactionRepository financialTransactionRepository,
        ChannelWriter<FinancialTransaction> channelWriter
    ) : ICreateFinancialTransaction
{
    public async Task<ResponseCreateFinancialTransaction> ExecuteAsync(RequestCreateFinancialTransaction request, CancellationToken cancellationToken)
    {
        var financialTransaction = new FinancialTransaction(request.CustomerId, request.Type, request.Value, request.Description);
        var creditLimit = await customerRepository.GetCreditLimitByIdAsync(request.CustomerId, cancellationToken);
        var newBalance = await financialTransactionRepository.UpdateBalanceAsync(financialTransaction, cancellationToken);
        await channelWriter.WriteAsync(financialTransaction, cancellationToken);

        return new(creditLimit, newBalance);
    }
}