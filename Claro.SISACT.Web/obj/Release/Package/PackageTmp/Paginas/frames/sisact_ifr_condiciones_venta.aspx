<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_condiciones_venta.aspx.cs"
    Inherits="Claro.SISACT.Web.frames.sisact_ifr_condiciones_venta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sisact_ifr_condiciones_venta</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link title="win2k-cold-1" media="all" href="../../estilos/calendar-blue.css" type="text/css"
        rel="stylesheet" />
    <script language='javascript' type='text/javascript' src='../../Scripts/funciones_sec.js'></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar_es.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendario_call.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar_setup.js"></script>
    <%--INICIATIVA - 733 - INI - C22--%>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
	<script language="javascript" src="../../../script/jquery-ui.js"></script>
	<script language='javascript' type='text/javascript' src='../../Scripts/security.js'></script>
    <%--<script src="../../Scripts/json2.js" type="text/javascript"></script>--%><!-- PROY-29123 -->
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script><!-- PROY-31636 -->
    <%--INICIATIVA - 733 - FIN - C22--%>
	<script type="text/javascript" language="JavaScript" src="../../Scripts/Lib_FuncValidacion.js"></script><!-- PROY-140546 -->

    <script language="JavaScript" type="text/javascript">


        function GetHeader() {
            var req = new XMLHttpRequest();
            req.open('GET', document.location, false);
            req.send(null);
            var lengHeader = req.getAllResponseHeaders().split('IE=').length;
            var retornoEmulador = '';
  
            if (lengHeader > 1)
                retornoEmulador = "OK";

            return retornoEmulador;

        }

        var columnaPaquete = 6; //PROY-140743
        var emuladorIE = GetHeader();
        var modoEdicion = 'NO';
        //PROY-140223 IDEA-140462
        var familiaFlag = '<%= ConfigurationManager.AppSettings["FamiliaPlanFlag"] %>';
//fin gaa20161020
        var flujoAlta = '<%= ConfigurationManager.AppSettings["flujoAlta"] %>';
        var flujoPortabilidad = '<%= ConfigurationManager.AppSettings["flujoPortabilidad"] %>';

        var topeConsumoAuto = '<%= ConfigurationManager.AppSettings["constCodTopeAutomatico"] %>';
        var topeConsumoCero = '<%= ConfigurationManager.AppSettings["constCodTopeCeroServicio"] %>';
        var topeConsumoSinCF = '<%= ConfigurationManager.AppSettings["constCodTopeSinCFServicio"] %>';

        var codConsumoAdicional = '<%= ConfigurationManager.AppSettings["constCodTopeAutomaticoServicio"] %>'; //PROY-29296
        var codConsumoExacto = '<%= ConfigurationManager.AppSettings["constCodTopeCeroServicio"] %>';   //PROY-29296
        var codConsumoAbierto = '<%= ConfigurationManager.AppSettings["constCodTopeSinCFServicio"] %>';  //PROY-29296

        var codTipoOficinaCAC = '<%= ConfigurationManager.AppSettings["constCodTipoOficinaCAC"] %>';

        var codTipoProdMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var tipoProductoBussiness = '<%= ConfigurationManager.AppSettings["constCodTipoProductoBUS"] %>';
        var codTipoProdFijo = '<%= ConfigurationManager.AppSettings["constTipoProductoFijo"] %>';
        var codTipoProdBAM = '<%= ConfigurationManager.AppSettings["constTipoProductoBAM"] %>';
        var codTipoProdDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        var codTipoProd3Play = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';
        var codTipoProdFTTH = '<%= ConfigurationManager.AppSettings["constTipoProductoFTTH"] %>'; // FTTH
        var codTipoProdVentaVarios = '<%= ConfigurationManager.AppSettings["constTipoProductoVentaVarios"] %>';
        var codModalidadPagoEnCuota = '<%= ConfigurationManager.AppSettings["constCodModalidadCuota"] %>';
        var codModalidadChipSuelto = '<%= ConfigurationManager.AppSettings["constCodModalidadChipSuelto"] %>';
        var codModalidadPagoEnCuotaSinCode = '<%= ConfigurationManager.AppSettings["constCodModalidadCuotaSinCode"] %>'; //INICIATIVA 920 INICIO
        var codModalidadContratoSinCode = '<%= ConfigurationManager.AppSettings["constCodModalidadContratoSinCode"] %>'; //INICIATIVA 920 INICIO
        var codModalidadContratoCede = '<%= ConfigurationManager.AppSettings["constCodModalidadContrato"] %>';
        var codPlazoAcuerdoSinPlazo = '<%= ConfigurationManager.AppSettings["constCodPlazoAcuerdoSinPlazo"] %>';
        var codTipoOperMigracion = '<%= ConfigurationManager.AppSettings["constTipoOperacionMIG"] %>';
        var codTipoProd3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';
        var codTipoProdInterInalam = '<%= ConfigurationManager.AppSettings["constTipoProductoInterInalam"] %>';
        var codValorDefectoTope = '<%= ConfigurationManager.AppSettings["constValorTopeDefecto"] %>';
        var codServProteccionMovil = '';
        var concatCodTipoPdvProteccionMovil = '';
        var concatCodTipoOfertaProteccionMovil = '';
        var concatCodTipoModalidadVentaProteccionMovil = '';
        var strServicioProteccionMovil = '';
        var vbCargaIni = true;
        var constCodTipoProductoB2E = '<%= ConfigurationManager.AppSettings["constCodTipoProductoB2E"] %>';
        var ConsPlanoCampanaFTTH = '<%= ConfigurationManager.AppSettings["ConsPlanoCampanaFTTH"] %>';
        var constCETrabajadoresCMA = '<%= ConfigurationManager.AppSettings["constCETrabajadoresCMA"] %>';
        var GvarFamiliaPlan = '<%= ConfigurationManager.AppSettings["varFamiliaPlan"] %>';
        var constPlazoConEquipo = '<%= ConfigurationManager.AppSettings["constPlazoConEquipo"] %>';
        var constConf3PITelefonia = '<%= ConfigurationManager.AppSettings["constConf3PITelefonia"] %>';
        var constConf3PIClaroTVDig = '<%= ConfigurationManager.AppSettings["constConf3PIClaroTVDig"] %>';
        var constConf3PIClaroTVAna = '<%= ConfigurationManager.AppSettings["constConf3PIClaroTVAna"] %>';
        var constConf3PIInternet = '<%= ConfigurationManager.AppSettings["constConf3PIInternet"] %>';
        var constMsgNoTelefInterMigra = '<%= ConfigurationManager.AppSettings["constMsgNoTelefInterMigra"] %>';
        var constCodModalidadCuota = '<%= ConfigurationManager.AppSettings["constCodModalidadCuota"] %>';
        var consCampanaDiaEnamorados = '<%= ConfigurationManager.AppSettings["CampanaDiaEnamorados"] %>';
        var FamiliaPlanxDefecto = '<%= ConfigurationManager.AppSettings["FamiliaPlanxDefecto"] %>';
        var TipoProductoBusiness = '<%= ConfigurationManager.AppSettings["TipoProductoBusiness"] %>';
        var constMsgNoClaroTvMigra = '<%= ConfigurationManager.AppSettings["constMsgNoClaroTvMigra"] %>';
        var codServRoamingI = '<%= ConfigurationManager.AppSettings["codServRoamingI"] %>';
        var codPlazoDeterminado = '<%= ConfigurationManager.AppSettings["codPlazoDeterminado"] %>';
        var IdProductoServicioEmail = '<%= ConfigurationManager.AppSettings["IdProductoServicioEmail"] %>';
        var constCodTopeAutomaticoServicio = '<%= ConfigurationManager.AppSettings["constCodTopeAutomaticoServicio"] %>';
        var constCodTopeCeroServicio = '<%= ConfigurationManager.AppSettings["constCodTopeCeroServicio"] %>';
        var FamiliaPlanLocal = '<%= ConfigurationManager.AppSettings["FamiliaPlanLocal"] %>';
        var constMontoTope = '<%= ConfigurationManager.AppSettings["constMontoTope"] %>';
        var constMsjErrorConfigListaPrecio = '<%= ConfigurationManager.AppSettings["constMsjErrorConfigListaPrecio"] %>';
        var ConstPlanSmart25P = '<%= ConfigurationManager.AppSettings["ConstPlanSmart25P"] %>';
        var ConstMensajeExcedePlanes = '<%= ConfigurationManager.AppSettings["ConstMensajeExcedePlanes"] %>';
        var ConstCasoEspBBEnamorados = '<%= ConfigurationManager.AppSettings["ConstCasoEspBBEnamorados"] %>';
        var ConstCasoEspSmartEnamorados = '<%= ConfigurationManager.AppSettings["ConstCasoEspSmartEnamorados"] %>';
        var PlanesClaroExactoIlimitado = '<%= ConfigurationManager.AppSettings["PlanesClaroExactoIlimitado"] %>';
        var consPlanesModemCIlimitado = '<%= ConfigurationManager.AppSettings["PlanesModemCIlimitado"] %>';
        var constCodPlanesRPC = '<%= ConfigurationManager.AppSettings["constCodPlanesRPC"] %>';
        var CodPlanes2PlayCorporativo = '<%=ConfigurationManager.AppSettings["CodPlanes2PlayCorporativo"] %>';
        var BloqueoEquipoSinStockColor = '<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>';
        var constMsjEquipoSinStockSel = '<%= ConfigurationManager.AppSettings["constMsjEquipoSinStockSel"] %>';
        var constRoamingCAC = '<%= ConfigurationManager.AppSettings["constRoamingCAC"] %>';
        var constRoamingDAC = '<%= ConfigurationManager.AppSettings["constRoamingDAC"] %>';
        var constRoamingCorner = '<%= ConfigurationManager.AppSettings["constRoamingCorner"] %>';
        var constRoamingMasivo = '<%= ConfigurationManager.AppSettings["constRoamingMasivo"] %>';
        var constRoamingB2E = '<%= ConfigurationManager.AppSettings["constRoamingB2E"] %>';
        var constRoamingCorporativo = '<%= ConfigurationManager.AppSettings["constRoamingCorporativo"] %>';
        var constLimiteCredito = '<%= ConfigurationManager.AppSettings["constLimiteCredito"] %>';
        var constCodTipoOficinaCorner = '<%=  ConfigurationManager.AppSettings["constCodTipoOficinaCorner"] %>';
        var CampanaDiaEnamoradosAsociada = '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"] %>';

        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }

    </script>
    <script type="text/javascript" language="javascript" src="../../Scripts/Paginas/frames/sisact_ifr_condiciones_venta.js"></script>
</head>
<body onload="inicio();" style="margin: 0px;">
    <form id="frmPrincipal2" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table class="Contenido3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr id="trGrilla">
                        <td>
                            <div class="clsGridRow1" id="divCondVent" style="width: 980px; height: 175px;">
                                <div class="clsGridRow" id="divGrillaCabecera" style="width: 970px; overflow: hidden">
                                    <table id="tblTablaTituloMovil" style="border-color:#95b7f3" cellspacing="0" cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
                                            <td class="TablaTitulos" id="tdPromocion" style="DISPLAY: none" align="center" width="190">Promocion</td> <%--PROY-140743 | Venta en cuotas --%>
                                            <td class="TablaTitulos" align="center" style="DISPLAY: none" width="145">Plazo Cuotas</td><%--PROY-140743 | Venta en cuotas --%>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteMovil" style="DISPLAY: none" align="center" width="200">Paquete</td>
                                            <td class="TablaTitulos" id="tdFamiliaPlanMovil" style="DISPLAY: none" align="center" width="150">Familia Plan</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
											<td class="TablaTitulos" id="tdMontoTope" align="center" width="50">Monto Tope S/.</td><%--PROY-140743 | Venta en cuotas --%>
                                            
                                             <script language="JavaScript" type="text/javascript">
                                                 if (emuladorIE == '')
                                                     document.write("<td class='TablaTitulos' id='tdEquipoMovil' align='center' width='190'>Equipo</td>");
                                                    else                          
                                                    document.write("<td class='TablaTitulos' id='tdEquipoMovil' align='center' width='250'>Equipo</td>");
                                              </script>
											 
											<td class="TablaTitulos" id="tdCuotaMovil" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" id="tdEquipoPrecioMovil" align="center" width="60">Precio Equipo</td>
                                            <!-- INI - PROY-140743 - Venta Accesorios Cuotas -->
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Linea/Cuenta</td>
                                            <!-- FIN - PROY-140743 - Venta Accesorios Cuotas -->
											<td class="TablaTitulos" id="tdTelefonoMovil" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
                                            <td class="TablaTitulos" id="tdEstadoCPMovil" style="DISPLAY: none" align="center" width="200">Estado CP</td>
                                            <td class="TablaTitulos" id="tdFechaCPMovil" style="DISPLAY: none" align="center" width="70">Fecha CP</td>
                                            <td class="TablaTitulos" id="tdDeudaCPMovil" style="DISPLAY: none" align="center" width="50">Deuda CP</td>
										</tr>
									</table>
									<table id="tblTablaTituloFijo" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteFijo" style="DISPLAY: none" align="center" width="200">Paquete</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
                                            <td class="TablaTitulos" align="center" width="50">Monto Tope S/.</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
											<td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar
											</td>
										</tr>
									</table>
									<table id="tblTablaTituloBAM" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteBAM" style="DISPLAY: none" align="center" width="200">Paquete</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
											<td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
                                            <td class="TablaTitulos" id="tdEstadoCPBAM" style="DISPLAY: none" align="center" width="200">Estado CP</td>
                                            <td class="TablaTitulos" id="tdFechaCPBAM" style="DISPLAY: none" align="center" width="70">Fecha CP</td>
                                            <td class="TablaTitulos" id="tdDeudaCPBAM" style="DISPLAY: none" align="center" width="50">Deuda CP</td>                                            
                                            
										</tr>
									</table>
									<table id="tblTablaTituloDTH" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
                                            <td class="TablaTitulos" align="center" width="150">Plazo</td>
											<td class="TablaTitulos" align="center" width="200">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Paq. Adic.</td>
											<td class="TablaTitulos" align="center" width="200">Kits</td>
											<td class="TablaTitulos" align="center" width="50">CF Plan Mensual</td>
											<td class="TablaTitulos" align="center" width="50">CF Men. Alq. Kit</td>
											<td class="TablaTitulos" align="center" width="50">Tot. CF Mensual</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
										</tr>
									</table>
									<table id="tblTablaTituloHFC" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" align="center" width="160">Solución</td>
											<td class="TablaTitulos" align="center" width="300">Paquete</td>
											<td class="TablaTitulos" align="center" width="275">Servicios</td>
                                            <td class="TablaTitulos" align="center" width="200">Tope Consumo</td>
											<td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
											<td class="TablaTitulos" align="center" width="40">Alq. Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
											<td class="TablaTitulos" align="center" width="35">Det. Ofert.</td>
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
										</tr>
									</table>
                                    <%--FTTH - Inicio - tblTablaTituloFTTH--%>
                                    <table id="tblTablaTituloFTTH" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
                                           cellpadding="0" border="0">
                                        <tr>
                                            <td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
                                            <td class="TablaTitulos" align="center" width="145">Plazo</td>
                                            <td class="TablaTitulos" align="center" width="160">Solución</td>
                                            <td class="TablaTitulos" align="center" width="300">Paquete</td>
                                            <td class="TablaTitulos" align="center" width="275">Servicios</td>
                                            <td class="TablaTitulos" align="center" width="200">Tope Consumo</td>
                                            <td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
                                            <td class="TablaTitulos" align="center" width="40">Alq. Equipo</td>
                                            <td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
                                            <td class="TablaTitulos" align="center" width="35">Det. Ofert.</td>
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
                                                Portar</td>
                                        </tr>
                                    </table>
                                    <%--FTTH - Fin - tblTablaTituloFTTH--%>
									<table id="tblTablaTituloVentaVarios" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
                                            <td class="TablaTitulos" align="center" width="150">Lista Precio</td>
											<td class="TablaTitulos" align="center" width="60">Precio</td>
											<td class="TablaTitulos" align="center" width="35" style="display:none">Cuotas</td>
										</tr>
									</table>
                                    <table id="tblTablaTitulo3PlayInalam" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" align="center" width="160">Solución</td>
											<td class="TablaTitulos" align="center" width="300">Paquete</td>
											<td class="TablaTitulos" align="center" width="275">Servicios</td>
                                            <td class="TablaTitulos" align="center" width="200">Tope Consumo3</td>
											<td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
											<td class="TablaTitulos" align="center" width="40">Alq. Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
											<td class="TablaTitulos" align="center" width="35">Det. Ofert.</td>
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar3</td>
										</tr>
									</table>
                                    <%-- PROY-31812 - IDEA-43340 - INICIO --%>
                                    <table id="tblTablaTituloInterInalam" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
                                            <td class="TablaTitulos" align="center" width="150">Plan</td>
                                            <td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
                                            <td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
                                            <td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
                                            <td class="TablaTitulos" align="center" width="30">Detalle Oferta.</td>
										</tr>
									</table>
                                    <%-- PROY-31812 - IDEA-43340 - FIN --%>
                                </div>
                                <div class="clsGridRow" id="divGrillaDetalle" style="width: 950px; height: 100px">
                                    <table id="tblTablaMovil" style="border-color:#95b7f3" cellspacing="0" cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaFijo" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaBAM" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaDTH" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaHFC" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <%--  FTTH Inicio- tblTablaFTTH Vista--%>
                                    <table id="tblTablaFTTH" style="display: none; border-color:#95b7f3" cellspacing="0"
                                           cellpadding="0" border="0">
                                    </table>
                                     <%--  FTTH Fin- tblTablaFTTH Vista--%>
                                    <table id="tblTablaVentaVarios" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                     <table id="tblTabla3PlayInalam" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                   <%-- PROY-31812 - IDEA-43340 - INICIO --%>
                                     <table id="tblTablaInterInalam" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <%-- PROY-31812 - IDEA-43340 - FIN --%>
                                </div>
                                <table id="tblCFTotal" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="Arial10B" style="white-space:normal" align="center">
                                            CF Total:&nbsp;&nbsp;
                                            <input class="clsInputDisable" id="txtCFTotal" style="width: 50px; text-align: right"
                                                readonly="readonly" name="txtCFTotal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="Contenido" id="tblServicios" style="display: none" height="270" cellspacing="1"
                                cellpadding="0" width="500" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header">
                                        Servicios Adicionales - Plan
                                        <asp:Label ID="lblIdLista" runat="server" CssClass="Arial10B" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr valign="top">
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Servicios Adicionales</u>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Servicios Contratados </u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <select class="clsSelectEnable" id="lbxServiciosDisponibles1" style="width: 310px"
                                                        onclick="mostrarPromociones('D', this.value)" size="5" name="lbxServiciosDisponibles1">
                                                    </select>
                                                </td>
                                                <td class="Arial10B" style="width: 150px; background-color: white" valign="top" align="center">
                                                    <input class="Boton" id="btnAgregarServicio" onmouseover="this.className='BotonResaltado';"
                                                         style="width: 90px; cursor: hand; height: 19px" onclick="javascript:ValidarServicioExcluyentes();" 
                                                        onmouseout="this.className='Boton';" type="button" value="Agregar >" name="btnAgregarServicio" /><br /> <!-- PROY-29296-->
                                                    <br />
                                                    <input class="Boton" id="btnQuitarServicio" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 90px; cursor: hand; height: 19px" onclick="javascript:quitarServicio();"
                                                        onmouseout="this.className='Boton';" type="button" value="< Quitar" name="btnQuitarServicio" /><br />
                                                    <br />
                                                    <input class="Boton" id="btnResetServicios" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 90px; cursor: hand; height: 19px" onclick="javascript:resetServicio('');"
                                                        onmouseout="this.className='Boton';" type="button" value="Limpiar" name="btnResetServicios" /><br />
                                                    <br />
                                                    <input class="Boton" id="btnCerrarServicios" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarServicio('c');"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarServicios" /> <!--INC000002245048-->
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <select class="clsSelectEnable" id="lbxServiciosAgregados1" style="width: 310px"
                                                        onclick="mostrarPromociones('A', this.value)" size="5" name="lbxServiciosAgregados1">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr id="trPromocion" style="display: none" valign="top">
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Promociones Adicionales</u>
                                                </td>
                                                <td>
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Promociones Agregadas</u>
                                                </td>
                                            </tr>
                                            <tr id="trPromocion1" style="display: none" valign="top">
                                                <td>
                                                    <select class="clsSelectEnable" id="lbxPromocionesDisponibles" style="width: 310px"
                                                        size="5" name="lbxPromocionesDisponibles">
                                                    </select>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <select class="clsSelectEnable" id="lbxPromocionesAgregadas" style="display: none;
                                                        width: 270px" size="5" name="lbxPromocionesAgregadas">
                                                    </select><select class="clsSelectEnable" id="lbxPromocionesSeleccionadas" style="width: 310px"
                                                        size="5" name="lbxPromocionesSeleccionadas"></select>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" id="tblRoamingI" style="display: none" cellspacing="1" cellpadding="0"
                                            width="200" align="right" border="0" runat="server">
                                            <tr>
                                                <td class="Contenido">
                                                    <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="Arial10B" style="background-color: white">
                                                                PLAZO
                                                            </td>
                                                            <td class="Arial10B" style="width: 320px">
                                                                <input id="rbtValDeterminado" onclick="f_cambiarPlazoRI(this.value)" type="radio"
                                                                    value="01" name="rbtTipoPlazo" />Determinado
                                                            </td>
                                                            <td class="Arial10B" style="width: 400px">
                                                                <input id="rbtValIndeterminado" onclick="f_cambiarPlazoRI(this.value)" type="radio"
                                                                    value="02" name="rbtTipoPlazo" />Indeterminado
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Arial10B" id="tdLblFechaDesde" style="display: none">
                                                                Desde:
                                                            </td>
                                                            <td id="tdTxtFechaDesde" style="display: none" colspan="2">
                                                                <asp:TextBox ID="txtFechaDesde" runat="server" Width="64px" CssClass="clsInputDisable"
                                                                    ReadOnly="true" MaxLength="10"></asp:TextBox>
                                                                <asp:ImageButton ID="btnFechaDesde" runat="server" ImageUrl="../../Imagenes/Botones/btn_Calendario.gif" />
                                                                <script type="text/javascript">
                                                                    Calendar.setup(
																				{
																				    inputField: "txtFechaDesde",      // id of the input field
																				    ifFormat: "%d/%m/%Y",           // format of the input field                                                        
																				    button: "btnFechaDesde",      // trigger for the calendar (button ID)
																				    singleClick: true,                 // double-click mode
																				    step: 1                     // show all years in drop-down boxes (instead of every other year as default)
																				}
																			);
                                                                </script>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Arial10B" id="tdLblFechaHasta" style="display: none">
                                                                Hasta:
                                                            </td>
                                                            <td id="tdTxtFechaHasta" style="display: none" colspan="2">
                                                                <asp:TextBox ID="txtFechaHasta" runat="server" Width="64px" CssClass="clsInputDisable"
                                                                    ReadOnly="true" MaxLength="10"></asp:TextBox><asp:ImageButton ID="btnFechaHasta" runat="server"
                                                                        ImageUrl="../../Imagenes/Botones/btn_Calendario.gif"></asp:ImageButton><script type="text/javascript">
                                                                                                                                                 Calendar.setup(
																			{
																			    inputField: "txtFechaHasta",      // id of the input field
																			    ifFormat: "%d/%m/%Y",           // format of the input field                                                        
																			    button: "btnFechaHasta",      // trigger for the calendar (button ID)
																			    singleClick: true,                 // double-click mode
																			    step: 1                     // show all years in drop-down boxes (instead of every other year as default)
																			}
																		);
                                                                        </script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            
                            <!-- INI - PROY-140743 - Venta Accesorios Cuotas -->
                            <table id="tbLineasFacturar" style="display: none; width: 300px; border: 1px solid white; margin: 0 auto; padding: 5px"  
                            class="Contenido" cellspacing="1" cellpadding="0" align="center" runat="server">
                                    <thead>
                                        <tr>
                                            <td class="Header" align="left" id="cabeceraLineaPU" height="auto" colspan="4" style="position: relative">Linea a facturar</td>
                                        </tr>
                                    </thead>
                                    <tbody class="Contenido">
                                        <tr>
                                            <td class="Arial10B" style="background-color: white; width: 70px" valign="top">Producto :
                                            </td>
                                            <td class="Arial10B" style="background-color: white" valign="top">
                                                <select class="clsSelectEnable" id="ddlProductoPU" style="width: 100px" onchange="cargarLineasCuentas()" name="ddlProductoPU" onclick=""></select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Arial10B" style="background-color: white; width: 70px" valign="top">Línea :</td>
                                            <td class="Arial10B" style="background-color: white" valign="top">
                                                <select class="clsSelectEnable" id="ddlLineasPU" style="width: 150px" onchange="" name="ddlLineasPU" onclick=""></select>
                                            </td>
                                        </tr>

                                        <tr>
                                         <td></td>
                                         <td><asp:Label ID="lblDireccion" runat="server" CssClass="clsInputEnable"></asp:Label> </td>
                                        </tr>

                                        <tr>
                                            <td align="center" colspan="4" style="background-color: white" >
                                                <input class="Boton" id="btnCerrarLineas" onmouseover="this.className='BotonResaltado';"
                                                    style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarLinea();"
                                                    onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar"
                                                    name="btnCerrarLineas" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <!-- FIN - PROY-140743 - Venta Accesorios Cuotas -->

                            <table class="Contenido" id="tblFormnCuotas" style="display: none" cellspacing="1" cellpadding="0"
                                width="400" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header" height="20">
                                        Cuotas
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Nro Cuotas
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <select class="clsSelectEnable0" id="ddlNroCuotas" style="width: 100px" onchange="cambiarSelecCuota(this.value)"
                                                        name="ddlNroCuotas">
                                                    </select>
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                 <!--   % Cuota Inicial -->
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtPorcCuotaIni" style="width: 50px; text-align: right; display:none"
                                                        readonly="readonly" type="text" name="txtPorcCuotaIni" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Monto Cuota Inicial
                                                </td>
                                                <!-- PROY-30166-IDEA–38863-INICIO -->
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputEnabled" id="txtMontoCuotaIni" style="width: 50px; text-align: right"
                                                        onchange="return CambiarMontoIni()" onkeypress="eventoSoloNumerosEnteros();" type="text" name="txtMontoCuotaIni" onpaste="return false;"  />
                                                </td>
                                                <!-- PROY-30166-IDEA–38863-FIN -->
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Monto Cuota
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtMontoCuota" style="width: 50px; text-align: right"
                                                        readonly="readonly" type="text" name="txtMontoCuota" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <input class="Boton" id="btnCerrarCuotas" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarCuotaBRMS();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarCuotas" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <!-- Proy 29123 -->
                                                    <label id="lblMensajeCuotas" style="font-weight: bold;color:black"></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table class="Contenido" id="tblEquipos" style="display: none" cellspacing="1" cellpadding="0"
                                width="650" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Arial10B" valign="top" align="center" width="45%">
                                        <table style="height:20" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="TablaTitulos" align="center" width="100%">
                                                    ALQUILER DE EQUIPOS
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table style="height:25" cellspacing="2" cellpadding="0" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Equipo&nbsp;&nbsp;
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <select class="clsSelectEnable0" id="ddlEquipo3Play" style="width: 170px" name="ddlEquipo3Play"
                                                        onchange="cambiarEquipo3Play(this.value)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    CF Alquiler&nbsp;&nbsp;
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtPrecAlqEquipo3Play" style="width: 50px" readonly="readonly"
                                                        type="text" name="txtPrecAlqEquipo3Play" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                    <td align="center" width="10%">
                                        <input class="Boton" id="btnAgregarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 90px; cursor: hand; height: 19px" onclick="javascript:agregarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="Agregar >" name="btnAgregarEquipo3Play" /><br />
                                        <br />
                                        <input class="Boton" id="btnQuitarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 90px; cursor: hand; height: 19px" onclick="javascript:quitarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="< Quitar" name="btnQuitarEquipo3Play" /><br />
                                        <br />
                                        <input class="Boton" id="btnCerrarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarEquipo3Play" />
                                    </td>
                                    <td class="Arial10B" valign="top" align="center" width="45%">
                                        <table style="height:20" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="TablaTitulos" align="center" width="100%">
                                                    RESUMEN DE ALQUILER DE EQUIPOS
                                                </td>
                                            </tr>
                                        </table>
                                        <b />
                                        <table style="height:25" cellspacing="2" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <div class="clsGridRow" style="width: 200PX; height: 100px; overflow: auto">
                                                        <table id="tblResumenAlqEquipo3Play" style="border-color:#95b7f3" cellspacing="0" cellpadding="0"
                                                            width="100%" border="0">
                                                            <tr>
                                                                <td class="TablaTitulos" align="center">
                                                                </td>
                                                                <td class="TablaTitulos" align="center">
                                                                    Equipo
                                                                </td>
                                                                <td class="TablaTitulos" align="center">
                                                                    CF Alq.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <table cellpadding="1" width="100%" border="0">
                                                        <tr>
                                                            <td class="Arial10B" align="center">
                                                                Total CF Alquiler:&nbsp;&nbsp;<input class="clsInputDisable" id="txtCFTotalEquipo"
                                                                    style="width: 50px; text-align: right" readonly name="txtCFTotalEquipo" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!-- INI PROY-29296 -->
                            <br />
                            <table class="Contenido" id="tbTopeConsumos" style="display: none" cellspacing="1"
                                cellpadding="0" height="180" width="400" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header">
                                        Tope de Consumos
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <u>Monto de tope de consumo</u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <select class="clsSelectEnable" id="lbxTopeConsumoDisponibles" style="width: 210px"
                                                        size="5" name="lbxTopeConsumoDisponibles">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <input class="Boton" id="btnGuardarTopeConsumo" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarTopeConsumo();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarTopeConsumo" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table class="Contenido" id="tbTopeConsumosLTE" style="display: none" cellspacing="1"
                                cellpadding="0" height="180" width="400" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header">
                                        Tope de Consumos
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <u>Topes de consumos</u>
                                                </td>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <u>Monto de tope de consumo</u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <select class="clsSelectEnable" id="lbxIdTope" style="width: 210px" size="5" name="lbxIdTope" onchange="LlenarTablaMontosTopeConsumoLTE(this);">
                                                    </select>
                                                </td>
                                                <td class="Arial10B" style="background-color: white; text-align: center;">
                                                    <select class="clsSelectEnable" id="lbxMonto" style="width: 210px" size="5" name="lbxMonto">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white; text-align: center;" colspan="2">
                                                    <input class="Boton" id="btnGuardarTopeConsumoLTE" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarTopeConsumoLTE();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarTopeConsumoLTE" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!-- FIN PROY-29296 -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trResumenCompras" style="display: none">
            <td>
                <table cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="Header" align="left" height="18">
                            &nbsp;Resumen de Compras
                        </td>
                    </tr>
                </table>
                <table id="tbResumenCompras" style="border-color:#95b7f3" cellspacing="0" cellpadding="0"
                    width="100%" border="0">
                    <tr>
                        <td class="TablaTitulos" align="center">
                            Tipo de Producto
                        </td>
                        <td class="TablaTitulos" align="center">
                            Plazo
                        </td>
                        <td class="TablaTitulos" align="center">
                            Grupo Producto
                        </td>
                        <td class="TablaTitulos" align="center">
                            Plan
                        </td>
                        <td class="TablaTitulos" align="center">
                            Campaña
                        </td>
                        <td class="TablaTitulos" align="center">
                            Equipo
                        </td>
                        <td class="TablaTitulos" align="center">
                            Cargo Fijo Total
                        </td>
                        <td class="TablaTitulos" align="center">
                            Precio de Venta Equipo
                        </td>
                        <!-- PROY-30166-IDEA–38863-INICIO -->
                        <td class="TablaTitulos" align="center">
                            Monto Inicial
                        </td>
                        <!-- PROY-30166-IDEA–38863-FIN -->
                        <td class="TablaTitulos" align="center">
                            Equipo en Cuotas
                        </td>
                        <td class="TablaTitulos" align="center">
                            Nro. Cuotas
                        </td>
                        <td class="TablaTitulos" align="center">
                            Monto Cuota
                        </td>
                        <td class="TablaTitulos" align="center">
                            Precio Inst.
                        </td>
			<!--PROY-24724-IDEA-28174 - INICIO-->
			<td class="TablaTitulos" align="center" style="display:none">
                            P.M.
                        </td>
			<!--PROY-24724-IDEA-28174 - FIN-->
                        <!--PROY-29215 - INICIO-->
                        <td id="tdFPago" class="TablaTitulos" align="center" style="display: none">
                            Forma de Pago
                        </td>
                        <td id="tdCuota" class="TablaTitulos" align="center" style="display: none">
                            Cuotas
                        </td>
                        <td id="tdCM" class="TablaTitulos" align="center" style="display: none">
                            CM
                        </td>
                        <!--PROY-29215 -FIN-->
                        <!--PROY-140546 -INICIO-->
                        <td id="tdMAI" class="TablaTitulos" align="center" style="display: none">
                            MAI
                        </td>
                        <!--PROY-140546 -FIN-->
                        <!--[PROY-140335 RF1] - INICIO-->
                        <td class="TablaTitulos" align="center">
                           Número
                        </td>
                        <td class="TablaTitulos" align="center">
                            E.CP
                        </td>
                        <!--[PROY-140335 RF1] -FIN-->
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
		<!--PROY-24724-IDEA-28174 - INICIO-->
		<table id="tbProteccionMovil"   class="Contenido" style="display:none ; width:100%">
                        <tr>
							<td width="150"></td>
                            <td class="Arial10B">&nbsp;</td>
							<td class="Arial10B">&nbsp;</td>
                            <td class="Arial10B">&nbsp;</td>
                           
						</tr>
						<tr>
							<td width="150"></td>
                            <td class="Arial10B" width="150">Equipos Asegurados:</td>
							<td class="Arial10B">
                            <input class="clsInputDisable" id="txtCantEquAsegurados"  value="0" style="width: 50px; text-align: right"
                            readonly="readonly" name="txtCantEquAsegurados" /></td>
                            <td class="Arial10B"></td>
                        </tr>
						<tr>
							<td width="150">&nbsp;</td>
                            <td class="Arial10B" width="150">Total Prima:</td>
							<td class="Arial10B" width="150">
                            <input class="clsInputDisable" id="txtTotalPrima" value="0.00" style="width: 50px; text-align: right"
                             readonly="readonly" name="txtTotalPrima" /></td>
                            <td class="Arial10B">&nbsp;</td>
						</tr>
                        <tr>
							<td width="150">&nbsp;</td>
                            <td class="Arial10B" colspan="2" align="center">
                            <input class="BotonOptm" id="btnConsultaPM" style="WIDTH: 50px; CURSOR: hand" 
                            onclick=""	type="button" value="..." /></td>
                            <td class="Arial10B">&nbsp;</td>
                        </tr>
                        <tr>
							<td width="150">&nbsp;</td>
                            <td class="Arial10B">&nbsp;</td>
							<td class="Arial10B">&nbsp;</td>
                            <td class="Arial10B">                            
                            </td>
                        </tr>
                </table>
		<!--PROY-24724-IDEA-28174 - FIN-->
                <table cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" align="center">
                            CF Total:&nbsp;&nbsp;<input class="clsInputDisable" id="txtCFTotalCarrito" style="width: 50px;
                                text-align: right" readonly="readonly" name="txtCFTotalCarrito" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
		<!--PROY-24724-IDEA-28174 - INICIO-->
                <input id="hidConcatPrima" type="hidden" name="hidConcatPrima" />
                <input id="hidConcatItemsPopUp" type="hidden" name="hidConcatItemsPopUp" />
		<!--PROY-24724-IDEA-28174 - FIN-->
                <!--PROY-29296 - INICIO-->
                <input id="hidMontosTopeConsumo" type="hidden" name="hidMontosTopeConsumo" />
                <input id="hidFilaSeleccionada" type="hidden" name="hidFilaSeleccionada" />
                <!--PROY-29296 - FIN-->
                <input id="hidnPlanServicioValue" type="hidden" name="hidnPlanServicioValue" />
                <input id="hidPlanServicioNo" type="hidden" name="hidPlanServicioNo" />
                <input id="hidPlanServicioNoGrupo" type="hidden" name="hidPlanServicioNoGrupo" />
                <input id="hidPlanServicioNGTemp" type="hidden" name="hidPlanServicioNGTemp" />
                <input id="hidLineaActual" type="hidden" name="hidLineaActual" />
                <input id="hidTotalLineas" type="hidden" value="0" name="hidTotalLineas" />
                <input id="hidPlazoActual" type="hidden" name="hidPlazoActual" runat="server" />
                <input id="hidPaqueteActual" type="hidden" name="hidPaqueteActual" />
                <input id="hidTienePaquete" type="hidden" value="N" name="hidTienePaquete" />
                <input id="hidPlan" type="hidden" name="hidPlan" />
                <input id="hidTraerPlazo" type="hidden" name="hidTraerPlazo" />
                <input id="hidCuota" type="hidden" name="hidCuota" />
                <input id="hidNroCuotaActual" type="hidden" name="hidNroCuotaActual" />
                <input id="hidCampana" type="hidden" name="hidCampana" />
                <input id="hidNGrupoPaqueteValues" type="hidden" name="hidNGrupoPaqueteValues" />
                <input id="hidTipoClienteActual" type="hidden" name="hidTipoClienteActual" />
                <input id="hidTipoProductoActual" type="hidden" value="Movil" name="hidTipoProductoActual" />
                <input id="hidTieneCuotas" type="hidden" name="hidTieneCuotas" />
                <input id="hidValidarGuardarCuota" type="hidden" name="hidValidarGuardarCuota" runat="server" />
                <input id="hidPlazos" type="hidden" name="hidPlazos" runat="server" />
                <input id="hidnValueAccion" type="hidden" name="hidnValueAccion" runat="server" />
                <input id="hidTipoDocumento" type="hidden" name="hidTipoDocumento" runat="server" />
                <input id="hidEquiposSel" type="hidden" name="hidEquiposSel" />
                <input id="hidPaqActCompleto" type="hidden" name="hidPaqActCompleto" />
                <input id="hidListaTipoProducto" type="hidden" name="hidListaTipoProducto" />
                <input id="hidServicioOriginal" type="hidden" name="hidServicioOriginal" />
                <input id="hidServicioOriginalNo" type="hidden" name="hidServicioOriginalNo" />
                <input id="hidPromociones" type="hidden" name="hidPromociones" />
                <input id="hidNPromocionValues" type="hidden" name="hidNPromocionValues" />
                <input id="hidPromocionTemp" type="hidden" name="hidPromocionTemp" />
                <input id="hidSolucion3Play" type="hidden" name="hidSolucion3Play" />
                <input id="hidPaquete3Play" type="hidden" name="hidPaquete3Play" />
                <input id="hidCodigoTipoProductoActual" type="hidden" value="01" name="hidCodigoTipoProductoActual" />
                <input id="hidCampanasDTH" type="hidden" name="hidCampanasDTH" />
                <input id="hidCadenaDetalle" type="hidden" name="hidCadenaDetalle" />
                <input id="hidFlgOrigen" type="hidden" name="hidFlgOrigen" />
                <input id="hidFlagVOD" type="hidden" name="hidFlagVOD" />
                <input id="hidCampanasHFC" type="hidden" name="hidCampanasHFC" />
                <input id="hidCampanasFTTH" type="hidden" name="hidCampanasFTTH" /> <%--FTTH  hidCampanasFTTH--%>
                <input id="hidEquiposXplan" type="hidden" name="hidEquiposXplan" />
                <input id="hidEquiposXfila3Play" type="hidden" name="hidEquiposXfila3Play" />
                <input id="hidTopesConsumo" type="hidden" name="hidTopesConsumo" />
                <input id="hidCampanasMovil" type="hidden" name="hidCampanasMovil" />
                <input id="hidCampanasFijo" type="hidden" name="hidCampanasFijo" />
                <input id="hidCampanasBAM" type="hidden" name="hidCampanasBAM" />
                <input id="hidCampanasInterInalam" type="hidden" name="hidCampanasInterInalam" /> <%--//PROY-31812 - IDEA-43340 --%>
                <input id="hidCampanasVentaVarios" type="hidden" name="hidCampanasVentaVarios" />
                <input id="hidPlazosMovil" type="hidden" name="hidPlazosMovil" />
                <input id="hidPlazosFijo" type="hidden" name="hidPlazosFijo" />
                <input id="hidPlazosBAM" type="hidden" name="hidPlazosBAM" />
                <input id="hidPlazosInterInalam" type="hidden" name="hidPlazosInterInalam" /> <%--//PROY-31812 - IDEA-43340 --%>
                <input id="hidPlazosHFC" type="hidden" name="hidPlazosHFC" />
                <input id="hidPlazosFTTH" type="hidden" name="hidPlazosHFC" /><%--FTTH  hidPlazosFTTH--%>
                <input id="hidPlazosDTH" type="hidden" name="hidPlazosDTH" />
                <input id="hidPlazosVentaVarios" type="hidden" name="hidPlazosVentaVarios" /> 
                <input id="hidPlanesMovil" type="hidden" name="hidPlanesMovil" />
                <input id="hidPlanesFijo" type="hidden" name="hidPlanesFijo" />
                <input id="hidPlanesBAM" type="hidden" name="hidPlanesBAM" />
                <input id="hidPlanesInterInalam" type="hidden" name="hidPlanesInterInalam" /> <%--//PROY-31812 - IDEA-43340 --%>
                <input id="hidPlanesDTH" type="hidden" name="hidPlanesDTH" />
                <input id="hidCampanasHFCInalamb" type="hidden" name="hidCampanasHFCInalamb" />                                
                <input id="hidPlanesHFC" type="hidden" name="hidPlanesHFC" />
                <input id="hidPlanesFTTH" type="hidden" name="hidPlanesFTTH" /><%--FTTH  hidPlanesFTTH--%>
                <input id="hidPlazosHFCInalamb" type="hidden" name="hidPlazosHFCInalamb" />
                <input id="hidPlanesHFCInalamb" type="hidden" name="hidPlanesHFCInalamb" />
                <!--PROY-29215 INICIO-->
                <input id="hidSelectFP" type="hidden" name="hidSelectFP" />
                <input id="hiSelectNC" type="hidden" name="hiSelectNC" />
                <!--PROY-29215 FIN-->
                <input id="hidPlanesVentaVarios" type="hidden" name="hidPlanesVentaVarios" />
                <input id="hidPlanesCombo" type="hidden" name="hidPlanesCombo" />
                <input id='hidListaDE' type="hidden" name="hidListaDE" />
                <input id='hidEsSecPendiente' type="hidden" name="hidEsSecPendiente" />
                <input id='hidFilaProa' type="hidden" name="hidFilaProa" />  <%--PROY-30748--%>
                <input id='hidFilaRS' type="hidden" name="hidFilaRS" />  <%--PROY-30748--%>
                <input id='hidEquipoN' type="hidden" name="hidEquipoN" />  <%--PROY-30748--%>
                <input id='hidProactiva' type="hidden" name="hidProactiva" />  <%--PROY-30748--%>
                <input id='hidEPSelect' type="hidden" name="hidEPSelect" />  <%--PROY-30748--%>
                <input id='hidContRC' type="hidden" name="hidContRC" />  <%--PROY-30748--%>
                <input id='hidStrCuota' type="hidden" name="hidStrCuota" />  <%--PROY-30748--%>
                <input id='hidPEquipo' type="hidden" name="hidPEquipo" />  <%--PROY-30748--%>
                <input id='hidPEquipoGama' type="hidden" name="hidPEquipoGama" />  <%--PROY-30748--%>
                <!-- PROY-30166-IDEA–38863-INICIO -->
                <input id="hidMontoCuotaMenorA" type="hidden" name="hidMontoCuotaMenorA" runat="server" />
                <input id="hidMontoCuotaMayorA" type="hidden" name="hidMontoCuotaMayorA" runat="server" />
                <input id="hidPorcBRMS" type="hidden" name="hidPorcBRMS" runat="server" />

    
                <input id="hidMsjMontoCuotaValido" type="hidden" name="hidMsjMontoCuotaValido" runat="server" />
          

                <input id="hidCuotaCalculada" type="hidden" name="hidCuotaCalculada" />
                <input id="hidConcatCuotaIni" type="hidden" name="hidConcatCuotaIni" />
                <input id="hidMsjActualizCuotaInicial" type="hidden" name="hidMsjActualizCuotaInicial" runat="server" />
                <input id="hidMaxPorcentajeCuotaInicial" type="hidden" name="hidMaxPorcentajeCuotaInicial" runat="server" />
                <input id="hidMontoCuotaMayorAPorcentaje" type="hidden" name="hidMontoCuotaMayorAPorcentaje" runat="server" />

                <!--INI-INC000002510501  Campañas-->
                <input id="hidModalidadPorta" type="hidden" name="hidModalidadPorta" runat="server" />
                <input id="hidOperadorCedente" type="hidden" name="hidOperadorCedente" runat="server" />
                <input id="hidFlagCampana" type="hidden" name="hidFlagCampana" runat="server" />
                <!--FIN-INC000002510501  Campañas-->

                <!-- PROY-30166-IDEA–38863-FIN -->
                <input id="hidCuotaProa" type="hidden" name="hidCuotaProa" /><!--PROY 30748 F2 MDE-->
                <!--PROY-140383-INI-->
                <input id="hidServExcluyentes" type ="hidden" name ="hidServExcluyentes" /> 
                <input id="hidServExCont" type="hidden" name ="hidServExCont" />
                <input id="hidFlagServvExcluyentes" type= "hidden" name ="hidFlagServvExcluyentes" runat="server" />
                <input id="hidMensValidacion" type="hidden" name="hidMensValidacion" runat="server" />
                <input id="hidServicioExcCaidoifr" type="hidden" name="hidServicioExcCaidoifr" runat ="server" />
                <input id="hidMensServCaido" type="hidden" name="hidMensServCaido" runat ="server" />
                <input id="hidProductosExc" type="hidden" name="hidProductosExc" runat = "server" />
                <!--PROY-140383-FIN-->
                <!--PROY-140618-INI-->
                <input id="hidFamiliaPlan" type="hidden" name ="hidFamiliaPlan" runat="server" />
                <!--PROY-140618-FIN-->
				<!--PROY-140546-INICIO-->
                <input id="hidFlagAplicaCAI" type="hidden" name="hidFlagAplicaCAI" runat = "server" />
                <input id="hidTiempoSecPendientePagoLink" type="hidden" name="hidTiempoSecPendientePagoLink" runat = "server" />
                <input id="hidCanalesPermitidosCAI" type="hidden" name="hidCanalesPermitidosCAI" runat = "server" />
				<!--PROY-140546-FIN-->

                <input id="hdiValoresRestringirCampanaFullClaro" type="hidden" name ="hdiValoresRestringirCampanaFullClaro" runat="server" />   <!--INCICIATIVA-1012-->
                <iframe id="iframeAuxiliar" name="iframeAuxiliar" frameborder="0" width="0" height="0" />
                <input id="hidAccPermiteCuotas" type ="hidden" name ="hidAccPermiteCuotas" /> <%--PROY-140743 | Venta en cuotas --%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
