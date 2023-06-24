using AutoMapper;
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
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IMapper _mapper;

        public CuentasController(
            ITiposCuentasRepository tiposCuentasRepository, 
            IUsuariosService usuariosService, 
            ICuentasRepository cuentasRepository,
            IMapper mapper)
        {
            _tiposCuentasRepository = tiposCuentasRepository;
            _usuariosService = usuariosService;
            _cuentasRepository = cuentasRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await _cuentasRepository.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta.GroupBy(x => x.TipoCuenta)
                .Select(grupo => new CuentaIndexViewModel
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }

            await _cuentasRepository.Crear(cuenta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var modelo = _mapper.Map<CuentaCreacionViewModel>(cuenta);

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var cuenta = await _cuentasRepository.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _cuentasRepository.Actualizar(cuentaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var cuenta = await _cuentasRepository.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _cuentasRepository.Borrar(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
        {
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
