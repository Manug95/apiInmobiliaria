using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class EditDisponible
{
    [Required]
    public int Id { get; set; }

    [Required]
    public bool Disponible { get; set; }
}
