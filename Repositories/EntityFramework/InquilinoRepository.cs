using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class InquilinoRepository : RepositoryBaseEF, IInquilinoRepository
    {
        public InquilinoRepository(BDContext context) : base(context) { }

        public Task<int> CreateAsync(Inquilino entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Inquilino?> GetByIdAsync(int id)
        {
            return await _dbContext.Inquilinos.FindAsync(id);
        }

        public Task<List<Inquilino>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Inquilino entidad)
        {
            throw new NotImplementedException();
        }
    }
}