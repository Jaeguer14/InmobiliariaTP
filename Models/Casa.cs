using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models; 

public class Casa

{
    [Key]
    public int CasaID { get; set; }
    public string Nombre { get; set; } = null!;
    public string Domicilio { get; set; } = null!;

    [Display(Name = "Propietario")]
    public string PropietarioNombre { get; set; } = null!;
    public byte[]? Imagen { get; set; } 
    public string?  ImagenContentType { get; set; } 

    [Display(Name = "Estado")]
    public bool EstaAlquilada { get; set; }

    public bool IsDeleted { get; set; }

   [NotMapped]
    public IFormFile? ImagenFile { get; set; } 



}

//Nombre de la Casa, Domicilio, Nombre del Dueño, Imagen y Estado.