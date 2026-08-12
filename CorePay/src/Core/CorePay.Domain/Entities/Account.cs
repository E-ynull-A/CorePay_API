using CorePay.Domain.Entities.Common;
using CorePay.Domain.Utilities.Enums;

namespace CorePay.Domain.Entities
{
    public class Account:BaseEntity
    {
        public decimal Balance { get; protected set; }
        public string IBAN { get; protected set; }
        public Currency Currency { get; protected set; }
        public AccountStatus Status { get; protected set; } = AccountStatus.Active;


        //Relations

        public AppUser AppUser { get; protected set; }
        public Guid AppUserId { get; protected set; }

        public ICollection<Card> Cards { get; } = new List<Card>();
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        public Account(string iBAN, Currency currency, Guid appUserId)
        {
            IBAN = iBAN;
            Currency = currency;
            AppUserId = appUserId;
            Balance = 0;
        }

        public void Activate()=>
            Status = AccountStatus.Active;
        public void BlokedByUser()=>
            Status = AccountStatus.UserBlocked;
    }
}
