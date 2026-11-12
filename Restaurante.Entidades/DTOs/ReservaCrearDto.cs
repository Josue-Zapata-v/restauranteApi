using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

public class ReservaCrearDto
{
    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del cliente debe tener entre 2 y 100 caracteres.")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "El teléfono debe tener entre 6 y 20 caracteres.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha y hora de la reserva son obligatorias.")]
    public DateTime FechaHora { get; set; }

    [Required(ErrorMessage = "La cantidad de personas es obligatoria.")]
    [Range(1, 20, ErrorMessage = "La cantidad de personas debe estar entre 1 y 20.")]
    public int CantidadPersonas { get; set; }

    [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder 300 caracteres.")]
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "El identificador de mesa es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El identificador de mesa debe ser mayor que cero.")]
    public int MesaId { get; set; }
}
