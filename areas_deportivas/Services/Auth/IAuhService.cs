
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;

namespace areas_deportivas.Services.Auth;

public interface IAuthService
{
	Task<bool> RegisterUserAsync(UserRegisterDto usuario);
	Task<bool> RegisterAdminAsync(UserRegisterDto usuario);
	Task<bool> EmployeeRegisterAsync(EmployeeRegisterDto empleado);
	Task<string> LoginUserAsync(string email, string password);
	string GenerateJwtToken(Usuario user);
}