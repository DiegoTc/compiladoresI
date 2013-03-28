using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project_Compiladores1.Arbol;


namespace Project_Compiladores1.Interpretador
{
    class ValorArreglo : Valor
    {
        private ArrayList elementos; //Valor

        public ValorArreglo(Arreglo tipo)
        {
            ArrayList dimensiones = tipo.Rangos; //Dimensiones Expresiones
            int size = 0;
            for (int i = 0; i < dimensiones.Count; i++)
            {
                Valor tmp = ((Expresiones) (dimensiones[i])).interpretar();
                size += ((ValorEntero)tmp).Valor;
            }
            Elementos = new ArrayList(size);
            this.Tipo = tipo;
        }
        
        private int MultipleToLineal(ArrayList indices)
        {
            Arreglo arr = (Arreglo)this.Tipo;
            int pos = 0;
            for (int i = 0; i < indices.Count; i++)
            {
                int tmp = 1;
                for (int j = i + 1; j < arr.Rangos.Count; j++)
                {
                    Valor tmpv = ((Expresiones)(arr.Rangos[j])).interpretar();
                    tmp *= ((ValorEntero)tmpv).Valor;                    
                }
                Valor tmpp = ((Expresiones)(indices[i])).interpretar();
                pos += tmp * ((ValorEntero)tmpp).Valor;                
            }
            return pos;
        }

        public void set(ArrayList indices, Valor valor)
        {
            int pos = MultipleToLineal(indices);
            Elementos[pos] = valor;
            //elementos.set(pos, valor);
        }

        public Valor get(ArrayList indices)
        {
            int pos = MultipleToLineal(indices);
            return (Valor)Elementos[pos];
            //return elementos.get(pos);
        }

        public ArrayList Elementos
        {
            get { return elementos; }
            set { elementos = value; }
        }

        public override Valor Clonar()
        {
            ValorArreglo arr = new ValorArreglo((Arreglo)Tipo);
            
            for (int i = 0; i < elementos.Count; i++)
            {
                Valor tmpcl = ((Valor) Elementos[i]).Clonar();
                arr.Elementos[i] = tmpcl;
                //arr.getElementos().set(i, elementos.get(i).Clonar());
            }
            return arr;    
        }
    }
}
