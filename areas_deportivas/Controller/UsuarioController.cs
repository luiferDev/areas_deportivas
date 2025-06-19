using areas_deportivas.Models;
using areas_deportivas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using areas_deportivas.DbContext;
using areas_deportivas.Models.DTO;

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
	
	[HttpPost("cancelar")]
	[Authorize(Roles = "User")]
	public async Task<IActionResult> CancelarReservacionAsync ([FromQuery] Guid reservaId)
	{
		try
		{
			await _usuarioService.CancelarAreaAsync(reservaId);
			return Ok(new
			{
				message = "Reservación cancelada exitosamente."
			});

		}
		catch (Exception ex)
		{
			// Manejar cualquier error que ocurra durante el proceso de reserva
			Console.WriteLine(ex.Message);
			return BadRequest("Error al reservar el área deportiva.");
		}
	}
	
	[HttpGet("user")]
	public IActionResult GetUserInfo([FromQuery] string email)
	{
		var user = _context.Usuarios.FirstOrDefault(u => u.Email == email);
		if (user == null)
		{
			return NotFound("Usuario no encontrado.");
		}

		var userId = user.Id;
		var userName = user.Nombre;
		var userEmail = user.Email;
		var userRole = user.Role.ToString();

		var userInfo = new
		{
			Id = userId,
			Nombre = userName,
			Email = userEmail,
			Rol = userRole
		};

		return Ok(userInfo);
	}
	
	[HttpGet("reservaciones")]
	[Authorize(Roles = "User")]
	public Task<IActionResult> GetReservasByUserAsync([FromQuery] Guid userId)
	{
		try
		{
			var reservas =  _context.Reservas;
			var reservasUsuario = from reserva in reservas
				join area in _context.AreaDeportivas on reserva.IdAreaDeportiva equals area.Id
				where reserva.IdUsuario == userId
				select new
				{
					Reserva = new
					{
						reserva.Id,
						reserva.Fecha,
						reserva.HoraInicio,
						reserva.HoraFin,
						Estado = reserva.EstadoReserva.ToString(),
					},
					AreaDeportiva = new
					{
						area.Id,
						area.Nombre,
						area.Description,
						area.TipoArea,
						area.Disponibilidad,
						area.ImageUrl,
						area.Precio
					}
				};

			return !reservasUsuario.Any() 
				? Task.FromResult<IActionResult>(NotFound("No se encontraron reservas para el usuario.")) 
				: Task.FromResult<IActionResult>(Ok(reservasUsuario));
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return Task.FromResult<IActionResult>(BadRequest("Error al obtener las reservas del usuario."));
		}
	}
}
