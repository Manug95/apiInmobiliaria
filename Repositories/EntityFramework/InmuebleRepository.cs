using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class InmuebleRepository : RepositoryBaseEF, IInmuebleRepository
    {
        public InmuebleRepository(BDContext context) : base(context) { }

        public Task<int> CountByPropietarioAsync(int idProp)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(Inmueble inmueble)
        {
            _dbContext.Inmuebles.Add(inmueble);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Inmueble?> GetByIdAsync(int id)
        {
            return await _dbContext.Inmuebles
            .Include(i => i.Tipo)
            .Include(i => i.Duenio)
            .SingleOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Inmueble>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inmueble>> ListActualmenteAlquiladosAsync(int idProp, int? offset, int? limit)
        {
            DateTime hoy = DateTime.Today;

            IQueryable<Inmueble> inmuebles = _dbContext.Contratos
                .Include(c => c.Inquilino)
                .Include(c => c.Inmueble)
                    .ThenInclude(i => i!.Tipo)
                .Where(c => (c.FechaInicio <= hoy && hoy <= c.FechaFin) && c.FechaTerminado == null)
                .Select(c => c.Inmueble!)
                .Where(i => i.IdPropietario == idProp)
                .OrderBy(i => i.Id)
            ;

            if (offset.HasValue && offset.Value > 0) inmuebles = inmuebles.Skip(offset.Value);
            if (limit.HasValue) inmuebles = inmuebles.Take(limit.Value);

            return await inmuebles.ToListAsync();
        }

        public async Task<List<Inmueble>> ListByPropietarioAsync(int idProp, int? offset, int? limit)
        {
            IQueryable<Inmueble> inmuebles = _dbContext.Inmuebles
                .Include(i => i.Tipo)
                .Where(i => i.IdPropietario == idProp)
                .OrderBy(i => i.Id)
            ;

            if (offset.HasValue && offset.Value > 0) inmuebles = inmuebles.Skip(offset.Value);
            if (limit.HasValue) inmuebles = inmuebles.Take(limit.Value);

            return await inmuebles.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Inmueble inmueble)
        {
            _dbContext.Inmuebles.Update(inmueble);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateDisponibleAsync(Inmueble inmueble)
        {
            var entryInmueble = _dbContext.Entry(inmueble);

            if (entryInmueble.State == EntityState.Detached)
                _dbContext.Attach(inmueble);

            entryInmueble.Property(nameof(Inmueble.Disponible)).IsModified = true;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> UpdateFotoAsync(Inmueble inmueble)
        {
            var entryInmueble = _dbContext.Entry(inmueble);

            if (entryInmueble.State == EntityState.Detached)
                _dbContext.Attach(inmueble);

            entryInmueble.Property(nameof(Inmueble.Foto)).IsModified = true;

            return (await _dbContext.SaveChangesAsync()) > 0;
        }
    }
}