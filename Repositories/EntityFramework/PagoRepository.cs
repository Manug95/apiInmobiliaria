using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class PagoRepository : RepositoryBase, IPagoRepository
    {
        public PagoRepository(BDContext context) : base(context) { }

        public Task<int> CountByContratoAsync(int idCon)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(Pago entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pago?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>>ListByContratoAsync(int idCon, int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Pago entidad)
        {
            throw new NotImplementedException();
        }
    }
}