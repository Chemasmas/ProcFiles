using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    class Program
    {
        //static string directorioBase = "C:\\Users\\eva_0.DESKTOP-VMPSCP2\\Desktop\\servicio\\V1";
        static string directorioBase = ".\\";
        static string[] TITULOS = { "PROGRAMACION", "PROGRAMACIÓN" };


        static void Main(string[] args)
        {

            //Obtendremos la ruta base desde los parametros , su no hay usaremos la carpeta actual

            if (args.Count() > 0)
            {
                directorioBase = args[0];
            }

            string[] carpeta = Directory.GetFiles(directorioBase);

            var programaciones = carpeta
                .Where((archivo) =>
                   archivo.ToUpper().Contains(TITULOS[0]) || archivo.ToUpper().Contains(TITULOS[1]))
                .Where((archivo) => Path.GetExtension(archivo).ToUpper().Equals(".XLS")
                                 || Path.GetExtension(archivo).ToUpper().Equals(".XLSX"))
                .ToList();

            var aulas = carpeta
                .Where((archivo) => !programaciones.Contains(archivo))
                .Where( (archivo) => Path.GetExtension(archivo).ToUpper().Equals(".XLS") 
                                  || Path.GetExtension(archivo).ToUpper().Equals(".XLSX"))
                .ToList();

            List<Materia> todas = new List<Materia>();
            foreach (string programacion in programaciones)
            {
                var h = new Horarios();
                h.readFile(programacion);
                Console.WriteLine(programacion);
                todas.AddRange(h.readMaterias());
            }
            //Limpieza
            List<Materia> matL = todas
                .Where(materia => materia.Valida())
                //.OrderByDescending( materia => materia.numeroHoras )
                .OrderByDescending(materia => materia.cupo)
                .ToList();
            Queue<Materia> matQ = new Queue<Materia>(matL);

            var aulasR = new Aulas();
            aulasR.readFile(aulas[0]);
            
            List<string> aulasLS = new List<string>();
            //Lectura y Limpieza
            List<Salon> aulasL = aulasR.readAulas()
                .Where( aula => aula.isValida() )
                .Where( aula => {
                    if (aulasLS.Contains(aula.nombre)) return false;
                    else { aulasLS.Add(aula.nombre); return true; }
                } )
                //.OrderBy(aula => aula.nombre)
                .OrderBy(aula=>aula.capacidad)
                //.OrderByDescending(aula => aula.capacidad)
                .ToList();


            BinPacking bp1 = new BinPacking();
            bp1.Sol(aulasL, matQ);
            bp1.guardarArchivo("1V1.csv");

            /*
            BinPacking bp2 = new BinPacking();
            bp2.Sol(aulasL.OrderByDescending( aula => aula.capacidad).ToList(), new Queue<Materia>(matQ.OrderByDescending(materia => materia.numeroHoras).ToList()));

            BinPacking bp3 = new BinPacking();
            bp3.Sol(aulasL.OrderByDescending(aula => aula.capacidad).ToList(), matQ);

            BinPacking bp4 = new BinPacking();
            bp4.Sol(aulasL, new Queue<Materia>(matQ.OrderByDescending(materia => materia.numeroHoras).ToList()));


            
            bp2.guardarArchivo("2V1.txt");
            bp3.guardarArchivo("3V1.txt");
            bp4.guardarArchivo("4V1.txt");
            */
        }
    }
}

