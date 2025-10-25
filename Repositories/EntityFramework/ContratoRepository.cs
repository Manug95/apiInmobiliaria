using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class ContratoRepository : RepositoryBase, IContratoRepository
    {
        public ContratoRepository(BDContext context) : base(context) { }

        public Task<int> CreateAsync(Contrato contrato)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Contrato?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contrato>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contrato>> ListVigentesByPropietario(int idProp, int? offset, int? limit)
        {
            DateTime hoy = DateTime.Today;

            IQueryable<Contrato> inmuebles = _dbContext!.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                    .ThenInclude(i => i!.Duenio)
                .Include(c => c.Inmueble)
                    .ThenInclude(i => i!.Tipo)
                .Where(c => (c.FechaInicio <= hoy && hoy <= c.FechaFin) && c.FechaTerminado == null && c.Inmueble!.Duenio!.Id == idProp)
                .OrderBy(c => c.Id)
            ;

            if (offset.HasValue && offset.Value > 0) inmuebles = inmuebles.Skip(offset.Value);
            if (limit.HasValue) inmuebles = inmuebles.Take(limit.Value);

            return await inmuebles.ToListAsync();
        }

        public Task<bool> UpdateAsync(Contrato contrato)
        {
            throw new NotImplementedException();
        }
    }
}