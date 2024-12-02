using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinylStore.Api.Persistence;
using VinylStore.Api.Persistence.Data;
using VinylStore.Api.Persistence.Entities;
using VinylStore.Api.Services;
using VinylStore.Shared.AuthModel;

namespace VinylStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly VinylStoreContext _context;

        public AuthController(IUserService userService, IJwtService jwtService, VinylStoreContext context)
        {
            _userService = userService;
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register(MyRegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return BadRequest("Passwords don't match");

            try
            {
                var user = await _userService.RegisterAsync(request);
                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                await _userService.UpdateRefreshTokenAsync(user, refreshToken);

                return Ok(new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = DateTime.UtcNow.AddMinutes(15)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            if (!await _userService.ValidateUserCredentialsAsync(request.Email, request.Password))
                return Unauthorized("Invalid credentials");

            var user = await _userService.GetUserByEmailAsync(request.Email);
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(user, refreshToken);

            return Ok(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = DateTime.UtcNow.AddMinutes(15)
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.RefreshToken == refreshToken &&
                u.RefreshTokenExpiryTime > DateTime.UtcNow);

            if (user == null)
                return BadRequest("Invalid refresh token");

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(user, newRefreshToken);

            return Ok(new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = DateTime.UtcNow.AddMinutes(15)
            });
        }
    }
}
