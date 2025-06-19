namespace areas_deportivas.Models.DTO;

public class ActualizarReservaDto
{
    public DateOnly? Fecha { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }
}