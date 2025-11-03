using api_inmobiliaria.Interfaces;
using api_inmobiliaria.Models;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class PropietarioRepository : RepositoryBaseEF, IPropietarioRepository
    {
        public PropietarioRepository(BDContext context) : base(context) { }

        public Task<int> CreateAsync(Propietario entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Propietario?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Propietario?> GetByEmailAsync(string email)
        {
            return await _dbContext!.Propietarios.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Propietario?> GetByIdAsync(int id)
        {
            return await _dbContext!.Propietarios.FindAsync(id);
        }

        public Task<List<Propietario>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<Propietario?> loginAsync(string email, string clave)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Propietario propietario)
        {
            try
            {
                // var entryPropietario = _dbContext!.Entry(propietario);

                // if (entryPropietario.State == EntityState.Detached)
                //     _dbContext!.Attach(propietario);

                // entryPropietario.Property(nameof(Propietario.Nombre)).IsModified = true;
                // entryPropietario.Property(nameof(Propietario.Apellido)).IsModified = true;
                // entryPropietario.Property(nameof(Propietario.Email)).IsModified = true;
                // entryPropietario.Property(nameof(Propietario.Dni)).IsModified = true;
                // entryPropietario.Property(nameof(Propietario.Telefono)).IsModified = true;

                _dbContext!.Propietarios.Update(propietario);
                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el propietario");
            }
        }

        public async Task<bool> UpdateClaveAsync(Propietario propietario)
        {
            var entryPropietario = _dbContext!.Entry(propietario);

            if (entryPropietario.State == EntityState.Detached)
                _dbContext!.Attach(propietario);

            entryPropietario.Property(nameof(Propietario.Clave)).IsModified = true;

            return (await _dbContext!.SaveChangesAsync()) > 0;
        }
    }
}