using CorePay.Application.Interfaces.Repositories.Common;
using CorePay.Domain.Entities.Common;
using CorePay.Persistance.Data_Access_Layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CorePay.Persistance.Implementations.Repositories.Common
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? func = null,
                                    string[]? includes = null,
                                    int page = 0,
                                    int take = 0,
                                    bool isFiltered = true,
                                    Expression<Func<T, bool>>? orderBy = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (func is not null)
                query = query.Where(func);

            if (page != 0 && take != 0)
                query = query.Skip((page - 1) * take).Take(take);

            if (!isFiltered)
                query = query.IgnoreQueryFilters();

            if (orderBy is not null)
                query = query.OrderBy(orderBy);

            if(includes is not null)
                query = _addIncludes(query, includes);

            return query;
        }


        private IQueryable<T> _addIncludes(IQueryable<T> query,string[] includes)
        {
            foreach (var include in includes)
            {
               query = query.Include(include);
            }

            return query;
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> func, string[]? includes = null)
        {         
            if(includes is not null)
               return await _addIncludes(_dbSet, includes).FirstOrDefaultAsync(func);
            
            return await _dbSet.FirstOrDefaultAsync(func);
        } 
           
       
        public async Task<T?> GetByIdAsync(Guid id,
                                    string[]? includes = null,
                                    bool isFiltered = true)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (includes is not null)
                query = _addIncludes(query, includes);

            if(!isFiltered)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(q => q.Id == id);           
        }

        public void Update(T updatedItem)=>        
            _dbSet.Update(updatedItem);
        
        public void Add(T item)=>
            _dbSet.Add(item);

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> func) =>
           await _dbSet.AnyAsync(func);

        public async Task<int> CountAsync(Expression<Func<T, bool>> func) =>
            await _dbSet.CountAsync(func);
            
    }
}
