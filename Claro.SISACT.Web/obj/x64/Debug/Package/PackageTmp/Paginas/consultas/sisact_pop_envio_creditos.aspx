<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_envio_creditos.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_envio_creditos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Envio a Créditos</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript">

        function uploadArchivo() {
            var url = '../documentos/sisact_pop_upload_consumer.aspx?';
            abrirVentana(url, "", '450', '120', '_blank', true);
        }

        function grabar() {
            var vRetorno = new Object();
            if (confirm("Esta Seguro de Grabar la Evaluacion?")) {
                vRetorno.Archivos = getValue('hidListaArchivos');
                vRetorno.Comentario = document.getElementById('txtComentarioPDV').value.toUpperCase();
                window.returnValue = vRetorno;
                window.close();
            }
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table id="tblEvaluacion" cellspacing="1" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Header" align="left" height="20">
                            &nbsp;Comentarios del Punto de Venta
                        </td>
                    </tr>
                </table>
                <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" valign="top" width="150">
                            &nbsp;Comentario :<br />
                            <span style="color: #ff0000">&nbsp;(Max 200 Caracteres)</span>
                        </td>
                        <td class="Arial10B">
                            <asp:TextBox ID="txtComentarioPDV" onblur="return validaTextAreaLongitud(this, 200, true);"
                                Style="text-transform: uppercase" runat="server" CssClass="inputTextArea" Width="80%"
                                TextMode="MultiLine" Rows="5" onpaste="return false;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Header" align="center" height="20">
                            &nbsp;Documentos Adjuntos
                        </td>
                    </tr>
                </table>
                <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td style="display: none">
                            <asp:Button ID="btnCargarArchivo" runat="server" CssClass="Boton" Style="width: 100px;
                                cursor: hand; height: 19px" Text="Cargar Archivo" OnClick="btnCargarArchivo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="udpGrilla" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <input id="hidListaArchivos" style="width: 1px; height: 22px" type="hidden" runat="server" />
                                    <asp:GridView ID="gvArchivo" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                        DataKeyNames="Codigo,Descripcion" OnRowDeleting="gvArchivo_RowDeleting">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemStyle HorizontalAlign="Left" Width="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgEliminar" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageUrl="~/Imagenes/close.gif" Text="Delete" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro de eliminar este elemento?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemStyle HorizontalAlign="Left" Width="100%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNomArchivo" runat="server" CssClass="Arial10B" Text='<%# DataBinder.Eval(Container.DataItem, "Descripcion")%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCargarArchivo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="30">
                            <input class="Boton" id="btnAgregar" style="width: 126px; cursor: hand; height: 19px"
                                onclick="uploadArchivo();" type="button" value="Adjuntar archivos" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" height="30">
                <input class="Boton" id="btnGrabar" style="width: 126px; cursor: hand; height: 19px"
                    onclick="grabar();" type="button" value="Aceptar" />
                <input class="Boton" id="btnCancelar" style="width: 126px; cursor: hand; height: 19px"
                    onclick="window.close();" type="button" value="Cancelar" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
