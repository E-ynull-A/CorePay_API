using CorePay.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class Transfer:BaseEntity
    {
        public decimal Amount { get;protected set; }


        public Guid SenderAccountId { get;protected set; }
        public Account SenderAccount { get;protected set; }

        public Guid RecieverAccountId { get; protected set; }
        public Account RecieverAccount { get; protected set; }



        public Guid? SenderCardId { get; protected set; }
        public Card? SenderCard { get;protected set; }

        public Guid? RecieverCardId { get; protected set; }
        public Card? RecieverCard { get; protected set; }

        public ICollection<Transaction> Transactions { get; protected set; } = new List<Transaction>();


        public Transfer(Guid senderAccountId,
                        Guid recieverAccountId,
                        Guid? senderCardId = null,
                        Guid? recieverCardId = null)
        {
            SenderAccountId = senderAccountId;
            RecieverAccountId = recieverAccountId;
            SenderCardId = senderCardId;
            RecieverCardId = recieverCardId;
        }
    }
}
