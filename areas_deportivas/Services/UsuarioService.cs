
namespace areas_deportivas.Services;
public class UsuarioService : IUsuarioService
{
	private readonly IReservaService _reservaService;

	public UsuarioService(IReservaService reservaService)
	{
		_reservaService = reservaService;
	}
	public async Task CancelarArea()
	{
		await _reservaService.CancelarReserva();
	}

	public void ReservarArea()
	{
		throw new NotImplementedException();
	}

	Task IUsuarioService.ReservarArea()
	{
		throw new NotImplementedException();
	}
}