<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_consulta_validacion_cliente_brms.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_consulta_validacion_cliente_brms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle BRMS</title>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <style type="text/css">
        .Header
        {
            border-right: #000 1px solid;
            border-top: #000 1px solid;
            background: #9ebff6;
            border-left: #000 1px solid;
            border-bottom: #000 1px solid;
            height: 18px;
            font-weight: bold;
            font-size: 9pt;
        }
        .TablaTitulos
        {
            border-right: #95b7f3 1px solid;
            border-top: #95b7f3 1px solid;
            font-size: 8pt;
            background-image: url(../../Imagenes/toolgrad.gif);
            border-left: #000 1px solid;
            border-bottom: #000 1px solid;
            background-repeat: repeat-x;
            font-family: Verdana;
            background-color: #BFD5F9;
            text-decoration: none;
        }
        .Arial10B
        {
            font-size: 11px;
            font-family: Arial;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
    <table border="0" cellspacing="1" cellpadding="0" width="100%">
        <tr id="trEncabezado">
            <td class="Header" align="center" colspan="12">
                <b>Reporte de Solicitudes con elementos BRMS </b>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Solicitud
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgSolicitud" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="sistema_evaluacion" HeaderText="Sistema de evaluación"></asp:BoundColumn>
                        <asp:BoundColumn DataField="tipo_operacion" HeaderText="Tipo de operación"></asp:BoundColumn>

                    </Columns>
                </asp:DataGrid>            
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgCliente" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="antiguedad_deuda" HeaderText="Antigüedad de la deuda"></asp:BoundColumn>
                        <asp:BoundColumn DataField="cantidad_documentos_deuda" HeaderText="Cantidad de documentos con deuda"></asp:BoundColumn>
                         <%-- PROY-140743 --INI--%>
                        <asp:BoundColumn DataField="CANTLINEASCUOTASPENDACC" HeaderText="Cantidad de Lineas con Cuotas Pendientes por Ventas de Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTLINEASCUOTAPENDACC_ULTVTAS" HeaderText="Cantidad de Lineas con Cuotas Pendientes por Ventas de Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTMAXCUOTASPENDACC" HeaderText="Cantidad Maxima de Cuotas Pendientes por Ventas Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTMAXCUOTASPENDACC_ULTVTAS" HeaderText="Cantidad Maxima de Cuotas Pendientes por Ventas Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <%-- PROY-140743 --FIN--%>
                        <asp:BoundColumn DataField="comportamiento_pago" HeaderText="Comportamiento de Pago(CP)"></asp:BoundColumn>

                        <asp:BoundColumn DataField="flag_bloqueos" HeaderText="Flag Bloqueos"></asp:BoundColumn>
                        <asp:BoundColumn DataField="flag_suspensiones" HeaderText="Flag Suspensiones"></asp:BoundColumn>


                         <%-- PROY-140743 --INI--%>
                        <asp:BoundColumn DataField="MONTOPENDCUOTASACC" HeaderText="Monto Pendiente en Cuotas por Ventas de Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTOPENDCUOTASACC_ULTVTAS" HeaderText="Monto Pendiente en Cuotas por Ventas de Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <%-- PROY-140743 --FIN--%>
                        <asp:BoundColumn DataField="monto_deuda_castigada" HeaderText="Monto de la deuda castigada"></asp:BoundColumn>
                        <asp:BoundColumn DataField="monto_deuda_vencida" HeaderText="Monto de la deuda vencida"></asp:BoundColumn>
                        <asp:BoundColumn DataField="monto_total_pago" HeaderText="Monto total de pago a la fecha"></asp:BoundColumn>

                        <asp:BoundColumn DataField="promedio_facturado_soles" HeaderText="Promedio facturado en soles del Cliente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="tiempo_permamencia" HeaderText="Tiempo de permanencia del Cliente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="tipos_fraude" HeaderText="Tipos de fraudes del cliente"></asp:BoundColumn>
                        <%-- INI PROY-140422 --%>
                        <asp:BoundColumn DataField="segmento_cliente" HeaderText="Segmento del cliente"></asp:BoundColumn>
                        <%-- FIN PROY-140422 --%>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Bloqueos
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgBloqueos" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="tipo" HeaderText="Tipo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="tipoLinea" HeaderText="Tipo de Línea"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Disputa
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDisputa" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="disputa_antiguedad" HeaderText="Antigüedad del monto"></asp:BoundColumn>
                        <asp:BoundColumn DataField="disputa_cantidad" HeaderText="Cantidad de montos"></asp:BoundColumn>
                        <asp:BoundColumn DataField="disputa_monto" HeaderText="Monto"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Documento
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDocumento" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="documento_numero" HeaderText="Número"></asp:BoundColumn>
                        <asp:BoundColumn DataField="documento_tipo" HeaderText="Tipo"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Suspensiones
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgSuspensiones" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="tipo" HeaderText="Tipo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="tipoLinea" HeaderText="Tipo de Línea"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                LineaARenovar
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgLineaARenovar" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="antiguedad" HeaderText="Antigüedad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="fecha_activacion" HeaderText="Fecha de activación"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                PuntoDeVenta
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPuntoDeVenta" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="canal" HeaderText="Canal"></asp:BoundColumn>
                        <asp:BoundColumn DataField="codigo" HeaderText="Código"></asp:BoundColumn>
                        <asp:BoundColumn DataField="departamento" HeaderText="Departamento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="nombre" HeaderText="Nombre"></asp:BoundColumn>
                        <asp:BoundColumn DataField="region" HeaderText="Region"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Vendedor
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgVendedor" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="vendedor_codigo" HeaderText="Código"></asp:BoundColumn>
                        <asp:BoundColumn DataField="vendedor_nombre" HeaderText="Nombre"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
