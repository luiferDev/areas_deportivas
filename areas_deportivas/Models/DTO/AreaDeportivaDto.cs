namespace areas_deportivas.Models.DTO;

public class AreaDeportivaDto
{
	public int Id { get; set; }

	public string Nombre { get; set; } = null!;

	public string? TipoArea { get; set; }

	public bool Disponibilidad { get; set; }
}