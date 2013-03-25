using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Lexico
{
    public enum TipoToken
    {
        TK_ID,
        TK_INT,
        TK_FLOAT,
        TK_STRING,
        TK_CHAR,
        TK_BOOL,
        TK_ARRAY,
        TK_ASSIGN,
        TK_IF,
        TK_ELSE,
        TK_WHILE,
        TK_FOR,
        TK_DO,
        TK_TO,
        TK_SWITCH,
        TK_CASE,
        TK_BREAK,
        TK_FINSENTENCIA,
        TK_OPENPAR,
        TK_CLOSEPAR,
        TK_OPENLLAVE,
        TK_CLOSELLAVE,
        TK_OPENCOR,
        TK_CLOSECOR,
        TK_PUNTO,
        TK_COMA,
        TK_DOSPUNTOS,
        TK_SUMA,
        TK_RESTA,
        TK_MULT,
        TK_DIVISION,
        TK_MOD,
        TK_DIV,
        TK_IGUALDAD,
        TK_DISTINTO,
        TK_MENORQUE,
        TK_MAYORQUE,
        TK_MENORIGUAL,
        TK_MAYORIGUAL,
        TK_AND,
        TK_OR,
        TK_NOT,
        TK_MASMAS,
        TK_MENOSMENOS,
        TK_MASIGUAL,
        TK_PORIGUAL,
        TK_ENTREIGUAL,
        TK_MENOSIGUAL,
        TK_CLASS,
        TK_VOID,
        TK_STRUCT,
        TK_TRUE,
        TK_FALSE,
        TK_FINFLUJO,
        TK_PRINT,
        TK_READ,
        TK_STRING_LIT,
        TK_CHAR_LIT,
        TK_THEN,
        TK_PROGRAM,
        TK_TYPE,
        TK_VAR,
        TK_BEGIN,
        TK_END,
        TK_FUNCTION,
        TK_OF,
        TK_FLOAT_LIT,
        TK_INT_LIT,
        TK_RETURN,
        TK_DEFAULT,
        TK_PUBLIC,
        TK_REPEAT,
        TK_UNTIL,
        TK_CONTINUE,
        TK_EXIT,
        TK_RECORD,
        TK_DOWNTO,
        TK_PRIVATE,
        TK_NEW
    }

    public class Token
    {
        public string Lexema;
        public TipoToken Tipo;
        public int Linea;
        public int Columna;

        public Token(string Lex, TipoToken Tip, int Lin, int Col)
        {
            Lexema = Lex;
            Tipo = Tip;
            Linea = Lin;
            Columna = Col;
        }

    }

    public class Lexico
    {
        public string Contenido;
        public int CurrentSymbol;
        public int Col;
        public int Fil;
        public Dictionary<string, TipoToken> PalabrasReservadas;
        public Dictionary<string, TipoToken> Operadores;
        public char symbol;

        public Lexico(string Cont)
        {
            Contenido = Cont;
            CurrentSymbol = 0;
            Col = 0;
            Fil = 0;
            PalabrasReservadas = new Dictionary<string, TipoToken>();
            Operadores = new Dictionary<string, TipoToken>();           
        }

        protected char nextSymbol()
        {
            if (CurrentSymbol >= Contenido.Length)
                return (char)0;
            char c = Contenido[CurrentSymbol];
            if (c == '\n' || c == '\r')
            {
                Fil++;
                Col = 0;
            }
            else
            {
                Col++;
            }
            CurrentSymbol++;
            return c;
        }

        public Token NextToken()
        {
            int estado = 0;            
            string lexema = "";
            while (true)
            {
                switch (estado)
                {
                    case 0:
                        if (char.IsDigit(symbol))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 3;
                        }
                        else if (char.IsLetter(symbol))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 1;
                        }
                        else if (Operadores.ContainsKey(symbol.ToString()) || symbol == '"' || symbol == '\'' || symbol == '&' || symbol == '|')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 8;
                        }
                        else if (char.IsWhiteSpace(symbol) || symbol == '\n' || symbol == '\r')
                            symbol = nextSymbol();
                        else if (symbol == '\0')
                            return new Token("", TipoToken.TK_FINFLUJO, Fil, Col);
                        else
                        {
                            throw new Exception("Simbolo no reconocido");
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(symbol) || symbol == '_')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 1;
                        }
                        else
                        {
                            estado = 2;
                        }
                        break;

                    case 2:
                        if (PalabrasReservadas.ContainsKey(lexema.ToLower()))
                        {
                            return new Token(lexema, PalabrasReservadas[lexema.ToLower()], Fil, Col);
                        }
                        else
                        {
                            return new Token(lexema, TipoToken.TK_ID, Fil, Col);
                        }

                    case 3:
                        if (char.IsDigit(symbol))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 3;
                        }
                        else if (symbol == '.')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 4;
                        }
                        else
                        {
                            estado = 7;
                        }
                        break;

                    case 4:
                        if (char.IsDigit(symbol))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 5;
                        }
                        else
                        {
                            throw new Exception("Se esperaba un numero!!");
                        }
                        break;

                    case 5:
                        if (char.IsDigit(symbol))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            estado = 5;
                        }
                        else
                        {
                            estado = 6;
                        }
                        break;

                    case 6:
                        return new Token(lexema, TipoToken.TK_FLOAT_LIT, Fil, Col); //Aqui regresa un numero flotante

                    case 7:
                        return new Token(lexema, TipoToken.TK_INT_LIT, Fil, Col); //Aqui regresa un numero entero

                    case 8:
                        if (lexema == "&" && symbol == '&')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            return new Token(lexema, Operadores[lexema], Fil, Col);
                        }else if (lexema == "|" && symbol == '|')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            return new Token(lexema, Operadores[lexema], Fil, Col);
                        }else if (Operadores.ContainsKey(lexema.ToLower() + symbol.ToString().ToLower()))
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            return new Token(lexema, Operadores[lexema], Fil, Col);
                        }
                        else if(lexema == "\"")
                        {
                            estado = 9;
                        }
                        else if (lexema == "'")
                        {
                            estado = 10;
                        }
                        else
                            return new Token(lexema, Operadores[lexema], Fil, Col);
                        break;
                    case 9:
                        if (symbol == '"')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            return new Token(lexema, TipoToken.TK_STRING_LIT, Fil, Col);
                        }
                        else
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                        }
                        break;
                    case 10:
                        if (symbol == '\'')
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                            if (lexema.Length <= 3)
                            {
                                return new Token(lexema, TipoToken.TK_CHAR_LIT, Fil, Col);
                            }
                            else
                            {
                                throw new Exception("Simbolo no reconocido");
                            }
                        }
                        else
                        {
                            lexema += symbol;
                            symbol = nextSymbol();
                        }
                        break;

                }
            }
        }
    }
}
