namespace api_inmobiliaria.Interfaces
{
    public interface IRepository<T, ID>
    {
        public Task<int> CreateAsync(T entidad);
        public Task<bool> UpdateAsync(T entidad);
        public Task<bool> DeleteAsync(ID id);
        public Task<List<T>> ListAsync(int? offset, int? limit);
        public Task<T?> GetByIdAsync(ID id);
    }
}