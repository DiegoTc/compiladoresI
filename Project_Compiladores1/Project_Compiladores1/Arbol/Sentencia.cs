using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Arbol
{
    class Sentencia
    {
        public Sentencia sig;
    }

    class S_Print : Sentencia
    {
        public Expresiones Expr;
    }

    class S_Read : Sentencia
    {
        public Variable var;        
    }

    class S_Asignacion : Sentencia
    {
        public Operadores Op;
        public Variable id = new Variable();
        public Expresiones Valor;
    }

    class S_If : Sentencia
    {
        public Expresiones Condicion;

        public Sentencia Cierto;
        public Sentencia Falso;
    }

    class S_While : Sentencia
    {
        public Expresiones Condicion;
        public Sentencia S;
    }

    class S_Do : Sentencia
    {
        public Sentencia S;
        public Expresiones Condicion;        
    }

    class S_For : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable();
        public Expresiones Inicio;
        public Expresiones Condicion;
        public Expresiones Iteracion;
        public Sentencia S;
    }

    class Structs : Sentencia
    {
        public Variable nombre= new Variable();
        public Campos c;
        //public ArrayList list = new ArrayList();
    }

    class Cases : Sentencia
    {
        public string Valor;
        public Sentencia S;
        Cases Sig;
    }

    class S_Switch : Sentencia
    {
        public Expresiones Var;
        public Cases Casos;
        public Sentencia sdefault;
    }

    class S_Functions : Sentencia
    {
        public Tipo Retorno;
        public Variable var = new Variable();
        public Campos Campo;
        public Sentencia S;
    }

    class Campos : Sentencia
    {
        public Tipo Tip;
        public Variable Var = new Variable();
        public Campos Sig;
        public Expresiones Valor;
        public int Dimension;
        public Expresiones Ex;
    }

    class S_Break: Sentencia
    {
        
    }

    class S_Continue : Sentencia { }

    class S_Return : Sentencia
    {
        public Expresiones Expr;
    }

    class S_Struct : Sentencia
    {
        public string nombre;
        public Campos miembros;
    }

    class S_LlamadaFunc : Sentencia
    {
        public Variable Var = new Variable();
        public Expresiones VarList;
    }

    class S_Class : Sentencia
    {
        public Variable Var = new Variable();
        public Sentencia CamposClase;        
    }

}
