namespace PequiBank.Infrastructure.Repository;

using Dapper;
using Microsoft.Extensions.Logging;
using PequiBank.Application.Repository;
using PequiBank.Domain.Exceptions;
using PequiBank.Domain.Model;
using PequiBank.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class FinancialTransactionRepository
    (
        IDatabase database
    ) : IFinancialTransactionRepository
{
    public async Task<int> UpdateBalanceAsync(FinancialTransaction financialTransaction, CancellationToken cancellationToken)
    {
        const string SQL =
            """
            UPDATE Customer
            SET
                Balance = Balance + @TransactionValue
            WHERE Id = @CustomerId
            AND (@Type = 'c' OR CreditLimit >= (Balance + @TransactionValue)*-1)
            RETURNING Balance;
            """;

        var parameters = new
        {
            financialTransaction.CustomerId,
            financialTransaction.Type,
            TransactionValue = financialTransaction.Type is 'c' ? financialTransaction.Value : -financialTransaction.Value
        };

        using var connection = await database.CreateDbConnectionAsync();

        return await connection.QuerySingleOrDefaultAsync<int?>(new CommandDefinition(SQL, parameters, cancellationToken: cancellationToken))
            ?? throw new CreditLimitExceededException();
    }

    public async ValueTask<(Customer Customer, List<FinancialTransaction> Transactions)> BankStatementAsync(int customerId, CancellationToken cancellationToken)
    {
        const string SQL =
            """
            SELECT
                Id,
                CreditLimit,
                Balance
            FROM Customer
            WHERE Id = @CustomerId;

            SELECT
                CustomerId,
                Type,
                Value,
                Description,
                ExecutedOn
            FROM FinancialTransaction
            WHERE CustomerId = @CustomerId
            ORDER BY Id DESC
            LIMIT 10;
            """;

        using var connection = await database.CreateDbConnectionAsync();

        using var result = await connection.QueryMultipleAsync(new CommandDefinition(SQL, new
        {
            customerId
        }, cancellationToken: cancellationToken));

        var customer = (await result.ReadAsync<Customer>()).SingleOrDefault() ?? throw new CustomerNotFoundException();
        var transactions = (List<FinancialTransaction>)(await result.ReadAsync<FinancialTransaction>());

        return (customer, transactions);
    }

    public async Task CreateAsync(FinancialTransaction financialTransaction, CancellationToken cancellationToken)
    {
        const string SQL =
            """
            INSERT INTO FinancialTransaction (CustomerId, Value, Type, Description, ExecutedOn)
            VALUES (@CustomerId, @Value, @Type, @Description, now());
            """;

        var parameters = new
        {
            financialTransaction.CustomerId,
            financialTransaction.Value,
            financialTransaction.Type,
            financialTransaction.Description
        };

        using var connection = await database.CreateDbConnectionAsync();
        await connection.ExecuteAsync(new CommandDefinition(SQL, parameters, cancellationToken: cancellationToken));
    }
}