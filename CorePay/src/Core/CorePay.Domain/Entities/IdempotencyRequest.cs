using CorePay.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class IdempotencyRequest:BaseEntity
    {
        public Guid UserId { get;protected set; }
        public string IdempotencyKey { get; private set; } = null!;
        public string RequestHash { get; private set; } = null!;
        public Guid? TransferId { get;private set; }
        public Transfer? Transfer { get;private set; }

        public IdempotencyRequest()
        {
            
        }

        public IdempotencyRequest(Guid userId, string idempotencyKey, string requestHash)
        {
            UserId = userId;
            IdempotencyKey = idempotencyKey;
            RequestHash = requestHash;
        }

        public void AttachTransfer(Guid transferId)
        {
            if(TransferId.HasValue)
                throw new InvalidOperationException(
                            "Transfer is already assigned.");

            TransferId = transferId;
        }

    }
}
