using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaGutierrezManuel.Models;

public class Contrato
{
    [Key]
    public int Id { get; set; }

    public int? IdInquilino { get; set; }

    [ForeignKey("IdInquilino")]
    public Inquilino? Inquilino { get; set; }

    public int? IdInmueble { get; set; }

    [ForeignKey("IdInmueble")]
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
        return new Contrato
        {
            IdInmueble = dto.IdInmueble,
            IdInquilino = dto.IdInquilino,
            Inmueble = dto.Inmueble,
            Inquilino = dto.Inquilino,
            MontoMensual = dto.MontoMensual,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            FechaTerminado = dto.FechaTerminado
        };
    }
}
