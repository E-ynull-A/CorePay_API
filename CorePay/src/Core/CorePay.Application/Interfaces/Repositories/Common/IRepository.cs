using CorePay.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Application.Interfaces.Repositories.Common
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? func = null,
                                    string[]? includes = null,
                                    int page = 0,
                                    int take = 0,
                                    bool isFiltered = true,
                                    Expression<Func<T, bool>>? orderBy = null);
        Task<T?> GetByIdAsync(Guid id,
                              string[]? includes = null,
                              bool isFiltered = true);
        void Update(T updatedItem);
        void Add(T item);
        void SoftDelete(Guid id);
        Task SaveChangesAsync();
    }
}
