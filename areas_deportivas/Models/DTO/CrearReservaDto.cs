using areas_deportivas.Models;

public class CrearReservaDto
{
	public DateOnly Fecha { get; set; }

	public DateTimeOffset HoraInicio { get; set; }

	public DateTimeOffset HoraFin { get; set; }

	public Estado EstadoReserva { get; set; }

	public Guid IdUsuario { get; set; }

	public int IdAreaDeportiva { get; set; }
}