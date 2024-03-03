namespace PequiBank.Infrastructure.Repository;

using Dapper;
using PequiBank.Application.Repository;
using PequiBank.Domain.Exceptions;
using PequiBank.Infrastructure.Data;

public sealed class CustomerRepository
    (
        IDatabase database
    ) : ICustomerRepository
{
    public async ValueTask<int> GetCreditLimitByIdAsync(int customerId, CancellationToken cancellationToken)
    {
        const string SQL =
            """
            SELECT
                CreditLimit
            FROM Customer
            WHERE Id = @Id
            """;

        using var connection = await database.CreateDbConnectionAsync();

        return await connection
            .QueryFirstOrDefaultAsync<int?>(new CommandDefinition(SQL, new { Id = customerId },
                cancellationToken: cancellationToken)) ?? throw new CustomerNotFoundException();
    }
}