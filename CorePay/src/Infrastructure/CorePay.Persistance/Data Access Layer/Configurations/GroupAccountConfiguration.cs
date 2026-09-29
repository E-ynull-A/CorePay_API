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
    public class GroupAccountConfiguration : IEntityTypeConfiguration<GroupAccount>
    {
        public void Configure(EntityTypeBuilder<GroupAccount> builder)
        {
            builder.Property(ga => ga.Title)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(ga => ga.Description)
                .HasMaxLength(700);

            builder
                .HasOne(ga => ga.Account)
                .WithOne()
                .HasForeignKey<GroupAccount>(ga => ga.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
       
        }
    }
}
