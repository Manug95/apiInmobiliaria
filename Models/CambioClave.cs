using System.ComponentModel.DataAnnotations;

namespace api_inmobiliaria.Models
{
    public class CambioClave
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        [DataType(DataType.Password)]
        [StringLength(60, ErrorMessage = "muy lago, max 60 caracteres")]
        public string ClaveActual { get; set; }
        
        [Required(ErrorMessage = "La contraseña nueva es requerida")]
        [DataType(DataType.Password)]
        [StringLength(60, ErrorMessage = "muy lago, max 60 caracteres")]
        public string ClaveNueva { get; set; }

        public CambioClave(string claveActual, string claveNueva)
        {
            this.ClaveActual = claveActual;
            this.ClaveNueva = claveNueva;
        }
    }
}