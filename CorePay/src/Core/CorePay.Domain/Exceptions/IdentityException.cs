using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Exceptions
{
    public class IdentityException:Exception
    {
        public IEnumerable<IdentityError> Errors { get; } = new Collection<IdentityError>();

        public IdentityException(IEnumerable<IdentityError> errors) : base("Identity operation failed!")
        {
            Errors = errors;
        }

    }
}
