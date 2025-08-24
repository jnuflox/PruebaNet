<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="sisact_evaluacion_unificada.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_evaluacion_unificada" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %><%--PROY-FULLCLARO--%>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<!--INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <meta http-equiv="X-UA-Compatible" content="IE=8" /> 
	<!--FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_evaluacion.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Lib_FuncValidacion.js"></script>
	<!--INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <script type="text/javascript" language="JavaScript" src="../../Scripts/JSUtil.js"></script>
	<!--FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/security.js"></script><!-- PROY - F12 -->
    <script src="../../Scripts/json2.js" type="text/javascript"></script><!-- PROY-29123 -->
    <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script><!-- PROY-31636 -->
	<!--INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <!--PROY-140546 Cobro Anticipado de Instalacion Inicio-->
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link href="../../Estilos/calendar-blue.css" type="text/css" rel="stylesheet" />
    <script src="../../Scripts/calendar/calendar.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_es.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_setup.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendario_call.js" type="text/javascript"></script>
    <!--PROY-140546 Cobro Anticipado de Instalacion Fin -->
    <style type="text/css">
        .selected
        {
            background-color: yellow;
        }
        
        .autocomplete
        {
            white-space: nowrap;
            text-overflow: ellipsis;
            overflow: hidden;
        }
        
        .autoCompleteParent
        {
            list-style-type: none;
            padding: 1px;
            font-family: Arial;
            font-size: 11px;
            margin: 0px;
            cursor: pointer;
        }
        
         <%-- PROY-FULLCLARO --%>
        
        #PanelCarga table thead th
        {
            background-color: #E9F2FE;
            border-bottom: 2px solid #95B7F3;
            line-height: 1.428571429;
        }
        
       #PanelCarga
        {
            height:auto;
	        position: absolute !important;
	        left: 30%;
	        top: 90%;
	        z-index: 50px;
	        border-bottom: 4px solid #95B7F3;
        }
        
          #tblFormnBuyBackIphone {
          position: relative;
          top: 120px;
          left: 20px;
          background: blue;
      }

       #PanelLineasAdicionales table thead th
        {
            background-color: #E9F2FE;
            border-bottom: 2px solid #95B7F3;
            line-height: 1.428571429;
        }
        
       #PanelLineasAdicionales
        {
            height:auto;
	        position: absolute !important;
	        left: 30%;
	        top: 90%;
	        z-index: 50px;
	        border-bottom: 4px solid #95B7F3;
        }
        
        <%-- PROY-FULLCLARO --%>
        
    </style>
	<!--FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <script type="text/javascript" language="JavaScript"> 

        //Constantes Tipos Productos
        var codTipoProductoMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var codTipoProductoFijo = '<%= ConfigurationManager.AppSettings["constTipoProductoFijo"] %>';
        var codTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        var codTipoProductoBAM = '<%= ConfigurationManager.AppSettings["constTipoProductoBAM"] %>';
        var codTipoProductoHFC = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';
        var codTipoProductoVentaVarios = '<%= ConfigurationManager.AppSettings["constTipoProductoVentaVarios"] %>';
        var codTipoProd3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';
        var codTipoProdInterInalam = '<%= ConfigurationManager.AppSettings["constTipoProductoInterInalam"] %>'; //PROY-31812 - IDEA-43340
        var codTipoProductoFTTH = '<%= ConfigurationManager.AppSettings["constFLAGProductoFTTH"] %>' == '' ?
                                  '<%= ConfigurationManager.AppSettings["constTipoProductoFTTH"] %>' : '<%= ConfigurationManager.AppSettings["constFLAGProductoFTTH"] %>'; //FTTH - FLAG - TAB
        //Constantes Opciones Pagina
        var opcionVerDetalleLinea = '<%= ConfigurationManager.AppSettings["ConstVerDetalleLinea"] %>';
        var opcionVentaPuertaPuerta = '<%= ConfigurationManager.AppSettings["PerfilVentaPuertaPuerta"] %>';
        var opcionConsultaCreditos = '<%= ConfigurationManager.AppSettings["constOpcionConsultaCreditos"] %>';
        var constTipoDocumentoDNI = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>';
        var constTipoDocumentoCE = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"] %>';
        var constTipoDocumentoRUC = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"] %>'; 
	var constCodTipoDocumentoPAS = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoPAS"] %>'; //INC000002748172
        var constPdvTeleventas = '<%= ConfigurationManager.AppSettings["constPdvTeleventas"] %>';
        var constTipoOperAlta = '<%= ConfigurationManager.AppSettings["constTipoOperacionVNA"] %>';
        var constTipoOperMigracion = '<%= ConfigurationManager.AppSettings["constTipoOperacionMIG"] %>';
        var constTipoOfertaBusiness = '<%= ConfigurationManager.AppSettings["constCodTipoProductoBUS"] %>';
        var constTipoOfertaB2E = '<%= ConfigurationManager.AppSettings["constCodTipoProductoB2E"] %>';
        var constKitCambioTitularidad = '<%= ConfigurationManager.AppSettings["CodKITCambioTitularidad"] %>';
        var constCodigoParamOfertaCorp = '<%= ConfigurationManager.AppSettings["constOfertaConsumer"] %>';
        var constTipoBilleteraFijo = '<%= ConfigurationManager.AppSettings["constClaseProductoFijo"] %>';
		//Constantes tipo oficina
        var constRoamingCorner = '<%= ConfigurationManager.AppSettings["constRoamingCorner"] %>';
        var constCodModalidadChipSuelto = '<%= ConfigurationManager.AppSettings["constCodModalidadChipSuelto"] %>';
        var constCodTipoProductoCON = '<%= ConfigurationManager.AppSettings["constCodTipoProductoCON"] %>';
        	var constCaracteresValidosRuc = '<%= ConfigurationManager.AppSettings["ValidaCaracteresEspecialesRUC"] %>'; 
        var constCaracteresValidos = '<%= ConfigurationManager.AppSettings["ValidaCaracteresEspeciales"] %>'; 
        var strHabilitarProactiva = '<%= ConfigurationManager.AppSettings["strHabilitarProactiva"] %>'; 
        var codServProteccionMovil = ""; //PROY-24724-IDEA-28174 - INICIO
        var msgEquipoSinCobertura = "";
        var msgEquipoPrecioPrepagoMenor = "";
        var montoPrecioPrepago = "";
        var msgErrorProcedurePrecioPrepago = "";
        var descServProteccionMovil = "";
        var msgErrorServicioClientePM = '<%= ConfigurationManager.AppSettings["consClienteProteccionMovilWS_Error"] %>';
        var strTieneProteccionMovil = ""; //PROY-24724-IDEA-28174 - FIN
        //EMMH I
        var ConstflagPlanesProactivos = getValue('hidFlagPlanesProactivos'); //PROY_30748 
        //EMMH F
        var mydate = new Date();
        var fechaActual = mydate.getDate() + "/" + parseInt(mydate.getMonth()) + 1 + "/" + mydate.getFullYear();
        var vienedeSecReno = false;

     //PROY - 140245 VARIABLES GLOBALES
        var cantMovilAct = 0; // almacena la cantidad actual de productos por cliente con caso especial colaborador
        var cantMovilMax = 0; //almacena la cantidad maxima de productos por cliente con caso especial colaborador
        var cantFijoInalAct = 0;
        var cantFijoInalMax = 0;
        var cantClaroTvAct = 0;
        var cantClaroTvMax = 0;
        var cantBamAct = 0;
        var cantBamMax = 0;
        var cant3PlayAct = 0;
        var cant3PlayMax = 0;
        var cantInterInalAct = 0;
        var cantInterInalMax = 0;
        var cantPlayInalAct = 0;
        var cantPlayInalMax = 0;
        var flagInicCantProd = false;// flag que controla la inicializacion de las variables globales de cantidad de productos actuales y maximos por clientes.
        var strCasoEspecialColab="";// almacena todos los codigos de casos especiales pertenecientes a Colaborador Claro
        var strMsgAutogestion = "";// almacena el mensaje de autogestion
        var strMsgValidaCantidadLineas = "";// almacena el mensaje cuando el cliente sobrepasa la cantidad de productos maximos permitidos
        var flagContinuarEvaluacion = "0"; //flag que controla la insercion de un nuevo item.
        var strMsgServicioValidConteoLineas = "";
        //NUEVO
        var strCantMaxPorProducto = ""; //almacena los codigos de productos y sus cantidades maximas para el caso colaborador claro
        var strFlagCarrito = 'N';
        //FIN PROY - 140245

        // FULLCLARO.V2-INI
        var cantCheckGeneradosMovil = 0;
        var cantCheckGeneradosFija = 0;
            var urlVentStock = '<%=ConfigurationManager.AppSettings["urlConsultaStock"] %>';
        var UrlAdjuntarSustentos = '<%= ConfigurationManager.AppSettings["UrlAdjuntarSustentos"] %>';
        var CurrentUser = '<%= CurrentUserSession.idUsuario.ToString()??"" %>';
        var constEstadoAPR = '<%= ConfigurationManager.AppSettings["constEstadoAPR"] %>';
        var constCodTipoOficinaCAC = '<%= ConfigurationManager.AppSettings["constCodTipoOficinaCAC"] %>';
        var constCodTipoOficinaDAC = '<%= ConfigurationManager.AppSettings["constCodTipoOficinaDAC"] %>';
        var constCodTipoOficinaCorner = '<%= ConfigurationManager.AppSettings["constCodTipoOficinaCorner"] %>';
        var constCodTipoDocumentoDNI = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>';
        var keyValidarLineas_CantidadPermitida = '<%= ConfigurationManager.AppSettings["keyValidarLineas_CantidadPermitida"] %>';
        var keyValidarLineas_MensajeValidacionAdvertencia = '<%= ConfigurationManager.AppSettings["keyValidarLineas_MensajeValidacionAdvertencia"] %>';
        var keyValidarLineas_MensajeValidacionRestrictiva = '<%= ConfigurationManager.AppSettings["keyValidarLineas_MensajeValidacionRestrictiva"] %>';
        var GV_Swicht_3G = '<%= ConfigurationManager.AppSettings["Swicht_3G"] %>';
        var constRUCInicio = '<%= ConfigurationManager.AppSettings["constRUCInicio"] %>';
        var ConstAnioMayorEdad = '<%= ConfigurationManager.AppSettings["ConstAnioMayorEdad"] %>';
        var constRespDataCredTipo7 = '<%= ConfigurationManager.AppSettings["constRespDataCredTipo7"] %>';
        var constCodTipoDocumentoCEX = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"] %>';
        var constRespDataCredTipo6 = '<%= ConfigurationManager.AppSettings["constRespDataCredTipo6"] %>';
        var constRespDataCredTipo1 = '<%= ConfigurationManager.AppSettings["constRespDataCredTipo1"] %>';
        var flujoAlta = '<%= ConfigurationManager.AppSettings["flujoAlta"] %>';
        var flujoPortabilidad = '<%= ConfigurationManager.AppSettings["flujoPortabilidad"] %>';
        var codservroamingi = '<%= ConfigurationManager.AppSettings["codservroamingi"] %>';
        var constMsjBolsaCompartidaII = '<%= ConfigurationManager.AppSettings["constMsjBolsaCompartidaII"] %>';
        var CM_consMsjNoConfiguracionCuotas = '<%= ConfigurationManager.AppSettings["consMsjNoConfiguracionCuotas"] %>';
        var constTextoNoAplicaCondiciones = '<%= ConfigurationManager.AppSettings["constTextoNoAplicaCondiciones"] %>';
        var constTextoAprobadoAutonomia = '<%= ConfigurationManager.AppSettings["constTextoAprobadoAutonomia"] %>';
        var constTextoNoAprobadoAutonomia = '<%= ConfigurationManager.AppSettings["constTextoNoAprobadoAutonomia"] %>';
        var constTipoProductoMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var constTipoProductoHFC = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';
        var constTipoProductoFTTH = '<%= ConfigurationManager.AppSettings["constTipoProductoFTTH"] %>';
        var constTipoProducto3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';
        var consMsjDupLineasCP = '<%= ConfigurationSettings.AppSettings["consMsjDupLineasCP"] %>';
        var CS_consMsjNoConfiguracionCuotas = '<%= ConfigurationSettings.AppSettings["consMsjNoConfiguracionCuotas"] %>';
        var constMsjEquipoSinStock = '<%= ConfigurationManager.AppSettings["constMsjEquipoSinStock"] %>';
        var MensajeAutonomiaAdjuntarSustentos = '<%= ConfigurationManager.AppSettings["MensajeAutonomiaAdjuntarSustentos"] %>';
        var constPaginaEnviarCreditos = '<%= ConfigurationManager.AppSettings["constPaginaEnviarCreditos"] %>';
        var consMsjErrorSolMigra = '<%= ConfigurationManager.AppSettings["consMsjErrorSolMigra"] %>';
        var constPaginaAdjDocumento = '<%= ConfigurationManager.AppSettings["constPaginaAdjDocumento"] %>';
        var constPaginaAdjDocumentoRuc = '<%= ConfigurationManager.AppSettings["constPaginaAdjDocumentoRuc"] %>';
        var constVentaCE4Play = '<%= ConfigurationManager.AppSettings["constVentaCE4Play"] %>';
        var constPostVentaCE4Play = '<%= ConfigurationManager.AppSettings["constPostVentaCE4Play"] %>';
        var constNroMaximoPlanesEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaximoPlanesEmpresa"] %>';
        var constNroMaxPlanesMovilEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesMovilEmpresa"] %>';
        var constNroMaxPlanesFijoEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesFijoEmpresa"] %>';
        var constNroMaxPlanesBAMEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesBAMEmpresa"] %>';
        var constNroMaxPlanesDTHEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesDTHEmpresa"] %>';
        var constNroMaxPlanesHFCEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCEmpresa"] %>';
        var constNroMaxPlanesHFCIEmpresa = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCIEmpresa"] %>';
        var constNroMaximoPlanesPersona = '<%= ConfigurationManager.AppSettings["constNroMaximoPlanesPersona"] %>';
        var constNroMaxPlanesMovilPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesMovilPersona"] %>';
        var constNroMaxPlanesFijoPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesFijoPersona"] %>';
        var constNroMaxPlanesBAMPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesBAMPersona"] %>';
        var constNroMaxPlanesDTHPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesDTHPersona"] %>';
        var constNroMaxPlanesHFCPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCPersona"] %>';
        var constNroMaxPlanesHFCIPersona = '<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCIPersona"] %>';
        var constPlanMovil4Play = '<%= ConfigurationManager.AppSettings["constPlanMovil4Play"] %>';
        var constExclusionPlanes4Play = '<%= ConfigurationManager.AppSettings["constExclusionPlanes4Play"] %>';
        var consTopeBloqueoRobo = '<%= ConfigurationManager.AppSettings["consTopeBloqueoRobo"] %>';
        var consTopeBloqueoASolicitud = '<%= ConfigurationManager.AppSettings["consTopeBloqueoASolicitud"] %>';
        var consTopeLineaDesactivaMorosidad = '<%= ConfigurationManager.AppSettings["consTopeLineaDesactivaMorosidad"] %>';
        var consTopeLineaDesactivaMigracion = '<%= ConfigurationManager.AppSettings["consTopeLineaDesactivaMigracion"] %>';
        var consMotivoBloqueoRobo = '<%= ConfigurationManager.AppSettings["consMotivoBloqueoRobo"] %>';
        var consMotivoBloqueoASolicitud = '<%= ConfigurationManager.AppSettings["consMotivoBloqueoASolicitud"] %>';
        var consMotivoLineaDesactivaMorosidad = '<%= ConfigurationManager.AppSettings["consMotivoLineaDesactivaMorosidad"] %>';
        var consMotivoLineaDesactivaMigracion = '<%= ConfigurationManager.AppSettings["consMotivoLineaDesactivaMigracion"] %>';
        var consCodGrupoCanalNoErrorTipo7 = '<%= ConfigurationManager.AppSettings["COD_GRUPO_CANAL_NO_ERROR_TIPO_7"] %>';
        var consCodGrupoDocNoErrorTipo7 = '<%= ConfigurationManager.AppSettings["COD_GRUPO_DOC_NO_ERROR_TIPO_7"] %>';
        var nroRegistroEvaMejPorta = "0"; //PROY-140618
		var consTramaDatosFranjaHoraria = '<%= ConfigurationManager.AppSettings["ConstFranjaHorariaLabel"] %>'; //PROY-140546 Cobro Anticipado de Instalacion
    </script>
    <script type="text/javascript" language="javascript" src="../../Scripts/Paginas/evaluacion_cons/sisact_evaluacion_unificada.js"></script>
    <script language="javascript" type="text/javascript">
        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }
    </script>
</head>
<body onkeydown="cancelarBackSpace();" onload="inicio()">
    <form id="frmPrincipal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td class="Header" align="center" height="19">Registro y Evaluación SEC</td>
			</tr>
			<tr>
				<td><img height="4" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
			<tr>
				<td><input class="Boton" id="btnNuevaEvaluacion" style="WIDTH: 125px; CURSOR: hand" onclick="nuevaEvaluacion();"
						type="button" value="Nueva Evaluación" /></td>
			</tr>
			<tr>
				<td><img height="3" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
			<tr>
				<td>
					<table class="Contenido" cellspacing="1" cellpadding="1" width="100%" border="0">
						<tr id="trPuntoVenta" style="DISPLAY: none">
							<td class="Arial10B" width="125">&nbsp;Canal:</td>
							<td width="170"><asp:dropdownlist id="ddlCanal" runat="server" onChange="cambiarCanal();" Width="130px" CssClass="clsSelectEnableC"></asp:dropdownlist></td>
							<td class="Arial10B" width="100">Punto de Venta:</td>
							<td colspan="3"><asp:dropdownlist id="ddlPuntoVenta" runat="server" onChange="cambiarPuntoVenta();" Width="200px"
									CssClass="clsSelectEnableC"></asp:dropdownlist></td>
						</tr>
						<tr id="trDatosCliente">
                            <!--INI PROY-31636-->
							<td class="Arial10B" width="125">&nbsp;Tipo Documento:</td>
							<td width="170"><asp:dropdownlist id="ddlTipoDocumento" runat="server" onChange="cambiarTipoDocumento();mostrarVendedor()"
									Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist></td>
							<td class="Arial10B" width="100">Nro. Documento:</td>
							<td width="160"><input class="clsInputEnabled" id="txtNroDoc" onkeyup="this.value = this.value.toUpperCase()" onkeypress="validaTxtDoc()" size="22"/>
							</td>
                            <td class="Arial10B" id="lblNacionalidad" width="100">&nbsp;Nacionalidad:</td>
                            <td width="170">
                                <asp:dropdownlist id="ddlNacionalidad" runat="server" Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist>
							</td>
                            <!--FIN PROY-31636-->
							<td class="Arial10B" id="tdLblFechaNac" width="110">Fecha Nacimiento:</td>
							<td id="tdTxtFechaNac"><input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789/')" onpaste="return false;"
									id="txtFechaNac" maxLength="10" size="18" />&nbsp;&nbsp;(dd/mm/yyyy)
							</td>
						</tr>
						<tr id="trVendedor" style="DISPLAY: none">
							<td class="Arial10B" width="125">&nbsp;DNI Vendedor:</td>
							<td width="170"><input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789')" id="txtDNIVendedor"
									onblur="cambiarVendedor();" maxLength="8" size="26" /></td>
							<td colspan="4"><input class="Boton" id="btnvalidarVendedor" style="WIDTH: 105px; CURSOR: hand" onclick="validarVendedor();"
									type="button" value="Validar" />
							</td>
						</tr>
						<tr>
							<td class="Arial10B">&nbsp;Portabilidad:</td>
							<td colspan="1"><asp:checkbox id="chkPortabilidad" onclick="cambiarPortabilidad(this);" runat="server"></asp:checkbox></td>
                                                        <td colspan="2">
                                                             <table id="tblCartaPoder">
                                                                <tr>
							             <td class="Arial10B"> &nbsp;Carta Poder:
                                                                     </td>
                                                                     <td colspan="1">
                                                                          <asp:CheckBox ID="chkCartaPoder" onclick="cambiarCartaPoder(this);" runat="server"></asp:CheckBox>
                                                                     </td>
                                                                </tr>
                                                             </table>
                                                        </td>
							<!--PROY-32439::INI-->

                           <!-- INICIATIVA-803  -->
                               <td colspan="3">
							<table id="tblTiendaVirtual" style="display:none;">
									<tr>
										<td class="Arial10B">Tienda Virtual:</td>
										 <td colspan="2">
											  <asp:CheckBox ID="chkFlagTienda" onclick="MostrarTiendaDatosTV(this);" runat="server"></asp:CheckBox>
											  <input id="txtNroPedidoWeb" runat="server" class="clsInputEnabled" onkeyup="this.value = this.value.toUpperCase()" style="display:none;" onkeypress="validaCaracteresTV()" onfocus="BorrarDatos()"  maxlength="15" size="30"/>
										 </td>
									</tr>
								</table>
							</td>
							<!-- INICIATIVA-803  -->

                            <td colspan="2">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:60%; text-align:right"">
                                            <label class="Arial10BRed" id="lblMensajeDeudaBloqueo"></label>
                                        </td>
                                        <td style="width:40%; text-align:left">
                                            <input class="Boton" id="btnDetalleLinea" style="DISPLAY: none; WIDTH: 120px; CURSOR: hand"
									        onclick="verDetalleLinea();" type="button" value="Ver Detalle Líneas" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--PROY-32439::FIN-->
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center" height="23"><input class="BotonOptm" id="btnvalidarClaro" style="WIDTH: 150px; CURSOR: hand" onclick="validarClaro();"
						type="button" value="Validación Claro" /></td>
			</tr>
			<tr id="trLineasDesactivas" style="DISPLAY:none">
				<td width="100%">
					<iframe id="ifrLineasDesactivas" style="WIDTH: 100%; HEIGHT: 310px" 
						src="" frameBorder="no" scrolling="auto" marginwidth="0" marginheight="0" vspace="0"
						class="clsGridRow" onload="autoSizeIframeLineas();"></iframe>
				</td>
			</tr>
			<tr id="trDetalleCliente" style="DISPLAY: none">
				<td>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="19">&nbsp;Datos del Cliente</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="1" width="100%" border="0">
						<tr>
							<td class="Arial10B" width="125">&nbsp;Tipo Cliente:</td>
							<td class="Arial10B" colspan="5"><label class="Arial10B" id="lblCategoriaCliente"></label></td>
						</tr>
						<tr id="trDetalleRUC" style="DISPLAY: none">
							<td class="Arial10B" width="125">&nbsp;Razón Social:</td>
							<td width="275"><input id="txtRazonSocial" class="clsInputEnabled" onkeypress="return validaNombresCaracteresEspRUC(event,constCaracteresValidosRuc);" onpaste="return false;"
									        maxLength="40" size="80" /></td>
                                            <!-- INI 32438-->
                                            <td class="Arial10B" width="135">&nbsp;Tipo de Contribuyente:</td>
                                            <td width="250"><input id="txtTipoContribuyente" class="clsInputDisabled"  size="50" /></td>
						 <!-- FIN 32438-->
							<td colspan="4"></td>
						</tr>
						<tr id="trDetalleDNI">
							<td class="Arial10B" width="125">&nbsp;Nombres:</td>
							<td width="250"><input id="txtNombre" class="clsInputEnabled" onkeypress="return validaNombresCaracteresEsp(event,constCaracteresValidos);" onpaste="return false;"
									        maxLength="40" size="35" /></td>
							<td class="Arial10B" width="125">&nbsp;Apellido Paterno:</td>
							<td width="250"><input id="txtApePat" class="clsInputEnabled" onkeypress="return validaNombresCaracteresEsp(event,constCaracteresValidos);" onpaste="return false;"
									        maxLength="40" size="45" /></td>
							<td class="Arial10B" width="125">&nbsp;Apellido Materno:</td>
							<td><input id="txtApeMat" class="clsInputEnabled" onkeypress="return validaNombresCaracteresEsp(event,constCaracteresValidos);" onpaste="return false;"
								maxLength="40" size="45" /></td>
						</tr>
                    <%--INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18--%>
                    <tr id="trCorreoElectronico">
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Correo Electrónico:
                        </td>
                        <td class="Arial10B" style="width: 150">
                            <input type="text" id="txtCorreoElectronico" name="txtCorreoElectronico" runat="server"
                                class="clsInputEnable" style="text-transform: uppercase; width: 200px" onpaste="return false;"
                                onkeypress="return CaracterPermitidoEmail(event);" />
                        </td>
                        <%--PROY-140579 RU10 NN INICIO--%>
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Flag Whitelist:
                        </td>
                        <td class="Arial10B" style="width: 250">
                            <input type="text" id="txtWhitelist" name="txtWhitelist" runat="server"
                                class="clsInputEnable" style="text-transform: uppercase; width: 200px" onpaste="return false;"/>
                        </td>
                        <%--PROY-140579 RU10 NN INICIO--%>
                    </tr>
                    <tr id="trLista">
                        <td>
                        </td>
                        <td>
                            <ul id="ul__txtCorreoElectronico" tabindex="-1" class="autoCompleteParent" style="display: none">
                            </ul>
                        </td>
                    </tr>
                    <tr id="trConfCorreoElectronico">
                        <td class="Arial10B" style="width: 150px">
                            &nbsp;Confirm. Correo &nbsp;Electrónico:
                        </td>
                        <td class="Arial10B" style="width: 150">
                            <input type="text" id="txtConfCorreoElectronico" runat="server" name="txtConfCorreoElectronico"
                                class="clsInputEnable" style="text-transform: uppercase; width: 200px" onpaste="return false;"
                                onkeypress="return CaracterPermitidoEmail(event);" />
                        </td>
                    </tr>
                    <%--FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18--%>
					</table>
				</td>
			</tr>
			<tr id="trConsultarDC" style="DISPLAY: none">
				<td align="center" height="23"><input class="BotonOptm" id="btnConsultaDC" style="WIDTH: 185px; CURSOR: hand" onclick="consultarDC();"
						type="button" value="Ingresar Condiciones de Venta" /></td>
			</tr>
			<tr id="trRepresentante" style="DISPLAY: none">
				<td>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Representante Legal</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr id="tblDatosRepresentante">
							<td height="70"><iframe class="clsGridRow" id="ifraRepresentante" style="WIDTH: 100%; HEIGHT: 100%"
									src="" frameborder="no" scrolling="yes"></iframe>
							</td>
						</tr>
                       
                        <tr>
                            <td valign="bottom" colspan="6" align="right">
                                <input class="BotonOptm" id="btnValidaRPLL" style="WIDTH: 135px; CURSOR: hand" onclick="btnValidaRPLL_Click(this);"
						        type="button" value="Siguiente Paso" />
							</td>
						</tr>
                        <!-- FIN PROY-29121 -->
					</table>
				</td>
			</tr>
			<tr id="trCondicionVenta" style="DISPLAY: none">
				<td>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Condiciones de Venta</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
							<td class="Arial10B" width="125">&nbsp;Tipo Operación:</td>
							<td><asp:dropdownlist id="ddlTipoOperacion" runat="server" Width="150px" CssClass="clsSelectEnableC" onchange="cambiarTipoOperacion(this);"></asp:dropdownlist></td>
                            <!-- Proy 29123 Venta en cuotas-->
                            <td colspan="2"><label id="lblCuotas" style="font-weight: bold;color:black"></label>
                            <!-- IDEA-142010 BENEFICIOS-->
                            <label id="lblBeneficio" style="font-weight: bold;color:red"></label>
                            <!-- IDEA-142010 BENEFICIOS-->
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>

						</tr>
						<tr>
							<td class="Arial10B" width="125">&nbsp;Oferta:</td>
							<td width="170"><asp:dropdownlist id="ddlOferta" runat="server" Width="150px" CssClass="clsSelectEnableC" onchange="cambiarTipoOferta(this);"></asp:dropdownlist></td>
							<td class="Arial10B" width="100">&nbsp;Caso Especial:</td>
							<td class="Arial10B" width="280"><select class="clsSelectEnableC" id="ddlCasoEspecial" style="WIDTH: 275px" onchange="cambiarCasoEspecial(this);">
                                    <option value="" selected>SELECCIONE...</option>
								</select>
							</td>
							<td colspan="2">&nbsp;</td>
                            <td class="Arial10B" valign="middle" align="right">
                                Color Equipos sin stock: &nbsp;
                                <span id="spaColor" 
                                
                                style="border-color:DodgerBlue;border=3px solid DodgerBlue;color:<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>">
                                Equipo
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</span>
                            </td>   
                        </tr>
						<tr>
							<td class="Arial10B" width="125">&nbsp;Modalidad de Venta:</td>
							<td width="170"><asp:dropdownlist id="ddlModalidadVenta" runat="server" Width="150px" CssClass="clsSelectEnableC" onchange="cambiarModalidadVenta(this);">
                            </asp:dropdownlist></td>
                            <td class="Arial10B" width="125">&nbsp;Grupo Producto:</td>
							<td class="Arial10B"><asp:dropdownlist id="ddlCombo" runat="server" Width="275px" CssClass="clsSelectEnableC" onchange="cambiarCombo(this);">
                                <asp:ListItem Value="" Selected>SELECCIONE...</asp:ListItem></asp:dropdownlist></td>
							<td colspan="2">&nbsp;</td>
                            <td class="Arial10B" valign="middle" align="right">
                                Consultar Puntos ClaroClub&nbsp;
                                <img style="width: 25px; cursor: hand; height: 25px" alt="Agregar Carrito" onclick="abrirModalCC();"
                                    src="../../Imagenes/ico_lupa.gif" />
                            </td>   
						</tr>
						<tr id="trConsultaStock" style="height:40px;">
							<td colspan="6"><img height="5" alt="" src="../../Imagenes/spacer.gif" width="5" border="0" /></td>
						<td rowspan="2" class="Arial10B" valign="middle" align="right" style="vertical-align:middle;">
                        Consultar Stock&nbsp;
                                <img style="width: 25px; cursor: hand; height: 25px" alt="Consultar Stock" onclick="ConsultarStock();"
                                    src="../../Imagenes/ico_lupa.gif" />
                        </td>
						</tr>
                        <tr id="trTabProducto">
                            <td colspan="6">
                                <table cellspacing="0" cellpadding="1" border="0">
                                    <tr>
                                        <td class="tab_inactivo" id="tdMovil" bordercolor="#000099" align="center" width="105">
                                            <a href="javascript:mostrarTab('Movil');">Móvil</a>
                                        </td>
                                        <td class="tab_inactivo" id="tdBAM" bordercolor="#000099" align="center" width="105">
                                            <a href="javascript:mostrarTab('BAM');">BAM</a>
                                        </td>
                                        <td class="tab_inactivo" id="tdDTH" bordercolor="#000099" align="center" width="105">
                                            <a href="javascript:mostrarTab('DTH');">Claro TV SAT</a>
                                        </td>
                                        <td class="tab_inactivo" id="tdHFC" bordercolor="#000099" align="center" width="105">
                                            <a href="javascript:mostrarTab('HFC');">3Play</a>
                                        </td>
                                        <td class="tab_inactivo" id="tdFijo" bordercolor="#000099" align="center" width="105">
                                            <a href="javascript:mostrarTab('Fijo');">Fijo Inalámbrico</a>
                                        </td>
                                        <td class="tab_inactivo" id="tdVentaVarios" bordercolor="#000099" align="center"
                                            width="105">
                                            <a href="javascript:mostrarTab('VentaVarios');">Venta Varios</a>
                                        </td>
                                        <td class="tab_inactivo" id="td3PlayInalam" bordercolor="#000099" align="center"
                                            width="105">
                                            <a href="javascript:mostrarTab('3PlayInalam');">3Play Inalambrico</a>
                                        </td>
                                        <%-- PROY-31812 - IDEA-43340 - INICIO --%>
                                        <td class="tab_inactivo" id="tdInterInalam" bordercolor="#000099" align="center"
                                            width="125">
                                            <a href="javascript:mostrarTab('InterInalam');">Internet Inalambrico</a>
                                        </td>
                                        <%-- PROY-31812 - IDEA-43340 - FIN --%>
                                        <%-- FTTH - Inicio- TAB --%>
                                        <td class="tab_inactivo" id="tdFTTH" bordercolor="#000099" align="center"
                                            width="125">
                                            <a href="javascript:mostrarTab('FTTH');">FIJA</a>
                                        </td>
                                        <%-- FTTH - Fin- TAB--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
						<tr id="trLCDisponible" style="display:none"><%-- PROY-140434 --%>
							<td class="Arial10B" colspan="6">&nbsp;LC Disponible&nbsp;<label id="lblLCDisponiblexProd">Móvil:&nbsp;&nbsp;&nbsp;&nbsp;</label>
								<input class="clsInputDisabled" id="txtLCDisponiblexProd" style="TEXT-ALIGN: right" readonly
									size="12" />
							</td>
						</tr>
						<tr id="trCarrito">
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td>
                                            <input class="Boton" id="btnAgregarPlan" style="width: 125px; cursor: hand"
                                                onclick="AgregarItem_ValidarDireccionIFI();" type="button" value="Agregar Item" /> <%--PROY-140439 BRMS CAMPAÑA NAVIDEÑA--%>
                                        </td>
                                        <td class="Arial10B" style="display: none" id="tdModalidad">
                                            &nbsp;&nbsp;&nbsp;Modalidad: &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlModalidadPorta" onclick="cambiarModalidad('M');" runat="server" Width="150px" CssClass="clsSelectEnable0">
                                                <asp:ListItem Value="">SELECCIONE...</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Arial10B" style="display: none" id="tdOperadorCedente">
                                            &nbsp;&nbsp;&nbsp;Operador cedente: &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlOperadorCedente" onclick="cambiarModalidad('O');" runat="server" Width="200px" CssClass="clsSelectEnable0">
                                                <asp:ListItem Value="">SELECCIONE...</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="Arial10B" valign="bottom" align="right" id="tdConsultaPrevia">
                            <input class="Boton" id="btnConsultaPrevia" style="width: 115px; cursor: hand" type="button"
                                onclick="realizarConsultaPrevia();" value="Consulta Previa" />
                        </td>
                        <td class="Arial10B" valign="middle" align="right" id="tdCarrito">
                                Agregar al Carrito&nbsp;
                                <img style="width: 35px; cursor: hand; height: 35px" alt="Agregar Carrito" onclick="AgregarCarrito_ValidarCoberturaIFI();"
                                    src="../../Imagenes/carrito.jpg" />
                            </td>
						</tr>
					</table>
				</td>
			</tr>
			<tr id="trCondicionVentaDetalle" style="DISPLAY: none">
				<td><iframe id="ifraCondicionesVenta" style="WIDTH: 100%" src="" frameborder="no" scrolling="no"></iframe>
				</td>
			</tr>
            <!-- PROY-32129 :: INI -->
            <tr id="trDatosAlumnoInstitucion" style="DISPLAY: none">
                <td>
                <table>
                    <tr>
                        <td class="Header" align="left" height="18" width="200" colspan="2">&nbsp;Ingresar datos de institución</td>                
                    </tr>
                    <tr>
                        <td class="Arial10B" width="180">&nbsp;Institucion :</td>
                        <td class="Arial10B" width="280">
                            <asp:DropDownList ID="ddlInstitucion" runat="server" 
                                CssClass="clsSelectEnableC" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" width="180">&nbsp;Cod. Colaborador / Alumno:</td>
                        <td width="250"><input class="clsInputEnabled" id="txtCodAlumno" size="45" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center"><input class="Boton" id="btnGrabarDatosAlumno" style="width: 125px; cursor: hand"
                                                onclick="GrabarDatosAlumno();" type="button" value="Grabar" /> 
                            &nbsp;<input class="Boton" id="btnCancelarGrabarAlu" style="width: 125px; cursor: hand"
                                                type="button" value="Cancelar" onclick="CancelarGrabarAlumno();" />                             
                            </td>
                    </tr>
                </table>
                </td>
             </tr>       
             <!-- PROY-32129 :: FIN -->
			<tr id="trResultado" style="DISPLAY: none">
				<td>
					<table cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Resultado Evaluación</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Arial10B" width="125">&nbsp;Resultado:</td>
							<td width="250"><input class="clsInputDisabled" id="txtResultado" style="TEXT-ALIGN: right" readonly size="45" /></td>
							<td class="Arial10B" style="DISPLAY: none" width="105">&nbsp;Riesgo:</td>
							<td style="DISPLAY: none" width="220"><input class="clsInputDisabled" id="txtRiesgo" style="TEXT-ALIGN: right" readonly size="8" /></td>
							<td class="Arial10B" id="tdLCDisponible" width="110">&nbsp;LC Disponible:</td>
							<td id="tdTxtLCDisponible"><input class="clsInputDisabled" id="txtLCDisponible" style="TEXT-ALIGN: right" readonly
									size="10" /></td>
							<td><input class="Boton" type="button" id="btnDetalleLineasBolsa"
									onclick="mostrarDetalleLineasBolsa();" value="Agregar Líneas"
									style="CURSOR: hand; WIDTH: 120px; HEIGHT: 19px; DISPLAY: none;" /></td>
                                    <td id="tdbtnOtrasOpc"><input 
                                    class="BotonOptm" id="btnOtrasOpc" 
                                    style="WIDTH: 155px; CURSOR: hand; margin-left: 2px;" onclick="verOtrasOpciones();"
									type="button" value="Ver Otras Opciones" />
                                    </td>
						</tr>
						<tr>
							<td id="tdRiesgoClaro" class="Arial10B" width="125">&nbsp;Riesgo Claro:</td>
							<td id="tdTxtRiesgoClaro" width="250"><input class="clsInputDisabled" id="txtRiesgoClaro" style="TEXT-ALIGN: right" readonly
									size="45" /></td>
							<td class="Arial10BRed" width="180">&nbsp;Comportamiento Consolidado &nbsp;Cliente:</td>
							<td width="100"><input class="clsInputDisabled" id="txtComportamiento" style="TEXT-ALIGN: right" readonly
									size="10" /></td>
							<td class="Arial10B" width="150">&nbsp;Rango LC Disponible:</td>
							<td><input class="clsInputDisabled" id="txtRangoLC" style="TEXT-ALIGN: right" readonly size="35" runat="server"/></td>
						</tr>
						<tr id="trGarantia" style="DISPLAY: none">
							<td class="Arial10B" width="125">&nbsp;Tipo Garantía:</td>
							<td width="250"><input class="clsInputDisabled" id="txtTipoGarantia" style="TEXT-ALIGN: right" readonly
									size="45" /></td>
							<td class="Arial10B" width="180">&nbsp;Importe S/.:</td>
							<td colspan="3"><input class="clsInputDisabled" id="txtImporte" style="TEXT-ALIGN: right" readonly size="10" />&nbsp;&nbsp;
								<input class="BotonOptm" id="btnDetalleGarantia" style="WIDTH: 150px; CURSOR: hand" onclick="verDetalleGarantia();"
									type="button" value="Detalle Garantías" /></td>
						</tr>
                        <!-- PROY-140579 RU01 INI RMR -->
                        <tr id="trMotivo" style="DISPLAY: none">
							<td class="Arial10B" width="125">&nbsp;Motivo:</td>
							<td width="250"><!-- <input class="clsInputDisabled" id="txtMotivo" 
                                    style="TEXT-ALIGN: right; height: 51px;" readonly
									size="45" />-->
                                    <asp:textbox id="txtMotivo" 
                                    onblur="return validaTextAreaLongitud(this, 200, true);" style="TEXT-TRANSFORM: uppercase"
									runat="server" Width="86%" CssClass="inputTextArea" TextMode="MultiLine" Rows="5"></asp:textbox>
                            </td>							
						</tr>
                        <!-- PROY-140579 RU01 FIN RMR -->

						<tr id="trPresentaPoderes" style="DISPLAY: none">
							<td class="Arial10B" colspan="6"><input id="chkPresentaPoderes" disabled type="checkbox" value="" />
								No Requiere Presentar Poderes
							</td>
						</tr>
                        <!--PROY-FULLCLARO.v2-INI-->
                        <tr>
                        <td> 
                           <input id="btnFullClaro2" type="button" class="BotonOptm" value="Full Claro" style="WIDTH: 155px; display:none; CURSOR: hand; margin-left: 2px;" onclick="ModalFullClaro()"/>
                           </td>
                        </tr>
                         <!--PROY-FULLCLARO.v2-FIN-->
					</table>
				</td>
			</tr>
			<tr>
				<td><img height="2" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
            <!-- INI PROY-140739 Formulario Leads  -->
            <tr id="tdFormulariosLeads"  style="DISPLAY: none">
                  <td>
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="height: 24px">
						<tr>
							<td class="Header" colspan="4" align="left" height="18">&nbsp;Codigo de Formulario Leads</td>
						</tr>
					    <tr>
							<td class="Arial10B" valign="top" width="200">&nbsp;Codigo Formulario<br />
                            &nbsp;(Leads):</td>
							<td class="Arial10B" style="width: 410px">
                                <input class="clsInputEnable" name="txtFormLead" runat="server" type="text" id="txtFormLead" style="width: 200px;" onkeypress="validaCaracteres('ABCDEFGHIJKLMNÃ‘OPQRSTUVWXYZabcdefghijklmnÃ±opqrstuvwxyz0123456789')"/>
                            </td>
						</tr>    
                    </table>
                </td>
            </tr>
            <!-- FIN PROY-140739 Formulario Leads -->
            <!-- INICIATIVA-803 - INI  -->
            <tr id="tdExcepcionPrecios"  style="DISPLAY: none">
                  <td>
                    <table cellpadding="0" cellspacing="0" width="100%" border="0" style="height: 24px">
						<tr>
							<td class="Header" colspan="4" align="left" height="18">&nbsp;Exepción de Precio</td>
						</tr>
					    <tr>
                            <td class="Arial10B" width="500">&nbsp;Solicitar excepcion de Precio: <asp:CheckBox ID="chkExcepPrecio" onclick="MostrarBoxSubsidio(this);" runat="server"/></td>
                            <td class="Arial10B" colspan="1"  width="250"><input class="clsInputEnabled" id="txtPrecioExcep" maxlength="7" onkeypress="validaCaracteres('0123456789.')" style="TEXT-ALIGN:right;DISPLAY: none" size="20" runat="server" onclick="" /></td>
                            <td class="Arial10B" id="lblcuotasTienda" style="DISPLAY: none" runat="server">&nbsp;Cuotas Inicial:</td>
							<td class="Arial10B" ><input class="clsInputEnabled" id="txtCuotasTienda" style="DISPLAY: none; TEXT-ALIGN: right"  onkeypress="validaCaracteres('0123456789.')"   size="20"/></td>
                        </tr>    
                    </table>
                </td>
            </tr>
            <!-- INICIATIVA-803 - INI  -->
			<!--INICIO PROY-140546-->
            <tr id="trVentanaCobroAnticipadoInstalacion" style="DISPLAY: none">
				<td>
					<table cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Datos de venta fija</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Arial10B" width="180">&nbsp;Fecha de agen. propuesta 1:</td>
							<td width="100">
                                <asp:TextBox ID="txtFechaAgendamiento1" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                                <img alt="" id="btnFechaAgendamiento1" style="border-right: 0px; border-top: 0px; cursor: pointer;
                                    border-bottom: 0px; order-left: 0px" src="../../Imagenes/calendario.gif" border="0" />
                                <script type="text/javascript">
                                    Calendar.setup(
                                                {
                                                    inputField: "txtFechaAgendamiento1",      // id of the input field
                                                    ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                    button: "btnFechaAgendamiento1",   // trigger for the calendar (button ID)
                                                    singleClick: true,           // double-click mode
                                                    step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                                }
                                            );
                                </script>
                            </td>
							<td class="Arial10B" width="105">&nbsp;Franja Horaria 1:</td>
							<td  width="220">
                                <asp:dropdownlist id="ddlFranja1" runat="server" onChange=""
									Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist>
                            </td>
							<td class="Arial10B" id="td1" width="150">&nbsp;Casilla de correo iClaro:</td>
							<td id="td2"> <input type="text" id="txtCasillaCorreoiClaro" name="txtCasillaCorreoiClaro" runat="server"
                                class="clsInputEnable" style="text-transform: uppercase; width: 100px" onpaste="return false;"
                                onkeypress="return CaracterPermitidoEmail(event);" />
                                <input id="txtCorreoDom" type="text" name="txtCorreoDom" value="@iclaro.com.pe" style="width:88px" class="clsInputDisable"/>
                              
                            </td>

							<td></td>
                            <td id="td3"></td>
						</tr>
                        <tr>
							<td class="Arial10B" width="180">&nbsp;Fecha de agen. propuesta 2:</td>
							<td width="100">
                                <asp:TextBox ID="txtFechaAgendamiento2" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                                <img alt="" id="btnFechaAgendamiento2" style="border-right: 0px; border-top: 0px; cursor: pointer;
                                    border-bottom: 0px; order-left: 0px" src="../../Imagenes/calendario.gif" border="0" />
                                <script type="text/javascript">
                                    Calendar.setup(
                                                {
                                                    inputField: "txtFechaAgendamiento2",      // id of the input field
                                                    ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                    button: "btnFechaAgendamiento2",   // trigger for the calendar (button ID)
                                                    singleClick: true,           // double-click mode
                                                    step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                                }
                                            );
                                </script>
                            </td>
							<td class="Arial10B" width="105">&nbsp;Franja Horaria 2:</td>
							<td  width="220">
                                <asp:dropdownlist id="ddlFranja2" runat="server" onChange=""
									Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist>
                            </td>
							<td class="Arial10B" id="td4" width="110">&nbsp;Publicar:</td>
							<td id="td5"><asp:CheckBox ID="chkPublicar" runat="server"></asp:CheckBox></td>
							<td></td>
                            <td id="td6"></td>
						</tr>
                        <tr>
							<td class="Arial10B" width="180">&nbsp;Fecha de agen. propuesta 3:</td>
							<td width="100">
                                <asp:TextBox ID="txtFechaAgendamiento3" runat="server" Width="75px" CssClass="clsInputEnable"
                                ReadOnly="True"></asp:TextBox>
                                <img alt="" id="btnFechaAgendamiento3" style="border-right: 0px; border-top: 0px; cursor: pointer;
                                    border-bottom: 0px; order-left: 0px" src="../../Imagenes/calendario.gif" border="0" />
                                <script type="text/javascript">
                                    Calendar.setup(
                                                {
                                                    inputField: "txtFechaAgendamiento3",      // id of the input field
                                                    ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                    button: "btnFechaAgendamiento3",   // trigger for the calendar (button ID)
                                                    singleClick: true,           // double-click mode
                                                    step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                                }
                                            );
                                </script>
                            </td>
							<td class="Arial10B" width="105">&nbsp;Franja Horaria 3:</td>
							<td  width="220">
                                <asp:dropdownlist id="ddlFranja3" runat="server" onChange=""
									Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist>
                            </td>
							<td class="Arial10B" id="td7" width="110">Medio de Pago</td>
							<td id="td8">
                            <asp:dropdownlist id="ddlMedioPago" runat="server" onChange=""
									Width="130px" CssClass="clsSelectEnableC">
								</asp:dropdownlist>
                            </td>
							<td></td>
                            <td id="td9"></td>
						</tr>
						
						
                        
                        <tr>
                            <td> 
                           
                           </td>
                        </tr>
                         
					</table>
				</td>
			</tr>
            <!--FIN PROY-140546-->
			<tr id="trAdjuntoPorta" style="DISPLAY: none">
				<td>
					<table cellpadding="0" cellspacing="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Portabilidad</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="0" width="100%"
						border="0">
						<tr>
							<td height="75"><iframe id="ifraPortabilidad" style="WIDTH: 100%; HEIGHT: 100%" src=""
									frameBorder="no" scrolling="auto"> </iframe>
							</td>
						</tr>
                        
                        <!--INICIO PROY-140419 Autorizar Portabilidad sin PIN-->
                        <tr id="pSmsSupervisor" style="display:none">
                            <td class="Arial10B" width="400" >
                            <input id="chkSmsSupervisor" type="checkbox" onclick="cambiarSmsSupervisor(this);"/>
                            Autorización de PIN por supervisor
                            </td>
                         </tr>
                        <!--INICIO PROY-140419 Autorizar Portabilidad sin PIN-->
					</table>
				</td>
			</tr>
			<tr id="trComentario" style="DISPLAY: none">
				<td>
					<table cellpadding="0" cellspacing="0" width="100%" border="0">
						<tr>
							<td class="Header" align="left" height="18">&nbsp;Comentarios del Punto de Venta</td>
						</tr>
					</table>
					<table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
						<tr>
							<td class="Arial10B" valign="top" width="150">&nbsp;Comentario:<br />
								<span style="COLOR: #ff0000">&nbsp; *(Max 200 Caracteres)</span></td>
							<td class="Arial10B"><asp:textbox id="txtComentarioPDV" onblur="return validaTextAreaLongitud(this, 200, true);" style="TEXT-TRANSFORM: uppercase"
									runat="server" Width="80%" CssClass="inputTextArea" TextMode="MultiLine" Rows="5"></asp:textbox>
                            </td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td><img height="2" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
			<tr id="trGrabar" style="DISPLAY: none" align="center">
                <!--PROY-SMSPORTA::INICIO - Cambio en funcion onclick--> <!--PROY-FULLCLARO::INICIO-->
				<td colspan="6"><input class="BotonOptm" id="btnGrabar" style="WIDTH: 150px; CURSOR: hand" onclick="validacionInicialGrabarEnviarCreditos('G');"
						type="button" value="Grabar" />&nbsp;<input class="BotonOptm" id="btnEnviarCreditos" style="WIDTH: 150px; CURSOR: hand" onclick="validacionInicialGrabarEnviarCreditos('C');"
						type="button" value="Enviar a Créditos" /></td>
                <!--PROY-FULLCLARO::FIN-->
                <!--PROY-SMSPORTA::FIN-->
			</tr>
		</table>
                   <!-- PROY 140736 INICIO -->
                              
                               <table class="Contenido" id="tblFormnBuyBackIphone" cellspacing="1"  style="DISPLAY: none" cellpadding="0"
                                width="400" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header" height="25" style="position:relative">
                                        BuyBack

                                <input id="btnCerrar" type="button" value="X" class="Boton" onclick="CloseBuyBack()"
                                style="border: none; font-size: small; position:absolute;text-align:right;color: Red; font-weight: 900; background-color: #E9F2FE;right:0px; top:0px"/></td>
                                 
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                 Equipos a canjear:
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                     <asp:DropDownList class="clsSelectEnable0" id="ddlBuyBackIphone" style="width: 210px" runat="server"
                                                        name="ddlBuyBackIphone">
                                                      </asp:DropDownList>
                                                </td>
                                             
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                   Código de cupón:
                                                </td>
                                   
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputEnabled" id="txtcodcuponBuyback" style="width: 210px; text-align: left"
                                                        onkeypress="eventoSoloNumerosEnteros();" type="text" name="txtcodcuponBuyback"  onpaste="return false;"  />
                                                </td>
                                              
                                            </tr>

                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                IMEI de equipo:
                                                </td>
                                              
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputEnabled" id="txtIMEIBuyback" style="width: 210px; text-align: left"
                                                     onkeypress="eventoSoloNumerosEnteros();" type="text" name="txtIMEIBuyback"   onpaste="return false;"  />
                                                </td>
                                              
                                            </tr>


                                            <tr>
                                                <td align="center" colspan="4">
                                                    <input class="Boton" id="Button4" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:GrabarBuyBack();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarCuotas" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <!-- Proy 29123 -->
                                                    <label id="Label1" style="font-weight: bold;color:black"></label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                       
                            <!-- PROY 140736 FIN -->
        <iframe id="iframeAuxiliar" frameBorder="no" width="10" height="10"></iframe>

 <input id="hidConstMsjeConBonoFC" type="hidden" name="hidConstMsjeConBonoFC" runat="server"  /><!--INC000003048070 -->


			<!-- --------------------------------------------------------------------------------------------------------------------------->
            <input id="hidOferta" type="hidden" name="hidOferta" runat="server"/> <!--INC000002547199 - fdq1-->
            <input id="hiModalidadPortTex" type="hidden" name="hiModalidadPortTex" runat="server"/> <!--//INC000002628010 - (INC000002547199) - fdq1-->            
            <input id="hidOperText" type="hidden" name="hidOperText" runat="server"/> <!--//INC000002628010 + 3 - fdq1-->           
			<input id="hidPerfil_149" type="hidden" name="hidPerfil_149" runat="server"/> 
            <input id="hidIntentos10" type="hidden" value="0" name="hidIntentos10" runat="server"/>
			<input id="hidBLVendedor" type="hidden" name="hidBLVendedor" runat="server"/> 
            <input id="hidListaPuntoVenta" type="hidden" name="hidListaPuntoVenta" runat="server"/>
			<input id="hidVerDetalleLinea" type="hidden" name="hidVerDetalleLinea" runat="server"/>
			<input id="hidVerVentaProactiva" type="hidden" name="hidVerVentaProactiva" runat="server"/>
			<input id="hidListaBlackList" type="hidden" name="hidListaBlackList" runat="server"/>
			<input id="hidListaParametro" type="hidden" name="hidListaParametro" runat="server"/>
            <input id="hidListaParametroII" type="hidden" name="hidListaParametroII" runat="server"/>
			<input id="hidTiempoInicioReg" type="hidden" name="hidTiempoInicioReg" runat="server"/>
			<input id="hidnMensajeValue" type="hidden" name="hidnMensajeValue" runat="server"/> 
            <input id="hidCodError" type="hidden" name="hidCodError" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS DATACREDITO --------------------------------------------->
			<input id="hidnRiesgoDCValue" type="hidden" name="hidnRiesgoDCValue" runat="server"/>
			<input id="hidNroOperacionDC" type="hidden" name="hidNroOperacionDC" runat="server"/>
			<input id="hidNRespuestaDCValue" type="hidden" name="hidNRespuestaDCValue" runat="server"/>
             <input id="hidnAutonomia" type="hidden" name="hidnAutonomia" runat="server"/>
			<input id="hidnCreditosxNombresV" type="hidden" name="hidnCreditosxNombresV" runat="server"/>
			<input id="hidCreditosxDC7" type="hidden" name="hidCreditosxDC7" runat="server"/>
			<input id="hidnAdjuntarIngreso" type="hidden" name="hidnAdjuntarIngreso" runat="server"/>
			<input id="hidCreditosxReglas" type="hidden" name="hidCreditosxReglas" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS DATACREDITO --------------------------------------------->
			<!-- ------------------------------------------------------ PARAMETROS PORTABILIDAD --------------------------------------------><input id="hidOperadorCedente" type="hidden" name="hidOperadorCedente" runat="server">
			<input id="hidNumeroContacto" type="hidden" name="hidNumeroContacto" runat="server"/>
			<input id="hidNumeroFolio" type="hidden" name="hidNumeroFolio" runat="server"/> 
            <input id="hidModalidad" type="hidden" name="hidModalidad" runat="server"/>
			<input id="hidArchivos" type="hidden" name="hidArchivos" runat="server"/> 
            <input id="hidNTienePortabilidadValues" type="hidden" name="hidNTienePortabilidadValues" runat="server"/>
    <input id="hidConsFlagConsultaPreviaChip" type="hidden" name="hidConsFlagConsultaPreviaChip"
        runat="server" />
    <!-- PROY-140223 IDEA-140462  -->
    <input id="hidConsCPCanalVenta" type="hidden" name="hidConsCPCanalVenta" runat="server" />
    <!-- PROY-140223 IDEA-140462  -->
    <input id="hidConsCPModVenta" type="hidden" name="hidConsCPModVenta" runat="server" />
    <input id="hidConsCPPuntoVenta" type="hidden" name="hidConsCPPuntoVenta" runat="server" />
    <input id="hidOficinaUsuario" type="hidden" name="hidOficinaUsuario" runat="server" />
     <!-- PROY-140223 IDEA-140462  -->
			<!-- ------------------------------------------------------ PARAMETROS PORTABILIDAD -------------------------------------------->
			<!-- ------------------------------------------------------ PARAMETROS RUC ----------------------------------------------------->
			<input id="hidListaRepresentante" type="hidden" name="hidListaRepresentante" runat="server"/>
			<input id="hidComentarioPDV" type="hidden" name="hidComentarioPDV" runat="server"/>
			<input id="hidRazonSocial" type="hidden" name="hidRazonSocial" runat="server"/> 
			<!-- ------------------------------------------------------ PARAMETROS RUC ----------------------------------------------------->
			<!-- ------------------------------------------------------ PARAMETROS DNI ----------------------------------------------------->
            <input id="hidNroDocumento" type="hidden" name="hidNroDocumento" runat="server"/>
			<input id="hidFechaNac" type="hidden" name="hidFechaNac" runat="server"/> 
            <input id="hidNombre" type="hidden" name="hidNombre" runat="server"/>
			<input id="hidApePaterno" type="hidden" name="hidApePaterno" runat="server"/>
             <input id="hidApeMaterno" type="hidden" name="hidApeMaterno" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS DNI ----------------------------------------------------->
			<input id="hidNroSEC" type="hidden" name="hidNroSEC" runat="server"/> 
            <input id="hidnOficinaActual" type="hidden" name="hidnOficinaActual" runat="server"/>
			<input id="hidnOficinaValue" type="hidden" name="hidnOficinaValue" runat="server"/> 
			<input id="hidTipoProductoActual" type="hidden" name="hidTipoProductoActual"/> 
            <input id="hidCreditosxCE" type="hidden" name="hidCreditosxCE" runat="server"/>
			<input id="hidFactorLC" type="hidden" name="hidFactorLC" runat="server"/> 
			<!-- ------------------------------------------------------ CASO ESPECIAL ------------------------------------------------------>
            <input id="hidlistaCEPlanBscs" type="hidden" name="hidlistaCEPlanBscs"/>
			<input id="hidlistaCEPlan" type="hidden" name="hidlistaCEPlan"/> 
            <input id="hidlistaCEPlanxProd" type="hidden" name="hidlistaCEPlanxProd"/>
			<input id="hidWhitelistCE" type="hidden" name="hidWhitelistCE"/> 
            <input id="hidCargoFijoMaxCE" type="hidden" name="hidCargoFijoMaxCE"/>
            <input id="hidlistaCENroPlanxProd" type="hidden" name="hidlistaCENroPlanxProd"/>
			<input id="hidCasoEspecial" type="hidden" name="hidCasoEspecial" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS CONSULTA BSCS ------------------------------------------->
			<input id="hidNroLineas" type="hidden" name="hidNroLineas" runat="server"/> 
			<input id="hidTop" type="hidden" name="hidTop" runat="server"/>
            <input id="hidEvaluarSoloFijo" type="hidden" name="hidEvaluarSoloFijo" runat="server"/>
			<input id="hidDeuda" type="hidden" name="hidDeuda" runat="server"/> 
            <input id="hidnPlanesActivos" type="hidden" name="hidnPlanesActivos" runat="server"/>
			<input id="hidArchivosEnvioCreditos" type="hidden" name="hidArchivosEnvioCreditos" runat="server"/>
			<input id="hidCreditosxAsesor" type="hidden" name="hidCreditosxAsesor" runat="server"/>
			<input id="hidnMensajeAutonomiaValue" type="hidden" name="hidnMensajeAutonomiaValue" runat="server"/>
			<input id="hidCentro" type="hidden" name="hidCentro" runat="server"/>
             <input id="hidCanalSap" type="hidden" name="hidCanalSap" runat="server"/>
			<input id="hidOrgVenta" type="hidden" name="hidOrgVenta" runat="server"/> 
            <input id="hidTipoDocVentaSap" type="hidden" name="hidTipoDocVentaSap" runat="server"/>
			<input id="hidnValueAccion" type="hidden" name="hidnValueAccion" runat="server"/> 
            <input id="hidTipoDocumento" type="hidden" name="hidTipoDocumento" runat="server"/>
			<!-- ------------------------------------------------------ UNIFICADA --------------------------------------------------------->
			<input id="hidCodEstadoSEC" type="hidden" name="hidCodEstadoSEC" runat="server"/>
			<input id="hidnLCDisponibleValue" type="hidden" name="hidnLCDisponibleValue" runat="server"/>
			<input id="hidPlanesDatosVoz" type="hidden" name="hidPlanesDatosVoz"/> 
            <input id="hidLCDisponiblexProd" type="hidden" name="hidLCDisponiblexProd" runat="server"/>
			<input id="hidPoderes" type="hidden" name="hidPoderes" runat="server"/> 
            <input id="hidListaTipoProducto" type="hidden" name="hidListaTipoProducto"/>
			<input id="hidCadenaSECPendiente" type="hidden" name="hidCadenaSECPendiente"/> 
            <input id="hidCadenaDetalle" type="hidden" name="hidCadenaDetalle" runat="server"/>
			<input id="hidGrupoPaqueteServerV" type="hidden" name="hidGrupoPaqueteServerV" runat="server"/>
			<input id="hidNServicioServerV" type="hidden" name="hidNServicioServerV" runat="server"/>
			<input id="hidNPromocionServerValue" type="hidden" name="hidNPromocionServerValue" runat="server"/>
			<input id="hidListaTipoOperacion" type="hidden" name="hidListaTipoOperacion" runat="server"/>
			<input id="hidTipoOperacion" type="hidden" name="hidTipoOperacion" runat="server"/>
			<input id="hidnTipoOfertaValue" type="hidden" name="hidnTipoOfertaValue" runat="server" /> 
			<!--ESALASB-->
            <input id="hidNroMinPlanesPorta" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidNroMinPlanesPorta" runat="server"/> 
                <input id="hidCodCampValidacion" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodCampValidacion" runat="server"/> <input id="hidCodPlanValidacion" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodPlanValidacion" runat="server"/> <input id="hidFlagRoamingI" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1" name="hidFlagRoamingI"
				runat="server"/> <input id="hidnCanalValue" type="hidden" name="hidnCanalValue" runat="server"/>
			<input id="hidCodPlanesValidacionI35" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodPlanesValidacionI35" runat="server"/> 
                <input id="hidNroMinPlanesI35" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidNroMinPlanesI35" runat="server"/> 
                <input id="hidnResultadoReglasValues" type="hidden" name="hidnResultadoReglasValues" runat="server"/>
			<input id="hidPerfilCreditos" type="hidden" name="hidPerfilCreditos" runat="server"/>
			<input id="hidLineasMarcadas" type="hidden" name="hidLineasMarcadas" runat="server"/>
			<input id="hidnRiesgoClaroValue" type="hidden" name="hidnRiesgoClaroValue" runat="server"/> 
            <input id="hidnComportamiento" type="hidden" name="hidnComportamiento" runat="server"/>
			<input id="hidnExoneracionRAValues" type="hidden" name="hidnExoneracionRAValues" runat="server"/>
			<input id="hidnPlanesActivoVozDatos" type="hidden" name="hidnPlanesActivoVozDatos" runat="server"/>
			<input id="hidListaPerfiles" type="hidden" name="hidListaPerfiles" runat="server"/>
            <input id="hidCantidadMotivoBloqueo" type="hidden" name="hidCantidadMotivoBloqueo" runat="server"/>
			<input id="hidCreditosxLineaDesactiva" type="hidden" name="hidCreditosxLineaDesactiva"
				runat="server"/> 
                <input id="hidMotivoxLineaDesactiva" type="hidden" name="hidMotivoxLineaDesactiva" runat="server"/>
            <input id="hidUsuarioRed" type="hidden" name="hidUsuarioRed" runat="server"/>
            <input id="hidNroEquipos3PlayMax" type="hidden" name="hidNroEquipos3PlayMax" runat="server" />
            <input id="hidListaComodato" type="hidden" name="hidListaComodato" runat="server" />
            <input id="hidEquipoServer" type="hidden" name="hidEquipoServer" runat="server" />
            <input id="hidnConsultPrepago" type="hidden" name="hidnConsultPrepago" runat="server" />
			<input id="hidPlanBase" type="hidden" name="hidPlanBase" runat="server"/>
			<input id="hidPlanCombo" type="hidden" name="hidPlanCombo" runat="server"/>
            <input id="hidFlgTitularidad" type="hidden" name="hidFlgTitularidad" runat="server"/>
			<input id="hidBlackListCuota" type="hidden" name="hidBlackListCuota"/>
            <input id="hidCombo" type="hidden" name="hidCombo" runat="server"/>
            <input id="hidComboText" type="hidden" name="hidComboText" runat="server"/>
            <input id="hidNroSOTMigracion" type="hidden" name="hidNroSOTMigracion" runat="server"/>
            <input id="hidFechaHoraConsulta" type="hidden" name="hidFechaHoraConsulta" runat="server"/>
            <input id="hidModalidadVenta" type="hidden" name="hidModalidadVenta" runat="server"/>
    <input id="hidBuscarSEC" type="hidden" name="hidBuscarSEC" />
    <input id="hidCombo1" type="hidden" name="hidCombo1" />
    <input id="hidTipoOperacion1" type="hidden" name="hidTipoOperacion1" />
    <input id="hidPlazoReno" type="hidden" name="hidPlazoReno" runat="server" />
    <input id="hidPlanReno" type="hidden" name="hidPlanReno" runat="server" /> <!-- [INC000002442213]VALIDACION HIDDEN -->
    <input id="hidPlanComboRestringido" type="hidden" name="hidPlanComboRestringido"
        runat="server" />
<!------------------VALIDACION DE LINEAS ------------- PROY- 140245 --------------------------->
<input id="hidcantMovilAct" type="hidden" name="hidcantMovilAct" />
<input id="hidcantFijoInalAct" type="hidden" name="hidcantFijoInalAct" />
<input id="hidcantClaroTvAct" type="hidden" name="hidcantClaroTvAct" />
<input id="hidcantBamAct" type="hidden" name="hidcantBamAct" />
<input id="hidcant3PlayAct" type="hidden" name="hidcant3PlayAct" />
<input id="hidcantInterInalAct" type="hidden" name="hidcantInterInalAct" />
<input id="hidcantPlayInalAct" type="hidden" name="hidcantPlayInalAct" />
<input id="hidcasosEspecialesColaborador" type="hidden" name="hidcasosEspecialesColaborador" />
<input id="hidFlagAgregarCarrito" type="hidden" name="hidFlagAgregarCarrito" />

<!------------------VALIDACION DE LINEAS -------------FIN  PROY- 140245 --------------------------->


  <%--//Inicio IDEA-30067--%>
            <input id="hidProductoPortAuto" type="hidden" name="hidProductoPortAuto" runat="server" />
            <%--//Fin IDEA-30067--%>

        <!-- PROY-32129 :: INI -->
        <input id="hidValoresCampEspcUniv" type="hidden" name="hidValoresCampEspcUniv" />        
        <!-- PROY-32129 :: FIN -->

<input id="hidDescCE" type="hidden" name="hidDescCE" runat="server" />
<!-- SD601959 JCFM  -->
             <input id="hidCadenaItemsCheckP" type="hidden" name="hidCadenaItemsCheckP" runat="server" /><%-- PROY-26358 - Evalenzs- hidCadenaItemsCheckP --%>
         <input id="hidClienteClarify" type="hidden" name="hidClienteClarify" runat="server" />
          <input id="hidCodProducto" type="hidden" name="hidCodProducto" runat="server" />
            <!--PROY-24724-IDEA-28174 - INICIO-->
            <input id="hidCodServProteccionMovil" type="text" name="hidCodServProteccionMovil" runat="server" style="display:none" />
            <input id="hidConcatPrimaServer" type="text" name="hidConcatPrimaServer" runat="server" style="display:none" />
            <input id="hidMsgEquipoSinCobertura" type="text" name="hidMsgEquipoSinCobertura" runat="server" style="display:none" />
            <input id="hidMsgEquipoPrecioPrepagoMenor" type="text" name="hidMsgEquipoPrecioPrepagoMenor" runat="server" style="display:none" />
            <input id="hidMontoPrecioPrepago" type="text" name="hidMontoPrecioPrepago" runat="server" style="display:none" />
            <input id="hidMsgErrorProcedurePrecioPrepago" type="text" name="hidMsgErrorProcedurePrecioPrepago" runat="server" style="display:none" />
            <input id="hidDescServProteccionMovil" type="text" name="hidDescServProteccionMovil" runat="server" style="display:none" />
            <input id="hidConcatCodTipoPdvProteccionMovil" type="text" name="hidConcatCodTipoPdvProteccionMovil" runat="server" style="display:none" />
            <input id="hidConcatCodTipoOfertaProteccionMovil" type="text" name="hidConcatCodTipoOfertaProteccionMovil" runat="server" style="display:none" />
            <input id="hidConcatCodTipoModalidadVentaProteccionMovil" type="text" name="hidConcatCodTipoModalidadVentaProteccionMovil" runat="server" style="display:none" />
            <input id="hidCodListaPrecioPrepagoMes" type="text" name="hidCodListaPrecioPrepagoMes" runat="server" style="display:none" />
            <!--PROY-24724-IDEA-28174 - FIN-->
            <input id="hidConsMensajesCPCarrito" type="hidden" name="hidConsMensajesCPCarrito" runat="server" />
            <input id="hidNroDiasCedenteOP" type="hidden" name="hidNroDiasCedenteOP" runat="server" value=""/> <!--2014 Campaña PORTABILIDAD 50% DSCTO - RMZ-->
            <input id="hidCodigoCampaniaPorta50Dscto" type="hidden" name="hidCodigoCampaniaPorta50Dscto" runat="server" value=""/> <!--2014 Campaña PORTABILIDAD 50% DSCTO - RMZ-->
            <input id="hidNroDiasPermitidosOP" type="hidden" name="hidNroDiasPermitidosOP" runat="server" value=""/> <!--2014 Campaña PORTABILIDAD 50% DSCTO - RMZ-->
            <input id="hidMsgPermanenciaOP" type="hidden" name="hidMsgPermanenciaOP" runat="server" value=""/> <!--2014 Campaña PORTABILIDAD 50% DSCTO - RMZ-->
            <input id="hidSecuenceCP" type="hidden" name="hidSecuenceCP"/>
	    <input id="hidTopeTipoProducto" type="hidden" name="hidTopeTipoProducto" runat=server/> <!--PROY-29296-FIN-->
<!--gaa20170215-->
<input id="hidBuroConsultado" type="hidden" name="hidBuroConsultado" runat="server" />
<input id="hidCampaniaPortabilidad" type="hidden" name="hidCampaniaPortabilidad" runat="server" />
<!--fin gaa20170215-->
<!-- INICIO: PROY 29215-->
<input id="hidNroCuotas" type="hidden" name="hidNroCuotas" runat="server" />
<input id="hidFormaPago" type="hidden" name="hidFormaPago" runat="server" />
<input id="hidCP" type="hidden" name="hidFormaPago" runat="server" />
<input id="hidFP" type="hidden" name="hidFormaPago" runat="server" />
<!-- FIN: PROY 29215-->
            <!--PROY-29123 VENTA EN CUOTAS -->
            <input id="hidDatosBRMS" type="hidden" name="hidDatosBRMS" runat="server" />
            <input id="hidCartaPoder" type="hidden" name="hidCartaPoder" runat="server" value="N" />
            <!-- PROY-32129 :: INI -->
             <input id='hidIdCampSelec' type="hidden" name="hidIdCampSelec" runat="server" />
             <input id='hidGraboDatosAlumnos' type="hidden" name="hidGraboDatosAlumnos" runat="server" />
             <!-- PROY-32129 :: FIN -->
            <!-- INI PROY-31636 -->
            <input id="hidCodNacionalidad" type="hidden" name="hidCodNacionalidad" runat="server"/>
            <input id="hidDesNacionalidad" type="hidden" name="hidDesNacionalidad" runat="server"/>
            <input id="hidDocsPostpago" type="hidden" name="hidDocsPostpago" runat="server"/>
           <input id="hidDocsPostpagos" type="hidden" name="hidDocsPostpagos" runat="server"/> <!-- INC000003048070--> 
            <!-- FIN PROY-31636 -->
            <!-- PROY-29121-INI -->
            <input id="hidDeudaCliente" type="hidden" name="hidDeudaCliente" runat="server" />
            <input id="hidCumpleReglaA" type="hidden" name="hidCumpleReglaA" runat="server" />
            <input id="hidFlagPrimerClic" type="hidden" name="hidFlagPrimerClic" runat="server" />
            <input id="hidCumpleReglaAClienteRRLL" type="hidden" name="hidCumpleReglaAClienteRRLL" runat="server" />
            <!-- PROY-29121-FIN -->
<!--PROY-30748 INICIO-->
            <input id="hidTotalPlanProac" type="hidden" name="hidTotalPlanProac" runat="server" value="0"/>
            <!--EMMH I-->
            <input id="hidFlagPlanesProactivos" type="hidden" name="hidFlagPlanesProactivos" runat="server" />
            <!--EMMH F-->
            <input id="hidProdMovil" type="hidden" name="hidProdMovil" runat="server" value="0"/>
    
            <input id='hidCFServAdicionales' type="hidden" name="hidCFServAdicionales" value="0"/> 
            <input id='hidServiciosAdicionales' type="hidden" name="hidServiciosAdicionales" value=""/> 
            <input id='hidServiciosAdicionalesCompatibles' type="hidden" name="hidServiciosAdicionalesCompatibles" value=""/> 
             <input id='hidFamilia' type="hidden" name="hidFamilia"/> 
             <input id='hidResumenCrediticio' type="hidden" name="hidResumenCrediticio" runat="server"/> <!-- [INC000002442213]VALIDACION HIDDEN-->
             <!--PROY-30748 FIN-->
             <input id='hidTopeMaximo' type="hidden" name="hidTopeMaximo"/>
    <input id='hidCuotaP' type="hidden" name="hidCuotaP" />
             <!--PROY-30748 INICIO-->
             <input id="hidNuevoServicio" type="hidden" name="hidNuevoServicio" runat="server"/>
             <input id="hidMontoRA" type="hidden" name="hidMontoRA" runat="server"/>
<!--PROY-30748 FIN-->
    <%--APOYO-30748-INICIO--%>
    <input id="hidPlanServicioNoTemp" type="hidden" name="hidPlanServicioNoTemp" runat="server" />
    <input id="hidPlanServicioTemp" type="hidden" name="hidPlanServicioTemp" runat="server" />
    <input id="hidFlagBtnVerOtrOpc" type="hidden" name="hidFlagBtnVerOtrOpc" runat="server" />
    <%--APOYO-30748-INICIO--%>
 <!-- PROY-26963-F3 - GPRD :: INI -->
             <input id='hAdjuntarDocumento' type="hidden" name="hAdjuntarDocumento" runat="server" />
             <!-- PROY-26963-F3 - GPRD :: FIN -->
            <!--PROY-30166-IDEA–38863–INICIO-->
            <input id="hidMontoInicialPro" type="hidden" name="hidMontoInicialPro" runat="server" /> 
            <input id="hidEsProactiva" type="hidden" name="hidEsProactiva" runat="server" />
            <input id="hidConcatCuotaServer" type="hidden" name="hidConcatCuotaServer" runat="server" />
            <!--PROY-30166-IDEA–38863–FIN -->
    <!--INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <input id="hdnEmailFacturaElectronica" type="hidden" name="hdnEmailFacturaElectronica" runat="server" />
    <input id="hdnCorreosHistoricos" type="hidden" name="hdnCorreosHistoricos" runat="server" />
    <!--FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18-->
    <%--INICIO JVERASTEGUI PROY-32280 IDEA-42248 PICKING FASE 3 DELIVERY--%>
        <input id="hdnFlagValidar" type="hidden" name="hdnFlagValidar" runat="server"/>
        <%--FIN JVERASTEGUI PROY-32280 IDEA-42248 PICKING FASE 3 DELIVERY--%>
    <!--PROY-140141–INICIO-->
    <input id='hdnSecCT' type="hidden" name="hdnSecCT" runat="server" />
    <input id='hdnNumCT' type="hidden" name="hdnNumCT" runat="server" />
    <input id='hdnOkCT' type="hidden" name="hdnOkCT" runat="server" />
    <input id='hdnEmail' type="hidden" name="hdnEmail" runat="server" />
    <!--PROY-140141–FIN-->
    <input id="hidMsgEquipoNoPlanesProac" type="hidden" name="hidMsgEquipoNoPlanesProac" runat="server"/><!--PROY 30748 F2 FALLA MDE-->
    <input id="hidCodigoValidador" type="hidden" name="hidCodigoValidador" runat="server"/>
    <input id="hdnCuotasBRMS" type="hidden" name="hdnCuotasBRMS" runat="server" />
    <input id="hdnOperacionGuardar" type="hidden" name="hdnOperacionGuardar" runat="server" />
    <%--PROY-FULLCLARO-INI--%>
    <input id="hdnLineasMovilFC" type="hidden" name="hdnLineasMovilFC" runat="server" />
    <input id="hdnLineasFijaFC" type="hidden" name="hdnLineasFijaFC" runat="server" />
    <input id="hdnVerificaBotonFC" type="hidden" name="hdnVerificaBotonFC" runat="server" />
     <%--PROY-FULLCLARO-FIN--%>

<%--INICIO PROY-140560 - Beneficio Full Claro antes grabar popup--%>
     <input id="hdnObligatoriedad" type="hidden" name="hdnObligatoriedad" runat="server" />
     <input id="CodigoFCP" type="hidden" name="CodigoFCP" runat="server" value="1"/>
     <%--FIN PROY-140560 - Beneficio Full Claro antes grabar popup--%>

     <%--INICIO PROY-140419 Autorizar Portabilidad sin PIN--%> 
       <input id="hdnValidaSupervisor" type="hidden" name="hdnValidaSupervisor" runat="server"/>
     <%--FIN PROY-140419 Autorizar Portabilidad sin PIN--%>
     
     <%--PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI--%> 
       <input id="hdnFlagPopupProactiva" type="hidden" name="hdnFlagPopupProactiva" runat="server"/>
     <%--PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN--%> 
     
     <%--PROY-140657 ADECUACIONES DE SIOP::INI--%> 
     <input id="hidnMensajeEnvioLink" type="hidden" name="hidnMensajeEnvioLink" runat="server"/>
     <%--PROY-140657 ADECUACIONES DE SIOP::FIN--%>      
     
     <%-- INI INICIATIVA 941 - IDEA 142525 --%>
     <input id="hidnNombres" type="hidden" name="hidnNombres" runat="server"/>
     <input id="hidnTipoDocumento" type="hidden" name="hidnTipoDocumento" runat="server"/>
     <input id="hidnMontoMaximo" type="hidden" name="hidnMontoMaximo" runat="server"/>
     <input id="hidnEnvioLinkFallido" type="hidden" name="hidnTipoOferta" runat="server"/>
     <input id="hidnIdAfiliacionPrevio" type="hidden" name="hidnIdAfiliacionPrevio" runat="server"/>
     <%-- FIN INICIATIVA 941 - IDEA 142525 --%>
       
        <asp:HyperLink id="hlOk" runat="server" ></asp:HyperLink>
        <asp:HyperLink id="hlControlID" runat="server" ></asp:HyperLink>
        <cc1:modalpopupextender id="modalpopup" runat="server" 
	        cancelcontrolid="hlCancel" okcontrolid="hlOk" 
	        targetcontrolid="hlControlID" popupcontrolid="PanelCarga" 
	        drag="true" RepositionMode="RepositionOnWindowResize"
	        backgroundcssclass="ModalPopupBG">
        </cc1:modalpopupextender>
    <asp:Panel ID="PanelCarga" runat="server" style="display:none">
            <table class="Contenido" cellpadding="4" bgcolor="white" style="max-width: 350px">
          <thead>
            <tr>
                <th colspan="3";>
                <label class="Arial10B">BENEFICIO FULL CLARO</label>
                </th>
                <th style="max-width: 20px">
                <input id="hlCancel" class="Boton" type="button" value="X" onclick="closeBeneficio()";  style="border:none; font-size:small; color:Red;font-weight:900; background-color:#E9F2FE; display:none"/>
                <input id="Button3" type="button" value="X" class="Boton" onclick="closeBeneficio()"; style="border:none; font-size:small; color:Red;font-weight:900; background-color:#E9F2FE" />
                </th>
            </tr>
          </thead>
          <tbody>
              <tr align="center">
               <td class="Arial10B" colspan="3">
                    <label id="prodCliente" style="color:Black"; > </label>
               </td>
               <br />
               </tr>
               <tr align="center">
                    <td colspan="3">
                        <label id="lblHeadStandar"> Desea activar el beneficio Full Claro en su servicio movil o fijo? </label><!--INC000002977281-->
                        <label id="lblHeadOnlyFija" style="display:none;MARGIN-LEFT:25px;"> Selecciona el servicio fijo para entregar el beneficio Full Claro.</label><!--INC000002977281-->
                    </td>
                </tr>
              <tr>
              <!--version 2-->
               <td colspan="2" id="idR1"> <!--INC000002977281-->
               <input id="r1" type="radio" class="Arial9B" name="radButton" value="Fija" onclick="habilitarMarcarTodos();" />Fija
                </td>
               <td  id="selLineaFija" align="center" style="display: none";>
               <table id="TblLineaFija" class="Contenido" cellpadding="3" cellspacing="5" bgcolor="white" align="left" >
                        <thead >
                            <tr>
                                <th>Linea </th>
                                <th>Plan</th>
                                <th>Elegir</th>
                            </tr>
                        </thead>
                        <tbody id="tblBodyFija">
                        </tbody>
                    </table>
               </td>
              </tr>

              <tr id="idR2"><!--INC000002977281-->
                    <td>
                    <input id="r2" type="radio" class="Arial9B" name="radButton" value="Movil" onclick="habilitarMarcarTodos();" />Movil
                    <br />
                    <br />
                </td>
                <td id="selLineaMovil" align="center" style="display: none"; >
                    <table id="TblLineaMovil" class="Contenido" cellpadding="3" cellspacing="5" bgcolor="white" align="left" >
                         <thead >
                            <tr>
                                <th>Linea </th>
                                <th>Plan</th>
                                <th>Elegir</th>
                            </tr>
                        </thead>
                        <tbody id="tblBodyMovil">
                        </tbody>
                    </table>
                    </td>
                </tr>
          </tbody>
          <tfoot>
              <tr>
                <td align="center" colspan="3">
                    <input id="Button1" type="button" value="Aceptar" class="Boton" onclick="saveBeneficio();"  style="width:80px" />
                    <input id="Button2" type="button" value="Cancelar" class="Boton" onclick="closeBeneficio()"; style="width:80px" />
                </td>
                               
              </tr>
              <tr>
              <td colspan="3">
                    <label id="lblDangerStandar" class="Arial9B" style="color:Red">*Solo se entregará el beneficio si tiene producto Fijo o Móvil activos</label> <!--INC000002977281-->
                    <label id="lblDangerOnlyFija" class="Arial9B" style="color:Red;display:none;MARGIN-LEFT: 25px;">*IMPORTANTE: El beneficio Full Claro tambien será entregado en tus líneas <br /> moviles, hasta en 5 líneas móviles de su menor cargo fijo </label> <!--INC000002977281-->
                </td>
                </tr>
          
          </tfoot>
        </table>
        </asp:Panel >
       <%--PROY-FULLCLARO-FIN--%>   
        <asp:Panel ID="PanelLineasAdicionales" runat="server" style="display:none">
            <table class="Contenido" cellpadding="4" bgcolor="white" style="max-width: 350px">
          <thead>
            <tr>
                <th colspan="3";>
                <label class="Arial10B">DETALLE DEL DESCUENTO</label>
                </th>
                <th style="max-width: 20px">
                <input id="hlCancelLineas" class="Boton" type="button" value="X" onclick="saveLineasAdicionales()";  style="border:none; font-size:small; color:Red;font-weight:900; background-color:#E9F2FE; display:none"/>
                <input id="btnCloseLineas" type="button" value="X" class="Boton" onclick="saveLineasAdicionales()"; style="border:none; font-size:small; color:Red;font-weight:900; background-color:#E9F2FE" />
                </th>
            </tr>
          </thead>
          <tbody>
              <tr align="center">
               <td class="Arial10B" colspan="3">
                    <label id="prodClienteL" style="color:Black"; > </label>
               </td>
               <br />
               </tr>
              <tr id="trLineasAdic">
                <td id="tdLineasAdic" align="center">
                    <table id="TblLineasAdic" class="Contenido" cellpadding="3" cellspacing="5" bgcolor="white" align="center" style="text-align:center;">
                         <thead >
                            <tr>
                                <th>Línea </th>
                                <th>Plan</th>
                                <th>Descuento 50%</th>
                            </tr>
                        </thead>
                        <tbody id="tblBodyLineasAdic">
                        </tbody>
                    </table>
                    </td>
                </tr>
          </tbody>
          <tfoot>
              <tr>
                <td align="center" colspan="3">
                    <input id="btnCloseLineasAdic" type="button" value="Aceptar" class="Boton" onclick="saveLineasAdicionales();"  style="width:80px" />
                </td>
                               
              </tr>
              <tr>
              <td colspan="3">
                    <label id="lblInfor" class="Arial9B" style="color:Red;display:none">*Solo se entregará el beneficio si tiene producto Fijo o Móvil activos</label>
                </td>
                </tr>
          
          </tfoot>
        </table>
        </asp:Panel >
    <!--PROY-140383-INI-->
    <input id="hidInsServCon" type="hidden" name ="hidServDetalles" runat="server" />
    <input id="hidServicioExcCaidoeval" type="hidden" name ="hidServicioExcCaidoeval" runat="server" />
    <!--PROY-140383-FIN-->
    <!--PROY-140618-INI-->
    <input id="hidCadDetalleMejPor" type="hidden" name ="hidCadDetalleMejPor" runat="server" />
    <!--PROY-140618-FIN-->
    <!--INI: PROY-140335 RF1-->
    <input id="hidEjecucionCPBRMS" value="0" type="hidden" name="hidFlagCPBRMS" runat="server" />
    <input id="hidLineasSinCP" type="hidden" name="hidLineasSinCP" runat="server" />
    <input id="hidDatosPorta" type="hidden" name="hidDatosPorta" runat="server" />
    <input id="hidDatosPortaProactivoCP" type="hidden" name="hidDatosPortaProactivoCP" runat="server" />
    <input id="hidFlagPortaProactivoCP" type="hidden" name="hidFlagPortaProactivoCP" runat="server" />
    <input id="hidlineaPortaCPOK" type="hidden" name="hidlineaPortaCPOK" runat="server" />
    <input id="hidLineasRec" type="hidden" name="hidLineasRec" runat="server" />
    <input id="hidFlagCPPermitidaProa" type="hidden" name="hidFlagCPPermitidaProa" runat="server" />
    <!--FIN: PROY-140335 RF1-->
<!--IDEA-142010 INI-->
    <input id="hdiValidacionCantidadCampanas" type="hidden" name ="hdiValidacionCantidadCampanas" runat="server" />
    <input id="hdiRestriccionCarrito" type="hidden" name ="hdiRestriccionCarrito" runat="server" />
    <!--IDEA-142010 FIN-->

    <!-- INICIATIVA - 803 - INI -->
     <input id="hidFlagTiendaVirtual" type="hidden" runat="server" />
     <input id="hidFlagServicio" type="hidden" runat="server" />
     <input id="hidMontoEquipoVenta" type="hidden" name ="hidMontoEquipoVenta" runat="server" />
     <input id="hidFlagDelivery" type="hidden" name="hidFlagDelivery" runat="server" />
     <input id="hidCuotaIncialTienda" type="hidden" name="hidCuotaIncialTienda" runat="server" />
     <input id="hidFlagApagadoExcepcionPrecio" type="hidden" name="hidFlagApagadoExcepcionPrecio" runat="server" />
    <!-- INICIATIVA - 803 - FIN-->
       <!-- PROY - 140736 - INI -->
     <input id="hdbuyback" type="hidden" runat="server" />
     <input id="hdCampaniaBuyBack" type="hidden" runat="server" />
     <input id="hidValorEquipo_cv" type="hidden" runat="server" />
      <input id="hdsecbuyback" type="hidden" name="hdsecbuyback" runat="server" value="0" />
     <input id="hdmaterialCanje" type="hidden" name="hdmaterialCanje" runat="server" />
    <!-- PROY - 140736- FIN-->
        <input id="hidDesProducto" type="hidden" name="hidDesProducto" runat="server" /><%--PROY-140657--%>
    <input id="hidDescTipoOperacion" type="hidden" name="hidDescTipoOperacion" runat="server" /><%--140657 --%>

	<!--PROY-140546 Inicio-->
    <!--FALLAS PROY-140546-->
    <input id="hidTipoCobroAnticipadoInstalacion" value="0" type="hidden" name ="hidTipoCobroAnticipadoInstalacion" runat="server" />
    <input id="hidCobroAnticipadoInstalacion" type="hidden" name ="hidCobroAnticipadoInstalacion" runat="server" />
    <input id="hidFlagAplicaCAI" type="hidden" name ="hidFlagAplicaCAI" runat="server" />
    <input id="hidTiempoSecPendientePagoLink" type="hidden" name ="hidTiempoSecPendientePagoLink" runat="server" />
    <input id="hidCodigoPDVTeleventas" type="hidden" name ="hidCodigoPDVTeleventas" runat="server" />
    <input id="hidMAI" type="hidden" name ="hidMAI" runat="server" />
    <input id="hidMsjValidacionSubformularioCAI" type="hidden" name ="hidMsjValidacionSubformularioCAI" runat="server" />
    <input id="hidCanalesPermitidosCAI" type="hidden" name ="hidCanalesPermitidosCAI" runat="server" />
    <input id="hidCaiDescuentoFullClaro" type="hidden" name ="hidCaiDescuentoFullClaro" runat="server" />
    <input id="hidFlagCaiFullClaro" type="hidden" name ="hidFlagCaiFullClaro" runat="server" />
    <input id="hidMAIFullClaro" type="hidden" name ="hidMAIFullClaro" runat="server" />
    <!--PROY-140546 Fin-->
    <input id="hidRestringirCoberturaIFI" type="hidden" name ="hidRestringirCoberturaIFI" runat="server" />
    <input id="hidPendienteEnvioBonoFC" type="hidden" name ="hidPendienteEnvioBonoFC" runat="server" />   <!--INC000004280198-->  
    <input id="hidEstadoFullClaro" type="hidden" name ="hidEstadoFullClaro" runat="server" />   <!--INC000004280198-->

    <input id="hidMensajeConfiguracion" type="hidden" name="hidMensajeConfiguracion" runat="server" />
    <!--IDEA-941.Modificaciones siop .js-->

    <!-- INICIATIVA - 920 - INI -->
      <input id="hdModalidaDefecto" name="hdModalidaDefecto" type="hidden" runat="server"/>
     <!-- INICIATIVA - 920 - FIN -->
<input id="hidLineasAdicionales" type="hidden" name="hidLineasAdicionales" runat="server" />
        <input id="hidPoblarTabla" type="hidden" name="hidCantLineasAdicionales" runat="server" />

    <input id="hdiRestriccionCampanasFullClaro" type="hidden" name ="hdiRestriccionCampanasFullClaro" runat="server" />   <!--INCICIATIVA-1012-->

    <!--PROY-140743 Inicio-->
    <input id="hidIsClienteClaro" type="hidden" name ="hidIsClienteClaro" runat="server" />
    <input id="hidFlagVentaVV" type="hidden" name ="hidFlagVentaVV" runat="server" />
    <input id="hidDatosEvalVV" type="hidden" name ="hidDatosEvalVV" runat="server" />
    <input id="hidAccPermiteCuotasVV" type="hidden" name ="hidAccPermiteCuotasVV" runat="server" />
    <input id="hidGuardarLineas" type="hidden" name ="hidGuardarLineas" runat="server" />
    <input id="hidProdCuentaFact" type="hidden" name ="hidProdCuentaFact" runat="server" />
    <!--PROY-140743 Fin -->

    </form>
</body>
</html>
