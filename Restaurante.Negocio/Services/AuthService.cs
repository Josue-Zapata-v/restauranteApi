using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Restaurante.Datos.Repositories;
using Restaurante.Entidades.DTOs;
using Restaurante.Entidades.Models;
using Restaurante.Negocio.Exceptions;

namespace Restaurante.Negocio.Services;

public class AuthService : IAuthService
{
    private const string RolPorDefecto = "Recepcionista";

    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepo, IConfiguration configuration)
    {
        _usuarioRepo = usuarioRepo;
        _configuration = configuration;
    }

    public async Task<TokenDto> RegistrarAsync(RegistroDto dto)
    {
        var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();

        if (await _usuarioRepo.ExisteCorreoAsync(correoNormalizado))
            throw new ReglaNegocioException($"Ya existe un usuario registrado con el correo '{correoNormalizado}'.");

        var usuario = new Usuario
        {
            Correo = correoNormalizado,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolPorDefecto
        };

        await _usuarioRepo.AgregarAsync(usuario);
        await _usuarioRepo.GuardarCambiosAsync();

        return GenerarToken(usuario);
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correoNormalizado = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(correoNormalizado);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expirationMinutes = int.TryParse(jwtSection["ExpirationMinutes"], out var mins) ? mins : 60;

        if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
            throw new InvalidOperationException("La clave JWT debe tener al menos 32 caracteres y estar configurada en Jwt:Key.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("rol", usuario.Rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiracion = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiracion,
            signingCredentials: credentials);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }
}
