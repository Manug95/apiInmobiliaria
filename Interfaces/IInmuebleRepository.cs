using InmobiliariaGutierrezManuel.Models;

namespace api_inmobiliaria.Interfaces
{
    public interface IInmuebleRepository : IRepository<Inmueble, int>
    {
        // public bool EditDisponible(int id, bool disponible);
        public Task<bool> EditDisponibleAsync(Inmueble inmueble);
        public Task<int> CountByPropietarioAsync(int idProp);
        public Task<List<Inmueble>> ListByPropietarioAsync(int idProp, int? offset, int? limit);
        public Task<List<Inmueble>> ListActualmenteAlquiladosAsync(int idProp, int? offset, int? limit);
    }
}