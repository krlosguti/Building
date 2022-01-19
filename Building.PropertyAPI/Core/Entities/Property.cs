using System;
using System.Collections.Generic;

namespace Building.PropertyAPI.Core.Entities
{
    public class Property
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public Guid IdOwner { get; set; }
        public ICollection<PropertyImage> PropertyImages { get; set; }

    }
}
