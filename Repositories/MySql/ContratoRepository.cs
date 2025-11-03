using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class ContratoRepository : RepositoryBaseMySql, IContratoRepository
    {
        public ContratoRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> CreateAsync(Contrato entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Contrato?> GetByIdAsync(int id)
        {
            Contrato? contrato = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        c.{nameof(Contrato.Id)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.MontoMensual)}, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)},   
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        i.{nameof(Inmueble.Latitud)}, 
                        i.{nameof(Inmueble.Longitud)}, 
                        i.{nameof(Inmueble.Precio)},
                        ti.{nameof(Inmueble.Tipo)}, 
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        p.{nameof(Propietario.Email)} AS emailProp, 
                        p.{nameof(Propietario.Telefono)} AS telProp, 
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                        inq.{nameof(Inquilino.Email)} AS emailInq, 
                        inq.{nameof(Inquilino.Telefono)} AS telInq 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS i 
                        ON c.{nameof(Contrato.IdInmueble)} = i.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 AND c.{nameof(Contrato.Id)} = @{nameof(Contrato.Id)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Contrato.Id)}", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                Id = reader.GetInt32(nameof(Contrato.Id)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Foto = reader[nameof(Inmueble.Foto)] == DBNull.Value ? null : reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader[nameof(Inmueble.Latitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader[nameof(Inmueble.Longitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    },
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp"),
                                        Email = reader.GetString("emailProp"),
                                        Telefono = reader.GetString("telProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq"),
                                    Email = reader.GetString("emailInq"),
                                    Telefono = reader.GetString("telInq")
                                }
                            };
                        }
                    }
                }
            }

            return Task.FromResult(contrato);
        }

        public Task<Contrato?> GetVigenteByInmuebleAsync(int idInmueble)
        {
            Contrato? contrato = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        c.{nameof(Contrato.Id)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.MontoMensual)}, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        i.{nameof(Inmueble.Latitud)}, 
                        i.{nameof(Inmueble.Longitud)}, 
                        i.{nameof(Inmueble.Precio)}, 
                        ti.{nameof(Inmueble.Tipo)}, 
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        p.{nameof(Propietario.Email)} AS emailProp, 
                        p.{nameof(Propietario.Telefono)} AS telProp,
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                        inq.{nameof(Inquilino.Email)} AS emailInq, 
                        inq.{nameof(Inquilino.Telefono)} AS telInq 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS i 
                        ON c.{nameof(Contrato.IdInmueble)} = i.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 AND c.{nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)};"
                ;

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Contrato.IdInmueble)}", idInmueble);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                Id = reader.GetInt32(nameof(Contrato.Id)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Foto = reader[nameof(Inmueble.Foto)] == DBNull.Value ? null : reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader[nameof(Inmueble.Latitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader[nameof(Inmueble.Longitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    },
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp"),
                                        Email = reader.GetString("emailProp"),
                                        Telefono = reader.GetString("telProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq"),
                                    Email = reader.GetString("emailInq"),
                                    Telefono = reader.GetString("telInq")
                                }
                            };
                        }
                    }
                }
            }

            return Task.FromResult(contrato);
        }

        public Task<List<Contrato>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contrato>> ListByInmuebleAsync(int idInmueble, int? offset, int? limit)
        {
            var contratos = new List<Contrato>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        c.{nameof(Contrato.Id)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.MontoMensual)}, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Latitud)}, 
                        i.{nameof(Inmueble.Longitud)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        ti.{nameof(Inmueble.Tipo)},  
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        p.{nameof(Propietario.Email)} AS emailProp, 
                        p.{nameof(Propietario.Telefono)} AS telProp,
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                        inq.{nameof(Inquilino.Email)} AS emailInq, 
                        inq.{nameof(Inquilino.Telefono)} AS telInq 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS i 
                        ON c.{nameof(Contrato.IdInmueble)} = i.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 
                        AND c.{nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)}"
                ;

                if (offset.HasValue && offset.Value > 0)
                    sql += " OFFSET @offset";
                if (limit.HasValue)
                    sql += " LIMIT @limit";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Contrato.IdInmueble)}", idInmueble);

                    if (offset.HasValue && offset.Value > 0)
                        command.Parameters.AddWithValue("offset", offset.Value);
                    if (limit.HasValue)
                        command.Parameters.AddWithValue("limit", limit.Value);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(new Contrato
                            {
                                Id = reader.GetInt32(nameof(Contrato.Id)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    Foto = reader[nameof(Inmueble.Foto)] == DBNull.Value ? null : reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader[nameof(Inmueble.Latitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader[nameof(Inmueble.Longitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    },
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp"),
                                        Email = reader.GetString("emailProp"),
                                        Telefono = reader.GetString("telProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq"),
                                    Email = reader.GetString("emailInq"),
                                    Telefono = reader.GetString("telInq")
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(contratos);
        }

        public Task<List<Contrato>> ListByPropietario(int idProp, int? offset, int? limit)
        {
            var contratos = new List<Contrato>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        c.{nameof(Contrato.Id)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.MontoMensual)}, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Latitud)}, 
                        i.{nameof(Inmueble.Longitud)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        ti.{nameof(Inmueble.Tipo)},  
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        p.{nameof(Propietario.Email)} AS emailProp, 
                        p.{nameof(Propietario.Telefono)} AS telProp,
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                        inq.{nameof(Inquilino.Email)} AS emailInq, 
                        inq.{nameof(Inquilino.Telefono)} AS telInq 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS i 
                        ON c.{nameof(Contrato.IdInmueble)} = i.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 
                        AND i.{nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)}"
                ;

                if (offset.HasValue && offset.Value > 0)
                    sql += " OFFSET @offset";
                if (limit.HasValue)
                    sql += " LIMIT @limit";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);

                    if (offset.HasValue && offset.Value > 0)
                        command.Parameters.AddWithValue("offset", offset.Value);
                    if (limit.HasValue)
                        command.Parameters.AddWithValue("limit", limit.Value);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(new Contrato
                            {
                                Id = reader.GetInt32(nameof(Contrato.Id)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    Foto = reader[nameof(Inmueble.Foto)] == DBNull.Value ? null : reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader[nameof(Inmueble.Latitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader[nameof(Inmueble.Longitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    },
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp"),
                                        Email = reader.GetString("emailProp"),
                                        Telefono = reader.GetString("telProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq"),
                                    Email = reader.GetString("emailInq"),
                                    Telefono = reader.GetString("telInq")
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(contratos);
        }

        public Task<List<Contrato>> ListVigentesByPropietario(int idProp, int? offset, int? limit)
        {
            var contratos = new List<Contrato>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        c.{nameof(Contrato.Id)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.MontoMensual)}, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        i.{nameof(Inmueble.IdPropietario)}, 
                        i.{nameof(Inmueble.IdTipoInmueble)}, 
                        i.{nameof(Inmueble.Calle)}, 
                        i.{nameof(Inmueble.NroCalle)}, 
                        i.{nameof(Inmueble.Precio)}, 
                        i.{nameof(Inmueble.CantidadAmbientes)}, 
                        i.{nameof(Inmueble.Foto)}, 
                        i.{nameof(Inmueble.Disponible)}, 
                        i.{nameof(Inmueble.Latitud)}, 
                        i.{nameof(Inmueble.Longitud)}, 
                        i.{nameof(Inmueble.Uso)}, 
                        ti.{nameof(Inmueble.Tipo)},  
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        p.{nameof(Propietario.Email)} AS emailProp, 
                        p.{nameof(Propietario.Telefono)} AS telProp,
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                        inq.{nameof(Inquilino.Email)} AS emailInq, 
                        inq.{nameof(Inquilino.Telefono)} AS telInq 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS i 
                        ON c.{nameof(Contrato.IdInmueble)} = i.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON i.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON i.{nameof(Inmueble.IdPropietario)} = p.id 
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 
                        AND i.{nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)} 
                        AND c.{nameof(Contrato.FechaTerminado)} IS NULL
                        AND @hoy BETWEEN c.{nameof(Contrato.FechaInicio)} AND c.{nameof(Contrato.FechaFin)}"
                ;

                if (offset.HasValue && offset.Value > 0)
                    sql += " OFFSET @offset";
                if (limit.HasValue)
                    sql += " LIMIT @limit";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);
                    command.Parameters.AddWithValue("hoy", DateTime.Today);

                    if (offset.HasValue && offset.Value > 0)
                        command.Parameters.AddWithValue("offset", offset.Value);
                    if (limit.HasValue)
                        command.Parameters.AddWithValue("limit", limit.Value);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(new Contrato
                            {
                                Id = reader.GetInt32(nameof(Contrato.Id)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                    IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                    IdTipoInmueble = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                    Calle = reader.GetString(nameof(Inmueble.Calle)),
                                    NroCalle = reader.GetInt32(nameof(Inmueble.NroCalle)),
                                    Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                                    CantidadAmbientes = reader.GetInt32(nameof(Inmueble.CantidadAmbientes)),
                                    Uso = reader.GetString(nameof(Inmueble.Uso)),
                                    Foto = reader[nameof(Inmueble.Foto)] == DBNull.Value ? null : reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader[nameof(Inmueble.Latitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader[nameof(Inmueble.Longitud)] == DBNull.Value ? 0 : reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    },
                                    Duenio = new Propietario
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                                        Nombre = reader.GetString("nombreProp"),
                                        Apellido = reader.GetString("apellidoProp"),
                                        Dni = reader.GetString("dniProp"),
                                        Email = reader.GetString("emailProp"),
                                        Telefono = reader.GetString("telProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq"),
                                    Email = reader.GetString("emailInq"),
                                    Telefono = reader.GetString("telInq")
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(contratos);
        }

        public Task<bool> UpdateAsync(Contrato entidad)
        {
            throw new NotImplementedException();
        }
    }
}