namespace PequiBank.Application.Features.CreateFinancialTransaction;

using System.Threading.Tasks;

public interface ICreateFinancialTransaction
{
    Task<ResponseCreateFinancialTransaction> ExecuteAsync(RequestCreateFinancialTransaction request, CancellationToken cancellationToken);
}