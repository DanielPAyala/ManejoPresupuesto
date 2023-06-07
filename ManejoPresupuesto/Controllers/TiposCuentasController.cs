using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Interfaces.IServices;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IUsuariosService _usuariosService;

        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository, IUsuariosService usuariosService)
        {
            _tiposCuentasRepository = tiposCuentasRepository;
            _usuariosService = usuariosService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = _usuariosService.ObtenerUsuarioId();

            var yaExisteTipoCuenta = await _tiposCuentasRepository.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }

            await _tiposCuentasRepository.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await _tiposCuentasRepository.Existe(nombre, usuarioId);

            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe.");
            }

            return Json(true);
        }
    }
}
