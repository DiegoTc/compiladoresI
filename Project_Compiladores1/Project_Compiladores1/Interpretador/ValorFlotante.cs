using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorFlotante : Valor
    {
        private float valor;

        public float Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public ValorFlotante(float val)
        {
            Valor = val;            
        }

        public override Valor Clonar()
        {
            return new ValorFlotante(Valor);
        }

        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}
