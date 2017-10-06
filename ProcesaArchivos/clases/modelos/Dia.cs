using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcesaArchivos.clases
{
    public class Dia
    {
        //private string regExp = "([0-9]{1,2}(:[0-9]{1,2}){0,1})[-aA ]{1,3}([0-9]{1,2}(:[0-9]{1,2}){0,1})";
        private string regExp = "([0-9]{1,2}):{0,1}([0-9]{1,2}){0,1}[-aA ]{1,3}([0-9]{1,2}):{0,1}([0-9]{1,2}){0,1}";

        //bool[] horas = new bool[14]; /* de x a y horas*/
        //bool[] horas = new bool[24];
        public string[] horas = new string[24];

        public Dia()
        {
            //Constructor Vacio.
            //genera matrices vacias
        }

        /**
         * id:  es una concatenacion de clave y grupo separadas por un guion bajo
         * 
         * */
        public Dia(string horario, string dia, string id)
        {
            int i;
            //Formatos validos
            //XX-XX
            //XX:XX-XX:XX ++++
            //XX:XX a XX:XX ++++
            Regex reg = new Regex(regExp);

            var parc = reg.Match(horario);
            if (parc.Success)
            {
                var ini = Int32.Parse(parc.Groups[1].Value);
                var fin = Int32.Parse(parc.Groups[3].Value);

                //Reglas del horario
                if (ini < 8) ini += 12;

                for (i = ini; i < fin; i++)
                {
                    horas[i] = id;
                }
            }
        }

        public bool traslapar(Dia otro)
        {
            for (int i = 0; i < 24; i++)
            {
                if (otro.horas[i] != null && this.horas[i] != null)
                    return false;
            }
            return true;
        }

        public void empalmar(Dia otro)
        {
            for (int i = 0; i < 24; i++)
            {
                if (otro.horas[i] != null)
                {
                    this.horas[i] = otro.horas[i];
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
