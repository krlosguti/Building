using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Building.OwnersAPI.Core.Entities
{
    public class FilterParameters<T> where T : class
    {
        public Expression<Func<Owner, bool>> expression { get; set; } = null;
        public Func<IQueryable<Owner>, IOrderedQueryable<Owner>> orderBy { get; set; } = null;
        public List<string> includes { get; set; } = null;
        public int MaxPageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
