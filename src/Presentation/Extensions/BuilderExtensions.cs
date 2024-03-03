namespace PequiBank.Presentation.Extensions;

using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.DependencyInjection;
using PequiBank.Application.Features.CreateFinancialTransaction;
using PequiBank.Application.Repository;
using PequiBank.Domain.Model;
using PequiBank.Infrastructure.BackgroundServices;
using PequiBank.Infrastructure.Data;
using PequiBank.Infrastructure.Repository;
using PequiBank.Presentation.ExceptionHandlers;
using PequiBank.Presentation.Health;
using System.Net;
using System.Threading.Channels;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddArchitectures(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        builder.Services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(30),
                TimeoutStatusCode = (int)HttpStatusCode.ServiceUnavailable
            };
        });

        builder
            .AddDatabaseOptions()
            .AddServices()
            .AddHealthChecks()
            .RegisterExceptionHandlers()
            .RegisterFinancialTransactionBackgroundService();

        return builder;
    }

    private static WebApplicationBuilder RegisterExceptionHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<CreditLimitExceededExceptionHandler>();
        builder.Services.AddExceptionHandler<CustomerNotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<InvalidFinancialTransactionTypeExceptionHandler>();
        builder.Services.AddExceptionHandler<InvalidValueExceptionHandler>();
        builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

        return builder;
    }

    private static WebApplicationBuilder RegisterFinancialTransactionBackgroundService(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<Channel<FinancialTransaction>>(ctx =>
            {
                return Channel.CreateUnbounded<FinancialTransaction>();
            })
            .AddSingleton<ChannelReader<FinancialTransaction>>(ctx =>
            {
                var channel = ctx.GetService<Channel<FinancialTransaction>>();
                return channel!.Reader;
            }).AddSingleton<ChannelWriter<FinancialTransaction>>(ctx =>
            {
                var channel = ctx.GetService<Channel<FinancialTransaction>>();
                return channel!.Writer;
            });

        builder.Services.AddHostedService<CreateFinancialTransactionBackgroundService>();

        return builder;
    }

    private static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("Database");

        return builder;
    }

    private static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddSingleton<IDatabase, Database>()
            .AddSingleton<ICustomerRepository, CustomerRepository>()
            .AddSingleton<IFinancialTransactionRepository, FinancialTransactionRepository>()
            .AddScoped<ICreateFinancialTransaction, CreateFinancialTransaction>()
            .AddScoped<PequiBank.Application.Features.GetBankStatement.IGetBankStatement, PequiBank.Application.Features.GetBankStatement.GetBankStatement>();

        return builder;
    }
}