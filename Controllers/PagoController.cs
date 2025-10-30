using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoRepository _repo;
        private readonly ITokenService _tokenService;

        public PagoController(IPagoRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]                                                                                  //id del contrato
        public async Task<IActionResult> List([FromServices] IContratoRepository repoContrato, [FromQuery] int c, [FromQuery] int offset = 1, [FromQuery] int limit = 10)
        {
            if (c <= 0)
                return BadRequest(new { error = "Parámetro de ruta incorrecto" });

            int? idPropietario = _tokenService.GetIdDelToken(User);
            if (!idPropietario.HasValue)
                return Unauthorized(new { error = "No esta autenticado" });

            Contrato? contrato = await repoContrato.GetByIdAsync(c);
            if (contrato == null)
                return BadRequest(new { error = "No existe el contrato" });
            if (contrato!.Inmueble!.IdPropietario != idPropietario.Value)
                return StatusCode(403, new { error = "Usted no es el propietario del inmueble de este contrato" });

            try
            {
                offset = (offset - 1) * limit;
                List<Pago> pagos = await _repo.ListByContratoAsync(c, offset, limit);
                return Ok(PagoDTO.ParseList(pagos));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "Ocurrió un error al listar los pagos del contrato" });
            }
        }
    }
}