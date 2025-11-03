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
                    return Unauthorized(new { mensaje = "No esta autenticado" });

                offset = (offset - 1) * limit;
                List<Contrato> contratos = await _repo.ListVigentesByPropietario(id.Value, offset, limit);

                return Ok(ContratoDTO.ParseList(contratos));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { mensaje = "Ocurrió un error al buscar los contratos vigentes" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetContrato(
            [FromServices] IInmuebleRepository repoInmueble,
            [FromQuery] int inmuebleId = 0,
            [FromQuery] int offset = 1,
            [FromQuery] int limit = 10
        )
        {
            if (inmuebleId < 0)
                return BadRequest(new { mensaje = "Parámetro de ruta incorrecto" });
            
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (propietarioId == null)
                    return Unauthorized(new { mensaje = "No esta autenticado" });

                offset = (offset - 1) * limit;
                List<Contrato> contratos;

                if (inmuebleId > 0)
                {
                    Inmueble? inmueble = await repoInmueble.GetByIdAsync(inmuebleId);
                    if (inmueble == null)
                        return NotFound(new { mensaje = "El inmueble no existe" });

                    if (inmueble.IdPropietario != propietarioId.Value)
                        return StatusCode(403, new { mensaje = "No es el dueño del inmueble de los contratos" });

                    contratos = await _repo.ListByInmuebleAsync(inmuebleId, offset, limit);
                }
                else
                    contratos = await _repo.ListByPropietario(propietarioId.Value, offset, limit);

                return Ok(ContratoDTO.ParseList(contratos));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { mensaje = "Ocurrió un error al buscar los contratos" });
            }
        }
    }
}