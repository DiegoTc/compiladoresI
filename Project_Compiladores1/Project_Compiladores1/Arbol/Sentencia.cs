using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Interpretador;
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

        protected abstract void interpretarSentencia();

        public void interpretar()
        {
            interpretarSentencia();
            if (sig != null)
            {
                sig.interpretar();
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

        protected override void interpretarSentencia()
        {            
            Valor tmp = Expr.interpretar();     
            if (tmp is ValorEntero)
                Console.WriteLine(((ValorEntero)tmp).Valor.ToString());
            if (tmp is ValorFlotante)
                Console.WriteLine(((ValorFlotante)tmp).Valor.ToString());
            if (tmp is ValorCadena)
                Console.WriteLine(((ValorCadena)tmp).Valor.ToString());
            if (tmp is ValorCaracter)
                Console.WriteLine(((ValorCaracter)tmp).Valor.ToString());
            if (tmp is ValorBooleano)
                Console.WriteLine(((ValorBooleano)tmp).Valor.ToString());
            if (tmp is ValorArreglo)
            {
                Variable tmpvar = (Variable) Expr;
                Access tmpacc = ((AccessArreglo) tmpvar.accesor).Last();
                Console.WriteLine(((ValorArreglo)tmp).get(((AccessArreglo)tmpacc).Cont));
            }
            
        }
    }

    class S_Read : Sentencia
    {
        public Variable var;
        public override void validarSemantica()
        {
            var.validarSemantica();
        }

        protected override void interpretarSentencia()
        {
            
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
                    Tipo tmptipo = InfSemantica.getInstance().tblTipos[sts.nombre];

                    while (ac != null)
                    {
                        if (ac is AccessMiembro)
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
                        else if (ac is AccessArreglo)
                        {
                            AccessArreglo access = ((AccessArreglo)ac);
                            Struct st = ((Struct)tmptipo);
                            tmptipo = st.Campos[access.nombre];

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
                                    throw new Exception("Error semantico -- No existe dicho accessor" + access.nombre);
                                }
                            }
                            else if (tmptipo is Arreglo)
                            {
                                Arreglo str = ((Arreglo)tmptipo);
                                if (st.Campos.ContainsKey(access.nombre))
                                {
                                    tmptipo = st.Campos[access.nombre];
                                    if (tmptipo is Arreglo)
                                    {
                                        str = ((Arreglo)tmptipo);
                                        tmptipo = str.Contenido;
                                    }
                                    ac = ac.Next;
                                }
                                else
                                {
                                    throw new Exception("Error semantico -- No existe dicho accessor" + access.nombre);
                                }
                            }
                            else
                            {

                                tmptipo = st.Campos[access.nombre];
                                ac = ac.Next;
                            }
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

        protected override void interpretarSentencia()
        {
            Valor val = Valor.interpretar();

            Access a = id.accesor;
            ArrayList lista = new ArrayList();
            String campo;
            Valor ident = InfInterpretador.getInstance().getValor(id.id);

            while (a != null)
            {
                if (a is AccessArreglo)
                {
                    lista = new ArrayList();
                    AccessArreglo tmp = (AccessArreglo) a;
                    ArrayList indices = tmp.Cont;
                    for (int i = 0; i < indices.Count; i++)
                    {
                        lista.Add((ValorEntero) ((Expresiones) indices[i]).interpretar());
                    }
                    if (a.Next != null)
                    {
                        ident = ((ValorArreglo) ident).get(lista);
                    } //TO DO ADD ELSE CLAUSE
                }
                a = a.Next;
            }
            if (id.accesor != null)//ACA ES DISTINTO
            {
                if (ident is ValorArreglo)
                {
                    ((ValorArreglo) ident).set(lista, val.Clonar());
                }
                else
                {
                    //TO DO: ADD CODE
                }
            }
            else
            {
                InfInterpretador.getInstance().asignarValor(id.id, val.Clonar());

                //System.out.println(var.getId()+"="+val);
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

        protected override void interpretarSentencia()
        {
            Valor tmp = Condicion.interpretar();
            if (((ValorBooleano)tmp).Valor)            
                Cierto.interpretar();
            else
                Falso.interpretar();
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

        protected override void interpretarSentencia()
        {
            Valor tmp = Condicion.interpretar();
            while (((ValorBooleano)tmp).Valor)
            {
                S.interpretar();
                tmp = Condicion.interpretar();
            }
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

        protected override void interpretarSentencia()
        {
            S.interpretar();
            Valor tmp = Condicion.interpretar();
            while (((ValorBooleano)tmp).Valor)
            {
                S.interpretar();
                tmp = Condicion.interpretar();
            }
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

        protected override void interpretarSentencia()
        {
            
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
            /*///c.validarSemantica();*/
        }

        protected override void interpretarSentencia()
        {
            //NO SE hace nada ACA
        }
    }

    class Cases : Sentencia
    {
        public Expresiones Valor;
        public Sentencia S;
        public Cases Sig;

        public override void validarSemantica()
        {
            //FALTA
            Tipo T = Valor.validarSemantica();
            if (T is Entero)
            {
                //NADA
            }
            else
            {
                throw new Exception("Error Semantico - Tipo de valor de evaluacion de case no soportado");
            }
            S.SentValSemantica();
            Cases tmp = Sig;
            while (tmp != null)
            {
                tmp.validarSemantica();
                tmp = tmp.Sig;
            }
        }

        protected override void interpretarSentencia()
        {
            S.interpretar();
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
            if (T is Entero)
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

        protected override void interpretarSentencia()
        {
            Valor val = Var.interpretar();
            bool def = false;
            if (Casos != null)
            {                
                Cases tmp = Casos;
                while (tmp != null)
                {
                    Valor valtmp = tmp.Valor.interpretar();
                    if (((ValorEntero)val).Valor == ((ValorEntero)valtmp).Valor)
                    {
                        tmp.interpretar();
                        def = true;
                        break;
                    }
                    tmp = tmp.Sig;
                }
            }
            if (def==false && sdefault != null)
                sdefault.interpretar();


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
            funciones func = new funciones();
            func.parametros = new T_Campos();
            func.retorno = Retorno;
            Tipo Var = null;
            if (InfSemantica.getInstance().tblFunciones.ContainsKey(this.Var))
            {
                throw new Exception("Error Semantico - La funcion " + this.Var + " ya existe");
            }
            if (InfSemantica.getInstance().tblSimbolos.ContainsKey(this.Var))
            {
                throw new Exception("Error Semantico - La variable " + this.Var + " ya existe declarada en una funcion");
            }
            if (Var == null)
            {
                //Campo.validarSemantica();

                Declaracion tmp = Campo;
                while (tmp != null)
                {
                    Variable v = tmp.Var;
                    if (func.parametros.ContainsKey(v.id))
                    {
                        throw new Exception("Error Semantico - La variable " + v.id + " ya existe");
                    }
                    func.parametros.Add(v.id, tmp.Tip);
                    InfSemantica.getInstance().tblSimbolos.Add(v.id, tmp.Tip);
                    //InfSemantica.getInstance().tblSimbolos.Add(v.id, tmp.Tip);
                    tmp = tmp.Sig;
                }
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
                bool flag = (tmp is S_Return);
                while (tmp != null && flag == false)
                {
                    tmp.SentValSemantica();
                    tmp = tmp.sig;
                    flag = (tmp is S_Return);
                }
                if (!(Retorno is Voids))
                {
                    if (tmp is S_Return)
                    {
                        S_Return ret = ((S_Return)tmp);
                        Tipo T = ret.Expr.validarSemantica();
                        if (!Retorno.esEquivalente(T))
                        {
                            throw new Exception("Error Semantico - Expresion de retorno no es el mismo que el retorno de la funcion");
                        }
                        
                    }
                    else
                    {
                        throw new Exception("Error Semantico - Se esperaba el valor de retorno que tiene que ser " + Retorno.ToString());
                    }
                }
                else
                {
                    if (tmp is S_Return)
                    {
                        throw new Exception("Error Semantico - La funcion es tipo void no tiene que retornar nada.");
                    }
                }
                InfSemantica.getInstance().tblFunciones.Add(this.Var, func);


                #endregion
            }
        }

        protected override void interpretarSentencia()
        {
            /*
            Campo.interpretar();
            S.interpretar();
            */
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

            if (Tip is Class)
            {
                Class ctmp = ((Class)Tip);
                if(!(InfSemantica.getInstance().tblTipos.ContainsKey(ctmp.Nombre)))
                {
                    throw new Exception("Error Semantico -- La clase "+ ctmp.Nombre+ " no a sido declarada");
                }

            }
            else if (Tip is Struct)
            {
                Struct stmp = ((Struct)Tip);
                if(!(InfSemantica.getInstance().tblTipos.ContainsKey(stmp.nombre)))
                {
                    throw new Exception("Error Semantico -- El struct " + stmp.nombre + " no a sido declarada");
                }
            }
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
                //InfInterpretador.getInstance().asignarValor(Var.id, null);
                var = InfSemantica.getInstance().tblSimbolos[Var.id];
            }
            if (Valor != null)
            {
                if (!var.esEquivalente(Val))
                {
                    throw new Exception("Error Semantico - No se puede inicializar variables con tipos diferentes");
                }
            }
            if (Sig != null)
                Sig.validarSemantica();
        }

        protected override void interpretarSentencia()
        {
            if (Valor == null)
            {
                if (Tip is Arreglo)
                {
                    Arreglo tmptip = (Arreglo)Tip;                    
                    ValorArreglo tmp = new ValorArreglo(tmptip);
                    InfInterpretador.getInstance().asignarValor(Var.id, tmp);
                }
                else
                {
                    InfInterpretador.getInstance().asignarValor(Var.id, null);
                }
            }
            else
            {
                Valor tmp = Valor.interpretar();
                InfInterpretador.getInstance().asignarValor(Var.id, tmp);
            }
        }
    }

    class S_Break : Sentencia
    {
        public override void validarSemantica()
        {
            //no FALTA
        }

        protected override void interpretarSentencia()
        {
            
        }
    }

    class S_Continue : Sentencia
    {
        public override void validarSemantica()
        {
            //no FALTA
        }

        protected override void interpretarSentencia()
        {
            
        }
    }

    class S_Return : Sentencia
    {
        public Expresiones Expr;
        public override void validarSemantica()
        {
            Expr.validarSemantica();
        }

        protected override void interpretarSentencia()
        {
            //Expr.interpretar();
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

        protected override void interpretarSentencia()
        {
            //NIPI NIJA
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

            if (var == null)
            {
                Class cl = new Class();
                cl.Campos = new T_Campos();
                Sentencia tmp=CamposClase;
                while(tmp!=null)
                {
                    if (tmp is Declaracion)
                    {
                        Declaracion d = ((Declaracion)tmp);
                        tmp.validarSemantica();
                        //if (d.Tip == null)
                         //   d.Tip = new T_Campos();
                        cl.Campos.Add(d.Var.id, d.Tip);
                        tmp = tmp.sig;
                    }
                    else if (tmp is S_Functions)
                    {
                        S_Functions s = ((S_Functions)tmp);
                        s.validarSemantica();
                        if (InfSemantica.getInstance().tblFunciones.ContainsKey(s.Var))
                        {
                            Tipo t = InfSemantica.getInstance().tblFunciones[s.Var];
                            funciones func = ((funciones)t);
                            cl.Campos.Add(s.Var, func);
                            tmp = tmp.sig;
                        }
                        else
                            throw new Exception("Error Semantico ---  Hubo un error al momento de declarar la variable");
                    }
                    else
                    {

                        throw new Exception("Error Semantico --- No se puede declarar esa sentencia aqui");
                    }
                }
                InfSemantica.getInstance().tblTipos.Add(Var.id, cl);
            }
            else
            {
                throw new Exception("Error Semantico - La variable " + Var.id + " ya existe");
            }
            #endregion

           

        }

        protected override void interpretarSentencia()
        {
            Var.interpretar();
            CamposClase.interpretar();
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

        protected override void interpretarSentencia()
        {
            
        }
    }

    class s_masmas : Sentencia
    {
        public ExpMasMas param;
        public override void validarSemantica()
        {
            try { param.validarSemantica(); }
            catch (Exception ex) { throw ex; }
        }

        protected override void interpretarSentencia()
        {
            try { param.interpretar(); }
            catch (Exception ex) { throw ex; }
        }
    }

    class s_menosmenos : Sentencia
    {
        public ExpMenosMenos param;
        public override void validarSemantica()
        {
            try { param.validarSemantica(); }
            catch (Exception ex) { throw ex; }
        }

        protected override void interpretarSentencia()
        {
            try { param.interpretar(); }
            catch (Exception ex) { throw ex; }
        }
    }
}
