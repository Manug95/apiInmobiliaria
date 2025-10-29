using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaGutierrezManuel.Models;

public class InmuebleFotoFileDTO
{
    [FromForm(Name = "inmueble")]
    public string? InmuebleJson { get; set; }

    [FromForm(Name = "foto")]
    public IFormFile? Foto { get; set; }

    /*public InmuebleFotoFileDTO(string InmuebleJson, IFormFile Foto)
    {
        this.InmuebleJson = InmuebleJson;
        this.Foto = Foto;
    }*/
}
