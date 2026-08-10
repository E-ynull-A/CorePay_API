using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities
{
    public class AppUser:IdentityUser<Guid>
    {
        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public DateOnly Birthdate { get; protected set; }
        public string FIN { get; protected set; }



        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
        public string CreatedBy { get; protected set; }

        public DateTimeOffset? UpdatedAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }


        //Relations

        public ICollection<Account> Accounts { get; } = new List<Account>();
        public ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

        private AppUser() { }
                           
        public AppUser(string name,
                       string surname,
                       string userName,
                       DateOnly birthdate,
                       string email,
                       string phoneNumber,
                       string fIN)
        {
            Name = name;
            Surname = surname;
            Birthdate = birthdate;
            FIN = fIN;
            Email = email;
            PhoneNumber = phoneNumber;  
            UserName = userName;
        }
    }
}
