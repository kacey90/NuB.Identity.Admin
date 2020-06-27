using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuBIdentity.STS.Identity.Helpers;
using NuBIdentity.STS.Identity.ViewModels.Account.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuBIdentity.STS.Identity.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/AccountAPI")]
    public class AccountAPIController<TUser, TKey, TRole> : ControllerBase
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserResolver<TUser> _userResolver;
        private readonly UserManager<TUser> _userManager;
        private readonly RoleManager<TRole> _roleManager;

        public AccountAPIController(
            UserResolver<TUser> userResolver,
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager)
        {
            _userManager = userManager;
            _userResolver = userResolver;
            _roleManager = roleManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(SecurityFilterAttribute))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserInputModel model)
        {
            var newUser = new TUser
            {
                Id = (TKey)Convert.ChangeType(model.Id, typeof(TKey)),
                UserName = model.Login,
                Email = model.Email,
                EmailConfirmed = model.IsActive,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = false
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                foreach (var roleName in model.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var role = new TRole
                        {
                            Name = roleName
                        };
                        _ = await _roleManager.CreateAsync(role);
                    }
                }

                // add user to role
                await _userManager.AddToRolesAsync(newUser, model.Roles);

                return Ok(newUser);
            }

            return BadRequest(result.Errors);
        }
    }
}
