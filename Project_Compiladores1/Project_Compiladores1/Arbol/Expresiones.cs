using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Interpretador;
using Project_Compiladores1.Semantico;

namespace Project_Compiladores1.Arbol
{
    public abstract class Expresiones
    {
        public abstract Tipo validarSemantica();

        public abstract Valor interpretar(); 
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

        public override Valor interpretar()
        {
            return new ValorEntero(Valor);
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

        public override Valor interpretar()
        {
            return new ValorFlotante(Valor);
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

        public override Valor interpretar()
        {
            return new ValorBooleano(Valor);
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

        public override Valor interpretar()
        {
            return new ValorCaracter(Valor);
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

        public override Valor interpretar()
        {
            return new ValorCadena(Valor);
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();
            
            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorEntero(((ValorEntero)vizq).Valor + ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor + ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorCadena && vder is ValorCadena)
            {
                return new ValorCadena(((ValorCadena)vizq).Valor + ((ValorCadena)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorEntero)vizq).Valor + ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor + ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorCadena && vder is ValorEntero)
            {
                return new ValorCadena(((ValorCadena)vizq).Valor + ((ValorEntero)vder).ToString());
            }
            if (vizq is ValorEntero && vder is ValorCadena)
            {
                return new ValorCadena(((ValorEntero)vizq).ToString() + ((ValorCadena)vder).Valor);
            }
            if (vizq is ValorCadena && vder is ValorFlotante)
            {
                return new ValorCadena(((ValorCadena)vizq).Valor + ((ValorFlotante)vder).ToString());
            }
            if (vizq is ValorFlotante && vder is ValorCadena)
            {
                return new ValorCadena(((ValorFlotante)vizq).ToString() + ((ValorCadena)vder).Valor);
            }
            if (vizq is ValorCadena && vder is ValorCaracter)
            {
                return new ValorCadena(((ValorCadena)vizq).Valor + ((ValorCaracter)vder).Valor);
            }
            if (vizq is ValorCaracter && vder is ValorCadena)
            {
                return new ValorCadena(((ValorCaracter)vizq).Valor + ((ValorCadena)vder).Valor);
            }

            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorEntero(((ValorEntero)vizq).Valor - ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor - ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorEntero)vizq).Valor - ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor - ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorEntero(((ValorEntero)vizq).Valor * ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor * ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorEntero)vizq).Valor * ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor * ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorFlotante(((float)((ValorEntero)vizq).Valor) / ((float)((ValorEntero)vder).Valor));
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor / ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorFlotante(((ValorEntero)vizq).Valor / ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorFlotante(((ValorFlotante)vizq).Valor / ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorEntero(((ValorEntero)vizq).Valor / ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorEntero(Convert.ToInt32(((ValorFlotante)vizq).Valor / ((ValorFlotante)vder).Valor));
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorEntero(Convert.ToInt32(((ValorEntero)vizq).Valor / ((ValorFlotante)vder).Valor));
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorEntero(Convert.ToInt32(((ValorFlotante)vizq).Valor / ((ValorEntero)vder).Valor));
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (((ValorBooleano)vizq).Valor && ((ValorBooleano)vizq).Valor)
            {
                return new ValorBooleano(true);
            }
            else
            {
                return new ValorBooleano(false);
            }
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (((ValorBooleano)vizq).Valor == false && ((ValorBooleano)vizq).Valor == false)
            {
                return new ValorBooleano(false);
            }
            else
            {
                return new ValorBooleano(true);
            }
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor == ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor == ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorCadena)
            {
                return new ValorBooleano(((ValorCadena)vizq).Valor == ((ValorCadena)vder).Valor);
            }
            if (vizq is ValorCaracter)
            {
                return new ValorBooleano(((ValorCaracter)vizq).Valor == ((ValorCaracter)vder).Valor);
            }
            if (vizq is ValorBooleano)
            {
                return new ValorBooleano(((ValorBooleano)vizq).Valor == ((ValorBooleano)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor != ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor != ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorCadena)
            {
                return new ValorBooleano(((ValorCadena)vizq).Valor != ((ValorCadena)vder).Valor);
            }
            if (vizq is ValorCaracter)
            {
                return new ValorBooleano(((ValorCaracter)vizq).Valor != ((ValorCaracter)vder).Valor);
            }
            if (vizq is ValorBooleano)
            {
                return new ValorBooleano(((ValorBooleano)vizq).Valor != ((ValorBooleano)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor > ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor > ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor > ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor > ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor < ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor < ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor < ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor < ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor >= ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor >= ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor >= ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor >= ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor vizq = Izq.interpretar();
            Valor vder = Der.interpretar();

            if (vizq is ValorEntero && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor <= ((ValorEntero)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor <= ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorEntero && vder is ValorFlotante)
            {
                return new ValorBooleano(((ValorEntero)vizq).Valor <= ((ValorFlotante)vder).Valor);
            }
            if (vizq is ValorFlotante && vder is ValorEntero)
            {
                return new ValorBooleano(((ValorFlotante)vizq).Valor <= ((ValorEntero)vder).Valor);
            }
            return null;
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

        public override Valor interpretar()
        {
            Valor valor = InfInterpretador.getInstance().getValor(id);
            //TO DO arreglo registro (si acces es distinto de null)
            return valor;
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

        public override Valor interpretar()
        {
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

        public override Valor interpretar()
        {
            if (ID.accesor == null)
            {
                Valor v = ID.interpretar();
                ((ValorEntero)v).Valor += 1;
                return v;
            }
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

        public override Valor interpretar()
        {
            if (ID.accesor == null)
            {
                Valor v = ID.interpretar();
                ((ValorEntero)v).Valor -= 1;
                return v;
            }
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

        public override Valor interpretar()
        {
            return null;
        }
    }

    abstract class OperacionUnaria : Expresiones
    {
        public Expresiones parametro;
        
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

        public override Valor interpretar()
        {
            Valor t = parametro.interpretar();
            return new ValorBooleano(!((ValorBooleano)t).Valor); 
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

        public override Valor interpretar()
        {
            Valor valor = InfInterpretador.getInstance().getValor(id);            
            return valor;
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

        public override Valor interpretar()
        {
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

        public override Valor interpretar()
        {
            Valor valor = InfInterpretador.getInstance().getValor(id);
            return valor;
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

        public override Valor interpretar()
        {
            return null;
        }
    }


}
