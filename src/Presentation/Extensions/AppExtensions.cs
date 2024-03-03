namespace PequiBank.Presentation.Extensions;

public static class AppExtensions
{
    public static WebApplication UseArchitectures(this WebApplication app)
    {
        app.ValidateDatabaseOptions();
        app.UseExceptionHandler(opt => { });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRequestTimeouts();

        app.MapHealthChecks("/healthcheck");

        return app;
    }
}