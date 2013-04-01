using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            btnSave.ServerClick += btnSave_ServerClick;
            
            if (IsPostBack) return;

            SetCaptchaText();
        }
        private void SetCaptchaText()
        {
            var oRandom = new Random();
            var iNumber = oRandom.Next(100000, 999999);
            Session["Captcha"] = iNumber.ToString();
            var sCaptchaText = Context.Session["Captcha"].ToString();
        }
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            if (Session["Captcha"].ToString() != txtCaptcha.Value.Trim())
            {
                msj.Visible = true;
                msj.Text = "ERROR : No se Ingreso la secuencia Correcta!";
            }
            else
            {
                //Valido la consistencia de las contraseñas
                if (txtUsuarioContrasena.Value.Equals(txtUsuarioContrasenaC.Value))
                {
                    //Valido la consistencia de los correos
                    if (txtUsuarioEmail.Value.Equals(txtUsuarioEmailC.Value))
                    {
                        //Validamos si el usuario ya existe
                        if (Conexion.ValidarUsuario(txtUsuarioId.Value,EncriptacionMD5.CreateMd5Hash(txtUsuarioContrasena.Value)).Equals(
                                false))
                            {
                                    try
                                    {
                                        Conexion.InsertarUsuario(txtUsuarioId.Value,txtUsuarioContrasena.Value,
                                                                                    txtUsuarioNombre.Value,1,
                                                                                    txtUsuarioEmail.Value
                                                                                    );

                                        Response.Redirect("~/Login.aspx");
                                    }
                                    catch (Exception)
                                    {
                                        msj.Visible = true;
                                        msj.Text =
                                            "Se produjo un error en el Registro!";
                                    }
                                }
                            
                          
                        else
                        {
                            msj.Visible = true;
                            msj.Text =
                                "Ya Existe el Usuario.";
                        }
                    }
                    else
                    {
                        msj.Visible = true;
                        msj.Text = "ERROR : No hay consistencia entre los correo ingresados!, vuelva a Ingresarlos!";
                    }
                }
                else
                {
                    msj.Visible = true;
                    msj.Text = "ERROR : No hay consistencia entre las contraseñas ingresadas!, vuelva a Ingresarlas!";
                }
            }
        }
    }
}