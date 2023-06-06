using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Implementations
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
            var id = await con.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden) 
                                            VALUES (@Nombre, @UsuarioId, 0);
                                            SELECT SCOPE_IDENTITY();", tipoCuenta);
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
    }
}
