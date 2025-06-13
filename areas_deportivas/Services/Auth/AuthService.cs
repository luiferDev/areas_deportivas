using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using Microsoft.IdentityModel.Tokens;

namespace areas_deportivas.Services.Auth;

#pragma warning disable CS9113 // Parameter is unread.
public class AuthService(UserRepository userRepository, IConfiguration configuration) : IAuthService
#pragma warning restore CS9113 // Parameter is unread.
{
	public async Task<bool> RegisterUserAsync(UserRegisterDto usuario)
	{
		if (await userRepository.GetByUsernameAsync(usuario.Email) != null)
			return false; // Usuario ya existe.

		var hashedPassword = PasswordHasher.HashPassword(usuario.Password);

		var user = new Usuario
		{
			Id = Guid.NewGuid(),
			Nombre = usuario.Nombre,
			Email = usuario.Email,
			Password = hashedPassword,
			Role = Role.User
		};

		await userRepository.AddUserAsync(user);
		return true;
	}

	public async Task<bool> RegisterAdminAsync(string name, string password, string? email)
	{
		if (await userRepository.GetByUsernameAsync(email) != null)
			return false; // Usuario ya existe.

		var hashedPassword = PasswordHasher.HashPassword(password);

		var admin = new Usuario
		{
			Id = Guid.NewGuid(),
			Nombre = name,
			Email = email ?? string.Empty,
			Password = hashedPassword,
			Role = Role.Admin
		};

		await userRepository.AddUserAsync(admin);
		return true;
	}

	public async Task<string> LoginUserAsync(string email, string password)
	{
		var user = await userRepository.GetByUsernameAsync(email);

		if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
			return "Credenciales inválidas no existen";

		// Aquí puedes generar un token
		return GenerateJwtToken(user);
	}

	public string GenerateJwtToken(Usuario user)
	{
		var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ??
					throw new InvalidOperationException("JWT_KEY no está configurada en las variables de entorno");
		var byteKey = Encoding.UTF8.GetBytes(jwtKey);

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDes = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity([
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException()),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			]),
			Expires = DateTime.UtcNow.AddDays(1),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey),
				SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDes);
		return tokenHandler.WriteToken(token);
	}
}