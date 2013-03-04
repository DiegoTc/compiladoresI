using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Semantico;

namespace Project_Compiladores1.Arbol
{

    public abstract class Sentencia
    {
        public Sentencia sig;

        public abstract void validarSemantica();

        public void SentValSemantica()
        {
            validarSemantica();
            if (sig != null)
            {
                sig.SentValSemantica();
            }
        }
    }

    class S_Print : Sentencia
    {
        public Expresiones Expr;

        public override void validarSemantica()
        {
            Expr.validarSemantica();
        }
    }

    class S_Read : Sentencia
    {
        public Variable var;
        public override void validarSemantica()
        {
            var.validarSemantica();
        }
    }

    class S_Asignacion : Sentencia
    {
        public Operadores Op;
        public Variable id = new Variable();
        public Expresiones Valor;
        public Campos campos; //Para las asignaciones de arreglos.

        public override void validarSemantica()
        {
            Tipo var = null;
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(id.id))
            {
                var = InfSemantica.getInstance().tblSimbolos[id.id];
            }
            Tipo val = Valor.validarSemantica();
            if (var != null)
            {
                if (!var.esEquivalente(val))
                {
                    throw new Exception("Error Semantico - No se pueden asignar tipos diferentes " + var + " con " + val);
                }
            }
            else
            {
                throw new Exception("Error Semantico - No existe la variable " + id.id);
            }
        }
    }

    class S_If : Sentencia
    {
        public Expresiones Condicion;

        public Sentencia Cierto;
        public Sentencia Falso;

        public override void validarSemantica()
        {
            Tipo Con = Condicion.validarSemantica();
            //VALIDAR CONDICION SEA BOOL
            if (Cierto != null)
                Cierto.SentValSemantica();
            if (Falso != null)
                Cierto.SentValSemantica();
        }
    }

    class S_While : Sentencia
    {
        public Expresiones Condicion;
        public Sentencia S;

        public override void validarSemantica()
        {
            Tipo Con = Condicion.validarSemantica();
            //VALIDAR QUE SEA BOOL
            if (S != null)
                S.SentValSemantica();
        }
    }

    class S_Do : Sentencia
    {
        public Sentencia S;
        public Expresiones Condicion;

        public override void validarSemantica()
        {
            Tipo Con = Condicion.validarSemantica();
            //VALIDAR QUE SEA BOOL
            if (S != null)
                S.SentValSemantica();

        }
    }

    class S_For : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable();
        public Expresiones Inicio;
        public Expresiones Condicion;
        public Expresiones Iteracion;
        public Sentencia S;

        public override void validarSemantica()
        {
            if (Tip is Entero == false)
            {
                throw new Exception("Error Semantico - Solo se puede declarar un tipo ENTERO para los ciclos FOR");
            }

            Tipo var = null;
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(Var.id))
            {
                var = InfSemantica.getInstance().tblSimbolos[Var.id];
            }
            else
            {
                if (Tip != null)
                {
                    InfSemantica.getInstance().tblSimbolos.Add(Var.id, Tip);
                    var = InfSemantica.getInstance().tblSimbolos[Var.id];
                }
                else
                {
                    throw new Exception("Error Semantico - No existe la variable" + Var.id);
                }
            }

            Tipo val = Inicio.validarSemantica();
            if (!var.esEquivalente(val))
            {
                throw new Exception("Error Semantico - No se pueden asignar tipos diferentes " + var + " con " + val);
            }
            Condicion.validarSemantica();
            Iteracion.validarSemantica();
            if (S != null)
                S.SentValSemantica();

        }
    }

    class Structs : Sentencia
    {
        public Variable nombre = new Variable();
        public Campos c;

        public override void validarSemantica()
        {
            //FALTA            
            Tipo var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(nombre.id))
            {
                var = InfSemantica.getInstance().tblFunciones[nombre.id];
            }


            if (var == null)
                throw new Exception("Error Semantico - La variable " + nombre.id + " ya esta siendo utilizada");
            else
                InfSemantica.getInstance().tblFunciones.Add(nombre.id, new Struct());
            c.validarSemantica();
        }

    }

    class Cases : Sentencia
    {
        public Expresiones Valor;
        public Sentencia S;
        Cases Sig;

        public override void validarSemantica()
        {
            //FALTA
            Tipo T = Valor.validarSemantica();
            if (T is Entero || T is Cadena || T is Caracter)
            {
                //NADA
            }
            else
            {
                throw new Exception("Error Semantico - Tipo de valor de evaluacion de case no soportado");
            }
            S.SentValSemantica();
            Sig.validarSemantica();
        }
    }

    class S_Switch : Sentencia
    {
        public Expresiones Var;
        public Cases Casos;
        public Sentencia sdefault;

        public override void validarSemantica()
        {
            //FALTA
            Tipo T = Var.validarSemantica();
            if (T is Entero || T is Cadena || T is Caracter)
            {
                //NADA
            }
            else
            {
                throw new Exception("Error Semantico - Tipo de valor no soportado en el switch");
            }
            Casos.validarSemantica();
            sdefault.SentValSemantica();
        }
    }

    class S_Functions : Sentencia
    {
        public Tipo Retorno;
        public Variable var = new Variable();
        public Campos Campo;
        public Sentencia S;

        public override void validarSemantica()
        {
            //FALTA
            #region Validar Existe Variable


            Tipo Var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(var.id))
            {
                Var = InfSemantica.getInstance().tblFunciones[var.id];
            }
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(var.id))
            {
                throw new Exception("Error Semantico - La variable " + var.id + " ya existe");
            }  
            if (Var == null)
            {
                if (Retorno != null)
                    InfSemantica.getInstance().tblFunciones.Add(var.id, Retorno);
                else
                    InfSemantica.getInstance().tblFunciones.Add(var.id, new Voids());
            }
            else
            {
                throw new Exception("Error Semantico - La variable " + var.id + " ya existe");
            }
            #endregion

            Campo.validarSemantica();

            if (Retorno != null)
            {
                #region Valida Valor de Retorno

                Sentencia tmp = S;
                while (tmp != null)
                {
                    if (tmp is S_Return)
                    {
                        S_Return ret = ((S_Return)tmp);
                        Tipo T = ret.Expr.validarSemantica();
                        if (!Retorno.esEquivalente(T))
                        {
                            throw new Exception(
                                "Error Semantico - Expresion de retorno no es el mismo que el retorno de la funcion");
                        }
                    }
                    else
                    {
                        tmp.SentValSemantica();
                    }
                    tmp = tmp.sig;
                }

                #endregion
            }
        }
    }

    class Campos : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable();
        public Campos Sig;
        public Expresiones Valor;
        public int Dimension;
        public List<Expresiones> dim = new List<Expresiones>();
        public Expresiones Ex;

        public override void validarSemantica()
        {
            //FALTA                        

            Tipo var = null;
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(Var.id))
            {
                var = InfSemantica.getInstance().tblSimbolos[Var.id];
            }            
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(Var.id))
            {
                throw new Exception("Error Semantico - La variable " + Var.id + " ya existe");
            }
            Tipo Val = null;
            if (Valor != null)
            {
                Val = Valor.validarSemantica();
            }
            if (var != null)
            {
                throw new Exception("Error Semantico - La variable " + Var.id + " ya existe");    
            }
            else
            {
                InfSemantica.getInstance().tblSimbolos.Add(Var.id, Tip);
                var = InfSemantica.getInstance().tblSimbolos[Var.id];
            }
            if (Valor != null)
            {
                if (!var.esEquivalente(Val))
                {
                    throw new Exception("Error Semantico - No se puede inicializar variables con tipos diferentes");
                }
            }
            if(Sig!=null)
                Sig.validarSemantica();
        }
    }

    class S_Break : Sentencia
    {
        public override void validarSemantica()
        {
            //FALTA
        }
    }

    class S_Continue : Sentencia
    {
        public override void validarSemantica()
        {
            //FALTA
        }
    }

    class S_Return : Sentencia
    {
        public Expresiones Expr;
        public override void validarSemantica()
        {
            Expr.validarSemantica();
        }
    }

    class S_LlamadaFunc : Sentencia
    {
        public Variable Var = new Variable();
        public Variable VarClase = new Variable();
        public Expresiones VarList;

        public override void validarSemantica()
        {
            //FALTA           
            Tipo var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(Var.id))
            {
                var = InfSemantica.getInstance().tblFunciones[Var.id];
            }


            if (var == null)
                throw new Exception("Error Semantico - La variable " + Var.id + " no existe");
        }
    }

    class S_Class : Sentencia
    {
        public Variable Var = new Variable();
        public Sentencia CamposClase;
        public override void validarSemantica()
        {
            //FALTA
            #region Validar Existe Variable

            Tipo var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(Var.id))
            {
                var = InfSemantica.getInstance().tblFunciones[Var.id];
            }

            if (var != null)
            {
                InfSemantica.getInstance().tblFunciones.Add(Var.id, new Class());
            }
            else
            {
                throw new Exception("Error Semantico - La variable " + Var.id + " ya existe");
            }
            #endregion
        }
    }

}
