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

        //Obtengo la lista de Eventos
        public static List<bitacora> ObtenerBitacoraUsuario(string usuario)
        {
            var query = from tbl in Bdd.bitacora.Include("usuario")
                        where tbl.usuario.UsuarioNick == usuario
                        orderby tbl.BitacoraId descending 
                        select tbl;

            return query.ToList();
        }

        //Adicion de un nuevo Evento
        public static bool InsertarEvento(string usuarioNick, string lenguaje, string descripcionEvento,string sentencia)
        {
            try
            {
                var query = new bitacora
                {
                    Bitacora_UsuarioId = ObtenerIdUsuario(usuarioNick),
                    BitacoraLenguaje = lenguaje,
                    BitacoraDescripcionEvento = descripcionEvento,
                    BitacoraSentencia = sentencia
                };

                Bdd.AddTobitacora(query);
                Bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //Obtengo el Id del Usuario
        public static int ObtenerIdUsuario(string usuarioNick)
        {
            var query = (from tbl in Bdd.usuario
                         where tbl.UsuarioNick == usuarioNick
                         select tbl.UsuarioId).Single();

            return Convert.ToInt32(query);
        }


     
    }
}