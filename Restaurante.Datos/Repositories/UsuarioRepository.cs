using Microsoft.EntityFrameworkCore;
using Restaurante.Datos.Models;
using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly RestauranteContext _context;

    public UsuarioRepository(RestauranteContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        var normalizado = correo.Trim().ToLower();
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo.ToLower() == normalizado);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        var normalizado = correo.Trim().ToLower();
        return await _context.Usuarios
            .AnyAsync(u => u.Correo.ToLower() == normalizado);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
