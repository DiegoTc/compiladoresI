using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebUI.Arbol;

namespace WebUI.Interpretador
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
