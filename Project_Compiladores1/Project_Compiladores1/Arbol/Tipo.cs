using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Project_Compiladores1.Arbol
{
    public abstract class Tipo
    {
        public abstract bool esEquivalente(Tipo t);
    }
    
    class Booleano : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Booleano;
        }
    }

    class Cadena : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Cadena;
        }
    }

    class Entero : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Entero;
        }

    }

    class Flotante : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Flotante;
        }
    }

    class Caracter : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Caracter;
        }
    }

    class Voids : Tipo
    {
        public override bool esEquivalente(Tipo t)
        {
            return t is Voids;
        }
    }

    class Class : Tipo
    {
        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public override bool esEquivalente(Tipo t)
        {
            return t is Class;
        }
    }

    class Struct : Tipo
    {
        private T_Campos _campos;

        public Struct(T_Campos Camp)
        {
            Campos = Camp;
        }

        public T_Campos Campos
        {
            get { return _campos; }
            set { _campos = value; }
        }

        public override bool esEquivalente(Tipo t)
        {
            return t is Struct;
        }
    }

    class Arreglo : Tipo
    {
        private int dimensiones; //Debe Ser Entero
        private ArrayList rangos;
        private Tipo contenido;

        public Tipo Contenido
        {
            get { return contenido; }
            set { contenido = value; }
        }

        public int Dimensiones
        {
            get { return dimensiones; }
            set { dimensiones = value; }
        }

        public ArrayList Rangos
        {
            get { return rangos; }
            set { rangos = value; }
        }


        public override bool esEquivalente(Tipo t)
        {
            return t is Arreglo;
        }

    }

    class T_Campos : Dictionary<string, Tipo>
    {
        private ArrayList ids = new ArrayList();

        public ArrayList Ids
        {
            get { return ids; }
            set { ids = value; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Add(String key, Tipo value)
        {
            ids.Add(key);
            base.Add(key, value);            

        }

        public void addVars(Object[] p)
        {
            ArrayList Ids = ((ArrayList) p[0]);
            Tipo t = ((Tipo) p[1]);
            for (int i = 0; i < ids.Count; i++)
            {
                this.Add(Ids[i].ToString(), t);

            }
        }
    }



}
