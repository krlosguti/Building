using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly PropertyContext _context;
        public PropertyRepository(PropertyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Property>> GetProperties()
        {
            return await _context.Property.Include("PropertyImages").ToListAsync();
        }
    }
}
