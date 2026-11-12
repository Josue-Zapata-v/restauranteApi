using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Services;

public interface IReservaService
{
    Task<List<ReservaDto>> ListarAsync(int? mesaId = null, DateTime? desde = null, DateTime? hasta = null, string? cliente = null);
    Task<ReservaDto?> ObtenerPorIdAsync(int id);
    Task<ReservaDto> CrearAsync(ReservaCrearDto dto);
    Task<bool> ActualizarAsync(int id, ReservaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
