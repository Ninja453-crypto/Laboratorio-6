
namespace SistemaLabTech
{
    public class Equipo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public bool Disponible { get; set; }

        public Equipo(string codigo, string nombre,
                      string categoria)
        {
            Codigo = codigo;
            Nombre = nombre;
            Categoria = categoria;
            Disponible = true;
        }
    }
}
