using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Pago
{
    [Key]
    [Display(Name = "Nro Pago")]
    public int Id { get; set; }

    [Required(ErrorMessage = "La ID del contrato es requerida")]
    public int IdContrato { get; set; }

    public Contrato? Contrato { get; set; }

    [Required(ErrorMessage = "La fecha es requerida")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "El importe es requerido")]
    [DataType(DataType.Currency)]
    public decimal? Importe { get; set; }

    [MaxLength(255, ErrorMessage = "La cantidad máxima de caracteres es de 255")]
    [DataType(DataType.MultilineText)]
    public string? Detalle { get; set; }

    public string? Tipo { get; set; }

    public bool Estado { get; set; }
}