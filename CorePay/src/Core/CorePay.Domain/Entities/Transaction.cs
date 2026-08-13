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
    }
}
