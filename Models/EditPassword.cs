using System.ComponentModel.DataAnnotations;

namespace api_inmobiliaria.Models
{
    public class EditPassword
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        [DataType(DataType.Password)]
        [StringLength(60, ErrorMessage = "muy lago, max 60 caracteres")]
        public string PasswordActual { get; set; }
        
        [Required(ErrorMessage = "La contraseña nueva es requerida")]
        [DataType(DataType.Password)]
        [StringLength(60, ErrorMessage = "muy lago, max 60 caracteres")]
        public string PasswordNueva { get; set; }

        public EditPassword(string passwordActual, string passwordNueva)
        {
            this.PasswordActual = passwordActual;
            this.PasswordNueva = passwordNueva;
        }
    }
}