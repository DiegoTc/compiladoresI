﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;
using Project_Compiladores1.Lexico;
using Project_Compiladores1.Sintactico;
using Project_Compiladores1.Arbol;


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
            Sentencia raiz = s.parse();
            Console.ReadKey();

        }
    }
}