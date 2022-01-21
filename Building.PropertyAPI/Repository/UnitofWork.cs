using Building.PropertyAPI.Core.Context;
using System;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly PropertyContext _context;
        private IPropertyRepository _properties;

        public UnitofWork(PropertyContext context, IPropertyRepository properties)
        {
            _context = context;
            _properties = properties;
        }


        public IPropertyRepository Properties => _properties ??= new PropertyRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
