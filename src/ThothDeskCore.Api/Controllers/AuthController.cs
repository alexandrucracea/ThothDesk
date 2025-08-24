using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThothDeskCore.Api.DTOs.Auth;
using ThothDeskCore.Api.Services;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            ITokenService tokenService) : ControllerBase
{


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email, FullName = request.FullName };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var signIn = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signIn.Succeeded)
        {
            return Unauthorized();
        }

        var token = tokenService.GenerateToken(user);

        return Ok(new AuthResponse(token));
    }


}

