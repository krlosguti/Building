using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.RemoteService.Interface;
using Building.PropertyAPI.RemoteService.Service;
using Microsoft.AspNetCore.Http;
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
        private readonly IOwnerService _ownerService;

        public PropertyRepository(PropertyContext context, IOwnerService ownerService)
        {
            _context = context;
            _dbProperties = _context.Set<Property>();
            _dbPropertyImages = _context.Set<PropertyImage>();
            _ownerService = ownerService;
        }

        public async Task<bool> ExistProperty(Guid IdProperty)
        {
            return await _dbProperties.AnyAsync(x => x.IdProperty == IdProperty);
        }
        public async Task AddImage(Guid IdProperty, IFormFile ImageFile)
        {
            if (ImageFile == null) throw new Exception("Image file invalid");

            var property = await _dbProperties.FirstOrDefaultAsync(x => x.IdProperty == IdProperty);

            if (property == null) throw new Exception("Property doesn't exist");

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = uniqueFileName,
                Enabled = true,
                IdProperty = IdProperty
            };

            await _dbPropertyImages.AddAsync(propertyImage);
        }

        public async void UpdatePrice(Guid IdProperty, long NewPrice)
        {
            var property = await _dbProperties.FirstOrDefaultAsync(x => x.IdProperty == IdProperty);

            if (property == null) throw new Exception("Property doesn't exist");

            property.Price = NewPrice;
            _dbProperties.Attach(property);
            _context.Entry(property).State = EntityState.Modified;
        }

        public async Task<PropertyDTO> Get(Guid id, string token)
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

            var property =  await query.FirstOrDefaultAsync();
            var result = _ownerService.GetOwner(property.IdOwner,token);
            var propertyDTO = new PropertyDTO
            {
                IdProperty = property.IdProperty,
                Address = property.Address,
                CodeInternal= property.CodeInternal,
                IdOwner= property.IdOwner,
                Name = property.Name,
                Price = property.Price,
                Year = property.Year,
                PropertyImages = property.PropertyImages,
                Owner = result.Result.owner
            };
            return propertyDTO;

        }

        public async Task<List<PropertyDTO>> GetAll(string token, RequestParameters request = null)
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
                var queryDTO = (from p in query
                                  select new PropertyDTO
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
                return await queryDTO.ToListAsync();
            }

            var queryDTO2 = (from p in query
                              select new PropertyDTO
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

            return await queryDTO2.AsNoTracking()
                              .Skip((request.pageParameters.PageNumber - 1) * request.pageParameters.PageSize)
                              .Take(request.pageParameters.PageSize)
                              .ToListAsync();
        }


        public async Task Insert(Property property, ICollection<IFormFile> ListImages = null)
        {
            
            if (ListImages != null)
            {
                string uniqueFileName = "";
                foreach (var obj in ListImages)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.FileName;
                    //upload the image file in local server
                    var propertyImage = new PropertyImage
                    {
                        IdPropertyImage = Guid.NewGuid(),
                        File = uniqueFileName,
                        Enabled = true,
                        IdProperty = property.IdProperty
                    };
                    await _dbPropertyImages.AddAsync(propertyImage);
                }
            }
            await _dbProperties.AddAsync(property);
        }
    }
}
