<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_consulta_detalle_plan.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_consulta_detalle_plan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sisact_consulta_detalle_plan</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="javascript">

        function mostrarServicios(idSol, orden, idProducto) {
            document.getElementById('lblOrden').innerText = 'Nº ' + orden;
            var selObjA = document.getElementById('lbxServicios');
            selObjA.length = 0;

            var serviciosAgregados = getValue('hidCadenaServicios').split('|');

            for (var i = 0; i < serviciosAgregados.length - 1; i++) {
                if (serviciosAgregados[i].split(';')[0] == idSol) {
                    var opcion = serviciosAgregados[i].split(';')[1];
                    var opcion2 = serviciosAgregados[i].split(';')[2];
                    addOption(selObjA, opcion, opcion2);
                }
            }

            tblServicio.style.display = 'inline';
            tblEquipo.style.display = 'none';
            tblCuotas.style.display = 'none';
        }

        function mostrarServiciosHFC(idSol, idFila, orden) {

            document.getElementById('lblOrden').innerText = 'Nº ' + orden;
            var selObjA = document.getElementById('lbxServicios');
            selObjA.length = 0;

            var serviciosAgregados = getValue('hidCadenaServicios').split('|');

            for (var i = 0; i < serviciosAgregados.length - 1; i++) {
                if (serviciosAgregados[i].split(';')[0] == idSol && serviciosAgregados[i].split(';')[1] == idFila) {
                    var opcion = serviciosAgregados[i].split(';')[2];
                    var opcion2 = serviciosAgregados[i].split(';')[3];
                    addOption(selObjA, opcion, opcion2);
                }
            }

            tblServicio.style.display = 'inline';
            tblEquipo.style.display = 'none';
            tblCuotas.style.display = 'none';
        }

        function mostrarEquipos(idSol) {

            var selObjA = document.getElementById('lbxEquipos');
            selObjA.length = 0;

            var equiposAgregados = getValue('hidCadenaEquiposHFC').split('|');

            for (var i = 0; i < equiposAgregados.length - 1; i++) {
                if (equiposAgregados[i].split(';')[0] == idSol) {
                    var opcion = equiposAgregados[i].split(';')[1];
                    var opcion2 = equiposAgregados[i].split(';')[2];
                    addOption(selObjA, opcion, opcion2);
                }
            }

            tblServicio.style.display = 'none';
            tblEquipo.style.display = 'inline';
            tblCuotas.style.display = 'none';
        }
        
        function cerrarServicio() {
            tblServicio.style.display = 'none';
        }

        function cerrarEquipo() {
            tblEquipo.style.display = 'none';
        }

        function mostrarCuotas(idSol) {
            var arrCuotas = document.getElementById('hidCadenaCuotas').value.split('|');

            for (var i = 0; i < arrCuotas.length; i++) {
                if (arrCuotas[i] != '') {
                    var arrCuota = arrCuotas[i].split(';');
                    var idSol1 = arrCuota[4];

                    if (idSol == idSol1) {
                        document.getElementById('txtNroCuota').value = arrCuota[0];
                        document.getElementById('txtPorcCuotaIni').value = (parseFloat(arrCuota[1])).toFixed(2);
                        document.getElementById('txtMontoCuotaIni').value = (parseFloat(arrCuota[2])).toFixed(2);
                        document.getElementById('txtMontoCuota').value = (parseFloat(arrCuota[3])).toFixed(2);
                    }
                }
            }
            tblServicio.style.display = 'none';
            tblEquipo.style.display = 'none';
            tblCuotas.style.display = 'inline';
        }

        function mostrarDireccion(nroSEC) {
            var url = '../consultas/sisact_direccion.aspx?';
            url += 'flgReadOnly=S' + '&nroSEC=' + nroSEC;
            abrirVentana(url, "", '950', '310', '_blank', true);
        }

        function mostrarDetalleOferta(nroSEC, strIdProd) {
            var url = '../consultas/sisact_pop_detalle_oferta.aspx?';
            url += 'flgReadOnly=S' + '&nroSEC=' + nroSEC;
            url += '&strIdProd=' + strIdProd;
            abrirVentana(url, "", '850', '400', '_blank', true);
        }
		
    </script>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <div style="overflow: auto; width: 100%; height: 100%; align: rigth">
        <asp:GridView ID="dgrMovil" runat="server" Width="100%" AutoGenerateColumns="False"
            BorderColor="#95B7F3" OnRowDataBound="dgrMovil_ItemDataBound" ShowHeaderWhenEmpty="true">
            <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
            <HeaderStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="PLAZO" HeaderText="Plazo" />
                <asp:BoundField DataField="PAQUETE" HeaderText="Paquete" />
                <asp:BoundField DataField="PLAN" HeaderText="Plan" />
                <asp:TemplateField HeaderText="Serv.">
                    <ItemTemplate>
                        <img id="btnMostrarSA" onclick='javascript:mostrarServicios("<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>","<%# Container.DataItemIndex + 1 %>","<%# DataBinder.Eval(Container.DataItem, "ID_PRODUCTO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Servicios Adicionales">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CF_TOTAL" HeaderText="Cargo Fijo" DataFormatString="{0:0.00}"/>
                <asp:BoundField DataField="MONTO_TOPE" HeaderText="Monto Tope S/." DataFormatString="{0:0.00}"/>
                <asp:BoundField DataField="CAMPANA" HeaderText="Campaña" />
                <asp:BoundField DataField="EQUIPO" HeaderText="Equipo" />
                <asp:BoundField DataField="PRECIO_VENTA" HeaderText="Precio Equipo" DataFormatString="{0:0.00}"/>
                <asp:TemplateField HeaderText="Cuotas">
                    <ItemTemplate>
                        <img id="btnMostrarCuotas" onclick='javascript:mostrarCuotas("<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Cuotas">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TELEFONO" HeaderText="Nro. a Portar" />
                <asp:BoundField DataField="ID_PRODUCTO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                    FooterStyle-CssClass="hiddencol" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="dgrDTH" runat="server" Width="100%" AutoGenerateColumns="False"
            BorderColor="#95B7F3" ShowHeaderWhenEmpty="true">
            <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
            <HeaderStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="CAMPANA" HeaderText="Campaña" />
                <asp:BoundField DataField="PLAN" HeaderText="Plan" />
                <asp:TemplateField HeaderText="Paq. Adic.">
                    <ItemTemplate>
                        <img id="btnMostrarSA" onclick='javascript:mostrarServicios("<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>","<%# Container.DataItemIndex + 1 %>","<%# DataBinder.Eval(Container.DataItem, "ID_PRODUCTO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Servicios Adicionales">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PLAZO" HeaderText="Plazo" />
                <asp:BoundField DataField="EQUIPO" HeaderText="Kits" />
                <asp:BoundField DataField="PRECIO_INSTALACION" HeaderText="Precio Inst." DataFormatString="{0:0.00}"/>
                <asp:BoundField DataField="CF_TOTAL" HeaderText="CF Plan Mensual" DataFormatString="{0:0.00}"/>
                <asp:BoundField DataField="CF_ALQUILER" HeaderText="CF Men. Alq. Kit" DataFormatString="{0:0.00}"/>
                <asp:BoundField DataField="CF_TOTAL_MENSUAL" HeaderText="Tot. CF Mensual" DataFormatString="{0:0.00}"/>
                <asp:TemplateField HeaderText="Dir. Inst.">
                    <ItemTemplate>
                        <img id="btnMostrarDir" onclick='javascript:mostrarDireccion("<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Dir. Inst.">
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="dgrHFC" runat="server" Width="100%" AutoGenerateColumns="False"
            BorderColor="#95B7F3" OnRowDataBound="dgrHFC_ItemDataBound" ShowHeaderWhenEmpty="true">
            <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
            <HeaderStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField DataField="CAMPANA" HeaderText="Campaña" />
                <asp:BoundField DataField="PLAZO" HeaderText="Plazo" />
                <asp:BoundField DataField="PAQUETE" HeaderText="Solucion" />
                <asp:BoundField DataField="PLAN" HeaderText="Paquete" />
                <asp:BoundField DataField="SERVICIO" HeaderText="Servicios" />
                <asp:TemplateField HeaderText="Serv. Adic.">
                    <ItemTemplate>
                        <img id="btnMostrarSA" onclick='javascript:mostrarServiciosHFC("<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>","<%# DataBinder.Eval(Container.DataItem, "ORDEN")%>","<%# Container.DataItemIndex + 1 %>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Servicios Adicionales">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Alq. Equipo">
                    <ItemTemplate>
                        <img id="btnMostrarEquipo" onclick='javascript:mostrarEquipos("<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Alq. Equipo">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CF_TOTAL" HeaderText="Cargo Fijo" DataFormatString="{0:0.00}"/>
                <asp:TemplateField HeaderText="Dir. Inst.">
                    <ItemTemplate>
                        <img id="btnMostrarDir" onclick='javascript:mostrarDireccion("<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Dir. Inst.">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TPCON_CODIGO" HeaderText="Monto Tope S/." DataFormatString="{0:0.00}"/>
                <asp:TemplateField HeaderText="Det. Ofert.">
                    <ItemTemplate>
                        <img id="btnMostrarDetalle" onclick='javascript:mostrarDetalleOferta("<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>","<%# DataBinder.Eval(Container.DataItem, "ID_PRODUCTO")%>");'
                            src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Det. Ofert.">
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TELEFONO" HeaderText="Nro. a Portar" />
            </Columns>
        </asp:GridView>
    </div>
    <table cellspacing="1" cellpadding="0" border="0" align="center">
        <tr>
            <td>
                <table class="Contenido" id="tblServicio" style="display: none; width: 300px" cellspacing="1"
                    cellpadding="0" border="0">
                    <tr>
                        <td class="Header">
                            &nbsp;Servicios Adicionales
                            <asp:Label ID="lblOrden" runat="server" CssClass="clsInputEnable"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table cellspacing="2" cellpadding="0" border="0">
                                <tr>
                                    <td class="Arial10B" valign="top" align="center">
                                        <asp:ListBox ID="lbxServicios" runat="server" CssClass="clsSelectEnable"
                                            Width="300px" Rows="6"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <input class="Boton" id="btnCerrarServicio" style="width: 80px; cursor: hand; height: 17px"
                                onclick="cerrarServicio();" type="button" value="Cerrar" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                <table class="Contenido" id="tblEquipo" style="display: none; width: 300px" border="0" cellspacing="1" cellpadding="0">
                    <tr>
                        <td class="Header">
                            Equipos
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table cellspacing="2" cellpadding="0" border="0">
                                <tr>
                                    <td class="Arial10B" valign="top" align="center">
                                        <asp:ListBox ID="lbxEquipos" runat="server" CssClass="clsSelectEnable" Width="300px"
                                            Rows="6"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <input class="Boton" id="btnCerrarEquipo" style="width: 80px; cursor: hand; height: 17px"
                                onclick="cerrarEquipo();" type="button" value="Cerrar" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td id="tdPromocion" style="display: none">
                <table style="width: 300px" class="Contenido" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Header">
                            Promociones
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="3" cellpadding="0" border="0">
                                <tr>
                                    <td class="Arial10b" valign="top" align="center">
                                        <asp:ListBox ID="lbxPromociones" runat="server" CssClass="clsSelectEnable" Width="300px"
                                            Rows="6"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="Contenido" id="tblCuotas" style="display: none;" cellspacing="1" cellpadding="0"
        width="400" border="0" align="center">
        <tr>
            <td class="Header">
                &nbsp;Cuotas
            </td>
        </tr>
        <tr>
            <td class="Contenido" align="center">
                <table cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            Nro Cuotas
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            <input class="clsInputDisable" id="txtNroCuota" style="width: 65px; text-align: right"
                                readonly type="text" />
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            % Cuota Inicial
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            <input class="clsInputDisable" id="txtPorcCuotaIni" style="width: 50px; text-align: right"
                                readonly type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            Monto Cuota Inicial
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            <input class="clsInputDisable" id="txtMontoCuotaIni" style="width: 65px; text-align: right"
                                readonly type="text" />
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            Monto Cuota
                        </td>
                        <td class="Arial10B" style="background-color: white" valign="top">
                            <input class="clsInputDisable" id="txtMontoCuota" style="width: 50px; text-align: right"
                                readonly type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <input class="Boton" id="btnCerrarCuota" style="width: 80px; cursor: hand; height: 17px"
                                onclick="tblCuotas.style.display = 'none'" type="button" value="Cerrar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" runat="server" id="hidCadenaServicios" />
    <input type="hidden" runat="server" id="hidCadenaKits" />
    <input type="hidden" runat="server" id="hidCadenaPromociones" />
    <input type="hidden" runat="server" id="hidCadenaEquiposHFC" />
    <input type="hidden" runat="server" id="hidCadenaCuotas" />
    </form>
</body>
</html>
