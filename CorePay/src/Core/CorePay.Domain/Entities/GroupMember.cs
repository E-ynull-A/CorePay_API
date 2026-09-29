using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class GroupMember:BaseEntity
    {
        public AppUser AppUser { get;protected set; }
        public Guid AppUserId { get;protected set; }

        public GroupAccount GroupAccount { get;protected set; }
        public Guid GroupAccountId { get;protected set; }


        public ICollection<GroupVote> GroupVotes { get; protected set; } = new List<GroupVote>();

        public GroupMemberStatus Status { get;protected set; }

    }
}
