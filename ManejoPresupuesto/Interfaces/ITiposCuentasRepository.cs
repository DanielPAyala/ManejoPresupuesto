using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface ITiposCuentasRepository
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
    }
}
