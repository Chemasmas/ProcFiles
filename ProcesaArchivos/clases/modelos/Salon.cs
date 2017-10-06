using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    public class Salon : IComparable<Salon> ,IEqualityComparer<Salon>
    {

        //public static string formato = "{0,30}{1,30}{2,30}{3,30}{4,30}";
        public static string formato = "{0},{1},{2},{3},{4}";

        public string nombre { get; set; }
        public int capacidad { get; set; }
        public Dictionary<string, Dia> horario;

        int IComparable<Salon>.CompareTo(Salon other)
        {
            if (nombre.Equals(other.nombre))
                return nombre.CompareTo(other.nombre);
            else
                return this.capacidad.CompareTo(other.capacidad);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool Equals(Salon x, Salon y)
        {
            //if (x.nombre.Equals(y.nombre) && x.capacidad == y.capacidad)
            if (x.nombre.Equals(y.nombre))
                return true;

            return false;
        }

        public int GetHashCode(Salon obj)
        {
            return JsonConvert.SerializeObject(obj).GetHashCode();
        }

        public Boolean isValida()
        {
            if (capacidad == -1)
                return false;
            if (nombre.Length < 1 || nombre.Length > 5)
                return false;
            return true;
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


            string s1 = (Lu == null ? "" : Lu.horas[i] );
            string s2 = (Ma == null ? "" : Ma.horas[i]);
            string s3 = (Mi == null ? "" : Mi.horas[i]);
            string s4 = (Ju == null ? "" : Ju.horas[i]);
            string s5 = (Vi == null ? "" : Vi.horas[i]);


            string ss = String.Format(formato,s1,s2,s3,s4,s5);

            return ss;
        }
    }
}
