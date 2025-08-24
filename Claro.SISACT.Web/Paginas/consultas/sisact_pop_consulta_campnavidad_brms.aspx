<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_consulta_campnavidad_brms.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_consulta_campnavidad_brms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Parámetros ODM Validación Campaña</title>
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
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header" align="center" colspan="12">
                <b>Reporte de Solicitudes de BRMS de Validacion Campaña</b>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Solicitud
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgSolicitud" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CARGO_FIJO_BOLSA" HeaderText="Cargo Fijo de Bolsa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FLAG_LICITACION" HeaderText="Flag de Licitación"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_BOLSA" HeaderText="Tipo de Bolsa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_DESPACHO" HeaderText="Tipo de Despacho"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_OPERACION" HeaderText="Tipo de Operación"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BURO_CONSULTADO" HeaderText="Buro Consultado"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FECHA_EJECUCION" HeaderText="Fecha de Ejecución"></asp:BoundColumn>
                        <asp:BoundColumn DataField="HORA_EJECUCION" HeaderText="Hora de Ejecución"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgCliente" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="ANTIGUEDAD" HeaderText="Antiguedad de Deuda"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ANTIGUEDAD_MONTO" HeaderText="Antiguedad Monto en Disputa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_LINEAS_ACTIVAS" HeaderText="Cantidad de Lineas Activas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_PLANES_X_PRODUCTO" HeaderText="Cant. de Planes por Producto"></asp:BoundColumn>
                        <%-- PROY-140743 --INI--%>
                        <asp:BoundColumn DataField="CANT_LINEA_PEN_ACC" HeaderText="Cantidad de Lineas con Cuotas Pendientes por Ventas de Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_LINEA_PEND_ULT_ACC" HeaderText="Cantidad de Lineas con Cuotas Pendientes por Ventas de Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MAX_CUO_PEND_ACC" HeaderText="Cantidad Maxima de Cuotas Pendientes por Ventas Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MAX_CUO_PEND_ULT_ACC" HeaderText="Cantidad Maxima de Cuotas Pendientes por Ventas Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <%-- PROY-140743 --FIN--%>
                        <asp:BoundColumn DataField="CANTIDAD_MONTOS" HeaderText="Cantidad Montos en Disputa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MAX_CUO_SIS" HeaderText="Cantidad Maxima de Cuotas Pendientes en Sistemas">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MAX_CUO_ULT" HeaderText="Cantidad Maxima de Cuotas Pendientes Ultimas Ventas">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_PLAN_CUO_SIS" HeaderText="Cantidad de Planes con Cuotas Pendientes en Sistema">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_PLAN_CUO_ULT" HeaderText="Cantidad de Planes con Cuotas Pendientes Ultimas Ventas">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="COMPORTAMIENTO_PAGO" HeaderText="Comportamiento de Pago"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CREDITO_SCORE" HeaderText="Credito Score"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DEUDA" HeaderText="Deuda"></asp:BoundColumn>
                        <asp:BoundColumn DataField="EDAD" HeaderText="Edad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FACTURACION_PROMEDIO_CLARO" HeaderText="Facturacion Promedio Claro"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FACTURACION_PROMEDIO_PRODUCTO" HeaderText="Facturacion Promedio Producto"></asp:BoundColumn>
                        <%--PROY-140579 RU11 NN se agrega FLAG_WHITELIST--%>
                        <asp:BoundColumn DataField="FLAG_WHITELIST" HeaderText="Flag Whitelist"></asp:BoundColumn>                        
                        <asp:BoundColumn DataField="LIMITE_CREDITO_DISPONIBLE" HeaderText="Limite de Crédito Disponible"></asp:BoundColumn>
                        <%-- PROY-140743 --INI--%>
                        <asp:BoundColumn DataField="MONTO_CUOTAS_ACC" HeaderText="Monto Pendiente en Cuotas por Ventas de Accesorios en Sistema"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_PEND_CUO_ULT_ACC" HeaderText="Monto Pendiente en Cuotas por Ventas de Accesorios en Ultimas Ventas"></asp:BoundColumn>
                        <%-- PROY-140743 --FIN--%>
                        <asp:BoundColumn DataField="MONTO_DEUDA_CASTIGADA" HeaderText="Monto Deuda Castigada"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_DEUDA_VENCIDA_CLIENTE" HeaderText="Monto Deuda Vencida"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_DISPUTA" HeaderText="Monto en Disputa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO" HeaderText="Monto Total de la Deuda"></asp:BoundColumn>
                        <asp:BoundColumn DataField="RIEGO" HeaderText="Riego"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SEGMENTO" HeaderText="Segmento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_PEND_CUO_SIS" HeaderText="Monto Pendiente en Cuotas en Sistema">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_PEND_CUO_ULT" HeaderText="Monto Pendiente en Cuotas Ultimas Ventas">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SEXO" HeaderText="Sexo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIEMPO_PERMANENCIA" HeaderText="Tiempo de Permanencia"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPCONTRIBUYENTE" HeaderText="Tipo de Contribuyente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMCOMERCIAL" HeaderText="Nombre Comercial del Contribuyente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FEC_INIACTIVIDADES" HeaderText="Fecha de Inicio de Actividades del Contribuyente">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ESTCONTRIBUYENTE" HeaderText="Estado del Contribuyente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CONDCONTRIBUYENTE" HeaderText="Condición del Contribuyente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTIDAD_MESES_CONTRIBUYENTE" HeaderText="Cantidad de meses desde su inicio de actividades del Contribuyente">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CIIUCONTRIBUYENTE" HeaderText="CIU del Contribuyente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTTRABAJADORES" HeaderText="Cantidad de Trabajadores del Contribuyente">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EMISIONCOMP" HeaderText="Comprobantes de Pago c/aut. de impresión(F.806u 816)">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SIST_EMIELECTRONICA" HeaderText="Sistema de Emisión Electrónica del Contribuyente">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Dirección de Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDireccion" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CODIGO_PLANO" HeaderText="Codigo de Plano"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DEPARTAMENTO" HeaderText="Departamento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DISTRITO" HeaderText="Distrito"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROVINCIA" HeaderText="Provincia"></asp:BoundColumn>
                        <asp:BoundColumn DataField="REGION" HeaderText="Región"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Documento del Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDocumento" runat="server" Width="100%" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="DESCRIPCION_DOCUMENTO" HeaderText="Tipo de Documento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NUMERO_DOCUMENTO" HeaderText="Nro. de Documento"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Representantes Legales
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgRepresentantes" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="DESCRIPCION_DOCUMENTO" HeaderText="Tipo de Documento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NUMERO_DOCUMENTO" HeaderText="Nro. de Documento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CARGO_RRLL" HeaderText="Cargo de los Representantes Legales"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FECHA_NOMBRAMIENTO_RRLL" HeaderText="Fecha de Nombramiento de los Representates Legales"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MESES_NOMBRAMIENTO_RRLL" HeaderText="Cantidad de meses desde el nombramiento del RRLL"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Equipos
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgEquipos" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="COSTO" HeaderText="Costo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CUOTAS" HeaderText="Cuotas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FORMA_PAGO" HeaderText="Forma de Pago"></asp:BoundColumn>
                        <asp:BoundColumn DataField="GAMA" HeaderText="Gama"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MODELO" HeaderText="Modelo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_CUOTA" HeaderText="Monto Cuota"></asp:BoundColumn>
                        <%--PROY-140579 RU09 NN INI--%>  
                        <asp:BoundColumn DataField="MONTO_CUOTA_COMERCIAL" HeaderText="Monto de la Cuota Comercial"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_CUOTA_INICIAL_COMERCIAL" HeaderText="Monto de Cuota Inicial Comercial"></asp:BoundColumn>
						<%--PROY-140579 RU09 NN FIN--%>
                        <asp:BoundColumn DataField="PORCENTAJE_CUOTA_INICIAL" HeaderText="Porcentaje Cuota Inicial">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PRECIO_VENTA" HeaderText="Precio Venta"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_DECO" HeaderText="Tipo de Deco"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_OPERACION_KIT" HeaderText="Tipo de Operacion Kit">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Oferta
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgOferta" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CANTIDAD_DECOS" HeaderText="Cant. de Decos"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CASO_ESPECIAL" HeaderText="Caso Especial"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CONTROL_CONSUMO" HeaderText="Control de Consumo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="KIT_INSTALACION" HeaderText="Kit de Instalación"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PLAZO_ACUERDO" HeaderText="Plazo de Acuerdo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PRODUCTO_COMERCIAL" HeaderText="Producto Comercial"></asp:BoundColumn>
                        <%--##PROY-140743--%>                        
                        <asp:BoundColumn DataField="PRODUCTO_CUENTA_FACTURAR" HeaderText="Producto de la Cuenta a Facturar"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROMOCIONES" HeaderText="Promociones"></asp:BoundColumn>       
                        <%--##--%>
                        <asp:BoundColumn DataField="SEGMENTO_OFERTA" HeaderText="Segmento de Oferta"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_OPERACION_EMPRESA" HeaderText="Tipo de Operación Empresa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_PRODUCTO" HeaderText="Tipo de Producto"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MODALIDAD" HeaderText="Modalidad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="OPERADOR_CEDENTE" HeaderText="Operador Cedente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="COMBO" HeaderText="Grupo Producto"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROTECCION_MOVIL" HeaderText="Protección Móvil"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIEMPO_PERMANENCIA_CP" HeaderText="Permanencia del Operador Cedente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANTIDAD_LINEAS_SEC" HeaderText="Cantidad Lineas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_CF_SEC" HeaderText="Cargo Fijo Acumulado"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Campaña
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgCampana" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="GRUPO" HeaderText="Grupo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Servicios
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgServicios" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre Servicio"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Plan Actual
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPlanActual" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CANT_PEND_CUO_RENO" HeaderText="Cantidad de Cuotas Pendientes"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CARGO_FIJO" HeaderText="Cargo Fijo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MESES_APADECE" HeaderText="Cantidad de Meses para cubrir Apadece"></asp:BoundColumn>
                        <%--PROY-140579 RU11 NN INI--%>                                               
                        <asp:BoundColumn DataField="MODA_CEDENTE_LINEA_RENO" HeaderText="Modalidad cedente de l&#237;nea a renovar"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_CUO_ACTUAL" HeaderText="Monto de cuota actual de la renovaci&#243;n"></asp:BoundColumn> 
                        <%--PROY-140579 RU11 NN FIN--%>
                        <asp:BoundColumn DataField="MONTO_PEND_CUO_RENO" HeaderText="Monto Pendiente en Cuotas"></asp:BoundColumn>
                        <%--PROY-140579 RU11 NN se agrega ACTI_LINEA_RENO--%> 
                        <asp:BoundColumn DataField="ACTI_LINEA_RENO" HeaderText="Motivo de activaci&#243;n de l&#237;nea a renovar"></asp:BoundColumn> 
                        <asp:BoundColumn DataField="PLAZO_ACUERDO" HeaderText="Plazo de Acuerdo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIEMPO_PERMANENCIA" HeaderText="Tiempo de Permanencia de la Renovación"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Plan Solicitado
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPlanSolicitado" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CARGO_FIJO" HeaderText="Cargo Fijo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PAQUETE" HeaderText="Paquete"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Punto de Venta
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPuntoVenta" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CALIDAD_VENDEDOR" HeaderText="Calidad del Vendedor"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANAL" HeaderText="Canal"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CODIGO_PDV" HeaderText="Codigo PDV"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMBRE_PDV" HeaderText="Nombre PDV"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="1" width="100%">
        <tr>
            <td class="Header">
                Dirección del PDV
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDireccionPDV" runat="server" Width="100%" AutoGenerateColumns="false">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B" />
                    <HeaderStyle Wrap="false" HorizontalAlign="Center" CssClass="TablaTitulos" />
                    <Columns>
                        <asp:BoundColumn DataField="CODIGO_PLANO" HeaderText="Codigo de Plano"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DEPARTAMENTO" HeaderText="Departamento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DISTRITO" HeaderText="Distrito"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROVINCIA" HeaderText="Provincia"></asp:BoundColumn>
                        <asp:BoundColumn DataField="REGION" HeaderText="Region"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
