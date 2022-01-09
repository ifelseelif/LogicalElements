using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IOptions<TokenOptions> _tokenOption;
        private readonly IAuthService _authService;

        public AuthController(IOptions<TokenOptions> tokenOption, IAuthService authService)
        {
            _tokenOption = tokenOption;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromQuery, Required] string login,
            [FromQuery, Required] string password)
        {
            var operationResult = await _authService.Register(login, password);
            return operationResult.ToResponseMessage();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery, Required] string login,
            [FromQuery, Required] string password)
        {
            ;
            var result = await _authService.Login(login, password);
            if (!result.IsSuccess())
            {
                return result.ToResponseMessage();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, result.Value.Username),
                new(ClaimTypes.NameIdentifier, result.Value.Id.ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256)), new JwtPayload(claims));

            var response = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Ok(response);
        }
    }
}