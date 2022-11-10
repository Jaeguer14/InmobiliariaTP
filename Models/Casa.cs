using System.ComponentModel.DataAnnotations;
namespace Inmobiliaria.Models; 

public class Casa

{
    [Key]
    public int CasaID { get; set; }
    public string Nombre { get; set; } = null!;
    public string Domicilio { get; set; } = null!;
    public string DueñoNombre { get; set; } = null!;
    public string Imagen { get; set; } = null!;
    public bool EstaAlquilada { get; set; }



}

//Nombre de la Casa, Domicilio, Nombre del Dueño, Imagen y Estado.