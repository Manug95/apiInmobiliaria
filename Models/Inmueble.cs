using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaGutierrezManuel.Models;

public class Inmueble
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdPropietario { get; set; }

    [ForeignKey("IdPropietario")]
    public Propietario? Duenio { get; set; }

    [Required]
    public int IdTipoInmueble { get; set; }

    [ForeignKey("IdTipoInmueble")]
    public TipoInmueble? Tipo { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
    public string? Uso { get; set; }

    [Required]
    public int CantidadAmbientes { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El maximo de caracteres es 100")]
    public string? Calle { get; set; }

    [Required]
    public int NroCalle { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public bool Disponible { get; set; }

    public bool Borrado { get; set; }

    public string? Foto { get; set; }

    public Inmueble() { }

    public override string ToString()
    {
        return $"Dirección {Calle} {NroCalle}{(Duenio != null ? $" perteneciente a {Duenio?.ToString()}" : "")}";
    }

    public string Direccion()
    {
        return $"{Calle} {NroCalle}";
    }

    public string MostrarDatos()
    {
        Console.WriteLine(Tipo);
        return @$"
        Id: {Id}
        IdPropietario: {IdPropietario}
        IdTipoInmueble: {IdTipoInmueble}
        Uso: {Uso}
        CantidadAmbientes: {CantidadAmbientes}
        Precio: {Precio}
        Calle: {Calle}
        NroCalle: {NroCalle}
        Latitud: {Latitud}
        Longitud: {Longitud}";
    }

    public static Inmueble Parse(InmuebleDTO dto)
    {
        return new Inmueble
        {
            Id = dto.Id,
            IdTipoInmueble = dto.IdTipoInmueble,
            IdPropietario = dto.IdPropietario,
            Tipo = new TipoInmueble
            {
                Tipo = dto.TipoInmueble
            },
            Duenio = dto.Duenio,
            Uso = dto.Uso,
            CantidadAmbientes = dto.CantidadAmbientes,
            Precio = dto.Precio,
            Calle = dto.Calle,
            NroCalle = dto.NroCalle,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Disponible = dto.Disponible,
            Foto = dto.Foto
        };
    }
}
