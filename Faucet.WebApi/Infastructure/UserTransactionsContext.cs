using Faucet.WebApi.Infastructure.EntityConfiguration;
using Faucet.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace Faucet.WebApi.Infastructure
{
    public class UserTransactionContext : DbContext
    {
        public DbSet<UserTransaction> UserTransactions { get; set; }

        public UserTransactionContext(DbContextOptions<UserTransactionContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserTransactionEntityTypeConfig());
        }
    }
}
