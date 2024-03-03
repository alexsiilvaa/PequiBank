namespace PequiBank.Presentation.Extensions;

using Microsoft.Extensions.Options;
using PequiBank.Infrastructure.Settings;

public static class DatabaseOptionsExtensions
{
    public static WebApplicationBuilder AddDatabaseOptions(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"))
            .AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();

        return builder;
    }

    public static WebApplication ValidateDatabaseOptions(this WebApplication app)
    {
        var databaseOptions = app.Services.GetRequiredService<IOptions<DatabaseOptions>>();

        app
            .Services
            .GetRequiredService<IValidateOptions<DatabaseOptions>>()
            .Validate(null, databaseOptions.Value);

        return app;
    }
}