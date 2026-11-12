using AutoMapper;
using Restaurante.Entidades.DTOs;
using Restaurante.Entidades.Models;

namespace Restaurante.Negocio.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Mesa, MesaDto>()
            .ForMember(dest => dest.TotalReservas, opt => opt.MapFrom(src => src.Reservas.Count));
        CreateMap<MesaCrearDto, Mesa>();
        CreateMap<MesaActualizarDto, Mesa>();

        CreateMap<Reserva, ReservaDto>()
            .ForMember(dest => dest.MesaNumero, opt => opt.MapFrom(src => src.Mesa.Numero))
            .ForMember(dest => dest.MesaUbicacion, opt => opt.MapFrom(src => src.Mesa.Ubicacion));
        CreateMap<ReservaCrearDto, Reserva>();
        CreateMap<ReservaActualizarDto, Reserva>();
    }
}
