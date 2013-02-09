using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Sintactico
{
    class javaParser:Parser
    {
        public javaParser(Lexico.Lexico lexer)
            :base(lexer)
        {
        }
    }
}
