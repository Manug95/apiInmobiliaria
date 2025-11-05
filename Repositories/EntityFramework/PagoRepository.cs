using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class PagoRepository : RepositoryBaseEF, IPagoRepository
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

        public Task<List<Pago>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Pago>>ListByContratoAsync(int idCon, int? offset, int? limit)
        {
            IQueryable<Pago> pagos = _dbContext.Pagos
                //.Include(p => p.Contrato)
                .Where(p => p.Estado && p.IdContrato == idCon)
                .OrderBy(p => p.Id)
            ;

            if (offset.HasValue)
                pagos = pagos.Skip(offset.Value);
            if (limit.HasValue)
                pagos = pagos.Take(limit.Value);

            return await pagos.ToListAsync();
        }

        public Task<bool> UpdateAsync(Pago entidad)
        {
            throw new NotImplementedException();
        }
    }
}