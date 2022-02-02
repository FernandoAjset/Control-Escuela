using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;

namespace CoreEscuela
{
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }
        public EscuelaEngine()
        {

        }
        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );
            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();
        }
        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }
        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioObjetos()
        {
            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>();

            diccionario.Add(LlaveDiccionario.Escuela, new[] { Escuela });
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());
            var listatmp = new List<Evaluacion>();
            var listatmpAsignaturas = new List<Asignatura>();
            var listatmpAlumnos = new List<Alumno>();

            foreach (var curso in Escuela.Cursos)
            {
                listatmpAsignaturas.AddRange(curso.Asignaturas);
                listatmpAlumnos.AddRange(curso.Alumnos);
                foreach (var alumno in curso.Alumnos)
                {
                    listatmp.AddRange(alumno.Evaluaciones);
                }
            }
            diccionario.Add(LlaveDiccionario.Asignatura, listatmpAsignaturas.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Alumno, listatmpAlumnos.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Evaluacion, listatmp.Cast<ObjetoEscuelaBase>());
            return diccionario;
        }
        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> diccionario,
            bool imprimirEvaluaciones = true)
        {
            foreach (var objeto in diccionario)
            {
                Printer.WriteTitle(objeto.Key.ToString());
                foreach (var valor in objeto.Value)
                {
                    switch (objeto.Key)
                    {
                        case LlaveDiccionario.Evaluacion:
                            if (imprimirEvaluaciones)
                                Console.WriteLine(valor);
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine($"Escuela: {valor}");
                            break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine($"Alumno: {valor}");
                            break;
                        case LlaveDiccionario.Curso:
                            var cursotmp = valor as Curso;
                            if (cursotmp != null)
                            {
                                int cantidadAlumnos = cursotmp.Alumnos.Count();
                                Console.WriteLine($"Curso: {valor.Nombre} , Cantidad alumnos: {cantidadAlumnos}");
                            }
                            break;
                        default:
                            Console.WriteLine(valor);
                            break;
                    }
                }
            }
        }

        #region MétodosGetObjetoEscuela-Sobrecarga
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela
        (
            bool traerEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
        )
        {
            return GetObjetoEscuela(out int dummy, out dummy, out dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela
        (
            out int conteoEvaluaciones,
            bool traerEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
        )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out int dummy, out dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela
        (
            out int conteoEvaluaciones,
            out int conteoCursos,
            bool traerEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
        )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out int dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela
        (
            out int conteoEvaluaciones,
            out int conteoCursos,
            out int conteoAsignaturas,
            bool traerEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
        )
        {
            return GetObjetoEscuela(out conteoEvaluaciones, out conteoCursos, out conteoAsignaturas, out int dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela
            (
            out int conteoEvaluaciones,
            out int conteoAsignaturas,
            out int conteoAlumnos,
            out int conteoCursos,
            bool traerEvaluaciones = true,
            bool traerAlumnos = true,
            bool traerAsignaturas = true,
            bool traerCursos = true
            )
        {
            conteoEvaluaciones = 0;
            conteoAsignaturas = 0;
            conteoAlumnos = 0;

            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);
            if (traerCursos)
            {
                listaObj.AddRange(Escuela.Cursos);
            }
            conteoCursos = Escuela.Cursos.Count;
            foreach (var curso in Escuela.Cursos)
            {
                conteoAsignaturas += curso.Asignaturas.Count;
                conteoAlumnos += curso.Alumnos.Count;
                if (traerAsignaturas)
                {
                    listaObj.AddRange(curso.Asignaturas);
                }
                if (traerAlumnos)
                {
                    listaObj.AddRange(curso.Alumnos);
                }
                if (traerEvaluaciones)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones += alumno.Evaluaciones.Count;
                    }
                }
            }
            return listaObj.AsReadOnly();
        }
        #endregion
        #region Métodos de carga
        private void CargarEvaluaciones()
        {
            //AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            var rnd = new Random();
            var lista = new List<Evaluacion>();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluacion
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                //Nota = (float)Math.Round(5 * rnd.NextDouble(),2),
                                Nota = MathF.Round((float)(5 * rnd.NextDouble()), 2), 
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }
        }
        //private void AccionDelEvento(object sender, EventArgs e)
        //{
        //    Printer.WriteTitle("TERMINANDO EJECUCIÓN...");
        //    Printer.Beep(3000,1000,3);
        //    Printer.WriteTitle("EJECUCION TERMINADA");
        //}
        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }
        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
        #endregion
    }
}