using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorCadena : Valor
    {
        private string valor;

        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public ValorCadena(string cad)
        {
            Valor = cad;
        }

        public override Valor Clonar()
        {
            return new ValorCadena(Valor);
        }

        public override string ToString()
        {
            return Valor;
        }

    }
}
