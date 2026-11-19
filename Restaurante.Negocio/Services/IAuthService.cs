using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Services;

public interface IAuthService
{
    Task<TokenDto> RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
