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
                    return Unauthorized(new { error = "No esta autenticado" });

                Propietario? propietario = await _repo.GetByIdAsync(id.Value);
                if (propietario != null)
                    return Ok(PropietarioDTO.Parse(propietario));
                else
                    return NotFound(new { error = "No se encontro el propietario" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "No se pudo recuperar el propietario" });
            }
        }
        
        [HttpPut]
        [Authorize]
		public async Task<ActionResult> PutPropietario([FromBody] PropietarioDTO dto)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null)
                return Unauthorized(new { error = "No esta autenticado" });
                
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Datos incorrectos" });

            // Propietario propietario = Propietario.Parse(dto);
            // Propietario? propietario = _repo.GetById(id.Value);
            dto.Id = id.Value;
			try
			{
				if (await _repo.UpdateAsync(Propietario.Parse(dto)))
                    return Ok(dto);
                else
                    return BadRequest(new { error = "No se pudo actualizar el propietario" });
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
				return BadRequest(new { error = "Ocurrió un error al actualizar el propietario" });
			}
		}

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                string hashed = HashearPassword(login.Clave!);

                Propietario? propietario = await _repo.GetByEmailAsync(login.Email!);
                if (propietario == null || propietario.Clave != hashed)
                    return Unauthorized(new { error = "Credenciales inválidas" });

                var claims = new List<Claim>
                {
                    new Claim("id", propietario.Id.ToString())
                };

                return Ok(_tokenService.GenerarToken(claims));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { error = "Error al loguearse" });
            }
        }
        
        [HttpPatch("cambiarContraseña")]
        [Authorize]
		public async Task<ActionResult> ChangePassword([FromBody] EditPassword editPassword)
        {
            int? id = _tokenService.GetIdDelToken(User);
            if (id == null) 
                return Unauthorized(new { error = "No esta autenticado" });
                
            if (!ModelState.IsValid) 
                return BadRequest(new { error = "Datos incorrectos" });

            Propietario? propietario = await _repo.GetByIdAsync(id.Value);
            if (propietario == null)
                return NotFound(new { error = "Propietario no encontrado" });

            string passwordActualHasheada = HashearPassword(editPassword.ClaveActual);
            if (propietario.Clave != passwordActualHasheada)
                return StatusCode(403, new { error = "Las contraseñas actuales no coinciden" });
            
			try
			{
				if (await _repo.ChangePasswordAsync(id.Value, HashearPassword(editPassword.ClaveNueva)))
                    return Ok();
                else
                    return BadRequest(new { error = "No se pudo cambiar la contraseña" });
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
				return BadRequest(new { error = "Ocurrió un error al cambiar la contraseña" });
			}
		}

        private string HashearPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]!),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8))
            ;
        }
    }
}