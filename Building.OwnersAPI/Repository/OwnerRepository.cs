using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    /// <summary>
    /// Implementation owner repository interface
    /// </summary>
    public class OwnerRepository : IOwnerRepository
    {
        //context to connect with the owner database
        private readonly OwnerContext _context;
        //DbSet owner with the objective to do transactions from the server side
        private readonly DbSet<Owner> _db;

        public OwnerRepository(OwnerContext context)
        {
            _context = context;
            _db = _context.Set<Owner>();
        }

        /// <summary>
        /// Deletes the owner identified by id
        /// </summary>
        /// <param name="id">owner id</param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            //get the owner identified by id
            var entity = await _db.FindAsync(id);
            //remove the owner
            _db.Remove(entity);
        }

        /// <summary>
        /// Get information about owner identified by id
        /// </summary>
        /// <param name="id">owner id</param>
        /// <returns></returns>
        public async Task<Owner> Get(Guid id)
        {
            //gets and returns the owner identified by id
            return await _db.AsNoTracking().FirstOrDefaultAsync( x=> x.IdOwner == id);
        }

        //Get owners list agree to the parameters of filtering, ordering and pagination
        public async Task<List<Owner>> GetAll(RequestParameters request = null)
        {
            //get the owners iqueryable
            IQueryable<Owner> query = _db;

            //if there is not of filter and paginate parameters returns all the owners.
            if (request == null)
            {
                return await query.ToListAsync();
            }
            //validate exist parameters
            if (request.filterParameters != null)
            {
                //Creates and apply search filter if it is not null
                var fp = request.filterParameters;
                if (fp.Search != null)
                {
                    query = query.Where(x => x.Address.ToUpper().Contains(fp.Search.ToUpper()) || x.Name.ToUpper().Contains(fp.Search.ToUpper()));
                }

                //creates and apply order parameters if it is not null
                //order by name, address or birthday
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
            
            //return all the owners without use pagination
            if (request.pageParameters == null)
            {
                return await query.AsNoTracking().ToListAsync();
            }

            //return all the owners using pagination
            return await query.AsNoTracking()
                              .Skip((request.pageParameters.PageNumber - 1) * request.pageParameters.PageSize)
                              .Take(request.pageParameters.PageSize)
                              .ToListAsync();
        }

        /// <summary>
        /// insert a new owner
        /// </summary>
        /// <param name="owner">owner model</param>
        /// <returns></returns>
        public async Task Insert(Owner owner)
        {
            await _db.AddAsync(owner);
        }

        /// <summary>
        /// Update information about an owner
        /// </summary>
        /// <param name="owner">owner model</param>
        public void Update(Owner owner)
        {
            _db.Attach(owner);
            _context.Entry(owner).State = EntityState.Modified;
        }
    }
}
