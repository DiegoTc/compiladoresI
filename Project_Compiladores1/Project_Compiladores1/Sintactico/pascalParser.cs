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

                    }
                }
            }
        }

        void VD()
        {
        }

        void FD()
        {
        }

        void CL()
        {

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

        }
    }
}
