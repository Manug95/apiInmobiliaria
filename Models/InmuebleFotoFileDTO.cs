using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaGutierrezManuel.Models;

public class InmuebleFotoFileDTO
{
    [FromForm(Name = "inmueble")]
    public string? InmuebleJson { get; set; }

    [FromForm(Name = "foto")]
    public IFormFile? Foto { get; set; }
}
