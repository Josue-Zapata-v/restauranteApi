using AutoMapper;
using Restaurante.Datos.Repositories;
using Restaurante.Entidades.DTOs;
using Restaurante.Entidades.Models;
using Restaurante.Negocio.Exceptions;

namespace Restaurante.Negocio.Services;

public class MesaService : IMesaService
{
    private readonly IMesaRepository _mesaRepo;
    private readonly IReservaRepository _reservaRepo;
    private readonly IMapper _mapper;

    public MesaService(IMesaRepository mesaRepo, IReservaRepository reservaRepo, IMapper mapper)
    {
        _mesaRepo = mesaRepo;
        _reservaRepo = reservaRepo;
        _mapper = mapper;
    }

    public async Task<List<MesaDto>> ListarAsync(int? capacidadMinima = null, string? ubicacion = null)
    {
        var mesas = await _mesaRepo.ListarAsync(capacidadMinima, ubicacion);
        return _mapper.Map<List<MesaDto>>(mesas);
    }

    public async Task<MesaDto?> ObtenerPorIdAsync(int id)
    {
        var mesa = await _mesaRepo.ObtenerPorIdAsync(id);
        return mesa is null ? null : _mapper.Map<MesaDto>(mesa);
    }

    public async Task<List<ReservaDto>?> ListarReservasDeMesaAsync(int mesaId)
    {
        var mesa = await _mesaRepo.ObtenerPorIdAsync(mesaId);
        if (mesa is null)
            return null;

        var reservas = await _reservaRepo.ListarPorMesaAsync(mesaId);
        return _mapper.Map<List<ReservaDto>>(reservas);
    }

    public async Task<MesaDto> CrearAsync(MesaCrearDto dto)
    {
        if (await _mesaRepo.ExisteNumeroAsync(dto.Numero))
            throw new ReglaNegocioException($"Ya existe una mesa registrada con el número {dto.Numero}.");

        var mesa = _mapper.Map<Mesa>(dto);
        mesa.Ubicacion = mesa.Ubicacion.Trim();

        var creada = await _mesaRepo.CrearAsync(mesa);
        return _mapper.Map<MesaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int id, MesaActualizarDto dto)
    {
        var mesa = await _mesaRepo.ObtenerPorIdAsync(id);
        if (mesa is null)
            return false;

        if (await _mesaRepo.ExisteNumeroAsync(dto.Numero, id))
            throw new ReglaNegocioException($"Ya existe otra mesa registrada con el número {dto.Numero}.");

        var maxPersonas = await _mesaRepo.ObtenerMaximoPersonasReservadasAsync(id);
        if (dto.Capacidad < maxPersonas)
            throw new ReglaNegocioException(
                $"No se puede reducir la capacidad de la mesa a {dto.Capacidad} porque tiene una reserva registrada para {maxPersonas} personas.");

        _mapper.Map(dto, mesa);
        mesa.Ubicacion = mesa.Ubicacion.Trim();

        await _mesaRepo.ActualizarAsync(mesa);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mesa = await _mesaRepo.ObtenerPorIdAsync(id);
        if (mesa is null)
            return false;

        if (await _mesaRepo.TieneReservasAsync(id))
            throw new ReglaNegocioException(
                $"No se puede eliminar la mesa {mesa.Numero} porque tiene reservas asociadas.");

        await _mesaRepo.EliminarAsync(mesa);
        return true;
    }
}
