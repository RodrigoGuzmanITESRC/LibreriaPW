using System;
using System.Collections.Generic;

namespace LibreriaPW.Models
{
    public partial class Genero
    {
        public Genero()
        {
            Libros = new HashSet<Libro>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Libro> Libros { get; set; }
    }
}
