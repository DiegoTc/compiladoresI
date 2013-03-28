using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorCadena : Valor
    {
        private string _cadena;

        public string Cadena
        {
            get { return _cadena; }
            set { _cadena = value; }
        }

        public ValorCadena(string cad)
        {
            Cadena = cad;
        }

        public override Valor Clonar()
        {
            return new ValorCadena(Cadena);
        }

        public override string ToString()
        {
            return Cadena;
        }

    }
}
