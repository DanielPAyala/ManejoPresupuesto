using Dapper;
using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Implementations.Repositories
{
    public class CuentasRepository : ICuentasRepository
    {
        private readonly string _connectionString;

        public CuentasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var con = new SqlConnection(_connectionString);
            var id = await con.QuerySingleAsync<int>(@"INSERT INTO Cuentas (Nombre, TipoCuentaId, Balance, Descripcion)
                VALUES (@Nombre, @TipoCuentaId, @Balance, @Descripcion);
                SELECT SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<Cuenta>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
                FROM Cuentas 
                INNER JOIN TiposCuentas tc ON tc.Id = Cuentas.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId
                ORDER BY Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryFirstOrDefaultAsync<Cuenta>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, Cuentas.TipoCuentaId
                FROM Cuentas 
                INNER JOIN TiposCuentas tc ON tc.Id = Cuentas.TipoCuentaId
                WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id",
                new { usuarioId, id });
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(@"UPDATE Cuentas
                SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId
                WHERE Id = @Id", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(@"DELETE Cuentas WHERE Id = @Id", new { id });
        }
    }
}
