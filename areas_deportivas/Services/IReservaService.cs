/// <summary>
/// Defines the contract for reservation-related operations within the application.
/// Implement this interface to provide methods for managing reservations.
/// </summary>
public interface IReservaService
{
	Task Reservar();
	Task CancelarReserva();
}