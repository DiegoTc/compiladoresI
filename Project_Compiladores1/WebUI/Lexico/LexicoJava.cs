using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Lexico
{
    class LexicoJava : Lexico
    {
        public LexicoJava(string Cad)
            : base(Cad)
        {
            Operadores.Add("+", TipoToken.TK_SUMA);
            Operadores.Add("-", TipoToken.TK_RESTA);
            Operadores.Add("*", TipoToken.TK_MULT);
            Operadores.Add("/", TipoToken.TK_DIVISION);
            Operadores.Add("%", TipoToken.TK_MOD);
            Operadores.Add("div", TipoToken.TK_DIV);
            Operadores.Add("==", TipoToken.TK_IGUALDAD);
            Operadores.Add("!=", TipoToken.TK_DISTINTO);
            Operadores.Add("<", TipoToken.TK_MENORQUE);
            Operadores.Add(">", TipoToken.TK_MAYORQUE);
            Operadores.Add("<=", TipoToken.TK_MENORIGUAL);
            Operadores.Add(">=", TipoToken.TK_MAYORIGUAL);
            Operadores.Add("&&", TipoToken.TK_AND);
            Operadores.Add("||", TipoToken.TK_OR);
            Operadores.Add("not", TipoToken.TK_NOT);
            Operadores.Add("++", TipoToken.TK_MASMAS);
            Operadores.Add("--", TipoToken.TK_MENOSMENOS);
            Operadores.Add("+=", TipoToken.TK_MASIGUAL);
            Operadores.Add("*=", TipoToken.TK_PORIGUAL);
            Operadores.Add("/=", TipoToken.TK_ENTREIGUAL);
            Operadores.Add("-=", TipoToken.TK_MENOSIGUAL);
            Operadores.Add(";", TipoToken.TK_FINSENTENCIA);
            Operadores.Add("(", TipoToken.TK_OPENPAR);
            Operadores.Add(")", TipoToken.TK_CLOSEPAR);
            Operadores.Add("[", TipoToken.TK_OPENCOR);
            Operadores.Add("]", TipoToken.TK_CLOSECOR);
            Operadores.Add("{", TipoToken.TK_OPENLLAVE);
            Operadores.Add("}", TipoToken.TK_CLOSELLAVE);
            Operadores.Add("=", TipoToken.TK_ASSIGN);
            Operadores.Add(".", TipoToken.TK_PUNTO);
            Operadores.Add(",", TipoToken.TK_COMA);
            Operadores.Add(":", TipoToken.TK_DOSPUNTOS);
            Operadores.Add("!", TipoToken.TK_NOT);


            PalabrasReservadas.Add("string", TipoToken.TK_STRING);
            PalabrasReservadas.Add("int", TipoToken.TK_INT);
            PalabrasReservadas.Add("float", TipoToken.TK_FLOAT);
            PalabrasReservadas.Add("char", TipoToken.TK_CHAR);
            PalabrasReservadas.Add("true", TipoToken.TK_TRUE);
            PalabrasReservadas.Add("boolean", TipoToken.TK_BOOL);
            PalabrasReservadas.Add("false", TipoToken.TK_FALSE);
            PalabrasReservadas.Add("if", TipoToken.TK_IF);
            PalabrasReservadas.Add("else", TipoToken.TK_ELSE);
            PalabrasReservadas.Add("while", TipoToken.TK_WHILE);
            PalabrasReservadas.Add("for", TipoToken.TK_FOR);
            PalabrasReservadas.Add("do", TipoToken.TK_DO);
            PalabrasReservadas.Add("switch", TipoToken.TK_SWITCH);
            PalabrasReservadas.Add("case", TipoToken.TK_CASE);
            PalabrasReservadas.Add("break", TipoToken.TK_BREAK);
            PalabrasReservadas.Add("class", TipoToken.TK_CLASS);
            PalabrasReservadas.Add("void", TipoToken.TK_VOID);
            PalabrasReservadas.Add("println", TipoToken.TK_PRINT);
            PalabrasReservadas.Add("return", TipoToken.TK_RETURN);
            PalabrasReservadas.Add("default", TipoToken.TK_DEFAULT);
            PalabrasReservadas.Add("public", TipoToken.TK_PUBLIC);
            PalabrasReservadas.Add("private", TipoToken.TK_PRIVATE);
            PalabrasReservadas.Add("new", TipoToken.TK_NEW);




            symbol = nextSymbol();
        }

    }
}