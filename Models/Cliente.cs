using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models;
public class Cliente
{
    [Key]
    public int ClienteID { get; set; }

    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string DNI { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }


}