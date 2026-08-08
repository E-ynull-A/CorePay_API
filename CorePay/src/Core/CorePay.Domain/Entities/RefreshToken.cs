using CorePay.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; protected set; }
        public DateTimeOffset ValidFrom { get; protected set; }
        public bool IsRevoked { get; protected set; }

        //Relation

        public AppUser AppUser { get; protected set; }
        public Guid AppUserId { get; protected set; }

        public RefreshToken(string token,
                            Guid appUserId,
                            DateTimeOffset validFrom)
        {
            Token = token;
            AppUserId = appUserId;
            ValidFrom = validFrom;
        }

        public void Revoke()=>        
            IsRevoked = true;
        
    }
}
