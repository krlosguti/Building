using Building.OwnersAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetOwners();
        Task<PaginationEntity> PaginationBy(
            Expression<Func<bool>> filterExpression,
            PaginationEntity pagination);

    }
}
