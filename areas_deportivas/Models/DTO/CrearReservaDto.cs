using areas_deportivas.Models;

public class CrearReservaDto
{
	public DateOnly Fecha { get; set; }

	public TimeOnly HoraInicio { get; set; }

	public TimeOnly HoraFin { get; set; }

	public int IdAreaDeportiva { get; set; }
}