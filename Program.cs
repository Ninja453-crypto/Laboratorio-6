//Equipo
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

//Prestamos
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

//GestorLaboratorio


namespace SistemaLabTech
{
    public class GestorLaboratorio
    {
        private List<Equipo> equipos = new List<Equipo>();
        private List<Prestamo> prestamos = new List<Prestamo>();

        public IReadOnlyList<Equipo> Equipos => equipos.AsReadOnly();

        // Registrar un equipo nuevo
        public bool RegistrarEquipo(Equipo equipo, out string mensaje)
        {
            if (equipos.Any(e => e.Codigo.Equals(
                equipo.Codigo, StringComparison.OrdinalIgnoreCase)))
            {
                mensaje = "El código ya está registrado.";
                return false;
            }

            equipos.Add(equipo);
            mensaje = "Equipo registrado correctamente.";
            return true;
        }

        // Realizar un préstamo
        public bool PrestarEquipo(string codigo,
                                  string estudiante,
                                  DateTime fecha,
                                  out string mensaje)
        {
            Equipo equipo = equipos.FirstOrDefault(
                e => e.Codigo.Equals(
                    codigo, StringComparison.OrdinalIgnoreCase));

            if (equipo == null)
            {
                mensaje = "El equipo no existe.";
                return false;
            }

            if (!equipo.Disponible)
            {
                mensaje = "El equipo no está disponible.";
                return false;
            }

            equipo.Disponible = false;

            prestamos.Add(new Prestamo(
                equipo.Codigo, estudiante, fecha));

            mensaje = "Préstamo registrado correctamente.";
            return true;
        }

        // Devolver un equipo
        public bool DevolverEquipo(string codigo,
                                   DateTime fecha,
                                   out string mensaje)
        {
            Equipo equipo = equipos.FirstOrDefault(
                e => e.Codigo.Equals(
                    codigo, StringComparison.OrdinalIgnoreCase));

            if (equipo == null)
            {
                mensaje = "El código del equipo no existe.";
                return false;
            }

            if (equipo.Disponible)
            {
                mensaje = "El equipo ya se encuentra disponible.";
                return false;
            }

            equipo.Disponible = true;

            Prestamo prestamoActivo = prestamos.LastOrDefault(
                p => p.CodigoEquipo.Equals(
                    codigo, StringComparison.OrdinalIgnoreCase)
                    && p.FechaDevolucion == null);

            if (prestamoActivo != null)
            {
                prestamoActivo.FechaDevolucion = fecha;
            }

            mensaje = "Devolución realizada correctamente.";
            return true;
        }

        // Mostrar todos los equipos
        public void ListarEquipos()
        {
            if (equipos.Count == 0)
            {
                Console.WriteLine("No hay equipos registrados.");
                return;
            }

            Console.WriteLine("\n--- LISTA DE EQUIPOS ---");

            foreach (Equipo equipo in equipos)
            {
                string estado = equipo.Disponible
                    ? "Disponible"
                    : "Prestado";

                Console.WriteLine(
                    $"Código: {equipo.Codigo} | " +
                    $"Nombre: {equipo.Nombre} | " +
                    $"Categoría: {equipo.Categoria} | " +
                    $"Estado: {estado}");
            }
        }

        // Consultar disponibilidad por categoría
        public int ContarDisponiblesPorCategoria(string categoria)
        {
            return equipos.Count(e =>
                e.Categoria.Equals(
                    categoria, StringComparison.OrdinalIgnoreCase)
                && e.Disponible);
        }
    }
}


//Programa principal

namespace SistemaLabTech
{
    class Program
    {
        // Función reutilizable para validar textos
        static string LeerTextoObligatorio(string mensaje)
        {
            string valor;

            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(valor))
                {
                    Console.WriteLine(
                        "Error: el dato no puede estar vacío.");
                }

            } while (string.IsNullOrWhiteSpace(valor));

            return valor.Trim();
        }

        // Mostrar menú principal
        static void MostrarMenu()
        {
            Console.WriteLine("\n===== SISTEMA LABTECH =====");
            Console.WriteLine("1. Registrar equipo");
            Console.WriteLine("2. Prestar equipo");
            Console.WriteLine("3. Devolver equipo");
            Console.WriteLine("4. Listar equipos");
            Console.WriteLine("5. Consultar disponibilidad por categoría");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void Main(string[] args)
        {
            GestorLaboratorio gestor = new GestorLaboratorio();

            int opcion = 0;

            do
            {
                MostrarMenu();

                string entrada = Console.ReadLine() ?? "";

                if (!int.TryParse(entrada, out opcion))
                {
                    Console.WriteLine("Ingrese una opción numérica.");
                    continue;
                }

                string mensaje;

                switch (opcion)
                {
                    case 1:
                    {
                        string codigo = LeerTextoObligatorio(
                            "Código del equipo: ");

                        string nombre = LeerTextoObligatorio(
                            "Nombre del equipo: ");

                        string categoria = LeerTextoObligatorio(
                            "Categoría: ");

                        Equipo nuevoEquipo = new Equipo(
                            codigo, nombre, categoria);

                        gestor.RegistrarEquipo(nuevoEquipo, out mensaje);
                        Console.WriteLine(mensaje);
                        break;
                    }

                    case 2:
                    {
                        string codigo = LeerTextoObligatorio(
                            "Código del equipo: ");

                        string estudiante = LeerTextoObligatorio(
                            "Nombre del estudiante: ");

                        gestor.PrestarEquipo(
                            codigo, estudiante, DateTime.Now,
                            out mensaje);

                        Console.WriteLine(mensaje);
                        break;
                    }

                    case 3:
                    {
                        string codigo = LeerTextoObligatorio(
                            "Código del equipo: ");

                        gestor.DevolverEquipo(
                            codigo, DateTime.Now, out mensaje);

                        Console.WriteLine(mensaje);
                        break;
                    }

                    case 4:
                        gestor.ListarEquipos();
                        break;

                    case 5:
                    {
                        string categoria = LeerTextoObligatorio(
                            "Ingrese la categoría: ");

                        int cantidad =
                            gestor.ContarDisponiblesPorCategoria(categoria);

                        Console.WriteLine(
                            $"Equipos disponibles de {categoria}: {cantidad}");
                        break;
                    }

                    case 6:
                        Console.WriteLine(
                            "Gracias por utilizar LabTech.");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

            } while (opcion != 6);
        }
    }
}
