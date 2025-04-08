using BlogApi.Models.Dtos;
using BlogApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid input data.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var result = await _userService.RegisterUserAsync(userDto);
                if (!result)
                    return BadRequest(new { Success = false, Message = "User registration failed." });

                return Ok(new { Success = true, Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while registering the user.", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid input data.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var authResponse = await _userService.AuthenticateUserAsync(userDto.Email, userDto.Password);
                if (authResponse == null)
                    return Unauthorized(new { Success = false, Message = "Invalid credentials." });

                return Ok(new
                {
                    Success = true,
                    Message = "Login successful.",
                    Token = authResponse.Token,
                    User = new { authResponse.UserId, authResponse.Email, authResponse.Role }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while logging in.", Error = ex.Message });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized(new { Success = false, Message = "No se encontró el token JWT en la cookie." });
                }

                var token = _jwtService.Verify(jwt);
                if (token == null)
                {
                    return Unauthorized(new { Success = false, Message = "Token inválido o expirado." });
                }

                int userId = int.Parse(token.Issuer);

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { Success = false, Message = "Usuario no encontrado." });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Usuario obtenido correctamente.",
                    Data = new
                    {
                        user.Id,
                        user.Email,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error al obtener el usuario.", Error = ex.Message });
            }
        }

    }
}
