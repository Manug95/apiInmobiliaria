using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class ContratoDTO
{
    public int Id { get; set; }

    [Required]
    public int? IdInquilino { get; set; }

    public InquilinoDTO? Inquilino { get; set; }

    [Required]
    public int? IdInmueble { get; set; }

    [Required]
    public InmuebleDTO? Inmueble { get; set; }

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
        ContratoDTO dto = new ContratoDTO
        {
            Id = contrato.Id,
            IdInmueble = contrato.IdInmueble,
            IdInquilino = contrato.IdInquilino,
            MontoMensual = contrato.MontoMensual,
            FechaInicio = contrato.FechaInicio,
            FechaFin = contrato.FechaFin,
            FechaTerminado = contrato.FechaTerminado
        };

        if (contrato.Inmueble != null)
            dto.Inmueble = InmuebleDTO.Parse(contrato.Inmueble);
        if (contrato.Inquilino != null)
            dto.Inquilino = InquilinoDTO.Parse(contrato.Inquilino);

        return dto;
    }

    public static List<ContratoDTO> ParseList(List<Contrato> contratos)
    {
        List<ContratoDTO> dtos = new List<ContratoDTO>();

        foreach (Contrato c in contratos)
        {
            dtos.Add(Parse(c));
        }

        return dtos;
    }
}
