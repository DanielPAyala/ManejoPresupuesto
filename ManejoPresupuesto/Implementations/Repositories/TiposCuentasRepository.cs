using Dapper;
using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Implementations.Repositories
{
    public class TiposCuentasRepository : ITiposCuentasRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TiposCuentasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Conexion");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var con = new SqlConnection(_connectionString);
            var id = await con.QuerySingleAsync<int>(
                "TiposCuentas_Insertar", new
                {
                    Nombre = tipoCuenta.Nombre,
                    UsuarioId = tipoCuenta.UsuarioId
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            var existe = await con.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 FROM TiposCuentas WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                new { nombre, usuarioId }
            );
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<TipoCuenta>(
                @"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE UsuarioId = @UsuarioId ORDER BY Orden",
                new { usuarioId }
            );
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(@"UPDATE TiposCuentas 
                            SET Nombre = @Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryFirstOrDefaultAsync<TipoCuenta>(
                @"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId",
                new { id, usuarioId }
            );
        }

        public async Task Borrar(int id)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(@"DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tiposCuentasOrdenados)
        {
            var query = "UPDATE TiposCuentas SET Orden = @Orden WHERE Id = @Id";
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(query, tiposCuentasOrdenados);
        }
    }
}
