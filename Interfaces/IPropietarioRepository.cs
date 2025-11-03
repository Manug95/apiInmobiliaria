using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Interfaces
{
    public interface IPropietarioRepository : IRepository<Propietario, int>
    {
        //metodos propios del repositorio de Propietario
        public Task<Propietario?> GetByEmailAsync(string email);
        public Task<bool> UpdateClaveAsync(Propietario propietario);
    }
}