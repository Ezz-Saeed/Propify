using System.Linq.Expressions;

namespace API.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Eagers { get; private set; } =
            new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddEager(Expression<Func<T, object>> eager) { Eagers.Add(eager); }
        protected void AddOrderBy(Expression<Func<T, object>> orderBy) { OrderBy = orderBy; }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending) 
        { OrderByDescending = orderByDescending; }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
