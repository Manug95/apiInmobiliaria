using api_inmobiliaria.Interfaces;
using api_inmobiliaria.Models;
using InmobiliariaGutierrezManuel.Models;
using Microsoft.EntityFrameworkCore;

namespace api_inmobiliaria.Repositories.EntityFramework
{
    public class PropietarioRepository : RepositoryBase, IPropietarioRepository
    {
        public PropietarioRepository(BDContext context) : base(context) { }

        public async Task<bool> ChangePasswordAsync(int id, string newPassword)
        {
            try
            {
                Propietario? propietario = await GetByIdAsync(id);
                
                if (propietario == null) return false;

                propietario.Clave = newPassword;

                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el propietario");
            }
        }

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

        public Task<List<Propietario>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Propietario?> loginAsync(string email, string clave)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Propietario propietarioModificado)
        {
            try
            {
                Propietario? propietario = await GetByIdAsync(propietarioModificado.Id);
                
                if (propietario == null) return false;
                
                propietario.Nombre = propietarioModificado.Nombre;
                propietario.Apellido = propietarioModificado.Apellido;
                propietario.Email = propietarioModificado.Email;
                propietario.Dni = propietarioModificado.Dni;
                propietario.Telefono = propietarioModificado.Telefono;

                return (await _dbContext!.SaveChangesAsync()) > 0;
            }
            catch (DbUpdateException dbue)
            {
                Console.WriteLine(dbue);
                throw new Exception("Error al actualizar el propietario");
            }
        }
    }
}