
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;

namespace areas_deportivas.Services.Auth;

public interface IAuthService
{
	Task<bool> RegisterUserAsync(UserRegisterDto usuario);
	Task<bool> RegisterAdminAsync(string name, string password, string? email);
	Task<string> LoginUserAsync(string email, string password);
	string GenerateJwtToken(Usuario user);
}