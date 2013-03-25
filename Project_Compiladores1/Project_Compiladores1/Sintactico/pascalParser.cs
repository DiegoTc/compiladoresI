using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;
using Project_Compiladores1.Arbol;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser : Parser
    {

        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }

        public Sentencia parse()
        {
            return StatementList();
        }

        Sentencia StatementList()
        {
            Sentencia ret = Statement();
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                ret.sig = StatementList();
            }
            return ret;
        }

        Sentencia Statement()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_IF:
                    currentToken = lex.NextToken();
                    return parseIf();

                case TipoToken.TK_FOR:
                    currentToken = lex.NextToken();
                    return parseFor();

                case TipoToken.TK_WHILE:
                    currentToken = lex.NextToken();
                    return parseWhile();

                case TipoToken.TK_REPEAT:
                    currentToken = lex.NextToken();
                    return parseDo();

                #region variable
                case TipoToken.TK_VAR:
                {
                    currentToken = lex.NextToken();
                    Declaracion ret = VariableDeclarationList();
                    if (currentToken.Tipo != TipoToken.TK_END)
                        throw new Exception("Se esperaba end.");
                    else
                    {
                        currentToken = lex.NextToken();
                        return ret;
                    }
                }
                #endregion

                case TipoToken.TK_ID:
                    return parseAssignOrCall();

                #region read/print
                case TipoToken.TK_PRINT:
                {
                    currentToken = lex.NextToken();
                    S_Print ret = new S_Print();
                    ret.Expr = Expr();
                    return ret;
                }
                case TipoToken.TK_READ:
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != TipoToken.TK_ID)
                        throw new Exception("Se esperaba identificador.");
                    else
                    {
                        S_Read ret = new S_Read();
                        ret.var = new Variable(currentToken.Lexema, AccessList());
                        return ret;
                    }
                }
                #endregion

                #region Procedure
                case TipoToken.TK_VOID:
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != TipoToken.TK_ID)
                        throw new Exception("Se esperaba ID");
                    else
                    {
                        S_Functions ret = new S_Functions();
                        ret.Retorno = new Voids();
                        ret.Var = currentToken.Lexema;
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_OPENPAR)
                            throw new Exception("Se esperaba \"(\"");
                        else
                        {
                            currentToken = lex.NextToken();
                            ret.Campo = VariableDeclarationList();
                            if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                                throw new Exception("Se esperaba\")\"");
                            else
                            {
                                currentToken = lex.NextToken();
                                ret.S = CodeBlock();
                                return ret;
                            }
                        }
                    }
                }
                #endregion

                #region Function
                case TipoToken.TK_FUNCTION:
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo != TipoToken.TK_ID)
                        throw new Exception("Se esperaba ID");
                    else
                    {
                        S_Functions ret = new S_Functions();
                        ret.Var = currentToken.Lexema;
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_OPENPAR)
                            throw new Exception("Se esperaba \"(\"");
                        else
                        {
                            currentToken = lex.NextToken();
                            ret.Campo = VariableDeclarationList();
                            if (currentToken.Tipo != TipoToken.TK_CLOSEPAR)
                                throw new Exception("Se esperaba\")\"");
                            else
                            {
                                currentToken = lex.NextToken();
                                if (currentToken.Tipo != TipoToken.TK_DOSPUNTOS)
                                    throw new Exception("Se esperaba\":\"");
                                else
                                {
                                    currentToken = lex.NextToken();
                                    ret.Retorno = ParseType();
                                    ret.S = CodeBlock();
                                    return ret;
                                }
                            }
                        }
                    }
                }
                #endregion

                case TipoToken.TK_TYPE:
                {
                    currentToken = lex.NextToken();
                    //TODO: Pensar que hacer acá
                }

                #region break/continue/return
                case TipoToken.TK_BREAK:
                    currentToken = lex.NextToken();
                    return new S_Break();

                case TipoToken.TK_CONTINUE:
                    currentToken = lex.NextToken();
                    return new S_Continue();

                case TipoToken.TK_RETURN:
                {
                    currentToken = lex.NextToken();
                    S_Return ret = new S_Return();
                    ret.Expr = Expr();
                    return ret;
                }
                #endregion
                default:
                throw new Exception("Sentencia no reconocida.");
            }
        }

        Sentencia CodeBlock()
        {
            if(currentToken.Tipo!=TipoToken.TK_BEGIN)
                throw new Exception("Se esperaba Begin");
            else
            {
                currentToken=lex.NextToken();
                Sentencia ret=CompoundStatementList();
                if(currentToken.Tipo!=TipoToken.TK_END)
                    throw new Exception("Se esperaba end");
                else
                {
                    currentToken=lex.NextToken();
                    return ret;
                }
            }
        }

        Sentencia CompoundStatementList()
        {
            Sentencia ret = CompoundStatement();
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                ret.sig = CompoundStatementList();
            }
            return ret;
        }

        Sentencia CompoundStatement()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_IF:
                    currentToken = lex.NextToken();
                    return parseIf();

                case TipoToken.TK_FOR:
                    currentToken = lex.NextToken();
                    return parseFor();

                case TipoToken.TK_WHILE:
                    currentToken = lex.NextToken();
                    return parseWhile();

                case TipoToken.TK_REPEAT:
                    currentToken = lex.NextToken();
                    return parseDo();

                case TipoToken.TK_ID:
                    return parseAssignOrCall();

                #region read/print/break/continue/return
                case TipoToken.TK_PRINT:
                    {
                        currentToken = lex.NextToken();
                        S_Print ret = new S_Print();
                        ret.Expr = Expr();
                        return ret;
                    }
                case TipoToken.TK_READ:
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_ID)
                            throw new Exception("Se esperaba identificador.");
                        else
                        {
                            S_Read ret = new S_Read();
                            ret.var = new Variable(currentToken.Lexema, AccessList());
                            return ret;
                        }
                    }

                case TipoToken.TK_BREAK:
                    currentToken = lex.NextToken();
                    return new S_Break();

                case TipoToken.TK_CONTINUE:
                    currentToken = lex.NextToken();
                    return new S_Continue();

                case TipoToken.TK_RETURN:
                    {
                        currentToken = lex.NextToken();
                        S_Return ret = new S_Return();
                        ret.Expr = Expr();
                        return ret;
                    }
                #endregion
                default:
                    throw new Exception("Sentencia no reconocida.");
            }
        }

        S_If parseIf()
        {
            S_If ret = new S_If();
            ret.Condicion = Expr();
            if (currentToken.Tipo != TipoToken.TK_THEN)
                throw new Exception("Se esperaba then");
            else
            {
                currentToken = lex.NextToken();
                ret.Cierto = CodeBlock();
                if (currentToken.Tipo == TipoToken.TK_ELSE)
                {
                    currentToken = lex.NextToken();
                    ret.Falso = CodeBlock();
                }
                return ret;
            }
        }

        S_For parseFor()
        {
            S_For ret = new S_For();
            ret.Tip = new Entero();
            if (currentToken.Tipo != TipoToken.TK_ID)
                throw new Exception("Se esperaba identificador.");
            else
            {
                ret.Var = new Variable(currentToken.Lexema, null);
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_ASSIGN)
                    throw new Exception("Se esperaba el operador de asignacion");
                else
                {
                    currentToken = lex.NextToken();
                    ret.Inicio = Expr();
                    switch (currentToken.Tipo)
                    {
                        case TipoToken.TK_TO:
                            currentToken = lex.NextToken();
                            MenorQue cond = new MenorQue(ret.Var, Expr());
                            ExpMasMas it = new ExpMasMas();
                            it.ID = ret.Var;
                            ret.Condicion = cond;
                            ret.Iteracion = it;
                            break;
                        case TipoToken.TK_DOWNTO:
                            currentToken = lex.NextToken();
                            MayorQue cond1 = new MayorQue(ret.Var, Expr());
                            ExpMenosMenos it1 = new ExpMenosMenos();
                            it1.ID = ret.Var;
                            ret.Condicion = cond1;
                            ret.Iteracion = it1;
                            break;
                        default:
                            throw new Exception("Se esperaba to/Downto");
                    }
                    if (currentToken.Tipo != TipoToken.TK_DO)
                        throw new Exception("Se esperaba do.");
                    else
                    {
                        currentToken = lex.NextToken();
                        ret.S = CodeBlock();
                        return ret;
                    }
                }
            }
        }

        S_While parseWhile()
        {
            S_While ret = new S_While();
            ret.Condicion = Expr();
            if (currentToken.Tipo != TipoToken.TK_DO)
                throw new Exception("Se esperaba do.");
            else
            {
                currentToken = lex.NextToken();
                ret.S = CodeBlock();
                return ret;
            }
        }

        S_Do parseDo()
        {
            S_Do ret = new S_Do();
            ret.S = CodeBlock();
            if (currentToken.Tipo != TipoToken.TK_UNTIL)
                throw new Exception("Se esperaba until.");
            else
            {
                currentToken = lex.NextToken();
                ret.Condicion = Expr();
                return ret;
            }
        }

        Sentencia parseAssignOrCall()//Asume que no se ha consumido el ID
        {
            string tmp = currentToken.Lexema;
            currentToken = lex.NextToken();
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_PUNTO:
                case TipoToken.TK_OPENCOR:
                    {
                        S_Asignacion ret = new S_Asignacion();
                        ret.id = new Variable(tmp, AccessList());
                        if (currentToken.Tipo != TipoToken.TK_ASSIGN)
                            throw new Exception("Se esperaba Asignacion.");
                        else
                        {
                            currentToken = lex.NextToken();
                            ret.Valor = Expr();
                            return ret;
                        }
                    }
                case TipoToken.TK_OPENPAR:
                    {
                        currentToken = lex.NextToken();
                        S_LlamadaFunc ret = new S_LlamadaFunc();
                        ret.Var.id = tmp;
                        //TODO: ver que pedo con los parametros
                        //QUE PEDO CON LOS PARAMETROS??? ffffffffffffffuuuuuuuuuuuuuuuuuuuuuuuuu!!!!!!!!!!!!!!!!!!!
                    }
            }
        }

        Declaracion VariableDeclarationList()
        {
            Declaracion ret = VariableDeclaration();
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                ret.Sig = VariableDeclarationList();
            }
            return ret;
        }

        Declaracion VariableDeclaration()
        {
            if (currentToken.Tipo != TipoToken.TK_ID)
                return null;
            else
            {
                Declaracion ret = new Declaracion();
                ret.Var = new Variable(currentToken.Lexema, null);
                currentToken = lex.NextToken();
                if (currentToken.Tipo != TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba :.");
                else
                {
                    currentToken = lex.NextToken();
                    ret.Tip = ParseType();
                    return ret;
                }
            }
        }

        Access AccessList()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_PUNTO:
                    {
                        currentToken = lex.NextToken();
                        if (currentToken.Tipo != TipoToken.TK_ID)
                            throw new Exception("Se esperaba identificador.");
                        else
                        {
                            AccessMiembro ret = new AccessMiembro();
                            ret.Id = currentToken.Lexema;
                            currentToken = lex.NextToken();
                            ret.Next = AccessList();
                            return ret;
                        }
                    }

                case TipoToken.TK_OPENCOR:
                    {
                        currentToken = lex.NextToken();
                        AccessArreglo ret = new AccessArreglo(ExprList());
                        if (currentToken.Tipo != TipoToken.TK_CLOSECOR)
                            throw new Exception("Se esperaba ].");
                        else
                        {
                            currentToken = lex.NextToken();
                            ret.Next = AccessList();
                            return ret;
                        }
                    }

                default: return null;
            }
        }

        ArrayList ExprList()
        {
            ArrayList ret = new ArrayList();
            ret.Add(Expr());
            while (currentToken.Tipo == TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                ret.Add(Expr());
            }
            return ret;
        }

        Tipo ParseType()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT:
                    {
                        currentToken = lex.NextToken();
                        return new Entero();
                    }
                case TipoToken.TK_BOOL:
                    {
                        currentToken = lex.NextToken();
                        return new Booleano();
                    }
                case TipoToken.TK_CHAR:
                    {
                        currentToken = lex.NextToken();
                        return new Caracter();
                    }
                case TipoToken.TK_ID:
                    {
                        //TODO: AVeriguar que hacer acá
                        //No se que hacer acá!!! :(
                    }
                case TipoToken.TK_ARRAY:
                    {
                        currentToken = lex.NextToken();
                        Arreglo ret = new Arreglo();
                        //TODO: pensar que hacer acá
                        //FAlta hacer unas cosas acá
                        return ret;
                    }
                case TipoToken.TK_STRING:
                    {
                        currentToken = lex.NextToken();
                        return new Cadena();
                    }
            }
        }

        Expresiones Expr()
        {
            return exPrime(andExp());
        }

        Expresiones exPrime(Expresiones par)
        {
            if (currentToken.Tipo == TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                return exPrime(new Or(par, andExp()));
            }
            else return par;
        }

        Expresiones andExp()
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
                Expresiones tmp = Expr();
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
                return new Not(Expr());
            }
            else if (currentToken.Tipo == TipoToken.TK_ID)
            {
                string tmp = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    ExprFuncion ret = new ExprFuncion();
                    //TODO: armar la llamada a funcion
                }
            }
            else return LIT();
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
    }
}