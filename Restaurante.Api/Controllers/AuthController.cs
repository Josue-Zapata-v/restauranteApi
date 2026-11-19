using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Exceptions;
using Restaurante.Negocio.Services;

namespace Restaurante.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TokenDto>> Registrar([FromBody] RegistroDto dto)
    {
        try
        {
            var token = await _authService.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, token);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token is null)
            return Unauthorized(new { mensaje = "Credenciales inválidas." });

        return Ok(token);
    }

    [HttpGet("perfil")]
    [Authorize]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<PerfilDto> Perfil()
    {
        var correo = User.FindFirstValue(JwtRegisteredClaimNames.Email)
            ?? User.FindFirstValue(ClaimTypes.Email)
            ?? string.Empty;

        var rol = User.FindFirstValue("rol")
            ?? User.FindFirstValue(ClaimTypes.Role)
            ?? string.Empty;

        return Ok(new PerfilDto
        {
            Correo = correo,
            Rol = rol
        });
    }
}
