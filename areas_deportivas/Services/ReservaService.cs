
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Services;

public class ReservaService : IReservaService
{
	private readonly DeportesDbContext _context;

	public ReservaService(DeportesDbContext context)
	{
		_context = context;
	}

	public Task CancelarReservaAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<ReservaRespuestaDto> ReservarAsync(CrearReservaDto crearReserva, int Id, Guid userId)
	{
		var area = _context.AreaDeportivas.FirstOrDefault(a => a.Id == Id) ?? throw new Exception("Area no encontrada");

		var reservar = new Reserva
		{
			Id = Guid.NewGuid(),
			Fecha = crearReserva.Fecha,
			HoraInicio = crearReserva.HoraInicio,
			HoraFin = crearReserva.HoraFin,
			EstadoReserva = Estado.PENDIENTE,
			IdUsuario = userId,
			IdAreaDeportiva = area.Id
		};

		await _context.Reservas.AddAsync(reservar);
		await _context.SaveChangesAsync();

		return new ReservaRespuestaDto
		{
			AreaDeportiva = new AreaDeportivaDto
			{
				Id = area.Id,
				Nombre = area.Nombre,
				TipoArea = area.TipoArea.ToString(),
				Disponibilidad = area.Disponibilidad
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