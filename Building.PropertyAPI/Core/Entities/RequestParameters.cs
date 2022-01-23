namespace Building.PropertyAPI.Core.Entities
{
    public class PageParameters
    {
        /// <summary>
        /// Maxim number of records to retrieve
        /// </summary>
        public int MaxPageSize { get; set; } = 50;
        /// <summary>
        /// Actual page number
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Actual page size
        /// </summary>
        private int _pageSize = 10;
        /// <summary>
        /// Assign to _pagesize a validate value
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }

    public class FilterParameters
    {
        /// <summary>
        /// String used to find values in the fields Name, Address than contains search string
        /// </summary>
        public string Search { get; set; } = null;
        /// <summary>
        /// Field name to order
        /// </summary>
        public string orderBy { get; set; } = null;
        /// <summary>
        /// Order type: asc or desc
        /// </summary>
        public bool asc { get; set; } = true;
    }
    public class RequestParameters
    {
        //parameters for searching and ordering information
        public FilterParameters filterParameters { get; set; } = null;
        //parameters to paginate information
        public PageParameters pageParameters { get; set; } = null;
    }
}
