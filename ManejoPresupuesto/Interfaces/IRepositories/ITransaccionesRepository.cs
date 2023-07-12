using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces.IRepositories
{
    public interface ITransaccionesRepository
    {
        Task Crear(Transaccion transaccion);
    }
}
