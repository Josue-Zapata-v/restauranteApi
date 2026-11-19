namespace Restaurante.Entidades.DTOs;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
}
