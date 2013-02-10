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
        public Expresiones Expr;        
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
        public string id;
        public Expresiones Inicio;
        public Expresiones Condicion;
        public Expresiones Iteracion;
        public Sentencia S;
    }

    class Cases : Expresiones
    {
        public Expresiones Expr;
        public Sentencia S;
        Cases Sig;
    }

    class S_Switch : Sentencia
    {
        public Variable Id;
        public Expresiones Casos;
    }

    class S_Functions : Sentencia
    {
        public 
    }

}
