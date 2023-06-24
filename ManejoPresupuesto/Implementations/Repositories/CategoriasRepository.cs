using Dapper;
using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Implementations.Repositories
{
    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly string _connectionString;

        public CategoriasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion");
        }

        public async Task Crear(Categoria categoria)
        {
            using var con = new SqlConnection(_connectionString);
            var id = await con.QuerySingleAsync<int>(
                @"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId) VALUES (@Nombre, @TipoOperacionId, @UsuarioId);
                SELECT SCOPE_IDENTITY();",
                categoria);

            categoria.Id = id;
        }
    }
}
