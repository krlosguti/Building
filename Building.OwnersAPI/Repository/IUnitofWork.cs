using Building.OwnersAPI.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public interface IUnitofWork:IDisposable
    {
        IOwnerRepository Owners { get; }
        Task Save();
    }
}
