using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;

namespace Project_Compiladores1.Sintactico
{
    class parserC : Parser
    {
        public parserC(Lexico.Lexico lexer)
            : base(lexer)
        {
        }
        
        public Sentencia parse()
        {
            try
            {
                Sentencia S = StatementList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_FINFLUJO)
                    throw new Exception("Se esperaba fin flujo");
                Console.WriteLine("Proyecto Terminado\n");
                return S;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Sentencia StatementList()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ || currentToken.Tipo == Lexico.TipoToken.TK_IF ||
                    currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO || currentToken.Tipo == Lexico.TipoToken.TK_FOR ||
                    currentToken.Tipo == Lexico.TipoToken.TK_BREAK || currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN ||
                    currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT ||
                    currentToken.Tipo == Lexico.TipoToken.TK_CHAR || currentToken.Tipo == Lexico.TipoToken.TK_BOOL || currentToken.Tipo == Lexico.TipoToken.TK_STRUCT || currentToken.Tipo == Lexico.TipoToken.TK_VOID)
                {
                    Sentencia s = Statement();
                    s.sig = StatementList();
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Tipo Tipo()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_INT)
                {
                    currentToken = lex.NextToken();
                    return new Entero();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR)
                {
                    currentToken = lex.NextToken();
                    return new Caracter();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_FLOAT)
                {
                    currentToken = lex.NextToken();
                    return new Flotante();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_STRING)
                {
                    currentToken = lex.NextToken();
                    return new Cadena();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_VOID)
                {
                    currentToken = lex.NextToken();
                    return new Voids();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_STRUCT)
                {
                    currentToken = lex.NextToken();
                    return new Struct();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_BOOL)
                {
                    currentToken = lex.NextToken();
                    return new Booleano();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Sentencia Statement()
        {
            try
            {
                //Para Declaraciones
                if (currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_STRUCT ||
                   currentToken.Tipo == Lexico.TipoToken.TK_CHAR || currentToken.Tipo == Lexico.TipoToken.TK_VOID || currentToken.Tipo == Lexico.TipoToken.TK_STRING ||
                    currentToken.Tipo == Lexico.TipoToken.TK_BOOL)
                {
                    return DeclaracionesC();
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_PRINT)
                {
                    #region
                    this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");
                    this.currentToken = lex.NextToken();
                    S_Print sprint = new S_Print();
                    sprint.Expr = Expression();

                    //this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");
                    this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");
                    this.currentToken = lex.NextToken();
                    return sprint;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_READ)
                {
                    #region
                    this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");
                    this.currentToken = lex.NextToken();

                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba un ID");

                    S_Read sread = new S_Read();
                    sread.var.id = currentToken.Lexema;
                    this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");
                    this.currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");
                    this.currentToken = lex.NextToken();
                    return sread;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_IF)
                {
                    #region
                    S_If sif = new S_If();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");

                    this.currentToken = lex.NextToken();
                    sif.Condicion = Expression();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");

                    this.currentToken = lex.NextToken();
                    sif.Cierto = CompoundStatement();
                    sif.Falso = Else();
                    return sif;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
                {
                    #region
                    S_While swhile = new S_While();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");

                    this.currentToken = lex.NextToken();
                    swhile.Condicion = Expression();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");

                    this.currentToken = lex.NextToken();
                    swhile.S = CompoundStatement();
                    return swhile;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_DO)
                {
                    #region
                    S_Do sdo = new S_Do();
                    this.currentToken = lex.NextToken();
                    sdo.S = CompoundStatement();

                    if (this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
                    {
                        this.currentToken = lex.NextToken();
                        if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                            throw new Exception("Se esperaba un (");

                        this.currentToken = lex.NextToken();
                        sdo.Condicion = Expression();

                        if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                            throw new Exception("Se esperaba un )");

                        this.currentToken = lex.NextToken();
                        if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                            throw new Exception("Se esperaba un ;");
                        this.currentToken = lex.NextToken();
                        return sdo;
                    }
                    else
                        throw new Exception("Se esperaba la palabra reservada WHILE");
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_FOR)
                {
                    #region
                    S_For sfor = new S_For();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba un ID");

                    sfor.Var.id = this.currentToken.Lexema;
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_ASSIGN)
                        throw new Exception("Se esperaba el simbolo =");

                    this.currentToken = lex.NextToken();
                    sfor.Inicio = Expression();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");

                    this.currentToken = lex.NextToken();
                    sfor.Condicion = Expression();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");

                    this.currentToken = lex.NextToken();
                    sfor.Iteracion = Expression();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");

                    this.currentToken = lex.NextToken();
                    sfor.Tip = new Entero();
                    sfor.S = CompoundStatement();
                    sfor.Tip = new Entero();
                    return sfor;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_RETURN)
                {
                    #region
                    S_Return sreturn = new S_Return();
                    this.currentToken = lex.NextToken();
                    sreturn.Expr = Expression();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");

                    this.currentToken = lex.NextToken();
                    return sreturn;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_BREAK)
                {
                    #region
                    S_Break sbreak = new S_Break();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");

                    this.currentToken = lex.NextToken();
                    return sbreak;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
                {
                    #region
                    S_Switch sSwitch = new S_Switch();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");
                    this.currentToken = lex.NextToken();
                    sSwitch.Var = Expression();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba una )");

                    this.currentToken = lex.NextToken();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENLLAVE)
                        throw new Exception("Se esperaba una {");

                    this.currentToken = lex.NextToken();
                    sSwitch.Casos = Cases();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                        throw new Exception("Se esperaba la palabra reservada DEFAULT");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                        throw new Exception("Se esperaba el simbolo :");
                    this.currentToken = lex.NextToken();
                    sSwitch.sdefault = StatementList();


                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                        throw new Exception("Se esperaba una }");

                    this.currentToken = lex.NextToken();
                    return sSwitch;
                    #endregion
                }
                else if (this.currentToken.Tipo == Lexico.TipoToken.TK_ID)
                {
                    #region
                    Sentencia s = SentenciaAssign_LlamaFun();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un token ;");
                    this.currentToken = lex.NextToken();
                    return s;
                    #endregion
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Sentencia SentenciaAssign_LlamaFun()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
                {
                    Variable var = new Variable(currentToken.Lexema, null);
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == Lexico.TipoToken.TK_PUNTO || currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR || currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
                    {
                        Access a = Accesories(var.accesor);
                        var.accesor = a;
                    }


                    if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN || currentToken.Tipo == Lexico.TipoToken.TK_MASIGUAL || currentToken.Tipo == Lexico.TipoToken.TK_MENORIGUAL || currentToken.Tipo == Lexico.TipoToken.TK_PORIGUAL ||
                        currentToken.Tipo == Lexico.TipoToken.TK_ENTREIGUAL)
                    {
                        S_Asignacion sAsignacion = new S_Asignacion();
                        if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
                            sAsignacion.Op = new Igual();
                        else if (currentToken.Tipo == Lexico.TipoToken.TK_MASIGUAL)
                            sAsignacion.Op = new MasIgual();
                        else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSIGUAL)
                            sAsignacion.Op = new MenosIgual();
                        else if (currentToken.Tipo == Lexico.TipoToken.TK_PORIGUAL)
                            sAsignacion.Op = new PorIgual();
                        else if (currentToken.Tipo == Lexico.TipoToken.TK_ENTREIGUAL)
                            sAsignacion.Op = new EntreIgual();

                        currentToken = lex.NextToken();
                        sAsignacion.id = var;
                        sAsignacion.Valor = Expression();
                        return sAsignacion;
                    }
                    else if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
                    {
                        Declaracion decl = new Declaracion();
                        Struct str = new Struct();
                        str.nombre = var.id;

                        Variable vVar = new Variable(currentToken.Lexema, null);
                        decl.Var = vVar;
                        decl.Tip = str;
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                            throw new Exception("Error Sintactico --Se esperaba ;");

                        return decl;
                    }

                    else
                    {
                        if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                            throw new Exception("Error Sintactico --Se esperaba ;");

                        //if (var.accesor.Last() is AccessFunc)
                        //{
                        S_LlamadaFunc sllamadafunc = new S_LlamadaFunc();
                        sllamadafunc.Var = var;
                        return sllamadafunc;
                        //}
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Sentencia CompoundStatement()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
                {
                    currentToken = lex.NextToken();
                    Sentencia S;
                    S = StatementList();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                        throw new Exception("Se esperaba una }");

                    currentToken = lex.NextToken();
                    return S;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Sentencia Else()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_ELSE)
                {
                    currentToken = lex.NextToken();
                    Sentencia S = CompoundStatement();
                    return S;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Cases Cases()
        {
            try
            {
                if (this.currentToken.Tipo == Lexico.TipoToken.TK_CASE)
                {
                    Cases cas = new Cases();
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                        throw new Exception("Se esperaba un numero o un char");

                    cas.Valor = Expression();

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                        throw new Exception("Se esperaba el simbolo :");
                    currentToken = lex.NextToken();
                    cas.S = StatementList();
                    cas.sig = Cases();
                    return cas;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Sentencia DeclaracionesC()
        {
            try
            {
                Tipo tipo = Tipo();
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba un token ID");

                Variable nombre = new Variable(currentToken.Lexema, null);
                Declaracion decl = new Declaracion();
                decl.Tip = tipo;
                decl.Var = nombre;

                currentToken = lex.NextToken();
                Sentencia s = DeclaracionesCPrima(decl);
                DeclOption(decl);
                if (s is Structs)
                    return ((Structs)s);
                else if (s is S_Functions)
                    return ((S_Functions)s);
                else
                    return decl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Sentencia DeclaracionesCPrima(Declaracion decl)
        {
            try
            {
                /*Cuando se declaran varias variables seguidas int x,t,y  */
                #region
                if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba un token ID");

                    Variable nombre = new Variable(currentToken.Lexema, null);
                    decl.Sig.Tip = decl.Tip;
                    decl.Sig.Var = nombre;
                    currentToken = lex.NextToken();
                    DeclCPrima(decl.Sig);
                    return decl;
                }
                #endregion
                /*Declaracion de arreglos */
                #region
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    Expresiones E1 = Expression();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                        throw new Exception("Se esperaba el token ] ");
                    currentToken = lex.NextToken();
                    Arreglo tipoArreglo = new Arreglo();
                    tipoArreglo.Contenido = decl.Tip;
                    tipoArreglo.Rangos.Add(E1);
                    DeclArreglo(tipoArreglo);
                    tipoArreglo.Dimensiones = tipoArreglo.Rangos.Count;
                    decl.Tip = tipoArreglo;
                    return decl;
                }
                #endregion
                /*Declaracion de funciones */
                #region
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    Declaracion listaParams = FuncionesParams();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba el token )");
                    currentToken = lex.NextToken();

                    S_Functions declFuncion = new S_Functions();
                    declFuncion.Retorno = decl.Tip;
                    declFuncion.Var = decl.Var.id;
                    declFuncion.S = CompoundStatement();
                    if (listaParams != null)
                        declFuncion.Campo = listaParams;
                    return declFuncion;
                }
                #endregion
                /*Declaracion de Structs */
                #region
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
                {
                    currentToken = lex.NextToken();
                    Declaracion strDec = StructDeclaration();
                    Declaracion tmp = StructDeclaration();
                    while (tmp != null)
                    {
                        strDec.Sig = tmp;
                        tmp = StructDeclaration();
                    }

                    Structs s = new Structs();
                    s.nombre = decl.Var.id;
                    s.campos = strDec;
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                        throw new Exception("Error sintactico se esperaba }");
                    currentToken = lex.NextToken();

                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Error sintactico se esperaba ;");
                    currentToken = lex.NextToken();

                    return s;
                }
                #endregion
                return decl;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /*Aqui se efectua la recursividad del ,id,id,id*/
        public Declaracion DeclCPrima(Declaracion decl)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba un token ID");

                    Variable nombre = new Variable(currentToken.Lexema, null);
                    decl.Sig.Tip = decl.Tip;
                    decl.Sig.Var = nombre;
                    currentToken = lex.NextToken();
                    DeclCPrima(decl.Sig);
                    return decl;
                }
                return decl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*Aqui se efectua la recursividad de [][][]*/
        public Arreglo DeclArreglo(Arreglo tipoArreglo)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    Expresiones E1 = Expression();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                        throw new Exception("Se esperaba el token ] ");
                    currentToken = lex.NextToken();
                    tipoArreglo.Rangos.Add(E1);
                    DeclArreglo(tipoArreglo);
                    return tipoArreglo;
                }
                return tipoArreglo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*Lista de Parametros*/
        public Declaracion FuncionesParams()
        {
            try
            {
                Declaracion decl = new Declaracion();
                Tipo tipo = Tipo();
                if (tipo == null)
                    return decl;
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                Variable nombre = new Variable(currentToken.Lexema, null);
                nombre.flag = 'F';
                currentToken = lex.NextToken();
                decl.Tip = tipo;
                decl.Var = nombre;
                decl.Sig = FParams();
                return decl;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Declaracion FParams()
        {
            try
            {
                Declaracion d = new Declaracion();
                if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
                {
                    currentToken = lex.NextToken();
                    //Declaracion decl = new Declaracion();
                    Tipo tipo = Tipo();
                    if (tipo == null)
                        return d;
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba el token ID");

                    Variable nombre = new Variable(currentToken.Lexema, null);
                    currentToken = lex.NextToken();
                    d.Tip = tipo;
                    d.Var = nombre;
                    d.Sig = FParams();
                    return d;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Declaracion StructDeclaration()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
                {
                    Struct t = new Struct();
                    t.nombre = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba el token Id");
                    Variable nombre = new Variable(currentToken.Lexema, null);
                    Declaracion decl = new Declaracion();
                    decl.Tip = t;
                    decl.Var = nombre;
                    currentToken = lex.NextToken();
                    DeclarsCstruct(decl);
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba el token ;");
                    currentToken = lex.NextToken();
                    return decl;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_CHAR)
                {
                    Tipo t = Tipo();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba el token Id");
                    Variable name = new Variable(currentToken.Lexema, null);
                    currentToken = lex.NextToken();

                    Declaracion decl = new Declaracion();
                    decl.Tip = t;
                    decl.Var = name;
                    DeclarsCstruct(decl);
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba el token ;");
                    currentToken = lex.NextToken();
                    return decl;

                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
               
        }

        public Declaracion DeclarsCstruct(Declaracion decl)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Se esperaba un token ID");

                    Variable nombre = new Variable(currentToken.Lexema, null);
                    decl.Sig.Var = nombre;
                    decl.Sig.Tip = decl.Tip;
                    DeclarsCstruct(decl.Sig);
                    return decl;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    Expresiones E1 = Expression();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                        throw new Exception("Se esperaba el token ] ");
                    currentToken = lex.NextToken();
                    Arreglo tipoArreglo = new Arreglo();
                    tipoArreglo.Rangos.Add(E1);
                    tipoArreglo.Contenido = decl.Tip;
                    decl.Tip = tipoArreglo;
                    DeclarsCstruct(decl.Sig);
                    return decl;
                }
                return decl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Declaracion DeclOption(Declaracion De)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_FINSENTENCIA)
                {
                    currentToken = lex.NextToken();
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
                {
                    currentToken = lex.NextToken();
                    De.Valor = Expression();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba ;");

                    currentToken = lex.NextToken();
                }
                //else
                //throw new Exception("Se esperaba ;");
                return De;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        ////////////////
        /*
         * EXPRESIONES
         * 
         * 
         * 
         * 
         *
      //////////////////////////*/

        public Expresiones Expression()
        {
            try
            {
                Expresiones E1 = Andexp();
                return Expression_prime(E1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Expression_prime(Expresiones E)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_OR)
                {
                    currentToken = lex.NextToken();
                    Expresiones E1 = Expression();
                    Or eor = new Or(E, Expression_prime(E1));
                    return eor;
                }
                return E;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Andexp()
        {
            try
            {
                Expresiones E1 = Relexp();
                return Andexp_prime(E1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Andexp_prime(Expresiones E1)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_AND)
                {
                    currentToken = lex.NextToken();
                    Expresiones E = Addexp();
                    And eAnd = new And(E1, Andexp_prime(E));
                    return eAnd;
                }
                return E1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Relexp()
        {
            try
            {
                Expresiones E = Addexp();
                return Relexp_prime(E);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Relexp_prime(Expresiones E)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_IGUALDAD)
                {
                    currentToken = lex.NextToken();
                    Equal equal = new Equal(E, Addexp());
                    return equal;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_DISTINTO)
                {
                    currentToken = lex.NextToken();
                    Distinto dist = new Distinto(E, Addexp());
                    return dist;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORQUE)
                {
                    currentToken = lex.NextToken();
                    Expresiones x = Addexp();
                    MayorQue mayorQ = new MayorQue(E, x);
                    return mayorQ; ;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORIGUAL)
                {
                    currentToken = lex.NextToken();
                    MayorIgual mayorI = new MayorIgual(E, Addexp());
                    return mayorI;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORQUE)
                {
                    currentToken = lex.NextToken();
                    MenorQue menorQ = new MenorQue(E, Addexp());
                    return menorQ;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORIGUAL)
                {
                    currentToken = lex.NextToken();
                    MenorIgual menorI = new MenorIgual(E, Addexp());
                    return menorI;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MASMAS)
                {
                    ExpMasMas mas = new ExpMasMas();
                    if (E is Variable)
                    {
                        Variable v = ((Variable)E);
                        mas.ID = v;
                        currentToken = lex.NextToken();
                    }
                    return mas;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSMENOS)
                {
                    ExpMenosMenos menos = new ExpMenosMenos();
                    if (E is Variable)
                    {
                        Variable v = ((Variable)E);
                        menos.ID = v;
                        currentToken = lex.NextToken();
                    }
                    return menos;
                }
                return E;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Expresiones Addexp()
        {
            try
            {
                Expresiones Ex = Multexp();
                return Addexp_prime(Ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Expresiones Addexp_prime(Expresiones E)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_SUMA)
                {
                    currentToken = lex.NextToken();
                    Suma sum = new Suma(E, Addexp_prime(Addexp()));
                    return sum;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_RESTA)
                {
                    currentToken = lex.NextToken();
                    Resta rest = new Resta(E, Addexp_prime(Addexp()));
                    return rest;
                }
                return E;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Multexp()
        {
            try
            {
                Expresiones Ex = Parexp();
                return Multexp_prime(Ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Multexp_prime(Expresiones E)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_MULT)
                {
                    currentToken = lex.NextToken();
                    Multiplicacion mult = new Multiplicacion(E, Multexp_prime(Multexp()));
                    return mult;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_DIV)
                {
                    currentToken = lex.NextToken();
                    Division div = new Division(E, Multexp_prime(Multexp()));
                    return div;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_MOD)
                {
                    currentToken = lex.NextToken();
                    Mod mod = new Mod(E, Multexp_prime(Multexp()));
                    return mod;
                }
                return E;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Expresiones Parexp()
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_INT_LIT)
                {
                    LiteralEntero lit = new LiteralEntero(Convert.ToInt32(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_FLOAT_LIT)
                {
                    LiteralFlotante lit = new LiteralFlotante(float.Parse(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR_LIT)
                {
                    LitChar lit = new LitChar(currentToken.Lexema);
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_STRING_LIT)
                {
                    LitString lit = new LitString(currentToken.Lexema);
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_TRUE)
                {
                    LitBool lit = new LitBool(true);
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
                {
                    LitBool lit = new LitBool(false);
                    currentToken = lex.NextToken();
                    return lit;
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
                {
                    ExprFuncion V = new ExprFuncion();
                    V.ID = new Variable(currentToken.Lexema, null);
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == Lexico.TipoToken.TK_PUNTO || currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR || currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                    {
                        try
                        {
                            V.ID.accesor = Accesories(V.ID.accesor);
                            Access tmp = ((Access)V.ID.accesor);
                            if (tmp.Next != null)
                            {
                                tmp = tmp.Last();
                            }
                            if (V.ID.accesor != null && tmp is AccessFunc)
                            {
                                AccessFunc v = ((AccessFunc)tmp);
                                V.VarList = v.Variables;
                                return V;
                            }
                            else
                                return V.ID;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        return V.ID;
                    }
                }
                return null;

                /*
                ExprFuncion V = new ExprFuncion();
                V.ID = new Variable(currentToken.Lexema, null);
                currentToken = lex.NextToken();
                V.ID.accesor = Accesories(V.ID.accesor);
                if (V.ID.accesor != null)
                    return V;
                else
                    return V.ID;	
                */


                // return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Access Accesories(Access List)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_PUNTO)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                        throw new Exception("Error Sintactico  -- Se esperaba un ID");
                    Lexico.Token tmp = currentToken;
                    currentToken=lex.NextToken();
                    if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                    {
                        AccessArreglo accAr = new AccessArreglo();
                        accAr.Cont = ArrayDim(accAr.Cont);
                        accAr.nombre = tmp.Lexema;
                        List = accAr;
                        List.Next = Accesories(List.Next);
                    }
                    else
                    {

                        AccessMiembro accM = new AccessMiembro();
                        accM.Id = tmp.Lexema;
                        List = accM;
                        //currentToken = lex.NextToken();
                        List.Next = Accesories(List.Next);
                    }
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                {
                    AccessArreglo accAr = new AccessArreglo();
                    accAr.Cont = ArrayDim(accAr.Cont);
                    List = accAr;
                    List.Next = Accesories(List.Next);
                }
                else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    AccessFunc accFun = new AccessFunc();
                    ListaExpre listaExpre = new ListaExpre();
                    Expresiones e = Expression();
                    if (e != null)
                        listaExpre.Ex.Add(e);
                    if (listaExpre.Ex.Count > 0)
                        accFun.Variables = ExpreList(listaExpre);
                    List = accFun;
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Error Sintactico -- Se esperaba simbolo )");
                    currentToken = lex.NextToken();
                    return List;
                }
                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ListaExpre ExpreList(ListaExpre E)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
                {
                    currentToken = lex.NextToken();
                    ((ListaExpre)E).Ex.Add(Expression());
                    return ExpreList(E);
                }
                else
                {
                    return ((ListaExpre)E);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ArrayList ArrayDim(ArrayList ListE)
        {
            try
            {
                if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    ListE.Add(Expression());
                    if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                        throw new Exception("Error Sintactico -- Se esperaba simbolo ]");
                    currentToken = lex.NextToken();
                    ArrayDim(ListE);
                }
                return ListE;
            }
            catch (Exception ex)
             {
                throw ex;
            }
        }

    







    }


}
