

using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using areas_deportivas.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace areas_deportivas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] LoginUserDto login)
	{
		var token = await authService.LoginUserAsync(login.Email, login.Password);
		if (token == null)
			return Unauthorized(new { Message = "Credenciales inválidas." });

		return Ok(new { Token = token });
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto request)
	{

		var success = await authService
			.RegisterUserAsync(request);
		if (!success)
			return BadRequest(new { Message = "El usuario ya existe." });

		return Ok(new { Message = "Usuario registrado exitosamente." });
	}

	[HttpPost("register-admin")]
	public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDto request)
	{

		var success = await authService.RegisterAdminAsync(request);
		if (!success)
			return BadRequest(new { Message = "El usuario ya existe." });

		return Ok(new { Message = "Usuario registrado exitosamente." });
	}

	[HttpPost("register-employee")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterDto request)
	{

		var success = await authService.EmployeeRegisterAsync(request);
		if (!success)
			return BadRequest(new { Message = "El empleado ya existe en la base de datos." });

		return Ok(new { Message = "Empleado registrado exitosamente." });
	}
}