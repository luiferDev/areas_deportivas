using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace areas_deportivas.Controller;

[ApiController]
[Route("api/[controller]")]
public class AreaDeportivaController : ControllerBase
{
	private readonly DeportesDbContext _context;

	public AreaDeportivaController(DeportesDbContext context)
	{
		_context = context;
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> CrearAreaDeportivaAsync([FromBody] CreateAreaDeportivaDto createAreaDeportiva)
	{
		try
		{
			var areaDeportiva = new AreaDeportiva
			{
				Nombre = createAreaDeportiva.Nombre,
				TipoArea = createAreaDeportiva.TipoArea,
				Disponibilidad = true
			};

			_context.AreaDeportivas.Add(areaDeportiva);
			await _context.SaveChangesAsync();

			return Ok(areaDeportiva);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}