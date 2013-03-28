using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorEntero : Valor
    {
        private int valor;

        public int Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public ValorEntero(int val)
        {
            Valor = val;
        }

        public override Valor Clonar()
        {
            return new ValorEntero(valor);
        }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}
