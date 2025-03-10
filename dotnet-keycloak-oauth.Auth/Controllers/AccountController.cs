using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_keycloak_oauth.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [Authorize]
        [HttpGet("/users/me")]
        public IActionResult GetUser()
        {
            var claimsPrincipal = HttpContext.User; // Fetch claims from context
            var claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);

            return Ok(claims);
        }
    }
}
