using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La clave debe tener entre 6 y 100 caracteres.")]
    public string Clave { get; set; } = string.Empty;
}
