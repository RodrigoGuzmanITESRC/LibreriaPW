using LibreriaPW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibreriaPW.Areas.Administrador.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Administrador")]
    public class GenerosController : Controller
    {
        private librosContext context;
        public GenerosController(librosContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var generos = context.Generos.Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
            return View(generos);
        }

        //GET
        public IActionResult Agregar()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Agregar(Genero g)
        {
            if (string.IsNullOrWhiteSpace(g.Nombre))
                ModelState.AddModelError("", "El nombre del genero no deberia de estar vacio");
            if (context.Generos.Any(x => x.Nombre == g.Nombre))
                ModelState.AddModelError("", "Ya existe un genero con el mismo nombre");

           
            if (ModelState.IsValid)
            {
                context.Add(g);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(g);
        }

        //GET
        public IActionResult Editar(int id)
        {
            var genero = context.Generos.Find(id);
            if (genero == null)
                return RedirectToAction("Index");
            return View(genero);
        }

        //POST
        [HttpPost]
        public IActionResult Editar(Genero g)
        {
            if (string.IsNullOrWhiteSpace(g.Nombre))
                ModelState.AddModelError("", "Favor de escribir el nombre del genero");
            if (context.Generos.Any(x => x.Nombre == g.Nombre && x.Id != g.Id))
                ModelState.AddModelError("", "Favor de escribir el nombre del genero");
            if (ModelState.IsValid)
            {
                var genero = context.Generos.Find(g.Id);
                if (genero == null)
                    return RedirectToAction("Index");
                genero.Nombre = g.Nombre;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(g);
        }

        //GET
        public IActionResult Eliminar(int id)
        {
            var genero = context.Generos.Find(id);
            if (genero == null)
                return RedirectToAction("Index");
            return View(genero);
        }

        //POST
        [HttpPost]
        public IActionResult Eliminar(Genero g)
        {
            var genero = context.Generos.Find(g.Id);
            if (genero == null)
                ModelState.AddModelError("", "El genero no existe o ya ha sido eliminado");
            else
            {
                if (context.Libros.Any(x => x.Idgenero == g.Id))
                    ModelState.AddModelError("", "El genero no se puede eliminar debido a que tiene libros");
                if (ModelState.IsValid)
                {
                    context.Remove(genero);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(genero);
        }
    }
}
