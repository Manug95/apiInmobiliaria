using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class InmuebleRepository : RepositoryBase, IInmuebleRepository
    {
        public InmuebleRepository(IConfiguration config) : base(config) { }

        public Task<int> CountByPropietarioAsync(int idProp)
        {
            int cantidadInmuebles = 0;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT COUNT({nameof(Inmueble.Id)}) AS cantidad 
                    FROM inmuebles 
                    WHERE {nameof(Inmueble.Borrado)} = 0
                         AND {nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)}"
                ;

                /*if (disponible.HasValue && disponible.Value >= 0 && disponible.Value < 2)
                    sql += $" AND {nameof(Inmueble.Disponible)} = {disponible}";*/

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);

                    connection.Open();

                    cantidadInmuebles = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();
                }
            }

            return Task.FromResult(cantidadInmuebles);
        }

        public Task<int> CreateAsync(Inmueble inmueble)
        {
            int id = 0;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    INSERT INTO inmuebles 
                    (
                        {nameof(Inmueble.IdPropietario)}, 
                        {nameof(Inmueble.IdTipoInmueble)}, 
                        {nameof(Inmueble.Uso)}, 
                        {nameof(Inmueble.CantidadAmbientes)}, 
                        {nameof(Inmueble.Calle)}, 
                        {nameof(Inmueble.NroCalle)}, 
                        {nameof(Inmueble.Latitud)}, 
                        {nameof(Inmueble.Longitud)}, 
                        {nameof(Inmueble.Precio)},
                        {nameof(Inmueble.Disponible)}
                    )
                    VALUES 
                    (
                        @{nameof(Inmueble.IdPropietario)}, 
                        @{nameof(Inmueble.IdTipoInmueble)}, 
                        @{nameof(Inmueble.Uso)}, 
                        @{nameof(Inmueble.CantidadAmbientes)}, 
                        @{nameof(Inmueble.Calle)}, 
                        @{nameof(Inmueble.NroCalle)}, 
                        @{nameof(Inmueble.Latitud)}, 
                        @{nameof(Inmueble.Longitud)}, 
                        @{nameof(Inmueble.Precio)},
                        @{nameof(Inmueble.Disponible)}
                    );
                    
                    SELECT LAST_INSERT_ID();"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", inmueble.IdPropietario);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdTipoInmueble)}", inmueble.IdTipoInmueble);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Uso)}", inmueble.Uso);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.CantidadAmbientes)}", inmueble.CantidadAmbientes);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Calle)}", inmueble.Calle);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.NroCalle)}", inmueble.NroCalle);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Precio)}", inmueble.Precio);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Disponible)}", inmueble.Disponible);

                    try
                    {
                        connection.Open();

                        id = Convert.ToInt32(command.ExecuteScalar());
                        inmueble.Id = id;
                    }
                    catch (MySqlException mye)
                    {
                        Console.WriteLine(mye.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    connection.Close();
                }
            }

            return Task.FromResult(id);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDisponibleAsync(Inmueble inmueble)
        {
            bool modificado = false;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    UPDATE inmuebles 
                    SET {nameof(Inmueble.Disponible)} = @{nameof(Inmueble.Disponible)} 
                    WHERE  {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Disponible)}", inmueble.Disponible);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", inmueble.Id);

                    connection.Open();

                    modificado = command.ExecuteNonQuery() > 0;

                    connection.Close();
                }
            }

            return Task.FromResult(modificado);
        }

        public Task<Inmueble?> GetByIdAsync(int id)
        {
            Inmueble? inmueble = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        i.{nameof(Inmueble.Id)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        IFNULL(i.{nameof(Inmueble.Latitud)}, 0) AS latitud, 
                        IFNULL(i.{nameof(Inmueble.Longitud)}, 0) AS longitud, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        ti.{nameof(TipoInmueble.Tipo)} AS tipoInmueble, 
                        p.{nameof(Propietario.Nombre)} AS nombreDuenio, 
                        p.{nameof(Propietario.Apellido)} AS apellidoDuenio, 
                        p.{nameof(Propietario.Dni)} AS dniDuenio 
                    FROM inmuebles AS i 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    WHERE i.{nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)} AND {nameof(Inmueble.Borrado)} = 0;"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Id)),
                                IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Uso = reader.GetString(nameof(Inmueble.Uso)),
                                CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                Calle = reader.GetString(nameof(Inmueble.Calle)),
                                NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                Latitud = reader.GetDecimal("latitud"),
                                Longitud = reader.GetDecimal("longitud"),
                                Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                Foto = reader.GetString(nameof(Inmueble.Foto)),
                                Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                Duenio = new Propietario
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    Nombre = reader.GetString("nombreDuenio"),
                                    Apellido = reader.GetString("apellidoDuenio"),
                                    Dni = reader.GetString("dniDuenio")
                                },
                                Tipo = new TipoInmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Tipo = reader.GetString("tipoInmueble")
                                }
                            };
                        }
                    }
                }
            }

            return Task.FromResult(inmueble);
        }

        public Task<List<Inmueble>> ListAsync()
        {
            var inmuebles = new List<Inmueble>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        i.{nameof(Inmueble.Id)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        IFNULL(i.{nameof(Inmueble.Latitud)}, 0) AS latitud, 
                        IFNULL(i.{nameof(Inmueble.Longitud)}, 0) AS longitud, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        ti.{nameof(TipoInmueble.Tipo)}, 
                        p.{nameof(Propietario.Nombre)}, 
                        p.{nameof(Propietario.Apellido)}, 
                        p.{nameof(Propietario.Dni)} 
                    FROM inmuebles AS i 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    WHERE {nameof(Inmueble.Borrado)} = 0"
                ;

                /*if (disponible >= 0 && disponible < 2)
                    sql += $" AND {nameof(Inmueble.Disponible)} = {disponible}";

                if (!string.IsNullOrWhiteSpace(nomApeProp))
                    sql += $" AND (p.{nameof(Propietario.Nombre)} LIKE @nomApe OR p.{nameof(Propietario.Apellido)} LIKE @nomApe)";

                if (offset.HasValue && limit.HasValue)
                    sql += $" LIMIT @limit OFFSET @offset";*/

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    /*if (!string.IsNullOrWhiteSpace(nomApeProp)) command.Parameters.AddWithValue($"nomApe", $"%{nomApeProp}%");
                    if (offset.HasValue && limit.HasValue)
                    {
                        command.Parameters.AddWithValue($"limit", limit.Value);
                        command.Parameters.AddWithValue($"offset", (offset.Value - 1) * limit.Value);
                    }*/

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new Inmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Id)),
                                IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Uso = reader.GetString(nameof(Inmueble.Uso)),
                                CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                Calle = reader.GetString(nameof(Inmueble.Calle)),
                                NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                Latitud = reader.GetDecimal("latitud"),
                                Longitud = reader.GetDecimal("longitud"),
                                Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                Foto = reader.GetString(nameof(Inmueble.Foto)),
                                Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                Duenio = new Propietario
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                    Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                    Dni = reader.GetString(nameof(Propietario.Dni))
                                },
                                Tipo = new TipoInmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(inmuebles);
        }

        public Task<List<Inmueble>> ListActualmenteAlquiladosAsync(int idProp, int? offset, int? limit)
        {
            var inmuebles = new List<Inmueble>();

                using (var connection = new MySqlConnection(connectionString))
                {
                    string sql = @$"
                        SELECT 
                            i.{nameof(Inmueble.Id)}, 
                            i.{nameof(Inmueble.IdPropietario)}, 
                            i.{nameof(Inmueble.IdTipoInmueble)}, 
                            i.{nameof(Inmueble.Uso)}, 
                            i.{nameof(Inmueble.CantidadAmbientes)}, 
                            i.{nameof(Inmueble.Calle)}, 
                            i.{nameof(Inmueble.NroCalle)}, 
                            IFNULL(i.{nameof(Inmueble.Latitud)}, 0) AS latitud, 
                            IFNULL(i.{nameof(Inmueble.Longitud)}, 0) AS longitud, 
                            i.{nameof(Inmueble.Precio)}, 
                            i.{nameof(Inmueble.Disponible)}, 
                            i.{nameof(Inmueble.Foto)}, 
                            ti.{nameof(TipoInmueble.Tipo)} AS tipoInmueble, 
                            p.{nameof(Propietario.Nombre)} AS nombreProp, 
                            p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                            p.{nameof(Propietario.Dni)} AS dniProp 
                        FROM inmuebles AS i 
                        INNER JOIN tipos_inmueble AS ti 
                            ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                        INNER JOIN propietarios AS p 
                            ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                        INNER JOIN contratos as c
                            ON c.{nameof(Contrato.IdInmueble)} = i.id 
                        WHERE i.{nameof(Inmueble.Borrado)} = 0 
                            AND i.{nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)}
                            AND (@hoy BETWEEN c.{nameof(Contrato.FechaInicio)} AND c.{nameof(Contrato.FechaFin)})
                            AND c.{nameof(Contrato.FechaTerminado)} IS NULL"
                    ;

                    if (offset.HasValue)
                        sql += $" OFFSET @offset";
                    if (limit.HasValue)
                        sql += $" LIMIT @limit";

                    using (var command = new MySqlCommand(sql + ";", connection))
                    {
                        command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);
                        command.Parameters.AddWithValue("hoy", DateTime.Today);

                        if (offset.HasValue && limit.HasValue)
                            command.Parameters.AddWithValue($"offset", offset.Value);
                        if (limit.HasValue)
                            command.Parameters.AddWithValue($"limit", limit.Value);

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                inmuebles.Add(new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.Id)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Latitud = reader.GetDecimal("latitud"),
                                    Longitud = reader.GetDecimal("longitud"),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Foto = reader.GetString(nameof(Inmueble.Foto)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp")
                                    },
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString("tipoInmueble")
                                    }
                                });
                            }
                        }
                    }
                }

                return Task.FromResult(inmuebles);
        }

        public Task<List<Inmueble>> ListByPropietarioAsync(int idProp, int? offset, int? limit)
        {
            var inmuebles = new List<Inmueble>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        i.{nameof(Inmueble.Id)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        IFNULL(i.{nameof(Inmueble.Latitud)}, 0) AS latitud, 
                        IFNULL(i.{nameof(Inmueble.Longitud)}, 0) AS longitud, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        ti.{nameof(TipoInmueble.Tipo)} AS tipoInmueble, 
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp 
                    FROM inmuebles AS i 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    WHERE i.{nameof(Inmueble.Borrado)} = 0 AND i.{nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)}"
                ;

                if (offset.HasValue)
                    sql += $" OFFSET @offset";
                if (limit.HasValue)
                    sql += $" LIMIT @limit";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);

                    if (offset.HasValue && limit.HasValue)
                        command.Parameters.AddWithValue($"offset", offset.Value);
                    if (limit.HasValue)
                        command.Parameters.AddWithValue($"limit", limit.Value);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inmuebles.Add(new Inmueble
                            {
                                Id = reader.GetInt32(nameof(Inmueble.Id)),
                                IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                Uso = reader.GetString(nameof(Inmueble.Uso)),
                                CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                Calle = reader.GetString(nameof(Inmueble.Calle)),
                                NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                Latitud = reader.GetDecimal("latitud"),
                                Longitud = reader.GetDecimal("longitud"),
                                Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                Foto = reader.GetString(nameof(Inmueble.Foto)),
                                Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                Duenio = new Propietario
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    Nombre = reader.GetString("nombreProp"),
                                    Apellido = reader.GetString("apellidoProp"),
                                    Dni = reader.GetString("dniProp")
                                },
                                Tipo = new TipoInmueble
                                {
                                    Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Tipo = reader.GetString("tipoInmueble")
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(inmuebles);
        }

        public Task<bool> UpdateAsync(Inmueble inmueble)
        {
            bool modificado = false;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
                UPDATE inmuebles 
                SET 
                {nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)},
                {nameof(Inmueble.IdTipoInmueble)} = @{nameof(Inmueble.IdTipoInmueble)},
                {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                {nameof(Inmueble.CantidadAmbientes)} = @{nameof(Inmueble.CantidadAmbientes)},
                {nameof(Inmueble.Calle)} = @{nameof(Inmueble.Calle)}, 
                {nameof(Inmueble.NroCalle)} = @{nameof(Inmueble.NroCalle)}, 
                {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)}, 
                {nameof(Inmueble.Disponible)} = @{nameof(Inmueble.Disponible)},  
                {nameof(Inmueble.Foto)} = @{nameof(Inmueble.Foto)} 
                WHERE {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};"
            ;

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", inmueble.IdPropietario);
                command.Parameters.AddWithValue($"{nameof(Inmueble.IdTipoInmueble)}", inmueble.IdTipoInmueble);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"{nameof(Inmueble.CantidadAmbientes)}", inmueble.CantidadAmbientes);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Calle)}", inmueble.Calle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.NroCalle)}", inmueble.NroCalle);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Precio)}", inmueble.Precio);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Disponible)}", inmueble.Disponible);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Foto)}", inmueble.Foto);
                command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", inmueble.Id);

                connection.Open();

                modificado = command.ExecuteNonQuery() > 0;

                connection.Close();
            }
        }

        return Task.FromResult(modificado);
        }

        public Task<bool> UpdateFotoAsync(Inmueble inmueble)
        {
            bool modificado = false;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    UPDATE inmuebles 
                    SET {nameof(Inmueble.Foto)} = @{nameof(Inmueble.Foto)} 
                    WHERE  {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Foto)}", inmueble.Foto);
                    command.Parameters.AddWithValue($"{nameof(Inmueble.Id)}", inmueble.Id);

                    connection.Open();

                    modificado = command.ExecuteNonQuery() > 0;

                    connection.Close();
                }
            }

            return Task.FromResult(modificado);
        }
    }
}