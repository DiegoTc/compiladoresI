using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Sintactico
{
    class cParser : Parser
    {
        public cParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }


        public void parse()
        {
            StatementList();
            if(currentToken.Tipo != Lexico.TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo");
            Console.WriteLine("Proyecto Terminado\n");
        }

        public void StatementList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_PRINT || currentToken.Tipo == Lexico.TipoToken.TK_READ||currentToken.Tipo == Lexico.TipoToken.TK_IF||
                currentToken.Tipo == Lexico.TipoToken.TK_WHILE || currentToken.Tipo == Lexico.TipoToken.TK_DO ||currentToken.Tipo == Lexico.TipoToken.TK_FOR||
                currentToken.Tipo == Lexico.TipoToken.TK_BREAK|| currentToken.Tipo == Lexico.TipoToken.TK_SWITCH || currentToken.Tipo == Lexico.TipoToken.TK_RETURN||
                currentToken.Tipo == Lexico.TipoToken.TK_ID || currentToken.Tipo == Lexico.TipoToken.TK_PRIVATE || currentToken.Tipo == Lexico.TipoToken.TK_PUBLIC||
                currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_CHAR||
                currentToken.Tipo == Lexico.TipoToken.TK_TRUE || currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
            {
                Statement();
                StatementList();
            }
            
        }

        public void Statement()
        {
#region
            if(this.currentToken.Tipo==Lexico.TipoToken.TK_PRINT)
            {
                this.currentToken = lex.NextToken();
                if(currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");
                Expression();

                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");
                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");
                this.currentToken = lex.NextToken();
#endregion
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_READ)
            {
                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");
                this.currentToken = lex.NextToken();
                
                if(currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba un ID");

                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");
                this.currentToken = lex.NextToken();
                if (currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");
                this.currentToken = lex.NextToken();

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_IF)
            {
                this.currentToken = lex.NextToken();
                if(this.currentToken.Tipo!=Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                Expression();

                if(this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");

                this.currentToken = lex.NextToken();
                CompoundStatement();
                Else();

            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                Expression();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");

                this.currentToken = lex.NextToken();
                CompoundStatement();

            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_DO)
            {
                this.currentToken = lex.NextToken();
                CompoundStatement();

                if (this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
                {
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");

                    this.currentToken = lex.NextToken();
                    Expression();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");
                    this.currentToken = lex.NextToken();
                }
                else
                    throw new Exception("Se esperaba la palabra reservada WHILE");
                
            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_FOR)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                if(this.currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba un ID");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_ASSIGN)
                    throw new Exception("Se esperaba el simbolo =");

                this.currentToken = lex.NextToken();
                Expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                        throw new Exception("Se esperaba un ;");
                
                this.currentToken = lex.NextToken();
                Expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
                Expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");
                
                this.currentToken = lex.NextToken();
                CompoundStatement();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_RETURN)
            {
                this.currentToken = lex.NextToken();
                Expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_BREAK)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");
                this.currentToken = lex.NextToken();
                Expression();

                if(this.currentToken.Tipo!=Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba una )");

                this.currentToken = lex.NextToken();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENLLAVE)
                    throw new Exception("Se esperaba una {");

                this.currentToken = lex.NextToken();
                if(this.currentToken.Tipo != Lexico.TipoToken.TK_CASE)
                    throw new Exception("Se esperaba la palabra reservada CASE");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                    throw new Exception("Se esperaba un numero o un char");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                currentToken = lex.NextToken();
                StatementList();


                Cases();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                    throw new Exception("Se esperaba la palabra reservada DEFAULT");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                this.currentToken = lex.NextToken();
                StatementList();
                

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Se esperaba una }");

                this.currentToken = lex.NextToken();

            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                StatementP();
                if(currentToken.Tipo != Lexico.TipoToken.TK_FINSENTENCIA)
                    throw new Exception("Se esperaba el simbolo ;");
                currentToken = lex.NextToken();
            }

            else if (currentToken.Tipo == Lexico.TipoToken.TK_PUBLIC || currentToken.Tipo == Lexico.TipoToken.TK_PRIVATE)
            {
                currentToken = lex.NextToken();
                Tipo();
                Declaration();
                
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || currentToken.Tipo == Lexico.TipoToken.TK_CHAR||
                    currentToken.Tipo == Lexico.TipoToken.TK_TRUE || currentToken.Tipo == Lexico.TipoToken.TK_FALSE)
            {
                currentToken = lex.NextToken();
                Declaration();
            }
        }

        public void Cases()
        {
            if (this.currentToken.Tipo == Lexico.TipoToken.TK_CASE)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT && this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                    throw new Exception("Se esperaba un numero o un char");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                currentToken = lex.NextToken();
                StatementList();
                Cases();
            }
        }

        public void Declaration()
        {
        
            if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                throw new Exception("Se esperaba el token ID");

            currentToken = lex.NextToken();
            DeclarationP();
            //AssignDecl();

            
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

        public void DeclarationP()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();

                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                currentToken = lex.NextToken();
                DeclarationP();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
                Expression();
                DeclarationP();
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                ParametroList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba el token )");
                currentToken = lex.NextToken();
                CompoundStatement();   
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_FINSENTENCIA)
                currentToken = lex.NextToken();

            else
                throw new Exception("Se esperaba el token ;");
            
        }

        public void ParametroList()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_CHAR || currentToken.Tipo == Lexico.TipoToken.TK_INT || currentToken.Tipo == Lexico.TipoToken.TK_FLOAT ||
                currentToken.Tipo == Lexico.TipoToken.TK_BOOL)
            {
                Tipo();
                if(currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                currentToken = lex.NextToken();
                ParametroListP();
            }
        }

        public void ParametroListP()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Tipo();
                if (currentToken.Tipo != Lexico.TipoToken.TK_ID)
                    throw new Exception("Se esperaba el token ID");

                currentToken = lex.NextToken();
                ParametroListP();
            }
        }

        public void AssignDecl()
        {
            if (currentToken.Tipo != Lexico.TipoToken.TK_ASSIGN)
                throw new Exception("Se esperaba el token =");
            currentToken = lex.NextToken();
            Expression();
        }

        public void StatementP()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENPAR)
            {
                currentToken = lex.NextToken();
                ExpreList();
                if (currentToken.Tipo == Lexico.TipoToken.TK_CLOSEPAR)
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
            if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN || currentToken.Tipo == Lexico.TipoToken.TK_MASIGUAL || currentToken.Tipo == Lexico.TipoToken.TK_MENOSIGUAL ||
                currentToken.Tipo == Lexico.TipoToken.TK_PORIGUAL || currentToken.Tipo == Lexico.TipoToken.TK_ENTREIGUAL)
            {
                currentToken = lex.NextToken();
                Expression();
            }
            else if(currentToken.Tipo == Lexico.TipoToken.TK_OPENCOR)
            {
                Expression();
                StatementP2();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSMENOS || currentToken.Tipo == Lexico.TipoToken.TK_MASMAS)
            {
                Expression();
            }
        }

        public void AssignOp()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_ASSIGN)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MASIGUAL)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_MENOSIGUAL)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_PORIGUAL)
            {
                currentToken = lex.NextToken();
            }
            else if (currentToken.Tipo == Lexico.TipoToken.TK_ENTREIGUAL)
            {
                currentToken = lex.NextToken();
            }
        }

        public void ExpreList()
        {
            Expression();
            ExprelistP();
        }

        public void ExprelistP()
        {
            if(currentToken.Tipo == Lexico.TipoToken.TK_COMA)
            {
                currentToken = lex.NextToken();
                Expression();
                ExprelistP();
            }
        }

        public void CompoundStatement()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
            {
                currentToken = lex.NextToken();
                StatementList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Se esperaba una }");

                currentToken = lex.NextToken();
            }
        }

        public void Else() 
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_ELSE)
            {
                currentToken = lex.NextToken();
                CompoundStatement();
            }
        }

        public void Expression() 
        {
            Andexp();
            Expression_prime();
        }

        public void Expression_prime()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OR)
            {
                currentToken = lex.NextToken();
                Expression();
                Expression_prime();
            }
        }
        
        public void Andexp()
        {
            Relexp();
            Andexp_prime();
        }

        public void Andexp_prime()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_AND)
            {
                currentToken = lex.NextToken();
                Addexp();
                Andexp_prime();
            }
        }

        public void Relexp() 
        {
            Addexp(); 
            Relexp_prime();
        }

        public void Relexp_prime()
        {
            Relop();
            Addexp();
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
            else if(currentToken.Tipo == Lexico.TipoToken.TK_MAYORQUE)
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

        public void Addexp()
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
                if(currentToken.Tipo!=Lexico.TipoToken.TK_CLOSEPAR)
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
