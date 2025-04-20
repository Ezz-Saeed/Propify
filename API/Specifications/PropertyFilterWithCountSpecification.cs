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
                (!propertyParams.MaxPrice.HasValue || p.Price <= propertyParams.MaxPrice) &&
                (!propertyParams.MinPrice.HasValue || p.Price >= propertyParams.MinPrice) &&
                (!propertyParams.BedRooms.HasValue || p.BedRooms == propertyParams.BedRooms) &&
                (!propertyParams.BathRooms.HasValue || p.BathRooms == propertyParams.BathRooms) &&
                (propertyParams.Area == null ? true : false || p.Area == propertyParams.Area) &&
                (!p.IsDeleted)
            )
        {
            
        }
    }
}
