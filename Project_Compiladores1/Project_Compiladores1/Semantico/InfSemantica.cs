using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;
using Project_Compiladores1.Lexico;

namespace Project_Compiladores1.Semantico
{
    class InfSemantica
    {
        public Dictionary<string, Tipo> tblSimbolos = new Dictionary<string, Tipo>();
        public Dictionary<string, Tipo> tblFunciones = new Dictionary<string, Tipo>();
        public Dictionary<string, Tipo> tblTipos = new Dictionary<string, Tipo>();
        private static InfSemantica instance = null;

        public InfSemantica()
        {
            tblTipos.Add("ENTERO", new Entero());
            tblTipos.Add("FLOTANTE", new Flotante());
            tblTipos.Add("CADENA", new Cadena());
            tblTipos.Add("BOOLEANO", new Booleano());
            tblTipos.Add("CARACTER", new Caracter());
        }

        public static InfSemantica getInstance()
        {
            if (instance == null)
                instance = new InfSemantica();
            return instance;
        }
    }
}
