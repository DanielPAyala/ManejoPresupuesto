using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces.IRepositories
{
    public interface ICuentasRepository
    {
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
    }
}
