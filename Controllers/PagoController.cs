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

        public PagoController(IPagoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("list/{idCon}")]
        [Authorize]
        public async Task<IActionResult> List(int idCon, int offset = 1, int limit = 10)
        {
            try
            {
                offset = (offset - 1) * limit;
                List<Pago> pagos = await _repo.ListByContratoAsync(idCon, offset, limit);
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}