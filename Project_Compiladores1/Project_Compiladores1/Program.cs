using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;
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
            LexicoC C = new LexicoC(text);

            parserC s = new parserC(C);
            Sentencia raiz = s.parse();
            raiz.SentValSemantica();
            raiz.interpretar();
            
            Console.ReadKey();
        }
    }
}