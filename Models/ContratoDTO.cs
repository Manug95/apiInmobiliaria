using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class ContratoDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int? IdInquilino { get; set; }

    public Inquilino? Inquilino { get; set; }

    [Required]
    public int? IdInmueble { get; set; }

    [Required]
    public Inmueble? Inmueble { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal? MontoMensual { get; set; }

    [Required]
    public DateTime? FechaInicio { get; set; }

    [Required]
    public DateTime? FechaFin { get; set; }

    public DateTime? FechaTerminado { get; set; }

    public ContratoDTO() { }

    public static ContratoDTO Parse(Contrato contrato)
    {
        return new ContratoDTO
        {
            IdInmueble = contrato.IdInmueble,
            IdInquilino = contrato.IdInquilino,
            Inmueble = contrato.Inmueble,
            Inquilino = contrato.Inquilino,
            MontoMensual = contrato.MontoMensual,
            FechaInicio = contrato.FechaInicio,
            FechaFin = contrato.FechaFin,
            FechaTerminado = contrato.FechaTerminado
        };
    }
}
