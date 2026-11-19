using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    public string Clave { get; set; } = string.Empty;
}
