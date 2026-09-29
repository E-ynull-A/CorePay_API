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
    public class GroupProposalConfiguration : IEntityTypeConfiguration<GroupProposal>
    {
        public void Configure(EntityTypeBuilder<GroupProposal> builder)
        {
            builder
                  .HasOne(gp => gp.GroupAccount)
                  .WithMany(ga => ga.Proposals)
                  .HasForeignKey(p => p.GroupAccountId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(gp => gp.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

        }
    }
}
