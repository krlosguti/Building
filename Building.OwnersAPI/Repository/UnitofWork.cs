using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly OwnerContext _context;
        private IGenericRepository<Owner> _owners;

        public UnitofWork(OwnerContext context, IGenericRepository<Owner> owners)
        {
            _context = context;
            _owners = owners;
        }


        public IGenericRepository<Owner> Owners => _owners ??= new GenericRepository<Owner>(_context);

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
