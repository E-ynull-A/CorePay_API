using CorePay.Domain.Entities;
using CorePay.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorePay.Persistance.Data_Access_Layer.Configurations
{
    public class AccountConfiguration:IEntityTypeConfiguration<Account>
    {

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.
                 HasOne(a => a.AppUser)
                 .WithMany(a => a.Accounts)
                 .HasForeignKey(a => a.AppUserId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(a => a.Balance)
                .HasPrecision(18,2)
                .IsRequired();

            builder
                 .HasIndex(a => a.IBAN)
                 .IsUnique();

            builder
                .Property(a => a.IBAN)
                .HasMaxLength(34)
                .IsRequired();
        }
    }
}
