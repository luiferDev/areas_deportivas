namespace areas_deportivas.Models.DTO;

public class CreateAreaDeportivaDto
{
	public string Nombre { get; set; } = null!;

	public Tipo TipoArea { get; set; }
}