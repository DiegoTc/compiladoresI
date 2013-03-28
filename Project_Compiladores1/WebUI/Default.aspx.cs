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
            //if (!string.IsNullOrEmpty(txtContenido.Value))
            //{
            //    EvaluarSentencia();
            //}
            //else
            //{
            //    txtResultado.Value = "No se ingreso ninguna sentencia en la caja de texto!!!.";
            //}
        }

        protected void EvaluarSentencia()
        {
            //switch (cbLenguaje.Value)
            //{
            //    case "Java":

            //        //Declaro la variable de sesion para el capturar el mensaje del analisis sintactico
            //        Session["MsjJava"] = string.Empty;

            //        var lj = new LexicoJava(txtContenido.Value);
            //        var jp = new javaParser(lj);
                    
            //        var raizJ = jp.parse();
            //        //raiz.SentValSemantica();

            //        txtResultado.Value = Session["MsjJava"].ToString();
                    
            //        break;
                                                                                                                                                                                                                                                                                                                                                                                                                                          
            //    case "C":

            //        //Declaro la variable de sesion para el capturar el mensaje del analisis sintactico
            //        Session["MsjC"] = string.Empty;

            //        var lc = new LexicoC(txtContenido.Value);
            //        var cp = new parserC(lc);

            //        var raizC = cp.parse();

            //        txtResultado.Value = Session["MsjC"].ToString();

            //        break;

            //    case "Pascal":

            //        //Declaro la variable de sesion para el capturar el mensaje del analisis sintactico
            //        Session["MsjPascal"] = string.Empty;
                                        
            //        var lp = new LexicoPascal(txtContenido.Value);
            //        var pp = new pascalParser(lp);

            //        var raizP = pp.parse();

            //        txtResultado.Value = Session["MsjPascal"].ToString();


            //        break;

            //    default:
            //        break;

            //}
        }
    }
}
