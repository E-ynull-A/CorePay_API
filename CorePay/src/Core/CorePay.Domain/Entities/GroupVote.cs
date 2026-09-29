using CorePay.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class GroupVote:BaseEntity
    {
        public GroupProposal GroupProposal { get;protected set; }
        public Guid GroupProposalId { get;protected set; }

        public GroupMember GroupMember { get;protected set; }
        public Guid GroupMemberId { get;protected set; }

        public bool IsApproved { get;protected set; }
       
    }
}
