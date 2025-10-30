using System;
using Microsoft.Extensions.Configuration;

namespace StudentManagement.Core
{
    public abstract class DatabaseServiceBase
    {
        protected readonly string ConnectionString;

        protected DatabaseServiceBase(string? connectionString = null)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                ConnectionString = connectionString;
            }
            else
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                IConfiguration config = builder.Build();

                ConnectionString = config.GetConnectionString("Default")
                    ?? throw new InvalidOperationException("Connection string 'Default' not found in appsettings.json.");
            }
        }
    }
}
