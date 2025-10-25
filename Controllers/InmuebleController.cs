using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InmuebleController : ControllerBase
    {
        private readonly IInmuebleRepository _repo;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _env;

        public InmuebleController(IInmuebleRepository repo, ITokenService tokenService, IWebHostEnvironment env)
        {
            _repo = repo;
            _tokenService = tokenService;
            _env = env;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInmueble(int id)
        {
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (propietarioId == null)
                {
                    return Unauthorized(new { msg = "No esta autenticado" });
                }

                Inmueble? inmueble = await _repo.GetByIdAsync(id);
                if (inmueble != null)
                {
                    if (inmueble.IdPropietario != propietarioId.Value)
                        return Forbid();
                    
                    return Ok(InmuebleDTO.Parse(inmueble));
                }
                else
                {
                    return NotFound(new { msg = "No se encontro el inmueble" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInmueble(int offset = 1, int limit = 10)
        {
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (!propietarioId.HasValue)
                {
                    return Unauthorized(new { msg = "No esta autenticado" });
                }

                offset = (offset - 1) * limit;
                List<Inmueble> inmuebles = await _repo.ListByPropietarioAsync(propietarioId.Value, offset, limit);
                //int cantidadInmuebles = _repo.Count(idProp);
                return Ok(InmuebleDTO.ParseList(inmuebles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("alquilados")]
        [Authorize]
        public async Task<IActionResult> Alquilados(int offset = 1, int limit = 10)
        {
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (!propietarioId.HasValue)
                {
                    return Unauthorized(new { msg = "No esta autenticado" });
                }

                offset = (offset - 1) * limit;
                List<Inmueble> inmuebles = await _repo.ListActualmenteAlquiladosAsync(propietarioId.Value, offset, limit);
                return Ok(InmuebleDTO.ParseList(inmuebles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostInmueble([FromBody] InmuebleDTO dto)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
            {
                return Unauthorized(new { msg = "No esta autenticado" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Datos incorrectos" });
            }

            Inmueble inmueble = Inmueble.Parse(dto);
            inmueble.IdPropietario = id.Value;
            
            if (dto.FotoFile != null)
            {
                GuardarImagen(dto);
                inmueble.Foto = dto.Foto;
            }

            try
            {
                if ((await _repo.CreateAsync(inmueble)) > 0)
                {
                    return CreatedAtAction("", InmuebleDTO.Parse(inmueble));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("disponible")]
        [Authorize]
        public async Task<ActionResult> EditDisponible([FromBody] EditDisponible edit)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
            {
                return Unauthorized(new { msg = "No esta autenticado" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Datos incorrectos" });
            }

            try
            {
                Inmueble? inmueble = await _repo.GetByIdAsync(edit.Id);
                if (inmueble == null)
                    return NotFound(new { msg = "No se encontro el inmueble" });

                if (inmueble.IdPropietario != id.Value)
                    return Forbid();

                inmueble.Disponible = edit.Disponible;

                if (await _repo.EditDisponibleAsync(inmueble))
                {
                    return Ok();
                }
                else
                {
                    return UnprocessableEntity(new { msg = "No se pudo actualizar el inmueble" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { msg = "No se pudo actualizar el inmueble" });
            }
        }
        
        private void GuardarImagen(InmuebleDTO dto)
        {
            string wwwPath = _env.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = "foto_" + dto.Id + Path.GetExtension(dto.FotoFile!.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            dto.Foto = Path.Combine("/Uploads", fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                dto.FotoFile.CopyTo(stream);
            }
        }

        private void BorrarImagen(int id, string nombreImagen)
        {
            var ruta = Path.Combine(_env.WebRootPath, "Uploads", $"foto_{id}" + Path.GetExtension(nombreImagen));
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);
        }
    }
}