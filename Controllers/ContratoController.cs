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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetContrato()
        {
            try
            {
                int? id = _tokenService.GetIdDelToken(User);
                if (id == null)
                {
                    return Unauthorized(new { msg = "No esta autenticado" });
                }

                Contrato? contrato = await _repo.GetByIdAsync(id.Value);
                if (contrato != null)
                {
                    return Ok(ContratoDTO.Parse(contrato));
                }
                else
                {
                    return NotFound(new { msg = "No se encontro el contrato" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}