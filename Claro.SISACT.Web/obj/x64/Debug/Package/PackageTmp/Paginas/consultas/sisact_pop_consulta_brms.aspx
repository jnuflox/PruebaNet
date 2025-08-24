<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_consulta_brms.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_consulta_brms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Detalle BRMS</title>
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
                        <asp:BoundColumn DataField="CARGO_FIJO_BOLSA" HeaderText="Cargo Fijo de Bolsa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FLAG_LICITACION" HeaderText="Flag de Licitaci&#243;n">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_BOLSA" HeaderText="Tipo de Bolsa"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_DESPACHO" HeaderText="Tipo de Despacho"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_OPERACION" HeaderText="Tipo de Operaci&#243;n">
                        </asp:BoundColumn>
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
                        <asp:BoundColumn DataField="CANT_LINEAS_ACTIVAS" HeaderText="Cantidad de Lineas Activas">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_PLANES_X_PRODUCTO" HeaderText="Cant. de Planes por Producto">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="COMPORTAMIENTO_PAGO" HeaderText="Comportamiento de Pago">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CREDITO_SCORE" HeaderText="Credit Score"></asp:BoundColumn>
                        <asp:BoundColumn DataField="EDAD" HeaderText="Edad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FACTURACION_PROMEDIO_CLARO" HeaderText="Facturacion Promedio Claro">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="FACTURACION_PROMEDIO_PRODUCTO" HeaderText="Facturaci&#243;n Promedio Producto">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LIMITE_CREDITO_DISPONIBLE" HeaderText="L&#237;mite de Cr&#233;dito Disponible">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="RIEGO" HeaderText="Riego"></asp:BoundColumn>
                        <asp:BoundColumn DataField="SEXO" HeaderText="Sexo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIEMPO_PERMANENCIA" HeaderText="Tiempo de Permanencia">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Direccion del Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDireccionCliente" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="CODIGO_PLANO" HeaderText="Codigo de Plano"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DEPARTAMENTO" HeaderText="Departamento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DISTRITO" HeaderText="Distrito"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROVINCIA" HeaderText="Provincia"></asp:BoundColumn>
                        <asp:BoundColumn DataField="REGION" HeaderText="Regi&#243;n"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Documento del Cliente
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDocumentoCliente" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="DESCRIPCION_DOCUMENTO" HeaderText="Tipo de Documento">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NUMERO_DOCUMENTO" HeaderText="Nro. de Documento"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Representantes Legales
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgRRLL" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="DESCRIPCION_DOCUMENTO" HeaderText="Tipo de Documento">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NUMERO_DOCUMENTO" HeaderText="Nro. de Documento"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Equipos
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgEquipos" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="COSTO" HeaderText="Costo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CUOTAS" HeaderText="Cuotas"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FORMA_PAGO" HeaderText="Forma de Pago"></asp:BoundColumn>
                        <asp:BoundColumn DataField="GAMA" HeaderText="Gama"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MODELO" HeaderText="Modelo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MONTO_CUOTA" HeaderText="Monto Cuota"></asp:BoundColumn>
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
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Oferta
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgOferta" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="CANTIDAD_DECOS" HeaderText="Cant. de Decos"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CASO_ESPECIAL" HeaderText="Caso Especial"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CONTROL_CONSUMO" HeaderText="Control de Consumo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="KIT_INSTALACION" HeaderText="Kit de Instalaci&#243;n">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PLAZO_ACUERDO" HeaderText="Plazo de Acuerdo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PRODUCTO_COMERCIAL" HeaderText="Producto Comercial">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SEGMENTO_OFERTA" HeaderText="Segmento de Oferta"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_OPERACION_EMPRESA" HeaderText="Tipo de Operaci&#243;n Empresa">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO_PRODUCTO" HeaderText="Tipo de Producto"></asp:BoundColumn>
                        <asp:BoundColumn DataField="MODALIDAD" HeaderText="Modalidad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="OPERADOR_CEDENTE" HeaderText="Operador Cedente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="COMBO" HeaderText="Grupo Producto"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Campaña
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgCampana" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="GRUPO" HeaderText="Grupo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TIPO" HeaderText="Tipo"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Servicios
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgServicios" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre Servicio"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Plan Actual
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPlanActual" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="CARGO_FIJO" HeaderText="Cargo Fijo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMBRE" HeaderText="Nombre"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CANT_MESES_APADECE" HeaderText="Cantidad de Meses para cubrir Apadece">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PLAZO_ACUERDO" HeaderText="Plazo de Acuerdo"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Plan Solicitado
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPlanSolicitado" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
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
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Punto de Venta
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgPuntoVenta" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="CALIDAD_VENDEDOR" HeaderText="Calidad del Vendedor">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CANAL" HeaderText="Canal"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CODIGO_PDV" HeaderText="Codigo PDV"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NOMBRE_PDV" HeaderText="Nombre PDV"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <table cellspacing="1" cellpadding="0" border="0">
        <tr>
            <td class="Header">
                Dirección del PDV
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgDireccionPDV" runat="server" AutoGenerateColumns="False">
                    <ItemStyle HorizontalAlign="Center" CssClass="Arial10B"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="CODIGO_PLANO" HeaderText="Codigo de Plano"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DEPARTAMENTO" HeaderText="Departamento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DISTRITO" HeaderText="Distrito"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PROVINCIA" HeaderText="Provincia"></asp:BoundColumn>
                        <asp:BoundColumn DataField="REGION" HeaderText="Regi&#243;n"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
