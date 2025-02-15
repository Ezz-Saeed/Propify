using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("addPropert")]      
        public async Task<IActionResult> AddProperty(AddPropertyDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(User.GetUserEmail());
            if(user is null) return Unauthorized("Unauthorized user!");

            var property = mapper.Map<Property>(dto);
            user.Properties!.Add(property);
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
            
            return Ok(property);
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await unitOfWork.Properies.GetAllAsync("Type", "Type.Category", "Images");
            var propertiesToReturn = mapper.Map<List<GetPropertiesDto>>(properties);
            return Ok(propertiesToReturn);
        }

        [HttpPost("uploadImage/{propertyId}")]
        public async Task<IActionResult> UploadImage(IFormFile file, int propertyId)
        {
            //var user = await userManager.FindByEmailAsync(User.GetUserEmail());
            var property = await unitOfWork.Properies.FindAsync(propertyId, "Images");

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
    }
}
