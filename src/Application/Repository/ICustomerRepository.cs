namespace PequiBank.Application.Repository;

using System.Threading.Tasks;

public interface ICustomerRepository
{
    ValueTask<int> GetCreditLimitByIdAsync(int customerId, CancellationToken cancellationToken);
}