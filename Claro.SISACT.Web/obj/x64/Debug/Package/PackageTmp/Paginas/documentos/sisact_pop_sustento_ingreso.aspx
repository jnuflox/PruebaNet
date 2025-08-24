<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_sustento_ingreso.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.documentos.sisact_pop_sustento_ingreso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sustento de Ingreso</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript">

        top.window.moveTo(0, 0);
        if (document.all) {
            top.window.resizeTo(screen.availWidth, screen.availHeight);
        } else if (document.layers || document.getElementById) {
            if (top.window.outerHeight < screen.availHeight || top.window.outerWidth < screen.availWidth) {
                top.window.outerHeight = screen.availHeight;
                top.window.outerWidth = screen.availWidth;
            }
        }

        function uploadArchivo() {
            var url = '../documentos/sisact_pop_upload_consumer.aspx?';
            abrirVentana(url, "", '450', '120', '_blank', true);
        }

        function grabar() {

            if (getValue('hidListaArchivos') == '') {
                alert("La solicitud Nº " + getValue('txtCodSolicitud') + " Rechazada por no adjuntar Sustento de ingreso");
                setValue('hidProceso', 'R');
                frmPrincipal.submit();
                window.close();
                return;
            }

            if (!confirm("Se grabaran los datos ingresados. ¿Desea continuar?")) {
                return false;
            }

            setValue('hidProceso', 'A');
            frmPrincipal.submit();
            window.close();
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
                <table class="Contenido" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td class="Arial12B" height="20">
                            &nbsp;N° Evaluación:&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtCodSolicitud" Style="text-transform: uppercase" ReadOnly="true"
                                runat="server" CssClass="clsInputDisable" Width="60px" onpaste="return false;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" width="100%" border="0">
                    <tr>
                        <td>
                            <img height="1" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial12B">
                            &nbsp;Código de Solicitud :
                            <asp:Label ID="lblNroSEC" runat="server" CssClass="clsSoloLectura"></asp:Label>
                        </td>
                        <td class="Arial12B">
                            Tipo de Documento :
                            <asp:Label ID="lblTipoDoc" runat="server" CssClass="clsSoloLectura"></asp:Label>
                        </td>
                        <td class="Arial12B">
                            Número de Documento :
                            <asp:Label ID="lblNroDoc" runat="server" CssClass="clsSoloLectura"></asp:Label>
                        </td>
                        <td class="Arial12B">
                            Nombres y Apellidos :
                            <asp:Label ID="lblRazon" runat="server" CssClass="clsSoloLectura"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" width="100%" border="0">
                    <tr>
                        <td class="Header" align="center" height="20">
                            &nbsp;Documentos Adjuntos
                        </td>
                    </tr>
                </table>
                <table class="Contenido" cellspacing="2" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td style="display: none">
                            <asp:Button ID="btnCargarArchivo" runat="server" CssClass="Boton" onmouseover="this.className='BotonResaltado';"
                                Style="width: 100px; cursor: hand; height: 19px" onmouseout="this.className='Boton';"
                                Text="Cargar Archivo" OnClick="btnCargarArchivo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="udpGrilla" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <input id="hidListaArchivos" style="width: 1px; height: 22px" type="hidden" runat="server" />
                                    <asp:GridView ID="gvArchivo" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                        DataKeyNames="Codigo,Descripcion" OnRowDeleting="gvArchivo_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="true" CssClass="TablaTitulos"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Width="25px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgEliminar" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageUrl="~/Imagenes/close.gif" Text="Delete" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro de eliminar este elemento?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="true" CssClass="TablaTitulos"></HeaderStyle>
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
                        <td height="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <input class="Boton" id="btnAgregar" onmouseover="this.className='BotonResaltado';"
                                style="width: 126px; cursor: hand; height: 19px" onclick="uploadArchivo();" onmouseout="this.className='Boton';"
                                type="button" value="Agregar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <input class="Boton" id="btnGrabar" style="width: 126px; cursor: hand; height: 19px"
                    onclick="grabar();" type="button" value="Aceptar" />
                <input class="Boton" id="btnCancelar" style="width: 126px; cursor: hand; height: 19px"
                    onclick="grabar();" type="button" value="Cancelar" />
            </td>
        </tr>
    </table>
    <input id="hidProceso" style="width: 1px; height: 22px" type="hidden" runat="server" />
    </form>
</body>
</html>
