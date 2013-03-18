using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Lexico;
using WebUI.Sintactico;

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

        protected void BtnCompilarOnClick(object sender, EventArgs e)
        {
            //Evaluo si el textbox no esta vacio
            if (!string.IsNullOrEmpty(txtContenido.Value))
            {
                
            }
            else
            {
                txtResultado.Value = "No se ingreso ninguna sentencia en la caja de texto!!!.";
            }
        }

        protected void EvaluarSentencia()
        {
            switch (cbLenguaje.Value)
            {
                case "Java":

                    var lj = new LexicoJava(txtContenido.Value);
                    var jp = new javaParser(lj);

                    var raiz = jp.parse();
                    raiz.SentValSemantica();

                    break;

                case "C":

                    var lc = new LexicoC(txtContenido.Value);

                    break;

                case "Pascal":

                    var lp = new LexicoPascal(txtContenido.Value);

                    break;

                default:
                    break;

            }
        }
    }
}
