using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Restaurante.Datos.Models;
using Restaurante.Datos.Repositories;
using Restaurante.Negocio.Mappings;
using Restaurante.Negocio.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RestauranteDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configura ConnectionStrings:RestauranteDb mediante user-secrets, appsettings.Development.json o variable de entorno.");
}

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "Configura Jwt:Key (mínimo 32 caracteres) mediante user-secrets, appsettings.Development.json o variable de entorno.");
}

// Capa de Datos: DbContext y Repositorios
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Capa de Negocio: Servicios y AutoMapper
builder.Services.AddScoped<IMesaService, MesaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Autenticación JWT Bearer
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = "rol",
            NameClaimType = "email",
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Capa API: Controladores y OpenAPI con esquema Bearer
builder.Services.AddControllers();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "Restaurante API — Mesas, Reservas y JWT (Semana 14)";
    document.Info.Version = "v1";
    document.Info.Description = "Web API REST en capas con DTOs, AutoMapper y autenticación JWT (roles Recepcionista y Gerente).";

    document.Components ??= new OpenApiComponents();
    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Ingresa el token JWT obtenido en POST /api/auth/login (sin escribir 'Bearer ')."
    };

    document.Security ??= new List<OpenApiSecurityRequirement>();
    document.Security.Add(new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });

    return Task.CompletedTask;
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Restaurante API v1");
        options.DocumentTitle = "Restaurante API | Swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
