namespace PequiBank.Infrastructure.Settings;

using Microsoft.Extensions.Options;

[OptionsValidator]
public partial class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
}