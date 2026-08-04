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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .HasOne(t => t.Account)
                .WithMany(t => t.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(t => t.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

        }
    }
}
