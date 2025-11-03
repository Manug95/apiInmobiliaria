using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api_inmobiliaria.Interfaces;
using api_inmobiliaria.Models;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api_inmobiliaria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropietarioController : ControllerBase
    {
        private readonly IPropietarioRepository _repo;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public PropietarioController(IPropietarioRepository repo, IConfiguration config, ITokenService tokenService)
        {
            _repo = repo;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPropietario()
        {
            try
            {
                int? id = _tokenService.GetIdDelToken(User);
                if (id == null)
                    return Unauthorized(new { mensaje = "No esta autenticado" });

                Propietario? propietario = await _repo.GetByIdAsync(id.Value);
                if (propietario != null)
                    return Ok(PropietarioDTO.Parse(propietario));
                else
                    return NotFound(new { mensaje = "No se encontro el propietario" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "No se pudo recuperar el propietario" });
            }
        }
        
        [HttpPut]
        [Authorize]
		public async Task<ActionResult> PutPropietario([FromBody] PropietarioDTO dto)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
                return Unauthorized(new { mensaje = "No esta autenticado" });
                
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos incorrectos" });

            Propietario? propietario = await _repo.GetByIdAsync(id.Value);
            if (propietario == null)
                return NotFound(new { mensaje = "El propietario no existe" });

            propietario.CopyValuesFrom(dto);
            dto.Id = id.Value;
			try
			{
				if (await _repo.UpdateAsync(propietario))
                    return Ok(dto);
                else
                    return BadRequest(new { mensaje = "No se pudo actualizar el propietario" });
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
				return BadRequest(new { mensaje = "Ocurrió un error al actualizar el propietario" });
			}
		}

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                string hashed = HashearClave(login.Clave!);

                Propietario? propietario = await _repo.GetByEmailAsync(login.Email!);
                if (propietario == null || propietario.Clave != hashed)
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });

                var claims = new List<Claim>
                {
                    new Claim("id", propietario.Id.ToString())
                };

                return Ok(_tokenService.GenerarToken(claims));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { mensaje = "Error al loguearse" });
            }
        }
        
        [HttpPatch("clave")]
        [Authorize]
		public async Task<ActionResult> CambiarClave([FromBody] CambioClave cambioClave)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null) 
                return Unauthorized(new { mensaje = "No esta autenticado" });
                
            if (!ModelState.IsValid) 
                return BadRequest(new { mensaje = "Datos incorrectos" });

            Propietario? propietario = await _repo.GetByIdAsync(id.Value);
            if (propietario == null)
                return NotFound(new { mensaje = "Propietario no encontrado" });

            string passwordActualHasheada = HashearClave(cambioClave.ClaveActual);
            if (propietario.Clave != passwordActualHasheada)
                return StatusCode(403, new { mensaje = "Las contraseñas actuales no coinciden" });

            propietario.Clave = HashearClave(cambioClave.ClaveNueva);
            
			try
			{
				if (await _repo.UpdateClaveAsync(propietario))
                    return Ok();
                else
                    return BadRequest(new { mensaje = "No se pudo cambiar la contraseña" });
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
				return BadRequest(new { mensaje = "Ocurrió un error al cambiar la contraseña" });
			}
		}

        private string HashearClave(string clave)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: clave,
                salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]!),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8))
            ;
        }
    }
}