using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class InmuebleDTO
{
    public int Id { get; set; }

    [Required]
    public int IdPropietario { get; set; }

    public Propietario? Duenio { get; set; }

    [Required]
    public int IdTipoInmueble { get; set; }

    public string? TipoInmueble { get; set; }

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

    public string? Foto { get; set; }

    public static InmuebleDTO Parse(Inmueble inmueble)
    {
        return new InmuebleDTO
        {
            Id = inmueble.Id,
            IdTipoInmueble = inmueble.IdTipoInmueble,
            IdPropietario = inmueble.IdPropietario,
            TipoInmueble = inmueble.Tipo!.Tipo,
            Duenio = inmueble.Duenio,
            Uso = inmueble.Uso,
            CantidadAmbientes = inmueble.CantidadAmbientes,
            Precio = inmueble.Precio,
            Calle = inmueble.Calle,
            NroCalle = inmueble.NroCalle,
            Latitud = inmueble.Latitud,
            Longitud = inmueble.Longitud,
            Disponible = inmueble.Disponible,
            Foto = inmueble.Foto
        };
    }

    public static List<InmuebleDTO> ParseList(List<Inmueble> inmuebles)
    {
        List<InmuebleDTO> dtos = new List<InmuebleDTO>();

        foreach (var i in inmuebles)
        {
            dtos.Add(new InmuebleDTO
            {
                Id = i.Id,
                IdTipoInmueble = i.IdTipoInmueble,
                IdPropietario = i.IdPropietario,
                TipoInmueble = i.Tipo!.Tipo,
                Duenio = i.Duenio,
                Uso = i.Uso,
                CantidadAmbientes = i.CantidadAmbientes,
                Precio = i.Precio,
                Calle = i.Calle,
                NroCalle = i.NroCalle,
                Latitud = i.Latitud,
                Longitud = i.Longitud,
                Disponible = i.Disponible,
                Foto = i.Foto
            });
        }

        return dtos;
    }
}
