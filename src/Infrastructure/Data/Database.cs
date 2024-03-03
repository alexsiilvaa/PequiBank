namespace PequiBank.Infrastructure.Data;

using Microsoft.Extensions.Options;
using Npgsql;
using PequiBank.Infrastructure.Settings;
using System.Threading.Tasks;

public class Database(IOptions<DatabaseOptions> databaseOptions) : IDatabase
{
    public async Task<NpgsqlConnection> CreateDbConnectionAsync()
    {
        var npgsqlConnection = new NpgsqlConnection(databaseOptions.Value.ConnectionString);

        try
        {
            await npgsqlConnection.OpenAsync();
        }
        catch
        {
            npgsqlConnection.Dispose();
            throw;
        }

        return npgsqlConnection;
    }
}