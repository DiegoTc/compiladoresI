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


        public override void parse()
        {
            StatementList();
        }

        public void StatementList()
        {
            Statement();
            StatementList();
        }

        public void Statement()
        {
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

                if(this.currentToken.Tipo!=Lexico.TipoToken.TK_OPENLLAVE)
                    throw new Exception("Se esperaba una }");

                this.currentToken = lex.NextToken();
                if(this.currentToken.Tipo != Lexico.TipoToken.TK_CASE)
                    throw new Exception("Se esperaba la palabra reservada CASE");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT || this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT || this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                    throw new Exception("Se esperaba un numero o un char");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                StatementList();
                this.currentToken = lex.NextToken();

                while (this.currentToken.Tipo == Lexico.TipoToken.TK_CASE)
                {
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CHAR_LIT || this.currentToken.Tipo != Lexico.TipoToken.TK_FLOAT_LIT || this.currentToken.Tipo != Lexico.TipoToken.TK_INT_LIT)
                        throw new Exception("Se esperaba un numero o un char");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                        throw new Exception("Se esperaba el simbolo :");
                    StatementList();
                    this.currentToken = lex.NextToken();
                }

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DEFAULT)
                    throw new Exception("Se esperaba la palabra reservada DEFAULT");

                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_DOSPUNTOS)
                    throw new Exception("Se esperaba el simbolo :");
                StatementList();
                this.currentToken = lex.NextToken();
            }
        }

        public void CompoundStatement()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
            {
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
                
            }
        }

        public void Expression() { }

    }
}
