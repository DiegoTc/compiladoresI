using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Lexico
{
    class LexicoPascal : Lexico
    {
        public LexicoPascal(string Cad)
            : base(Cad)
        {
            Operadores.Add("+", TipoToken.TK_SUMA);
            Operadores.Add("-", TipoToken.TK_RESTA);
            Operadores.Add("*", TipoToken.TK_MULT);
            Operadores.Add("/", TipoToken.TK_DIVISION);
            Operadores.Add("=", TipoToken.TK_IGUALDAD);
            Operadores.Add("<>", TipoToken.TK_DISTINTO);
            Operadores.Add("<", TipoToken.TK_MENORQUE);
            Operadores.Add(">", TipoToken.TK_MAYORQUE);
            Operadores.Add("<=", TipoToken.TK_MENORIGUAL);
            Operadores.Add(">=", TipoToken.TK_MAYORIGUAL);             /*
             Operadores.Add("++", TipoToken.TK_MASMAS);
             Operadores.Add("--", TipoToken.TK_MENOSMENOS);
             Operadores.Add("+=", TipoToken.TK_MASIGUAL);
             Operadores.Add("*=", TipoToken.TK_PORIGUAL);
             Operadores.Add("/=", TipoToken.TK_ENTREIGUAL);
             Operadores.Add("-=", TipoToken.TK_MENOSIGUAL);*/
            Operadores.Add(";", TipoToken.TK_FINSENTENCIA);
            Operadores.Add("(", TipoToken.TK_OPENPAR);
            Operadores.Add(")", TipoToken.TK_CLOSEPAR);
            Operadores.Add("[", TipoToken.TK_OPENCOR);
            Operadores.Add("]", TipoToken.TK_CLOSECOR);/*
             Operadores.Add("{", TipoToken.TK_OPENLLAVE);
             Operadores.Add("}", TipoToken.TK_CLOSELLAVE);*/
            Operadores.Add(":=", TipoToken.TK_ASSIGN);
            Operadores.Add(".", TipoToken.TK_PUNTO);
            Operadores.Add(",", TipoToken.TK_COMA);
            Operadores.Add(":", TipoToken.TK_DOSPUNTOS);

            PalabrasReservadas.Add("div", TipoToken.TK_DIV);
            PalabrasReservadas.Add("and", TipoToken.TK_AND);
            PalabrasReservadas.Add("or", TipoToken.TK_OR);
            PalabrasReservadas.Add("not", TipoToken.TK_NOT);
            PalabrasReservadas.Add("integer", TipoToken.TK_INT);
            PalabrasReservadas.Add("real", TipoToken.TK_FLOAT);
            PalabrasReservadas.Add("char", TipoToken.TK_CHAR);
            PalabrasReservadas.Add("true", TipoToken.TK_TRUE);
            PalabrasReservadas.Add("boolean", TipoToken.TK_BOOL);
            PalabrasReservadas.Add("false", TipoToken.TK_FALSE);
            PalabrasReservadas.Add("if", TipoToken.TK_IF);
            PalabrasReservadas.Add("else", TipoToken.TK_ELSE);
            PalabrasReservadas.Add("then", TipoToken.TK_THEN);
            PalabrasReservadas.Add("while", TipoToken.TK_WHILE);
            PalabrasReservadas.Add("for", TipoToken.TK_FOR);
            PalabrasReservadas.Add("do", TipoToken.TK_DO);
            PalabrasReservadas.Add("case", TipoToken.TK_CASE);
            PalabrasReservadas.Add("procedure", TipoToken.TK_VOID);
            PalabrasReservadas.Add("writeln", TipoToken.TK_PRINT);
            PalabrasReservadas.Add("readln", TipoToken.TK_READ);
            PalabrasReservadas.Add("program", TipoToken.TK_PROGRAM);
            PalabrasReservadas.Add("type", TipoToken.TK_TYPE);
            PalabrasReservadas.Add("var", TipoToken.TK_VAR);
            PalabrasReservadas.Add("begin", TipoToken.TK_BEGIN);
            PalabrasReservadas.Add("end", TipoToken.TK_END);
            PalabrasReservadas.Add("function", TipoToken.TK_FUNCTION);
            PalabrasReservadas.Add("of", TipoToken.TK_OF);
            PalabrasReservadas.Add("to", TipoToken.TK_TO);
            PalabrasReservadas.Add("mod", TipoToken.TK_MOD);
            PalabrasReservadas.Add("repeat", TipoToken.TK_REPEAT);
            PalabrasReservadas.Add("until", TipoToken.TK_UNTIL);
            PalabrasReservadas.Add("continue", TipoToken.TK_CONTINUE);
            PalabrasReservadas.Add("exit", TipoToken.TK_EXIT);
            PalabrasReservadas.Add("record", TipoToken.TK_RECORD);
            PalabrasReservadas.Add("array", TipoToken.TK_ARRAY);
            PalabrasReservadas.Add("string", TipoToken.TK_STRING);

            symbol = nextSymbol();
        }
    }
}