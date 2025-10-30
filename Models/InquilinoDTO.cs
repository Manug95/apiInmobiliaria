using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class InquilinoDTO
{
    [Key]
    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "El máximo de caracteres es 50")]
    [Required(ErrorMessage = "El nombre es requerido")]
    public string? Nombre { get; set; }

    [StringLength(50, ErrorMessage = "El máximo de caracteres es 50")]
    [Required(ErrorMessage = "El apellido es requerido")]
    public string? Apellido { get; set; }

    [StringLength(13, ErrorMessage = "El máximo de caracteres es 13")]
    [Required(ErrorMessage = "El DNI es requerido")]
    public string? Dni { get; set; }

    [StringLength(25, ErrorMessage = "El máximo de caracteres es 25")]
    [Required(ErrorMessage = "El teléfono es requerido")]
    public string? Telefono { get; set; }

    [StringLength(100, ErrorMessage = "El máximo de caracteres es 100")]
    [Required(ErrorMessage = "El e-mail es requerido")]
    [EmailAddress(ErrorMessage = "El valor ingresado NO es un EMAIL")]
    public string? Email { get; set; }

    public InquilinoDTO(int id, string nombre, string apellido, string dni, string telefono, string email)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Telefono = telefono;
        Email = email;
    }

    public InquilinoDTO() { }

    public InquilinoDTO(string nombre, string apellido, string dni, string telefono, string email)
    {
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Telefono = telefono;
        Email = email;
    }

    public static InquilinoDTO Parse(Inquilino inquilino)
    {
        return new InquilinoDTO
        {
            Id = inquilino.Id,
            Nombre = inquilino.Nombre,
            Apellido = inquilino.Apellido,
            Dni = inquilino.Dni,
            Email = inquilino.Email,
            Telefono = inquilino.Telefono
        };
    }

    public static List<InquilinoDTO> ParseList(List<Inquilino> inquilinos)
    {
        List<InquilinoDTO> dtos = new List<InquilinoDTO>();

        foreach (Inquilino i in inquilinos)
        {
            dtos.Add(Parse(i));
        }

        return dtos;
    }
}
