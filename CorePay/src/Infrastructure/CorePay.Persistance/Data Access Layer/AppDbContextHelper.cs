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
            builder._applyQueryFilter<RefreshToken>();
            builder._applyQueryFilter<Transfer>();
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
                        entity.Property(u => u.UpdatedAt).CurrentValue = DateTimeOffset.UtcNow;
                        entity.Property(u => u.UpdatedBy).CurrentValue = Guid.Parse("af29e40f-ff70-4d9a-1615-08def795fab3");
                        break;
                    case EntityState.Added:
                        entity.Property(u => u.CreatedAt).CurrentValue = DateTimeOffset.UtcNow;
                        entity.Property(u => u.CreatedBy).CurrentValue = Guid.Parse("af29e40f-ff70-4d9a-1615-08def795fab3");
                        break;
                }
            }

            var userEntries = tracker.Entries<AppUser>();

            foreach (var entity in userEntries)
            {
                switch (entity.State)
                {
                    case EntityState.Modified:
                        entity.Property(nameof(AppUser.UpdatedAt)).CurrentValue = DateTimeOffset.UtcNow;
                        entity.Property(nameof(AppUser.UpdatedBy)).CurrentValue = Guid.Parse("af29e40f-ff70-4d9a-1615-08def795fab3");
                        break;

                    case EntityState.Added:
                        entity.Property(nameof(AppUser.CreatedAt)).CurrentValue = DateTimeOffset.UtcNow;
                        entity.Property(nameof(AppUser.CreatedBy)).CurrentValue = Guid.Parse("af29e40f-ff70-4d9a-1615-08def795fab3");
                        break;
                }
            }


            //entity.OriginalValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)) ==
            //entity.CurrentValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)))
        }
    }
}
