using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class ValorCaracter : Valor
    {
        private char caracter;

        public char Caracter
        {
            get { return caracter; }
            set { caracter = value; }
        }

        public ValorCaracter(char carac)
        {
            Caracter = carac;
        }

        public override Valor Clonar()
        {
            return new ValorCaracter(Caracter);
        }

        public override string ToString()
        {
            return caracter.ToString();
        }
    }
}
