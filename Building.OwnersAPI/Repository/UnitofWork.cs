using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        //context to connect with owners database
        private readonly OwnerContext _context;
        //repository to management transactions in the owner database
        private IOwnerRepository _owners;

        public UnitofWork(OwnerContext context, IOwnerRepository owners)
        {
            _context = context;
            _owners = owners;
        }


        public IOwnerRepository Owners => _owners ??= new OwnerRepository(_context);

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
