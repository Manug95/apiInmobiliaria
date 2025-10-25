using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api_inmobiliaria.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api_inmobiliaria.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public int? GetIdDelToken(ClaimsPrincipal user)
        {
            string? idStr = user.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (int.TryParse(idStr, out int id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        public string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_config["TokenAuthentication:SecretKey"]!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["TokenAuthentication:Issuer"],
                audience: _config["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60 * 24 * 365),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}