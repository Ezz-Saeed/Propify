using API.DTOs.PropertyDtos;
using API.Models;

namespace API.Specifications
{
    public class PropertyFilterWithCountSpecification : BaseSpecification<Property>
    {
        public PropertyFilterWithCountSpecification(PropertySpecificationParamsDto propertyParams)
            :base(p=>
                (string.IsNullOrEmpty(propertyParams.Search) || p.Address.ToLower() == propertyParams.Search) &&
                (!propertyParams.TypeId.HasValue || p.TypeId == propertyParams.TypeId) &&
                (!propertyParams.CategoryId.HasValue || p.Type.CategoryId == propertyParams.CategoryId) &&
                (string.IsNullOrEmpty(propertyParams.OwnerId) || p.AppUserId == propertyParams.OwnerId) &&
                (!p.IsDeleted)
            )
        {
            
        }
    }
}
