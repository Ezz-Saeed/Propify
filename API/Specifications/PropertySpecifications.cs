using API.DTOs.PropertyDtos;
using API.Models;

namespace API.Specifications
{
    public class PropertySpecifications : BaseSpecification<Property>
    {
        public PropertySpecifications(PropertySpecificationParamsDto propertyParams)
            : base(p =>
                (string.IsNullOrEmpty(propertyParams.Search) || p.Address.ToLower() == propertyParams.Search) &&
                (!propertyParams.TypeId.HasValue || p.TypeId == propertyParams.TypeId) && 
                (!propertyParams.CategoryId.HasValue || p.Type.CategoryId == propertyParams.CategoryId) &&
                (string.IsNullOrEmpty(propertyParams.OwnerId) || p.AppUserId == propertyParams.OwnerId) &&
                (!p.IsDeleted)
            )
        {
            AddEager(p=>p.Type);
            AddEager(p=>p.Type.Category);
            AddEager(p=>p.Images);
            AddOrderBy(p=>p.Price);

            ApplyPaging(propertyParams.PageSize * (propertyParams.PageNumber - 1), propertyParams.PageSize);
        }

        public PropertySpecifications(int id):base(p=>p.Id == id)
        {
            AddEager(p => p.Type);
            AddEager(p => p.Type.Category);
            AddEager(p => p.Images);
        }
    }
}
