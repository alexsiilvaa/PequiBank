namespace PequiBank.Infrastructure.BackgroundServices;

using Microsoft.Extensions.Hosting;
using PequiBank.Application.Repository;
using PequiBank.Domain.Model;
using System.Threading;
using System.Threading.Channels;

public sealed class CreateFinancialTransactionBackgroundService
    (
        ChannelReader<FinancialTransaction> reader,
        IFinancialTransactionRepository financialTransactionRepository
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var financialTransaction in reader.ReadAllAsync(stoppingToken))
        {
            await financialTransactionRepository.CreateAsync(financialTransaction, stoppingToken);
        }
    }
}