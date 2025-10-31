using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class TipoInmuebleRepository : RepositoryBase, ITipoInmuebleRepository
    {
        public TipoInmuebleRepository(IConfiguration configuration) : base(configuration) { }
        
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

        public Task<List<TipoInmueble>> ListAsync()
        {
            var tiposInmuebles = new List<TipoInmueble>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        {nameof(TipoInmueble.Id)}, 
                        {nameof(TipoInmueble.Tipo)}, 
                        IFNULL({nameof(TipoInmueble.Descripcion)}, 'Sin Descripción') AS {nameof(TipoInmueble.Descripcion)} 
                    FROM tipos_inmueble;"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tiposInmuebles.Add(new TipoInmueble
                            {
                                Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                                Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),
                                Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion))
                            });
                        }
                    }
                }
            }

            return Task.FromResult(tiposInmuebles);
        }

        public Task<bool> UpdateAsync(TipoInmueble entidad)
        {
            throw new NotImplementedException();
        }
    }
}