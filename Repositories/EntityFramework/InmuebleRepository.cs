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

        // public bool EditDisponible(int id, bool disponible)
        public async Task<bool> EditDisponibleAsync(Inmueble inmueble)
        {
            try
            {
                _dbContext!.Entry(inmueble).State = EntityState.Modified;
                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el propietario");
            }
        }

        public async Task<Inmueble?> GetByIdAsync(int id)
        {
            return await _dbContext!.Inmuebles.Include(i => i.Tipo).SingleAsync(i => i.Id == id);
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

        public async Task<bool> UpdateAsync(Inmueble inmuebleModificado)
        {
            try
            {
                Inmueble? inmueble = await GetByIdAsync(inmuebleModificado.Id);
                
                if (inmueble == null) return false;

                inmueble.IdTipoInmueble = inmuebleModificado.IdTipoInmueble;
                //propietario.IdPropietario = inmuebleModificado.IdPropietario;
                inmueble.Uso = inmuebleModificado.Uso;
                inmueble.CantidadAmbientes = inmuebleModificado.CantidadAmbientes;
                inmueble.Precio = inmuebleModificado.Precio;
                inmueble.Calle = inmuebleModificado.Calle;
                inmueble.NroCalle = inmuebleModificado.NroCalle;
                inmueble.Latitud = inmuebleModificado.Latitud;
                inmueble.Longitud = inmuebleModificado.Longitud;
                inmueble.Disponible = inmuebleModificado.Disponible;
                inmueble.Foto = inmuebleModificado.Foto;

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