
using areas_deportivas.Models.DTO;

namespace areas_deportivas.Services;
public class UsuarioService : IUsuarioService
{
	private readonly IReservaService _reservaService;

	public UsuarioService(IReservaService reservaService)
	{
		_reservaService = reservaService;
	}
	
	public Task CancelarAreaAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<ReservaRespuestaDto> ReservarAreaAsync(CrearReservaDto crearReserva, int idArea, Guid userId)
	{
		return await _reservaService.ReservarAsync(crearReserva, idArea, userId);
	}
}