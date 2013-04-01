<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="WebUI.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <br />
    <div class="container">
        <div class="row-fluid">
            <div class="row-fluid">
                <div class="span4" style="padding: 10px">
                    <div class="thumbnail" style="padding: 20px; background-color: #FFFFFF;">
                        <legend>Inicio de Sesión </legend>
                        <label id="username_label">
                            Usuario</label>
                        <input type="text" id="username" runat="server" minlenght="4" required="required"
                            class="input-block-level" />
                        <label id="password_label">
                            Contraseña</label>
                        <input type="password" id="password" runat="server" required="required" class=" input-block-level" />
                        <br />
                        <label class="label label-important" id="msg" visible="False" runat="server" style="color: white;
                            font-size: x-small; font-weight: normal; margin-right: 3px; font-family: Arial, Helvetica, sans-serif;">
                        </label>
                        <br />
                        <div class="row-fluid">
                            <div class="span8">
                                <asp:HyperLink ID="HyperLink1"  runat="server" NavigateUrl="~/Registro.aspx">Registrate!</asp:HyperLink>
                            </div>
                            <div class="span4">
                                <asp:Button ID="BtnIngresar" CssClass="btn btn-primary" OnClick="BtnIngresarOnClick"
                                    runat="server" Text="Ingresar" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="span8">
                    <legend>
                        <h1>
                            Bienvenido!</h1>
                    </legend>
                    <br />
                    <blockquote>
                        <p>
                            <strong>Estimado Usuario te damos la cordial bienvenida
                                a esta herramienta educativa!</strong></p>
                        <br />
                        <p>
                            <strong>Objetivos del Proyecto</strong></p>
                        <small>Conocer de forma rapida la sintaxis de los tres lenguajes soportados: Java, C y Pascal.</small>
                        <small>Realizar pruebas rapidas de algunas Operaciones.</small>
                        <small>Aprender interactuando.</small>
                    </blockquote>
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
