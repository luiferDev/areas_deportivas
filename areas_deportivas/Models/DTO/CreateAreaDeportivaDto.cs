using areas_deportivas.Models.Enums;

namespace areas_deportivas.Models.DTO;

public class CreateAreaDeportivaDto
{
	public string Nombre { get; set; } = null!;
	
	public string Description { get; set; } = null!;

	public string ImageUrl { get; set; } = null!;
	
	public Tipo TipoArea { get; set; }
	
	public decimal Precio { get; set; }
}