namespace areas_deportivas.Models.DTO;

public class EmployeeRegisterDto
{
	public string Nombre { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Password { get; set; } = null!;
	
	public string Rol { get; set; } = null!;
}