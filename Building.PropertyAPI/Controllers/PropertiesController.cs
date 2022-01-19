using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertiesController(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        [HttpGet("properties")]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            var properties = await _propertyRepository.GetProperties();
            return Ok(properties);
        }
    }
}
