
namespace SistemaLabTech
{
    public class Prestamo
    {
        public string CodigoEquipo { get; set; }
        public string Estudiante { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public Prestamo(string codigoEquipo,
                        string estudiante,
                        DateTime fechaPrestamo)
        {
            CodigoEquipo = codigoEquipo;
            Estudiante = estudiante;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = null;
        }
    }
}
