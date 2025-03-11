﻿using API.DTOs.PropertyDtos;
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

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var types = mapper.Map<List<TypeDto>>(await unitOfWork.Types.GetAllAsync(p => true, "Category"));
            return Ok(types);
        }
    }
}
