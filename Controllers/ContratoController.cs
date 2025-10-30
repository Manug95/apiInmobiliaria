using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContratoController : ControllerBase
    {
        private readonly IContratoRepository _repo;
        private readonly ITokenService _tokenService;

        public ContratoController(IContratoRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        [HttpGet("vigentes")]
        [Authorize]
        public async Task<IActionResult> ContratosVigentes([FromQuery] int offset = 1, [FromQuery] int limit = 10)
        {
            try
            {
                int? id = _tokenService.GetIdDelToken(User);
                if (id == null)
                    return Unauthorized(new { error = "No esta autenticado" });
                
                offset = (offset - 1) * limit;
                List<Contrato> contratos = await _repo.ListVigentesByPropietario(id.Value, offset, limit);

                return Ok(ContratoDTO.ParseList(contratos));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "Ocurrió un error al buscar los contratos vigentes" });
            }
        }
    }
}