using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Interfaces
{
    public interface IContratoRepository : IRepository<Contrato, int>
    {
        public Task<List<Contrato>> ListVigentesByPropietario(int idProp, int? offset, int? limit);
    }
}