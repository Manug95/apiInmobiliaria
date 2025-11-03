using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class InquilinoRepository : RepositoryBaseMySql, IInquilinoRepository
    {
        public InquilinoRepository(IConfiguration config) : base(config) { }

        public Task<int> CreateAsync(Inquilino entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Inquilino?> GetByIdAsync(int id)
        {
            Inquilino? inquilino = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        {nameof(Inquilino.Id)}, 
                        {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Apellido)}, 
                        {nameof(Inquilino.Dni)}, 
                        {nameof(Inquilino.Telefono)}, 
                        {nameof(Inquilino.Email)}, 
                        {nameof(Inquilino.Activo)} 
                    FROM inquilinos 
                    WHERE {nameof(Inquilino.Activo)} = 1
                         AND {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inquilino.Id)}", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(nameof(Inquilino.Id)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                Dni = reader.GetString(nameof(Inquilino.Dni)),
                                Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                                Email = reader.GetString(nameof(Inquilino.Email)),
                                Activo = reader.GetBoolean(nameof(Inquilino.Activo))
                            };
                        }
                    }
                }
            }

            return Task.FromResult(inquilino);
        }

        public Task<List<Inquilino>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Inquilino entidad)
        {
            throw new NotImplementedException();
        }
    }
}