using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Interfaces
{
    public interface IContratoRepository : IRepository<Contrato, int>
    {
        public Task<List<Contrato>> ListVigentesByPropietario(int idProp, int? offset, int? limit);
        public Task<Contrato?> GetVigenteByInmuebleAsync(int idInmueble);
        public Task<List<Contrato>> ListByInmuebleAsync(int idInmueble, int? offset, int? limit);
        public Task<List<Contrato>> ListByPropietario(int idProp, int? offset, int? limit);
    }
}