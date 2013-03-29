<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebUI._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Encabezado" runat="server">
    <script type="text/c#" runat="server">
        [System.Web.Services.WebMethod]
        public static string test()
        {
            return "Sexo!!!!!!!!!!!!!!";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <%--header--%>
    <div class="navbar navbar-static-top">
        <div class="navbar-inner">
            <div class="container">
                <div class="row-fluid">
                    <div class="span3">
                        <div class="brand" href="http://www.unitec.edu">
                            <img src="img/UnitecLogo250.png" width="310" height="94" alt="" />
                        </div>
                    </div>
                    <div class="span9" align="right">
                        <div style="margin: 30px 3px 30px 30px;">
                            <div>
                                <h4>
                                    Multi-Interpretador - Proyecto Compiladores I</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--Contenido--%>
    <div class="container">
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="tabbable tabs-stacked">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="#pane1" data-toggle="tab">Lenguaje Java</a></li>
                        <li><a href="#pane2" data-toggle="tab">Lenguaje C</a></li>
                        <li><a href="#pane3" data-toggle="tab">Lenguaje Pascal</a></li>
                    </ul>
                    <div class="tab-content">
                        <div id="pane1" class="tab-pane active">
                            <div class="container">
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
                                        class="span12" rows="10"></textarea>
                                </div>
                            </div>
                        </div>
                        <div id="pane2" class="tab-pane">
                            <div class="container">
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
                                    <textarea id="txtResultadoC" disabled="disabled" runat="server" placeholder="El Resultado se muestra aqui!"
                                        class="span12" rows="10"></textarea>
                                </div>
                            </div>
                        </div>
                        <div id="pane3" class="tab-pane">
                            <div class="container">
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
                                    <textarea id="txtResultadoPascal" disabled="disabled" runat="server" placeholder="El Resultado se muestra aqui!"
                                        class="span12" rows="10"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
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
