namespace Restaurante.Entidades.DTOs;

public class ReservaDto
{
    public int Id { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public int CantidadPersonas { get; set; }
    public string? Observaciones { get; set; }
    public int MesaId { get; set; }
    public int MesaNumero { get; set; }
    public string MesaUbicacion { get; set; } = string.Empty;
}
