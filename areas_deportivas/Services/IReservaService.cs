/// <summary>
/// Defines the contract for reservation-related operations within the application.
/// Implement this interface to provide methods for managing reservations.
/// </summary>
using areas_deportivas.Models.DTO; // Add this line or update with the correct namespace for ReservaDTO

namespace areas_deportivas.Services;
public interface IReservaService
{
	Task<ReservaRespuestaDto> ReservarAsync(CrearReservaDto crearReserva, int Id, Guid userId);
	Task CancelarReservaAsync(Guid reservaId);
}