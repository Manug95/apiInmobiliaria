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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInquilino()
        {
            try
            {
                int? id = _tokenService.GetIdDelToken(User);
                if (id == null)
                {
                    return Unauthorized(new { msg = "No esta autenticado" });
                }

                Inquilino? inquilino = await _repo.GetByIdAsync(id.Value);
                if (inquilino != null)
                {
                    return Ok(InquilinoDTO.Parse(inquilino));
                }
                else
                {
                    return NotFound(new { msg = "No se encontro el inquiñino" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}