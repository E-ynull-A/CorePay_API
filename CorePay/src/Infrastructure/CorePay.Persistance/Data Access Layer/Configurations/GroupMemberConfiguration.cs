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
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder
                 .HasOne(gm => gm.GroupAccount)
                 .WithMany(ga => ga.GroupMembers)
                 .HasForeignKey(g => g.GroupAccountId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(gm=>gm.AppUser)
                .WithMany(a=>a.GroupMemberships)
                .HasForeignKey(k=>k.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
                
        }
    }
}
