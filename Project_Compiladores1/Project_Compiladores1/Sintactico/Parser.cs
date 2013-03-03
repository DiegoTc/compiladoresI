using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Project_Compiladores1.Sintactico
{
    class Parser
    {
        public Lexico.Lexico lex;
        public Lexico.Token currentToken;

        public Parser(Lexico.Lexico lexer)
        {
            lex = lexer;
            currentToken = lex.NextToken();
        }



        public void parse()
        {
            
        }
    }
}
