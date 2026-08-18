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
                                    bool orderByAscending = true);
        Task<T?> GetByIdAsync(Guid id,
                              string[]? includes = null,
                              bool isFiltered = true);

        Task<int> CountAsync(Expression<Func<T,bool>> func);
        Task<bool> AnyAsync(Expression<Func<T,bool>> func);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> func,
                                          string[]? includes = null,
                                          bool isFiltered = true);
        void Update(T updatedItem);
        void Add(T item);
        Task SaveChangesAsync();
    }
}
