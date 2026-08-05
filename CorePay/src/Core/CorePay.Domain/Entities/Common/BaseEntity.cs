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

        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedBy { get; set; }


        public DateTimeOffset UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }


        public void SoftDelete(Guid? deletedBy)=>
            IsDeleted = true;

    }
}
