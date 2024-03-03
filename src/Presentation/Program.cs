using PequiBank.Presentation.Endpoints;
using PequiBank.Presentation.Extensions;

var builder = WebApplication
    .CreateSlimBuilder(args)
    .AddArchitectures();

var app = builder.Build()
    .MapClienteEndpoints()
    .UseArchitectures();

app.Run();