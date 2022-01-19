using Building.PropertyAPI.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetProperties();
    }
}
