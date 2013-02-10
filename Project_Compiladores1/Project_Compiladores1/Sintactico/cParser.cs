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
            statementList();
        }

        public void statementList()
        {

        }

        public void statement()
        {/*
            if (this.currentToken.Tipo == Lexico.TipoToken.TK_CHAR || this.currentToken.Tipo == Lexico.TipoToken.TK_INT || this.currentToken.Tipo == Lexico.TipoToken.TK_FLOAT || this.currentToken.Tipo == Lexico.TipoToken.TK_DOUBLE)
            {
                assign();
            }
            else if(this.currentToken.Tipo==Lexico.TipoToken.TK_PRINT)
            {

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SCANF)
            {

            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_IF)
            {
                this.currentToken = lex.NextToken();
                if(this.currentToken.Tipo!=Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                expression();

                if(this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");

                compoundStatement();
                this.currentToken = lex.NextToken();

                if (this.currentToken.Tipo == Lexico.TipoToken.TK_ELSE)
                {
                    compoundStatement();
                }
            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                expression();

                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");

                this.currentToken = lex.NextToken();
                compoundStatement();

            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_DO)
            {
                this.currentToken = lex.NextToken();
                compoundStatement();

                if (this.currentToken.Tipo == Lexico.TipoToken.TK_WHILE)
                {
                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                        throw new Exception("Se esperaba un (");

                    this.currentToken = lex.NextToken();
                    expression();

                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                        throw new Exception("Se esperaba un )");

                    this.currentToken = lex.NextToken();
                    if (this.currentToken.Tipo != Lexico.TipoToken.TK_PUNTOCOMA)
                        throw new Exception("Se esperaba un ;");
                }
            }
            else if(this.currentToken.Tipo == Lexico.TipoToken.TK_FOR)
            {
                this.currentToken = lex.NextToken();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_OPENPAR)
                    throw new Exception("Se esperaba un (");

                this.currentToken = lex.NextToken();
                expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_PUNTOCOMA)
                    throw new Exception("Se esperaba un ;");
                
                this.currentToken = lex.NextToken();
                expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_PUNTOCOMA)
                    throw new Exception("Se esperaba un ;");

                this.currentToken = lex.NextToken();
                expression();
                if (this.currentToken.Tipo != Lexico.TipoToken.TK_CLOSEPAR)
                    throw new Exception("Se esperaba un )");
                
                this.currentToken = lex.NextToken();
                compoundStatement();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_RETURN)
            {
                this.currentToken = lex.NextToken();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_BREAK)
            {
                this.currentToken = lex.NextToken();
            }
            else if (this.currentToken.Tipo == Lexico.TipoToken.TK_SWITCH)
            {

            }*/
        }

        public void expression() { }

        public void compoundStatement()
        {
            if (currentToken.Tipo == Lexico.TipoToken.TK_OPENLLAVE)
            {
                statementList();
                if (currentToken.Tipo != Lexico.TipoToken.TK_CLOSELLAVE)
                    throw new Exception("Se esperaba una }");

                currentToken = lex.NextToken();
            }
        }

        public void assign() { }
    }
}
