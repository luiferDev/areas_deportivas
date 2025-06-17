using areas_deportivas.Models.DTO;

namespace areas_deportivas.Services;

public interface IUsuarioService
{
	Task<ReservaRespuestaDto> ReservarAreaAsync(CrearReservaDto crearReserva, int Id, Guid userId);
	Task CancelarAreaAsync(Guid userId);
}