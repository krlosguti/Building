using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Building.OwnersAPI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly OwnerContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(OwnerContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);

        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach(var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetPagedList(FilterParameters<T> requestParams)
        {
            IQueryable<T> query = _db;

            if (requestParams.expression != null)
            {
                query = query.Where(requestParams.expression);
            }

            if (requestParams.includes != null)
            {
                foreach (var includeProperty in requestParams.includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (requestParams.orderBy != null)
            {
                query = requestParams.orderBy(query);
            }

            return await query.AsNoTracking()
                              .Skip((requestParams.PageNumber - 1) * requestParams.PageSize)
                              .Take(requestParams.PageSize)
                              .ToListAsync();
        }
        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
