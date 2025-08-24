<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_popup_evaluacion_proactiva.aspx.cs" Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_popup_evaluacion_proactiva" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
	<head id="Head1" runat="server">
		<title>Sisact Evaluación Proactiva</title>
        <link href="../../Estilos/general.css" rel="stylesheet" type="text/css" />
        <script language="javascript" type="text/javascript" src="../../Scripts/funciones_sec.js"></script>
        <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_evaluacion.js"></script>
        <script type="text/javascript" language="JavaScript" src="../../Scripts/Lib_FuncValidacion.js"></script>
        <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
        <script src="../../Scripts/security.js" type="text/javascript"></script>
        <script src="../../Scripts/json2.js" type="text/javascript"></script>
        <script type="text/javascript">
		    var g_constModalidadPagoEnCuota = '<%= ConfigurationManager.AppSettings["constCodModalidadCuota"] %>';
             var constCodModalidadChipSuelto = '<%= ConfigurationManager.AppSettings["constCodModalidadChipSuelto"] %>';
             var constTextoNoAplicaCondiciones = '<%= ConfigurationManager.AppSettings["constTextoNoAplicaCondiciones"] %>';
             var constTextoNoAprobadoAutonomia = '<%= ConfigurationManager.AppSettings["constTextoNoAprobadoAutonomia"] %>';
             var BloqueoEquipoSinStockColor = '<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>';
             var constMsjEquipoSinStockSel = '<%= ConfigurationManager.AppSettings["constMsjEquipoSinStockSel"] %>';
             //INICIATIVA 920
             var g_constModalidadPagoEnCuotaSinCode = '<%= ConfigurationManager.AppSettings["constCodModalidadCuotaSinCode"] %>';
        </script>
        <script type="text/javascript" language="JavaScript" src="../../Scripts/Paginas/evaluacion_cons/sisact_popup_evaluacion_proactiva.js"></script>
	</head>
	<body onload="carga();">
		<form id="Form1" method="post" runat="server">
		
        <table class="contenido" border="0" cellPadding="1" width="100%">
				<tr>
					<td class="Header" align="center" height="19" colspan="8">
						<label>Evaluación Proactiva</label>
					</td>
				</tr>
				<tr>
					<td colspan="8">
						<div style="OVERFLOW:hidden">
							<div id="DivComparar1" style="WIDTH:70%; FLOAT:left">
								
								<table>
                                <tr></tr>
                                <tr></tr>
                                <tr></tr>
									<tr>
										<td class="Arial10B">CAMPAÑA</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblcampania"></label></td>
									</tr>
									<tr>
										<td class="Arial10B">PLAZO</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblplazo"></label></td>
									</tr>
									<tr id="trCuotas" style="display:none;">
										<td class="Arial10B">N° CUOTAS</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblNroCuotas" runat="server" ></label></td>
									</tr>
                                    <tr id="trPlan1">
										<td class="Arial10B">PLAN 1</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblPlan1"></label></td>
									</tr>
                                    <tr id="trPlan2" style="display:none;">
										<td class="Arial10B">PLAN 2</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblPlan2"></label></td>
									</tr>
                                    <tr id="trPlan3" style="display:none;">
										<td class="Arial10B">PLAN 3</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblPlan3"></label></td>
									</tr>
                                    <tr id="trPlan4" style="display:none;">
										<td class="Arial10B">PLAN 4</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblPlan4"></label></td>
									</tr>
                                    <tr>
										<td class="Arial10B"><label id="lblEquipoDes"></label></td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblEquipo"></label></td>
									</tr>
								</table>
														
								<table id="tblPlanesProAct" border="0" cellSpacing="0" borderColor="#95b7f3" cellPadding="0"
									style="align: center" width="100%">
									<thead>
										<tr>
											<td class="TablaTitulos" align="center" height="19" colspan="9">
												<!--<label id="lblEquipo"></label>-->
											</td>
										</tr>
										<tr>
                                        <!--PROY-CAMPANA NAVIDEÑA::INI-->
											<td class="TablaTitulos" align="center"></td>
											<td class="TablaTitulos" align="center">Plan</td>
                                        <!--<td class="TablaTitulos" align="center">Nº Cuotas</td>--> 
										<td class="TablaTitulos" align="center">Cuota Inicial (A)</td>
										<td class="TablaTitulos" align="center" id="TDMontoRa"> </td>
										<td class="TablaTitulos" align="center">Precio de Venta (B)</td>
										<td class="TablaTitulos" align="center" id="TDPagoInicial"> </td>
                                        <td class="TablaTitulos" align="center">Cuota Mensual Equipo (C)</td>
                                        <td class="TablaTitulos" align="center">Cargo Fijo (D)</td>
                                        <td class="TablaTitulos" align="center">Pago Combo (Mensual) (C+D)</td>
                                         <!--PROY-CAMPANA NAVIDEÑA::FIN-->
										</tr>
									    
									</thead>
								    <tr>
								        <td align="center" height="19" colspan="9">
								        </td>
								    </tr>
								</table>
								<br/>

								<table border="0" cellSpacing="0" cellPadding="1" width="100%">
									<TR>
										<TD align="center">
											<INPUT style="WIDTH: 190px; HEIGHT: 19px; CURSOR: hand" id="btnVerOpciones" class="Boton" onmouseover="this.className='BotonResaltado';"
												onmouseout="this.className='Boton';" value="Ver otras opciones de equipos" type="button"
												name="btnVerOpciones" onclick="FBtnVerOpciones()" >
										</TD>
									</TR>
								</table>
							</div>
							
							<div id="DivComparar2" style="WIDTH:32%; DISPLAY:none; FLOAT:left; MARGIN-LEFT:0.5%">
							
								<table>
								<tr>
									<td>
										<div class='AutoComplete_Div'>
											<input type='hidden' id='hidMaterial1' name='hidMaterial1' />
											<input type='hidden' id='hidValorEquipo1' name='hidValorEquipo1' />
											<input id='txtTextoEquipo1' name='txtTextoEquipo1'  
												class='clsSelectEnable0' style='width: 165px' 
												onclick="mostrarListaSel(1);" onkeyup="buscarCoincidente(1);" onblur="ocultarListaTxt(1);"/>
											<img id='imgListaEquipo1' src='../../Imagenes/cmb.gif' 
												style='height: 17px; cursor: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' 
												onclick="mostrarListaSel(1);" onmouseover="imgSel(this);" onmouseout="imgNoSel(this);" onblur="ocultarListaTxt(1);" />
										</div>
										
										<div id='divListaEquipo1'
											class='AutoComplete_List' style='z-index: 10; width: 255px; DISPLAY: none; font-size:10px'; 
											onblur="ocultarListaTxt(1);">
										</div>
										<input type='hidden' id='hidKit1' name='hidKit1' />
										<input type='hidden' id='hidListaPreciosEquipo1' name='hidListaPreciosEquipo1' />
										
									</td>
                                    <td class="Arial10b">
										<input type="button" style="WIDTH: 100px; HEIGHT: 19px; CURSOR: hand" id="btnEvaluar2" class="Boton" 
											onmouseover="this.className='BotonResaltado';"
											onmouseout="this.className='Boton';" 
											value="Comparar" name="btnEvaluar2" onclick="FbtnEvaluar2();">
									</td>

								</tr>
                                </table>
                                <table>
                                 <tr>
										<td class="Arial10B">PLAZO &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblplazo2"></label></td>
									</tr>
									<tr id="trCuotas2" style="display:none;">
										<td class="Arial10B">N° CUOTAS</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblNroCuotas2" runat="server" ></label></td>
									</tr>
                                    <tr id="trCom2Plan1" style="display:none;">
										<td class="Arial10B">PLAN 1</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom2Plan1"></label></td>
									</tr>
                                    <tr id="trCom2Plan2" style="display:none;">
										<td class="Arial10B">PLAN 2</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom2Plan2"></label></td>
									</tr>
                                    <tr id="trCom2Plan3" style="display:none;">
										<td class="Arial10B">PLAN 3</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom2Plan3"></label></td>
									</tr>
                                    <tr id="trCom2Plan4" style="display:none;">
										<td class="Arial10B">PLAN 4</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom2Plan4"></label></td>
									</tr>
                                    <tr>
										<td class="Arial10B">EQUIPO</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblEquipo1"></label></td>
									</tr>

                                </table>
							       
								
							
								<table id="tblPlanesProActEq1" border="0" cellSpacing="0" borderColor="#95b7f3" cellPadding="0"
									style="align: center; width:100%;">
									<thead>
									<tr>
										<td class="TablaTitulos" align="center" height="19" colspan="9">
											<%--<label id="lblEquipo1"></label>--%>
										</td>
									</tr>
									<tr>
										<!--PROY-CAMPANA NAVIDEÑA::INI-->
										<td class="TablaTitulos" align="center"></td>
										<td class="TablaTitulos" align="center">Plan</td>
                                        <!--<td class="TablaTitulos" align="center">Nº Cuotas</td>--> 
										<td class="TablaTitulos" align="center">Cuota Inicial (A)</td>
										<td class="TablaTitulos" align="center" id="TDMontoRa1"> </td>
										<td class="TablaTitulos" align="center">Precio de Venta (B)</td>
										<td class="TablaTitulos" align="center" id="TDPagoInicial1"></td>
                                        <td class="TablaTitulos" align="center">Cuota Mensual Equipo (C)</td>
                                        <td class="TablaTitulos" align="center">Cargo Fijo (D)</td>
                                        <td class="TablaTitulos" align="center">Pago Combo (Mensual) (C+D)</td>
                                         <!--PROY-CAMPANA NAVIDEÑA::FIN-->
									</tr>
									</thead>
								</table>
							</div>
							
							<div id="DivComparar3" style="WIDTH:32%; DISPLAY:none; FLOAT:left; MARGIN-LEFT:0.5%">
								
								<table>
								<tr>
									<td class="Arial10b" style="font-weight:normal;">
										<div class='AutoComplete_Div'>
											<input type='hidden' id='hidMaterial2' name='hidMaterial2' />
											<input type='hidden' id='hidValorEquipo2' name='hidValorEquipo2' />
											<input id='txtTextoEquipo2' name='txtTextoEquipo2' 
												class='clsSelectEnable0' style='WIDTH: 179px' 
												onclick="mostrarListaSel(2);" onkeyup="buscarCoincidente(2);" onblur="ocultarListaTxt(2);" >
											<img id='imgListaEquipo2' src='../../Imagenes/cmb.gif' 
												style='HEIGHT: 17px; CURSOR: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' 
												onclick="mostrarListaSel(2);" onmouseover="imgSel(this);" onmouseout="imgNoSel(this);" onblur="ocultarListaTxt(2);" >
										</div>
										
										<div id='divListaEquipo2'
											class='AutoComplete_List' style='Z-INDEX: 10; WIDTH: 262px; DISPLAY: none; font-size:10px'; 
											onblur="ocultarListaTxt(2);">
										</div>
										<input type='hidden' id='hidKit2' name='hidKit2' />
										<input type='hidden' id='hidListaPreciosEquipo2' name='hidListaPreciosEquipo1' />
									</td>
                                    <td class="Arial10b">
										<input type="button" style="WIDTH: 100px; HEIGHT: 19px; CURSOR: hand" id="btnEvaluar3" class="Boton" 
											onmouseover="this.className='BotonResaltado';"
											onmouseout="this.className='Boton';" 
											value="Comparar" 
											name="btnEvaluar3" onclick="FbtnEvaluar3();"/>
									</td>
								</tr>
								</table>
                                 <table>
                                   <tr>
										<td class="Arial10B">PLAZO &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblplazo3"></label></td>
									</tr>
									<tr id="trCuotas3" style="display:none;">
										<td class="Arial10B">N° CUOTAS</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblNroCuotas3" runat="server" ></label></td>
									</tr>
                                    <tr id="trCom3Plan1" style="display:none;">
										<td class="Arial10B">PLAN 1</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom3Plan1"></label></td>
									</tr>
                                    <tr id="trCom3Plan2" style="display:none;">
										<td class="Arial10B">PLAN 2</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom3Plan2"></label></td>
									</tr>
                                    <tr id="trCom3Plan3" style="display:none;">
										<td class="Arial10B">PLAN 3</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom3Plan3"></label></td>
									</tr>
                                    <tr id="trCom3Plan4" style="display:none;">
										<td class="Arial10B">PLAN 4</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblCom3Plan4"></label></td>
									</tr>
                                    <tr>
										<td class="Arial10B">EQUIPO</td>
										<td class="Arial9B" style="FONT-WEIGHT:normal"><label id="lblEquipo2"></label></td>
									</tr>

                                </table>
							
								<table id="tblPlanesProActEq2" border="0" cellSpacing="0" borderColor="#95b7f3" cellPadding="0"
									style="align: center; width:100%;">
									<thead>
									<tr>
										<td class="Header" align="center" height="19" colspan="9">
											<%--<label id="lblEquipo2"></label>--%>
										</td>
									</tr>
									<tr>
										<!--PROY-CAMPANA NAVIDEÑA::INI-->
										<td class="TablaTitulos" align="center"></td>
										<td class="TablaTitulos" align="center">Plan</td>
                                        <!--<td class="TablaTitulos" align="center">Nº Cuotas</td>--> 
										<td class="TablaTitulos" align="center">Cuota Inicial (A)</td>
										<td class="TablaTitulos" align="center"id="TDMontoRa2"> </td>
										<td class="TablaTitulos" align="center">Precio de Venta (B)</td>
										<td class="TablaTitulos" align="center" id="TDPagoInicial2"></td>
                                        <td class="TablaTitulos" align="center">Cuota Mensual Equipo (C)</td>
                                        <td class="TablaTitulos" align="center">Cargo Fijo (D)</td>
                                        <td class="TablaTitulos" align="center">Pago Combo (Mensual) (C+D)</td>
                                         <!--PROY-CAMPANA NAVIDEÑA::FIN-->
									</tr>
									</thead>
								</table>
							</div>
							
						</div>
					</td>
				</tr>
			</table>
			
            <iframe id="iframeAuxiliar" style="display:none;"></iframe>
			<input id="hidEquiposSel1" type="hidden" name="hidEquiposSel1"/>
			<input id="hidEquiposSel2" type="hidden" name="hidEquiposSel2"/>
			<input type="hidden" id="hidTablePlan" name="hidTablePlan" runat="server"/>
            <input type="hidden" id="HidColorBloqueo" name="HidColorBloqueo" runat="server"/>
            <input type="hidden" id="HidMenColorBloqueo" name="HidMenColorBloqueo" runat="server"/>
            <input id="hidCodcampania" type="hidden" name="hidCodcampania" runat="server" />
            <input id="hidMsgEquipoNoPlanesProac" type="hidden" name="hidMsgEquipoNoPlanesProac" runat="server" />
            <input id="hidListaPrecio1" type="hidden" name="hidListaPrecio1" runat="server" /><!--PROY 30748 F2 MDE-->
            <input id="hidEquipoPrecio1" type="hidden" name="hidEquipoPrecio1" runat="server" /><!--PROY 30748 F2 MDE-->
            <input id="hidListaPrecio2" type="hidden" name="hidListaPrecio2" runat="server" /><!--PROY 30748 F2 MDE-->
            <input id="hidEquipoPrecio2" type="hidden" name="hidEquipoPrecio2" runat="server" /><!--PROY 30748 F2 MDE-->
            <div id="DCargando" 
				style="position:absolute; top:0px; right:0px; z-index:100; width:100%; height:100%; display:none; background-color:silver; opacity:0.3; filter:alpha(opacity=30);">
				
				<div style="position:absolute; z-index:101; margin-left:50%; margin-top:200px; background-color:#fff;">
					<img src="../../Imagenes/cargando3.gif" >
				</div>
			</div>
		</form>
        <style type="text/css">
            table.Contenido
            {
                font-size: 3pt !important;
            }
            .Arial10B
            {
                font-size: 10px;
            }

            .TablaTitulos, tr, td
            {
                font-size: 9px !important; 
            }
        </style>
	</body>
</html>