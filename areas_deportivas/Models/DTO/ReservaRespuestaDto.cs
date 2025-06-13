using areas_deportivas.Models.DTO;

namespace areas_deportivas.Models.DTO;
public class ReservaRespuestaDto
{
	public AreaDeportivaDto AreaDeportiva { get; set; } = null!;
	public ReservaDto Reserva { get; set; } = null!;
}
