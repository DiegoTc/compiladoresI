using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;

namespace Project_Compiladores1.Interpretador
{
    public abstract class Valor
    {
        public abstract Valor Clonar();
        private Tipo tipo;


        public Tipo Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
