using System.Text;
using areas_deportivas.Models;
using areas_deportivas.Services;
using areas_deportivas.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar servicios de Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "API Gestión de Areas Deportivas",
		Version = "v1",
		Description = "API para gestionar usuarios con autenticación JWT."
	});

	// Configurar el esquema de autenticación Bearer
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Ingrese el token JWT en este formato: Bearer {token}"
	});

	// Requerir el esquema en todos los endpoints protegidos
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Configuration.AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ??
	throw new InvalidOperationException("La variable de entorno no está definida o está vacía");

builder.Services.AddDbContext<DeportesDbContext>(options =>
{
	options.UseNpgsql(connectionString);
});

// Obtener la key desde la variable de entorno
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ??
		throw new InvalidOperationException("JWT_KEY no está configurada en las variables de entorno");
var key = Encoding.UTF8.GetBytes(jwtKey);


builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
	var signingkey = new SymmetricSecurityKey(key);
	var signingCredential = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256Signature);

	opt.RequireHttpsMetadata = false;

	opt.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateAudience = false,
		ValidateIssuer = false,
		IssuerSigningKey = signingkey
	};
});

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString)
	.EnableUnmappedTypes()             // 👈 así permites enums no mapeados
	.MapEnum<UserRole>("user_role")
	.MapComposite<Usuario>()
	.MapEnum<Tipo>("tipo")
	.MapComposite<AreaDeportiva>()
	.MapEnum<Estado>("estado")
	.MapComposite<Reserva>();

await using var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<DeportesDbContext>(options =>
	options.UseNpgsql(dataSource, o => {
		o.MapEnum<UserRole>("user_role");
		o.MapEnum<Tipo>("tipo");
		o.MapEnum<Estado>("estado");
	}));


// Configurar CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins", policy =>
	{
		policy.WithOrigins("http://localhost:5173",
				"https://zorvanz.vercel.app") // Especificar dominios permitidos
			.AllowAnyHeader() // Permitir cualquier encabezado
			.AllowAnyMethod() // Permitir cualquier método HTTP (GET, POST, etc.)
			.AllowCredentials(); // Permitir cookies o credenciales
	});

	/*options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permitir cualquier dominio
            .AllowAnyHeader()
            .AllowAnyMethod();
    });*/
});

builder.Services.AddScoped<IAreaDeportivaService, AreaDeportivaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Aplicar CORS (antes de los controladores)
app.UseCors("AllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Zorvanz v1");
		c.RoutePrefix = string.Empty; // Para que Swagger esté en la raíz del proyecto
	});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();