using CorePay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance.Data_Access_Layer.Configurations
{
    public class IdempotencyRequestConfiguration : IEntityTypeConfiguration<IdempotencyRequest>
    {
        public void Configure(EntityTypeBuilder<IdempotencyRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IdempotencyKey)
                            .HasMaxLength(128)
                            .IsRequired();

            builder.Property(x => x.RequestHash)
                           .HasMaxLength(64)
                           .IsRequired();

            builder.HasIndex(x => new
            {
                x.UserId,
                x.IdempotencyKey
            })
            .IsUnique();

            builder.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(ir => ir.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ir=>ir.Transfer)
                .WithOne(t=>t.IdempotencyRequest)
                .HasForeignKey<IdempotencyRequest>(ir=>ir.TransferId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
