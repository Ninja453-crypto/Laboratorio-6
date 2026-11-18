
using System;
using System.Collections.Generic;
using System.Linq;

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
