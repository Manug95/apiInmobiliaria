using api_inmobiliaria.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoInmuebleController : ControllerBase
    {
        private readonly ITipoInmuebleRepository _repo;
        private readonly ITokenService _tokenService;

        public TipoInmuebleController(ITipoInmuebleRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTipoInmueble()
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
                return Unauthorized(new { error = "No esta autenticado" });
            
            return Ok(await _repo.ListAsync());
        }
    }
}