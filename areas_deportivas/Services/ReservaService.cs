
using areas_deportivas.Models;
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
	public Task CancelarReserva()
	{
		throw new NotImplementedException();
	}

	public async Task Reservar(CrearReservaDto crearReserva)
	{
		try
		{
			// Assuming crearReserva contains the user ID, otherwise you need to pass it as a parameter
			var userId = crearReserva.IdUsuario;
			var reserva = new Reserva
			{
				Fecha = crearReserva.Fecha,
				HoraInicio = crearReserva.HoraInicio,
				HoraFin = crearReserva.HoraFin,
				IdUsuario = userId,
				IdAreaDeportiva = crearReserva.IdAreaDeportiva
			};

			await _context.Reservas.AddAsync(reserva);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}