using LibreriaPW.Models;
namespace LibreriaPW.Services
   
{
    public class MenuServices
    {
        librosContext cx; 
        public MenuServices(librosContext context)
        {
            cx = context;
        }
        public IEnumerable<Genero> Get()
        {
            return cx.Generos.Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
        }
    }
}
