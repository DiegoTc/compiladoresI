using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebUI.Arbol;


namespace WebUI.Interpretador
{
    class ValorArreglo : Valor
    {
        private ArrayList elementos; //Valor

        public ValorArreglo(Arreglo tipo)
        {
            ArrayList dimensiones = tipo.Rangos; //Dimensiones Expresiones
            Valor tmpsize = ((Expresiones) (dimensiones[0])).interpretar();
            int size = ((ValorEntero)tmpsize).Valor;
            for (int i = 1; i < dimensiones.Count; i++)
            {
                Valor tmp = ((Expresiones) (dimensiones[i])).interpretar();
                size *= ((ValorEntero)tmp).Valor;//POIO LO TIENE CON MAS
            }
            Elementos = new ArrayList();
            for (int i = 0; i < size;i++)
            {
                if (tipo.Contenido is Entero)
                    Elementos.Add(new ValorEntero(0));
                if (tipo.Contenido is Cadena)                   
                    Elementos.Add(new ValorCadena(""));
                if (tipo.Contenido is Flotante)
                    Elementos.Add(new ValorFlotante((float)(0.0)));
                if (tipo.Contenido is Caracter)
                    Elementos.Add(new ValorCaracter(""));
                if (tipo.Contenido is Booleano)
                    Elementos.Add(new ValorBooleano(true));

            }           
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
                    tmp *= ((ValorEntero)tmpv).Valor;  //POIO TENIA CON POR                  
                }
                //Valor tmpp = (ValorEntero)((Expresiones)(indices[i])).interpretar();
                Valor tmpp ;
                if (indices[i] is Valor)
                {    
                    tmpp = (ValorEntero)indices[i];
                    pos += tmp * ((ValorEntero)tmpp).Valor;                
                }
                else
                {
                    Valor tmpv = ((Expresiones) (indices[i])).interpretar();
                    //tmpp = new ValorEntero(((ValorEntero)tmpv).Valor);
                    pos += tmp * ((ValorEntero)tmpv).Valor;
                }
                
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
