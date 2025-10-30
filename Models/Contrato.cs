using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaGutierrezManuel.Models;

public class Contrato
{
    [Key]
    public int Id { get; set; }

    public int? IdInquilino { get; set; }

    [ForeignKey("IdInquilino")]
    [Required]
    public Inquilino? Inquilino { get; set; }

    public int? IdInmueble { get; set; }

    [ForeignKey("IdInmueble")]
    [Required]
    public Inmueble? Inmueble { get; set; }

    public decimal? MontoMensual { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public DateTime? FechaTerminado { get; set; }

    public bool Borrado { get; set; }

    public Contrato() { }

    public override string ToString()
    {
        return @$"
        IdInquilino: {IdInquilino}
        IdInmueble: {IdInmueble}
        MontoMensual: {MontoMensual}
        FechaInicio: {FechaInicio}
        FechaFin: {FechaFin}
        ";
    }

    public static Contrato Parse(ContratoDTO dto)
    {
        Contrato contrato = new Contrato
        {
            Id = dto.Id,
            IdInmueble = dto.IdInmueble,
            IdInquilino = dto.IdInquilino,
            MontoMensual = dto.MontoMensual,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            FechaTerminado = dto.FechaTerminado
        };

        if (dto.Inmueble != null)
            contrato.Inmueble = Inmueble.Parse(dto.Inmueble);
        if (dto.Inquilino != null)
            contrato.Inquilino = Inquilino.Parse(dto.Inquilino);

        return contrato;
    }

    public static List<Contrato> ParseList(List<ContratoDTO> dtos)
    {
        List<Contrato> contratos = new List<Contrato>();

        foreach (ContratoDTO dto in dtos)
        {
            contratos.Add(Parse(dto));
        }

        return contratos;
    }
}
