<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebUI._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <div class="container">
        <legend>Multi-Interpretador</legend>

        <div class="row-fluid" >
            Seleccione un Lenguaje :

            <select id="cbLenguaje" class="span2">
                <option value="Java" selected="selected">Java</option>
                <option value="Pascal" >Pascal</option>
                <option value="C">C</option>
            </select>
        </div>

        <div class="row-fluid">
            <textarea id="txtContenido" placeholder="Ingrese una sentencia aqui!" class="span12" rows="10"></textarea>
        </div>
        
        <div class="row-fluid">
            <asp:Button ID="btnCompilar" CssClass="btn btn-primary btn-large" runat="server" Text="Compilar Sentencia" />

        </div>
        <br/>
        <div class="row-fluid">
            <textarea id="txtResultado" placeholder="El Resultado se muestra aqui!" class="span12" rows="10"></textarea>
        </div>
    </div>
</asp:Content>
