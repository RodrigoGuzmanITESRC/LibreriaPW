using LibreriaPW.Models;

namespace LibreriaPW.Areas.Administrador.Models
{
    public class LibrosViewModel
    {
        public Libro? Libro { get; set; }
        public IEnumerable<Genero>? Generos { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
