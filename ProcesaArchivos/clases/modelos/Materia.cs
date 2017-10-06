using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    

    public class Materia : IComparable<Materia>
    {
        //public static string formato = "{0,30}{1,30}{2,30}{3,30}{4,30}";
        public static string formato = "{0},{1},{2},{3},{4}";

        public string nombre { get; set; }
        public string clave { get; set; }
        public string grupo { get; set; }
        public int cupo { get; set; }
        public int numeroHoras { get; set; }
        public Dictionary<string, Dia> horario { get; set; }
        public bool completado;/* Indica si toda la materia ha sido asignada */
        public int intentos = 0;

        public void setHorario(string dia,string cadena)
        {
            if (horario == null) horario = new Dictionary<string, Dia>();
            if (cadena.Length == 0) return;
            Dia d = new Dia(cadena, dia, this.clave);
            horario.Add(dia, d);
        }

        public void setHorario(string label, Dia desc)
        {
            if (horario == null) horario = new Dictionary<string, Dia>();
            horario.Add(label, desc);
        }

        public Dia getHorario(string label)
        {
            if (horario == null) horario = new Dictionary<string, Dia>();
            Dia temp = new Dia();
            if (horario.TryGetValue(label, out temp))
            {
                return temp;
            }
            return null;
        }

        internal bool Valida()
        {
            //Por definir, por le momento uso la clave como validez
            if (clave == "")
                return false;
            return true;
        }

        /**
         * 0 >
         * 0 =
         * 0 <
         */
        public int CompareTo(Materia obj)
        {
            int wCap, wHoras; //Propios
            int woCap, woHoras; //obj
            /*
            if (obj.cupo < 10)
                woCap = 1;
            else if (obj.cupo < 25)
                woCap = 6;
            else if (obj.cupo < 30)
                woCap = 9;
            else woCap = 12;

            if (cupo < 10)
                wCap = 1;
            else if (cupo < 25)
                wCap = 6;
            else if (cupo < 30)
                wCap = 9;
            else wCap = 12;

            if (obj.numeroHoras < 5)
                woHoras = 1;
            else if (obj.numeroHoras < 6)
                woHoras = 2;
            else if (obj.numeroHoras < 8)
                woHoras = 3;
            else if (obj.numeroHoras < 9)
                woHoras = 4;
            else if (obj.numeroHoras < 10)
                woHoras = 5;
            else
                woHoras = 6;

            if (numeroHoras < 5)
                wHoras = 1;
            else if (numeroHoras < 6)
                wHoras = 2;
            else if (numeroHoras < 8)
                wHoras = 3;
            else if (numeroHoras < 9)
                wHoras = 4;
            else if (numeroHoras < 10)
                wHoras = 5;
            else
                wHoras = 6;

            
            return (wHoras + wCap) - (woHoras + woCap);
            */
            return (numeroHoras + cupo) - (obj.numeroHoras + obj.cupo);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        internal string getHorarioLine(int i)
        {
            Dia Lu, Ma, Mi, Ju, Vi;
            if (horario == null) return null;
            horario.TryGetValue("LUNES", out Lu);
            horario.TryGetValue("MARTES", out Ma);
            horario.TryGetValue("MIERCOLES", out Mi);
            horario.TryGetValue("JUEVES", out Ju);
            horario.TryGetValue("VIERNES", out Vi);


            string s1 = (Lu == null ? "" : Lu.horas[i]);
            string s2 = (Ma == null ? "" : Ma.horas[i]);
            string s3 = (Mi == null ? "" : Mi.horas[i]);
            string s4 = (Ju == null ? "" : Ju.horas[i]);
            string s5 = (Vi == null ? "" : Vi.horas[i]);


            string ss = String.Format(formato, s1, s2, s3, s4, s5);

            return ss;
        }
    }
}
