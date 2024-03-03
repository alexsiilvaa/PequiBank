namespace PequiBank.Infrastructure.Data;

using Npgsql;
using System.Threading.Tasks;

public interface IDatabase
{
    Task<NpgsqlConnection> CreateDbConnectionAsync();
}