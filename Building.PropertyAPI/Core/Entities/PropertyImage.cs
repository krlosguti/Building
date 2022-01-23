using System;

namespace Building.PropertyAPI.Core.Entities
{
    public class PropertyImage
    {
        //identifier of an image
        public Guid IdPropertyImage { get; set; }
        //path image file in the local server
        public string File { get; set; }
        //image is enabled to show
        public bool Enabled { get; set; }
        //identifier of the property that the image represent
        public Guid IdProperty { get; set; }
        //relationship with the property
        public Property Property { get; set; }
    }
}
