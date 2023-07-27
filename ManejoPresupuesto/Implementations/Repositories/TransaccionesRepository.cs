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

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync("Transaccion_Actualizar", new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CuentaId,
                transaccion.CategoriaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryFirstOrDefaultAsync<Transaccion>(
                @"SELECT Transacciones.*, cat.TipoOperacionId
                FROM Transacciones INNER JOIN Categorias cat
                ON cat.Id = Transacciones.CategoriaId
                WHERE Transacciones.Id = @Id AND Transacciones.UsuarioId = @UsuarioId",
                new { id, usuarioId });
        }
    }
}
