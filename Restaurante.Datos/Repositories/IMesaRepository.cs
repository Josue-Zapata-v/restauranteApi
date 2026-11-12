using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public interface IMesaRepository
{
    Task<List<Mesa>> ListarAsync(int? capacidadMinima = null, string? ubicacion = null);
    Task<Mesa?> ObtenerPorIdAsync(int id);
    Task<Mesa?> ObtenerPorIdConReservasAsync(int id);
    Task<bool> ExisteNumeroAsync(int numero, int? excluirId = null);
    Task<bool> TieneReservasAsync(int mesaId);
    Task<int> ObtenerMaximoPersonasReservadasAsync(int mesaId);
    Task<Mesa> CrearAsync(Mesa mesa);
    Task ActualizarAsync(Mesa mesa);
    Task EliminarAsync(Mesa mesa);
}
