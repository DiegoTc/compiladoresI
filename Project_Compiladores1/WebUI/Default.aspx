﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebUI._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
    <script type="text/javascript">
        function ObtenerResultadoJavaClient() {
            var sentencia = editor.getValue();
            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/Default.aspx/ObtenerResultadoJava") %>',
                data: JSON.stringify({ sentencia: sentencia }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    //editor.setValue(msg.d)
                    txtResultadoJava.value = msg.d
                }
            });

        }


        function ObtenerResultadoCClient() {
            var sentencia = editor2.getValue();
            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/Default.aspx/ObtenerResultadoC") %>',
                data: JSON.stringify({ sentencia: sentencia }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    //editor.setValue(msg.d)
                    txtResultadoC.value = msg.d
                }
            });
        }


        function ObtenerResultadoPascalClient() {
            var sentencia = editor3.getValue();
            $.ajax({
                type: 'POST',
                url: '<%= ResolveUrl("~/Default.aspx/ObtenerResultadoPascal") %>',
                data: JSON.stringify({ sentencia: sentencia }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    //editor.setValue(msg.d)
                    txtResultadoPascal.value = msg.d
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <%--Contenido--%>
    <div class="container">
        <div class="row-fluid">
            <div class="span12">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="span8">
                            <div class="tabbable tabs-stacked">
                                <ul class="nav nav-tabs">
                                    <li class="active"><a href="#pane1" data-toggle="tab">Lenguaje Java</a></li>
                                    <li><a href="#pane2" data-toggle="tab">Lenguaje C</a></li>
                                    <li><a href="#pane3" data-toggle="tab">Lenguaje Pascal</a></li>
                                </ul>
                                <div class="tab-content">
                                    <div id="pane1" class="tab-pane active">
                                        <div class="row-fluid">
                                            <div class="row-fluid">
                                                <textarea id="codejava" name="codec" class="span12" rows="6"></textarea>
                                            </div>
                                            <br />
                                            <script type="text/javascript">
                                                var editor = CodeMirror.fromTextArea(document.getElementById("codejava"), {
                                                    lineNumbers: true,
                                                    matchBrackets: true,
                                                    mode: "text/x-java"
                                                });
                                            </script>
                                            <div class="row-fluid">
                                                <input type="button" class="btn btn-primary" id="botonCompilar" value="Compilar Sentencia Java"
                                                    onclick="ObtenerResultadoJavaClient()" />
                                            </div>
                                            <br />
                                            <div class="row-fluid">
                                                <textarea id="txtResultadoJava" disabled="disabled" placeholder="El Resultado se muestra aqui!"
                                                    class="span12" rows="5"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pane2" class="tab-pane">
                                        <div class="row-fluid">
                                            <div class="row-fluid">
                                                <textarea id="codec" name="codec" class="span12" rows="6"></textarea>
                                            </div>
                                            <br />
                                            <script type="text/javascript">
                                                var editor2 = CodeMirror.fromTextArea(document.getElementById("codec"), {
                                                    lineNumbers: true,
                                                    matchBrackets: true,
                                                    mode: "text/x-csharp"
                                                });
                                            </script>
                                            <div class="row-fluid">
                                                <input type="button" class="btn btn-primary" id="BtnCompilarC" value="Compilar Sentencia C"
                                                    onclick="ObtenerResultadoCClient()" />
                                            </div>
                                            <br />
                                            <div class="row-fluid">
                                                <textarea id="txtResultadoC" disabled="True" placeholder="El Resultado se muestra aqui!"
                                                    class="span12" rows="5"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pane3" class="tab-pane">
                                        <div class="row-fluid">
                                            <div class="row-fluid">
                                                <textarea id="codepascal" name="codepascal" class="span12" rows="6"></textarea>
                                            </div>
                                            <br />
                                            <script type="text/javascript">
                                                var editor3 = CodeMirror.fromTextArea(document.getElementById("codepascal"), {
                                                    lineNumbers: true,
                                                    matchBrackets: true,
                                                    mode: "text/x-pascal"
                                                });
                                            </script>
                                            <div class="row-fluid">
                                                <input type="button" class="btn btn-primary" id="BtnCompilarPascal" value="Compilar Sentencia Pascal"
                                                    onclick="ObtenerResultadoPascalClient()" />
                                            </div>
                                            <br />
                                            <div class="row-fluid">
                                                <textarea id="txtResultadoPascal" disabled="True" placeholder="El Resultado se muestra aqui!"
                                                    class="span12" rows="5"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span4">
                            <div style="text-align: center">
                                <div class="table">
                                    <tbody>
                                        <tr>
                                            <div class="span4">
                                                <asp:Image runat="server" ID="Image1" ImageUrl="img/USER.png" CssClass="img-rounded" />
                                            </div>
                                            <div class="span4">
                                                <h4 align="left">
                                                    Bienvenido</h4>
                                            </div>
                                            <div class="span4">
                                                <asp:Button runat="server" ID="BtnCerrarCesion" CssClass="btn btn-warning" Text="Salir"
                                                    OnClick="BtnCerrarSesionOnClick" />
                                            </div>
                                        </tr>
                                    </tbody>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <table class="table table-striped table-bordered">
                                <tr>
                                    <td style="text-align: right">
                                        Perfil
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblPerfil">
                                            
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        Nombre
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNombre">
                                            
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <td>Integrantes:</td>
                                    </tr>
                                    <tr>
                                        <td>- Diego Turcios</td>
                                    </tr>
                                    <tr>
                                        <td>- Cristian Pineda</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>- Erick Viera</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>- Noel Recarte</td>
                                    </tr>
                                </tbody>
                            </table>
                            <div style="text-align: center;" ><img src="img/LogoJaguares.png"/></div>
                            
                           
                        </div>
                        <div class="row-fluid">
                            <div class="span12">
                                <label id="lblhdd">
                                    Bitácora de Pruebas</label>
                                <div class="row-fluid">
                                    <asp:GridView ID="gvBitacora" DataKeyNames="BitacoraId" AutoGenerateColumns="False"
                                        BorderStyle="None" class="table table-striped table-bordered" runat="server">
                                        <Columns>
                                            <%--ID--%>
                                            <asp:TemplateField HeaderText="ID" Visible="false" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("BitacoraId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Sentencia--%>
                                            <asp:TemplateField HeaderText="Sentencia" Visible="true" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSentencia" runat="server" Text='<%# Bind("BitacoraSentencia") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Lenguaje--%>
                                            <asp:TemplateField HeaderText="Lenguaje">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsuarioNick" runat="server" Text='<%#Eval("BitacoraLenguaje")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Evento--%>
                                            <asp:TemplateField HeaderText="Evento">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsuarioNombre" runat="server" Text='<%#Eval("BitacoraDescripcionEvento")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <label class="label label-important" id="msgTransac" visible="False" runat="server"
                                        style="color: white; font-size: x-small; font-weight: normal; margin-right: 3px;
                                        font-family: Arial, Helvetica, sans-serif;">
                                    </label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <br />
</asp:Content>
