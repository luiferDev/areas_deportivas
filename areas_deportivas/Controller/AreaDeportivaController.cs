using areas_deportivas.DbContext;
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Controller;

[ApiController]
[Route("api/[controller]")]
public class AreaDeportivaController : ControllerBase
{
	private readonly DeportesDbContext _context;
	private readonly IOutputCacheStore _outputCacheStore;

	public AreaDeportivaController(DeportesDbContext context, IOutputCacheStore outputCacheStore)
	{
		_context = context;
		_outputCacheStore = outputCacheStore;
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
				Description = createAreaDeportiva.Description,
				ImageUrl = createAreaDeportiva.ImageUrl,
				TipoArea = createAreaDeportiva.TipoArea,
				Disponibilidad = true,
				Precio = createAreaDeportiva.Precio
			};

			_context.AreaDeportivas.Add(areaDeportiva);
			await _context.SaveChangesAsync();
			await _outputCacheStore.EvictByTagAsync("areas", CancellationToken.None);

			return Ok(areaDeportiva);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
	
	[HttpGet]
	[OutputCache(PolicyName = "areas")]
	public async Task<IActionResult> ObtenerAreasDeportivasAsync()
	{
		try
		{
			var areasDeportivas = await _context.AreaDeportivas.ToListAsync();
			return Ok(areasDeportivas);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}