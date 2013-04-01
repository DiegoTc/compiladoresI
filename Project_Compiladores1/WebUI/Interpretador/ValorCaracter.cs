using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebUI.Interpretador
{
    class ValorCaracter : Valor
    {
        private string valor;

        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public ValorCaracter(string carac)
        {
            Valor = carac;
        }

        public override Valor Clonar()
        {
            return new ValorCaracter(Valor);
        }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}
