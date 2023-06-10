using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Interfaces.IServices;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IUsuariosService _usuariosService;

        public CuentasController(ITiposCuentasRepository tiposCuentasRepository, IUsuariosService usuariosService)
        {
            _tiposCuentasRepository = tiposCuentasRepository;
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);

            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

            return View(modelo);
        }
    }
}
