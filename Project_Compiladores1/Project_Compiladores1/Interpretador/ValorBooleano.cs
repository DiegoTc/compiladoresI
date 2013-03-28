using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorBooleano : Valor
    {
        private bool valor;

        public bool Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public ValorBooleano(bool val)
        {
            Valor = val;
        }

        public override Valor Clonar()
        {
            return new ValorBooleano(Valor);
        }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}
