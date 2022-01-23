using Building.OwnersAPI.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public interface IUnitofWork:IDisposable
    {
        public IOwnerRepository Owners { get; }
        Task Save();
    }
}
