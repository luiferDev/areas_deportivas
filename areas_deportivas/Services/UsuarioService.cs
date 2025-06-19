
using areas_deportivas.Models.DTO;

namespace areas_deportivas.Services;
public class UsuarioService : IUsuarioService
{
	private readonly IReservaService _reservaService;

	public UsuarioService(IReservaService reservaService)
	{
		_reservaService = reservaService;
	}
	
	public Task CancelarAreaAsync(Guid reservaId)
	{
		return _reservaService.CancelarReservaAsync(reservaId);
	}

	public async Task EliminarAreaAsync(Guid reservaId)
	{
		await _reservaService.EliminarReservaAsync(reservaId);
	}

	public Task ActualizarAreaAsync(Guid reservaId, ActualizarReservaDto actualizarReserva)
	{
		return _reservaService.ActualizarReservaAsync(reservaId, actualizarReserva);
	}

	public async Task<ReservaRespuestaDto> ReservarAreaAsync(CrearReservaDto crearReserva, int idArea, Guid userId)
	{
		return await _reservaService.ReservarAsync(crearReserva, idArea, userId);
	}
}