using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces.IRepositories
{
    public interface ITransaccionesRepository
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId);
        Task Crear(Transaccion transaccion);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
    }
}
