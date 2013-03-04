using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;
using Project_Compiladores1.Arbol;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser:Parser
    {
        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }

        public Sentencia parse()
        {
            Sentencia ret=SL();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
            return ret;
        }

        Sentencia SL()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_ID:
                case TipoToken.TK_IF:
                case TipoToken.TK_WHILE:
                case TipoToken.TK_FOR:
                case TipoToken.TK_REPEAT:
                case TipoToken.TK_CASE:
                case TipoToken.TK_BREAK:
                case TipoToken.TK_CONTINUE:
                case TipoToken.TK_EXIT:
                case TipoToken.TK_PRINT:
                case TipoToken.TK_READ:
                case TipoToken.TK_VAR:
                case TipoToken.TK_VOID:
                case TipoToken.TK_FUNCTION:
                case TipoToken.TK_TYPE:
                    
                    Sentencia raiz =S();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        raiz.sig=SL();
                        return raiz;
                    }
                    else throw new Exception("Se esperaba ;");
                default:
                    return null;
            }
        }

        Sentencia S()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                Variable par = new Variable();
                par.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                return S_prime(par);
            }
            else if (currentToken.Tipo == TipoToken.TK_IF)
            {
                currentToken = lex.NextToken();
                S_If ret = new S_If();
                ret.Condicion = expr();
                if (currentToken.Tipo == TipoToken.TK_THEN)
                {
                    currentToken = lex.NextToken();
                    return IFP(ret);
                }
                else throw new Exception("Se esperaba then");
            }
            else if (currentToken.Tipo == TipoToken.TK_WHILE)
            {
                currentToken = lex.NextToken();
                S_While ret = new S_While();
                ret.Condicion = expr();
                if (currentToken.Tipo == TipoToken.TK_DO)
                {
                    currentToken = lex.NextToken();
                    ret.S = CS();
                    return ret;
                }
                else throw new Exception("Se esperaba do");
            }
            else if (currentToken.Tipo == TipoToken.TK_FOR)
            {
                currentToken = lex.NextToken();
                S_For ret = new S_For();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    ret.Var.id = currentToken.Lexema;
                    ret.Tip = new Entero();
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_ASSIGN)
                    {
                        currentToken = lex.NextToken();
                        ret.Inicio = expr();
                        if (currentToken.Tipo == TipoToken.TK_TO)
                        {
                            currentToken = lex.NextToken();
                            ret.Iteracion = expr();
                            if (currentToken.Tipo == TipoToken.TK_DO)
                            {
                                currentToken = lex.NextToken();
                                ret.S = CS();
                                ret.Condicion = new MenorQue(ret.Inicio, ret.Iteracion);
                                return ret;
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
                S_Do ret = new S_Do();
                currentToken = lex.NextToken();
                ret.S = SL();
                if (currentToken.Tipo == TipoToken.TK_UNTIL)
                {
                    ret.Condicion = expr();
                    return ret;
                }
                else throw new Exception("Se esperaba until");
            }
            else if (currentToken.Tipo == TipoToken.TK_CASE)
            {
                S_Switch ret = new S_Switch();
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    ret.Var = new Variable();
                    ((Variable)ret.Var).id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_OF)
                    {
                        currentToken = lex.NextToken();
                        ret.Casos = CL();
                        if (currentToken.Tipo == TipoToken.TK_END)
                        {
                            currentToken = lex.NextToken();
                            return ret;
                        }
                        else throw new Exception("Se esperaba end");
                    }
                    else throw new Exception("Se esperaba of");
                }
                else throw new Exception("Se esperaba id");
            }
            else if (currentToken.Tipo == TipoToken.TK_BREAK)
            {
                currentToken = lex.NextToken();
                return new S_Break();
            }
            else if (currentToken.Tipo == TipoToken.TK_CONTINUE)
            {
                currentToken = lex.NextToken();
                return new S_Continue();
            }
            else if (currentToken.Tipo == TipoToken.TK_EXIT)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    S_Return ret = new S_Return();
                    ret.Expr = expr();
                    if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                    {
                        currentToken = lex.NextToken();
                        return ret;
                    }
                    else throw new Exception("Se esperaba )");
                }
                else throw new Exception("Se esperaba (");
            }
            else if (currentToken.Tipo == TipoToken.TK_PRINT)
            {
                currentToken = lex.NextToken();
                S_Print ret = new S_Print();
                ret.Expr = expr();
                return ret;
            }
            else if (currentToken.Tipo == TipoToken.TK_READ)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    S_Read ret = new S_Read();
                    ret.var = new Variable();
                    ret.var.id = currentToken.Lexema;
                    return ret;
                }
                else throw new Exception("Se espeeraba identificador.");
            }
            else if (currentToken.Tipo == TipoToken.TK_TYPE)
                return TD();
            else if (currentToken.Tipo == TipoToken.TK_VAR)
                return VD();
            else if (currentToken.Tipo == TipoToken.TK_FUNCTION || currentToken.Tipo == TipoToken.TK_VOID)
                return FD();
            else throw new Exception("Sentencia no reconocida");
        }

        Structs TD()
        {
            currentToken = lex.NextToken();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                Structs ret = new Structs();
                ret.nombre = new Variable();
                ret.nombre.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_IGUALDAD)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_RECORD)
                    {
                        currentToken = lex.NextToken();
                        ret.c=FL();
                        if (currentToken.Tipo == TipoToken.TK_END)
                        {
                            currentToken = lex.NextToken();
                            return ret;
                        }
                        else throw new Exception("Se esperaba end");
                    }
                    else throw new Exception("Se esperaba record");
                }
                else throw new Exception("Se esperaba =");
            }
            else throw new Exception("Se esperaba id");
        }

        Campos FL()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                List<Variable> idl = IDL();
                Campos tmp, ret = new Campos();
                tmp = ret;
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    currentToken = lex.NextToken();
                    Tipo tip = STYPE();
                    ret.Tip = tip;
                    ret.Var = idl[0];
                    idl.RemoveAt(0);
                    foreach (Variable id in idl)
                    {
                        tmp.Sig = new Campos();
                        tmp.Sig.Tip = tip;
                        tmp.Sig.Var = id;
                        tmp = tmp.Sig;
                    }
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        tmp.Sig = FL();
                        return ret;
                    }
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba :");
            }
            else return null;
        }

        Campos PL()
        {
            List<Variable> idl = IDL();
            if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
            {
                currentToken = lex.NextToken();
                Tipo t = STYPE();
                Campos tmp, ret = new Campos();
                ret.Tip = t;
                ret.Var = idl[0];
                idl.RemoveAt(0);
                tmp = ret;
                foreach (Variable id in idl)
                {
                    tmp.Sig = new Campos();
                    tmp = tmp.Sig;
                    tmp.Var = id;
                    tmp.Tip = t;
                }
                tmp.Sig=PLprime();
                return ret;
            }
            else throw new Exception("Se esperaba \":\".");
        }

        Campos PLprime()
        {
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                List<Variable> idl = IDL();
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    currentToken = lex.NextToken();
                    Tipo t = STYPE();
                    Campos tmp, ret = new Campos();
                    ret.Tip = t;
                    ret.Var = idl[0];
                    idl.RemoveAt(0);
                    tmp = ret;
                    foreach (Variable id in idl)
                    {
                        tmp.Sig = new Campos();
                        tmp = tmp.Sig;
                        tmp.Var = id;
                        tmp.Tip = t;
                    }
                    tmp.Sig = PLprime();
                    return ret;
                }
                else throw new Exception("Se esperaba \":\".");
            }
            else return null;
        }

        List<Variable> IDL()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                List<Variable> ret = new List<Variable>();
                Variable first = new Variable();
                first.id = currentToken.Lexema;
                ret.Add(first);
                currentToken = lex.NextToken();
                return IDLprime(ret);
            }
            else throw new Exception("Se esperba id");
        }

        List<Variable> IDLprime(List<Variable> par)
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    Variable nueva = new Variable();
                    nueva.id = currentToken.Lexema;
                    par.Add(nueva);
                    currentToken = lex.NextToken();
                    return IDLprime(par);
                }
                else throw new Exception("Se esperaba id");
            }
            else return par;
        }

        Campos TYPE()
        {
            if (currentToken.Tipo == TipoToken.TK_ARRAY)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENCOR)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_INT_LIT)
                    {
                        int ini = int.Parse(currentToken.Lexema);
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo == TipoToken.TK_PUNTO)
                        {
                            currentToken = lex.NextToken();
                            if (currentToken.Tipo == TipoToken.TK_PUNTO)
                            {
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo == TipoToken.TK_INT_LIT)
                                {
                                    int fin = int.Parse(currentToken.Lexema);
                                    currentToken = lex.NextToken();
                                    if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                                    {
                                        currentToken = lex.NextToken();
                                        if (currentToken.Tipo == TipoToken.TK_OF)
                                        {
                                            currentToken = lex.NextToken();
                                            Campos ret = new Campos();
                                            ret.Dimension = 1;
                                            ret.Tip = STYPE();
                                            LiteralEntero tam = new LiteralEntero(fin - ini);
                                            ret.dim.Add(tam);
                                            return ret;
                                        }
                                        else throw new Exception("Se esperaba of");
                                    }
                                    else throw new Exception("Se esperaba ]");
                                }
                                else throw new Exception("Se esperaba un numero");
                            }
                            else throw new Exception("Se esperaba ..");
                        }
                        else throw new Exception("Se esperaba ..");
                    }
                    else throw new Exception("Se esperaba un numero");
                }
                else throw new Exception("Se esperaba [");
            }
            else throw new Exception("se esperaba algo");
        }//falta

        Tipo STYPE()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT:
                    currentToken = lex.NextToken();
                    return new Entero();
                case TipoToken.TK_FLOAT:
                    currentToken = lex.NextToken();
                    return new Flotante();
                case TipoToken.TK_CHAR:
                    currentToken = lex.NextToken();
                    return new Caracter();
                case TipoToken.TK_STRING:
                    currentToken = lex.NextToken();
                    return new Cadena();
                case TipoToken.TK_BOOL:
                    currentToken = lex.NextToken();
                    return new Booleano();
                default:
                    throw new Exception("Se esperaba un tipo primitivo");
            }
        }

        Campos VD()
        {
            if (currentToken.Tipo == TipoToken.TK_VAR)
            {
                currentToken = lex.NextToken();
                List<Variable> idl = IDL();
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    Campos tmp, ret = new Campos();
                    tmp = ret;
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != TipoToken.TK_ARRAY)
                    {
                        Tipo t = STYPE();
                        ret.Tip = t;
                        ret.Var = idl[0];
                        idl.RemoveAt(0);
                        foreach (Variable id in idl)
                        {
                            tmp.Sig = new Campos();
                            tmp = tmp.Sig;
                            tmp.Tip = t;
                            tmp.Var = id;
                        }
                    }
                    else
                    {
                        Campos arreglo = TYPE();
                        ret.Tip = arreglo.Tip;
                        ret.dim = arreglo.dim;
                        ret.Dimension = arreglo.Dimension;
                        ret.Var = idl[0];
                        idl.RemoveAt(0);
                        foreach (Variable id in idl)
                        {
                            tmp.Sig = new Campos();
                            tmp = tmp.Sig;
                            tmp.Tip = arreglo.Tip;
                            tmp.dim = arreglo.dim;
                            ret.Dimension = arreglo.Dimension;
                            tmp.Var = id;
                        }
                    }
                    return ret;
                }
                else throw new Exception("Se esperaba :");
            }
            else throw new Exception("Se esperaba var.");
        }

        S_Functions FD()
        {
            S_Functions ret=HEAD();
            Campos f, l;
            f = l = null;
            if (currentToken.Tipo == TipoToken.TK_VAR)
            {
                f = l = VD();
            }
            while (currentToken.Tipo == TipoToken.TK_VAR)
            {
                l.Sig=VD();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    currentToken = lex.NextToken();
                else
                    throw new Exception("se esperaba ;");
                l = l.Sig;
            }
            if (f == null)
            {
                ret.S = CS();
            }
            else
            {
                l.sig = CS();
                ret.S = f;
            }
            return ret;
        }

        S_Functions HEAD()
        {
            if (currentToken.Tipo == TipoToken.TK_FUNCTION)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    S_Functions ret = new S_Functions();
                    ret.var = new Variable();
                    ret.var.id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    ret.Campo=ARGS();
                    if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                    {
                        currentToken = lex.NextToken();
                        ret.Retorno = STYPE();
                        if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        {
                            currentToken = lex.NextToken();
                            return ret;
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
                    S_Functions ret = new S_Functions();
                    ret.var = new Variable();
                    ret.var.id = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    ret.Campo = ARGS();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        return ret;
                    }
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba id");
            }
            else throw new Exception("Se esperaba declaracion de subprograma");
        }

        Campos ARGS()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Campos ret = PL();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return ret;
                }
                else throw new Exception("Se esperaba )");
            }
            else throw new Exception("Se esperaba (");
        }

        Cases CL()
        {
            Cases ret = new Cases();
            ret.Valor=LIT();
            if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
            {
                currentToken = lex.NextToken();
                ret.S = CS();
                ret.sig = CL();
                return ret;
            }
            else throw new Exception("Se esperaba \":\".");
        }

        Expresiones LIT()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT_LIT:
                    LiteralEntero asd = new LiteralEntero(int.Parse(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return asd;
                case TipoToken.TK_FLOAT_LIT:
                    LiteralFlotante flo = new LiteralFlotante(float.Parse(currentToken.Lexema));
                    currentToken = lex.NextToken();
                    return flo;
                case TipoToken.TK_CHAR_LIT:
                    string car = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    return new LitChar(car);
                case TipoToken.TK_STRING_LIT:
                    string cad = currentToken.Lexema;
                    currentToken = lex.NextToken();
                    return new LitString(cad);
                case TipoToken.TK_TRUE:
                    currentToken = lex.NextToken();
                    return new LitBool(true);
                case TipoToken.TK_FALSE:
                    currentToken = lex.NextToken();
                    return new LitBool(false);
                default:
                    throw new Exception("Se esperaba una literal");
            }
        }

        Sentencia S_prime(Variable par)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                S_LlamadaFunc ret = new S_LlamadaFunc();
                ret.Var = par;
                ret.VarList = exprl();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return ret;
                }
                else throw new Exception("Se esperaba )");
            }
            else if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
                S_Asignacion ret = new S_Asignacion();
                ret.id = par;
                ret.Valor = expr();
                return ret;
            }
            else return S_prime2(par);
        }

        Sentencia S_prime2(Variable par)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                par.access.Add(expr());
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    currentToken = lex.NextToken();
                    return S_prime(par);
                }
                else throw new Exception("Se esperaba ID.");
            }
            else if (currentToken.Tipo == TipoToken.TK_PUNTO)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    Variable v = new Variable();
                    v.id = currentToken.Lexema;
                    par.access.Add(v);
                    currentToken = lex.NextToken();
                    return S_prime(par);
                }
                else throw new Exception("Se esperaba ID.");
            }
            else throw new Exception("solo asignacion, llamada y declaracion pueden ser sentencias.");
        }

        ListaExpre exprl()
        {
            ListaExpre arg = new ListaExpre();
            arg.Ex.Add(expr());
            return exprlPrime(arg);
        }

        ListaExpre exprlPrime(ListaExpre par)
        {
            if (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                par.Ex.Add(expr());
                return exprlPrime(par);
            }
            else return par;
        }

        S_If IFP(S_If par)
        {
            par.Cierto=CS();
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                par.Falso=ELSE();
            }
            else throw new Exception("se esperaba ;");
            return par;
        }

        Sentencia CS()
        {
            if (currentToken.Tipo == TipoToken.TK_BEGIN)
            {
                currentToken = lex.NextToken();
                Sentencia ret=SL();
                if (currentToken.Tipo == TipoToken.TK_END)
                {
                    currentToken = lex.NextToken();
                    return ret;
                }
                else throw new Exception("se esperaba end");
            }
            else throw new Exception("Se esperaba begin");
        }

        Sentencia ELSE()
        {
            if (currentToken.Tipo == TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                return CS();
            }
            return null;//else do nothing and return :-)
        }

        Expresiones expr()
        {
            return exPrime(andexp());
        }

        Expresiones exPrime(Expresiones par)
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                return exPrime(new Or(par, andexp()));
            }
            else return par;
        }

        Expresiones andexp()
        {
            return andexPrime(relexp());
        }

        Expresiones andexPrime(Expresiones par)
        {
            if (currentToken.Tipo == TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                return andexPrime(new And(par, relexp()));
            }
            else return par;
        }

        Expresiones relexp()
        {
            return relexprime(addexp());
        }

        Expresiones relexprime(Expresiones par)
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_IGUALDAD:
                    currentToken = lex.NextToken();
                    return new Equal(par, addexp());
                case TipoToken.TK_DISTINTO:
                    currentToken = lex.NextToken();
                    return new Distinto(par, addexp());
                case TipoToken.TK_MAYORIGUAL:
                    currentToken = lex.NextToken();
                    return new MayorIgual(par, addexp());
                case TipoToken.TK_MAYORQUE:
                    currentToken = lex.NextToken();
                    return new MayorQue(par, addexp());
                case TipoToken.TK_MENORIGUAL:
                    currentToken = lex.NextToken();
                    return new MenorIgual(par, addexp());
                case TipoToken.TK_MENORQUE:
                    currentToken = lex.NextToken();
                    return new MenorQue(par, addexp());
                default:
                    return par;
            }
        }

        Expresiones addexp()
        {
            return addexprime(multexp());
        }

        Expresiones addexprime(Expresiones par)
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_SUMA:
                    currentToken = lex.NextToken();
                    return new Suma(par, addexprime(multexp()));
                case TipoToken.TK_RESTA:
                    currentToken = lex.NextToken();
                    return new Resta(par, addexprime(multexp()));
                default:
                    return par;
            }
        }

        Expresiones multexp()
        {
            return multexprime(parexp());
        }

        Expresiones multexprime(Expresiones par)
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_MULT:
                    currentToken = lex.NextToken();
                    return new Multiplicacion(par, multexprime(parexp()));
                case TipoToken.TK_DIVISION:
                    currentToken = lex.NextToken();
                    return new Division(par, multexprime(parexp()));
                case TipoToken.TK_MOD:
                    currentToken = lex.NextToken();
                    return new Mod(par, multexprime(parexp()));
                case TipoToken.TK_DIV:
                    currentToken = lex.NextToken();
                    return new Division(par, multexprime(parexp()));
                default:
                    return par;
            }
        }

        Expresiones parexp()
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Expresiones tmp = expr();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return tmp;
                }
                else throw new Exception("Se esperaba )");
            }
            else if (currentToken.Tipo == TipoToken.TK_NOT)
            {
                currentToken = lex.NextToken();
                return new Not(expr());
            }
            else if (currentToken.Tipo == TipoToken.TK_ID)
            {
                Variable id = new Variable();
                id.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                return e_prime(id);
            }
            else return LIT();
        }

        Expresiones e_prime(Variable par)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                ExprFuncion ret = new ExprFuncion();
                ret.ID = par;
                ret.VarList = exprl();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return ret;
                }
                else throw new Exception("Se esperaba )");
            }
            else if (currentToken.Tipo == TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                par.access.Add(expr());
                if (currentToken.Tipo == TipoToken.TK_CLOSECOR)
                {
                    return e_prime(par);
                }
                else throw new Exception("Se esperaba ]");
            }
            else if (currentToken.Tipo == TipoToken.TK_PUNTO)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    Variable v = new Variable();
                    v.id = currentToken.Lexema;
                    par.access.Add(v);
                    return e_prime(v);
                }
                else throw new Exception("Se esperaba ID.");
            }
            else return par;
        }
    }
}
