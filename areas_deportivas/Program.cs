using areas_deportivas.Models;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ??
	throw new InvalidOperationException("La variable de entorno no está definida o está vacía");

builder.Services.AddDbContext<DeportesDbContext>(options =>
{
	options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IAreaDeportivaService, AreaDeportivaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();