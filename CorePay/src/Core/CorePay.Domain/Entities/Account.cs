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
        public AccountType Type { get; protected set; }

        //Relations

        public AppUser? AppUser { get; protected set; }
        public Guid? AppUserId { get; protected set; }

        public ICollection<Card> Cards { get; } = new List<Card>();
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        public Account(string iBAN, Currency currency, AccountType type, Guid? appUserId = null)
        {
            IBAN = iBAN;
            Currency = currency;
            AppUserId = appUserId;
            Balance = 0;
            AppUserId = appUserId;
            Type = type;
            Validate();       
        }

        public void Validate()
        {
            if (Type == AccountType.Personal && AppUserId is null
              || Type == AccountType.Group && AppUserId is not null)
                throw new InvalidOperationException("Account Type and AppUser requirements are wrong!");
        }

        public void IncreaseBalance(decimal amount) =>
            Balance += amount;
        public void DecreaseBalance(decimal amount)=>
            Balance -= amount;

        public void Activate()=>
            Status = AccountStatus.Active;
        public void BlokedByUser()=>
            Status = AccountStatus.UserBlocked;
        public void Close()=>
            Status = AccountStatus.Closed;
    }
}
