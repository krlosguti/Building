using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly PropertyContext _context;
        private readonly DbSet<Property> _dbProperties;
        private readonly DbSet<PropertyImage> _dbPropertyImages;

        public PropertyRepository(PropertyContext context)
        {
            _context = context;
            _dbProperties = _context.Set<Property>();
            _dbPropertyImages = _context.Set<PropertyImage>();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Property> Get(Guid id)
        {
            IQueryable<Property> query = _dbProperties;
            IQueryable<PropertyImage> queryI = _dbPropertyImages;

            query = (from p in query
                     select new Property
                     {
                         IdProperty = p.IdProperty,
                         Name = p.Name,
                         Address = p.Address,
                         CodeInternal = p.CodeInternal,
                         IdOwner = p.IdOwner,
                         Price = p.Price,
                         Year = p.Year,
                         PropertyImages = (from i in queryI
                                           where i.IdProperty == p.IdProperty
                                           select new PropertyImage
                                           {
                                               IdPropertyImage = i.IdPropertyImage,
                                               Enabled = i.Enabled,
                                               File = i.File,
                                               IdProperty = i.IdProperty
                                           }).ToList()
                     }
                    )
                    .Where(x => x.IdProperty == id);

            return query.FirstOrDefault();
        }

        public async Task<List<Property>> GetAll(RequestParameters request = null)
        {
            IQueryable<Property> query = _dbProperties;
            IQueryable<PropertyImage>  queryI = _dbPropertyImages;

            if (request.filterParameters != null)
            {
                var fp = request.filterParameters;
                
                if (fp.Search != null)
                {
                    query = query.Where(x => x.Address.ToUpper().Contains(fp.Search.ToUpper()) || 
                                             x.Name.ToUpper().Contains(fp.Search.ToUpper()) ||
                                             x.CodeInternal.ToUpper().Contains(fp.Search.ToUpper()));
                }

                if (fp.orderBy != null)
                {
                    switch (fp.orderBy.ToUpper())
                    {
                        case "NAME": query = fp.asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name); break;
                        case "ADDRESS": query = fp.asc ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address); break;
                        case "CODEINTERNAL": query = fp.asc ? query.OrderBy(x => x.CodeInternal) : query.OrderByDescending(x => x.CodeInternal); break;
                        case "YEAR": query = fp.asc ? query.OrderBy(x => x.CodeInternal) : query.OrderByDescending(x => x.CodeInternal); break;
                        case "IDOWNER": query = fp.asc ? query.OrderBy(x => x.IdOwner) : query.OrderByDescending(x => x.IdOwner); break;
                            default: query = fp.asc ? query.OrderBy(x => x.IdProperty) : query.OrderByDescending(x => x.IdProperty); break;
                    }
                }
            }


            if (request.pageParameters == null)
            {
                query = (from p in query
                                  select new Property
                                  {
                                      IdProperty = p.IdProperty,
                                      Name = p.Name,
                                      Address = p.Address,
                                      CodeInternal = p.CodeInternal,
                                      IdOwner = p.IdOwner,
                                      Price = p.Price,
                                      Year = p.Year,
                                      PropertyImages = (from i in queryI
                                                        where i.IdProperty == p.IdProperty
                                                        select new PropertyImage
                                                        {
                                                            IdPropertyImage = i.IdPropertyImage,
                                                            Enabled = i.Enabled,
                                                            File = i.File,
                                                            IdProperty = i.IdProperty
                                                        }).ToList()
                                  }
                    ).AsQueryable();
                return await query.ToListAsync();
            }

            query = (from p in query
                              select new Property
                              {
                                  IdProperty = p.IdProperty,
                                  Name = p.Name,
                                  Address = p.Address,
                                  CodeInternal = p.CodeInternal,
                                  IdOwner = p.IdOwner,
                                  Price = p.Price,
                                  Year = p.Year,
                                  PropertyImages = (from i in queryI
                                                    where i.IdProperty == p.IdProperty
                                                    select new PropertyImage
                                                    {
                                                        IdPropertyImage = i.IdPropertyImage,
                                                        Enabled = i.Enabled,
                                                        File = i.File,
                                                        IdProperty = i.IdProperty
                                                    }).ToList()
                              }
                    ).AsQueryable();

            return await query.AsNoTracking()
                              .Skip((request.pageParameters.PageNumber - 1) * request.pageParameters.PageSize)
                              .Take(request.pageParameters.PageSize)
                              .ToListAsync();
        }


        public Task Insert(Property property)
        {
            throw new NotImplementedException();
        }

        public void Update(Property property)
        {
            throw new NotImplementedException();
        }
    }
}
