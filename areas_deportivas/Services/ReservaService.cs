
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Services;

public class ReservaService : IReservaService
{
	private readonly DeportesDbContext _context;
	private readonly IAreaDeportivaService _areaDeportivaService;
	
	public ReservaService(DeportesDbContext context, IAreaDeportivaService areaDeportivaService)
	{
		_context = context;
		_areaDeportivaService = areaDeportivaService;
	}

	public async Task CancelarReservaAsync(Guid reservaId)
	{
		var reserva = await _context.Reservas.FindAsync(reservaId) ?? 
			throw new Exception("Reserva no encontrada");
		
		int areaId = reserva.IdAreaDeportiva;
		reserva.EstadoReserva = Estado.CANCELADA;
		
		await _context.SaveChangesAsync();
		
		// Actualizar la disponibilidad del área después de cancelar la reserva
		await _areaDeportivaService.ActualizarDisponibilidadAsync(areaId);
	}

	public async Task<ReservaRespuestaDto> ReservarAsync(CrearReservaDto crearReserva, int Id, Guid userId)
	{
		var area = _context.AreaDeportivas.FirstOrDefault(a => a.Id == Id) ?? throw new Exception("Area no encontrada");
		
		// Verificar si ya existe una reserva activa para esta área
		var reservaExistente = await _context.Reservas
			.AnyAsync(r => r.IdAreaDeportiva == Id && 
				r.Fecha == crearReserva.Fecha && 
				r.EstadoReserva != Estado.CANCELADA);
				
		if (reservaExistente)
		{
			throw new Exception("El área deportiva ya está reservada para esta fecha");
		}

		var reservar = new Reserva
		{
			Id = Guid.NewGuid(),
			Fecha = crearReserva.Fecha,
			HoraInicio = crearReserva.HoraInicio,
			HoraFin = crearReserva.HoraFin,
			EstadoReserva = Estado.CONFIRMADA,
			IdUsuario = userId,
			IdAreaDeportiva = area.Id
		};

		await _context.Reservas.AddAsync(reservar);
		await _context.SaveChangesAsync();
		
		// Actualizar la disponibilidad del área después de crear la reserva
		await _areaDeportivaService.ActualizarDisponibilidadAsync(Id);

		return new ReservaRespuestaDto
		{
			AreaDeportiva = new AreaDeportivaDto
			{
				Id = area.Id,
				Nombre = area.Nombre,
				Description = area.Description,
				TipoArea = area.TipoArea.ToString(),
				Disponibilidad = area.Disponibilidad,
				Precio = area.Precio
			},
			Reserva = new ReservaDto
			{
				Fecha = reservar.Fecha,
				HoraInicio = reservar.HoraInicio,
				HoraFin = reservar.HoraFin,
				EstadoReserva = reservar.EstadoReserva.ToString(),
				IdAreaDeportiva = reservar.IdAreaDeportiva
			}
		};
	}
}