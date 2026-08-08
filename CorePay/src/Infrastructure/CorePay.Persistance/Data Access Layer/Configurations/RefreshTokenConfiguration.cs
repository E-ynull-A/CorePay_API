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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {

            builder
                    .HasOne(x => x.AppUser)
                    .WithMany(x=>x.RefreshTokens)
                    .HasForeignKey(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.
                HasIndex(r=>r.Token).IsUnique();

            builder
                .Property(r => r.Token)
                .IsRequired();
               
        }
    }
}
