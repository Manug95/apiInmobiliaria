using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class PropietarioRepository : RepositoryBaseMySql, IPropietarioRepository
    {
        public PropietarioRepository(IConfiguration config) : base(config) { }

        public Task<int> CreateAsync(Propietario entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Propietario?> GetByIdAsync(int id)
        {
            Propietario? propietario = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        {nameof(Propietario.Id)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)}, 
                        {nameof(Propietario.Activo)},
                        {nameof(Propietario.Clave)} 
                    FROM propietarios 
                    WHERE {nameof(Propietario.Activo)} = 1
                        AND {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Propietario.Id)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Email = reader.GetString(nameof(Propietario.Email)),
                                Activo = reader.GetBoolean(nameof(Propietario.Activo)),
                                Clave = reader.GetString(nameof(Propietario.Clave))
                            };
                        }
                    }
                }
            }

            return Task.FromResult(propietario);
        }

        public Task<List<Propietario>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<Propietario?> GetByEmailAsync(string email)
        {
            Propietario? propietario = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        {nameof(Propietario.Id)}, 
                        {nameof(Propietario.Nombre)}, 
                        {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Dni)}, 
                        {nameof(Propietario.Telefono)}, 
                        {nameof(Propietario.Email)}, 
                        {nameof(Propietario.Clave)}, 
                        {nameof(Propietario.Activo)}
                    FROM propietarios 
                    WHERE {nameof(Propietario.Activo)} = 1
                        AND {nameof(Propietario.Email)} = @{nameof(Propietario.Email)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Propietario.Email)}", email);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Propietario.Id)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Email = reader.GetString(nameof(Propietario.Email)),
                                Clave = reader.GetString(nameof(Propietario.Clave)),
                                Activo = reader.GetBoolean(nameof(Propietario.Activo))
                            };
                        }
                    }
                }
            }

            return Task.FromResult(propietario);
        }

        public Task<bool> UpdateAsync(Propietario propietario)
        {
            bool modificado = false;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    UPDATE propietarios 
                    SET 
                    {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)},
                    {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)},
                    {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
                    {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)},
                    {nameof(Propietario.Email)} = @{nameof(Propietario.Email)}
                    WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Propietario.Nombre)}", propietario.Nombre);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Apellido)}", propietario.Apellido);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Dni)}", propietario.Dni);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Telefono)}", propietario.Telefono);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Email)}", propietario.Email);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", propietario.Id);

                    connection.Open();

                    modificado = command.ExecuteNonQuery() > 0;

                    connection.Close();
                }
            }

            return Task.FromResult(modificado);
        }

        public Task<bool> UpdateClaveAsync(Propietario propietario)
        {
            bool claveCambiada = false;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    UPDATE propietarios 
                    SET 
                    {nameof(Propietario.Clave)} = @{nameof(Propietario.Clave)}
                    WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Propietario.Clave)}", propietario.Clave);
                    command.Parameters.AddWithValue($"{nameof(Propietario.Id)}", propietario.Id);

                    connection.Open();

                    claveCambiada = command.ExecuteNonQuery() > 0;

                    connection.Close();
                }
            }

            return Task.FromResult(claveCambiada);
        }
    }
}