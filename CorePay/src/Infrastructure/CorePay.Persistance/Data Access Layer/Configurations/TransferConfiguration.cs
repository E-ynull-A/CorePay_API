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
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasOne(t => t.SenderAccount)
                .WithMany() 
                .HasForeignKey(t => t.SenderAccountId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(t => t.RecieverAccount)
                   .WithMany() 
                   .HasForeignKey(t => t.RecieverAccountId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(t => t.SenderCard)
                   .WithMany()
                   .HasForeignKey(t => t.SenderCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.RecieverCard)
                   .WithMany()
                   .HasForeignKey(t => t.RecieverCardId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
