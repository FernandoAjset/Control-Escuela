using System;

namespace CoreEscuela.Entidades
{
    public class Evaluacion: ObjetoEscuelaBase
    {
        public Alumno Alumno { get; set; }
        public Asignatura Asignatura  { get; set; }
        public float Nota { get; set; }

        public override string ToString()
        {
            return $"Nota: {Nota}, Nombre Alumno: {Alumno.Nombre}, Nombre Asignatura: {Asignatura.Nombre}";
        }

    }
}