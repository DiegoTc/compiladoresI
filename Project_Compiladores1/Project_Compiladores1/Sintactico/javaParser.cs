﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;

namespace Project_Compiladores1.Sintactico
{
    class javaParser : Parser
    {

        //public Token currentToken;
        //public LexicoJava Lex;
        public javaParser(Lexico.LexicoJava lexer)
            : base(lexer)
        {

        }

        public void parse()
        {
            StatementList();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
        }

        public void StatementList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ || currentToken.Tipo == Lexico.TipoToken.TK_IF ||
                currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO || currentToken.Tipo == Lexico.TipoToken.TK_FOR ||
                currentToken.Tipo == Lexico.TipoToken.TK_BREAK || currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN ||
                currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL ||
                currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                Statement();
                StatementList();
            }

        }

        public void Statement()
        {
            if (currentToken.Tipo == TipoToken.TK_PRINT)
            {
                #region Print
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
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

                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
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
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
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
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
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
                currentToken = lex.NextToken();
                CompoundStatement();
                if (currentToken.Tipo == TipoToken.TK_WHILE)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                    {
                        currentToken = lex.NextToken();
                        Expr();
                        if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = lex.NextToken();
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
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_ID)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                        {
                            currentToken = lex.NextToken();
                            Expr();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = lex.NextToken();
                                Expr();
                                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                                {
                                    currentToken = lex.NextToken();
                                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                                    {
                                        currentToken = lex.NextToken();
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
            {
                #region Return
                currentToken = lex.NextToken();
                Expr();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_BREAK)
            {
                #region Break
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                }
                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_ID)
            {
                #region Id
                currentToken = lex.NextToken();
                StatementP();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                }

                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                Declaration();
            }

        }

        public void Declaration()
        {
            Type();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                DeclarationP();
                AssignDeclaration();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un fin sentencia");
                }
            }
            else
            {
                throw new Exception("Error Sintactico - Se esperaba un identificador");
            }
        }

        public void Type()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                currentToken = lex.NextToken();
            }
        }

        public void DeclarationP()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    DeclarationP();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }

        }

        public void AssignDeclaration()
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
                Expr();
            }
        }


        public void StatementP()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                ExprList();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();

                }
            }
            else
            {
                StatementP2();
            }
        }

        public void StatementP2()
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN || currentToken.Tipo == TipoToken.TK_MASIGUAL || currentToken.Tipo == TipoToken.TK_MENOSIGUAL || currentToken.Tipo == TipoToken.TK_PORIGUAL || currentToken.Tipo == TipoToken.TK_ENTREIGUAL)
            {
                currentToken = lex.NextToken();
                Expr();

            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                Expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo ]");
                }
            }
        }

        public void ExprList()
        {
            Expr();
            ExprListP();
        }

        public void ExprListP()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Expr();
                ExprListP();
            }
        }


        public void ELSE()
        {
            if (currentToken.Tipo == TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                CompoundStatement();
            }
        }

        public void CompoundStatement()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENLLAVE)
            {
                currentToken = lex.NextToken();
                StatementList();
                if (currentToken.Tipo == TipoToken.TK_CLOSELLAVE)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo }");
                }
            }
        }

        public void Expr()
        {
            ANDExpr();
            ExprP();
        }

        public void ExprP()
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                Expr();
                ExprP();
            }
        }

        public void ANDExpr()
        {
            RelExpr();
            ANDExprP();
        }

        public void ANDExprP()
        {
            if (currentToken.Tipo == TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                ANDExpr();
                ANDExprP();
            }
        }

        public void RelExpr()
        {
            AddExpr();
            RelExprP();
        }

        public void RelExprP()
        {
            RelOP();
            AddExpr();
        }

        public void RelOP()
        {
            if (currentToken.Tipo == TipoToken.TK_IGUALDAD || currentToken.Tipo == TipoToken.TK_DISTINTO || currentToken.Tipo == TipoToken.TK_MAYORQUE || currentToken.Tipo == TipoToken.TK_MENORQUE || currentToken.Tipo == TipoToken.TK_MENORIGUAL || currentToken.Tipo == TipoToken.TK_MAYORIGUAL)
            {
                currentToken = lex.NextToken();
            }
        }

        public void AddExpr()
        {
            MultExpr();
            AddExprP();
        }

        public void AddExprP()
        {
            if (currentToken.Tipo == TipoToken.TK_SUMA || currentToken.Tipo == TipoToken.TK_RESTA)
            {
                currentToken = lex.NextToken();
                AddExpr();
                AddExprP();
            }
        }

        public void MultExpr()
        {
            ParExpr();
            MultExprP();
        }

        public void MultExprP()
        {
            if (currentToken.Tipo == TipoToken.TK_MULT || currentToken.Tipo == TipoToken.TK_DIVISION || currentToken.Tipo == TipoToken.TK_MOD)
            {
                currentToken = lex.NextToken();
                MultExpr();
                MultExprP();
            }
        }

        public void ParExpr()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                StatementP();
            }
            else if (currentToken.Tipo == TipoToken.TK_INT_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_FLOAT_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_STRING_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_CHAR_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_TRUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_FALSE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo )");
                }
            }
        }


    }
}
