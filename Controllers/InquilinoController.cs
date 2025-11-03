using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InquilinoController : ControllerBase
    {
        private readonly IInquilinoRepository _repo;
        private readonly ITokenService _tokenService;

        public InquilinoController(IInquilinoRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInquilino([FromRoute] int id)
        {
            try
            {
                int? idPropietario = _tokenService.GetIdDelToken(User);
                if (idPropietario == null)
                    return Unauthorized(new { mensaje = "No esta autenticado" });

                Inquilino? inquilino = await _repo.GetByIdAsync(id);
                if (inquilino != null)
                    return Ok(InquilinoDTO.Parse(inquilino));
                else
                    return NotFound(new { mensaje = "No se encontro el inquiñino" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "Ocurrió un error al buscar el inquilino" });
            }
        }

    }
}