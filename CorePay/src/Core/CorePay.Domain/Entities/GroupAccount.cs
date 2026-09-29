using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class GroupAccount:BaseEntity
    {
        public string Title { get;protected set; }
        public string? Description { get;protected set; }

        public Account Account { get;protected set; }
        public Guid AccountId { get;protected set; }


        public DateTimeOffset ExpireAt { get;protected set; }
        public GroupAccountStatus Status { get;protected set; }

        public ICollection<GroupMember> GroupMembers { get; protected set; } = new List<GroupMember>();

        public ICollection<GroupProposal> Proposals { get; protected set; } = new List<GroupProposal>();
    }
}
