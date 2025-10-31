using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class ContratoRepository : RepositoryBase, IContratoRepository
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
                        inm.{nameof(Inmueble.IdPropietario)}, 
                        inm.{nameof(Inmueble.Calle)}, 
                        inm.{nameof(Inmueble.NroCalle)}, 
                        inm.{nameof(Inmueble.Uso)}, 
                        inm.{nameof(Inmueble.IdTipoInmueble)}, 
                        ti.{nameof(Inmueble.Tipo)}, 
                        p.{nameof(Propietario.Nombre)} AS nombreProp, 
                        p.{nameof(Propietario.Apellido)} AS apellidoProp, 
                        p.{nameof(Propietario.Dni)} AS dniProp, 
                        inq.{nameof(Inquilino.Nombre)} AS nombreInq, 
                        inq.{nameof(Inquilino.Apellido)} AS apellidoInq, 
                        inq.{nameof(Inquilino.Dni)} AS dniInq, 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS inm 
                        ON c.{nameof(Contrato.IdInmueble)} = inm.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON inm.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN propietarios AS p 
                        ON inm.{nameof(Inmueble.IdPropietario)} = p.id 
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 AND c.{nameof(Contrato.Id)} = @{nameof(Contrato.Id)}"
                ;

                using (var command = new MySqlCommand(sql + ";", connection))
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
                                        Dni = reader.GetString("dniProp")
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString("nombreInq"),
                                    Apellido = reader.GetString("apellidoInq"),
                                    Dni = reader.GetString("dniInq")
                                }
                            };
                        }
                    }
                }
            }

            return Task.FromResult(contrato);
        }

        public Task<List<Contrato>> ListAsync()
        {
            throw new NotImplementedException();
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
                        inm.{nameof(Inmueble.IdPropietario)}, 
                        inm.{nameof(Inmueble.IdTipoInmueble)}, 
                        inm.{nameof(Inmueble.Calle)}, 
                        inm.{nameof(Inmueble.NroCalle)}, 
                        inm.{nameof(Inmueble.Precio)}, 
                        inm.{nameof(Inmueble.CantidadAmbientes)}, 
                        inm.{nameof(Inmueble.Foto)}, 
                        inm.{nameof(Inmueble.Disponible)}, 
                        inm.{nameof(Inmueble.Latitud)}, 
                        inm.{nameof(Inmueble.Longitud)}, 
                        inm.{nameof(Inmueble.Uso)}, 
                        ti.{nameof(Inmueble.Tipo)}, 
                        inq.{nameof(Inquilino.Nombre)}, 
                        inq.{nameof(Inquilino.Apellido)}, 
                        inq.{nameof(Inquilino.Telefono)}, 
                        inq.{nameof(Inquilino.Dni)}, 
                        inq.{nameof(Inquilino.Email)} 
                    FROM contratos AS c 
                    INNER JOIN inmuebles AS inm 
                        ON c.{nameof(Contrato.IdInmueble)} = inm.id 
                    INNER JOIN tipos_inmueble AS ti 
                        ON inm.{nameof(Inmueble.IdTipoInmueble)} = ti.id 
                    INNER JOIN inquilinos AS inq 
                        ON c.{nameof(Contrato.IdInquilino)} = inq.id 
                    WHERE c.{nameof(Contrato.Borrado)} = 0 
                        AND i.{nameof(Inmueble.IdPropietario)} = @{nameof(Inmueble.IdPropietario)}
                        AND c.{nameof(Contrato.FechaTerminado)} IS NULL
                        AND @hoy BETWEEN c.{nameof(Contrato.FechaInicio)} AND c.{nameof(Contrato.FechaFin)}"
                ;

                if (offset.HasValue && limit.HasValue)
                        sql += $" LIMIT @limit OFFSET @offset";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Inmueble.IdPropietario)}", idProp);
                    command.Parameters.AddWithValue("hoy", DateTime.Today);

                    if (offset.HasValue && limit.HasValue)
                    {
                        command.Parameters.AddWithValue("limit", limit.Value);
                        command.Parameters.AddWithValue("offset", offset.Value);
                    }

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
                                    Foto = reader.GetString(nameof(Inmueble.Foto)),
                                    Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                                    Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                                    Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                                    Tipo = new TipoInmueble
                                    {
                                        Id = reader.GetInt32(nameof(Inmueble.IdTipoInmueble)),
                                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo))
                                    }
                                },
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                    Dni = reader.GetString(nameof(Inquilino.Dni)),
                                    Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                                    Email = reader.GetString(nameof(Inquilino.Email))
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