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
        public Variable id = new Variable("", null);
        public Expresiones Valor;
        public Declaracion campos; //Para las asignaciones de arreglos.

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
                if (var is Struct)
                {
                    Access ac = id.accesor;
                    Struct sts = ((Struct)var);
                    Tipo tmptipo=InfSemantica.getInstance().tblTipos[sts.nombre];
                                    
                    while (ac != null)
                    {
                        AccessMiembro access = ((AccessMiembro)ac);
                        Struct st = ((Struct)tmptipo); 
                        tmptipo = st.Campos[access.Id];
                        
                        if (tmptipo is Struct)
                        {
                            Struct str = ((Struct)tmptipo);
                            if (InfSemantica.getInstance().tblTipos.ContainsKey(str.nombre))
                            {
                                tmptipo = InfSemantica.getInstance().tblTipos[str.nombre];
                                ac = ac.Next;
                            }
                            else
                            {
                                throw new Exception("Error semantico -- No existe dicho accessor" + access.Id);
                            }
                        }
                        else
                        {
                           
                            tmptipo = st.Campos[access.Id];
                            ac = ac.Next;
                        }
                    }
                    if (!tmptipo.esEquivalente(val))
                    {
                        throw new Exception("Error Semantico - No se pueden asignar tipos diferentes " + var + " con " + val);
                    }

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
            if (Con is Booleano)
            {
                if (Cierto != null)
                    Cierto.SentValSemantica();
                if (Falso != null)
                    Cierto.SentValSemantica();
            }
            else throw new Exception("La condicion debe ser booleana");
        }
    }

    class S_While : Sentencia
    {
        public Expresiones Condicion;
        public Sentencia S;

        public override void validarSemantica()
        {
            Tipo Con = Condicion.validarSemantica();
            if (Con is Booleano)
            {
                if (S != null)
                    S.SentValSemantica();
            }
            else throw new Exception("La condicion debe ser booleana");
        }
    }

    class S_Do : Sentencia
    {
        public Sentencia S;
        public Expresiones Condicion;

        public override void validarSemantica()
        {
            Tipo Con = Condicion.validarSemantica();
            if (Con is Booleano)
            {
                if (S != null)
                    S.SentValSemantica();
            }
            else throw new Exception("La condicion debe ser booleana");
        }
    }

    class S_For : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable("", null);
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
                    throw new Exception("Error Semantico - No existe la variable" + Var);
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

    class Structs : TypeDef
    {
        public string nombre;// = new Variable();
        public Declaracion campos;
        //public Dictionary<string, Tipo> tblSimbolosStruct = new Dictionary<string, Tipo>();

        public override void validarSemantica()
        {
            //FALTA            
            Tipo var = null;
            if (InfSemantica.getInstance().tblTipos.ContainsKey(nombre))
            {
                var = InfSemantica.getInstance().tblTipos[nombre];
            }

            if (var != null)
                throw new Exception("Error Semantico - La variable " + nombre + " ya esta siendo utilizada");
            else
            {
                Struct s = new Struct();
                s.Campos = new T_Campos();
                s.nombre = nombre;
                Declaracion tmp = campos;
                while (tmp != null)
                {
                    string id = tmp.Var.id;
                    if (s.Campos.ContainsKey(id))
                    {
                        throw new Exception("Error Semantico - La variable " + id + " ya esta siendo utilizada");
                    }
                    s.Campos.Add(id, tmp.Tip);
                    tmp = tmp.Sig;
                }
                InfSemantica.getInstance().tblTipos.Add(nombre, s);
            }
            //c.validarSemantica();
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
            //ya no FALTA
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
        private String var;
        //public Variable var = new Variable();
        public Declaracion Campo;
        public Sentencia S;

        public string Var
        {
            get { return var; }
            set { var = value; }
        }

        public override void validarSemantica()
        {
            //FALTA
            #region Validar Existe Variable


            Tipo Var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(this.Var))
            {
                Var = InfSemantica.getInstance().tblFunciones[this.Var];
            }
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(this.Var))
            {
                throw new Exception("Error Semantico - La variable " + this.Var + " ya existe");
            }  
            if (Var == null)
            {
                Campo.validarSemantica();
                /*Declaracion tmp=Campo;
                while(tmp!=null)
                {
                    Variable v = tmp.Var;
                    if(v.id!="")
                    {
                        if (InfSemantica.getInstance().tblTipos.ContainsKey(v.id))
                        {
                            throw new Exception("Error Semantico - La variable " + v.id + " ya existe");
                        }
                        InfSemantica.getInstance().tblSimbolos.Add(v.id, tmp.Tip);
                    }
                    tmp = tmp.Sig;
                }*/
                InfSemantica.getInstance().tblFunciones.Add(this.Var, Retorno);                
            }
            else
            {
                throw new Exception("Error Semantico - La variable " + this.Var + " ya existe");
            }
            #endregion

           

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

    class Declaracion : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable("", null);
        public Declaracion Sig;
        public Expresiones Valor;

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
                if (Tip is Struct)
                {
                    
                }
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
            //no FALTA
        }
    }

    class S_Continue : Sentencia
    {
        public override void validarSemantica()
        {
            //no FALTA
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
        public Variable Var; // = new Variable();
        public ListaExpre Variables;
        

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
        public Variable Var = new Variable("", null);
        public Sentencia CamposClase;
        public Dictionary<string, Tipo> tblSimbolosClass = new Dictionary<string, Tipo>();

        public override void validarSemantica()
        {
            //FALTA
            #region Validar Existe Variable

            Tipo var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(Var.id))
            {
                var = InfSemantica.getInstance().tblFunciones[Var.id];
            }
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(Var.id))
            {
                throw new Exception("Error Semantico - La variable " + Var.id + " ya existe");
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

            Sentencia tmp = CamposClase;
            while (tmp != null)
            {
                if (tmp is Declaracion)
                {
                    Declaracion tmpCampo = ((Declaracion)tmp);
                    tblSimbolosClass.Add(tmpCampo.Var.id, tmpCampo.Tip);
                }
                else
                {
                    tmp.validarSemantica();
                }
                tmp = tmp.sig;
            }    

        }
    }

    abstract class TypeDef : Sentencia
    {
        public TypeDef Sig;
    }

    class Alias : TypeDef
    {
        public UserType type;
        public override void validarSemantica()
        {
            InfSemantica.getInstance().tblTipos.Add(type.Nombre, type.Tip);
        }
    }
}
