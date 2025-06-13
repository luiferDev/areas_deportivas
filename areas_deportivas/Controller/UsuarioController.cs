using areas_deportivas.Models;
using Microsoft.AspNetCore.Mvc;

namespace areas_deportivas.Controller;

public class UsuarioController : ControllerBase
{
	private readonly DeportesDbContext _context;

	public UsuarioController(DeportesDbContext context)
	{
		_context = context;
	}

	[HttpPost]
	public async Task<IActionResult> ReservarAreaDepostiva()
	{
		return Ok();
	}

}
