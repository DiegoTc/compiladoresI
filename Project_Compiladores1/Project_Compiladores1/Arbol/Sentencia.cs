using System;
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
        public Variable id;
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
        public Variable Var;
        public Expresiones Inicio;
        public Expresiones Condicion;
        public Expresiones Iteracion;
        public Sentencia S;
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
        public Expresiones Casos;
        public Sentencia sdefault;
    }

    class S_Functions : Sentencia
    {
        public Tipo Retorno;
        public Variable var;
        public Campos Campo;
        public Sentencia S;
    }

    class Campos : Expresiones
    {
        public Tipo Tip;
        public Variable var;
        public Campos Sig;
    }

    class S_Break: Sentencia
    {
        
    }

    class S_Return : Sentencia
    {
        public Expresiones Expr;
    }

}
