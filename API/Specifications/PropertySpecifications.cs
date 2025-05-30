using API.DTOs.PropertyDtos;
using API.Models;

namespace API.Specifications
{
    public class PropertySpecifications : BaseSpecification<Property>
    {
        public PropertySpecifications(PropertySpecificationParamsDto propertyParams)
            : base(p =>
                (string.IsNullOrEmpty(propertyParams.Search) || p.Address.ToLower().Contains(propertyParams.Search) ||
                 p.City.ToLower().Contains(propertyParams.Search)) &&
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
