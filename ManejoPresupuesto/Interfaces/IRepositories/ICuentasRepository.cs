using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces.IRepositories
{
    public interface ICuentasRepository
    {
        Task Crear(Cuenta cuenta);
    }
}
