using System;

namespace Building.PropertyAPI.RemoteService.Entities
{
    public class OwnerRemote
    {
        /// <summary>
        /// Unique identifier of the owner
        /// </summary>
        public Guid IdOwner { get; set; }
        /// <summary>
        /// Name of the owner
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Address of the owner of the property
        /// </summary>
        public string Address { get; set; } = "";
        /// <summary>
        /// Localization in of the photo file on the server
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Owner's birthday
        /// </summary>
        public DateTime Birthday { get; set; } = DateTime.UtcNow;
    }
}
