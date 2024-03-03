namespace PequiBank.Presentation.Health;

using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PequiBank.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

public sealed class DatabaseHealthCheck(IDatabase database) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await database.CreateDbConnectionAsync();
            _ = await connection.ExecuteScalarAsync<int>("SELECT 1");
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}