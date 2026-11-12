using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public interface IReservaRepository
{
    Task<List<Reserva>> ListarAsync(int? mesaId = null, DateTime? desde = null, DateTime? hasta = null, string? cliente = null);
    Task<List<Reserva>> ListarPorMesaAsync(int mesaId);
    Task<Reserva?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteSolapamientoAsync(int mesaId, DateTime fechaHora, int? excluirReservaId = null);
    Task<Reserva> CrearAsync(Reserva reserva);
    Task ActualizarAsync(Reserva reserva);
    Task EliminarAsync(Reserva reserva);
}
