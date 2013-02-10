using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser:Parser
    {
        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }

        public void parse()
        {
            SL();
            if (currentToken.Tipo != TipoToken.TK_FINFLUJO)
                throw new Exception("Se esperaba fin flujo ");
        }

        void SL()
        {
            S();

        }

        void S()
        {
            if (currentToken.Tipo == TipoToken.TK_ID)
            {
                currentToken = lex.NextToken();
                expr();
                if (currentToken.Tipo == TipoToken.TK_THEN)
                {
                    IFP();
                }
            }
        }

        void IFP()
        {
            CS();
            ELSE();
        }

        void CS()
        {
        }

        void ELSE()
        {

        }

        void expr()
        {

        }
    }
}
