using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class Card : BaseEntity
    {
        public string CardNumber { get; protected set; }
        public DateOnly ExpireDate { get; protected set; }
        public string CVN { get; protected set; }
        public CardStatus Status { get; protected set; }

        //Relations
        public Account Account { get; protected set; }
        public Guid AccountId { get; protected set; }
    }
}
