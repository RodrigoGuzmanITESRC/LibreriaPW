using System;
using System.Collections.Generic;

namespace LibreriaPW.Models
{
    public partial class Libro
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Editorial { get; set; }
        public int? NumeroPaginas { get; set; }
        public int? AñoEdicion { get; set; }
        public int? Idgenero { get; set; }
        public decimal? Precio { get; set; }
        public string? Descripcion { get; set; }

        public virtual Genero? IdgeneroNavigation { get; set; }
    }
}
