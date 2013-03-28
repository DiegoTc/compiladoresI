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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero)
                if (right is Entero || right is Flotante)
                    return right;
                else
                    throw new Exception("Tipos incompatibles.");
            else if (left is Flotante)
                if (right is Entero || right is Flotante)
                    return left;
                else
                    throw new Exception("Tipos incompatibles.");
            else if (left is Booleano)
                throw new Exception("No se pueden sumar booleanos, con NADA, NADA!!!");
            else if (left is Caracter)
                if (right is Cadena)
                    return right;
                else throw new Exception("Tipos incompatibles");
            else if (left is Cadena)
                if (right is Booleano)
                    throw new Exception("Tipos incompatibles.");
                else return left;
            else throw new Exception("WTF?");
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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero)
                if (right is Entero || right is Flotante)
                    return right;
                else
                    throw new Exception("Tipos incompatibles.");
            else if (left is Flotante)
                if (right is Entero || right is Flotante)
                    return left;
                else
                    throw new Exception("Tipos incompatibles.");
            else throw new Exception("Solo se pueden restar numeros!");
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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero)
                if (right is Entero || right is Flotante)
                    return right;
                else
                    throw new Exception("Tipos incompatibles.");
            else if (left is Flotante)
                if (right is Entero || right is Flotante)
                    return left;
                else
                    throw new Exception("Tipos incompatibles.");
            else throw new Exception("Solo se pueden multiplicar numeros!");
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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero || left is Flotante)
                if (right is Entero || right is Flotante)
                    return new Flotante();
                else
                    throw new Exception("Tipos incompatibles.");
            else throw new Exception("Solo se pueden dividir numeros!");
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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero)
            {
                if (right is Entero || right is Flotante)
                    return right;
                else throw new Exception("Tipos incompatibles.");
            }
            else if (left is Flotante)
            {
                if (right is Entero || right is Flotante)
                    return left;
                else throw new Exception("Tipos incompatibles.");
            }else throw new Exception("Modulo solo acepta numeros!!!");
        }
    }

    class And : OperacionBinaria
    {
        public And(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Booleano && right is Booleano)
                return left;
            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Or : OperacionBinaria
    {
        public Or(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Booleano && right is Booleano)
                return left;
            throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Equal : OperacionBinaria
    {
        public Equal(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left.esEquivalente(right))
                return new Booleano();
            else throw new Exception("Error Semantico - Comparacion Invalida");
        }
    }

    class Distinto : OperacionBinaria
    {
        public Distinto(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left.esEquivalente(right))
                return new Booleano();
            else throw new Exception("Error Semantico - No se puede pueden comparar los tipos " + left + " con " + right);
        }
    }

    class MayorQue : OperacionBinaria
    {
        public MayorQue(Expresiones izq, Expresiones der)
            :base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero || left is Flotante)
                if (right is Entero || right is Flotante)
                    return new Booleano();
                else throw new Exception("No se puede comparar un numero con otra cosa.");
            else throw new Exception("Tipos incompatibles.");
        }
    }

    class MenorQue : OperacionBinaria
    {
        public MenorQue(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero || left is Flotante)
                if (right is Entero || right is Flotante)
                    return new Booleano();
                else throw new Exception("No se puede comparar un numero con otra cosa.");
            else throw new Exception("Tipos incompatibles.");
        }
    }

    class MayorIgual : OperacionBinaria
    {
        public MayorIgual(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero || left is Flotante)
                if (right is Entero || right is Flotante)
                    return new Booleano();
                else throw new Exception("No se puede comparar un numero con otra cosa.");
            else throw new Exception("Tipos incompatibles.");
        }
    }

    class MenorIgual : OperacionBinaria
    {
        public MenorIgual(Expresiones izq, Expresiones der)
            : base(izq, der)
        {
        }

        public override Tipo validarSemantica()
        {
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero || left is Flotante)
                if (right is Entero || right is Flotante)
                    return new Booleano();
                else throw new Exception("No se puede comparar un numero con otra cosa.");
            else throw new Exception("Tipos incompatibles.");
        }
    }

    class Variable : Expresiones
    {
        public string id { get; set; }
        public Access accesor { get; set; }
        //public List<Expresiones> access = new List<Expresiones>();
        //public Expresiones acces { get; set; }

        public Variable(string iden, Access acc)
        {
            id = iden;
            accesor = acc;
        }

        public override Tipo validarSemantica()
        {
            Tipo t = InfSemantica.getInstance().tblSimbolos[id];

            if (t == null)
                throw new Exception("Error Semantico - Variable " + id + " no existe");


            #region Validacion Accessories

            if (t == null)
                throw new Exception("Variable " + id + " no existe!");
            Access tmp = accesor;
            while (tmp != null)
            {

                if (tmp is AccessMiembro)
                {
                    #region Si es Registro

                    AccessMiembro am = ((AccessMiembro) tmp);
                    if (!(t is Struct))
                        throw new Exception(id + " no es un registro");
                    Struct reg = ((Struct) t);
                    t = reg.Campos[am.Id];
                    if (t == null)
                        throw new Exception("miembro " + am.Id + " no existe!");

                    #endregion
                }else if(tmp is AccessArreglo)
                {
                    #region Si es Arreglo
                    #endregion
                }


                tmp = tmp.Next;
            }
            #endregion

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
        public Variable ID;// = new Variable();
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
        public Variable ID;// = new Variable();
        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class ExprFuncion : Expresiones
    {
        public Variable ID;// = new Variable();
        public ListaExpre VarList;
        public Tipo tipo;//esta mierda no se usa

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

    public abstract class Access : Expresiones
    {
        private Access _next;

        public Access Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public Access Last()
        {
            Access tmp = Next;
            while (tmp.Next != null)
            {
                tmp = tmp.Next;
            }
            return tmp;
        }
    }

    class AccessMiembro : Access
    {
        private String id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class AccessFunc : Access
    {
        public ListaExpre Variables;

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class AccessClass : Access
    {
        private String id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }

    class AccessArreglo : Access
    {
        private ArrayList cont =new ArrayList();

       

        public void addexp(Expresiones par)
        {
            cont.Add(par);
        }


        public ArrayList Cont
        {
            get { return cont; }
            set { cont = value; }
        }

        public override Tipo validarSemantica()
        {
            //FALTA
            return null;
        }
    }


}
