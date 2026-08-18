using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public CardStatus Status { get; protected set; } = CardStatus.Active;

        //Relations
        public Account Account { get; protected set; }
        public Guid AccountId { get; protected set; }


        public ICollection<Transaction> Transactions { get; protected set; } = new Collection<Transaction>();


        public Card(string cardNumber, DateOnly expireDate, string cvn, Guid accountId)
        {
            CardNumber = cardNumber;
            ExpireDate = expireDate;
            CVN = cvn;
            AccountId = accountId;
        }

        public void Lock() =>
            Status = CardStatus.Locked;
        public void Active()=>
            Status = CardStatus.Active;
    }
}
