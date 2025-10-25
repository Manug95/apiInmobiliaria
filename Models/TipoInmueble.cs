using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaGutierrezManuel.Models;

[Table("tipos_inmueble")]
public class TipoInmueble
{
    [Key]
    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
    [Required(ErrorMessage = "El nombre de tipo es requerido")]
    public string? Tipo { get; set; }

    [StringLength(255, ErrorMessage = "El maximo de caracteres es 255")]
    public string? Descripcion { get; set; }

    public TipoInmueble() { }

    public override string ToString()
    {
        return $"{Tipo}";
    }

}