using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class InmuebleDTO
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La id del propietario no puede ser 0 o negativa")]
    public int IdPropietario { get; set; }

    public PropietarioDTO? Duenio { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La id del tipo de inmueble no puede ser 0 o negativa")]
    public int IdTipoInmueble { get; set; }

    public string? TipoInmueble { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
    public string? Uso { get; set; }

    [Required]
    [Range(1, 9999, ErrorMessage = "La cantidad de ambientes de ber un número entre 1 y 9999")]
    public int CantidadAmbientes { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El maximo de caracteres es 100")]
    public string? Calle { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int NroCalle { get; set; }

    [Range(-90.0, 90.0, ErrorMessage = "La latitud debe ser un número decimal entre -90 y 90")]
    public decimal? Latitud { get; set; }

    [Range(-180.0, 180.0, ErrorMessage = "La longitud debe ser un número decimal entre -180 y 180")]
    public decimal? Longitud { get; set; }

    public bool Disponible { get; set; }

    public string? Foto { get; set; }

    public List<ValidationResult> Validar()
    {
        var contexto = new ValidationContext(this);
        var resultados = new List<ValidationResult>();
        bool esValido = Validator.TryValidateObject(this, contexto, resultados, validateAllProperties: true);
        return resultados;
    }

    public static InmuebleDTO Parse(Inmueble inmueble)
    {
        InmuebleDTO dto = new InmuebleDTO
        {
            Id = inmueble.Id,
            IdTipoInmueble = inmueble.IdTipoInmueble,
            IdPropietario = inmueble.IdPropietario,
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

        if (inmueble.Tipo != null)
            dto.TipoInmueble = inmueble.Tipo.Tipo;

        if (inmueble.Duenio != null)
            dto.Duenio = PropietarioDTO.Parse(inmueble.Duenio);

        return dto;
    }

    public static List<InmuebleDTO> ParseList(List<Inmueble> inmuebles)
    {
        List<InmuebleDTO> dtos = new List<InmuebleDTO>();

        foreach (var i in inmuebles)
        {
            dtos.Add(Parse(i));
        }

        return dtos;
    }
}
