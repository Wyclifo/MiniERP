using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MiniERP.Data
{

    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");

            var connectionStringBuilder =
                new SqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.InitialCatalog;

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException(
                    "Database name is missing from DefaultConnection.");
            }

            // Connect to master first.
            connectionStringBuilder.InitialCatalog = "master";

            await using var connection =
                new SqlConnection(connectionStringBuilder.ConnectionString);

            await connection.OpenAsync();

            var escapedDatabaseName =
                databaseName.Replace("]", "]]");

            await using var command = connection.CreateCommand();

            command.CommandText = $"""
            IF DB_ID(@databaseName) IS NULL
            BEGIN
                CREATE DATABASE [{escapedDatabaseName}];
            END
            """;

            command.Parameters.AddWithValue(
                "@databaseName",
                databaseName);

            await command.ExecuteNonQueryAsync();

            Console.WriteLine(
                $"Database '{databaseName}' is ready.");

            // Apply EF Core migrations.
            using var scope = services.CreateScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            Console.WriteLine(
                $"EF Core migrations applied successfully to '{databaseName}'.");
        }
    }
}
