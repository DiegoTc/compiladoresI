using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Project_Compiladores1.Lexico;


namespace Project_Compiladores1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            StreamReader streamReader = new StreamReader("C:\\Pascal.txt");
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            /*LexicoC l = new LexicoC(text);
            Token t = l.NextToken();
            while (t.Tipo != TipoToken.TK_FINFLUJO)
            {
                Console.WriteLine(t.Tipo.ToString() + "  " + t.Lexema);
                t = l.NextToken();
            }
            
            LexicoJava lj = new LexicoJava(text);
            Token tj = lj.NextToken();
            while (tj.Tipo != TipoToken.TK_FINFLUJO)
            {
                Console.WriteLine(tj.Tipo.ToString() + "  " + tj.Lexema);
                tj = lj.NextToken();
            }*/

            LexicoPascal lp = new LexicoPascal(text);
            Token tp = lp.NextToken();
            while (tp.Tipo != TipoToken.TK_FINFLUJO)
            {
                Console.WriteLine(tp.Tipo.ToString() + "  " + tp.Lexema);
                tp = lp.NextToken();
            }


            Console.ReadKey();
        }
    }
}
