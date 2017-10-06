using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    class BinPacking
    {

        static int Intentos = 3;

        public List<Salon> aulas;
        public Queue<Materia> materias;
//        Stack<Materia> terminadas;

        public void Sol(List<Salon> a, Queue<Materia> m1)
        {
            //Serializare usando JSON

            var serialA = JsonConvert.SerializeObject(a);
            var serialM = JsonConvert.SerializeObject(m1);

            int pasos = 0;

            aulas = new List<Salon>( JsonConvert.DeserializeObject<List<Salon>>(serialA) );
            materias = new Queue<Materia>(JsonConvert.DeserializeObject<Queue<Materia>>(serialM) );
            guardarMaterias();
            //List<Salon> terminados = new List<Salon>();
            while (materias.Count > 0)
            {

                guardarProgreso(pasos++);
                
                Materia m = materias.Dequeue();

                if (m.intentos > Intentos)
                    break;

                //probar(aulas, m);
                probar(m);
                if (!m.completado)
                    materias.Enqueue(m);
            }
        }

        //void probar(List<Salon> aulas, Materia mat)
        void probar(Materia mat)
        {
            int i;
            foreach (Salon aula in aulas)
            {
                if (aula.capacidad < mat.cupo)
                    continue;

                var ha = aula.horario;
                if (ha == null)
                {
                    aula.horario = new Dictionary<string, Dia>();
                    ha = aula.horario;
                }
                var cha = ha.Keys.ToList();

                var hm = mat.horario;

                var chm = hm.Keys.ToList();

                foreach (string key in chm)
                {
                    Dia da = new Dia();
                    Dia dm = new Dia();

                    if (!hm.TryGetValue(key, out dm))
                        dm = new Dia();

                    if (!ha.TryGetValue(key, out da))
                    {
                        //NO habia anda en ese dia en el aula
                        mat.horario.Remove(key);
                        aula.horario.Add(key, dm);
                        continue;
                    }
                    
                    if (da.traslapar(dm))
                    {
                        da.empalmar(dm);
                        mat.horario.Remove(key);
                    }
                }
                if(mat.horario.Count > 0)
                {
                    mat.intentos++;
                    mat.completado = false;
                }
                else
                {
                    mat.completado = true;
                }
            }
        }

        public void guardarProgreso(int paso)
        {

            List<string> parc = new List<string>();
            //parc.Add("-------------------------------------------------------------------------\n");
            parc.Add("Paso "+paso);
            foreach (Salon s in aulas)
            {
                parc.Add(s.nombre + ", capacidad: ," + s.capacidad);
                string ss = String.Format(Salon.formato, "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES");
                parc.Add(ss);

                for (int i = 8; i < 20; i++)
                {
                    parc.Add(s.getHorarioLine(i));
                }
            }


            //parc.Add("-------------------------------------------------------------------------\n");
            File.AppendAllLines("pasosAulas.csv", parc);
        }

        public void guardarMaterias()
        {

            List<string> parc = new List<string>();
            parc.Add("Materias");
            foreach (Materia s in materias)
            {
                parc.Add(s.nombre + ", Clave: ," + s.clave);
                string ss = String.Format(Materia.formato, "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES");
                string[] etiquetas = { "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES" };
                parc.Add(ss);
                
                for (int i = 8; i < 20; i++)
                {
                    parc.Add(s.getHorarioLine(i));
                }
            }
            File.WriteAllLines("Materias.csv", parc);
            //File.GetAttributes("Materias.csv")
        }

        public void guardarArchivo(string nombre)
        {
            aulas = aulas.OrderBy(aula => aula.nombre).ToList();
            //var archivo = File.Create(nombre);
            List<string> res = new List<string>();
            res.Add("MAterias Faltantes:");
            res.Add(materias.Count.ToString());
            res.Add("Asignacion:");
            foreach(Salon s in aulas)
            {
                res.Add(s.nombre + ", capacidad: ," + s.capacidad);


                string ss = String.Format(Salon.formato ,"LUNES","MARTES","MIERCOLES","JUEVES","VIERNES");
                res.Add(ss);
                for(int i =8;i<20;i++)
                {
                    res.Add(s.getHorarioLine(i));
                }

                //foreach(string nomD in s.)
            }
            res.Add("Faltantes:");
            foreach (Materia m in materias)
            {
                res.Add(m.nombre);
                res.Add(m.clave);
                res.Add("Dias Faltantes:," + m.horario.Count);
            }
            // JsonConvert.SerializeObject(aulas), "Faltantes:", JsonConvert.SerializeObject(materias) };
            //File.WriteAllText(nombre, res);
            File.WriteAllLines(nombre, res);
            Console.WriteLine(File.GetAttributes(nombre).ToString());
        }

        public override string ToString()
        {
            return "{materias:"+materias.Count+"}";
        }
    }
}

