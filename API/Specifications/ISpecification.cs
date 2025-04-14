using System.Linq.Expressions;

namespace API.Specifications
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T,bool>> Criteria { get; }
        List<Expression<Func<T, object>>>Eagers { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
