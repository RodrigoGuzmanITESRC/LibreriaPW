using LibreriaPW.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LibreriaPW.Models.ViewModels;

namespace LibreriaPW.Controllers
{
    public class HomeController : Controller
    {
        private readonly librosContext context;

        public HomeController(librosContext context)
        {
            this.context = context;
        }
        [Route("/")]
        [Route("/Principal")]
        [Route("/Home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/c/{id}")]
        public IActionResult Genero(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index");
            id = id.Replace("-", " ");
            var datos = context.Generos.Include(x => x.Libros)
                .Where(x => x.Nombre == id)
                .Select(x => new GeneroViewModel
                {
                    NombreGenero = x.Nombre,
                    Libros = x.Libros.Select(x => new Libro
                    {
                        Id = x.Id,
                        Precio = x.Precio,
                        Titulo = x.Titulo
                    })
                }).FirstOrDefault();
            return View(datos);
        }
        [Route("/p/{titulo}")]
        public IActionResult Ver(string? titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return RedirectToAction("Index");
            titulo = titulo.Replace("-", " ");
            var libro = context.Libros.Include(x => x.IdgeneroNavigation).FirstOrDefault(x => x.Titulo == titulo);
            if (libro == null)
                return RedirectToAction("Index");
            return View(libro);
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IniciarSesion(Login login)
        {
            if (login.UserName == "libreria" && login.Password == "libro")
            {
                //Para acceder hay que hacer las siguientes 3 etapas
                //Crear claims
                //crear identidad
                //Autenticar
                //Claims
                var listaclaims = new List<Claim>()
                {
                    new Claim("Id","5"),
                    new Claim("Departamento","Bibliotecario"),
                    new Claim(ClaimTypes.Name,"Rodrigo Alejandro Guzman Cruz"),
                    new Claim(ClaimTypes.Role,"Administrador") //Esto es para la impersonalizacion

                };
                //crear identidad
                var identidad = new ClaimsIdentity(listaclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                //autenticar
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));
                return RedirectToAction("Index", "Home", new { Area = "Administrador" }); //index, controlador, area
            }
            else
            {
                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrecta");
                return View(login);
            }
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
