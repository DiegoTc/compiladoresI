using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;
using Project_Compiladores1.Sintactico;


namespace Project_Compiladores1
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamReader streamReader = new StreamReader("C:\\Pascal.txt");
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            LexicoC l = new LexicoC(text);
            cParser s = new cParser(l);
            s.parse();
            Console.ReadKey();

            /*LexicoPascal lp = new LexicoPascal(text);
            Token tp = lp.NextToken();
            while (tp.Tipo != TipoToken.TK_FINFLUJO)
            {
                Console.WriteLine(tp.Tipo.ToString() + "  " + tp.Lexema);
                tp = lp.NextToken();
            }*/


            Console.ReadKey();
        }
    }
}
