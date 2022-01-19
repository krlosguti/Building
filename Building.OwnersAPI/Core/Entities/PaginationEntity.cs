using System.Collections.Generic;

namespace Building.OwnersAPI.Core.Entities
{
    public class PaginationEntity
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
        public string Sort { get; set; }
        public string SortDirection { get; set; } = "desc";
        public string Filter { get; set; }
        public int PagesQuantity { get; set; }
        public IEnumerable<Owner> Owners {get;set;}

    }
}
