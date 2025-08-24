<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_portabilidad.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.frames.sisact_ifr_portabilidad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js" ></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="JavaScript">

        function validar() {
            if (txtNumContacto.value.length < 7) {
                txtNumContacto.focus();
                alert('Ingrese un número de contacto válido.');
                return false;
            }
        }

        function listarOperadorCedente(tipoPortabilidad) {
            PageMethods.CargarOperadorCedente(tipoPortabilidad, cargarOperadorCedente);
        }

        function cargarOperadorCedente(objResponse) {
            llenarCombo(document.getElementById('ddlOperadorCedente'), objResponse.Cadena, true);
        }

        function verArchivo(archivo) {
            var url = '../documentos/sisact_pop_ver_documento.aspx?archivo=' + archivo;
            abrirVentana(url, "", '700', '600', '_blank', true);
        }

        function uploadArchivo() {
            var url = '../documentos/sisact_pop_upload_portabilidad.aspx?';
            abrirVentana(url, "", '490', '150', '_blank', true);
        }

    </script>
</head>
<body style="margin: 0px;">
    <form id="frmPrincipal" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td style="width: 35%">
                <table cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr style="display: none">
                        <td class="Arial10B" width="250">
                            &nbsp;Modalidad: (*)
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlModalidadPorta" runat="server" Width="180px" CssClass="clsSelectEnable0">
                                <asp:ListItem Value="">SELECCIONE...</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="Arial10B" width="250">
                            &nbsp;Operador cedente: (*)
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperadorCedente" runat="server" CssClass="clsSelectEnable0"
                                Width="180px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Nro de folio de formato de portabilidad:
                        </td>
                        <td>
                            <input class="clsInputEnable" id="txtNroFolio" maxlength="7" size="18" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="250">
                            &nbsp;Número de contacto: (*)
                        </td>
                        <td>
                            <input class="clsInputEnable" id="txtNumContacto" maxlength="10" size="18" onkeypress="validaCaracteres('0123456789');"
                                runat="server" />
                        </td>
                    </tr>
                    <!-- PROY-26358 Inicio - Evalenzs - trAdjuntarArchivo Display:none   -->
                    <tr id ="trAdjuntarArchivo"  style="DISPLAY: none"   runat="server">
                        <td class="Arial10B" width="250">
                            &nbsp;Adjuntar archivo: (*)
                        </td>
                        <td>
                            <input class="Boton" id="btnExaminar" style="width: 100px; cursor: hand; height: 19px"
                                onclick="uploadArchivo()" type="button" value="Agregar" />
                        </td>
                    </tr>
                    <!-- PROY-26358 Fin - Evalenzs -->
                </table>
            </td>
            <td style="width: 5%">
                &nbsp;
            </td>
            <td style="width: 60%">
                <div class="clsGridRow" style="width: 75%">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tr>
                            <td style="display: none" colspan="2">
                                <asp:Button ID="btnCargarArchivo" runat="server" CssClass="Boton" Style="width: 100px;
                                    cursor: hand; height: 19px" Text="Cargar Archivo" OnClick="btnCargarArchivo_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="udpGrilla" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <input id="hidListaArchivos" style="width: 1px; height: 22px" type="hidden" runat="server" />
                                        <asp:GridView ID="gvArchivo" runat="server" AutoGenerateColumns="False" BorderColor="#95B7F3"
                                            CellPadding="1" CellSpacing="1" HeaderStyle-CssClass="TablaTitulos" Width="98%"
                                            DataKeyNames="ARCH_CODIGO" OnRowDeleting="gvArchivo_RowDeleting">
                                            <RowStyle CssClass="TablaFilasGrid"></RowStyle>
                                            <HeaderStyle CssClass="TablaTitulos"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ARCH_CODIGO" HeaderText="Codigo"></asp:BoundField>
                                                <asp:BoundField DataField="ARCH_NOMBRE" HeaderText="Archivo"></asp:BoundField>
                                                <asp:BoundField DataField="ARCH_DESCRIPCION" HeaderText="Documento"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Acci&#243;n">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <img id="imgVer" src="../../Imagenes/botones/btn_VerArchivo.gif" alt="Ver Archivo"
                                                            onclick='verArchivo("<%# DataBinder.Eval(Container.DataItem, "ARCH_RUTA")%>")' style="cursor: hand;" />
                                                        <asp:ImageButton ID="imgEliminar" runat="server" CausesValidation="False" CommandName="Delete"
                                                            ImageUrl="~/Imagenes/rechazar.gif" Text="Delete" ToolTip="Eliminar Archivo" OnClientClick="return confirm('¿Está seguro de eliminar este elemento?');" />
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
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
