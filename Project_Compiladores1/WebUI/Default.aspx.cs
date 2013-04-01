using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Arbol;
using WebUI.Lexico;
using WebUI.Sintactico;
using System.Web.Services;

namespace WebUI
{
    public partial class _Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if ((Session["UsuarioId"]).Equals(string.Empty))
                {
                    Response.Redirect("Login.aspx");
                }
                if (IsPostBack)
                {
                    return;
                }
            }
            catch (Exception)
            {
                Response.Redirect("Login.aspx");

            }

            CargarBitacoraUsuario();

            if (!IsPostBack)
            {
                lblNombre.Text = Session["UsuarioNombre"].ToString();
                lblPerfil.Text = ObtenerPerfil();
            }
            
            if(IsPostBack)
            {
                CargarBitacoraUsuario();
            }
        }

        public string ObtenerPerfil()
        {
            var h = Conexion.ObtenerUsuarioAdministrativo(Session["UsuarioNick"].ToString());
            return h.perfil.Nombre;
        }

        public void CargarBitacoraUsuario()
        {
            gvBitacora.DataSource = Conexion.ObtenerBitacoraUsuario(Session["UsuarioNick"].ToString());
            gvBitacora.DataBind();
        }

        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoPascal(string sentencia)
        {
            HttpContext.Current.Session["MsjPascal"] = string.Empty;

            var lp = new LexicoPascal(sentencia);
            var pp = new pascalParser(lp);
            Sentencia raizP= pp.parse();

            //raizP.SentValSemantica();
            //raizP.interpretar();

            Conexion.InsertarEvento(HttpContext.Current.Session["UsuarioNick"].ToString(), "Pascal",
                                    HttpContext.Current.Session["MsjPascal"].ToString(),sentencia);

        
            return HttpContext.Current.Session["MsjPascal"].ToString();

        }


        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoC(string sentencia)
        {          
            HttpContext.Current.Session["MsjC"] = string.Empty;

            var lc = new LexicoC(sentencia);
            var cp = new parserC(lc);

            Sentencia raipzC = cp.parse();

            //raipzC.SentValSemantica();
            //raipzC.interpretar();

            Conexion.InsertarEvento(HttpContext.Current.Session["UsuarioNick"].ToString(), "C",
                                    HttpContext.Current.Session["MsjC"].ToString(),sentencia);

            return HttpContext.Current.Session["MsjC"].ToString();
        }


        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoJava(string sentencia)
        {
            HttpContext.Current.Session["MsjJava"] = string.Empty;
            var lj = new LexicoJava(sentencia);
            var jp = new javaParser(lj);

            Sentencia raizJ = jp.parse();

            //raizJ.SentValSemantica();
            //raizJ.interpretar();

            Conexion.InsertarEvento(HttpContext.Current.Session["UsuarioNick"].ToString(), "Java",
                                    HttpContext.Current.Session["MsjJava"].ToString(),sentencia);

            return HttpContext.Current.Session["MsjJava"].ToString();
           
        }

        protected void BtnCerrarSesionOnClick(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Redirect("Login.aspx");
        }
    }
}
