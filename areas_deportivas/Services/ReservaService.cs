
using areas_deportivas.DbContext;
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using areas_deportivas.Models.Enums;
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

	public async Task EliminarReservaAsync(Guid reservaId)
	{
		var reserva = await _context.Reservas.FindAsync(reservaId)
		              ?? throw new Exception("Reserva no encontrada");

		switch (reserva.EstadoReserva)
		{
			case Estado.CONFIRMADA:
				throw new Exception("No se puede eliminar una reserva confirmada. Debes cancelarla antes.");

			case Estado.PENDIENTE:
				throw new Exception("No se puede eliminar una reserva pendiente. Debes cancelarla antes.");

			case Estado.CANCELADA:
				_context.Reservas.Remove(reserva);
				await _context.SaveChangesAsync();
				break;

			case Estado.NO_CONFIRMADA:
				throw new Exception("No se puede eliminar una reserva no confirmada. Debes cancelarla antes.");
			
			default:
				throw new Exception("Estado de reserva no válido para eliminación.");
		}
	}

	public async Task ActualizarReservaAsync(Guid reservaId, ActualizarReservaDto actualizarReserva)
	{
		var reserva = await _context.Reservas.FindAsync(reservaId)
		              ?? throw new Exception("Reserva no encontrada");

		// Guardamos valores originales por si los necesitamos
		var horaInicioOriginal = reserva.HoraInicio;
		var horaFinOriginal = reserva.HoraFin;

		// Calcular duración previa
		var duracionOriginal = TimeSpan.FromMinutes(
			(horaFinOriginal - horaInicioOriginal).TotalMinutes
		);

		// Aplicar cambios recibidos
		if (actualizarReserva.Fecha.HasValue)
			reserva.Fecha = actualizarReserva.Fecha.Value;

		if (actualizarReserva.HoraInicio.HasValue)
		{
			reserva.HoraInicio = actualizarReserva.HoraInicio.Value;
			// Si horaFin no se envió, ajustar con la duración original
			if (!actualizarReserva.HoraFin.HasValue)
				reserva.HoraFin = reserva.HoraInicio.Add(duracionOriginal);
		}

		if (actualizarReserva.HoraFin.HasValue)
			reserva.HoraFin = actualizarReserva.HoraFin.Value;

		// Validar que horaFin sea después de horaInicio
		if (reserva.HoraFin <= reserva.HoraInicio)
			throw new Exception("La hora de fin debe ser mayor que la hora de inicio");

		// Validar disponibilidad (puedes adaptar esta lógica según tus reglas de negocio)
		var hayConflicto = await _context.Reservas.AnyAsync(r =>
			r.Id != reserva.Id && // excluir la misma reserva
			r.Fecha == reserva.Fecha &&
			r.IdAreaDeportiva == reserva.IdAreaDeportiva && // asumiendo que reservas pertenecen a un área
			(
				(reserva.HoraInicio >= r.HoraInicio && reserva.HoraInicio < r.HoraFin) ||
				(reserva.HoraFin > r.HoraInicio && reserva.HoraFin <= r.HoraFin) ||
				(reserva.HoraInicio <= r.HoraInicio && reserva.HoraFin >= r.HoraFin)
			)
		);

		if (hayConflicto)
			throw new Exception("La nueva fecha u hora de la reserva no está disponible.");

		// Confirmar la reserva si pasó todas las validaciones
		reserva.EstadoReserva = Estado.CONFIRMADA;

		await _context.SaveChangesAsync();
	}


	public async Task<ReservaRespuestaDto> ReservarAsync(CrearReservaDto crearReserva, int Id, Guid userId)
	{
		var area = _context.AreaDeportivas.FirstOrDefault(a => a.Id == Id) ?? throw new Exception("Area no encontrada");
		
		var reservaExistente = await _context.Reservas
			.AnyAsync(r =>
				r.IdAreaDeportiva == Id &&
				r.Fecha == crearReserva.Fecha &&
				r.HoraInicio == crearReserva.HoraInicio &&
				r.HoraFin   == crearReserva.HoraFin   &&
				r.EstadoReserva != Estado.CANCELADA);

		if (reservaExistente)
			throw new Exception("El área ya está reservada en esa fecha y horario");
		
		var haySolapamiento = await _context.Reservas
			.AnyAsync(r =>
				r.IdAreaDeportiva == Id &&
				r.Fecha           == crearReserva.Fecha &&
				r.EstadoReserva   != Estado.CANCELADA &&

				// estas dos comparaciones detectan cualquier traslape
				r.HoraInicio < crearReserva.HoraFin  &&
				crearReserva.HoraInicio < r.HoraFin
			);

		if (haySolapamiento)
			throw new Exception("Ya hay otra reserva en esa franja horaria");


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