using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;

namespace Project_Compiladores1.Sintactico
{
    class cParser : Parser
    {
        public cParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }


        public Sentencia parse()
        {
            Sentencia S = StatementList();
            if (currentToken.Tipo != Lexico.TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo");
            Console.WriteLine("Proyecto Terminado\n");
            return S;
        }

        public Sentencia StatementList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ || currentToken.Tipo == Lexico.TipoToken.TK_IF ||
                currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO || currentToken.Tipo == Lexico.TipoToken.TK_FOR ||
                currentToken.Tipo == Lexico.TipoToken.TK_BREAK || currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN ||
                currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == Lexico.TipoToken.TK_PRIVATE || currentToken.Tipo == Lexico.TipoToken.TK_PUBLIC ||
                currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_CHAR ||
                currentToken.Tipo == Lexico.TipoToken.TK_TRUE || currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
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

        public Sentencia Statement()
        {

            if (this.currentToken.Tipo == Lexico.TipoToken.TK_PRINT)
            {
                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");
                S_Print sprint = new S_Print();
                sprint.Expr = Expression();

                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");
                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");
                this.currentToken = lex.NextToken();
                return sprint;

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_READ)
            {
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

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_IF)
            {
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

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
            {
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
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_DO)
            {
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

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_FOR)
            {
                S_For sfor = new S_For();
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba un ID");

                sfor.id.var = this.currentToken.Lexema;
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
                sfor.S = CompoundStatement();
                return sfor;
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_RETURN)
            {
                S_Return sreturn = new S_Return();
                this.currentToken = lex.NextToken();
                sreturn.Expr=Expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
                return sreturn;
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_BREAK)
            {
                S_Break sbreak = new S_Break();
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
                return sbreak;
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {
                S_Switch sSwitch = new S_Switch();
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");
                this.currentToken = lex.NextToken();
                sSwitch.Var= Expression();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba una )");

                this.currentToken = lex.NextToken();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENLLAVE)
                    throw new Exception("Se esperaba una {");

                this.currentToken = lex.NextToken();
                sSwitch.Casos= Cases();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                    throw new Exception("Se esperaba la palabra reservada DEFAULT");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                this.currentToken = lex.NextToken();
               sSwitch.sdefault=StatementList();


                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Se esperaba una }");

                this.currentToken = lex.NextToken();
                return sSwitch;

            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
            {
                Sentencia S = new Sentencia();
                Variable id;
                id.id= currentToken.Lexema;
                currentToken = lex.NextToken();
                S=StatementP(id);
                if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba el simbolo ;");
                currentToken = lex.NextToken();
                return S;
            }

            else if (currentToken.Tipo == Lexico.TipoToken.TK_PUBLIC || currentToken.Tipo == Lexico.TipoToken.TK_PRIVATE)
            {
                currentToken = lex.NextToken();
                
                Sentencia s = new Sentencia();
                s=Declaration();
                return s;

            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_CHAR ||
                    currentToken.Tipo == Lexico.TipoToken.TK_TRUE || currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
            {
                
                Sentencia s = new Sentencia();
                s = Declaration();
                return s;
            }
        }

        public Cases Cases()
        {
            if (this.currentToken.Tipo == Lexico.TipoToken.TK_CASE)
            {
                Cases cas = new Cases();
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                    throw new Exception("Se esperaba un numero o un char");

                cas.Valor = currentToken.Lexema;
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                currentToken = lex.NextToken();
                cas.S= StatementList();
                cas.sig=Cases();
                return cas;
            }
            return null;
        }

        public Campos Declaration()
        {
            Campos campos = new Campos();
            campos.Tip= Tipo();
            if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                throw new Exception("Se esperaba el token ID");

            campos.Var.id = currentToken.Lexema;
            currentToken = lex.NextToken();
            DeclarationP(campos);

            return campos;
        }

        public void Tipo()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_INT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_FLOAT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_STRING)
            {

            }
        }

        public Sentencia DeclarationP(Sentencia campos)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Campos c = new Campos();
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                c.Var.id = currentToken.Lexema;
                currentToken = lex.NextToken();
               
                campos.sig=DeclarationP(c);
                return campos;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
            {
                S_Asignacion sasignacion = new S_Asignacion();
                
                sasignacion.id.id = ((Campos)campos).Var.id;
                currentToken = lex.NextToken();
                sasignacion.Valor= Expression();
                return DeclarationP(sasignacion);
                
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
            {
                S_Functions sfunciones = new S_Functions();
                currentToken = lex.NextToken();
                sfunciones.Retorno = ((Campos)campos).Tip;
                sfunciones.var.id = ((Campos)campos).Var.id;
                sfunciones.Campo= ParametroList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba el token )");
                currentToken = lex.NextToken();
                sfunciones.S= CompoundStatement();
                
                return sfunciones;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_FINSENTENCIA)
            {
                currentToken = lex.NextToken();
                return campos;
            }
            else
                throw new Exception("Se esperaba el token ;");

        }

        public Campos ParametroList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR || currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT ||
                currentToken.Tipo == Lexico.TipoToken.TK_BOOL)
            {
                Campos c = new Campos();
                c.Tip= Tipo();
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");
                c.Var.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                c.Sig= ParametroListP();
                return c;
            }
        }

        public Campos ParametroListP()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                Campos c = new Campos();
                currentToken = lex.NextToken();
                c.Tip= Tipo();
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                c.Var.id = currentToken.Lexema;
                currentToken = lex.NextToken();
                c.Sig= ParametroListP();
                return c;
            }
            return null;
        }


        public Sentencia StatementP(Variable id)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
            {
                S_LlamadaFunc sfunc = new S_LlamadaFunc();
                sfunc.Var=id;
                currentToken = lex.NextToken();
                sfunc.VarList= ExpreList();
                if (currentToken.Tipo == Lexico.TipoToken.TK_CLOSEPAR)
                {
                    currentToken = lex.NextToken();
                    return sfunc;
                }
                else
                    throw new Exception("Se esperaba el token )");
                
            }
            else
            {
                Sentencia S = new Sentencia();
                S=StatementP2(id);
                return S;
            }
        }

        public Sentencia StatementP2(Variable id)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
            {
                S_Asignacion sasign = new S_Asignacion();
                sasign.Op = new Igual();
                sasign.id = id;
                currentToken = lex.NextToken();
                sasign.Valor= Expression();
                return sasign;
                
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_MASIGUAL)
            {
                S_Asignacion sasign = new S_Asignacion();
                sasign.Op = new MasIgual();
                sasign.id = id;
                currentToken = lex.NextToken();
                sasign.Valor = Expression();
                return sasign;
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_MENOSIGUAL)
            {
                S_Asignacion sasign = new S_Asignacion();
                sasign.Op = new MenosIgual();
                sasign.id = id;
                currentToken = lex.NextToken();
                sasign.Valor = Expression();
                return sasign;
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_PORIGUAL)
            {
                S_Asignacion sasign = new S_Asignacion();
                sasign.Op = new PorIgual();
                sasign.id = id;
                currentToken = lex.NextToken();
                sasign.Valor = Expression();
                return sasign;
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_ENTREIGUAL)
            {
                S_Asignacion sasign = new S_Asignacion();
                sasign.Op = new EntreIgual();
                sasign.id = id;
                currentToken = lex.NextToken();
                sasign.Valor = Expression();
                return sasign;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
            {
                currentToken = lex.NextToken();
                S_Asignacion assig = new S_Asignacion();
                assig.id = id;
                assig.id.acces=Expression();

                 if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSECOR)
                    throw new Exception("Se esperaba el token ]");

                 Sentencia S = new Sentencia();
                 S=StatementP2(assig.id);
                 assig.Valor = ((S_Asignacion)S).Valor;
                if(currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba el token ;");

                currentToken = lex.NextToken();
                return assig;
            }
           else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSMENOS || currentToken.Tipo == Lexico.TipoToken.TK_MASMAS)
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

       

        public Expresiones ExpreList()
        {
            Expresiones E= Expression();
            return ExprelistP(E);
        }

        public ListaExpre ExprelistP(Expresiones E)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Expresiones E1= Expression();
                return ExprelistP(E1);
            }
        }

        public Sentencia CompoundStatement()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
            {
                currentToken = lex.NextToken();
                Sentencia S = new Sentencia();
                S=StatementList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Se esperaba una }");

                currentToken = lex.NextToken();
                return S;
            }
            return null;
        }

        public Sentencia Else()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                Sentencia S= CompoundStatement();
                return S;
            }
            return null;
        }

        public Expresiones Expression()
        {
            Expresiones E1= Andexp();
            return Expression_prime(E1);
        }

        public Expresiones Expression_prime(Expresiones E)
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

        public Expresiones Andexp()
        {
            Expresiones E1= Relexp();
            return Andexp_prime(E1);
        }

        public Expresiones Andexp_prime(Expresiones E1)
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                Expresiones E= Addexp();
                And eAnd= new And(E1, Andexp_prime(E));
                return eAnd;
            }
            return E1;
        }

        public Expresiones Relexp()
        {
            Expresiones E= Addexp();
            return Relexp_prime(E);
        }

        public Expresiones Relexp_prime(Expresiones E)
        {

            if (currentToken.Tipo == Lexico.TipoToken.TK_IGUALDAD)
            {
                Equal equal = new Equal(E, Addexp());
                currentToken = lex.NextToken();
                return equal;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_DISTINTO)
            {
                Equal equal = new Equal(E, Addexp());
                currentToken = lex.NextToken();
                return equal;
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORQUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORIGUAL)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORQUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORIGUAL)
            {
                currentToken = lex.NextToken();
            }

        }

        public void Relop()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_IGUALDAD)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_DISTINTO)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORQUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MAYORIGUAL)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORQUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENORIGUAL)
            {
                currentToken = lex.NextToken();
            }
        }

        public Expresiones Addexp()
        {
            Multexp();
            Addexp_prime();
        }

        public void Addexp_prime()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_SUMA)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_RESTA)
            {
                currentToken = lex.NextToken();
            }
        }

        public void Multexp()
        {

            Parexp();
            Multexp_prime();

        }

        public void Multexp_prime()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_MULT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_DIV)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MOD)
            {
                currentToken = lex.NextToken();
            }
        }

        public void Parexp()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                Expression();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");

                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_INT_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_FLOAT_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_STRING_LIT)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();

                StatementP();

            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_TRUE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MASMAS)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSMENOS)
            {
                currentToken = lex.NextToken();
            }
        }

    }
}
