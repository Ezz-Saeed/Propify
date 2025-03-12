using API.DTOs.PropertyDtos;
using API.Extensions;
using API.Interfaces;
using API.Models;
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
    [Authorize]
    public class PropertiesController(IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager, IImageService imageService) : ControllerBase
    {
        [HttpPost("addProperty")]      
        public async Task<IActionResult> AddProperty([FromForm]AddPropertyDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(User.GetUserEmail());
            if(user is null) return Unauthorized("Unauthorized user!");

            var property = mapper.Map<Property>(dto);
            
            user.Properties!.Add(property);
            await userManager.AddToRolesAsync(user, ["Owner"]);
            var reult = await userManager.UpdateAsync(user);

            if(!reult.Succeeded)
            {
                var errors = string.Empty;
                foreach(var error in reult.Errors)
                {
                    errors += $"{error}, ";
                }
                return BadRequest(errors);
            }
            var propertyToReturn = mapper.Map<GetPropertiesDto>(property);
            return Ok(propertyToReturn);
        }

        [AllowAnonymous]
        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await unitOfWork.Properies.GetAllAsync(p=>true,"Type", "Type.Category", "Images");
            var propertiesToReturn = mapper.Map<List<GetPropertiesDto>>(properties);
            return Ok(propertiesToReturn);
        }

        [HttpGet("getProperty/{propertyId}")]
        public async Task<IActionResult> GetPropertyWithId(int propertyId)
        {
            var property = await unitOfWork.Properies.FindAsync(p => p.Id == propertyId, "Type", "Type.Category", "Images");
            if (property is null) return BadRequest(new { Message = "Invalid property ID!" });
            //var propertyToReturn = 
            return Ok(mapper.Map<GetPropertiesDto>(property));
        }


        [Authorize(Roles ="Owner")]
        [HttpGet("ownerProperties")]
        public async Task<IActionResult> GetPropertiesForOwner()
        {
            var userId = User.GetUserId();
            var properties = await unitOfWork.Properies.GetAllAsync(p => p.AppUserId==userId, "Type", "Type.Category", "Images");
            var propertiesToReturn = mapper.Map<List<GetPropertiesDto>>(properties);
            return Ok(propertiesToReturn);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("updateProperty/{id}")]
        public async Task<ActionResult> UpdateProperty(int id,  UpdatePropertyDto dto)
        {
            var property = await unitOfWork.Properies.FindAsync(p=>p.Id==id);
            if(property is null) return BadRequest("Invalid property Id!");
            property.Description = dto.Description;
            property.Address = dto.Address;
            property.Area = dto.Area;
            property.BedRooms = dto.BedRooms;
            property.BathRooms = dto.BathRooms;
            property.City = dto.City; 
            property.Price = dto.Price;
            property.IsAvailable = dto.IsAvailable;
            property.IsRental = dto.IsRental;
            property.TypeId = dto.TypeId;
            unitOfWork.Properies.Update(property);
            await unitOfWork.Dispose();
            return Ok(property);
        }
        [Authorize(Roles = "Owner")]
        [HttpPost("uploadImage/{propertyId}")]
        public async Task<IActionResult> UploadImage(IFormFile file, int propertyId)
        {
            var property = await unitOfWork.Properies.FindAsync(p=>p.Id==propertyId, "Images");

            var result = await imageService.UploadImageAsync(file);
            if(result.Error is not null) return BadRequest(new {Message= result.Error.Message});

            Image image = new()
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

            return Ok(mapper.Map<ImageDto>(image));
        }

        [Authorize(Roles = "Owner")]
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
        public async Task<IActionResult> GetTypes()
        {
            var types = mapper.Map<List<TypeDto>>(await unitOfWork.Types.GetAllAsync(p => true, "Category"));
            return Ok(types);
        }
    }
}
