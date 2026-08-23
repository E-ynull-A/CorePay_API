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
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder
                .HasOne(c => c.Account)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(c => c.PinHash)
                .IsRequired()
                .HasMaxLength(256);


            builder
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            builder.
                Property(c => c.CardNumber)
                .IsRequired()
                .HasMaxLength(19);

            builder.
                Property(c => c.CVN)
                .IsRequired()
                .HasMaxLength(4);

            builder.
                Property(c => c.ExpireDate)
                .IsRequired();
            
        }
    }
}
