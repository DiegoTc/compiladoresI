using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                return;
            }
        }

        protected void BtnIngresarOnClick(object sender, EventArgs e)
        {
            //Valido que el usuario existe
            if (Conexion.ValidarUsuario(username.Value, EncriptacionMD5.CreateMd5Hash(password.Value)))
            {
                //Valido que el usuario este activo
                if (Conexion.ValidarUsuarioActivo(username.Value))
                {
                    //Obtengo el nombre del cliente para el usuario
                    var usuario = Conexion.ObtenerUsuarioAdministrativo(username.Value);
                    Session["UsuarioId"] = usuario.UsuarioId;
                    Session["UsuarioNick"] = usuario.UsuarioNick;
                    Session["UsuarioNombre"] = usuario.UsuarioNombre;
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    msg.Visible = true;
                    msg.InnerText = "Su Usuario aun no esta activo!";
                }
            }
            else
            {
                msg.Visible = true;
                msg.InnerText = "Credenciales Inválidas, Intente de Nuevo!";
            }
        }
    }
}