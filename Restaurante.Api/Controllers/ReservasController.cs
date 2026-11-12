using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Exceptions;
using Restaurante.Negocio.Services;

namespace Restaurante.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservasController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReservaDto>>> GetReservas(
        [FromQuery] int? mesaId,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] string? cliente)
    {
        var reservas = await _reservaService.ListarAsync(mesaId, desde, hasta, cliente);
        return Ok(reservas);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservaDto>> GetReserva(int id)
    {
        var reserva = await _reservaService.ObtenerPorIdAsync(id);
        if (reserva is null)
            return NotFound(new { mensaje = $"No se encontró la reserva con ID {id}." });

        return Ok(reserva);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservaDto>> CrearReserva([FromBody] ReservaCrearDto dto)
    {
        try
        {
            var creada = await _reservaService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetReserva), new { id = creada.Id }, creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActualizarReserva(int id, [FromBody] ReservaActualizarDto dto)
    {
        try
        {
            var actualizada = await _reservaService.ActualizarAsync(id, dto);
            if (!actualizada)
                return NotFound(new { mensaje = $"No se encontró la reserva con ID {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarReserva(int id)
    {
        var eliminada = await _reservaService.EliminarAsync(id);
        if (!eliminada)
            return NotFound(new { mensaje = $"No se encontró la reserva con ID {id}." });

        return NoContent();
    }
}
