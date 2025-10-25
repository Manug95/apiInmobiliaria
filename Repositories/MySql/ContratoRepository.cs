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

        public Task<bool> UpdateAsync(Contrato entidad)
        {
            throw new NotImplementedException();
        }
    }
}