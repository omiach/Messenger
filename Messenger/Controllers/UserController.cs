﻿using MessengerData.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MessengerData.Extensions;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserProvider _provider;
        public UserController(UserProvider provider)
        {
            _provider = provider;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var guidString = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (guidString == null)
            {
                return StatusCode(500);
            }

            var tt = await _provider.GetRepository().GetDbContext().Users.Where(x => x.Guid == new Guid(guidString)).Include(x => x.UserChats).Include(x => x.Contacts).FirstOrDefaultAsync();

            User? user = await _provider.GetRepository()
                .FirstOrDefaultAsync(x => x.Guid == new Guid(guidString)
                , x => x.Include(y => y.UserChats)
                        .Include(y => y.Contacts));
               
            if (user == null)
            {
                return StatusCode(500);
            }

            return Ok(_provider.ToUserDTO(user));
        
        }

        [Authorize]
        [HttpGet("~/GetContacts")]
        [ActionName("GetContacts")]
        public async Task<IActionResult> GetContacts(string? name, string? firstname, string? lastname, string? phonenumber,string? orderby, int pageindex = 0, int pagesize = 20)
        {

            Expression<Func<User, bool>> filter = (x) =>      
                name == null ? true : x.Name.Contains(name)
                && firstname == null ? true : x.FirstName.Contains(firstname!)
                && lastname == null ? true : x.LastName.Contains(lastname!)
                && phonenumber == null ? true : x.PhoneNumber.Contains(phonenumber!);

            Func<IQueryable<User>, IOrderedQueryable<User>>? order = 
                orderby == null 
                ? null 
                : (x) => x.OrderBy(orderby);

            User[] users = await _provider.GetRepository().Get(filter, order).ToArrayAsync();

            return Ok(_provider.ToContactDTO(users));

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewUserDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _provider.CreateUserAsync(newUserDTO);
            if (result.Result)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserDTO updateUserDTO)
        {
            string? userGuidString = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userGuidString == null)
            {
                return StatusCode(500);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _provider.UpdateUserAsync(new Guid(userGuidString), updateUserDTO);
            if (result.Result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }

        }

    }
}
