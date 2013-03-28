﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project_Compiladores1.Interpretador
{
    class InfInterpretador
    {
        private static InfInterpretador instance = null;
        Dictionary<string, Valor> valores = new Dictionary<string, Valor>();

        private InfInterpretador() { }

        public static InfInterpretador getInstance()
        {
            if (instance == null)
                instance = new InfInterpretador();
            return instance;
        }

        public void asignarValor(String id, Valor valor)
        {
            valores.Add(id, valor);
        }

        public Valor getValor(String id)
        {
            return valores[id];            
        }

    }
}
