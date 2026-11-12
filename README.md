# Restaurante API — Arquitectura en Capas, DTOs y AutoMapper (Semana 13)

Web API REST desarrollada en **.NET 10** con **ASP.NET Core (Controladores)**, **Entity Framework Core (Database-First / Scaffold sobre SQL Server)** y **AutoMapper**, organizada en cuatro capas:

- `Restaurante.Entidades`: entidades generadas por scaffold (`Mesa`, `Reserva`, sin `[JsonIgnore]`) y DTOs de lectura/entrada (`MesaDto`, `MesaCrearDto`, `MesaActualizarDto`, `ReservaDto`, `ReservaCrearDto`, `ReservaActualizarDto`).
- `Restaurante.Datos`: `RestauranteContext` y repositorios asíncronos con LINQ (`IMesaRepository` / `MesaRepository`, `IReservaRepository` / `ReservaRepository`).
- `Restaurante.Negocio`: servicios con reglas de negocio (`IMesaService` / `MesaService`, `IReservaService` / `ReservaService`), excepción personalizada `ReglaNegocioException` y perfil único `MappingProfile`.
- `Restaurante.Api`: controladores (`MesasController`, `ReservasController`), inyección de dependencias en `Program.cs` y documentación interactiva con OpenAPI + SwaggerUI.

## Configuración local

1. Ejecuta el script [sql/restaurante.sql](sql/restaurante.sql) en SQL Server / LocalDB (`(localdb)\MSSQLLocalDB`) para crear la base `RestauranteDb` con datos de ejemplo.
2. Configura la cadena de conexión en `Restaurante.Api/appsettings.Development.json` (excluido de Git) o mediante `dotnet user-secrets`:

```powershell
cd Restaurante.Api
dotnet user-secrets set "ConnectionStrings:RestauranteDb" "Server=<TU_SERVIDOR_SQL>;Database=RestauranteDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

3. Ejecuta la API y abre Swagger en el navegador:

```powershell
dotnet run --project Restaurante.Api
```

URL de SwaggerUI (perfil `http`): `http://localhost:5133/swagger` (o `https://localhost:7063/swagger` con perfil `https`).
