using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.RemoteService.Interface;
using Building.PropertyAPI.RemoteService.Service;
using System;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly PropertyContext _context;
        private IPropertyRepository _properties;
        private IOwnerService _ownerService;

        public UnitofWork(PropertyContext context, IPropertyRepository properties, IOwnerService ownerService)
        {
            _context = context;
            _properties = properties;
            _ownerService = ownerService;
        }


        public IPropertyRepository Properties => _properties ??= new PropertyRepository(_context, _ownerService);

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
