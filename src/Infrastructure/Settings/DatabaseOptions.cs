namespace PequiBank.Infrastructure.Settings;

using System.ComponentModel.DataAnnotations;

public sealed class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }
}