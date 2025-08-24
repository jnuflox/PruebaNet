<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_lineas_desactivas.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_lineas_desactivas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle Líneas Desactivas</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">

        function verDetalle() {
            var url = '../consultas/sisact_pop_lineas_desactivas.aspx?';
            url += 'flgConsultaPopup=S';
            url += '&flgConsultaEval=N';
            url += "&nroDoc=" + window.parent.getValue('txtNroDoc');
            window.showModalDialog(url, 'lineas_desactivas', 'dialogHeight: 400px; dialogWidth: 720px; edge: Raised; center:Yes; help: No; resizable=yes; status: No');
        }
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table cellspacing="2" cellpadding="0" width="100%" border="0">
        <tr id="trLineasDesactivas">
            <td>
                <table cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Header" align="left" height="19">
                            &nbsp;Líneas Desactivas y/o con Bloqueo
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="1" border="0">
                    <tr id="trDetalleBSCS" runat="server">
                        <td class="tr_activo" align="center">
                            Detalle BSCS
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvDetalleBSCS" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                OnRowDataBound="gvDetalleBSCS_RowDataBound">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                                <Columns>
                                    <asp:BoundField DataField="numeroLinea" HeaderText="Número">
                                        <ItemStyle Wrap="False" Width="70px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tipoProducto" HeaderText="Tipo Producto">
                                        <ItemStyle Wrap="False" Width="80px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estadoLinea" HeaderText="Estado de la Línea" Visible="False">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="motivoDesactivacion" HeaderText="Motivo">
                                        <ItemStyle Wrap="False" Width="120px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaActivacion" HeaderText="Fecha de Activación">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaDesactivacion" HeaderText="Fecha de Desactivación/Bloqueo">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vigenciaApadece" HeaderText="Vigencia APADECE" Visible="False">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="recomendacion" HeaderText="Recomendación">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="textgrilla"></PagerStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label class="Arial10B" ID="lblCantLineaBSCS" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trDetalleSGA" runat="server">
                        <td class="tr_activo" align="center">
                            Detalle SGA
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvDetalleSGA" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                OnRowDataBound="gvDetalleSGA_RowDataBound">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10" />
                                <Columns>
                                    <asp:BoundField DataField="numeroLinea" HeaderText="Número">
                                        <ItemStyle Wrap="False" Width="70px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tipoServicio" HeaderText="Tipo Servicio">
                                        <ItemStyle Wrap="False" Width="80px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="agrupacionPaquete" HeaderText="Solución">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estadoServicioLinea" HeaderText="Estado del Servicio"
                                        Visible="False">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="motivoDesactivacion" HeaderText="Motivo">
                                        <ItemStyle Wrap="False" Width="120px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaActivacion" HeaderText="Fecha de Activación">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaDesactivacion" HeaderText="Fecha de Desactivación">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="recomendacion" HeaderText="Recomendación">
                                        <ItemStyle Wrap="False" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="textgrilla"></PagerStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label class="Arial10B" ID="lblCantLineaSGA" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trDetalleMasPlanes" runat="server">
                        <td class="Arial10BRed">
                            Más Planes.&nbsp;<a class="clsSobreLink" id="ahrAvisoMasPlanes" onclick="verDetalle();">Ver
                                Detalle</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trBotonCerrar" runat="server">
            <td align="center" height="30">
                <input class="Boton" id="btnCerrar" style="width: 100px; cursor: hand; height: 19px"
                    onclick="window.close();" type="button" value="Cerrar" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
