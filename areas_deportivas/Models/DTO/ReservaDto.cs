namespace areas_deportivas.Models.DTO;

public class ReservaDto
{
	public DateOnly Fecha { get; set; }

	public TimeOnly HoraInicio { get; set; }

	public TimeOnly HoraFin { get; set; }

	public string? EstadoReserva { get; set; }

	public int IdAreaDeportiva { get; set; }
}