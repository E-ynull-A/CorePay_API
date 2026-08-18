using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public bool IsDeleted { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }
        public Guid CreatedBy { get; protected set; }


        public DateTimeOffset UpdatedAt { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }


        public void SoftDelete()=>
            IsDeleted = true;

    }
}
