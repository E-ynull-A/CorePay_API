using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class GroupInvitation:BaseEntity
    {
        public GroupAccount GroupAccount { get;protected set; }
        public Guid GroupAccounId { get;protected set; }

        public AppUser CreatedUser { get;protected set; }
        public Guid CreatedUserId { get;protected set; }

        public InvitationStatus Status { get;protected set; }

        public DateTimeOffset ExpireAt { get; protected set; } = DateTimeOffset.Now.AddHours(24);


    }
}
