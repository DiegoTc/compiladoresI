using System;
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
                    return sReturn;
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
                    S_Break sBreak = new S_Break();
                    currentToken = lex.NextToken();
                    return sBreak;
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
                Sentencia S = new Sentencia();
                Variable Id = new Variable();
                Id.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                
                S = StatementP(Id);
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                    return S;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                }

                #endregion
            }
            else if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT || currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC)
            {
                Sentencia S = new Sentencia();
                S = Declaration();
                return S;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Error Sintactico - Se esperaba un (");
                currentToken = lex.NextToken();
                S_Switch sSwitch = new S_Switch();
                sSwitch.Var = Expr();

                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Error Sintactico - Se esperaba una )");

                currentToken = lex.NextToken();

                if (currentToken.Tipo != Lexico.TipoToken.TK_OPENLLAVE)
                    throw new Exception("Error Sintactico - Se esperaba una {");

                currentToken = lex.NextToken();                

                sSwitch.Casos = Cases();

                if (currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                    throw new Exception("Error Sintactico - Se esperaba la palabra reservada DEFAULT");

                currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Error Sintactico - Se esperaba el simbolo :");
                currentToken = lex.NextToken();
                sSwitch.sdefault = StatementList();


                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Error Sintactico - Se esperaba una }");

                currentToken = lex.NextToken();
                return sSwitch;
            }
        }

        public Cases Cases()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_CASE)
            {                
                currentToken = lex.NextToken();
                Cases C = new Cases();
                
                if (currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT &&
                        currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT &&
                        currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                {
                    C.Valor = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                    {
                        currentToken = lex.NextToken();
                        C.S = StatementList();
                        C.sig = Cases();
                        return C;
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
            else
            {
                return null;
            }
        }

        public Sentencia Declaration()
        {
            VARTYPE();
            Campos C = new Campos();
            C.Tip = Type();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                C.Var.id = currentToken.Lexema;                
                currentToken = lex.NextToken();
                DeclarationP(C);
                return C;
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_ID)
                    {
                        C.Dimension = 1;
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
                            return C;
                        }
                        else
                        {
                            throw new Exception("Error Sintactico - Se esperaba fin sentencia ;");
                        }
                    }
                    else
                    {
                        throw new Exception("Error Sintactico - Se esperaba un identificador");
                    }
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un simbolo ]");
                }
            }
            else
            
            {
                throw new Exception("Error Sintactico - Se esperaba un identificador o simbolo [");
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

        public Sentencia DeclarationP(Sentencia C)
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Campos CP = new Campos();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {                    
                    CP.Var.id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    DeclarationP(CP);
                    C.sig = CP;
                    return C;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }
            else if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                return AssignDeclaration(C);

            }
            else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                S_Functions sFunctions = new S_Functions();
                sFunctions.Retorno = ((Campos)C).Tip;
                sFunctions.var.id = ((Campos)C).Var.id;
                sFunctions.Campo = ParameterList();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    sFunctions.S = CompoundStatement();
                    return sFunctions;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo )");
                }
            }
            else if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                return C;
            }
            else
            {
                throw new Exception("Error Sintactico - Se esperaba un fin sentencia");
            }
        }

        public Campos ParameterList()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                Campos C = new Campos();
                C.Tip = Type();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    C.Var.id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    C.Sig = ParameterListP();
                    return C;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }
            else
            {
                return null;
            }
        }

        public Campos ParameterListP()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                Campos C1 = new Campos();
                currentToken = lex.NextToken();
                C1.Tip = Type();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    C1.Var.id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    C1.Sig = ParameterListP();
                    return C1;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }
            else
            {
                return null;
            }
        }

        public Sentencia AssignDeclaration(Sentencia S)
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                S_Asignacion sAsignacion = new S_Asignacion();
                currentToken = lex.NextToken();
                sAsignacion.id.id = ((Campos)S).Var.id;
                sAsignacion.Valor = Expr();
                return DeclarationP(sAsignacion);

            }
        }

        public Sentencia StatementP(Variable Id)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                S_LlamadaFunc sLlamadaFunc = new S_LlamadaFunc();
                sLlamadaFunc.Var = Id;
                currentToken = lex.NextToken();
                sLlamadaFunc.VarList = ExprList();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return sLlamadaFunc;
                }
                else
                {
                    throw new Exception("Error Sintactico - Falta simbolo )");
                }
            }
            else
            {
                Sentencia S =new Sentencia();
                S = StatementP2(Id);
                return S;
            }
        }

        public Sentencia StatementP2(Variable Id)
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN || currentToken.Tipo == TipoToken.TK_MASIGUAL || currentToken.Tipo == TipoToken.TK_MENOSIGUAL || currentToken.Tipo == TipoToken.TK_PORIGUAL || currentToken.Tipo == TipoToken.TK_ENTREIGUAL)
            {
                var Tip = currentToken.Tipo;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_READ)
                {
                    S_Read sRead = new S_Read();
                    sRead.var = Id;
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
                                return sRead;
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
                    Expresiones E = Expr();                    
                    S_Asignacion sAsignacion = new S_Asignacion();
                    sAsignacion.id = Id;
                    if (Tip == TipoToken.TK_ASSIGN)
                        sAsignacion.Op = new Igual();
                    else if (Tip == TipoToken.TK_MASIGUAL)
                        sAsignacion.Op = new MasIgual();
                    else if (Tip == TipoToken.TK_MENOSIGUAL)
                        sAsignacion.Op = new MenosIgual();
                    else if (Tip == TipoToken.TK_PORIGUAL)
                        sAsignacion.Op = new PorIgual();
                    else if (Tip == TipoToken.TK_ENTREIGUAL)
                        sAsignacion.Op = new EntreIgual();
                    sAsignacion.Valor = E;
                    return sAsignacion;
                }
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                S_Asignacion sAsignacion = new S_Asignacion();
                sAsignacion.id = Id;
                sAsignacion.id.acces = Expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    Sentencia S = new Sentencia();
                    S = StatementP2(sAsignacion.id);
                    sAsignacion.Valor = ((S_Asignacion) S).Valor;
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        return sAsignacion;
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
                S_Asignacion sAsignacion = new S_Asignacion();
                sAsignacion.id = Id;
                if (currentToken.Tipo == TipoToken.TK_MASMAS)
                    sAsignacion.Op = new MasMas();
                else if (currentToken.Tipo == TipoToken.TK_MENOSMENOS)
                    sAsignacion.Op = new MenosMenos();
                Expresiones Ex = Expr();
                sAsignacion.Valor = Ex;
                return sAsignacion;
            }
        }

        public Expresiones ExprList()
        {            
            Expresiones E = Expr();
            return ExprListP(E);
        }

        public ListaExpre ExprListP(Expresiones E)
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Expresiones E1 = Expr();
                return ExprListP(E1);
            }
            else
            {
                return E;
            }
        }

        public Sentencia ELSE()
        {
            if (currentToken.Tipo == TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                Sentencia S = CompoundStatement();
                return S;
            }
            else
            {
                return null;
            }
        }

        public Sentencia CompoundStatement()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENLLAVE)
            {
                currentToken = lex.NextToken();
                Sentencia S = new Sentencia();
                S = StatementList();

                if (currentToken.Tipo == TipoToken.TK_CLOSELLAVE)
                {
                    currentToken = lex.NextToken();
                    return S;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo }");
                }
            }
            else
            {
                return null;
            }
        }

        public Expresiones Expr()
        {
            Expresiones E1 = ANDExpr();
            return ExprP(E1);
        }

        public Expresiones ExprP(Expresiones E)
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                
                currentToken = lex.NextToken();
                Expresiones E1 = Expr();
                Or eOr = new Or(E, ExprP(E1));
                return eOr;
            }
            else
            {
                return E;
            }
        }

        public Expresiones ANDExpr()
        {
            Expresiones E = RelExpr();
            return ANDExprP(E);
        }

        public Expresiones ANDExprP(Expresiones E)
        {
            if (currentToken.Tipo == TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                Expresiones E1 = ANDExpr();
                And eAnd = new And(E, ANDExprP(E1));
                return eAnd;
            }
            else
            {
                return E;
            }
        }

        public Expresiones RelExpr()
        {
            Expresiones E = AddExpr();
            return RelExprP(E);
        }

        public Expresiones RelExprP(Expresiones E)
        {
            

            if (currentToken.Tipo == TipoToken.TK_IGUALDAD)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_DISTINTO)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_MAYORQUE)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_MENORQUE)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_MENORIGUAL)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_MAYORIGUAL)
            {
                Equal eEqual = new Equal(E, AddExpr());
                currentToken = lex.NextToken();
                return eEqual;
            }

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
