using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Arbol
{
    public abstract class Tipo
    {
        public abstract bool esEquivalente(Tipo t);
    }
    
    class Booleano : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Booleano;
        }
    }

    class Cadena : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Cadena;
        }
    }

    class Entero : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Entero;
        }

    }

    class Flotante : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Flotante;
        }
    }

    class Caracter : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Caracter;
        }
    }

    class Voids : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Voids;
        }
    }  
}
