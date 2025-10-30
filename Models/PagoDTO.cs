using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaGutierrezManuel.Models;

public class PagoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La ID del contrato es requerida")]
    public int IdContrato { get; set; }

    public Contrato? Contrato { get; set; }

    [Required(ErrorMessage = "La fecha es requerida")]
    [DataType(DataType.Date)]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "El importe es requerido")]
    [DataType(DataType.Currency)]
    public decimal? Importe { get; set; }

    [MaxLength(255, ErrorMessage = "La cantidad máxima de caracteres es de 255")]
    public string? Detalle { get; set; }

    public string? Tipo { get; set; }

    public static PagoDTO Parse(Pago pago)
    {
        PagoDTO dto = new PagoDTO
        {
            Id = pago.Id,
            IdContrato = pago.IdContrato,
            Fecha = pago.Fecha,
            Importe = pago.Importe,
            Detalle = pago.Detalle,
            Tipo = pago.Tipo
        };

        if (pago.Contrato != null)
            dto.Contrato = pago.Contrato;

        return dto;
    }

    public static List<PagoDTO> ParseList(List<Pago> pagos)
    {
        List<PagoDTO> dtos = new List<PagoDTO>();

        foreach (Pago p in pagos)
        {
            dtos.Add(Parse(p));
        }

        return dtos;
    }
}