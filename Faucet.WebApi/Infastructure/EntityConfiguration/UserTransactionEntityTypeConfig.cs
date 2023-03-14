using Faucet.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faucet.WebApi.Infastructure.EntityConfiguration
{
    public class UserTransactionEntityTypeConfig : IEntityTypeConfiguration<UserTransaction>
    {
        public void Configure(EntityTypeBuilder<UserTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Address).IsRequired().HasMaxLength(256);

            builder.Property(x => x.Date).IsRequired();

            builder.Property(x => x.IP).IsRequired();

            builder.Property(x => x.Id).UseHiLo("user_tran_hilo").IsRequired();
        }
    }
}
