namespace Restaurante.Entidades.DTOs;

public class MesaDto
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public int Capacidad { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public int TotalReservas { get; set; }
}
