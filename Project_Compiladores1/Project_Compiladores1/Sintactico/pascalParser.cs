using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Sintactico
{
    class pascalParser:Parser
    {
        public pascalParser(Lexico.Lexico lexer)
            : base(lexer)
        {
        }
    }
}
