using System.Text.Json;
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
        public async Task<IActionResult> GetInmueble([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "Parámetro de ruta incorrecto" });

            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (propietarioId == null)
                    return Unauthorized(new { mensaje = "No está autenticado" });

                Inmueble? inmueble = await _repo.GetByIdAsync(id);
                if (inmueble != null)
                {
                    if (inmueble.IdPropietario != propietarioId.Value)
                        return StatusCode(403, new { mensaje = "Usted no es el dueño del inmueble" });
                    
                    return Ok(InmuebleDTO.Parse(inmueble));
                }
                else
                    return NotFound(new { mensaje = "No se encontro el inmueble" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "No se pudo recuperar el inmueble" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInmueble([FromQuery] int offset = 1, [FromQuery] int limit = 10)
        {
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (!propietarioId.HasValue)
                    return Unauthorized(new { mensaje = "No está autenticado" });

                offset = (offset - 1) * limit;
                List<Inmueble> inmuebles = await _repo.ListByPropietarioAsync(propietarioId.Value, offset, limit);
                
                return Ok(InmuebleDTO.ParseList(inmuebles));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "No se pudieron recuperar los inmuebles" });
            }
        }

        [HttpGet("alquilados")]
        [Authorize]
        public async Task<IActionResult> Alquilados([FromQuery] int offset = 1, [FromQuery] int limit = 10)
        {
            try
            {
                int? propietarioId = _tokenService.GetIdDelToken(User);
                if (!propietarioId.HasValue)
                    return Unauthorized(new { mensaje = "No está autenticado" });

                offset = (offset - 1) * limit;
                List<Inmueble> inmuebles = await _repo.ListActualmenteAlquiladosAsync(propietarioId.Value, offset, limit);
                return Ok(InmuebleDTO.ParseList(inmuebles));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "No se pudieron recuperar los inmuebles actualmente alquilados" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostInmueble([FromForm] InmuebleFotoFileDTO request)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
                return Unauthorized(new { mensaje = "No está autenticado" });

            if (request.Foto == null || request.InmuebleJson == null)
                return BadRequest(new { mensaje = "Faltan datos" });

            InmuebleDTO? dto = JsonSerializer.Deserialize<InmuebleDTO>(request.InmuebleJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null)
                return BadRequest(new { mensaje = "Json de Inmueble mal formado" });

            Inmueble inmueble = Inmueble.Parse(dto);
            inmueble.IdPropietario = id.Value;

            try
            {
                if ((await _repo.CreateAsync(inmueble)) > 0)
                {
                    string urlFoto = await GuardarImagen(request.Foto, "foto_" + inmueble.Id);
                    inmueble.Foto = urlFoto;

                    if (await _repo.UpdateFotoAsync(inmueble))
                        return CreatedAtAction(nameof(GetInmueble), new { id = inmueble.Id }, InmuebleDTO.Parse(inmueble));
                    else
                        return BadRequest(new { mensaje = "Foto no guardada" });
                }
                else
                    return BadRequest(new { mensaje = "Inmueble no guardado" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado. No se pudo crear el inmueble" });
            }
        }

        [HttpPatch("{id}/disponible")]
        [Authorize]
        public async Task<ActionResult> EditarDisponible([FromRoute] int id, [FromBody] Disponible disponible)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "Parámetro de ruta incorrecto" });

            int? idPropietario = _tokenService.GetIdDelToken(User);
            if (idPropietario == null)
                return Unauthorized(new { mensaje = "No está autenticado" });

            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos incorrectos" });

            try
            {
                Inmueble? inmueble = await _repo.GetByIdAsync(id);
                if (inmueble == null)
                    return NotFound(new { mensaje = "No se encontro el inmueble" });

                if (inmueble.IdPropietario != idPropietario.Value)
                    return StatusCode(403, new { mensaje = "Usted no es el dueño del inmueble que quiere modificar" });


                inmueble.Disponible = disponible.Estado;

                if (await _repo.UpdateDisponibleAsync(inmueble))
                    return Ok();
                else
                    return UnprocessableEntity(new { mensaje = "Algo está mal en los datos recibidos" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "No se pudo actualizar el inmueble" });
            }
        }
        
        private async Task<string> GuardarImagen(IFormFile imagen, string nombreImagen)
        {
            string wwwPath = _env.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = nombreImagen + Path.GetExtension(imagen.FileName);
            string pathCompleto = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                await imagen.CopyToAsync(stream);

            return Path.Combine("/Uploads", fileName);
        }

        private void BorrarImagen(int id, string nombreImagen)
        {
            var ruta = Path.Combine(_env.WebRootPath, "Uploads", $"foto_{id}" + Path.GetExtension(nombreImagen));
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);
        }
    }
}