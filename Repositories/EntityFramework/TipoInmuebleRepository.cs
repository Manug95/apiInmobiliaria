using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class TipoInmuebleRepository : RepositoryBaseEF, ITipoInmuebleRepository
    {
        public TipoInmuebleRepository(BDContext context) : base(context) { }

        public Task<int> CreateAsync(TipoInmueble entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TipoInmueble?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TipoInmueble>> ListAsync(int? offset, int? limit)
        {
            return await _dbContext!.TipoInmuebles.ToListAsync();
        }

        public Task<bool> UpdateAsync(TipoInmueble entidad)
        {
            throw new NotImplementedException();
        }
    }
}