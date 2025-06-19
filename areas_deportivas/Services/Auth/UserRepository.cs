using areas_deportivas.DbContext;
using areas_deportivas.Models;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Services.Auth;

public class UserRepository(DeportesDbContext context)
{
	public async Task<Usuario?> GetByUsernameAsync(string? email)
	{
		return await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
	}

	public async Task AddUserAsync(Usuario user)
	{
		await context.Usuarios.AddAsync(user);
		await context.SaveChangesAsync();
	}
	public async Task UpdateUserAsync(Usuario user)
	{
		context.Usuarios.Update(user);
		await context.SaveChangesAsync();
	}
}