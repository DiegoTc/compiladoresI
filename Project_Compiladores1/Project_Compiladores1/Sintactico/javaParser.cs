using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;

namespace Project_Compiladores1.Sintactico
{
    /*
    class javaParser:Parser
    {
        
        public Token currentToken;
        public LexicoJava Lex;
        public javaParser (Lexico.LexicoJava lexer) : base(lexer)
        {
            
            Lex = lexer;
            currentToken = Lex.NextToken();

        }
                                
        public void Parse()
        {
            StatementList();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
        }

        public void StatementList()
        {            
            Statement();
            StatementList();
            
        }

        public void Statement()
        {
            if (currentToken.Tipo == TipoToken.TK_PRINT)
            {
                #region Print
                currentToken = Lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = Lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = Lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = Lex.NextToken();
                        }
                        else
                        {
                            throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                        }
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo )");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo (");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_READ)
            {
                #region Read

                currentToken = Lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = Lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = Lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = Lex.NextToken();
                        }
                        else
                        {
                            throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                        }
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo )");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo (");
                }

                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_IF)
            {
                #region If
                currentToken = Lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = Lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = Lex.NextToken();
                        CompoundStatement();
                        ELSE();                        
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo )");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo (");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_WHILE)
            {
                #region While
                currentToken = Lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = Lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = Lex.NextToken();
                        CompoundStatement();                        
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo )");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo (");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_DO)
            {
                #region Do
                currentToken = Lex.NextToken();
                CompoundStatement();
                if (currentToken.Tipo == TipoToken.TK_WHILE)
                {                   
                    currentToken = Lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                    {
                        currentToken = Lex.NextToken();
                        Expr();
                        if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                        {
                            currentToken = Lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = Lex.NextToken();
                            }
                            else
                            {
                                throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                            }
                        }
                        else
                        {
                            throw new Exception("Error Sintactico - Se esperaba simbolo )");
                        }
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo (");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba palabra fin de ciclo While");
                }

                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_FOR)
            {
                #region For
                currentToken = Lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = Lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_ID)
                    {
                        currentToken = Lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                        {
                            currentToken = Lex.NextToken();
                            Expr();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = Lex.NextToken();
                                Expr();
                                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                                {
                                    currentToken = Lex.NextToken();
                                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                                    {
                                        currentToken = Lex.NextToken();
                                        CompoundStatement();
                                    }
                                    else
                                    {
                                        throw new Exception("Error Sintactico - Se esperaba simbolo )");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                                }
                            }
                            else
                            {
                                throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                            }
                        }
                        else
                        {
                            throw new Exception("Error Sintactico - Se esperaba simbolo asignacion =");
                        }
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba un Identificador");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo (");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_RETURN)
        }

        public void ELSE()
        {
            
        }

        public void Expr()
        {
            
        }

        public void CompoundStatement()
        {
            
        }
    }*/
}
