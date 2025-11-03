using api_inmobiliaria.Interfaces;
using InmobiliariaGutierrezManuel.Models;
using MySql.Data.MySqlClient;

namespace api_inmobiliaria.Repositories.MySql
{
    public class PagoRepository : RepositoryBaseMySql, IPagoRepository
    {
        public PagoRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> CountByContratoAsync(int idCon)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(Pago entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pago?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ListAsync(int? offset, int? limit)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ListByContratoAsync(int idCon, int? offset, int? limit)
        {
            var pagos = new List<Pago>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"
                    SELECT 
                        p.{nameof(Pago.Id)},  
                        p.{nameof(Pago.IdContrato)},  
                        p.{nameof(Pago.Fecha)}, 
                        p.{nameof(Pago.Importe)}, 
                        p.{nameof(Pago.Tipo)} AS tipoPago, 
                        p.{nameof(Pago.Estado)}, 
                        IFNULL({nameof(Pago.Detalle)}, 'Sin Detalle') AS det, 
                        c.{nameof(Contrato.FechaInicio)}, 
                        c.{nameof(Contrato.FechaFin)}, 
                        c.{nameof(Contrato.FechaTerminado)}, 
                        c.{nameof(Contrato.IdInquilino)}, 
                        c.{nameof(Contrato.IdInmueble)}, 
                        c.{nameof(Contrato.MontoMensual)} 
                    FROM pagos AS p 
                    INNER JOIN contratos AS c 
                        ON p.{nameof(Pago.IdContrato)} = c.{nameof(Contrato.Id)} 
                    WHERE p.{nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)}"
                ;

                if (offset.HasValue && offset.Value > 0)
                    sql += " OFFSET @offset";
                if (limit.HasValue)
                    sql += " LIMIT @limit";

                using (var command = new MySqlCommand(sql + ";", connection))
                {
                    command.Parameters.AddWithValue($"{nameof(Pago.IdContrato)}", idCon);

                    if (offset.HasValue && offset.Value > 0)
                        command.Parameters.AddWithValue("offset", offset.Value);
                    if (limit.HasValue)
                        command.Parameters.AddWithValue("limit", limit.Value);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagos.Add(new Pago
                            {
                                Id = reader.GetInt32(nameof(Pago.Id)),
                                Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                                Importe = reader.GetDecimal(nameof(Pago.Importe)),
                                Tipo = reader.GetString("tipoPago"),
                                Detalle = reader.GetString("det"),
                                Estado = reader.GetBoolean(nameof(Pago.Estado)),
                                Contrato = new Contrato
                                {
                                    Id = reader.GetInt32(nameof(Pago.IdContrato)),
                                    FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                                    FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                                    FechaTerminado = reader[nameof(Contrato.FechaTerminado)] == DBNull.Value ? null : reader.GetDateTime(nameof(Contrato.FechaTerminado)),
                                    IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                    IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble))
                                }
                            });
                        }
                    }
                }
            }

            return Task.FromResult(pagos);
        }

        public Task<bool> UpdateAsync(Pago entidad)
        {
            throw new NotImplementedException();
        }
    }
}