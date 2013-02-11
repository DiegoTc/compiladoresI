using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser:Parser
    {
        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }

        public void parse()
        {
            SL();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
        }

        void SL()
        {
            S();
            SL();
        }

        void S()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                S_prime();
            }
            else if (currentToken.Tipo == TipoToken.TK_IF)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_THEN)
                {
                    currentToken = lex.NextToken();
                    IFP();
                }
                else throw new Exception("Se esperaba then");
            }
            else if (currentToken.Tipo == TipoToken.TK_WHILE)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_DO)
                {
                    currentToken = lex.NextToken();
                    CS();
                }
                else throw new Exception("Se esperaba do");
            }
            else if (currentToken.Tipo == TipoToken.TK_FOR)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                    {
                        currentToken = lex.NextToken();
                        expr();
                        if (currentToken.Tipo == TipoToken.TK_TO)
                        {
                            currentToken = lex.NextToken();
                            expr();
                            if (currentToken.Tipo == TipoToken.TK_DO)
                            {
                                currentToken = lex.NextToken();
                                CS();
                            }
                            else throw new Exception("Se esperaba do");
                        }
                        else throw new Exception("Se esperaba to");
                    }
                    else throw new Exception("Se esperaba :=");
                }
                else throw new Exception("Se esperaba id");
            }
            else if (currentToken.Tipo == TipoToken.TK_REPEAT)
            {
                currentToken = lex.NextToken();
                SL();
                if (currentToken.Tipo == TipoToken.TK_UNTIL)
                {
                    expr();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        currentToken = lex.NextToken();
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba until");
            }
            else if (currentToken.Tipo == TipoToken.TK_CASE)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_OF)
                    {
                        currentToken = lex.NextToken();
                        CL();
                        if (currentToken.Tipo == TipoToken.TK_END)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                                currentToken = lex.NextToken();
                            else throw new Exception("Se esperaba ;");
                        }
                        else throw new Exception("Se esperaba end");
                    }
                    else throw new Exception("Se esperaba of");
                }
                else throw new Exception("Se esperaba id");
            }
            else if (currentToken.Tipo == TipoToken.TK_BREAK || currentToken.Tipo == TipoToken.TK_CONTINUE)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    currentToken = lex.NextToken();
                else throw new Exception("Se esperaba ;");
            }
            else if (currentToken.Tipo == TipoToken.TK_EXIT)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            currentToken = lex.NextToken();
                        else throw new Exception("Se esperaba ;");
                    }
                    else throw new Exception("Se esperaba )");
                }
                else throw new Exception("Se esperaba (");
            }
            else if (currentToken.Tipo == TipoToken.TK_READ || currentToken.Tipo == TipoToken.TK_PRINT)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    currentToken = lex.NextToken();
                else throw new Exception("Se esperaba ;");
            }
            else if (currentToken.Tipo == TipoToken.TK_TYPE)
                TD();
            else if (currentToken.Tipo == TipoToken.TK_VAR)
                VD();
            else if (currentToken.Tipo == TipoToken.TK_FUNCTION || currentToken.Tipo == TipoToken.TK_VOID)
                FD();
        }

        void TD()
        {
            currentToken = lex.NextToken();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_RECORD)
                    {
                        currentToken = lex.NextToken();
                        PL();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_END)
                            {
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                                    currentToken = lex.NextToken();
                                else throw new Exception("Se esperaba ;");
                            }
                            else throw new Exception("Se esperaba end");
                        }
                        else throw new Exception("Se esperaba ;");
                    }
                    else throw new Exception("Se esperaba record");
                }
                else throw new Exception("Se esperaba :=");
            }
            else throw new Exception("Se esperaba id");
        }

        void PL()
        {
            IDL();
            if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
            {
                currentToken = lex.NextToken();
                TYPE();
                PLprime();
            }
        }

        void PLprime()
        {
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                IDL();
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    currentToken = lex.NextToken();
                    TYPE();
                    PLprime();
                }
            }
            //else do nothing
        }

        void IDL()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                IDLprime();
            }
            else throw new Exception("Se esperba id");
        }

        void IDLprime()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    IDLprime();
                }
                else throw new Exception("Se esperaba id");
            }
        }

        void TYPE()
        {
            if (currentToken.Tipo == TipoToken.TK_ARRAY)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_INT_LIT)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_PUNTO)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_PUNTO)
                            {
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo == TipoToken.TK_INT_LIT)
                                {
                                    currentToken = lex.NextToken();
                                    if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                                    {
                                        currentToken = lex.NextToken();
                                        if (currentToken.Tipo == TipoToken.TK_OF)
                                        {
                                            currentToken = lex.NextToken();
                                            STYPE();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else STYPE();
        }

        void STYPE()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT:
                    return;
                case TipoToken.TK_FLOAT:
                    return;
                case TipoToken.TK_CHAR:
                    return;
                case TipoToken.TK_STRING:
                    return;
                case TipoToken.TK_BOOL:
                    return;
                default:
                    throw new Exception("Se esperaba un tipo primitivo");
            }
        }

        void VD()
        {
            if (currentToken.Tipo == TipoToken.TK_VAR)
            {
                currentToken = lex.NextToken();
                IDL();
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    TYPE();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        VD();
                    }
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba :");
            }//else return
        }

        void FD()
        {
            HEAD();
            VD();
            CS();
        }

        void HEAD()
        {
            if (currentToken.Tipo == TipoToken.TK_FUNCTION)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    ARGS();
                    if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                    {
                        currentToken = lex.NextToken();
                        STYPE();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
                        }
                        else throw new Exception("Se esperaba ;");
                    }
                    else throw new Exception("Se esperaba :");
                }
                else throw new Exception("Se esperaba id");
            }
            else if (currentToken.Tipo == TipoToken.TK_VOID)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    ARGS();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        currentToken = lex.NextToken();
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba id");
            }
            else throw new Exception("Se esperaba declaracion de subprograma");
        }

        void ARGS()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                PL();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    currentToken = lex.NextToken();
                else throw new Exception("Se esperaba )");
            }
            else throw new Exception("Se esperaba (");
        }

        void CL()
        {
            LIT();
            if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
            {
                currentToken = lex.NextToken();
                CS();
                CL();
            }
        }

        void LIT()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT_LIT:
                    return;
                case TipoToken.TK_FLOAT_LIT:
                    return;
                case TipoToken.TK_CHAR_LIT:
                    return;
                case TipoToken.TK_STRING_LIT:
                    return;
                case TipoToken.TK_TRUE:
                    return;
                case TipoToken.TK_FALSE:
                    return;
                default:
                    throw new Exception("Se esperaba una literal");
            }
        }

        void S_prime()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                exprl();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        currentToken = lex.NextToken();
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba )");
            }
            else
                S_prime2();
        }

        void S_prime2()
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    currentToken = lex.NextToken();
                else throw new Exception("Se esperaba ;");
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    S_prime2();
                }
            }
            else throw new Exception("solo asignacion, llamada y declaracion pueden ser sentencias.");
        }

        void exprl()
        {
            expr();
            exprlPrime();
        }

        void exprlPrime()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                expr();
                exprlPrime();
            }
            //else epsilon
        }

        void IFP()
        {
            CS();
            ELSE();
        }

        void CS()
        {
            if (currentToken.Tipo == TipoToken.TK_BEGIN)
            {
                currentToken = lex.NextToken();
                SL();
                if (currentToken.Tipo == TipoToken.TK_END)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        currentToken = lex.NextToken();
                    else throw new Exception("se esperaba ;");
                }
                else throw new Exception("se esperaba end");
            }
            else throw new Exception("Se esperaba begin");
        }

        void ELSE()
        {
            if (currentToken.Tipo == TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                CS();
            }
            //else do nothing and return :-)
        }

        void expr()
        {
            andexp();
            exPrime();
        }

        void exPrime()
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                andexp();
                exPrime();
            }
        }

        void andexp()
        {
            relexp();
            andexPrime();
        }

        void andexPrime()
        {
            if (currentToken.Tipo == TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                relexp();
                andexPrime();
            }
        }

        void relexp()
        {
            addexp();
            relexprime();
        }

        void relexprime()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_IGUALDAD:
                case TipoToken.TK_DISTINTO:
                case TipoToken.TK_MAYORIGUAL:
                case TipoToken.TK_MAYORQUE:
                case TipoToken.TK_MENORIGUAL:
                case TipoToken.TK_MENORQUE:
                    currentToken = lex.NextToken();
                    addexp();
                    break;
                default:
                    break;
            }
        }

        void addexp()
        {
            multexp();
            addexprime();
        }

        void addexprime()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_SUMA:
                case TipoToken.TK_RESTA:
                    currentToken = lex.NextToken();
                    multexp();
                    addexprime();
                    break;
                default:
                    break;
            }
        }

        void multexp()
        {
            parexp();
            multexprime();
        }

        void multexprime()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_MULT:
                case TipoToken.TK_DIVISION:
                case TipoToken.TK_MOD:
                case TipoToken.TK_DIV:
                    currentToken = lex.NextToken();
                    parexp();
                    multexprime();
                    break;
                default:
                    break;
            }
        }

        void parexp()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    currentToken = lex.NextToken();
                else throw new Exception("Se esperaba )");
            }
            else if (currentToken.Tipo == TipoToken.TK_NOT)
            {
                currentToken = lex.NextToken();
                expr();
            }
            else if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                S_prime();
            }
            else LIT();
        }
    }
}
