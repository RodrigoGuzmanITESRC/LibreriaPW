namespace LibreriaPW.Models.ViewModels
{
    public class GeneroViewModel
    {
        public string? NombreGenero { get; set; } = "";
        public IEnumerable<Libro>? Libros { get; set; }
    }
}
