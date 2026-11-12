using AutoMapper;
using Restaurante.Datos.Repositories;
using Restaurante.Entidades.DTOs;
using Restaurante.Entidades.Models;
using Restaurante.Negocio.Exceptions;

namespace Restaurante.Negocio.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepo;
    private readonly IMesaRepository _mesaRepo;
    private readonly IMapper _mapper;

    public ReservaService(IReservaRepository reservaRepo, IMesaRepository mesaRepo, IMapper mapper)
    {
        _reservaRepo = reservaRepo;
        _mesaRepo = mesaRepo;
        _mapper = mapper;
    }

    public async Task<List<ReservaDto>> ListarAsync(
        int? mesaId = null,
        DateTime? desde = null,
        DateTime? hasta = null,
        string? cliente = null)
    {
        var reservas = await _reservaRepo.ListarAsync(mesaId, desde, hasta, cliente);
        return _mapper.Map<List<ReservaDto>>(reservas);
    }

    public async Task<ReservaDto?> ObtenerPorIdAsync(int id)
    {
        var reserva = await _reservaRepo.ObtenerPorIdAsync(id);
        return reserva is null ? null : _mapper.Map<ReservaDto>(reserva);
    }

    public async Task<ReservaDto> CrearAsync(ReservaCrearDto dto)
    {
        var mesa = await ValidarReglasReservaAsync(dto.MesaId, dto.CantidadPersonas, dto.FechaHora);

        var reserva = _mapper.Map<Reserva>(dto);
        reserva.NombreCliente = reserva.NombreCliente.Trim();
        reserva.Telefono = reserva.Telefono.Trim();
        reserva.Observaciones = string.IsNullOrWhiteSpace(reserva.Observaciones)
            ? null
            : reserva.Observaciones.Trim();
        reserva.Mesa = mesa;

        var creada = await _reservaRepo.CrearAsync(reserva);
        return _mapper.Map<ReservaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int id, ReservaActualizarDto dto)
    {
        var reserva = await _reservaRepo.ObtenerPorIdAsync(id);
        if (reserva is null)
            return false;

        var mesa = await ValidarReglasReservaAsync(dto.MesaId, dto.CantidadPersonas, dto.FechaHora, id);

        _mapper.Map(dto, reserva);
        reserva.NombreCliente = reserva.NombreCliente.Trim();
        reserva.Telefono = reserva.Telefono.Trim();
        reserva.Observaciones = string.IsNullOrWhiteSpace(reserva.Observaciones)
            ? null
            : reserva.Observaciones.Trim();
        reserva.Mesa = mesa;

        await _reservaRepo.ActualizarAsync(reserva);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var reserva = await _reservaRepo.ObtenerPorIdAsync(id);
        if (reserva is null)
            return false;

        await _reservaRepo.EliminarAsync(reserva);
        return true;
    }

    private async Task<Mesa> ValidarReglasReservaAsync(
        int mesaId,
        int cantidadPersonas,
        DateTime fechaHora,
        int? excluirReservaId = null)
    {
        var mesa = await _mesaRepo.ObtenerPorIdAsync(mesaId);
        if (mesa is null)
            throw new ReglaNegocioException($"No existe una mesa registrada con el ID {mesaId}.");

        if (cantidadPersonas > mesa.Capacidad)
            throw new ReglaNegocioException(
                $"La reserva solicita {cantidadPersonas} personas, pero la mesa {mesa.Numero} tiene capacidad máxima para {mesa.Capacidad}.");

        if (await _reservaRepo.ExisteSolapamientoAsync(mesaId, fechaHora, excluirReservaId))
            throw new ReglaNegocioException(
                $"La mesa {mesa.Numero} ya tiene una reserva dentro del margen de 2 horas respecto a {fechaHora:dd/MM/yyyy HH:mm}.");

        return mesa;
    }
}
