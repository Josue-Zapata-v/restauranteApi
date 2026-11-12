using Microsoft.EntityFrameworkCore;
using Restaurante.Datos.Models;
using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly RestauranteContext _context;

    public ReservaRepository(RestauranteContext context)
    {
        _context = context;
    }

    public async Task<List<Reserva>> ListarAsync(
        int? mesaId = null,
        DateTime? desde = null,
        DateTime? hasta = null,
        string? cliente = null)
    {
        var query = _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .AsQueryable();

        if (mesaId.HasValue)
            query = query.Where(r => r.MesaId == mesaId.Value);

        if (desde.HasValue)
            query = query.Where(r => r.FechaHora >= desde.Value);

        if (hasta.HasValue)
            query = query.Where(r => r.FechaHora <= hasta.Value);

        if (!string.IsNullOrWhiteSpace(cliente))
        {
            var texto = cliente.Trim().ToLower();
            query = query.Where(r => r.NombreCliente.ToLower().Contains(texto));
        }

        return await query
            .OrderBy(r => r.FechaHora)
            .ToListAsync();
    }

    public async Task<List<Reserva>> ListarPorMesaAsync(int mesaId)
    {
        return await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Mesa)
            .Where(r => r.MesaId == mesaId)
            .OrderBy(r => r.FechaHora)
            .ToListAsync();
    }

    public async Task<Reserva?> ObtenerPorIdAsync(int id)
    {
        return await _context.Reservas
            .Include(r => r.Mesa)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> ExisteSolapamientoAsync(int mesaId, DateTime fechaHora, int? excluirReservaId = null)
    {
        // Ventana de 2 horas alrededor del horario reservado para evitar choques en la misma mesa.
        var inicioVentana = fechaHora.AddHours(-2);
        var finVentana = fechaHora.AddHours(2);

        return await _context.Reservas
            .AnyAsync(r =>
                r.MesaId == mesaId &&
                r.FechaHora > inicioVentana &&
                r.FechaHora < finVentana &&
                (!excluirReservaId.HasValue || r.Id != excluirReservaId.Value));
    }

    public async Task<Reserva> CrearAsync(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        await _context.Entry(reserva).Reference(r => r.Mesa).LoadAsync();
        return reserva;
    }

    public async Task ActualizarAsync(Reserva reserva)
    {
        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Reserva reserva)
    {
        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
    }
}
