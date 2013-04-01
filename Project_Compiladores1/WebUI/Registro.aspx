<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Registro.aspx.cs" Inherits="WebUI.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <div class="row-fluid">
            <div class="span12">
                <div class="form-horizontal">
                    <div class="row-fluid">
                        <div class="span12">
                            <div class="span8">
                                <p style="padding: 0px 15px 15px 15px">
                                    <strong>Nota : </strong>Para crear una cuenta de Usuario en Línea debe de tomar
                                    en cuenta lo siguiente:
                                </p>
                                <ul style="padding: 0px 15px 15px 15px">
                                    <li>1. La contraseña debe ser de 8 caracteres mínimo.</li>
                                    <li>2. Contener al menos una letra mayúscula.</li>
                                    <li>3. Contener un Caracter especial.</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div style="padding-right: 15px; padding-bottom: 15px; padding-left: 15px" align="center">
                        <asp:Label ID="msj" CssClass="label label-important" runat="server" Visible="False"></asp:Label>
                    </div>
                    <br />
                    <div class="row-fluid">
                        <div class="span5">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioId">
                                    Usuario</label>
                                <div class="controls">
                                    <input type="text" required="required" id="txtUsuarioId" name="txtUsuarioId" minlength="4"
                                        runat="server" title="Asegúrese de escribir un ID que no se le olvide, para ingresar posteriormente al sitio" />
                                </div>
                            </div>
                        </div>
                        <div class="span4">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioNombre">
                                    Nombre</label>
                                <div class="controls">
                                    <input type="text" required="required" id="txtUsuarioNombre" name="txtUsuarioNombre" minlength="45"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span5">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioContrasena">
                                    Contraseña</label>
                                <div class="controls">
                                    <input type="password" required="required" id="txtUsuarioContrasena" name="txtUsuarioContrasena"
                                        runat="server" pattern="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                        title="La contraseña debe ser de 8 caracteres mínimo, contener al menos una letra mayúscula y un número o caracter especial ej: Seguro#1" />
                                </div>
                            </div>
                        </div>
                        <div class="span4">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioContrasenaC">
                                    Confirmar contraseña</label>
                                <div class="controls">
                                    <input type="password" required="required" id="txtUsuarioContrasenaC" name="txtUsuarioContrasenaC"
                                        runat="server" pattern="(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                        title="La contraseña debe ser igual a la que ingreso en la casilla anterior" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span5">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioEmail">
                                    Email</label>
                                <div class="controls">
                                    <input type="text" required="required" id="txtUsuarioEmail" name="txtUsuarioEmail"
                                        minlength="4" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="span4">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioEmailC">
                                    Confirmar Email</label>
                                <div class="controls">
                                    <input type="text" required="required" id="txtUsuarioEmailC" name="txtUsuarioEmailC"
                                        minlength="4" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row-fluid">
                        <div class="span5">
                            <div class="control-group">
                                <label class="control-label" for="txtUsuarioContrasena">
                                    Ingresa la Secuencia</label>
                                <div class="controls" align="center">
                                    <input type="text" required="required" id="txtCaptcha" name="txtCaptcha" runat="server"
                                        title="Ingrese Numeros nada mas!" />
                                    <asp:Image ID="imgCaptcha" ImageUrl="Captcha.ashx" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div align="center">
                    <br />
                    <input type="submit" class="btn btn-primary btn-large" value="Enviar Solicitud" runat="server"
                        id="btnSave" />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx"> Salir...</asp:HyperLink>
                    <br />
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
