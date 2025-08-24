<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pool_aprob_excepcion_precio.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_pool_aprob_excepcion_precio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link href="../../Estilos/calendar-blue.css" type="text/css" rel="stylesheet" />
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/JSUtil.js"></script>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script>
    <script src="../../Scripts/calendar/calendar.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_es.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_setup.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendario_call.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_evaluacion.js" type="text/javascript"></script>
    <script src="../../Scripts/Paginas/evaluacion_cons/sisact_pool_aprob_excepcion_precio.js"
        type="text/javascript"></script>
    <style type="text/css">
        #PanelCarga table thead th
        {
            background-color: #E9F2FE;
            border-bottom: 2px solid #95B7F3;
            line-height: 1.428571429;
        }
        
        #PanelCarga
        {
            height: auto;
            position: absolute !important;
            left: 20%;
            top: 30%;
            z-index: 50px;
            border-bottom: 4px solid #95B7F3;
            background-color: white;
        }
        
        
        
        #lblsolicitud
        {
            font-size: 11px;
            color: navy;
            font-family: Arial;
            text-decoration: none;
            font-weight: bold;
            height: 19px;
            width: 313px;
        }
        
        #lblcomentario
        {
            font-size: 11px;
            color: navy;
            font-family: Arial;
            text-decoration: none;
            font-weight: bold;
            height: 19px;
            width: 313px;
        }
        
        input:disabled
        {
            color: Navy;
            font-weight: bold;
        }
        .style13
        {
            font-size: 11px;
            color: navy;
            font-family: Arial;
            text-decoration: none;
            font-weight: bold;
            height: 16px;
        }
        #tdExcepcion tr td
        {
            width: 20%;
            text-align: left;
        }
        #tblPanelCarga thbody tr
        {
            padding: 5px 0;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table cellspacing="1" cellpadding="0" class="Contenido" width="100%" border="0">
        <tr>
            <td class="Header" align="center" height="19">
                POOL DE SOLICITUDES PENDIENTES DE AUTORIZACION
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="tdExcepcion" class="center" width="80%" cellspacing="1" cellpadding="0"
                    border="0">
                    <tr>
                        <td class="Arial10B">
                            &nbsp;Fecha:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFecha" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                            <img alt="" id="btnFecha" style="border: 0px; cursor: pointer;" src="../../Imagenes/calendario.gif"
                                border="0" />
                            <script type="text/javascript">
                                Calendar.setup(
                                            {
                                                inputField: "txtFecha",      // id of the input field
                                                ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                button: "btnFecha",   // trigger for the calendar (button ID)
                                                singleClick: true,           // double-click mode
                                                step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                            }
                                        );
                            </script>
                        </td>
                        <td class="Arial10B">
                            &nbsp;Tipo Busqueda:
                        </td>
                        <td>
                            <asp:DropDownList ID="dllTipoBusqueda" runat="server" Width="180px" CssClass="clsSelectEnableC">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtbusqueda" runat="server" Width="150px" onkeypress="return validaNumeros(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B">
                            &nbsp;Estado Solicitud:
                        </td>
                        <td>
                            <asp:DropDownList ID="dllEstadoSolicitud" Width="180px" runat="server" CssClass="clsSelectEnableC">
                            </asp:DropDownList>
                        </td>
                        <td class="style12">
                        </td>
                        <td class="style3">
                        </td>
                        <td class="style7">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="5">
                <table class="center" cellspacing="2" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnConsultar" OnClick="btnConsultar_Click" OnClientClick=" return ValidarConsulta();"
                                Style="width: 100px; cursor: hand; height: 19px; float: right; margin: 0 5px"
                                CssClass="Boton" Text="Buscar" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnLimpiar" OnClick="btnLimpiar_Click" Style="width: 100px;
                                cursor: hand; height: 19px; float: left; margin: 0 5px" CssClass="Boton" Text="Limpiar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="lblCantReg" runat="server" class="Arial10BRed"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:GridView ID="gvPool" runat="server" Width="100%" AutoGenerateColumns="False"
                    ShowHeaderWhenEmpty="true" BorderColor="#CDE0F5" BorderStyle="Solid" CellPadding="5"
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPool_PageIndexChanging"
                    BorderWidth="1px" CssClass="TablePool" OnRowDataBound="gvPool_RowDataBound" DataKeyNames="estado,cuotaInicialTV,cuotaInicialSISACT,precioTienda,precioSisact">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                    <RowStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" BackColor="#E9EBEE" />
                    <AlternatingRowStyle Wrap="False" HorizontalAlign="Center" CssClass="Arial10B" BackColor="#DDDEE2" />
                    <Columns>
                        <asp:BoundField DataField="fechaSolicitud" HeaderText="Fecha Solicitud"></asp:BoundField>
                        <asp:BoundField DataField="pedidoTienda" HeaderText="Id pedido Tienda"></asp:BoundField>
                        <asp:BoundField DataField="pedidoSinergia" HeaderText="Nro Pedido SISACT"></asp:BoundField>
                        <asp:BoundField DataField="solinCodigo" HeaderText="Nro Sec"></asp:BoundField>
                        <asp:BoundField DataField="campana" HeaderText="Campaña"></asp:BoundField>
                        <asp:BoundField DataField="plazo" HeaderText="Plazo Acuerdo"></asp:BoundField>
                        <asp:BoundField DataField="codMaterial" HeaderText="Código Material"></asp:BoundField>
                        <asp:BoundField DataField="plan" HeaderText="Plan"></asp:BoundField>
                        <asp:BoundField DataField="tipoOperacion" HeaderText="Tipo Operación"></asp:BoundField>
                        <asp:BoundField DataField="tipoProducto" HeaderText="Tipo Producto"></asp:BoundField>
                        <asp:BoundField DataField="tipoVenta" HeaderText="Tipo Venta"></asp:BoundField>
                        <asp:BoundField DataField="modalidadVenta" HeaderText="Modalidad Venta"></asp:BoundField>
                        <asp:BoundField DataField="cuotaInicialTV" HeaderText="Cuota Inicial Tienda"></asp:BoundField>
                        <asp:BoundField DataField="cuotaInicialSISACT" HeaderText="Cuota Inicial Sisact">
                        </asp:BoundField>
                        <asp:BoundField DataField="precioTienda" DataFormatString="{0:N}" HeaderText="Precio tienda virtual">
                        </asp:BoundField>
                        <asp:BoundField DataField="precioSisact" DataFormatString="{0:N}" HeaderText="Precio SISACT">
                        </asp:BoundField>
                        <asp:BoundField DataField="codigoOficina" HeaderText="Cod.Oficina"></asp:BoundField>
                        <asp:BoundField DataField="descOficina" HeaderText="Desc.Oficina"></asp:BoundField>
                        <asp:BoundField DataField="estadoSolicitud" HeaderText="Estado"></asp:BoundField>
                        <asp:TemplateField HeaderText="Acci&#243;n">
                            <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ToolTip="Validar" ID="imgValidar" Style="cursor: hand; margin: 0 15px;"
                                    ImageUrl="../../Imagenes/aceptar.gif" runat="server" onclick=<%# "javascript:validar('" +Eval("idSolicitud")+"','" +Eval("pedidoSinergia")+"','" +Eval("solinCodigo")+"');" %> />
                                <asp:Image ToolTip="Aprobar" ID="imgEvaluar" Style="cursor: hand; margin: 0 15px;"
                                    ImageUrl="../../Imagenes/activar.gif" runat="server" onclick=<%# "javascript:procesar('" +Eval("idSolicitud")+"','" +Eval("pedidoSinergia")+"','" +Eval("solinCodigo")+"');" %> />
                                <asp:Image ToolTip="Rechazar" ID="imgRechazar" Style="cursor: hand; margin: 0 15px;"
                                    ImageUrl="../../Imagenes/rechazar.gif" runat="server" onclick=<%# "javascript:rechazar('" +Eval("idSolicitud")+"','" +Eval("pedidoSinergia")+"','" +Eval("solinCodigo")+"');" %> />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <input id="hdFlag" type="hidden" name="hdFlag" runat="server" />
    <input id="hdId" type="hidden" name="hdId" runat="server" />
    <input id="hdFechaG" type="hidden" name="HdFechaG" runat="server" />
    <input id="hdidFlujo" type="hidden" name="hdidFlujo" runat="server" />
    <input id="hdsolincod" type="hidden" name="hdsolincod" runat="server" />
    <input id="hdestadoPosterior" type="hidden" name="hdestadoPosterior" runat="server" />
    <input id="hdpedidossingergia" type="hidden" name="hdpedidossingergia" runat="server" />
    <input id="hdFlagRol" type="hidden" name="hdFlagRol" runat="server" />
    <asp:HyperLink ID="hlOk" runat="server"></asp:HyperLink>
    <asp:HyperLink ID="hlControlID" runat="server"></asp:HyperLink>
    <cc1:ModalPopupExtender ID="modalpopup" runat="server" CancelControlID="hlCancel"
        OkControlID="hlOk" TargetControlID="hlControlID" PopupControlID="PanelCarga"
        Drag="true" RepositionMode="RepositionOnWindowResize" BackgroundCssClass="ModalPopupBG">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PanelCarga" runat="server" Style="display: block">
        <table id="tblPanelCarga" cellspacing="1" cellpadding="0" class="center" style="width: 70%;
            border: 1px solid black; margin: 0 auto; padding: 5px">
            <thead>
                <tr>
                    <td class="Header" align="center" id="CabeceraPopup" height="22" colspan="2" style="position: relative">
                        Información del motivo de rechazo de excepción de precios
                        <input id="hlCancel" class="Boton" type="button" value="X" onclick="closeBeneficio()"
                            style="border: none; font-size: small; color: Red; font-weight: 900; background-color: #E9F2FE;
                            display: none" />
                        <input id="btnCerrar" type="button" value="X" class="Boton" onclick="closeBeneficio()"
                            style="border: none; font-size: small; color: Red; font-weight: 900; background-color: #E9F2FE;
                            right: 0; position: absolute; top: 0" />
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr align="center">
                    <td class="Arial10B" colspan="2">
                        <label id="prodCliente" style="color: Black">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label id="lblsolicitud">
                            Nro de Solicitud:&nbsp;&nbsp;</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtnroSolicitud" ReadOnly="true" CssClass="colorAzul" runat="server"
                            Font-Bold="true" Width="120px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label id="lblcomentario">
                            Comentario:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComentario" runat="server" TextMode="multiline" Font-Bold="true"
                            Width="600px"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td align="center" colspan="2">
                        <input id="btnaceptar" type="button" onclick="AprobarAnular();" style="width: 80px"
                            class="Boton" value="Aceptar" />
                        <input id="btncancelar" type="button" value="Cancelar" class="Boton" onclick="closeBeneficio();"
                            style="width: 80px" />
                    </td>
                </tr>
            </tfoot>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
