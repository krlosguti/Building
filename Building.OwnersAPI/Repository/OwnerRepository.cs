using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly OwnerContext _context;
        private readonly DbSet<Owner> _db;

        public OwnerRepository(OwnerContext context)
        {
            _context = context;
            _db = _context.Set<Owner>();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task<Owner> Get(Guid id)
        {
            return await _db.AsNoTracking().FirstOrDefaultAsync( x=> x.IdOwner == id);
        }

        public async Task<List<Owner>> GetAll(RequestParameters request = null)
        {
            IQueryable<Owner> query = _db;

            if (request == null)
            {
                return await query.ToListAsync();
            }
            if (request.filterParameters != null)
            {
                var fp = request.filterParameters;
                if (fp.Search != null)
                {
                    query = query.Where(x => x.Address.ToUpper().Contains(fp.Search.ToUpper()) || x.Name.ToUpper().Contains(fp.Search.ToUpper()));
                }

                if (fp.orderBy != null)
                {
                    switch (fp.orderBy.ToUpper())
                    {
                        case "NAME": query = fp.asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name); break;
                        case "ADDRESS": query = fp.asc ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address); break;
                        case "BIRTHDAY": query = fp.asc ? query.OrderBy(x => x.Birthday) : query.OrderByDescending(x => x.Birthday); break;
                        default: query = fp.asc ? query.OrderBy(x => x.IdOwner) : query.OrderByDescending(x => x.IdOwner); break;
                    }

                }
            }
            
            
            if (request.pageParameters == null)
            {
                return await query.AsNoTracking().ToListAsync();
            }

            return await query.AsNoTracking()
                              .Skip((request.pageParameters.PageNumber - 1) * request.pageParameters.PageSize)
                              .Take(request.pageParameters.PageSize)
                              .ToListAsync();
        }

        public async Task Insert(Owner owner)
        {
            await _db.AddAsync(owner);
        }

        public void Update(Owner owner)
        {
            _db.Attach(owner);
            _context.Entry(owner).State = EntityState.Modified;
        }
    }
}
