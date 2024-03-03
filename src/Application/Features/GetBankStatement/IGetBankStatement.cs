namespace PequiBank.Application.Features.GetBankStatement;

using System.Threading.Tasks;

public interface IGetBankStatement
{
    ValueTask<ResponseGetBankStatement> ExecuteAsync(int customerId, CancellationToken cancellationToken);
}