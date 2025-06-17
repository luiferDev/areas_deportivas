using areas_deportivas.Models;
using areas_deportivas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace areas_deportivas.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
	private readonly DeportesDbContext _context;
	private readonly IUsuarioService _usuarioService;

	public UsuarioController(DeportesDbContext context, IUsuarioService usuarioService)
	{
		_context = context;
		_usuarioService = usuarioService;
	}

	[HttpPost("reservar")]
	[Authorize(Roles = "User")]
	public async Task<IActionResult> ReservarAreaAsync([FromQuery] int Id, [FromBody] CrearReservaDto crearReserva)
	{
		try
		{
			var area = _context.AreaDeportivas.FirstOrDefault(a => a.Id == Id);
			if (area == null)
			{
				return NotFound("Área deportiva no encontrada.");
			}
			// Obtener el ID del usuario del token JWT
			var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!Guid.TryParse(userIdStr, out var userId))
				return BadRequest("Usuario no identificado.");

			if (string.IsNullOrEmpty(userIdStr))
			{
				return BadRequest("No se pudo identificar al usuario.");
			}

			var reserva = await _usuarioService.ReservarAreaAsync(crearReserva, Id, userId);

			var respuesta = new
			{
				reserva.AreaDeportiva,
				reserva.Reserva,
				Usuario = new
				{
					Id = userId,
					Email = User.FindFirst(ClaimTypes.Email)?.Value,
					Rol = User.FindFirst(ClaimTypes.Role)?.Value
				}
			};

			return Ok(respuesta);

		}
		catch (Exception ex)
		{
			// Manejar cualquier error que ocurra durante el proceso de reserva
			Console.WriteLine(ex.Message);
			return BadRequest("Error al reservar el área deportiva.");
		}
	}
}
