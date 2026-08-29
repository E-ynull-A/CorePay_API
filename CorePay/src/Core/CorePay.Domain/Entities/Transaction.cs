using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; protected set; }
        public TransactionType Type { get; protected set; }

        //Relations
        public Account Account { get; protected set; }
        public Guid AccountId { get; protected set; }

        public Guid? CardId { get; protected set; }
        public Card? Card { get; protected set; }


        public Transfer? Transfer { get;protected set; }
        public Guid? TransferId { get;protected set; }


        public Transaction(decimal amount,
                           TransactionType type,
                           Guid accountId,
                           Guid? cardId = null,
                           Guid? transferId = null)
        {
            Amount = amount;
            Type = type;
            AccountId = accountId;
            CardId = cardId;
            TransferId = transferId;
            Validate();           
        }

        public void Validate()
        {
            if (TransactionType.Withdraw == Type && CardId is null)
                throw new InvalidOperationException("These transaction is required a CardId!");
            else if (TransactionType.Transfer == Type && Transfer is null)
                throw new InvalidOperationException("These transaction is required a Transfer object!");
        }

    }
}
