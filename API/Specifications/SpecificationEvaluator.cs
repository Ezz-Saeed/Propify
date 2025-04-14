using Microsoft.EntityFrameworkCore;

namespace API.Specifications
{
    public class SpecificationEvaluator <T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;
            if(specification.Criteria is not null) query = query.Where(specification.Criteria);

            if(specification.OrderBy is not null) query = query.OrderBy(specification.OrderBy);

            if(specification.OrderByDescending is not null) query = query.OrderByDescending(specification.OrderByDescending);

            if(specification.IsPagingEnabled) query = query.Skip(specification.Skip).Take(specification.Take);

            if(specification.Eagers is not null)
                query = specification.Eagers.Aggregate(query, (current, eager)=>current.Include(eager));
            return query;
        }
    }
}
