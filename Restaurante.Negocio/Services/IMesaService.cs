using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Services;

public interface IMesaService
{
    Task<List<MesaDto>> ListarAsync(int? capacidadMinima = null, string? ubicacion = null);
    Task<MesaDto?> ObtenerPorIdAsync(int id);
    Task<List<ReservaDto>?> ListarReservasDeMesaAsync(int mesaId);
    Task<MesaDto> CrearAsync(MesaCrearDto dto);
    Task<bool> ActualizarAsync(int id, MesaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
