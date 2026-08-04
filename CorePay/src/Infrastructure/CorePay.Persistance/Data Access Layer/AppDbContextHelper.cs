using CorePay.Domain.Entities;
using CorePay.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance.Data_Access_Layer
{
    public static class AppDbContextHelper
    {
        public static void ApplyAllQueryFilters(this ModelBuilder builder)
        {
            builder._applyQueryFilter<Account>();
            builder._applyQueryFilter<Card>();
            builder._applyQueryFilter<Transaction>();
        }
        private static void _applyQueryFilter<T>(this ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => e.IsDeleted == false);
        }

        public static void SaveChangeInterseptor(this ChangeTracker tracker)
        {
            var entries = tracker.Entries<BaseEntity>();

            foreach (var entity in entries)
            {
                switch (entity.State)
                {
                    case EntityState.Modified:                
                            entity.Entity.UpdatedAt = DateTime.UtcNow;                        
                        break;

                }
            }
            //entity.OriginalValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)) ==
            //entity.CurrentValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)))
        }
    }
}
