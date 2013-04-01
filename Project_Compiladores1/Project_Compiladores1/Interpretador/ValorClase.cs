using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorClase : Valor
    {
        Dictionary<string, Valor> miembros;

        public ValorClase(Dictionary<String, Valor> param)
        {
            miembros = new Dictionary<string, Valor>(param);
        }

        public ValorClase()
        {
            miembros = new Dictionary<string, Valor>();
        }        

        public void asignar(string mem, Valor val)
        {
            if (miembros.ContainsKey(mem))
            {
                //if (miembros[mem].Tipo.esEquivalente(val.Tipo))
                //{
                    miembros[mem] = val;
                //}
                //else throw new Exception("Tipos incompatibles.");
            }
            else throw new Exception("No existe ese miembro.");
        }

        public Valor obtener(string mem)
        {
            if (miembros.ContainsKey(mem))
                return miembros[mem];
            else throw new Exception("No existe ese miembro.");
        }

        public override Valor Clonar()
        {
            return new ValorRegistro(miembros);
        }
    }
}
