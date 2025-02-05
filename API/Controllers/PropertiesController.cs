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
    public class PropertiesController(IUnitOfWork unitOfWork, IMapper mapper,UserManager<AppUser> userManager) : ControllerBase
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
            var properties = await unitOfWork.Properies.GetAllAsync("Type", "Type.Category");
            var propertiesToReturn = mapper.Map<List<GetPropertiesDto>>(properties);
            return Ok(propertiesToReturn);
        }
    }
}
