using System;

namespace Building.PropertyAPI.Core.Entities
{
    public class PropertyImage
    {
        public Guid IdPropertyImage { get; set; }
        public string File { get; set; }
        public bool Enabled { get; set; }
        public Guid PropertyId { get; set; }
        //public Property Property { get; set; }
    }
}
