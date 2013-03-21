using System;
using System.Collections.Generic;
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
                //type definitions
                //variable declarations
                //functiondefinitionlist
                case TipoToken.TK_IF:
                {
                    currentToken = lex.NextToken();
                    S_If ret = new S_If();
                    ret.Condicion = Expr();
                    if (currentToken.Tipo != TipoToken.TK_THEN)
                        throw new Exception("Se esperaba then");
                    else
                    {
                        currentToken = lex.NextToken();
                        ret.Cierto = CompoundStatement();
                        if (currentToken.Tipo == TipoToken.TK_ELSE)
                        {
                            currentToken = lex.NextToken();
                            ret.Falso = CompoundStatement();
                        }
                        return ret;
                    }
                }
                case TipoToken.TK_FOR:
                {
                    currentToken = lex.NextToken();
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
                                    it.ID = ret.Var;
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
                                ret.S = CompoundStatement();
                                return ret;
                            }
                        }
                    }
                }
                case TipoToken.TK_WHILE:
                {
                    currentToken = lex.NextToken();
                    S_While ret = new S_While();
                    ret.Condicion = Expr();
                    if (currentToken.Tipo != TipoToken.TK_DO)
                        throw new Exception("Se esperaba do.");
                    else
                    {
                        currentToken = lex.NextToken();
                        ret.S = CompoundStatement();
                        return ret;
                    }
                }
                case TipoToken.TK_REPEAT:
                {
                    currentToken = lex.NextToken();
                    S_Do ret = new S_Do();
                    ret.S = CompoundStatement();
                    if (currentToken.Tipo != TipoToken.TK_UNTIL)
                        throw new Exception("Se esperaba until.");
                    else
                    {
                        currentToken = lex.NextToken();
                        ret.Condicion = Expr();
                        return ret;
                    }
                }
                case TipoToken.TK_RETURN:
                {
                    S_Return ret = new S_Return();
                    
                }
                //id assignment or functioncall
            }
        }
    }
}