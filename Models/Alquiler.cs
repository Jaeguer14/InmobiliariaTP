using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models

{
  public class Alquiler
  {
    [Key]
    public int AlquilerId { get; set; }

    [DataType (DataType.Date)]

    public DateTime Date { get; set; }

    public int ClienteID { get; set; }

    public string Nombre { get; set; }  = null!;

    public string Apellido { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }

    public int CasaID  { get; set; }

    public string NombreCasa { get; set;}= null!;

    public virtual Casa? Casa { get; set; }

  }
  

    
}
