using System.Security.Claims;

namespace api_inmobiliaria.Interfaces
{
    public interface ITokenService
    {
        public int? GetIdDelToken(ClaimsPrincipal user);
        public string GenerarToken(List<Claim> claims);
    }
}