using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class Account:BaseEntity
    {
        public decimal Balance { get; protected set; }
        public string IBAN { get; protected set; }
        public Currency Currency { get; protected set; }
        public AccountStatus Status { get; protected set; }


        //Relations

        public AppUser AppUser { get; protected set; }
        public Guid AppUserId { get; protected set; }

        public ICollection<Card> Cards { get; } = new List<Card>();
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
