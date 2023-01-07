using LibreriaPW.Areas.Administrador.Models;
using LibreriaPW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibreriaPW.Areas.Administrador.Controllers
{
    [Authorize(Roles = "Administrador, Supervisor")]
    [Area("Administrador")]
    public class LibrosController : Controller
    {
        private readonly librosContext context;
        private readonly IWebHostEnvironment em;
        public LibrosController(librosContext context, IWebHostEnvironment em)
        {
            this.context = context;
            this.em = em;
        }
        public IActionResult Index(IndexLibrosViewModel vm)
        {
            if (vm.IdGenero == 0)//Esto sera en caso de que no hayan seleccionado nada en el combobox
                vm.Libros = context.Libros.Include(x => x.IdgeneroNavigation).OrderBy(x => x.Titulo);
            else
                vm.Libros = context.Libros.Include(x => x.IdgeneroNavigation).Where(x => x.Idgenero == vm.IdGenero).OrderBy(x => x.Titulo);
            vm.Generos = context.Generos.OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Agregar()
        {
            LibrosViewModel vm = new LibrosViewModel();
            vm.Generos = context.Generos.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(LibrosViewModel vm)
        {
            //Tendremos que validar en el proyecto final todo lo mejor posible, aqui sera una sencilla pero no olvidar esto
            if (vm.Libro != null)
            {
                if (string.IsNullOrWhiteSpace(vm.Libro.Titulo))
                    ModelState.AddModelError("", "Escribe el titulo del Libro");
                if (context.Libros.Any(x => x.Titulo == vm.Libro.Titulo))
                    ModelState.AddModelError("", "Ya existe un libro con el mismo nombre");
                if (string.IsNullOrWhiteSpace(vm.Libro.Descripcion))
                    ModelState.AddModelError("", "Escriba la descripcion del Libro");
                if (string.IsNullOrWhiteSpace(vm.Libro.Autor))
                    ModelState.AddModelError("", "Escriba el nombre del autor del Libro");
                if (string.IsNullOrWhiteSpace(vm.Libro.Editorial))
                    ModelState.AddModelError("", "Escriba el nombre de la editorial del Libro");
                if (vm.Libro.NumeroPaginas <= 0)
                    ModelState.AddModelError("", "Escriba numero de paginas del Libro");
                if (vm.Libro.AñoEdicion <= 0)
                    ModelState.AddModelError("", "Escriba el año de edicion del Libro");
                if (vm.Libro.Precio <= 0)
                    ModelState.AddModelError("", "Escriba el precio del Libro");
                //if (vm.Libro.Idgenero == 0)
                //    ModelState.AddModelError("", "Escriba el genero del Libro");

                if (ModelState.IsValid)
                {
                    //Agregar y guardar cambios a la bd
                    context.Add(vm.Libro);
                    context.SaveChanges();
                    //agregar la imagen al Libro (le ponemos la imagen no disponible)
                    if (vm.Imagen == null)
                    {
                        string nodisp = em.WebRootPath + "/imagenesPW/0.jpg";
                        string nuevaimg = em.WebRootPath + $"/imagenesPW/{vm.Libro.Id}.jpg";
                        System.IO.File.Copy(nodisp, nuevaimg, true);
                    }
                    else
                    {
                        string nuevaimg = em.WebRootPath + $"/imagenesPW/{vm.Libro.Id}.jpg";
                        var archivo = System.IO.File.Create(nuevaimg);
                        vm.Imagen.CopyTo(archivo);
                        archivo.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            vm.Generos = context.Generos.OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Editar(int id)
        {
            var l = context.Libros.Find(id);
            if (l == null)
                return RedirectToAction("Index");
            LibrosViewModel vm = new LibrosViewModel();
            vm.Libro = l;
            vm.Generos = context.Generos.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(LibrosViewModel vm)
        {
            if (vm.Libro != null)
            {
                var l = context.Libros.Find(vm.Libro.Id);
                if (l == null)
                    return RedirectToAction("Index");
                //Validacion, hacer las demas despues
                if (string.IsNullOrWhiteSpace(vm.Libro.Titulo))
                    ModelState.AddModelError("", "Escriba el titulo del Libro");
                
                if (string.IsNullOrWhiteSpace(vm.Libro.Descripcion))
                    ModelState.AddModelError("", "Escriba la descripcion del Libro");
                if (string.IsNullOrWhiteSpace(vm.Libro.Autor))
                    ModelState.AddModelError("", "Escriba el nombre del autor del Libro");
                if (string.IsNullOrWhiteSpace(vm.Libro.Editorial))
                    ModelState.AddModelError("", "Escriba el nombre de la editorial del Libro");
                if (vm.Libro.NumeroPaginas<=0)
                    ModelState.AddModelError("", "Escriba numero de paginas del Libro");
                if (vm.Libro.AñoEdicion <= 0)
                    ModelState.AddModelError("", "Escriba el año de edicion del Libro");
                if (vm.Libro.Precio <= 0)
                    ModelState.AddModelError("", "Escriba el precio del Libro");
                //if (vm.Libro.Idgenero == 0)
                //    ModelState.AddModelError("", "Escriba el genero del Libro");

                if (ModelState.IsValid)
                {
                    l.Titulo = vm.Libro.Titulo;
                    l.Descripcion = vm.Libro.Descripcion;
                    l.Idgenero = vm.Libro.Idgenero;
                    
                    l.Precio = vm.Libro.Precio;
                    context.SaveChanges();
                }
                if (vm.Imagen != null)
                {
                    string ruta = em.WebRootPath + $"/imagenesPW/{vm.Libro.Id}.jpg";
                    var archivo = System.IO.File.Create(ruta);
                    vm.Imagen.CopyTo(archivo);
                    archivo.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Generos = context.Generos.OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var libro = context.Libros.Find(id);
            if (libro == null)
                return RedirectToAction("Index");
            return View(libro);
        }

        [HttpPost]
        public IActionResult Eliminar(Libro l)
        {
            var libro = context.Libros.Find(l.Id);
            if (libro == null)
                ModelState.AddModelError("", "El libro no existe o ya ha sido eliminado.");
            else
            {
                string ruta = em.WebRootPath + $"/imagenesPW/{libro.Id}.jpg";
                context.Remove(libro);
                context.SaveChanges();
                System.IO.File.Delete(ruta);
                return RedirectToAction("Index");
            }
            return View(l);
        }
    }
}
