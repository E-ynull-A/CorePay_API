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
    public class GroupVoteConfiguration : IEntityTypeConfiguration<GroupVote>
    {
        public void Configure(EntityTypeBuilder<GroupVote> builder)
        {
            builder
                .HasOne(gv=>gv.GroupMember)
                .WithMany(gm=>gm.GroupVotes)
                .HasForeignKey(gv=>gv.GroupMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(gv => gv.GroupProposal)
                .WithMany(gp => gp.GroupVotes)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(gv => gv.IsApproved)
                .IsRequired();
        }
    }
}
