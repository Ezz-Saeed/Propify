using API.Models;

namespace API.Specifications
{
    public class TypeSpecification : BaseSpecification<PropertyType>
    {
        public TypeSpecification()
        {
            AddEager(t => t.Category);
        }
    }
}
