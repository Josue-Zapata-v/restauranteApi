using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

public class MesaCrearDto
{
    [Required(ErrorMessage = "El número de mesa es obligatorio.")]
    [Range(1, 200, ErrorMessage = "El número de mesa debe estar entre 1 y 200.")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "La capacidad es obligatoria.")]
    [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20 personas.")]
    public int Capacidad { get; set; }

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "La ubicación debe tener entre 2 y 80 caracteres.")]
    public string Ubicacion { get; set; } = string.Empty;
}
