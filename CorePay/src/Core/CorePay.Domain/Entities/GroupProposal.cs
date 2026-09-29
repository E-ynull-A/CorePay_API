using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class GroupProposal:BaseEntity
    {
        public GroupAccount GroupAccount { get;protected set; }
        public Guid GroupAccountId { get;protected set; }

        public GroupMember CreatedByMember { get;protected set; }
        public Guid CreatedByMemberId { get;protected set; }

        public ProposalType Type { get;protected set; }
        public ProposalStatus Status { get; protected set; }

        public decimal? Amount { get;protected set; }     
        public DateTimeOffset ExpireAt { get; } = DateTimeOffset.UtcNow.AddHours(24);

        public ICollection<GroupVote> GroupVotes { get; protected set; } = new List<GroupVote>();

    }
}
