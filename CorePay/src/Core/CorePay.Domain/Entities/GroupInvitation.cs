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

        public AppUser InvitedUser { get;protected set; }
        public Guid InvitedUserId { get;protected set; }

        public InvitationStatus Status { get;protected set; }

        public DateTimeOffset ExpireAt { get; protected set; } = DateTimeOffset.UtcNow.AddHours(24);

        public GroupInvitation() { }

        public GroupInvitation(Guid groupAccounId, Guid invitedUserId)
        {
            IsExpired();
            GroupAccounId = groupAccounId;
            InvitedUserId = invitedUserId;
        }

        public void Accept() =>
            Status = InvitationStatus.Accepted;

        public void Reject()=>
            Status = InvitationStatus.Rejected;

        public void IsExpired()
        {
            if (ExpireAt < DateTimeOffset.UtcNow)
                    Status = InvitationStatus.Expired;
        }
       

       

    }
}
