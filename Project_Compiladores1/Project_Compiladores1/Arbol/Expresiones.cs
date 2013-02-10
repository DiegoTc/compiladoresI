using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Arbol
{
    class Expresiones
    {
    }

    class NumeroEntero : Expresiones
    {
        public int Valor { get; set; }
        public NumeroEntero(int valor)
        {
            Valor = valor;
        }
    }

    class NumeroFlotante : Expresiones
    {
        public float Valor { get; set; }
        public NumeroFlotante(float valor)
        {
            Valor = valor;
        }
    }

    class LitBool : Expresiones
    {
        public bool Valor { get; set; }
        public LitBool(bool valor)
        {
            Valor = valor;
        }
    }

    class LitChar : Expresiones
    {
        public char Valor { get; set; }
        public LitChar(char valor)
        {
            Valor = valor;
        }
    }

    class LitString : Expresiones
    {
        public string Valor { get; set; }
        public LitString(string valor)
        {
            Valor = valor;
        }
    }

    class OperacionBinaria : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public OperacionBinaria(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }
    }

    class Suma : OperacionBinaria
    {
        public Suma(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
    }
    class Resta : OperacionBinaria
    {
        public Resta(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
    }
    class Multiplicacion : OperacionBinaria
    {
        public Multiplicacion(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
    }
    class Division : OperacionBinaria
    {
        public Division(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
    }

    class Condicion : OperacionBinaria
    {
        public string Operador { get; set; }
        public Condicion(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
    }


}
