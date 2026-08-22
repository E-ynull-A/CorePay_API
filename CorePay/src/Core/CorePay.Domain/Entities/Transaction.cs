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
        public Account? Account { get; protected set; }
        public Guid? AccountId { get; protected set; }


        public Guid? CardId { get; protected set; }
        public Card? Card { get; protected set; }


        public Transaction(decimal amount, TransactionType type, Guid? accountId = null, Guid? cardId = null)
        {
            Amount = amount;
            Type = type;
            AccountId = accountId;
            CardId = cardId;

            Validate();
        }

        public void Validate()
        {
            if (AccountId is null && CardId is null)
                throw new InvalidOperationException("There must be one of two property: AccountId or CardId");

            else if(AccountId is not null && CardId is not null)
                throw new InvalidOperationException("There must be one of two property: AccountId or CardId");
        }

    }
}
