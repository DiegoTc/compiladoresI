using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Semantico;

namespace Project_Compiladores1.Arbol
{
    public abstract class Expresiones
    {
        public abstract Tipo validarSemantica();
    }

    class LiteralEntero : Expresiones
    {
        public int Valor { get; set; }
        public LiteralEntero(int valor)
        {
            Valor = valor;
        }

        public override Tipo validarSemantica()
        {
            return InfSemantica.getInstance().tblTipos["ENTERO"];            
        }
    }

    class  LiteralFlotante : Expresiones
    {
        public float Valor { get; set; }
        public LiteralFlotante(float valor)
        {
            Valor = valor;
        }

        public override Tipo validarSemantica()
        {
            return InfSemantica.getInstance().tblTipos["FLOTANTE"];
        }
    }

    class LitBool : Expresiones
    {
        public bool Valor { get; set; }
        public LitBool(bool valor)
        {
            Valor = valor;
        }

        public override Tipo validarSemantica()
        {
            return InfSemantica.getInstance().tblTipos["BOOLEANO"];
        }
    }

    class LitChar : Expresiones
    {
        public string Valor { get; set; }
        public LitChar(string valor)
        {
            Valor = valor;
        }

        public override Tipo validarSemantica()
        {
            return InfSemantica.getInstance().tblTipos["CARACTER"];
        }
    }

    class LitString : Expresiones
    {
        public string Valor { get; set; }
        public LitString(string valor)
        {
            Valor = valor;
        }

        public override Tipo validarSemantica()
        {
            return InfSemantica.getInstance().tblTipos["CADENA"];
        }
    }

    public abstract class OperacionBinaria : Expresiones
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

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();
            if (left is Entero && right is Entero)
            {
                return left;
            }
            throw new Exception("Error Semantico - No se puede sumar " + left + " con " + right);
        }
    }

    class Resta : OperacionBinaria
    {
        public Resta(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();
            if (left is Entero && right is Entero)
            {
                return left;
            }
            throw new Exception("Error Semantico - No se puede restar " + left + " con " + right);
        }
    }
    class Multiplicacion : OperacionBinaria
    {
        public Multiplicacion(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }
        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();
            if (left is Entero && right is Entero)
            {
                return left;
            }
            throw new Exception("Error Semantico - No se puede multiplicar " + left + " con " + right);
        }
    }
    class Division : OperacionBinaria
    {
        public Division(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();
            if (left is Entero && right is Entero)
            {
                return left;
            }
            throw new Exception("Error Semantico - No se puede dividir " + left + " con " + right);
        }
    }

    class Mod : OperacionBinaria
    {
        public Mod(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();
            if (left is Entero && right is Entero)
            {
                return left;
            }
            throw new Exception("Error Semantico - No se puede realizar esta operacion entre " + left + " con " + right);
        }
    }

    class And : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public And(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Booleano && right is Booleano)
                return left;
            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Or : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public Or(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Booleano && right is Booleano)
                return left;
            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Equal : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public Equal(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Entero && right is Entero)
            {
                return new Booleano();
            }
            if (left is Flotante && right is Flotante)
            {
                return new Booleano();
            }
            if (left is Cadena && right is Cadena)
            {
                return new Booleano();
            }
            if (left is Caracter && right is Caracter)
            {
                return new Booleano();
            }
            if (left is Booleano && right is Booleano)
            {
                return left;
            }

            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Distinto : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public Distinto(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (!left.esEquivalente(right))
            {
                throw new Exception("Error Semantico - No se puede pueden comparar los tipos " + left + " con " + right);
            }
            return left;
        }
    }

    class MayorQue : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public MayorQue(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Entero && right is Entero)
            {
                return new Booleano();
            }
            if (left is Flotante && right is Flotante)
            {
                return new Booleano();
            }            

            throw new Exception("Error Semantico - Comparacion Invalida");
            
        }
    }

    class MenorQue : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public MenorQue(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Entero && right is Entero)
            {
                return new Booleano();
            }
            if (left is Flotante && right is Flotante)
            {
                return new Booleano();
            }

            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class MayorIgual : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public MayorIgual(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Entero && right is Entero)
            {
                return new Booleano();
            }
            if (left is Flotante && right is Flotante)
            {
                return new Booleano();
            }

            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class MenorIgual : Expresiones
    {
        public Expresiones Izq { get; set; }
        public Expresiones Der { get; set; }
        public MenorIgual(Expresiones izq, Expresiones der)
        {
            Izq = izq;
            Der = der;
        }

        public override Tipo validarSemantica()
        {
            Tipo left = Izq.validarSemantica();
            Tipo right = Der.validarSemantica();

            if (left is Entero && right is Entero)
            {
                return new Booleano();
            }
            if (left is Flotante && right is Flotante)
            {
                return new Booleano();
            }

            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Variable : Expresiones
    {
        public string id { get; set; }
        public List<Expresiones> access = new List<Expresiones>();
        //public Expresiones acces { get; set; }

        public override Tipo validarSemantica()
        {
            Tipo t = InfSemantica.getInstance().tblSimbolos[id];
            if (t == null)
                throw new Exception("Error Semantico - Variable " + id + " no existe" );
            return t;
        }
    }

    class ListaExpre: Expresiones
    {
        public ArrayList Ex = new ArrayList();

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }

    }

    class ExpMasMas : Expresiones
    {
        public Variable ID = new Variable();
        public override Tipo validarSemantica()
        {
            //FALTA          
            Tipo T = ID.validarSemantica();
            if (T is Entero)
            { }
            else
                throw new Exception("Error Semantico - Se esperaba un valor Entero");
            return null;
        }
    }

    class ExpMenosMenos : Expresiones
    {
        public Variable ID = new Variable();
        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class ExprFuncion : Expresiones
    {
        public Variable ID = new Variable();
        public Expresiones VarList;
        public Tipo tipo;

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class OperacionUnaria : Expresiones
    {
        public Expresiones parametro;
        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class Not : OperacionUnaria
    {
        public Not(Expresiones par)
        {
            this.parametro = par;
        }

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }
}
