using API.DTOs;
using API.DTOs.PropertyDtos;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class PropertiesController(IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager, IImageService imageService) : ControllerBase
    {
        [HttpPost("addProperty")]
        [Authorize(Roles ="Owner")]
        public async Task<IActionResult> AddProperty([FromForm]AddPropertyDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(User.GetUserEmail());
            if(user is null) return Unauthorized("Unauthorized user!");

            var property = mapper.Map<Property>(dto);
            
            user.Properties!.Add(property);
            var type = await unitOfWork.Types.FindAsync(t => t.Id == property.TypeId, "Category");
            var propertyToReturn = mapper.Map<GetPropertiesDto>(property);
            propertyToReturn.TypeId = property.TypeId;
            propertyToReturn.TypeName = type.Name;
            propertyToReturn.CategoryName = type.Category.Name;
            return Ok(propertyToReturn);
        }

        [AllowAnonymous]
        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties([FromQuery]PropertySpecificationParamsDto paramsDto)
        {
            var spec = new PropertySpecifications(paramsDto);
            var countSpec = new PropertyFilterWithCountSpecification(paramsDto);
            var properties = await unitOfWork.Properies.GetAllAsync(spec);
            var totalCount = await unitOfWork.Properies.CountAsync(countSpec);
            var propertiesToReturn = mapper.Map<IReadOnlyList<GetPropertiesDto>>(properties);
            var result = new PaginatedResultDto<GetPropertiesDto>(paramsDto.PageNumber, paramsDto.PageSize, 
                totalCount, propertiesToReturn);
            return Ok(result);
        }

        [HttpGet("getProperty/{propertyId}")]
        public async Task<IActionResult> GetPropertyWithId(int propertyId)
        {
            var property = await unitOfWork.Properies.FindAsync(p => p.Id == propertyId, "Type", "Type.Category", "Images");
            if (property is null) return BadRequest(new { Message = "Invalid property ID!" });
            return Ok(mapper.Map<GetPropertiesDto>(property));
        }


        [HttpGet("ownerProperties")]
        public async Task<IActionResult> GetPropertiesForOwner([FromQuery] PropertySpecificationParamsDto paramsDto)
        {
            var userId = User.GetUserId();
            paramsDto.OwnerId = userId;
            var spec = new PropertySpecifications(paramsDto);
            var countSpec = new PropertyFilterWithCountSpecification(paramsDto);
            var properties = await unitOfWork.Properies.GetAllAsync(spec);
            var totalCount = await unitOfWork.Properies.CountAsync(countSpec);
            var propertiesToReturn = mapper.Map<List<GetPropertiesDto>>(properties);
            //var result = new PaginatedResultDto<GetPropertiesDto>(paramsDto.PageNumber, paramsDto.PageSize,
            //    totalCount, propertiesToReturn);
            return Ok(propertiesToReturn);
        }

        [HttpPut("updateProperty/{id}")]
        public async Task<ActionResult> UpdateProperty(int id,  UpdatePropertyDto dto)
        {
            var property = await unitOfWork.Properies.FindAsync(p=>p.Id==id);
            if(property is null) return BadRequest("Invalid property Id!");
            if(dto.Description != property.Description)
                property.Description = dto.Description;
            if (dto.Address != property.Address)
                property.Address = dto.Address;
            if (dto.Area != property.Area)
                property.Area = dto.Area;
            if (dto.BedRooms != property.BedRooms)
                property.BedRooms = dto.BedRooms;
            if (dto.BathRooms != property.BathRooms)
                property.BathRooms = dto.BathRooms;
            if (dto.City != property.City)
                property.City = dto.City;
            if (dto.Price != property.Price)
                property.Price = dto.Price;
            if (dto.IsAvailable != property.IsAvailable)
                property.IsAvailable = dto.IsAvailable;
            if (dto.IsRental != property.IsRental)
                property.IsRental = dto.IsRental;
            if (dto.TypeId != property.TypeId)
                property.TypeId = dto.TypeId;
            unitOfWork.Properies.Update(property);
            await unitOfWork.Dispose();
            var updatedProperty = await unitOfWork.Properies.FindAsync(p => p.Id == id, "Type", "Type.Category", "Images");
            var propertyToReturn = mapper.Map<GetPropertiesDto>(updatedProperty);
            return Ok(propertyToReturn);
        }

        [HttpPut("deleteProperty/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await unitOfWork.Properies.FindAsync(p => p.Id == id);
            if (property is null) return NotFound();
            property.IsDeleted = true;
            unitOfWork.Properies.Update(property);
            await unitOfWork.Dispose();
            return Ok();
        }

        [HttpPost("uploadImage/{propertyId}")]
        public async Task<IActionResult> UploadImage(IFormFile file, int propertyId)
        {
            var property = await unitOfWork.Properies.FindAsync(p=>p.Id==propertyId, "Images");

            var result = await imageService.UploadImageAsync(file);
            if(result.Error is not null) return BadRequest(new {Message= result.Error.Message});

            PropertyImage image = new()
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            if(property.Images!.Count == 0)
                image.IsMain = true;
            property.Images.Add(image);
            unitOfWork.Properies.Update(property);
            if (!await unitOfWork.Dispose())
                return BadRequest(new { Message = "Couldn't upload image!" });

            return Ok(mapper.Map<PropertyImageDto>(image));
        }

        
        [HttpDelete("deleteImage/{propertyId}")]
        public async Task<IActionResult>DeleteImage(int propertyId,[FromQuery] string publicId)
        {
            var property = await unitOfWork.Properies.FindAsync(p=>p.Id==propertyId, "Images");
            if (property is null) return NotFound();
            if(property.Images?.Count == 0) return BadRequest("No images");
            var image = property.Images!.FirstOrDefault(p=>p.PublicId== publicId);
            if(image is  null) return NotFound();
            var result = await imageService.DeleteImageAsync(image.PublicId!);
            if(result.Error is not null) return BadRequest(result.Error.Message);
            property.Images!.Remove(image);
            unitOfWork.Properies.Update(property);
            await unitOfWork.Dispose();
            return Ok();
        }

        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTypes()
        {
            var spec = new TypeSpecification();
            var types = mapper.Map<List<TypeDto>>(await unitOfWork.Types.GetAllAsync(spec));
            return Ok(types);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await unitOfWork.Categories.GetAllAsync(null);
            return Ok(categories);
        }
    }
}
