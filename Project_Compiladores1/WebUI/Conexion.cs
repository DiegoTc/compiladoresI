using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI
{
    public class Conexion
    {
        public static multiinterpretadorEntities Bdd = new multiinterpretadorEntities();

        //Valido si el Usuario Existe
        public static List<usuario> ObtenerUsuarios()
        {
            var query = from tbl in Bdd.usuario
                        select tbl;

            return query.ToList();
        }

        //Valido si el usuario existe
        public static bool ValidarUsuario(string usuario, string contrasena)
        {
            try
            {
                var query = from tblCliente in Bdd.usuario
                            where tblCliente.UsuarioNick == usuario && tblCliente.UsuarioClave == contrasena
                            select tblCliente;

                return query.Count() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Validar si el Usuario esta activo
        public static bool ValidarUsuarioActivo(string usuario)
        {
            try
            {
                var query = (from tbl in Bdd.usuario
                             where tbl.UsuarioNick == usuario
                             select tbl).Single();

                return query.UsuarioEstado == 1;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //Obtener Usuario Especifico
        public static usuario ObtenerUsuarioAdministrativo(string usuario)
        {
            var query = (from tbl in Bdd.usuario
                         where tbl.UsuarioNick == usuario
                         select tbl).Single();

            return query;
        }


     
    }
}