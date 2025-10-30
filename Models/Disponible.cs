using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Disponible
{
    [Required]
    public bool Estado { get; set; }
}
