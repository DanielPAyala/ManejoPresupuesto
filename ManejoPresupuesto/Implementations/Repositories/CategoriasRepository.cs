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

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<Categoria>(
                @"SELECT * FROM Categorias WHERE UsuarioId = @usuarioId",
                new { usuarioId }
                );
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

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QueryFirstOrDefaultAsync<Categoria>(
                @"SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId",
                new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var con = new SqlConnection(_connectionString);
            await con.ExecuteAsync(
                @"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                WHERE Id = @Id", categoria);
        }
    }
}
