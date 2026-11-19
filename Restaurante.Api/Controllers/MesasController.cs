using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Exceptions;
using Restaurante.Negocio.Services;

namespace Restaurante.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MesasController : ControllerBase
{
    private readonly IMesaService _mesaService;

    public MesasController(IMesaService mesaService)
    {
        _mesaService = mesaService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MesaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MesaDto>>> GetMesas(
        [FromQuery] int? capacidadMinima,
        [FromQuery] string? ubicacion)
    {
        var mesas = await _mesaService.ListarAsync(capacidadMinima, ubicacion);
        return Ok(mesas);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(MesaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MesaDto>> GetMesa(int id)
    {
        var mesa = await _mesaService.ObtenerPorIdAsync(id);
        if (mesa is null)
            return NotFound(new { mensaje = $"No se encontró la mesa con ID {id}." });

        return Ok(mesa);
    }

    [HttpGet("{id:int}/reservas")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReservaDto>>> GetReservasPorMesa(int id)
    {
        var reservas = await _mesaService.ListarReservasDeMesaAsync(id);
        if (reservas is null)
            return NotFound(new { mensaje = $"No se encontró la mesa con ID {id}." });

        return Ok(reservas);
    }

    [HttpPost]
    [Authorize(Roles = "Gerente")]
    [ProducesResponseType(typeof(MesaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MesaDto>> CrearMesa([FromBody] MesaCrearDto dto)
    {
        try
        {
            var creada = await _mesaService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetMesa), new { id = creada.Id }, creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Gerente")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActualizarMesa(int id, [FromBody] MesaActualizarDto dto)
    {
        try
        {
            var actualizada = await _mesaService.ActualizarAsync(id, dto);
            if (!actualizada)
                return NotFound(new { mensaje = $"No se encontró la mesa con ID {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Gerente")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> EliminarMesa(int id)
    {
        try
        {
            var eliminada = await _mesaService.EliminarAsync(id);
            if (!eliminada)
                return NotFound(new { mensaje = $"No se encontró la mesa con ID {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
