using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<bool> ExisteCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    Task GuardarCambiosAsync();
}
