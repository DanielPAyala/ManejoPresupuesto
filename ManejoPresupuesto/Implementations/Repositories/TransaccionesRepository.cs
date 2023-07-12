using Dapper;
using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ManejoPresupuesto.Implementations.Repositories
{
    public class TransaccionesRepository : ITransaccionesRepository
    {
        private readonly string _connectionString;
        public TransaccionesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var con = new SqlConnection(_connectionString);
            var id = await con.QuerySingleAsync<int>("Transaccion_Insertar", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota
            }, commandType: CommandType.StoredProcedure);

            transaccion.Id = id;
        }
    }
}
