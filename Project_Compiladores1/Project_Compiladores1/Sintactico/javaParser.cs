﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;
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

        public Sentencia parse()
        {
            Sentencia S = StatementList();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
            return S;
            Console.WriteLine("Evaluacion Sintactica Correcta");
        }

        public Sentencia StatementList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ || currentToken.Tipo == Lexico.TipoToken.TK_IF ||
                currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO || currentToken.Tipo == Lexico.TipoToken.TK_FOR ||
                currentToken.Tipo == Lexico.TipoToken.TK_BREAK || currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN ||
                currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL ||
                currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT || currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC)
            {
                Sentencia S = Statement();
                S.sig = StatementList();
                return S;
            }
            else
            {
                return null;
            }

        }

        public Sentencia Statement()
        {
            if (currentToken.Tipo == TipoToken.TK_PRINT)
            {
                #region Print
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    S_Print sPrint = new S_Print();
                    sPrint.Expr = Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
                            return sPrint;
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
            }/*
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
            }*/
            else if (currentToken.Tipo == TipoToken.TK_IF)
            {
                #region If
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    S_If sIf = new S_If();
                    sIf.Condicion = Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        sIf.Cierto = CompoundStatement();
                        sIf.Falso = ELSE();
                        return sIf;
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
                    S_While sWhile = new S_While();
                    sWhile.Condicion = Expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        sWhile.S = CompoundStatement();
                        return sWhile;
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
                S_Do sDo = new S_Do();
                sDo.S = CompoundStatement();
                if (currentToken.Tipo == TipoToken.TK_WHILE)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                    {
                        currentToken = lex.NextToken();
                        sDo.S = Expr();
                        if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = lex.NextToken();
                                return sDo;
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
                    S_For sFor = new S_For();
                    if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
                    {
                        sFor.Tip = Type();
                    }
                    if (currentToken.Tipo == TipoToken.TK_ID)
                    {
                        sFor.Var.id = currentToken.Lexema;
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                        {
                            currentToken = lex.NextToken();
                            sFor.Inicio = Expr();
                            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                            {
                                currentToken = lex.NextToken();
                                sFor.Condicion = Expr();
                                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                                {
                                    currentToken = lex.NextToken();
                                    sFor.Iteracion = Expr();                                
                                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                                    {
                                        currentToken = lex.NextToken();
                                        sFor.S = CompoundStatement();
                                        return sFor;
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
                S_Return sReturn = new S_Return();
                sReturn.Expr = Expr();
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
            else if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT || currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC)
            {
                Declaration();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Error Sintactico - Se esperaba un (");
                this.currentToken = lex.NextToken();
                Expr();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Error Sintactico - Se esperaba una )");

                this.currentToken = lex.NextToken();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENLLAVE)
                    throw new Exception("Error Sintactico - Se esperaba una {");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CASE)
                    throw new Exception("Error Sintactico - Se esperaba la palabra reservada CASE");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT &&
                    this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT &&
                    this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                    throw new Exception("Error Sintactico - Se esperaba un numero o un char");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Error Sintactico - Se esperaba el simbolo :");
                currentToken = lex.NextToken();
                StatementList();


                /*while (this.currentToken.Tipo == Lexico.TipoToken.TK_CASE)
                {
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT &&
                        this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT &&
                        this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                        throw new Exception("Se esperaba un numero o un char");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                        throw new Exception("Se esperaba el simbolo :");
                    currentToken = lex.NextToken();
                    StatementList();
                }*/

                Cases();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                    throw new Exception("Error Sintactico - Se esperaba la palabra reservada DEFAULT");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Error Sintactico - Se esperaba el simbolo :");
                this.currentToken = lex.NextToken();
                StatementList();


                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Error Sintactico - Se esperaba una }");

                this.currentToken = lex.NextToken();
            }
        }

        public void Cases()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_CASE)
            {                
                currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT &&
                        this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT &&
                        this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                    {
                        currentToken = lex.NextToken();
                        StatementList();
                        Cases();
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba el simbolo :");   
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un numero o un char");
                }
            }
        }

        public void Declaration()
        {
            VARTYPE();
            Type();
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

        public void VARTYPE()
        {
            if (currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC)
            {
                currentToken = lex.NextToken();
            }
        }

        public void Type()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT || currentToken.Tipo == TipoToken.TK_VOID)
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
            else if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                AssignDeclaration();
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                ParameterList();
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
            else if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
            }
            else
            {
                throw new Exception("Error Sintactico - Se esperaba un fin sentencia");
            }
        }

        public void ParameterList()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                Type();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    ParameterListP();
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }
        }

        public void ParameterListP()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Type();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    ParameterListP();
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
                DeclarationP();

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
                if (currentToken.Tipo == TipoToken.TK_READ)
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
                else
                {
                    Expr();    
                }
                

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
            else if (currentToken.Tipo == TipoToken.TK_MASMAS || currentToken.Tipo == TipoToken.TK_MENOSMENOS)
            {
                Expr();
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
            else if (currentToken.Tipo == TipoToken.TK_MASMAS || currentToken.Tipo == TipoToken.TK_MENOSMENOS)
            {
                currentToken = lex.NextToken();
            }
        }

    }
}
