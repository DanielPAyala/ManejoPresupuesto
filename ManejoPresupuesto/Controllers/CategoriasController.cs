using ManejoPresupuesto.Interfaces.IRepositories;
using ManejoPresupuesto.Interfaces.IServices;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriasRepository _categoriasRepository;
        private readonly IUsuariosService _usuariosService;

        public CategoriasController(ICategoriasRepository categoriasRepository, IUsuariosService usuariosService)
        {
            _categoriasRepository = categoriasRepository;
            _usuariosService = usuariosService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();
            var categorias = await _categoriasRepository.Obtener(usuarioId);
            return View(categorias);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            var usuarioId = _usuariosService.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.UsuarioId = usuarioId;
            await _categoriasRepository.Crear(categoria);
            return RedirectToAction("Index");
        }
    }
}
