namespace areas_deportivas.Models.DTO;

public class UserRegisterDto
{
	public string Nombre { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Password { get; set; } = null!;
}