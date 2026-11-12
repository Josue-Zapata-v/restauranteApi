using Microsoft.EntityFrameworkCore;
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

// Capa de Datos: DbContext y Repositorios
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// Capa de Negocio: Servicios y AutoMapper
builder.Services.AddScoped<IMesaService, MesaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Capa API: Controladores y OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info.Title = "Restaurante API — Mesas y Reservas (Semana 13)";
    document.Info.Version = "v1";
    document.Info.Description = "Web API REST en capas (Entidades, Datos, Negocio y API) con DTOs y AutoMapper.";
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
app.UseAuthorization();
app.MapControllers();

app.Run();
