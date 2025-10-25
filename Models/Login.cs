using System.ComponentModel.DataAnnotations;

namespace api_inmobiliaria.Models
{
    public class Login
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato correcto")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }
    }
}