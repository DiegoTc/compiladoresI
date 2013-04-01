using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorRegistro :Valor
    {
        Dictionary<string, Valor> miembros;// = new Dictionary<string, Valor>();

        public ValorRegistro(Dictionary<String, Valor> param)
        {
            miembros = new Dictionary<string, Valor>(param);
        }

        public ValorRegistro()
        {
            miembros = new Dictionary<string, Valor>();
        }

        public override Valor Clonar()
        {
            return new ValorRegistro(miembros);
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
    }
}
