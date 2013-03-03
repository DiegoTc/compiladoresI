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
            Tipo var = InfSemantica.getInstance().tblSimbolos[id.id];
            Tipo val = Valor.validarSemantica();
            if (!var.esEquivalente(val))
            {
                throw new Exception("Error Semantico - No se pueden asignar tipos diferentes " + var + " con " + val);
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
            Tipo var = InfSemantica.getInstance().tblSimbolos[Var.id];
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
        public Variable nombre= new Variable();
        public Campos c;

        public override void validarSemantica()
        {           
            //FALTA
        }

    }

    class Cases : Sentencia
    {
        public string Valor;
        public Sentencia S;
        Cases Sig;

        public override void validarSemantica()
        {
            //FALTA
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
        }
    }

    class S_Break: Sentencia
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
            //FALTA
        }
    }

    class S_Struct : Sentencia
    {
        public string nombre;
        public Campos miembros;
        public override void validarSemantica()
        {
            //FALTA
        }
    }

    class S_LlamadaFunc : Sentencia
    {
        public Variable Var = new Variable();
        public Expresiones VarList;
        public override void validarSemantica()
        {
            //FALTA
        }
    }

    class S_Class : Sentencia
    {
        public Variable Var = new Variable();
        public Sentencia CamposClase;
        public override void validarSemantica()
        {
            //FALTA
        }
    }

}
