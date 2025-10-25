using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Interfaces
{
    public interface IPagoRepository : IRepository<Pago, int>
    {
        public Task<List<Pago>> ListByContratoAsync(int idCon, int? offset, int? limit);
        public Task<int> CountByContratoAsync(int idCon);
    }
}