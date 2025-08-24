<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_reporte_solicitud_corporativo_Pdv_Renovacion.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_reporte_solicitud_corporativo_Pdv_Renovacion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Datos de la Solicitud de Evaluación Empresa</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'
        type="text/javascript"></script>
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_creditos.js" type="text/javascript"></script>
    <script src="../../Scripts/security.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript">
        var constTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        //EMMH I
        //var ConstflagPlanesProactivos = '<%= ConfigurationManager.AppSettings["ConstFlagPlanesProactivos"] %>'; //PROY_30748 
        var ConstflagPlanesProactivos = getValue('hidFlagPlanesProactivos'); //PROY_30748 
                var perfil = '<%= ConfigurationManager.AppSettings["constOpcionConsultaCreditos"] %>';
        var constTipoOperacionRenovacion = '<%= ConfigurationManager.AppSettings["constTipoOperacionRenovacion"] %>';
        var perfilCuotasEmp = '<%= ConfigurationManager.AppSettings["constOpcionConsultaCuotasEmp"] %>';
            var operacion = '<%= ConfigurationManager.AppSettings["constTipoOperacionRenovacion"] %>';
        var constPaginaPoolEmpresa = '<%= ConfigurationManager.AppSettings["constPaginaPoolEmpresa"] %>';
        var constPaginaChecklist = '<%= ConfigurationManager.AppSettings["constPaginaChecklist"] %>';
        var ConstCodTipoDespaDescentra = '<%= ConfigurationManager.AppSettings["ConstCodTipoDespaDescentra"] %>';
        var constPaginaVerAcuerdos = '<%= ConfigurationManager.AppSettings["constPaginaVerAcuerdos"] %>';
    </script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/evaluacion_cons/sisact_reporte_solicitud_corporativo_Pdv_Renovacion.js"></script>
</head>
<body onkeydown="cancelarBackSpace();" style="margin: 0px;">
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <table cellspacing="1" cellpadding="0" width="100%" border="0"> <!--PROY-31948 INI -->
        <tr id="trOpciones">
            <td>
                <table cellspacing="1" cellpadding="0" border="0">
                    <tr>
                        <td class="tab_activo" id="tdReporteSEC" align="center">
                            <a href="javascript:mostrarTabVisible('datos');">Datos de la Evaluación</a>
                        </td>
                        <td class="tab_inactivo" id="tdReporteHistoricoCliente" align="center">
                            <a href="javascript:mostrarTabVisible('historico');">Historico del Cliente</a>
                        </td>
                        <td class="tab_inactivo" id="tdReporteEstadoHistorico" align="center">
                            <a href="javascript:mostrarTabVisible('estado');">Log de Estados</a>
                        </td>
                        <td class="tab_inactivo" id="tdReporteEstadoHistoricoSGA" align="center">
                            <a href="javascript:mostrarTabVisible('estadoSGA');">Log de Estados SGA</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tblReporteSEC" cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr id="trPuntoVenta">
                        <td>
                            <table class="Contenido" cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Arial10B" width="150">
                                        &nbsp;N° Evaluación:
                                    </td>
                                    <td width="300">
                                        <asp:TextBox ID="txtNroSEC" runat="server" CssClass="clsInputDisable" Width="70px"
                                            ReadOnly="True" Font-Size="X-Small"></asp:TextBox>
                                    </td>
                                    <td id="tdEstadoVenta" class="Arial10B" width="80">
                                        Estado Venta:
                                    </td>
                                    <td id="tdTxtEstadoVenta" width="200">
                                        <asp:TextBox ID="txtEstadoVenta" runat="server" CssClass="clsInputDisable" Width="165px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="Arial10B" width="110">
                                        Caso Especial:
                                    </td>
                                    <td width="250">
                                        <asp:TextBox ID="txtCasoEspecial" runat="server" CssClass="clsInputDisable" Width="85%"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Arial10B" width="150">
                                        &nbsp;Cliente SAP:
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtCodClienteSAP" runat="server" CssClass="clsInputDisable" Width="120px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Arial10B" width="110">
                                        &nbsp;Canal:
                                    </td>
                                    <td width="100">
                                        <asp:TextBox ID="txtCanal" runat="server" CssClass="clsInputDisable" Width="70px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="Arial10B" width="80">
                                        Punto Venta:
                                    </td>
                                    <td width="200">
                                        <asp:TextBox ID="txtPuntoVenta" runat="server" CssClass="clsInputDisable" Width="150px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="Arial10B" width="120">
                                        Tipo Operación:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtTipoOperacion" runat="server" CssClass="clsInputDisable" Width="200px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Arial10B" width="150">
                                        &nbsp;Distribuidor:
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtDistribuidorDes" runat="server" CssClass="clsInputDisable" Width="250px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Arial10B" width="150">
                                        &nbsp;Motivo:
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtMotivo" CssClass="clsInputDisable" Width="85%" ReadOnly="True"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Arial10B" width="150">
                                        &nbsp;Observación:
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtObservacion" CssClass="clsInputDisable" Width="85%" ReadOnly="True"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trRecibos" style="display: none">
                                    <td width="150">
                                        &nbsp;
                                    </td>
                                    <td class="Arial10B" align="left" colspan="7">
                                        <asp:DataGrid ID="dgRecibo" runat="server" AutoGenerateColumns="FALSE" EnableViewState="False">
                                            <ItemStyle HorizontalAlign="Center" Height="15px" CssClass="Arial10B"></ItemStyle>
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="BANCO_DES" HeaderText="Banco">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="NRO_OPERACION" HeaderText="N&#250;mero de Operaci&#243;n">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="FECHA_DEPOSITO" HeaderText="Fecha Deposito" DataFormatString="{0:dd/MM/yyyy}">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="MONTO_DEPOSITO" HeaderText="Monto del Deposito" DataFormatString="{0:#,###,###.00}">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDatosEmpresa">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Datos de la Empresa
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <%--<td class="Arial10B" width="150">
                                                    &nbsp;Tipo Cliente:
                                                </td>--%>
                                                 <td class="Arial10B" width="150" style="padding-left: 2px;">  <!-- INI 32438 -->
                                                    Tipo Cliente: <!-- INI 32438 -->
                                                </td>
                                                <td width="120">
                                                    <asp:TextBox ID="txtTipoCliente" runat="server" ReadOnly="True" Width="100px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                               <%-- <td class="Arial10B" width="150">
                                                    &nbsp;Nro RUC:
                                                </td>--%>
                                                <td class="Arial10B" width="150" style="padding-left: 2px;">  <!-- INI 32438 -->
                                                    Nro RUC: <!-- INI 32438 -->
                                                </td>
                                                <td width="120">
                                                    <asp:TextBox ID="txtNroRUC" runat="server" ReadOnly="True" Width="100px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <%--<td class="Arial10B" align="right">
                                                    Razón Social / Nombres y Apellidos:&nbsp;&nbsp;
                                                </td>--%>
                                                 <td class="Arial10B" align="left" style="padding-left: 4px;"> <!-- INI 32438-->
                                                    Razón Social / Nombres y Apellidos:&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRazonSocial" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                              <!-- INI 32438-->
                                                <td class="Arial10B" width="150" style="padding-left: 2px;">  <!-- INI 32438 -->
                                                    Tipo de Contribuyente: <!-- INI 32438 -->
                                                </td>
                                                  <td width="120">
                                                    <asp:TextBox ID="txtTipoContribuyente" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" align="left" style="padding-left: 4px;"> <!-- INI 32438 -->
                                                    Nombre Comercial del Contribuyente:&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNombreContribuyente" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="150" style="PADDING-LEFT: 2px">  <!-- INI 32438 -->
                                                    Estado del Contribuyente: <!-- INI 32438 -->
                                                </td>
                                                  <td width="120">
                                                    <asp:TextBox ID="txtEstadoContribuyente" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" align="left" style="padding-left: 4px;">
                                                    Fecha de Inicio de Actividades del Contribuyente:&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFechaActividades" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" align="left" style="padding-left: 2px;"> 
                                                    Condicion del Contribuyente:&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCondicion" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" align="left" style="padding-left: 4px;"> <!-- INI 32438 -->
                                                    Comprobantes de Pago c/aut. de impresión(F.806u 816):&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComprobantes" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr> <!-- INI 32438 
                                                <td class="Arial10B" width="150">
                                                    &nbsp;Actividades Económicas del Contribuyente:
                                                </td>
                                                  <td width="120">
                                                    <asp:TextBox ID="txtActividadesE" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td> FIN  32438-->
												<td class="Arial10B" width="150" style="padding-left: 2px;">  <!-- INI 32438 -->
                                                    CIU del Contribuyente: <!-- INI 32438 -->
                                                </td>
                                                  <td width="120">
                                                    <asp:TextBox ID="txtCIU" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" align="left" style="padding-left: 4px;">
                                                    Sistema de Emisión Electrónica del Contribuyente:&nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSistemaEmision" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td class="Arial10B" width="150" style="padding-left: 2px;">
                                                    Cantidad de Trabajadores del Contribuyente: <!-- INI 32438 -->
                                                 
                                                </td>
                                                <td width="120">
                                                    <asp:TextBox ID="txtCantTrabajadores" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                               <!-- FIN 32438-->
                                                <td colspan="4" height="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="150">
                                                    &nbsp;Resumen Líneas &nbsp;Móvil:
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:GridView ID="dgNroLineaActivas" runat="server" Width="98%" BorderColor="#95B7F3"
                                                        AutoGenerateColumns="False">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="TablaTitulos" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Nro Líneas Activas" DataField="CLIEN_NRO_LINEA" />
                                                            <asp:BoundField HeaderText="Nro Líneas > 180 Días" DataField="CLIEN_NRO_LINEA_MAY180" />
                                                            <asp:BoundField HeaderText="Nro Líneas <= 180 Días" DataField="CLIEN_NRO_LINEA_180" />
                                                            <asp:BoundField HeaderText="Nro Líneas <= 90 Días" DataField="CLIEN_NRO_LINEA_90" />
                                                            <asp:BoundField HeaderText="Nro Líneas <= 30 Días" DataField="CLIEN_NRO_LINEA_30" />
                                                            <asp:BoundField HeaderText="Nro Líneas <= 7 Días" DataField="CLIEN_NRO_LINEA_7" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" height="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="150">
                                                    &nbsp;RRLL:
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:DataGrid ID="dgRepresentantes" runat="server" BorderColor="#95B7F3" AutoGenerateColumns="False"
                                                        ItemStyle-BorderColor="#95B7F3">
                                                        <ItemStyle CssClass="TablaFilasGrid" HorizontalAlign="Center"></ItemStyle>
                                                        <HeaderStyle CssClass="TablaTitulos" HorizontalAlign="Center"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundColumn HeaderText="Nombre del Representante Legal" DataField="APODV_NOM_REP_LEG">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Apellido Paterno" DataField="APODV_APA_REP_LEG"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Apellido Materno" DataField="APODV_AMA_REP_LEG"></asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Tipo de Documento" DataField="TDOCV_DESCRIPCION_REP">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Número de Documento" DataField="APODV_NUM_DOC_REP">
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn HeaderText="Cargo" DataField="APODV_CAR_REP"></asp:BoundColumn>
                                                             <%--PROY -32438--%>
                                                              <asp:BoundColumn HeaderText="Fecha de Nombramiento del RRLL" ItemStyle-Width="15%" DataField="APODD_FECHA_NOMBRAMIENTO">
                                                             </asp:BoundColumn>
                                                             <asp:BoundColumn HeaderText="Cantidad de meses desde el nombramiento del RRLL"  ItemStyle-Width="20%" DataField="APODI_CANT_MESES_NOMBRAMIENTO">
                                                             </asp:BoundColumn>
                                                              <%-- PROY -32438--%>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" height="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="120">
                                                    &nbsp;Nro RRLL:
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtNroRRLL" runat="server" ReadOnly="True" Width="40px" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <%--PROY-140579 RU07 NN INICIO--%>
                                                <td class="Arial10B" align="left" style="padding-left: 4px;"> <!-- INI 32438-->
                                                    Flag Whitelist: &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWhitelist" runat="server" ReadOnly="True" Width="300px" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <%--PROY-140579 RU07 NN FIN--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trCondicionVenta">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Condiciones de Venta
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" width="100px">
                                                    &nbsp;Oferta:
                                                </td>
                                                <td width="200px">
                                                    <asp:TextBox ID="txtOferta" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="idlblCombo" class="Arial10B" width="100px">
                                                    &nbsp;Grupo Producto:
                                                </td>
                                                <td id="idtxtCombo" width="200px">
                                                    <asp:TextBox ID="txtCombo" runat="server" CssClass="clsInputDisable" Width="180px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 INI -->
                                                 <td id="tdCantCuotasPendLineaE" class="Arial10B" width="150px">
                                                    Cantidad de Cuotas Pendientes:
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtCantCuotasPendLineaE" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 FIN -->
                                                <td class="Arial10B" width="100px">
                                                    &nbsp;Modalidad Venta:
                                                </td>
                                                <td width="150px"> <!-- PROY-31948 -->
                                                    <asp:TextBox ID="txtModalidadVenta" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trRenovacion" style="display: none">
                                                <td id="tdTelefonoRenovar" class="Arial10B" width="150px" style="display: none">
                                                    &nbsp;Número a renovar:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTelefonoRenovar" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 INI -->
                                                <td id="tdMontoPendCuotasLineaE" class="Arial10B" width="150px">
                                                    Monto Pendiente en Cuotas:
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtMontoPendCuotasLineaE" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 FIN -->
                                                <td class="Arial10B" width="150px">
                                                    &nbsp;Plan Comercial:
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtPlanComercial" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="100px">
                                                    &nbsp;CF Actual:
                                                </td>
                                                <td width="350px">
                                                    <asp:TextBox ID="txtCFActual" runat="server" CssClass="clsInputDisable" Width="80px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <%--PROY-140579 RU07 NN INICIO--%>
                                            <tr id="trNuevosDatosBRMS" style="display: none">                                                    
                                                <td id="tdActiLineaReno" class="Arial10B" width="120px">                                                    
                                                    &nbsp;Motivo de Activación de línea a Renovar:
                                                </td>
                                                <td width="150px">                                                    
                                                    <asp:TextBox ID="txtActiLineaReno" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>

                                                <td id="tdModaCedenteLineaReno" class="Arial10B" width="150px">                                                    
                                                    &nbsp;Modalidad Cedente de línea a Renovar:
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtModaCedenteLineaReno" runat="server" CssClass="clsInputDisable"
                                                        Width="125px" ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>

                                                <td class="Arial10B" width="110px">                                                                                                       
                                                </td>
                                                <td width="150px">
                                                </td>  

                                                <td class="Arial10B" width="110px">                                                   
                                                    &nbsp;Monto de cuota actual:
                                                </td>
                                                <td width="150px">
                                                    <asp:TextBox ID="txtMontoCuotaActual" runat="server" CssClass="clsInputDisable" Width="79px"
                                                        ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                </td>                                              
                                            </tr>
                                            <%--PROY-140579 RU07 NN FIN--%>
                                            <tr>
                                                <td colspan="8" align="center"> <!-- PROY-31948 -->
                                                    <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; width: 100%; height: 100px; align: rigth">
                                                                    <!--PROY-24724-IDEA-28174 - INICIO-->
                                                                    <asp:GridView ID="dgPlanes" runat="server" Width="95%" BorderColor="#95B7F3" AutoGenerateColumns="False"
                                                                        OnRowDataBound="dgPlanes_RowDataBound" ShowHeaderWhenEmpty="true">
                                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PRODUCTO" HeaderText="Tipo de Producto" />
                                                                            <asp:BoundField DataField="PLAZO" HeaderText="Plazo" />
                                                                            <asp:BoundField DataField="PAQUETE" HeaderText="Paquete" Visible="False" />
                                                                            <asp:BoundField DataField="PLAN" HeaderText="Plan" />
                                                                            <asp:BoundField DataField="CAMPANA" HeaderText="Campaña" />
                                                                            <asp:BoundField DataField="MONTO_INICIAL" HeaderText="Monto Inicial" DataFormatString="{0:0.00}"/>
                                                                            <asp:BoundField DataField="EQUIPO" HeaderText="Equipo" />
                                                                            <asp:BoundField DataField="CF_TOTAL_MENSUAL" HeaderText="Cargo Fijo Total" DataFormatString="{0:0.00}" />
                                                                            <asp:BoundField DataField="PRECIO_VENTA" HeaderText="Precio de Venta Equipo" DataFormatString="{0:0.00}" />
                                                                            <asp:BoundField DataField="RIESGOTOTALEQUIPO" HeaderText="Riesgo de Equipo" />
                                                                            <asp:BoundField DataField="CAPACIDADDEPAGO" HeaderText="Capacidad de Pago" />
                                                                            <asp:BoundField DataField="EN_CUOTAS" HeaderText="En Cuotas" />
                                                                            <asp:BoundField DataField="NRO_CUOTAS" HeaderText="Nro Cuotas" />
                                                                            <asp:BoundField DataField="MONTO_CUOTA" HeaderText="Monto Cuota" DataFormatString="{0:0.00}"/>
                                                                            <asp:BoundField DataField="EXONERACIONDERENTAS" HeaderText="Exonera RA" />
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <img id="btnMostrarDetalle" onclick='javascript:mostrarDetalle("<%# DataBinder.Eval(Container.DataItem, "ID_PRODUCTO")%>", "<%# DataBinder.Eval(Container.DataItem, "ORDEN")%>", "<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>", "<%# DataBinder.Eval(Container.DataItem, "SOPLN_CODIGO")%>");'
                                                                                        src="../../Imagenes/ico_lupa.gif" style="cursor: hand;" alt="Mostrar Detalle">
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="PM"> 
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="ID_PRODUCTO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                                FooterStyle-CssClass="hiddencol" />
                                                                            <asp:BoundField DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                                FooterStyle-CssClass="hiddencol" />
                                                                            <asp:BoundField DataField="SOPLN_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                                FooterStyle-CssClass="hiddencol" />
                                                                            <asp:BoundField DataField="AGRUPA" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                                FooterStyle-CssClass="hiddencol" />
                                                                            <asp:BoundField DataField="ORDEN" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                                FooterStyle-CssClass="hiddencol" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <!--PROY-24724-IDEA-28174 - FIN-->
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Arial10B" align="center">
                                                                CF Total:&nbsp;&nbsp;<asp:TextBox ID="txtCFTotal" runat="server" ReadOnly="True"
                                                                    Width="50px" CssClass="clsInputDisable" Style="text-align: right"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trInformacionCrediticia">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Información Crediticia
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td nowrap="nowrap" class="Arial10B">
                                                    &nbsp;Buro Consultado:
                                                </td>
                                                <td class="Arial10B" width="110"> <!-- PROY-31948 -->
                                                    <asp:TextBox ID="txtBuro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                                <td nowrap="nowrap" class="Arial10BRed">
                                                    Rango LC Disponible:
                                                </td>
                                                <td> <!-- PROY-31948 --><!-- PROY-140579 LM -->
                                                    <asp:TextBox ID="txtRangoLC" ReadOnly="True" Width="120px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>

                                                <!-- PROY-140579 LM ODM VALIDACION CLIENTE INICIO -->
                                                <td>
                                                    &nbsp;</td>

                                                <td>
                                                    &nbsp;</td>

                                                <td>
                                                    &nbsp;</td>

                                                <td>
                                                    &nbsp;</td>

                                                <td colspan="4">
                                                <input class="Boton" id="btnExportarBRMSValidaciónCliente" style="cursor: hand; width: 250px;  float: right"
                                                        type="button" value="ver parámetros ODM Validación Cliente" />
                                                </td>
                                                <!-- PROY-140579 LM ODM VALIDACION CLIENTE FIN -->

                                            </tr>
                                            <tr id="trRiesgo">
                                                <td class="Arial10BRed">
                                                    &nbsp;Riesgo Claro:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRiesgoClaro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td nowrap="nowrap" class="Arial10B">
                                                    Riesgo Buro:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRiesgoBuro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Deuda Financiera:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDeudaFinanciera" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td nowrap="nowrap" class="Arial10B">
                                                    Línea de Crédito:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLineaCredito" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 INI -->
                                                <td id= "tdSistemaE" class="Arial10B" colspan="6">
                                                    Sistema:
                                                </td>
                                                <!-- PROY-31948 FIN -->
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="120"> <!-- PROY-31948-->
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="120" align="left"> <!-- PROY-31948-->
                                                    Promedio Facturado
                                                </td>
                                                <td class="Arial10B" width="100"> <!-- PROY-31948-->
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="140" align="left"> <!-- PROY-31948-->
                                                    Monto aún NO Facturado
                                                </td>
                                                <td width="165"> <!-- PROY-31948 INI-->
                                                    &nbsp;
                                                </td>
                                                <td width="110"> <!-- PROY-31948 FIN-->
                                                    &nbsp;
                                                </td>
                                                <td width="120">
                                                    &nbsp;
                                                </td>
                                                <td width="110"> <!-- PROY-31948 -->
                                                    &nbsp;
                                                </td>
                                                <td nowrap="nowrap" id="tdTotalCuotasPendE"class="Arial10B" width="180"> <!-- PROY-31948 INI -->
                                                    Monto Pendiente en Cuotas:
                                                </td>
                                                 <td width="60" align="left" >
                                                    <asp:TextBox ID="txtTotalCuotasPendE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td nowrap="nowrap" id="tdCantLineasCuotaE" class="Arial10B" width="180">
                                                    Cantidad de Planes en Cuotas:
                                                </td>
                                                <td width="60" align="left" >
                                                    <asp:TextBox ID="txtCantLineasCuotaE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td nowrap="nowrap" id="tdCantMaxCuotasE"  class="Arial10B" width="180">
                                                    Cantidad Maxima de Cuotas: 
                                                </td>
                                                <td width="50" align="left">
                                                    <asp:TextBox ID="txtCantMaxCuotasE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox> 
                                                </td> <!-- PROY-31948 FIN -->
                                            </tr>
                                            <tr>
                                                <td class="Arial10B">
                                                    &nbsp;Movil
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFactMovil" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Movil
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoFactMovil" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdLcDisponibleMovil" class="Arial10B">
                                                    LC Disponible Móvil:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLcDisponibleMovil" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Monto Vencido:
                                                </td>
                                                <td colspan="7"> <!-- PROY-31948 -->
                                                    <asp:TextBox ID="txtMontoVencido" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B">
                                                    &nbsp;Internet Fijo
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFactInternet" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Internet Fijo
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoFactInternet" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdLcDisponibleInternet" class="Arial10B">
                                                    LC Disponible Internet Fijo:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLcDisponibleInternet" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Dias Vencimiento:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDiasVencimiento" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <!-- PROY-31948 INI -->
                                                <td id="tdUltimaVentaE" class="Arial10B" colspan="6">
                                                    Últimas ventas:
                                                </td>
                                                <!-- PROY-31948 FIN -->
                                            </tr>
                                            <tr>
                                                <td class="Arial10B">
                                                    &nbsp;Claro TV
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFactClaroTV" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Claro TV
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoFactClaroTV" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdLcDisponibleCable" class="Arial10B">
                                                    LC Disponible TV Fijo:
                                                </td>
                                                <td colspan="1"> <%--PROY-29121--%>
                                                    <asp:TextBox ID="txtLcDisponibleCable" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <%--PROY-29121- INI--%>
                                                <td id="td1" class="Arial10B">
                                                    Deuda:
                                                </td>
                                                 <td colspan="1"> 
                                                     <asp:Label ID="lblDeudaEmpresa" runat="server" Text="" Width="90px"  class="Arial10B">
                                                     </asp:Label>
                                                </td>
                                                <%--PROY-29121- FIN--%>
                                                <%-- PROY-31948 INI --%>
                                                <td id="tdTotalImportCuotaUltE" class="Arial10B" width="180">
                                                    Monto Pendiente en Cuotas:
                                                </td>
                                                 <td align="left" width="60" >
                                                    <asp:TextBox ID="txtTotalImportCuotaUltE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>

                                                <td id="tdCantTotalLineaCuotaUltE" class="Arial10B" width="180">
                                                    Cantidad de Planes en Cuotas:
                                                </td>
                                                <td align="left" width="60">
                                                    <asp:TextBox ID="txtCantTotalLineaCuotaUltE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdCantMaxCuotasGenUltE"class="Arial10B" width="180">
                                                    Cantidad Maxima de Cuotas: 
                                                </td>
                                                <td align="left" width="50">
                                                    <asp:TextBox ID="txtCantMaxCuotasGenUltE" CssClass="clsInputDisable" Width="50px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                             <%-- PROY-31948 FIN --%>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B">
                                                    &nbsp;Telefonía Fija
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFactTelefonia" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Telefonía Fija
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoFactTelefonia" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdLcDisponibleTelefonia" class="Arial10B">
                                                    LC Disponible Telefonía Fija:
                                                </td>
                                                <td colspan="9"> <!-- PROY-31948-->
                                                    <asp:TextBox ID="txtLcDisponibleTelefonia" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B">
                                                    &nbsp;BAM
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFactBAM" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    BAM
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoFactBAM" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td id="tdLcDisponibleBAM" class="Arial10B">
                                                    LC Disponible BAM:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLcDisponibleBAM" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td colspan="8"> <!-- PROY-31948 -->
                                                    <input class="Boton" id="btnDetalleLineaDesactiva" type="button" style="cursor: hand;
                                                        width: 205px; height: 19px" value="Líneas Desactivas y/o con Bloqueo" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                                <td id="tdLcDisponibleClie" class="Arial10BRed">
                                                    LC Disponible Cliente:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtLcDisponibleClie" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td colspan="8"> <!-- PROY-31948 -->
                                                    <input class="Boton" id="btnDetalleLinea" type="button" style="cursor: hand; width: 120px;
                                                        height: 19px" value="Ver Detalle Líneas" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDatosEvaluador">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        Información del Evaluador
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Resultado de la Evaluación:
                                                </td>
                                                <td colspan="4">
                                                    <input class="clsInputDisable" id="txtResultadoEval" readonly="readonly" size="45"
                                                        runat="server" />
                                                </td>
                                                    <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td id="tdBotonBRMS">
                                                    <input class="Boton" id="btnExportarBRMS" style="cursor: hand; width: 130px; height: 17px; float: right;"
                                                        type="button" value="Ver Parámetros ODM" />
                                                </td>
                                            </tr>
                                            <tr id="trExoneraRA">
                                                <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="150">
                                                    Exoneración RA:
                                                </td>
                                                <td class="Arial10B" colspan="4">
                                                    <asp:CheckBox ID="chkExoneracionRA" runat="server" Enabled="False" Text=" " Visible="True"
                                                        CssClass="Arial10B"></asp:CheckBox>
                                                </td>
                                                  <!--PROY 30748 - INICIO - Empresa -->
                                                <td id="tdBotonProacBRMS" colspan="2" align="right">
                                                    <input class="Boton" id="btnExportarProacBRMS" style="cursor: hand; width: 200px; height: 17px"
                                                        type="button" value="Ver Parámetros ODM Proactivo" />
                                                </td>
                                                <!--PROY 30748 - FIN-->
                                            </tr>
                                            <tr id="trRiesgoBuro">
                                                <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td id="tdRiesgoBuroEval" class="Arial10B" width="150">
                                                    Riesgo Buro:
                                                </td>
                                                <td id="tdTxtRiesgoBuroEval" class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtRiesgo" ReadOnly="True" Width="90px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="250">
                                                    CP:
                                                </td>
                                                <td class="Arial10B" id="tdImgPagador" runat="server">
                                                    <img id="imgPagador" runat="server" alt="" src="" width="100" height="22" />
                                                </td>
                                                <td width="120">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td id="tdLCDisponible" class="Arial10BRed" width="150">
                                                    LC Disponible Cliente:
                                                </td>
                                                <td class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtLCDisponible" runat="server" Width="90" ReadOnly="True" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10BRed" width="250">
                                                    Comportamiento Consolidado Cliente:
                                                </td>
                                                <td class="Arial10B">
                                                    <asp:TextBox ID="txtComportamiento" runat="server" Width="90" ReadOnly="True" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="120">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="100">
                                                    Tipo Garantía:
                                                </td>
                                                <td width="200">
                                                    <asp:TextBox ID="txtTipoGarantia" runat="server" Width="120" ReadOnly="True" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Importe Garantía:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMontoTotalDG" runat="server" Width="90" ReadOnly="True" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="120" style="height: 20px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" colspan="5">
                                                    <asp:CheckBox ID="chkPresentaPoderes" runat="server" Enabled="false" Text="No Requiere Presentar Poderes"
                                                        Visible="True" CssClass="Arial10B"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" colspan="6">
                                                    &nbsp;Resultado de Créditos:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" align="center" colspan="6">
                                                    <asp:GridView ID="gvResultadoCredito" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito_RowDataBound">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                                            <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="SOLIN_SUM_CAR_CON"
                                                                DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderText="Tipo Garantía">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTipoGarantia" CssClass="clsSelectEnable" runat="server"
                                                                        Width="135px" onclick="cambiarTipoGarantia(this.value);">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="NRO_CF_EVAL" />
                                                            <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="SOLIN_IMP_DG" DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvNroCargosFijos" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Garantías:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Monto Garantias (S/.)">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvMontoGarantias" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress='eventoSoloNumerosEnteros();' Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtGvTotalMontoGarantias" runat="server" ReadOnly="True" CssClass="clsInputDisable"
                                                                        Width="50px" Style="text-align: right"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PRDC_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="CF" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="gvResultadoCredito1" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito1_RowDataBound">
                                                        <RowStyle HorizontalAlign="Center" CssClass="TablaFilasGrid" />
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle HorizontalAlign="Center" Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                                            <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="SOLIN_SUM_CAR_CON"
                                                                DataFormatString="{0:0.00}" />
                                                            <asp:BoundField HeaderText="Tipo Garantía" DataField="GARANTIA_EVAL" />
                                                            <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="NRO_CF_EVAL" />
                                                            <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="SOLIN_IMP_DG" DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "NRO_CF_CRED")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Garantías:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Monto Garantias (S/.)" FooterStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "SOLIN_IMP_DG_MAN", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PRDC_CODIGO" Visible="False" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div style="text-align: center; width: 100%">
                                                        <div style="text-align: center; width: 880px">
                                                            <div id="container" style="overflow: scroll; overflow-x: hidden">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:GridView ID="gvResultadoCredito2" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito2_RowDataBound"
                                                        Width="880px">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Plan" DataField="plnv_descripcion" />
                                                            <asp:BoundField HeaderText="Producto" DataField="prdv_descripcion" />
                                                            <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="solin_sum_cf" DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderText="Tipo Garantía">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTipoGarantia" CssClass="clsSelectEnable" runat="server"
                                                                        Width="135px" onchange="cambiarTipoGarantia2(this.value);">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="solin_num_cf" />
                                                            <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="solin_importe" DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Nro de Cargos Fijos<br>Copiar a todos <input type='checkbox' onclick='copiarNroCargoFijo(this.checked);'/>">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvNroCargosFijos" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Garantías:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Monto Garantias (S/.)">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtGvMontoGarantias" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress='eventoSoloNumerosEnteros();' Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtGvTotalMontoGarantias" runat="server" ReadOnly="True" CssClass="clsInputDisable"
                                                                        Width="50px" Style="text-align: right"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="prdc_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="solin_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="solin_sum_cf" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField DataField="slpln_codigo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"
                                                                FooterStyle-CssClass="hiddencol" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="gvResultadoCredito3" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="gvResultadoCredito3_RowDataBound"
                                                        Width="880px">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Plan" DataField="plnv_descripcion" />
                                                            <asp:BoundField HeaderText="Producto" DataField="prdv_descripcion" />
                                                            <asp:BoundField HeaderText="Cargo Fijo Total (S/.)" DataField="solin_sum_cf" DataFormatString="{0:0.00}" />
                                                            <asp:BoundField HeaderText="Tipo Garantía" DataField="garantia_man" />
                                                            <asp:BoundField HeaderText="Nro de Cargos Fijos" DataField="solin_num_cf" />
                                                            <asp:BoundField HeaderText="Monto Garantias (S/.)" DataField="solin_importe" DataFormatString="{0:0.00}" />
                                                            <asp:TemplateField HeaderText="Nro de Cargos Fijos">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "solin_num_cf_man")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Garantías:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Monto Garantias (S/.)" FooterStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "solin_importe_man", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="prdc_codigo" Visible="False" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" align="center" colspan="6">
                                                    <asp:GridView ID="dgCostoInstalacion" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacion_RowDataBound"
                                                        ShowHeaderWhenEmpty="true">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                                            <asp:TemplateField HeaderText="Producto">
                                                                <ItemTemplate>
                                                                    <span>CLARO TV SAT</span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Kit Seleccionado" DataField="KIT" />
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Costo de Instalación:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCostoInstalacion" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                                                        Width="50px" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="dgCostoInstalacion1" runat="server" AllowCustomPaging="False" AllowPaging="False"
                                                        AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacion1_RowDataBound">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                                            <asp:TemplateField HeaderText="Producto">
                                                                <ItemTemplate>
                                                                    <span>CLARO TV SAT</span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Kit Seleccionado" DataField="KIT" />
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Costo de Instalación:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                                                        Width="50px" Style="text-align: right"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" align="center" colspan="6">
                                                    <asp:GridView ID="dgCostoInstalacionHFC" runat="server" AllowCustomPaging="False"
                                                        AllowPaging="False" AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacionHFC_RowDataBound"
                                                        ShowHeaderWhenEmpty="true">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                                            <asp:BoundField HeaderText="Plan Seleccionado" DataField="PLNV_DESCRIPCION" />
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Costo de Instalación:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCostoInstalacion" runat="server" CssClass="clsInputEnable" Width="50px"
                                                                        MaxLength="5" onkeypress="return soloMontosIngreso(event, this);" Style="text-align: right"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                                                        Width="50px" MaxLength="5" Style="text-align: right"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="dgCostoInstalacionHFC1" runat="server" AllowCustomPaging="False"
                                                        AllowPaging="False" AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="dgCostoInstalacionHFC1_RowDataBound">
                                                        <RowStyle CssClass="TablaFilasGrid" HorizontalAlign="Center" />
                                                        <HeaderStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <FooterStyle Wrap="False" CssClass="TablaTitulos" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="SEC" DataField="SOLIN_CODIGO" ItemStyle-CssClass="hiddencol"
                                                                HeaderStyle-CssClass="hiddencol" FooterStyle-CssClass="hiddencol" />
                                                            <asp:BoundField HeaderText="Producto" DataField="PRDV_DESCRIPCION" />
                                                            <asp:BoundField HeaderText="Plan Seleccionado" DataField="PLNV_DESCRIPCION" />
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL_EVAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    Total Costo de Instalación:
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Costo de Instalación (S/.)">
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container.DataItem, "COSTO_INSTAL", "{0:0.00}")%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="txtTotalCostoInstalacion" runat="server" CssClass="clsInputDisable"
                                                                        Width="50px" Style="text-align: right"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Comentario del PDV:
                                                </td>
                                                <td class="Arial10B" colspan="5">
                                                    <textarea class="inputTextArea" id="txtComentarioPdv" style="width: 600px; height: 45px"
                                                        rows="2" readonly="readonly" cols="72" runat="server"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Comentario para el PDV:
                                                </td>
                                                <td class="Arial10B" colspan="5">
                                                    <textarea class="inputTextArea" id="txtComentarioAlPdv" style="width: 600px; height: 45px"
                                                        rows="2" cols="72" runat="server"></textarea>
                                                </td>
                                            </tr>
                                            <tr id="trComentarioEvaluador">
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Comentario&nbsp;del&nbsp;Evaluador:
                                                </td>
                                                <td class="Arial10B" colspan="5">
                                                    <textarea class="inputTextArea" id="txtComentarioEvaluador" style="width: 600px;
                                                        height: 45px" rows="2" cols="72" runat="server"></textarea>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>

                        <!-- INI PROY-31948_Migracion %-->

                        <tr id="idTablaComentariosPDV">
													<td>
														<table style="WIDTH: 100%" class="TablaTitulos">
															<tr>
																<td width="300"><b>Comentarios finales para PDV</b>
																</td>
																<td width="540"></td>
																<td width="150"></td>
																<td width="150"></td>
															</tr>
															<tr>
																<td colspan="4">
                                                                    <asp:textbox id="txtComentarioFinalPDV" runat="server" Width="783px" MaxLength="1000" TextMode="MultiLine" Height="53px"></asp:textbox><br/>
																	<span id="sBann1">
																		<%=txtComentarioFinalPDV.MaxLength - txtComentarioFinalPDV.Text.Length%>
																		caracter(es) restante(s).</span>
																</td>
															</tr>
														</table>
														<table style="WIDTH: 100%" class="TablaTitulos">
															<tr>
																<td width="40%"><b>Comentarios finales para C&amp;A</b>
																</td>
																<td width="40%"></td>
																<td width="10%"></td>
																<td width="10%"></td>
															</tr>
															<tr>
																<td colspan="4">
                                                                    <asp:textbox id="txtComentarioFinalCA" runat="server" Width="780px" MaxLength="1000" TextMode="MultiLine" Height="60px"></asp:textbox><br/>
																	<span id="sBann2">
																		<%=txtComentarioFinalCA.MaxLength - txtComentarioFinalCA.Text.Length%>
																		caracter(es) restante(s).</span>
																</td>
															</tr>
														</table>
														<br>
													</td>
												</tr>

                        <!-- FIN PROY-31948_Migracion -->

                    </tr>
                    <tr id="trDatosPortabilidad">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Datos de la Solicitud de Portabilidad
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Cedente:
                                                </td>
                                                <td class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtCedente" runat="server" Width="120" ReadOnly="True" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="250">
                                                    Nro de folio de formato de portabilidad:
                                                </td>
                                                <td class="Arial10B">
                                                    <asp:TextBox ID="txtNroFormatoPort" runat="server" Width="100" ReadOnly="True" CssClass="clsInputDisable"
                                                        Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="160">
                                                    &nbsp;Modalidad:
                                                </td>
                                                <td class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtModalidad" runat="server" Width="120" ReadOnly="True" CssClass="clsInputDisable"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="250">
                                                    Cargo Fijo del Operador Cedente:
                                                </td>
                                                <td class="Arial10B">
                                                    <asp:TextBox ID="txtCargoFijoCedente" runat="server" Width="100" ReadOnly="True"
                                                        CssClass="clsInputDisable" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <img height="4" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" colspan="4" align="left">
                                                    <asp:DataGrid ID="dgListaArchivos" runat="server" Width="400px" AutoGenerateColumns="False"
                                                        ShowHeader="False">
                                                        <AlternatingItemStyle BackColor="#dddee2"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="Arial10B" BackColor="#E9EBEE"></ItemStyle>
                                                        <Columns>
                                                            <asp:BoundColumn DataField="ARCH_TIPO" HeaderText="Tipo de Archivo"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ARCH_NOMBRE" HeaderText="Nombre de Archivo Adjunto">
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn ItemStyle-CssClass="Arial10B" HeaderText="Accion">
                                                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <img id="imgArchivo" src="../../Imagenes/botones/btn_VerArchivo.gif" alt="Ver Archivo"
                                                                        onclick='javascript:verArchivo("<%# DataBinder.Eval(Container.DataItem, "ARCH_RUTA") %>")'
                                                                        style="cursor: hand;" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td> 
                    </tr>
                    <tr id="trDocumentosAdjuntos">
                        <td>
                            <table cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                                <tr>
                                    <td class="Header" align="left" height="20">
                                        &nbsp;Documentos Adjuntos
                                    </td>
                                    <td class="Header" align="left" width="20" height="20">
                                        <img onclick="verAcuerdos();" alt="Ver" src="../../Imagenes/ico_lupa.gif" border="0" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trArchivosAdjuntos">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Archivos Adjuntos
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="dgArchivos" runat="server" Width="100%" AutoGenerateColumns="false"
                                            ShowHeader="false">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" CssClass="TablaTitulos"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Width="100%" CssClass="Arial10B"></ItemStyle>
                                                    <ItemTemplate>
                                                        <a style="cursor: hand" alt="Ver Archivo" href='javascript:verArchivo("<%# DataBinder.Eval(Container.DataItem, "ARCH_RUTA")%>");'>
                                                            <asp:Label ID="lblNomArchivo" runat="server" CssClass="Arial10B" Text='<%# DataBinder.Eval(Container.DataItem, "ARCH_NOMBRE")%>'>
                                                            </asp:Label></a>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trComentarioPorta">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header">
                                        <table cellspacing="1" cellpadding="0" width="100%" align="center" border="0">
                                            <tr>
                                                <td class="Arial10B" align="left" width="30%" colspan="2" height="20">
                                                    &nbsp;Comentario
                                                </td>
                                                <td align="left">
                                                    <input class="BotonOptm" id="btnComentarioPorta" type="button" style="cursor: hand;
                                                        width: 180px; height: 19px" value="Agregar" />
                                                </td>
                                                <td align="left" style="display: none">
                                                    <asp:Button ID="btnComentario" runat="server" CssClass="BotonOptm" Style="width: 180px;
                                                        cursor: hand; height: 19px" Text="Agregar" OnClick="btnComentario_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="udpGrilla" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgComentarioPorta" runat="server" Width="95%" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                                    <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                                    <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                                    <Columns>
                                                        <asp:BoundField DataField="COMEC_FLA_COM_DES" HeaderText="Origen" />
                                                        <asp:BoundField DataField="COMEV_COMENTARIO" HeaderStyle-Width="70%" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Descripción" />
                                                        <asp:BoundField DataField="COMEC_USU_REG" HeaderText="Usuario" />
                                                        <asp:BoundField DataField="COMED_FEC_REG" HeaderText="Fecha Registro" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnComentario" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trComentario">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Comentario
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="dgComentario" runat="server" AutoGenerateColumns="False" EnableViewState="False"
                                            ShowHeaderWhenEmpty="true">
                                            <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                            <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                            <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                            <Columns>
                                                <asp:BoundField DataField="COMEV_COMENTARIO" HeaderText="DESCRIPCION" />
                                                <asp:BoundField DataField="COMED_FEC_REG" HeaderText="FECHA REGISTRO" />
                                                <asp:BoundField DataField="COMEC_FLA_COM_DES" HeaderText="ORIGEN COMENTARIO" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblHistorico" style="overflow: auto" cellspacing="1" cellpadding="0" width="100%"
                    border="0">
                    <tr>
                        <td>
                            <asp:GridView ID="dgHistorico" runat="server" Width="100%" AutoGenerateColumns="False"
                                EnableViewState="False">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Acción">
                                        <ItemTemplate>
                                            <a href='javascript:verDetalle("<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>", "<%# DataBinder.Eval(Container.DataItem, "PRDC_CODIGO")%>")'>
                                                <img src="../../Imagenes/ico_lupa.gif" border="0" alt='Ver Detalle'></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SOLIN_CODIGO" HeaderText="Nro SEC" />
                                    <asp:BoundField DataField="ESTAV_DESCRIPCION" HeaderText="Estado" />
                                    <asp:BoundField DataField="TDOCV_DESCRIPCION" HeaderText="Tipo Documento" />
                                    <asp:BoundField DataField="NUM_DOCU" HeaderText="Nro Documento" />
                                    <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="Nombre/Razón Social" />
                                    <asp:BoundField DataField="SOLID_FEC_REG" HeaderText="Fecha Registro" />
                                    <asp:BoundField DataField="OVENV_DESCRIPCION" HeaderText="Oficina Venta" />
                                    <asp:BoundField DataField="CANTIDAD_LINEAS" HeaderText="Cantidad Lineas" />
                                    <asp:BoundField DataField="TCESC_DESCRIPCION" HeaderText="Caso Especial" />
                                    <asp:BoundField DataField="TCARV_DESCRIPCION" HeaderText="Tipo Garantia" />
                                    <asp:BoundField DataField="SOLIN_IMP_DG_MAN" HeaderText="Importe" />
                                    <asp:BoundField DataField="MOTIVO_RECHAZO" HeaderText="Motivo Rechazo" />
                                    <%-- obs6: PROY-31948_Migracion INI--%>
                                    <asp:TemplateField HeaderText="Reingreso">																	
										 <ItemTemplate>													
												<%# FlagReingresoDes(DataBinder.Eval(Container.DataItem, "SOLIC_FLAG_REINGRESO"))%>																		
										</ItemTemplate>									
                                     </asp:TemplateField>
                                     <%--PROY-31948_Migracion FIN--%>
                                  <%--  <asp:BoundField DataField="SOLIC_FLAG_REINGRESO" HeaderText="Reingreso" />--%>
                                    <asp:BoundField DataField="ACTIVADOR_ID" HeaderText="Usuario" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblEstados" cellspacing="1" cellpadding="0" width="90%" border="0">
                    <tr>
                        <td>
                            <asp:GridView ID="dgEstados" runat="server" Width="60%" AutoGenerateColumns="False"
                                EnableViewState="False">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                <Columns>
                                    <asp:BoundField DataField="NROSEC" HeaderText="Nro SEC" />
                                    <asp:BoundField DataField="ESTAV_DESCRIPCION" HeaderText="Estado SEC" />
                                    <asp:BoundField DataField="HISEV_USUA_REG" HeaderText="Usuario" />
                                    <asp:BoundField DataField="HISED_FEC_REG" HeaderText="Fecha de Cambio" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblEstadosSGA" cellspacing="1" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <asp:GridView ID="dgEstadosSGA" runat="server" Width="60%" AutoGenerateColumns="False"
                                EnableViewState="False">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                <Columns>
                                    <asp:BoundField DataField="NROSOT" HeaderText="NroSOT" />
                                    <asp:BoundField DataField="ESTAV_DESCRIPCION" HeaderText="Descripcion" />
                                    <asp:BoundField DataField="HISED_FEC_REG" HeaderText="Fecha" />
                                    <asp:BoundField DataField="HISEV_USUA_REG" HeaderText="Cod. Usuario" />
                                    <asp:BoundField DataField="HISEV_COMENTARIO" HeaderText="Observacion" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <img height="2" alt="0" src="../../Imagenes/spacer.gif" width="100%" border="0" />
            </td>
        </tr>
        <tr id="trBotones">
            <td align="center" width="90%">
                <input class="Boton" id="btnCreditos" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Grabar Rentas" />&nbsp;
                <!--INI PROY-31948_Migracion -->
                <input class="Boton" id="btnAprobar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Aprobar" />&nbsp;
                <input class="Boton" id="btnRechazarReno" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Rechazar" />&nbsp;
                <input class="Boton" id="btnCancelarReno" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Cancelar" />&nbsp;
                <!--FIN PROY-31948_Migracion -->
                <input class="Boton" id="btnCerrar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Cerrar" />
            </td>
        </tr>
        <tr id="trBotonesPorta">
            <td align="center" width="90%">
                <input class="Boton" id="btnObservar" style="width: 126px; cursor: hand; height: 19px;
                    display: none" type="button" value="Observar" />&nbsp;
                <input class="Boton" id="btnSubsanar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Subsanar" />&nbsp;
                <input class="Boton" id="btnGrabar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Enviar a MP" />&nbsp;
                <input class="Boton" id="btnRechazar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Rechazar" />&nbsp;
                <input class="Boton" id="btnLiberar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Liberar Solicitud" />&nbsp;
                <input class="Boton" id="btnCancelar" style="width: 126px; cursor: hand; height: 19px"
                    type="button" value="Cancelar" />
            </td>
        </tr>
        <tr>
            <td>
                <input id="hidNroDoc" type="hidden" runat="server" />
                <input id="hidFlgConsulta" type="hidden" runat="server" />
                <input id="hidProceso" type="hidden" runat="server" />
                <input id="hidUsuarioExt" type="hidden" runat="server" />
                <input id="hidFlgChecklist" type="hidden" runat="server" />
                <input id="hidListaPerfiles" type="hidden" runat="server" />
                <input id="hidFlagPortabilidad" type="hidden" runat="server" />
                <input id="hidUsuarioAutorizador" type="hidden" runat="server" />
                <input id="hidGarantia" type="hidden" runat="server" />
                <input id="hidCostoInstalacion" type="hidden" runat="server" />
                <input id="hidFlgProductoDTH" type="hidden" runat="server" />
                <input id="hidTipoOperacion" type="hidden" runat="server" />
                <input id="hidListaRecibo" type="hidden" runat="server" />
                <input id="hidTiempoInicio" type="hidden" runat="server" />
                <input id="hidflgOrigenPagina" type="hidden" runat="server" />
                <input id="hidnMensajeValue" type="hidden" runat="server" />
                <input id="hidContFila" type="hidden" runat="server" />
                <input id="hidFlgConvergente" type="hidden" runat="server" />
                <input id="hidUsuario" type="hidden" runat="server" />
                <input id="hidFlgProductoHFC" type="hidden" runat="server" />
                <input id="hidFlgCombo" type="hidden" runat="server" />
                <input id="hidFlagPlanesProactivos" type="hidden" name="hidFlagPlanesProactivos" runat="server" />
                <%--//Inicio IDEA-30067--%>
                <input id="hidProductoPortAuto" type="hidden" name="hidProductoPortAuto" runat="server" />
                <%--//Fin IDEA-30067--%>
                <!-- Inicio 29121-->
                <input id="hidDeudaClienteEmpresa" type="hidden" name="hidDeudaClienteEmpresa" runat="server" />
                <!-- Fin 29121-->
                <!-- INI PROY-31948_Migracion -->
                <input id="hdnFlagEval" type="hidden" name="hdnFlagEval" runat="server" />
                <input id="hdnRA_DG" type="hidden" name="hdnRA_DG" runat="server" />
                <input id="hdnValRA_DG" type="hidden" name="hdnValRA_DG" runat="server"/> 
                <input id="hdncargo_fijo" type="hidden" name="hdncargo_fijo" runat="server"/>
                <input id="hdnGuardarAlCancelar" type="hidden" runat="server" />
                <input id="hdnflagHistorico" type="hidden" runat="server" />
                <!-- FIN PROY-31948_Migracion -->

            </td>
        </tr>
    </table>
    <iframe id="ifrmExportar_BRMS" style="width: 0px; height: 0px" />
    </form>
</body>
</html>
