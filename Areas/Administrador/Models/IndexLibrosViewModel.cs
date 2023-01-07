using LibreriaPW.Models;

namespace LibreriaPW.Areas.Administrador.Models
{
    public class IndexLibrosViewModel
    {
        public IEnumerable<Genero>? Generos { get; set; }
        public IEnumerable<Libro>? Libros { get; set; }
        public int IdGenero { get; set; }
    }
}
