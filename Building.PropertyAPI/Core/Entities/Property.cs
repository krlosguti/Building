using System;
using System.Collections.Generic;

namespace Building.PropertyAPI.Core.Entities
{
    public class Property
    {
        /// <summary>
        /// Identifier of the property
        /// </summary>
        public Guid IdProperty { get; set; }
        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Address of the property
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Price of the property
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        /// Code Internal of the property
        /// </summary>
        public string CodeInternal { get; set; }
        /// <summary>
        /// Year of building of the property
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Identifier of the property owner
        /// </summary>
        public Guid IdOwner { get; set; }
        /// <summary>
        /// List of images of the property
        /// </summary>
        public ICollection<PropertyImage> PropertyImages { get; set; }

    }
}
