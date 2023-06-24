using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces.IRepositories
{
    public interface ICategoriasRepository
    {
        Task Crear(Categoria categoria);
    }
}
