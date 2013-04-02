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
        public char flag;
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
            Tipo left, right;
            try
            {
                left = Izq.validarSemantica();
                right = Der.validarSemantica();
            }
            catch (Exception ex) { throw ex; }
            if (left is Entero)
                if (right is Entero || right is Flotante || right is Cadena)
                    return right;
                else
                    throw new Exception("Tipos incompatibles.");
            else if (left is Flotante)
                if (right is Entero || right is Flotante)
                    return left;
                else if (right is Cadena)
                    return right;
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
            Tipo t;

            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(id))
                t = InfSemantica.getInstance().tblSimbolos[id];
            else
                throw new Exception("Error Semantico - Variable " + id + " no existe");
            Access tmp = accesor;
            while (tmp != null)
            {
                if (tmp is AccessMiembro)
                {
                    AccessMiembro am = ((AccessMiembro) tmp);
                    if (t is Struct)
                    {
                        Struct regTmp=null;
                        Struct reg = ((Struct) t);
                        Tipo tip;
                        if (InfSemantica.getInstance().tblTipos.ContainsKey(reg.nombre))
                        {
                            tip = InfSemantica.getInstance().tblTipos[reg.nombre];
                            regTmp = ((Struct) tip);
                        }
                        else
                            throw new Exception("Error Semantico -- El registro no a sido declarado");
                        t = regTmp.Campos[am.Id];
                        if (t == null)
                            throw new Exception("miembro " + am.Id + " no existe!");
                    }
                    else if (t is Class)
                    {
                        if (((Class)t).Campos.ContainsKey(am.Id))
                        {
                            t=((Class)t).Campos[am.Id];
                        }
                        else throw new Exception(((Class)t).Nombre+" no contiene a "+am.Id);
                    }
                    else
                        throw new Exception(id + " No es Miembro de ningun objeto");
                }
                else if (tmp is AccessArreglo)
                {
                    if (t is Class)
                    {
                        if (((Class)t).Campos.ContainsKey(((AccessArreglo)tmp).nombre))
                        {
                            Tipo x = ((Class)t).Campos[((AccessArreglo)tmp).nombre];
                            if (x is Arreglo)
                                t = ((Arreglo)x).Contenido;
                            else throw new Exception(((AccessArreglo)tmp).nombre+" no es arreglo.");
                        }
                        else throw new Exception(((AccessArreglo)tmp).nombre+" no existe en "+((Class)t).Nombre);
                    }
                    else if (t is Struct)
                    {
                        if (((Struct)t).Campos.ContainsKey(((AccessArreglo)tmp).nombre))
                        {
                            Tipo x = ((Struct)t).Campos[((AccessArreglo)tmp).nombre];
                            if (x is Arreglo)
                                t = ((Arreglo)x).Contenido;
                            else throw new Exception(((AccessArreglo)tmp).nombre + " no es arreglo.");
                        }
                        else throw new Exception(((AccessArreglo)tmp).nombre + " no existe en " + ((Class)t).Nombre);
                    }
                }
                tmp = tmp.Next;
            }
            return t;
        }

        public override Valor interpretar()
        {
            if (accesor == null)
            {
                Valor valor = InfInterpretador.getInstance().getValor(id);
                return valor;
            }
            else
            {
                Access tmp = accesor;
                while (tmp != null)
                {
                    if (tmp is AccessArreglo)
                    {
                        //((AccessArreglo) tmp).Cont
                        return InfInterpretador.getInstance().getValor(id);
                    }
                    if (tmp is AccessMiembro)
                    {
                        Valor valcl = InfInterpretador.getInstance().getValor(id);
                        ValorClase tmpvalcl = (ValorClase) valcl;
                        AccessMiembro am = (AccessMiembro) tmp;
                        return tmpvalcl.obtener(am.Id);
                    }
                    tmp = tmp.Next;
                }
            }
            //TO DO arreglo registro (si acces es distinto de null)
            return null;//NUEVO

        }
    }

    class ListaExpre: Expresiones
    {
        public ArrayList Ex = new ArrayList();

        public override Tipo validarSemantica()
        {
            //FALTA
            Expresiones expre;
            ArrayList lista = new ArrayList();
            for (int i = 0; i < Ex.Count; i++)
            {
                if (Ex[i] is Expresiones)
                {
                    expre = ((Expresiones)Ex[i]);
                    expre.validarSemantica();
                    lista.Add(expre);
                }
                else
                    throw new Exception("Errro semantico " + Ex[i].ToString() + " no es el tipo correspondiente");
            }
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
            Tipo var = null;
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(ID.id))
            {
                var = InfSemantica.getInstance().tblSimbolos[ID.id];
                Class c= ((Class)var);
                var = InfSemantica.getInstance().tblTipos[c.Nombre];
            }
            Class cl;
            if (var is Class)
            {
                cl = ((Class)var);
                AccessMiembro acc= ((AccessMiembro)ID.accesor);
                while (acc != null)
                {
                    if (cl.Campos.ContainsKey(acc.Id))
                    {
                        Tipo tmp3 = cl.Campos[acc.Id];
                        if (tmp3 is Class)
                        {
                            cl = ((Class)tmp3);
                            var = InfSemantica.getInstance().tblTipos[cl.Nombre];
                            cl = ((Class)var);
                            acc = ((AccessMiembro)acc.Next);
                        }
                        else if (tmp3 is funciones)
                        {
                            funciones func = ((funciones)tmp3);
                            if (func.parametros.Count == VarList.Ex.Count)
                            {
                                for (int i = 0; i < func.parametros.Count; i++)
                                {
                                    string value = func.parametros.Ids[i].ToString();
                                    Tipo t = func.parametros[value];
                                    Expresiones ex = ((Expresiones)VarList.Ex[i]);
                                    Tipo tp = ex.validarSemantica();
                                    if (!(t.esEquivalente(tp)))
                                        throw new Exception("Error Semantico ---  El tipo no corresponde con el declarado en la funcion");
                                }
                                return func.retorno;
                             }
                            else
                            {
                                throw new Exception("Error Semantico --La cantidad de parametros no es la misma con la funcion declarada");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Error Semantico --- No se encuentra declarado " + acc.Id + " en la clase " + cl.Nombre);
                    }
                }
                
            }
            else
            {

                if (InfSemantica.getInstance().tblFunciones.ContainsKey(ID.id))
                {
                    var = InfSemantica.getInstance().tblFunciones[ID.id];
                }
                else
                    throw new Exception("Error Semantico --No se encuentra declarada esa funcion");

                if (var is funciones)
                {
                    funciones func = ((funciones)var);
                    if (func.parametros.Count == VarList.Ex.Count)
                    {
                        for (int i = 0; i < func.parametros.Count; i++)
                        {
                            string value = func.parametros.Ids[i].ToString();
                            Tipo t = func.parametros[value];
                            Expresiones ex = ((Expresiones)VarList.Ex[i]);
                            Tipo tp = ex.validarSemantica();
                            if (!(t.esEquivalente(tp)))
                                throw new Exception("Error Semantico ---  El tipo no corresponde con el declarado en la funcion");
                        }
                        return func.retorno;
                    }
                    else
                    {
                        throw new Exception("Error Semantico --La cantidad de parametros no es la misma con la funcion declarada");
                    }
                }
            }
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
            if (Next != null)
            {
                Access tmp = Next;
                while (tmp.Next != null)
                {
                    tmp = tmp.Next;
                }
                return tmp;
            }
            return this;

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
    /*
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
    */
    class AccessArreglo : Access
    {
        private ArrayList cont =new ArrayList();
        public string nombre;
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
