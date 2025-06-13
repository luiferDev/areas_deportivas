using areas_deportivas.Models;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Services;
public class AreaDeportivaService : IAreaDeportivaService
{
	private readonly DeportesDbContext _context;

	public AreaDeportivaService(DeportesDbContext context)
	{
		_context = context;
	}

	public async Task ActualizarDisponibilidadAsync(int areaId)
	{
		var area = await _context.AreaDeportivas.FindAsync(areaId) ?? 
			throw new Exception("Área deportiva no encontrada");
			
		// Verificar si hay reservas activas para esta área
		bool tieneReservasActivas = await _context.Reservas
			.AnyAsync(r => r.IdAreaDeportiva == areaId && 
				r.EstadoReserva != Estado.CANCELADA);
		
		// Actualizar disponibilidad según si hay reservas activas
		area.Disponibilidad = !tieneReservasActivas;
		
		await _context.SaveChangesAsync();
	}
	
	public async Task ActualizarTodasLasDisponibilidadesAsync()
	{
		// Obtener todas las áreas deportivas
		var areas = await _context.AreaDeportivas.ToListAsync();
		
		foreach (var area in areas)
		{
			// Verificar si hay reservas activas para esta área
			bool tieneReservasActivas = await _context.Reservas
				.AnyAsync(r => r.IdAreaDeportiva == area.Id && 
					r.EstadoReserva != Estado.CANCELADA);
			
			// Actualizar disponibilidad según si hay reservas activas
			area.Disponibilidad = !tieneReservasActivas;
		}
		
		await _context.SaveChangesAsync();
	}
}