using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/thothdesk/v1/admin/users")]
public class AdminUsersController(UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager) : ControllerBase
{

    [HttpPost("{userId}/roles/{roleName}")]
    public async Task<IActionResult> AddRole(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound("User was not found");
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var result = await userManager.AddToRoleAsync(user, roleName);

        return result.Succeeded ? NoContent() : BadRequest(result.Errors);
    }

    [HttpGet]
    public IActionResult List() => Ok(userManager.Users.Select(user => new
    {
        user.Id,
        user.Email,
        user.UserName
    }));

    [HttpDelete]
    public async Task<IActionResult> DeleteRole(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        var result = await userManager.RemoveFromRoleAsync(user, roleName);

        return result.Succeeded ? NoContent() : BadRequest(result.Errors);

    }
}

