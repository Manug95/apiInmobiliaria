using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class InmuebleRepository : RepositoryBase, IInmuebleRepository
    {
        public InmuebleRepository(BDContext context) : base(context) { }

        public Task<int> CountByPropietarioAsync(int idProp)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(Inmueble inmueble)
        {
            _dbContext!.Inmuebles.Add(inmueble);
            return await _dbContext!.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Inmueble?> GetByIdAsync(int id)
        {
            return await _dbContext!.Inmuebles
            .Include(i => i.Tipo)
            //.Include(i => i.Duenio)
            .SingleOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Inmueble>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Inmueble>> ListActualmenteAlquiladosAsync(int idProp, int? offset, int? limit)
        {
            DateTime hoy = DateTime.Today;

            IQueryable<Inmueble> inmuebles = _dbContext!.Contratos
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
            IQueryable<Inmueble> inmuebles = _dbContext!.Inmuebles
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
            try
            {
                // _dbContext!.Attach(inmueble);        //actualizo solo las propiedades listadas

                // _dbContext!.Entry(inmueble).Property(i => i.Calle).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.NroCalle).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.IdTipoInmueble).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.Uso).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.CantidadAmbientes).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.Precio).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.Latitud).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.Longitud).IsModified = true;
                // _dbContext!.Entry(inmueble).Property(i => i.Disponible).IsModified = true;
                // _dbContext!.Entry(nuevo).Property(i => i.Foto).IsModified = true;
                
                //con esto se actualiza completo
                _dbContext!.Inmuebles.Update(inmueble);
                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el inmueble");
            }
        }

        public async Task<bool> UpdateDisponibleAsync(Inmueble inmueble)
        {
            return await UpdateParcial(inmueble);
        }

        public async Task<bool> UpdateFotoAsync(Inmueble inmueble)
        {
            return await UpdateParcial(inmueble);
        }

        private async Task<bool> UpdateParcial(Inmueble inmueble)
        {
            try
            {
                _dbContext!.Entry(inmueble).State = EntityState.Modified;
                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el inmueble");
            }
        }
    }
}