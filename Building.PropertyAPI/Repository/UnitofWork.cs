using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.RemoteService.Interface;
using Building.PropertyAPI.RemoteService.Service;
using System;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        //context to connect with property database
        private readonly PropertyContext _context;
        //repository to management transactions in the property database
        private IPropertyRepository _properties;
        //services to get information about the owner property from owner microservice
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
