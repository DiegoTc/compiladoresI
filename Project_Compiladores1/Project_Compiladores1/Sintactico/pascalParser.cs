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
                ret.Condicion=expr();
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
                ret.Condicion=expr();
                if (currentToken.Tipo == TipoToken.TK_DO)
                {
                    currentToken = lex.NextToken();
                    ret.S=CS();
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
                        ret.Inicio=expr();
                        if (currentToken.Tipo == TipoToken.TK_TO)
                        {
                            currentToken = lex.NextToken();
                            ret.Iteracion=expr();
                            if (currentToken.Tipo == TipoToken.TK_DO)
                            {
                                currentToken = lex.NextToken();
                                ret.S=CS();
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
                ret.S=SL();
                if (currentToken.Tipo == TipoToken.TK_UNTIL)
                {
                    ret.Condicion=expr();
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
                        ret.Casos=CL();
                        if (currentToken.Tipo == TipoToken.TK_END)
                        {
                            currentToken = lex.NextToken();
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
                return new S_Continue
            }
            else if (currentToken.Tipo == TipoToken.TK_EXIT)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_OPENPAR)
                {
                    currentToken = lex.NextToken();
                    S_Return ret=new S_Return();
                    ret.Expr=expr();
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
                S_Print ret =new S_Print();
                ret.Expr=expr();
                return ret;
            }
            else if(currentToken.Tipo == TipoToken.TK_READ)
            {
                currentToken=lex.NextToken();
                S_Read ret = new S_Read();
                ret.var=expr();
                return ret;
            }
            else if (currentToken.Tipo == TipoToken.TK_TYPE)
                return TD();
            else if (currentToken.Tipo == TipoToken.TK_VAR)
                return VD();
            else if (currentToken.Tipo == TipoToken.TK_FUNCTION || currentToken.Tipo == TipoToken.TK_VOID)
                return FD();
        }

        S_Struct TD()
        {
            currentToken = lex.NextToken();
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                S_Struct ret = new S_Struct();
                ret.nombre = currentToken.Lexema;
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_IGUALDAD)
                {
                    currentToken = lex.NextToken();
                    if (currentToken.Tipo == TipoToken.TK_RECORD)
                    {
                        currentToken = lex.NextToken();
                        ret.miembros=FL();
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
                IDL();
                if (currentToken.Tipo == TipoToken.TK_DOSPUNTOS)
                {
                    currentToken = lex.NextToken();
                    TYPE();
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    {
                        currentToken = lex.NextToken();
                        FL();
                    }
                    else throw new Exception("Se esperaba ;");
                }
                else throw new Exception("Se esperaba :");
            }
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
            else STYPE();
        }

        void STYPE()
        {
            switch (currentToken.Tipo)
            {
                case TipoToken.TK_INT:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_FLOAT:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_CHAR:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_STRING:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_BOOL:
                    currentToken = lex.NextToken();
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
                    currentToken = lex.NextToken();
                    TYPE();
                }
                else throw new Exception("Se esperaba :");
            }//else return
        }

        void FD()
        {
            HEAD();
            do
            {
                VD();
                if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                    currentToken = lex.NextToken();
                else
                    throw new Exception("se esperaba ;");
            } while (currentToken.Tipo == TipoToken.TK_VAR);
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
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_FLOAT_LIT:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_CHAR_LIT:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_STRING_LIT:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_TRUE:
                    currentToken = lex.NextToken();
                    return;
                case TipoToken.TK_FALSE:
                    currentToken = lex.NextToken();
                    return;
                default:
                    throw new Exception("Se esperaba una literal");
            }
        }

        Sentencia S_prime(Variable par)
        {
            if (currentToken.Tipo == TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                exprl();
                if (currentToken.Tipo == TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                }
                else throw new Exception("Se esperaba )");
            }
            else
                return S_prime2(par);
        }

        S_Asignacion S_prime2(Variable par)
        {
            if (currentToken.Tipo == TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
                S_Asignacion 
                expr();
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
            else if (currentToken.Tipo == TipoToken.TK_PUNTO)
            {
                currentToken = lex.NextToken();
                if (currentToken.Tipo == TipoToken.TK_ID)
                {
                    currentToken = lex.NextToken();
                    S_prime2();
                }
            }
            //else throw new Exception("solo asignacion, llamada y declaracion pueden ser sentencias.");
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

        void IFP(S_If par)
        {
            par.Cierto=CS();
            if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                par.Falso=ELSE();
            }
            else throw new Exception("se esperaba ;");
            
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
                    /*
                    if (currentToken.Tipo == TipoToken.TK_FINSENTENCIA)
                        currentToken = lex.NextToken();
                    else throw new Exception("se esperaba ;");
                    */
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
