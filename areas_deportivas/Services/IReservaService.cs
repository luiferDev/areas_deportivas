/// <summary>
/// Defines the contract for reservation-related operations within the application.
/// Implement this interface to provide methods for managing reservations.
/// </summary>
namespace areas_deportivas.Services;
public interface IReservaService
{
	Task Reservar(CrearReservaDto crearReserva);
	Task CancelarReserva();
}