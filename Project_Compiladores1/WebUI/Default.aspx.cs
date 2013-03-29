using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Lexico;
using WebUI.Sintactico;
using System.Web.Services;

namespace WebUI
{
    public partial class _Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {           
            if(IsPostBack)
            {
                
            }
        }

        //protected void BtnCompilarOnClick(object sender, EventArgs e)
        //{
        //    //Evaluo si el textbox no esta vacio
        //    //if (!string.IsNullOrEmpty(txtContenido.Value))
        //    //{
        //    //    EvaluarSentencia();
        //    //}
        //    //else
        //    //{
        //    //    txtResultado.Value = "No se ingreso ninguna sentencia en la caja de texto!!!.";
        //    //}
        //}

        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoPascal(string sentencia)
        {
            HttpContext.Current.Session["MsjPascal"] = string.Empty;

            var lp = new LexicoPascal(sentencia);
            var pp = new pascalParser(lp);

            var raizP = pp.parse();

            return HttpContext.Current.Session["MsjPascal"].ToString();
        }


        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoC(string sentencia)
        {          
            HttpContext.Current.Session["MsjC"] = string.Empty;

            var lc = new LexicoC(sentencia);
            var cp = new parserC(lc);

            var raizC = cp.parse();

            return HttpContext.Current.Session["MsjC"].ToString();
        }


        [WebMethod(EnableSession = true)]
        public static string ObtenerResultadoJava(string sentencia)
        {
            HttpContext.Current.Session["MsjJava"] = string.Empty;
            var lj = new LexicoJava(sentencia);
            var jp = new javaParser(lj);

            var raizJ = jp.parse();
            return HttpContext.Current.Session["MsjJava"].ToString();
           
        }
    }
}
