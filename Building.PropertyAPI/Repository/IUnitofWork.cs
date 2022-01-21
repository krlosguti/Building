using System;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public interface IUnitofWork : IDisposable
    {
        IPropertyRepository Properties { get; }
        Task Save();
    }
}
