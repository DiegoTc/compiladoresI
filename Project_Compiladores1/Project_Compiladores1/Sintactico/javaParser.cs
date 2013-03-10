using System;
using System.Collections;
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
            
            Console.WriteLine("Evaluacion Sintactica Correcta");
            return S;
        }

        public Sentencia StatementList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ || currentToken.Tipo == Lexico.TipoToken.TK_IF ||
                currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO || currentToken.Tipo == Lexico.TipoToken.TK_FOR ||
                currentToken.Tipo == Lexico.TipoToken.TK_BREAK || currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN ||
                currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL ||
                currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT || currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC || currentToken.Tipo == TipoToken.TK_CLASS)
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
            }
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
                        sDo.Condicion = Expr();
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
                Sentencia S ;
                Variable Id = new Variable(currentToken.Lexema, null);
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
            else if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT )
            {
                #region Declaraciones
                Sentencia S;
                S = Declaration();
                return S;
                #endregion
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {
                #region Switch
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
                #endregion 
            }
            else if (currentToken.Tipo == TipoToken.TK_PRIVATE || currentToken.Tipo == TipoToken.TK_PUBLIC)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_CLASS)
                {
                    return Clases();
                }
                else
                {
                    #region Declaraciones
                    Sentencia S  ;
                    S = Declaration();
                    return S;
                    #endregion
                }
            }
           
            return null;
        }

        public Sentencia Clases()
        {
            currentToken = lex.NextToken();
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Error Sintactico - Se esperaba un ID");
            S_Class sClass = new S_Class();
            sClass.Var.id = currentToken.Lexema;
            currentToken = lex.NextToken();
            if (currentToken.Tipo != TipoToken.TK_OPENLLAVE)
                throw new Exception("Error Sintactico - Se esperaba el simbolo {");
            currentToken = lex.NextToken();
            //sClass.CamposClase = ((Campos)DeclarationClass());
            sClass.CamposClase = DeclarationClass();
            if (currentToken.Tipo != TipoToken.TK_CLOSELLAVE)
                throw new Exception("Error Sintactico - Se esperaba el simbolo }");
            currentToken = lex.NextToken();
            return sClass;
        }

        public Sentencia DeclarationClass()
        {
            /*Campos S = ((Campos)Declaration());
            if (currentToken.Tipo == TipoToken.TK_PUBLIC || currentToken.Tipo == TipoToken.TK_PRIVATE)
            {
                S.Sig = ((Campos)DeclarationClass());
            }*/
            Sentencia S = Declaration();
            if (currentToken.Tipo == TipoToken.TK_PUBLIC || currentToken.Tipo == TipoToken.TK_PRIVATE|| currentToken.Tipo == TipoToken.TK_ID)
            {
                S.sig = DeclarationClass();
            }
            return S;
        }

        public Cases Cases()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_CASE)
            {                
                currentToken = lex.NextToken();
                Cases C = new Cases();
                
                if (currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT ||
                        currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT ||
                        currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                {
                    C.Valor = Expr();
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

        public int arrayDimensions(int dim)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                if(currentToken.Tipo != TipoToken.TK_CLOSECOR)
                    throw new Exception("Error Sintactico - Se esperaba ]");
                currentToken = lex.NextToken();
                dim++;
                arrayDimensions(dim);
            }
            return dim;
        }

        public Sentencia Declaration()
        {
            VARTYPE();
            Declaracion C = new Declaracion();
            C.Tip = Type();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                C.Var.id = currentToken.Lexema;                
                currentToken = lex.NextToken();
                Sentencia S = DeclarationP(C);
                return S;
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    int dim = arrayDimensions(1);
                    if (currentToken.Tipo == TipoToken.TK_ID)
                    {
                        Arreglo ArrTip = new Arreglo();
                        ArrTip.Contenido = C.Tip;
                        ArrTip.Dimensiones = dim;
                        C.Tip = ArrTip;
                        
                        C.Var.id = currentToken.Lexema;
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

        public Tipo Type()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR)
            {
                Caracter C = new Caracter();
                currentToken = lex.NextToken();
                return C;
            }
            else if (currentToken.Tipo == TipoToken.TK_BOOL)
            {
                Booleano B = new Booleano();
                currentToken = lex.NextToken();
                return B;
            }
            else if (currentToken.Tipo == TipoToken.TK_STRING)
            {
                Cadena C = new Cadena();
                currentToken = lex.NextToken();
                return C;
            }
            else if (currentToken.Tipo == TipoToken.TK_FLOAT)
            {
                Flotante F = new Flotante();
                currentToken = lex.NextToken();
                return F;
            }
            else if (currentToken.Tipo == TipoToken.TK_INT)
            {
                Entero C = new Entero();
                currentToken = lex.NextToken();
                return C;
            }
            else if (currentToken.Tipo == TipoToken.TK_VOID)
            {
                Voids V = new Voids();
                currentToken = lex.NextToken();
                return V;
            }
            return null;
        }

        public Sentencia DeclarationP(Sentencia C)
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                //Campos CP = new Campos();                
                if (C is Declaracion)
                {
                    ((Declaracion)C).Sig = new Declaracion();
                }
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                   ((Declaracion)C).Sig.Var.id = currentToken.Lexema;
                   ((Declaracion)C).Sig.Tip = ((Declaracion)C).Tip;
                    currentToken = lex.NextToken();
                    DeclarationP(((Declaracion)C).Sig);
                    
                    return C;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un identificador");
                }
            }
            else if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                Sentencia S  ;
                S = AssignDeclaration(C);
                if (((Declaracion)C).Tip == null)
                    return S;
                else
                {
                    ((Declaracion)C).Valor = ((S_Asignacion) S).Valor;
                    return C;
                }                                
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                S_Functions sFunctions = new S_Functions();
                sFunctions.Retorno = ((Declaracion)C).Tip;
                sFunctions.Var = ((Declaracion)C).Var.id;
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

        public Declaracion ParameterList()
        {
            if (currentToken.Tipo == TipoToken.TK_CHAR || currentToken.Tipo == TipoToken.TK_BOOL || currentToken.Tipo == TipoToken.TK_STRING || currentToken.Tipo == TipoToken.TK_FLOAT || currentToken.Tipo == TipoToken.TK_INT)
            {
                Declaracion C = new Declaracion();
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

        public Declaracion ParameterListP()
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                Declaracion C1 = new Declaracion();
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
                if (currentToken.Tipo == TipoToken.TK_NEW)
                {
                    currentToken = lex.NextToken();
                    sAsignacion.id.id = ((Declaracion)S).Var.id;
                    sAsignacion.Valor = Expr();
                    Class c = new Class();
                    if(sAsignacion.Valor is ExprFuncion)
                        ((ExprFuncion)sAsignacion.Valor).tipo = c;
    
                    return DeclarationP(sAsignacion);
                }
                else
                {
                    sAsignacion.id.id = ((Declaracion)S).Var.id;
                    sAsignacion.Valor = Expr();
                    return DeclarationP(sAsignacion);
                }
            }
            else
            {
                return S;
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
                Sentencia S ;
                S = StatementP2(Id);
                return S;
            }
        }
        
        private Sentencia declareArray(Declaracion c)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                Expresiones E = Expr();
                if (E is LiteralEntero)
                {
                    LiteralEntero e = ((LiteralEntero)E);
                    c.dim.Add(E);
                }
                else
                {
                    c.dim.Add(E);
                }
                if (currentToken.Tipo == Lexico.TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    Sentencia s  ;
                    s = declareArray(c);
                    return s;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba un simbolo ]");
                }
            }
            else if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                return c;
            }
            throw new Exception("Error Sintactico - Se esperaba un simbolo ;");
            
        }

        public Sentencia StatementP2(Variable Id)
        {
            if (currentToken.Tipo == TipoToken.TK_PUNTO)
            {
                AccessMiembro Ls = new AccessMiembro();
                Id.accesor = Atributos(Ls);              

            }
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
                else if (currentToken.Tipo == TipoToken.TK_NEW)
                {
                    Declaracion campos = new Declaracion();
                    campos.Var.id = Id.id;
                    currentToken = lex.NextToken();
                    campos.Tip = Type();
                    //currentToken = lex.NextToken();
                    if(currentToken.Tipo != TipoToken.TK_OPENCOR)
                        throw new Exception("Error Sintactico - Se esperaba simbolo [");

                    Sentencia s = declareArray(campos);
                    S_Asignacion sAsignacion = new S_Asignacion();
                    sAsignacion.Op = new Igual();
                    sAsignacion.id = Id;
                    sAsignacion.campos = ((Declaracion)campos);
                    return sAsignacion;
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
                sAsignacion.id.accesor = Expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    recorrerArray(sAsignacion);
                    Sentencia S  ;
                    S = StatementP2(sAsignacion.id);
                    sAsignacion.Valor = ((S_Asignacion) S).Valor;
                    /*if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        return sAsignacion;
                    }*/
                    return sAsignacion;
                    /*else
                    {
                        throw new Exception("Error Sintactico - Se esperaba simbolo ;");
                    }*/
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
            else if (Id.accesor.Last() is AccessFunc)
            {
                S_LlamadaFunc SL = new S_LlamadaFunc();
                AccessFunc tmp = ((AccessFunc) Id.accesor.Last());
                SL.Var = Id;
                SL.VarList = tmp.Info.VarList;                

                return SL;
            }
            else
            {
                return null;
            }
        }

        public Access Atributos(Access List)
        {
            currentToken = lex.NextToken();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                string tmpid = currentToken.Lexema;
                Expresiones e = Expr();
                if (e is ExprFuncion)
                {
                    AccessFunc accF = new AccessFunc();
                    accF.Info = ((ExprFuncion)e);
                    List = accF;
                    return List;
                }
                else
                {
                    AccessMiembro accM = new AccessMiembro();
                    accM.Id = ((Variable) e).id;
                    List = accM;
                }
                if (currentToken.Tipo == TipoToken.TK_PUNTO)
                {
                    Atributos(List.Next);
                }                                              
            }
            return List;
        }
        

        public void recorrerArray(S_Asignacion assig)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                assig.id.access.Add(Expr());
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                    throw new Exception("Se esperaba el token ]");
                currentToken = lex.NextToken();
                recorrerArray(assig);
            }
        }

        public Expresiones ExprList()
        {
            ListaExpre listaExpre = new ListaExpre();
            listaExpre.Ex.Add(Expr());            

            return ExprListP(listaExpre);
        }

        public ListaExpre ExprListP(Expresiones E)
        {
                        
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {                
                currentToken = lex.NextToken();
                ((ListaExpre) E).Ex.Add(Expr());                
                return ExprListP(E);
            }
            else
            {
                return ((ListaExpre)E);
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
                Sentencia S ;
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
                currentToken = lex.NextToken();
                Equal eEqual = new Equal(E, AddExpr());
                return eEqual;
            }
            else if (currentToken.Tipo == TipoToken.TK_DISTINTO)
            {
                currentToken = lex.NextToken();
                Distinto eDist = new Distinto(E, AddExpr());
                
                return eDist;
            }
            else if (currentToken.Tipo == TipoToken.TK_MAYORQUE)
            {
                currentToken = lex.NextToken();
                MayorQue eMayQ = new MayorQue(E, AddExpr());
                
                return eMayQ;
            }
            else if (currentToken.Tipo == TipoToken.TK_MENORQUE)
            {
                currentToken = lex.NextToken();
                MenorQue eMenQ = new MenorQue(E, AddExpr());
                
                return eMenQ;
            }
            else if (currentToken.Tipo == TipoToken.TK_MENORIGUAL)
            {
                currentToken = lex.NextToken();
                MenorIgual eMenI = new MenorIgual(E, AddExpr());
                
                return eMenI;
            }
            else if (currentToken.Tipo == TipoToken.TK_MAYORIGUAL)
            {
                currentToken = lex.NextToken();
                MayorIgual eMayI = new MayorIgual(E, AddExpr());
                
                return eMayI;
            }
            else
            {
                return E;
            }
        }

        public void RelOP()
        {
            if (currentToken.Tipo == TipoToken.TK_IGUALDAD || currentToken.Tipo == TipoToken.TK_DISTINTO || currentToken.Tipo == TipoToken.TK_MAYORQUE || currentToken.Tipo == TipoToken.TK_MENORQUE || currentToken.Tipo == TipoToken.TK_MENORIGUAL || currentToken.Tipo == TipoToken.TK_MAYORIGUAL)
            {
                currentToken = lex.NextToken();
            }
        }

        public Expresiones AddExpr()
        {
            Expresiones EX = MultExpr();
            return AddExprP(EX);
        }

        public Expresiones AddExprP(Expresiones E)
        {
            if (currentToken.Tipo == TipoToken.TK_SUMA)
            {
                currentToken = lex.NextToken();
                Suma Sum = new Suma(E, AddExprP(AddExpr()));

                return Sum;
            }
            else if (currentToken.Tipo == TipoToken.TK_RESTA)
            {
                currentToken = lex.NextToken();
                Resta Res = new Resta(E, AddExprP(AddExpr()));

                return Res;
            }
            else
            {
                return E;
            }
        }

        public Expresiones MultExpr()
        {
            Expresiones EX = ParExpr();
            return MultExprP(EX);
        }

        public Expresiones MultExprP(Expresiones E)
        {
            if (currentToken.Tipo == TipoToken.TK_MULT)
            {
                currentToken = lex.NextToken();
                Multiplicacion Mult = new Multiplicacion(E, MultExprP(MultExpr()));
                return Mult;

            }
            else if (currentToken.Tipo == TipoToken.TK_DIVISION)
            {
                currentToken = lex.NextToken();
                Division Div = new Division(E, MultExprP(MultExpr()));
                return Div;
            }
            else if (currentToken.Tipo == TipoToken.TK_MOD)
            {
                currentToken = lex.NextToken();
                Mod Md = new Mod(E, MultExprP(MultExpr()));
                return Md;
            }
            else
            {
                return E;
            }
        }

        public Expresiones ParExpr()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                Variable V = new Variable();
                V.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_MASMAS)
                {
                    ExpMasMas expMasMas = new ExpMasMas();
                    expMasMas.ID = V;
                    currentToken = lex.NextToken();
                    return expMasMas;
                }
                else if (currentToken.Tipo == TipoToken.TK_MENOSMENOS)
                {
                    ExpMenosMenos expMenosMenos = new ExpMenosMenos();
                    expMenosMenos.ID = V;
                    currentToken = lex.NextToken();
                    return expMenosMenos;
                }
                else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    ExprFuncion exprFuncion = new ExprFuncion();
                    exprFuncion.ID = V;
                    exprFuncion.VarList =  ExprFun(V);
                    return exprFuncion;
                }
                else
                {
                    //if(currentToken.Tipo != TipoToken.TK_ASSIGN)
                    //    StatementP(V);
                }
                return V;
               
            }
            else if (currentToken.Tipo == TipoToken.TK_INT_LIT)
            {
                LiteralEntero LE = new LiteralEntero(Convert.ToInt32(currentToken.Lexema));
                currentToken = lex.NextToken();
                return LE;
            }
            else if (currentToken.Tipo == TipoToken.TK_FLOAT_LIT)
            {
                LiteralFlotante LF = new LiteralFlotante(float.Parse(currentToken.Lexema));
                currentToken = lex.NextToken();
                return LF;
            }
            else if (currentToken.Tipo == TipoToken.TK_STRING_LIT)
            {
                LitString LS = new LitString(currentToken.Lexema);
                currentToken = lex.NextToken();
                return LS;
            }
            else if (currentToken.Tipo == TipoToken.TK_CHAR_LIT)
            {
                LitChar LC = new LitChar(currentToken.Lexema);
                currentToken = lex.NextToken();
                return LC;
            }
            else if (currentToken.Tipo == TipoToken.TK_TRUE)
            {
                LitBool LB = new LitBool(true);
                currentToken = lex.NextToken();
                return LB;
            }
            else if (currentToken.Tipo == TipoToken.TK_FALSE)
            {
                LitBool LB = new LitBool(false);
                currentToken = lex.NextToken();
                return LB;
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Expresiones EX = Expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return EX;
                }
                else
                {
                    throw new Exception("Error Sintactico - Se esperaba simbolo )");
                }
            }            

            return null;

        }

        public Expresiones ExprFun (Variable Id)
        {                    
            return ((S_LlamadaFunc) StatementP(Id)).VarList;            
        }

    }
}
