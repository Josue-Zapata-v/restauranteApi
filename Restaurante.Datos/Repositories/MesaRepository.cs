using Microsoft.EntityFrameworkCore;
using Restaurante.Datos.Models;
using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public class MesaRepository : IMesaRepository
{
    private readonly RestauranteContext _context;

    public MesaRepository(RestauranteContext context)
    {
        _context = context;
    }

    public async Task<List<Mesa>> ListarAsync(int? capacidadMinima = null, string? ubicacion = null)
    {
        var query = _context.Mesas
            .AsNoTracking()
            .Include(m => m.Reservas)
            .AsQueryable();

        if (capacidadMinima.HasValue)
            query = query.Where(m => m.Capacidad >= capacidadMinima.Value);

        if (!string.IsNullOrWhiteSpace(ubicacion))
        {
            var texto = ubicacion.Trim().ToLower();
            query = query.Where(m => m.Ubicacion.ToLower().Contains(texto));
        }

        return await query
            .OrderBy(m => m.Numero)
            .ToListAsync();
    }

    public async Task<Mesa?> ObtenerPorIdAsync(int id)
    {
        return await _context.Mesas
            .Include(m => m.Reservas)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mesa?> ObtenerPorIdConReservasAsync(int id)
    {
        return await _context.Mesas
            .AsNoTracking()
            .Include(m => m.Reservas.OrderBy(r => r.FechaHora))
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> ExisteNumeroAsync(int numero, int? excluirId = null)
    {
        return await _context.Mesas
            .AnyAsync(m => m.Numero == numero && (!excluirId.HasValue || m.Id != excluirId.Value));
    }

    public async Task<bool> TieneReservasAsync(int mesaId)
    {
        return await _context.Reservas
            .AnyAsync(r => r.MesaId == mesaId);
    }

    public async Task<int> ObtenerMaximoPersonasReservadasAsync(int mesaId)
    {
        return await _context.Reservas
            .Where(r => r.MesaId == mesaId)
            .Select(r => (int?)r.CantidadPersonas)
            .MaxAsync() ?? 0;
    }

    public async Task<Mesa> CrearAsync(Mesa mesa)
    {
        _context.Mesas.Add(mesa);
        await _context.SaveChangesAsync();
        return mesa;
    }

    public async Task ActualizarAsync(Mesa mesa)
    {
        _context.Mesas.Update(mesa);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Mesa mesa)
    {
        _context.Mesas.Remove(mesa);
        await _context.SaveChangesAsync();
    }
}
