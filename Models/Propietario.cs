using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Propietario
{
    [Key]
    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "El máximo de caracteres es 50")]
    [Required(ErrorMessage="El nombre es requerido")]
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

    public bool? Activo { get; set; }

    public string? Clave { get; set; }

    public Propietario() { }

    public Propietario(string nombre, string apellido, string dni, string telefono, string email, string clave)
    {
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Telefono = telefono;
        Email = email;
        Clave = clave;
    }

    public Propietario(int id, string nombre, string apellido, string dni, string telefono, string email, string clave)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Telefono = telefono;
        Email = email;
        Clave = clave;
    }

    public Propietario(string nombre, string apellido, string dni)
    {
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
    }

    public override string ToString()
    {
        return $"{Apellido}, {Nombre} - {Dni}";
    }

    public static Propietario Parse(PropietarioDTO dto)
    {
        return new Propietario
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Dni = dto.Dni,
            Email = dto.Email,
            Telefono = dto.Telefono
        };
    }

    public static List<Propietario> ParseList(List<PropietarioDTO> dtos)
    {
        List<Propietario> propietarios = new List<Propietario>();

        foreach (PropietarioDTO dto in dtos)
        {
            propietarios.Add(Parse(dto));
        }

        return propietarios;
    }

    public void CopyValuesFrom(Propietario propietario)
    {
        Nombre = propietario.Nombre;
        Apellido = propietario.Apellido;
        Dni = propietario.Dni;
        Email = propietario.Email;
        Telefono = propietario.Telefono;
    }

    public void CopyValuesFrom(PropietarioDTO dto)
    {
        Nombre = dto.Nombre;
        Apellido = dto.Apellido;
        Dni = dto.Dni;
        Email = dto.Email;
        Telefono = dto.Telefono;
    }

}