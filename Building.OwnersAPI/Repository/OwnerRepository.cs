using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly OwnerContext _context;
        public OwnerRepository(OwnerContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Owner>> GetOwners()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<PaginationEntity> PaginationBy(Expression<Func<bool>> filterExpression, PaginationEntity pagination)
        {
            var queryLinq = _context.Owners
                                .AsNoTracking()
                                .Skip(pagination.Page * pagination.PageSize)
                                .Take(pagination.PageSize)
                                .Include("PropertyImage");
            pagination.Owners = await queryLinq.ToListAsync();
            pagination.Page++;
            return pagination;
        }
    }
}
