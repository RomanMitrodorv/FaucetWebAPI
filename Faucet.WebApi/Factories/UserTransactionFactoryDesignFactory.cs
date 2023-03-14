using Faucet.WebApi.Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Faucet.WebApi.Factories
{
    public class UserTransactionFactoryDesignFactory : IDesignTimeDbContextFactory<UserTransactionContext>
    {
        UserTransactionContext IDesignTimeDbContextFactory<UserTransactionContext>.CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<UserTransactionContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("Faucet.WebApi"));

            return new UserTransactionContext(optionsBuilder.Options);
        }
    }
}
