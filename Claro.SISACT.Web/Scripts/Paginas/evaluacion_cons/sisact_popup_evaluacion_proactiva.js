
		    var lstEquipos, lstplanes;
		    var blnOtrasOpciones = true;
		    var g_strCodEquipo;
		    var g_strCodModalidad;
		    var g_strNroDocumento;
		    var g_strIdtipoOperacion;
		    var g_stfamilia;
		    var g_strCFServAdic;
		    var g_strServAdic;

		    var g_intRow;
		    var g_CodCampania;
		    var g_CodPlazo;
		    var g_CodPlan;
		    var g_CodTipProdActual;
		    
		    var g_CodOferta;
		    var g_CodOficina;
		    var g_CodCanalSap;
		    var g_CodOrgVenta;
		    var g_CodTipoDocVentaSap;
		    var g_CodOficinaActual;
		    var g_CodCFPlanServicio;
		    var g_lstMaterialPrecio;
		    var g_decMaterialPrecio;
		    var g_CodCuota;
		    var g_strCodFamPlan;

		    var g_nrcuotas;
		    var g_porcIni;
		    var g_cuotaini;
		    var g_idFlujo;
		    var g_idFilaProa;
		    var g_TopeMaxCuotas;

		    function carga() {
		        $('#lblcampania').text(ExtraeURL(document.URL, 'descampana'));
		        $('#lblplazo').text(ExtraeURL(document.URL, 'desplazo'));
		        $('#lblplazo2').text(ExtraeURL(document.URL, 'desplazo'));
		        $('#lblplazo3').text(ExtraeURL(document.URL, 'desplazo'));
		        $('#hidCodEquipo').val(ExtraeURL(document.URL, 'codequipo'));
		        lstEquipos = window.dialogArguments.lstMaterial;

		        g_TopeMaxCuotas = window.dialogArguments.TopeMaxCuotas;

		        g_strCodModalidad = ExtraeURL(document.URL, 'codmodalidad');
		        $('#lblEquipo').text(ExtraeURL(document.URL, 'desequipo'));
		        $('#lblEquipoDes').text('EQUIPO');
		        g_strNroDocumento = ExtraeURL(document.URL, 'nroDocumento');
		        g_strIdtipoOperacion = ExtraeURL(document.URL, 'idtipoopera');
		        g_stfamilia = ExtraeURL(document.URL, 'familia');
		        g_strCFServAdic = ExtraeURL(document.URL, 'strCFServAdic');
		        //cristhian
                g_strServAdic = ExtraeURL(document.URL, 'strServAdic');
                //fin
		        g_nrcuotas = ExtraeURL(document.URL, 'nrcuotas');
		        g_porcIni = ExtraeURL(document.URL, 'porcIni');
		        g_cuotaini = ExtraeURL(document.URL, 'cuotaini');
		        g_idFlujo = ExtraeURL(document.URL, 'idFlujo');
		        g_idFilaProa = ExtraeURL(document.URL, 'idFilaProa');

		        if (g_strCodModalidad == constCodModalidadChipSuelto) {
		            $('#lblEquipoDes').text('');
		            $('#lblEquipo').text('CHIP SUELTO');
		        }


		        g_intRow = window.dialogArguments.Row;
		        g_CodCampania = ExtraeURL(document.URL, 'campana');
		        g_CodPlazo = ExtraeURL(document.URL, 'plazo');
		        g_strCodFamPlan = ExtraeURL(document.URL, 'familia');
		        g_CodPlan = ExtraeURL(document.URL, 'codplan');
		        g_strCodEquipo = window.dialogArguments.codequipo;
		        g_CodTipProdActual = window.dialogArguments.CodTipProdActual;
		        g_lstMaterialPrecio = window.dialogArguments.lstMaterialPrecio;
		        g_decMaterialPrecio = window.dialogArguments.decMaterialPrecio;
		        g_CodOferta = window.dialogArguments.CodOferta;
		        g_CodOficina = ExtraeURL(document.URL, 'CodOficina');
		        g_CodCanalSap = window.dialogArguments.CodCanalSap;
		        g_CodOrgVenta = window.dialogArguments.CodOrgVenta;
		        g_CodTipoDocVentaSap = window.dialogArguments.CodTipoDocVentaSap;
		        g_CodOficinaActual = '';
		        g_CodCFPlanServicio = window.dialogArguments.CodCFPlanServicio;
		        g_CodCuota = window.dialogArguments.CodCuota;
		        //INICIATIVA 920
		        if (g_strCodModalidad == '3' || g_strCodModalidad == '5') {
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(2)').fadeIn('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(6)').fadeIn('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(7)').fadeIn('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(8)').fadeIn('fast');

		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(2)').fadeIn('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(6)').fadeIn('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(7)').fadeIn('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(8)').fadeIn('fast');

		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(2)').fadeIn('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(6)').fadeIn('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(7)').fadeIn('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(8)').fadeIn('fast');

		        } else {
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(2)').fadeOut('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(6)').fadeOut('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(7)').fadeOut('fast');
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(8)').fadeOut('fast');

		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(2)').fadeOut('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(6)').fadeOut('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(7)').fadeOut('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(8)').fadeOut('fast');

		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(2)').fadeOut('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(6)').fadeOut('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(7)').fadeOut('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(8)').fadeOut('fast');
		        }
                        //INICIATIVA 920
		        if (g_strCodModalidad == '2' || g_strCodModalidad == '4') {
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(4)').fadeIn('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(4)').fadeIn('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(4)').fadeIn('fast');
		        } else {
		            $('#tblPlanesProAct thead tr:eq(1) td:eq(4)').fadeOut('fast');
		            $('#tblPlanesProActEq1 thead tr:eq(1) td:eq(4)').fadeOut('fast');
		            $('#tblPlanesProActEq2 thead tr:eq(1) td:eq(4)').fadeOut('fast');
		        }

		        lstplanes = window.dialogArguments.arrEvaluacion;

		        switch ($('#hidTablePlan').val()) {
		            case 'ERROR': window.returnValue = 'ERROR';
		                window.close();
		                break;
		            case 'VALIDACION': window.returnValue = 'VALIDACION';
		                window.close();
		                break;
		            default: FInicio(g_strCodModalidad, lstplanes);
		                break;
		        }


		        //		        if ($('#hidTablePlan').val() != '') {
		        //		            FInicio(g_strCodModalidad, lstplanes);
		        //		        }
		        //		        else {
		        //		            window.returnValue = 'ERROR';
		        //		            window.close();
		        //		        }

		        if (lstEquipos == '') {
		            $('#btnVerOpciones').hide();
		        } else {
		            $('#btnVerOpciones').show();
		        }
				
		        
		    }

		    function FVerOpciones() {
		        if (blnOtrasOpciones == false) {

		            $('#btnVerOpciones').val('Ver otras opciones de equipos');
		            blnOtrasOpciones = true;
		            $('#DivComparar2').fadeOut('fast');
		            $('#DivComparar3').fadeOut('fast');
		            document.getElementById('DivComparar1').style.width = '70%';
		        } 
                else {

		            $('#btnVerOpciones').val('Ocultar otras opciones de equipo');
		            blnOtrasOpciones = false;
		            $('#DivComparar2').fadeIn('fast');
		            $('#DivComparar3').fadeIn('fast');
                    //EMMH I
		            document.getElementById('DivComparar1').style.width = '33%';
		            document.getElementById('DivComparar2').style.width = '33%';
		            document.getElementById('DivComparar3').style.width = '33%';
		            //EMMH F
		            var divListaEquipo1 = document.getElementById('divListaEquipo1');
		            var divListaEquipo2 = document.getElementById('divListaEquipo2');
		            var hidValorEquipo1 = document.getElementById('hidValorEquipo1');
		            var txtTextoEquipo1 = document.getElementById('txtTextoEquipo1');
		            var hidValorEquipo2 = document.getElementById('hidValorEquipo2');
		            var txtTextoEquipo2 = document.getElementById('txtTextoEquipo2');

		            $('#hidMaterial1').val(lstEquipos);
		            $('#hidMaterial2').val(lstEquipos);

		            hidValorEquipo1.value = '';
		            txtTextoEquipo1.value = 'SELECCIONE...';
		            hidValorEquipo2.value = '';
		            txtTextoEquipo2.value = 'SELECCIONE...';

		            llenarDatosCombo1(divListaEquipo1, lstEquipos, 1, 'Equipo', false);
		            llenarDatosCombo1(divListaEquipo2, lstEquipos, 2, 'Equipo', false);
		        }

		        $('#tblPlanesProActEq1,#tblPlanesProActEq2').find('tbody').remove();
		        $('#lblEquipo1,#lblEquipo2').text('');
		        $('#DCargando').fadeOut('fast');
		    }
		    function FBtnVerOpciones() {
		        cargarImagenEsperando(
				function () {
				    $('#DCargando').fadeIn('fast');
				},

                function () {
                    window.setTimeout(FVerOpciones, 1000);
                },

				function () {

				}

                );
		    }

		    function FInicio(CodModalidad, lstplanes) {
		        var cantPlanes = lstplanes.length;   
		        var arrPlanes = "";
		        for (var row = 0; row < cantPlanes; row++) {
		            arrPlanes += lstplanes[row].desplan + '|';
		        }

		        var arrPlan = arrPlanes.split('|');
		        if (cantPlanes == 1) {
		            $('#trPlan1').fadeIn('fast');
		            $('#trCom2Plan1').fadeIn('fast');
		            $('#trCom3Plan1').fadeIn('fast');
		            document.getElementById('lblPlan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom2Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom3Plan1').innerHTML = arrPlan[0];	
                    	            
		        }
		        if (cantPlanes == 2) {
		            $('#trPlan1').fadeIn('fast');
		            $('#trPlan2').fadeIn('fast');
		            $('#trCom2Plan1').fadeIn('fast');
		            $('#trCom2Plan2').fadeIn('fast');
		            $('#trCom3Plan1').fadeIn('fast');
		            $('#trCom3Plan2').fadeIn('fast');
		            document.getElementById('lblPlan1').innerHTML = arrPlan[0];
		            document.getElementById('lblPlan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom2Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom2Plan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom3Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom3Plan2').innerHTML = arrPlan[1];	
		        }
		        else if (cantPlanes == 3) {
		            $('#trPlan1').fadeIn('fast');
		            $('#trPlan2').fadeIn('fast');
		            $('#trPlan3').fadeIn('fast');
		            $('#trCom2Plan1').fadeIn('fast');
		            $('#trCom2Plan2').fadeIn('fast');
		            $('#trCom2Plan3').fadeIn('fast');
		            $('#trCom3Plan1').fadeIn('fast');
		            $('#trCom3Plan2').fadeIn('fast');
		            $('#trCom3Plan3').fadeIn('fast');
		            document.getElementById('lblPlan1').innerHTML = arrPlan[0];
		            document.getElementById('lblPlan2').innerHTML = arrPlan[1];
		            document.getElementById('lblPlan3').innerHTML = arrPlan[2];
		            document.getElementById('lblCom2Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom2Plan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom2Plan3').innerHTML = arrPlan[2];
		            document.getElementById('lblCom3Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom3Plan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom3Plan3').innerHTML = arrPlan[2];
			        }
		        else if (cantPlanes == 4) {
		            $('#trPlan1').fadeIn('fast');
		            $('#trPlan2').fadeIn('fast');
		            $('#trPlan3').fadeIn('fast');
		            $('#trPlan4').fadeIn('fast');
		            $('#trCom2Plan1').fadeIn('fast');
		            $('#trCom2Plan2').fadeIn('fast');
		            $('#trCom2Plan3').fadeIn('fast');
		            $('#trCom2Plan4').fadeIn('fast');
		            $('#trCom3Plan1').fadeIn('fast');
		            $('#trCom3Plan2').fadeIn('fast');
		            $('#trCom3Plan3').fadeIn('fast');
		            $('#trCom3Plan4').fadeIn('fast');
		            document.getElementById('lblPlan1').innerHTML = arrPlan[0];
		            document.getElementById('lblPlan2').innerHTML = arrPlan[1];
		            document.getElementById('lblPlan3').innerHTML = arrPlan[2];
		            document.getElementById('lblPlan4').innerHTML = arrPlan[3];
		            document.getElementById('lblCom2Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom2Plan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom2Plan3').innerHTML = arrPlan[2];
		            document.getElementById('lblCom2Plan4').innerHTML = arrPlan[3];
		            document.getElementById('lblCom3Plan1').innerHTML = arrPlan[0];
		            document.getElementById('lblCom3Plan2').innerHTML = arrPlan[1];
		            document.getElementById('lblCom3Plan3').innerHTML = arrPlan[2];
		            document.getElementById('lblCom3Plan4').innerHTML = arrPlan[3];
		        }

		        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
		        if (CodModalidad == '1') {//chip
		            document.getElementById("TDMontoRa").innerHTML = "Renta Adelantada";
		            document.getElementById("TDPagoInicial").innerHTML = "Pago Inicial";
		            document.getElementById("TDMontoRa1").innerHTML = "Renta Adelantada";
		            document.getElementById("TDPagoInicial1").innerHTML = "Pago Inicial";
		            document.getElementById("TDMontoRa2").innerHTML = "Renta Adelantada";
		            document.getElementById("TDPagoInicial2").innerHTML = "Pago Inicial";
		        } else if (CodModalidad == '2') {//Contrato
		            document.getElementById("TDMontoRa").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa1").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial1").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa2").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial2").innerHTML = "Pago Inicial (A+B)";
		        } else if (CodModalidad == '3') {//cuotas
		            document.getElementById("TDMontoRa").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa1").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial1").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa2").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial2").innerHTML = "Pago Inicial (A+B)";
		        } else if (CodModalidad == '4') {//Contrato SIN CODE
		            document.getElementById("TDMontoRa").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa1").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial1").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa2").innerHTML = "Renta Adelantada (A)";
		            document.getElementById("TDPagoInicial2").innerHTML = "Pago Inicial (A+B)";
		        } else if (CodModalidad == '5') {//cuotas SIN CODE
		            document.getElementById("TDMontoRa").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa1").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial1").innerHTML = "Pago Inicial (A+B)";
		            document.getElementById("TDMontoRa2").innerHTML = "Renta Adelantada (B)";
		            document.getElementById("TDPagoInicial2").innerHTML = "Pago Inicial (A+B)";
		        }
		        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

		        if (CodModalidad == '1') {//chip
		            $('#btnVerOpciones').fadeOut('fast');
		            $('#trCuotas').fadeOut('fast');

		        } else if (CodModalidad == '2') {//Contrato
		            $('#btnVerOpciones').fadeIn('fast');
		            $('#trCuotas').fadeOut('fast');

		        } else if (CodModalidad == '3') {//cuotas
		            $('#btnVerOpciones').fadeIn('fast');
		            $('#trCuotas').fadeIn('fast');
		            $('#trCuotas2').fadeIn('fast');
		            $('#trCuotas3').fadeIn('fast');
		            $('#lblNroCuotas').text($('#hidTablePlan').val().split('&')[1]);
		            $('#lblNroCuotas2').text($('#hidTablePlan').val().split('&')[1]);
		            $('#lblNroCuotas3').text($('#hidTablePlan').val().split('&')[1]);
		        }
                        //INICIATIVA 920
                        else if (CodModalidad == '4') {//Contrato Sin code
		            $('#btnVerOpciones').fadeIn('fast');
		            $('#trCuotas').fadeOut('fast');

		        } else if (CodModalidad == '5') {//cuotas sin code
		            $('#btnVerOpciones').fadeIn('fast');
		            $('#trCuotas').fadeIn('fast');
		            $('#trCuotas2').fadeIn('fast');
		            $('#trCuotas3').fadeIn('fast');
		            $('#lblNroCuotas').text($('#hidTablePlan').val().split('&')[1]);
		            $('#lblNroCuotas2').text($('#hidTablePlan').val().split('&')[1]);
		            $('#lblNroCuotas3').text($('#hidTablePlan').val().split('&')[1]);
		        }

		        g_CodOficinaActual = $('#hidTablePlan').val().split('&')[2];
		        FConsultaPlanes(CodModalidad, $('#hidTablePlan').val().split('&')[0], 'tblPlanesProAct');
		    }

		    //Consultar planes, ingresar: Cod.Modalidad, Data formato cadena | y ; y la tabla
		    function FConsultaPlanes(CodModalidad, strData, destable) {
		        if (strData == null || strData == '' || strData == "VALIDACION") {

		            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI

                //EMMH I
		            MsgEquipoNoPlanesProac = getValue('hidMsgEquipoNoPlanesProac');
		            alert(MsgEquipoNoPlanesProac); 
		            //alert('<%= ConfigurationManager.AppSettings["constMsjEquipoNoPlanesProac"] %>'); 
		            return;
		        //EMMH F
		        }
		        var table = strData;
		        var filas = table.split('|');
		        var strbody = '';

		        $('#' + destable).find('tbody').remove();

                //INICIATIVA 920
		        if (CodModalidad == '3' || CodModalidad == '5') {
		            $('#' + destable + ' thead tr:eq(1) td:eq(2)').fadeIn('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(6)').fadeIn('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(7)').fadeIn('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(8)').fadeIn('fast');

		        } else {
		            $('#' + destable + ' thead tr:eq(1) td:eq(2)').fadeOut('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(6)').fadeOut('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(7)').fadeOut('fast');
		            $('#' + destable + ' thead tr:eq(1) td:eq(8)').fadeOut('fast');
		        }
		        if (CodModalidad == '2' || CodModalidad == '4') {
		            $('#' + destable + ' thead tr:eq(1) td:eq(4)').fadeIn('fast');
		        }
		        else {
		            $('#' + destable + ' thead tr:eq(1) td:eq(4)').fadeOut('fast');
		        }

		        var strDisableRadio = 'disabled="disabled"';
		        var flagDisable = 'S';
		        var strBuscaDisable = '';
		        var countDis = 1;
		        //var MontoMax = window.opener.document.getElementById("hidDatosBRMS").value;

		        if (!isNaN(g_TopeMaxCuotas) && g_TopeMaxCuotas != "") {
		            var MontoMax = parseFloat(g_TopeMaxCuotas);
		        }
		        else {
		            var MontoMax = 0;
		        }
		    

		        for (var row = 0; row < filas.length; row++) {
		            flagDisable = 'S';
		            strBuscaDisable = "|" + countDis.toString() + "|";
                                                                              
		            strbody += '<tr>';
		            var columnas = filas[row].split(';');
		            var Autonomia = columnas[21];
		            var NoAplicaEval= 0;
		            strbody += '<td><input type="radio" class="rdbPlan" name="plan" value="' + columnas[1] + '" ' + strBuscaDisable + ' ></td>'; //item.descripcion
		            strbody += '<td style="display:none;">' + columnas[0] + '</td>'; //item.codigo
		            strbody += '<td class="TablaFilas" align="center">' + columnas[1] + '</td>'; //item.descripcion

		            //NRO: CUOTA::INI
		            if (CodModalidad == '3' || CodModalidad == '5') {//INICIATIVA 920

		                if (MontoMax > 0 && MontoMax <= parseFloat(columnas[3])) {
		                    //EMMH I
		                    //strbody += '<td class="Arial10BRed" align="center">' + 'No Califica' + '</td>'; //item.NroCuota
		                    strbody += '<td class="Arial10BRed" align="center" style="display:none;">' + 'No Aplica' + '</td>'; //item.NroCuota Modificado por EMMH F
		                    strbody += '<td class="TablaFilas" align="center">' + '   -   ' + '</td>'; //item.porcenCuotaInicial
		                    flagDisable = 'S';
		                }

		                else {

		                    if (columnas[4] == '0') {
		                        strbody += '<td class="Arial10BRed" align="center" style="display:none;">' + constTextoNoAplicaCondiciones + '</td>'; //item.NroCuota
		                        strbody += '<td class="TablaFilas" align="center">' + '   -   ' + '</td>'; //item.porcenCuotaInicial
		                        flagDisable = 'S';
		                        NoAplicaEval++;
		                    }
		                    else {
		                        strbody += '<td class="TablaFilas" align="center" style="display:none;">' + columnas[4] + '</td>'; //item.NroCuota
		                        strbody += '<td class="TablaFilas" align="center" >' + parseFloat(columnas[5]).toFixed(2) + '</td>'; //item.porcenCuotaInicial
		                        flagDisable = 'N';
		                    }
		                }
		            }
		            else {
		                strbody += '<td class="TablaFilas" style="display:none;">' + 0 + '</td>';
		                strbody += '<td class="TablaFilas" style="display:none;">' + 0.00 + '</td>';
		            }
		            //NRO: CUOTA::FIN
                    

                    //MONTO RA:INI --INICIATIVA 920
		            if ((g_strCodModalidad == '3' || g_strCodModalidad == '5') && MontoMax > 0 && MontoMax <= parseFloat(columnas[3])) {

		               //comentado por EMMH // strbody += '<td class="Arial10BRed" align="center">' + 'No Califica' + '</td>'; //item.MontoRA
		                    flagDisable = 'S';  
		            }
		            else {

		                    if (columnas[19] == 'NO') {//restriccion
		                        if (Autonomia == 'S') {
		                            strbody += '<td class="TablaFilas" align="center">' + parseFloat(columnas[2]).toFixed(2) + '</td>'; //item.MontoRA
		                            flagDisable = 'N';
		                        }
		                        else {
		                            strbody += '<td class="TablaFilas" align="center">' + constTextoNoAprobadoAutonomia + '</td>'; //'REQUIERE EVALUACION CREDITICIA'
		                            flagDisable = 'N';
		                            NoAplicaEval++;
		                        }
		                    }
		                    else {
		                        strbody += '<td class="Arial10BRed" align="center">' + constTextoNoAplicaCondiciones + '</td>'; //item.MontoRA
		                        flagDisable = 'S';
		                        NoAplicaEval++;
		                    }
		            }
		            //MONTO RA:INI

		           
                   //PRECIO DE VENTA:: INI -- INICIATIVA 920
		            if (CodModalidad == '2' || CodModalidad=='4') {
		                strbody += '<td class="TablaFilas" align="center">' + parseFloat(columnas[3]).toFixed(2) + '</td>'; //item.PrecionVenta
		            }
		            else{
		                strbody += '<td style="display:none;">' + parseFloat(columnas[3]).toFixed(2) + '</td>';
		            }
                    //PRECIO DE VENTA::FIN

		            //TOTAL PAGAR = CUOTA INICIAL + RA::INI
		            if (NoAplicaEval == '0' && flagDisable == 'N') {
		                var cuotaInicial = parseFloat(columnas[5]).toFixed(2);
		                var rentaAdelantada = parseFloat(columnas[2]).toFixed(2); //chip
		                var pagoInicial = parseFloat(+cuotaInicial + +rentaAdelantada).toFixed(2);
		                var precioEquipo = parseFloat(columnas[3]).toFixed(2);
		                var pagoContratoCode = parseFloat(+rentaAdelantada + +precioEquipo).toFixed(2);

		                if (CodModalidad == '3' || CodModalidad == '5') {
		                    strbody += '<td class="TablaFilas" align="center">' + pagoInicial + '</td>'; // item.TotalPagar //PROY 30748 F2 MDE

		                } else if (CodModalidad == '1') {
		                    strbody += '<td class="TablaFilas" align="center">' + rentaAdelantada + '</td>'; // item.TotalPagar
		                } else if (CodModalidad == '2' || CodModalidad == '4') {
		                    strbody += '<td class="TablaFilas" align="center">' + pagoContratoCode + '</td>'; // item.TotalPagar
		                        }
		                        else {
		                    strbody += '<td class="TablaFilas" align="center">' + '   -   ' + '</td>'; // item.TotalPagar
		                        }
		            } else {
		                strbody += '<td class="TablaFilas" align="center">' + '   -   ' + '</td>'; // item.TotalPagar
		                }
                    //TOTAL PAGAR = CUOTA INICIAL + RA::INI

                    //CUOTA MENSUAL EQUIPO::INI
                    //Cuota Mensual/Cargo Fijo/
                    if (NoAplicaEval == '0' && flagDisable == 'N') {
                        var precioEquipo = parseFloat(columnas[3]).toFixed(2);
                        var cantCuotas = parseFloat(columnas[4]).toFixed(2);
                        var cuotaInicial = parseFloat(columnas[5]).toFixed(2);
                        var cuotaMensual = parseFloat((precioEquipo - cuotaInicial) / cantCuotas).toFixed(2);
                        var rentaAdelantada = parseFloat(columnas[2]).toFixed(2);
                        var cargoMensual = parseFloat(columnas[11]).toFixed(2);
                        var cargoFijo = parseFloat(cargoMensual - cuotaMensual).toFixed(2);
                        if (CodModalidad == '3' || CodModalidad == '5') {//INICIATIVA 920
                            strbody += '<td class="TablaFilas" align="center" >' + cuotaMensual + '</td>'; //CuotaMensual
                            strbody += '<td class="TablaFilas" align="center">' + cargoFijo + '</td>'; //item.cargoFijo
                            strbody += '<td class="TablaFilas" align="center">' + cargoMensual + '</td>'; //PagoTotal
                        } else {
                            strbody += '<td class="TablaFilas" align="center" style="display:none;" >' + 0.00 + '</td>'; //CuotaMensual
                            strbody += '<td class="TablaFilas" align="center" style="display:none;">' + cargoMensual + '</td>'; //item.cargoFijo
                            strbody += '<td class="TablaFilas" align="center" style="display:none;">' + cargoMensual + '</td>'; //PagoTotal
		            } 
                    } else {
                        var cargoMensual = parseFloat(columnas[11]).toFixed(2);
                        strbody += '<td class="TablaFilas" align="center" style="display:none;">' + 0.00 + '</td>';
                        strbody += '<td class="TablaFilas" align="center" style="display:none;">' + cargoMensual + '</td>'; //item.cargoFijo
                        strbody += '<td class="TablaFilas" align="center" style="display:none;">' + cargoMensual + '</td>'; //PagoTotal
		            }
                    //CUOTA MENSUAL EQUIPO::FIN

                    //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

		            strbody += '<td style="display:none;">' + columnas[7] + '</td>'; // item.cantidad
		            strbody += '<td style="display:none;">' + columnas[8] + '</td>'; // item.cantidadDeLineasAdicionalesRUC
		            strbody += '<td style="display:none;">' + columnas[9] + '</td>'; //item.cantidadDeLineasMaximas
		            strbody += '<td style="display:none;">' + columnas[10] + '</td>'; //item.capacidadDePago
		            strbody += '<td style="display:none;">' + columnas[11] + '</td>'; //item.cargoFijo
		            strbody += '<td style="display:none;">' + columnas[12] + '</td>'; // item.Cuota.cuota
		            strbody += '<td style="display:none;">' + columnas[13] + '</td>'; // item.factorDeEndeudamientoCliente
		            strbody += '<td style="display:none;">' + columnas[14] + '</td>'; // item.factorDeRenovacionCliente
		            strbody += '<td style="display:none;">' + columnas[15] + '</td>'; // item.montoCFParaRUC
		            strbody += '<td style="display:none;">' + columnas[16] + '</td>'; // item.montoDeGarantia
		            strbody += '<td style="display:none;">' + columnas[17] + '</td>'; // item.precioDeVentaTotalEquipos
		            strbody += '<td style="display:none;">' + columnas[18] + '</td>'; // item.procesoIDValidator
		            strbody += '<td style="display:none;">' + columnas[19] + '</td>'; // item.restriccion
		            strbody += '<td style="display:none;">' + columnas[20] + '</td>'; // item.riesgoTotalEquipo
		            strbody += '<td style="display:none;">' + columnas[21] + '</td>'; // item.TieneAutonomia
		            strbody += '<td style="display:none;">' + columnas[22] + '</td>'; // item.TipoDeAutonomiaCargoFijo
		            strbody += '<td style="display:none;">' + columnas[23] + '</td>'; // item.tipodecobro
		            strbody += '<td style="display:none;">' + columnas[24] + '</td>'; // item.TotalPagar
		            strbody += '<td style="display:none;">' + columnas[26] + '</td>'; // item.ServiciosAdicionalesCompatibles
		            strbody += '<td style="display:none;">' + columnas[25] + '</td>'; // item.CostoEquipo
		            strbody += '<td style="display:none;">' + columnas[27] + '</td>'; // item.TopeMonto
		            strbody += '<td style="display:none;">' + columnas[2] + '</td>'; // item.MontoRa
		            strbody += '<td style="display:none;">' + columnas[28] + '</td>'; // item.idListaPrecio
		            strbody += '<td style="display:none;">' + columnas[29] + '</td>'; // item.DesListaPrecio
		            strbody += '<td style="display:none;">' + columnas[30] + '</td>'; // item.CodListaPrecio //PROY 30748 F2 MDE
                            strbody += '<td style="display:none;">' + columnas[31] + '</td>'; // item.PorIniOsb //PROY 30748 F2 MDE
		            strbody += '<td style="display:none;">' + columnas[32] + '</td>'; // item.ejecucionConsultaPrevia //PROY 140335 RF1
		            strbody += '</tr>';

		            if (flagDisable == 'S') {
		                strbody = strbody.toString().replace(strBuscaDisable.toString(), strDisableRadio.toString());
		            }
		            else {
		                strbody = strbody.toString().replace(strBuscaDisable.toString(), '');
		            }

		            countDis = countDis + 1;
		        }

		        $('#' + destable).append('<tbody>' + strbody + '</tbody>');

		    }

		    function ExtraeURL(document, param) {
		        url = document;
		        url = String(url.match(/\?+.+/));
		        url = url.replace("?", "");
		        url = url.split("&");
		        x = 0;
		        while (x < url.length) {
		            p = url[x].split("=");
		            if (p[0] == param) {
		                return decodeURIComponent(p[1]);
		            }
		            x++;
		        }
		    }

		    //Seleccionar fila
		    function mostrarListaSel(idFila) {
		        var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
		        mostrarLista('divListaEquipo', idFila);
		        txtTextoEquipo.select();
		    }

		    function buscarCoincidente(idFila) {
		        var valorIngresado = window.event.keyCode;
		        if (valorIngresado == 46)
		            return;

		        var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
		        var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
		        var strTexto = txtTextoEquipo.value.toUpperCase();
		        var strTextoCoincid;
		        var strTextoEnc = '';
		        var intLongTexto = strTexto.length;
		        var strEquipos = getValue('hidMaterial' + idFila);
		        var strCoincid = '';
		        var arrEquipos = strEquipos.split('|');
		        var arrEquipo;
		        var strEquiposC = '';
		        var divListaEquipo = document.getElementById('divListaEquipo' + idFila);
		        var intIndEncontro = -1;

		        if (intLongTexto > 0) {
		            for (i = 0; i < arrEquipos.length; i++) {
		                arrEquipo = arrEquipos[i].split(';');

		                if (arrEquipo[0].length > 0) {
		                    strTextoCoincid = arrEquipo[1];

		                    if (strTextoCoincid.length >= intLongTexto) {
		                        strTextoCoincid = strTextoCoincid.toUpperCase();
		                        intIndEncontro = strTextoCoincid.indexOf(strTexto);

		                        if (intIndEncontro > -1)
		                            strEquiposC += '|' + arrEquipo[0] + ';' + arrEquipo[1];
		                    }
		                }
		            }
		        }

		        if (strEquiposC.length == 0)
		            strEquiposC = strEquipos;

		        llenarDatosCombo1(divListaEquipo, strEquiposC, idFila, 'Equipo', false);
		        divListaEquipo.style.display = 'inline';

		        if (valorIngresado == 40) {
		            var arrDatos = strEquiposC.split('|');
		            if (arrDatos.length > 1) {
		                var arrItem = arrDatos[1].split(';');
		                var divOpcion = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + 1);
		                divOpcion.focus();
		                setValue('hidEquiposSel' + idFila, strEquiposC);
		            }
		        }
		        else {
		            hidValorEquipo.value = '';
		        }
		    }

		    function ocultarListaTxt(idFila) {
		        ocultarLista('txt', idFila);
		    }

		    function ocultarLista(control, idFila) {
		        var idElementoActivo = document.activeElement.id;

		        if (control != 'txt')
		            estiloNoSel(control);

		        if (idElementoActivo.indexOf('txtTextoEquipo' + idFila) < 0 &&
					idElementoActivo.indexOf('divOption' + idFila + '_') < 0 &&
					idElementoActivo.indexOf('divListaEquipo' + idFila) < 0) {
		            document.getElementById('divListaEquipo' + idFila).style.display = 'none';
		        }
		    }

		    function llenarDatosCombo1(div, strDatos, idFila, prefijo, booSeleccione) {
		        var arrDatos;
		        var arrItem;
		        var strDato;
		        var prefijo1 = prefijo + idFila;
		        var strAnterior = '';
		        var strSiguiente = '';
		        if (div != undefined) {
		            div.innerHTML = "";
		        }

		        if (booSeleccione) {
		            if (div != undefined) {
		                divOpcion = document.createElement('span');
		                divOpcion.style.width = '100%';
		                divOpcion.id = '';
		                divOpcion.innerHTML = 'SELECCIONE...';
		                document.getElementById('hidValor' + prefijo1).value = '';
		                document.getElementById('txtTexto' + prefijo1).value = 'SELECCIONE...';
		                divOpcion.className = "AutoComplete_Item";
		                divOpcion.onmouseover = function () { estiloSel(this); };
		                divOpcion.onmouseout = function () { estiloNoSel(this); };
		                divOpcion.onclick = function () { seleccionarItem('hidValor', 'txtTexto', 'divLista', this, idFila, prefijo); };
		                divOpcion.onfocus = function () { estiloSel(this); };
		                divOpcion.onblur = function () { ocultarLista(this, idFila); };
		                div.appendChild(divOpcion);
		            }
		        }
		        if (strDatos != null)
		            var arrDatos = strDatos.split("|");
		        else
		            return;

		        for (i = 0; i < arrDatos.length; i++) {
		            if (div != undefined) {

		                divOpcion = document.createElement('span');
		                divOpcion.style.width = '90%';
		                divOpcion.style.display = 'inline-block';
		                divOpcion.className = "AutoComplete_Item";
		                divOpcion.onmouseover = function () { estiloSel(this); };
		                divOpcion.onmouseout = function () { estiloNoSel(this); };
		                divOpcion.onclick = function () { seleccionarItem('hidValor', 'txtTexto', 'divLista', this, idFila, prefijo); };
		                divOpcion.onfocus = function () { estiloSel(this); };
		                divOpcion.onblur = function () { ocultarLista(this, idFila); };

		                if (arrDatos[i].length > 0) {
		                    strDato = arrDatos[i];
		                    arrItem = strDato.split(";");

		                    if (strDato != 'null') {
		                        if (arrItem.length > 1) {
		                            if (arrItem[0].indexOf('^') > -1) {
		                                divOpcion.style.color = document.getElementById('HidColorBloqueo').value;
		                            }
		                            divOpcion.id = 'divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + i;
		                            divOpcion.innerHTML = arrItem[1] + '<br />';

		                            divOpcion.onkeydown = function () { return cambiarFocoSpan(this.id, idFila); };
		                        }
		                        else {
		                            option.value = 'divOption_' + arrDatos[i] + '_' + arrDatos[i];
		                            divOpcion.innerHTML = arrItem[1] + '<br />';
		                        }


		                        div.appendChild(divOpcion);
		                    }
		                }
		            }
		        }
		    }

		    function cambiarFocoSpan(strId, idFila) {
		        var idSel = parseInt(strId.split('_')[3]);
		        var idAnterior = idSel - 1;
		        var idSiguiente = idSel + 1;
		        var strEquipos = getValue('hidEquiposSel' + idFila);
		        var arrDatos = strEquipos.split('|');
		        var strDato;
		        var arrItem;
		        var divAnterior = 0;
		        var divSiguiente = 1;

		        if (arrDatos[idAnterior].length > 0) {
		            strDato = arrDatos[idAnterior];
		            arrItem = strDato.split(";");

		            if (strDato != 'null') {
		                if (arrItem.length > 1)
		                    divAnterior = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + idAnterior);
		            }
		        }

		        if (idSiguiente == arrDatos.length)
		            idSiguiente = 0;

		        if (arrDatos[idSiguiente].length > 0) {
		            strDato = arrDatos[idSiguiente];
		            arrItem = strDato.split(";");

		            if (strDato != 'null') {
		                if (arrItem.length > 1)
		                    divSiguiente = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + idSiguiente);
		            }
		        }

		        if (window.event.keyCode == 40) {
		            if (divSiguiente != 1)
		                divSiguiente.focus();
		        }
		        else {
		            if (window.event.keyCode == 38) {
		                if (divAnterior != 0)
		                    divAnterior.focus();
		                else {
		                    var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
		                    txtTextoEquipo.select();
		                    txtTextoEquipo.focus();
		                    return false;
		                }

		            }
                    else {
		                if (window.event.keyCode == 13) {
		                    seleccionarItem('hidValor', 'txtTexto', 'divLista', document.getElementById(strId), idFila, 'Equipo');
		                    return false;
		                }
		            }
		        }
		        return true;
		    }

		    function seleccionarItem(txtValor, txtTexto, divLista, div, idFila, prefijo) {
		            var arrValores = div.id.split('_');
		            var valor = '';
		            if (arrValores.length > 1)
		                valor = arrValores[1];
		            txtValor += prefijo + idFila;
		            txtTexto += prefijo + idFila;
		            divLista += prefijo + idFila;
		            if (valor.indexOf('^') > -1) {
		                valor = valor.replace('^', '');
		                document.getElementById(txtTexto).style.color = BloqueoEquipoSinStockColor ;
		                alert(constMsjEquipoSinStockSel);
		            }
		            else
		                document.getElementById(txtTexto).style.color = '';

		            document.getElementById(txtValor).value = valor;
		            document.getElementById(txtTexto).value = arrValores[2];
		            document.getElementById(divLista).style.display = 'none';


		        if (prefijo == 'Equipo')
		            cambiarEquipo1(idFila);
		    
		    }

		    function cambiarEquipo1(idFila) {
		        var strEquipo = document.getElementById('hidValorEquipo' + idFila).value;
		        var codigoTipoProductoActual = g_CodTipProdActual; 
                //INICIATIVA 920
		        if (g_strCodModalidad == g_constModalidadPagoEnCuota || g_strCodModalidad == g_constModalidadPagoEnCuotaSinCode) {
		            inicializarPrecioEquipo(idFila);
		        }

		        var strEquipoDesc = document.getElementById('txtTextoEquipo' + idFila);
		        var strPlan = g_CodPlan; 
		        var strPlazo = g_CodPlazo; 
		        var strCampana = g_CodCampania;

		        if (strPlan == "" || strPlazo.length == 0 || strCampana.length == 0) {
		            alert('No se pudo cargar los datos del equipo, vuelva ingresar.');
		            return;
		        }

		        LlenarEquipoPrecioIfr(idFila, strPlan, strPlazo, strCampana, strEquipo);
		    }

		    function LlenarEquipoPrecioIfr(idFila, strPlanSap, strPlazo, strCampana, strEquipo) {
		        var strCuota = '';
		        //INICIATIVA 920
		        if (g_strCodModalidad == g_constModalidadPagoEnCuota || g_strCodModalidad == g_constModalidadPagoEnCuotaSinCode) {

		            strCuota = $('#lblNroCuotas').text(); ;
		        }
		        var url = '../frames/sisact_ifr_consulta_unificada.aspx?';//PROY 30748 F2 FALLA MDE
		        url += 'idFila=' + idFila;
		        url += '&strOficina=' + g_CodOficina;
		        url += '&strOferta=' + g_CodOferta;
		        url += '&strPlazo=' + strPlazo;
		        url += '&strPlanSap=' + strPlanSap;
		        url += '&strCampana=' + strCampana;
		        url += '&strMaterial=' + strEquipo;
		        url += '&strCanal=' + g_CodCanalSap;
		        url += '&strOrgVenta=' + g_CodOrgVenta;
		        url += '&strTipoDocVenta=' + g_CodTipoDocVentaSap;
		        url += '&strTipoModalidadVenta=' + g_strCodModalidad;
		        url += '&strTipoOficina=' + g_CodOficinaActual;
		        url += '&strCuota=' + strCuota;
		        url += '&strMetodo=' + "LlenarEquipoPrecio";

		        self.frames['iframeAuxiliar'].location.replace(url);
		    }


		    function asignarPrecio(idFila, strValor) {//PROY 30748 F2 FALLA MDE
		        var cfPlanServicio = g_CodCFPlanServicio;

		        if (strValor == 0) {
		            inicializarEquipo(idFila);
		        }
		        else {
		            var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
		            var txtEquipoPrecio = document.getElementById('hidEquipoPrecio' + idFila);//PROY 30748 F2 FALLA MDE

		            if (hidListaPrecio != null) {
		                hidListaPrecio.value = strValor;
		                txtEquipoPrecio.value = strValor.split('_')[0];
		            }
		            else {
		                if (strValor.indexOf('_') > -1) {
		                    var arrValor = strValor.split('_');
		                    var cfMenAlqKit = arrValor[1];
		                }
		            }
		        }
		    }

		    function mostrarLista(strDivLista, idFila) {
		        var divLista = document.getElementById(strDivLista + idFila);
		        if (divLista.style.display == 'none') {
		            divLista.style.display = 'inline';
		        }
		        else {
		            divLista.style.display = 'none';
		        }
		    }

		    function estiloSel(div) {
		        div.className = "AutoComplete_Highlight";
		    }
		    function estiloNoSel(div) {
		        div.className = "AutoComplete_Item";
		    }
		    function imgSel(img) {
		        img.src = '../../Imagenes/cmb.gif';
		    }
		    function imgNoSel(img) {
		        img.src = '../../Imagenes/cmb.gif';
		    }
		    function FbtnEvaluar2() {
		        habilitarBoton('btnEvaluar2', 'Cargando...', false);
		        cargarImagenEsperando(
					function () { $('#DCargando').fadeIn('fast'); },

					function () {
					    var hidcodigo1 = $('#hidValorEquipo1').val();
					    var hidcodigo2 = $('#hidValorEquipo2').val();
					    var hidDescripcion1 = $('#txtTextoEquipo1').val();
					    var equipo = $('#lblEquipo').text();
					    var equipo1 = $('#lblEquipo1').text();
					    var equipo2 = $('#lblEquipo2').text();
					    var hidPrecio1 = $('#hidListaPrecio1').val(); //PROY 30748 F2 FALLA MDE
					    var strNewEquipo = '';

					    if (hidcodigo1 == '') {
					        $('#lblEquipo1').text('');
					        $('#DCargando').fadeOut('fast');
					        alert('Seleccione equipo');
					        $('#txtTextoEquipo1').focus();

					    } 
                        else {
                            if (hidDescripcion1 == equipo || hidDescripcion1 == equipo2 ) {
                                $('#DCargando').fadeOut('fast');
					            alert('Seleccione otro equipo, no debe ser el mismo');
					            $('#txtTextoEquipo1').focus();

                                if(hidDescripcion1 != equipo1)
					                 $('#tblPlanesProActEq1').find('tbody').remove();
					        }
                            else {
                                $('#tblPlanesProActEq1').find('tbody').remove();
					            $('#lblEquipo1').text(hidDescripcion1);
					            strNewEquipo += g_idFilaProa + ";"; 				//[0] - idFila
					            strNewEquipo += '' + ";"; 				//[1] - idProducto
					            strNewEquipo += hidcodigo1 + ";"; 	    //[2] - idEquipo
					            strNewEquipo += hidDescripcion1 + ";";   //[3] - Equipo
					            strNewEquipo += hidPrecio1.split('_')[3] + ";"; //[4] - Costo PROY 30748 F2 FALLA MDE
					            strNewEquipo += hidPrecio1.split('_')[0] + ";"; //[5] - Precio Venta PROY 30748 F2 FALLA MDE
					            strNewEquipo += g_nrcuotas + ";"; 			    //[6] - nro Cuota
					            strNewEquipo += g_porcIni + ";"; 			    //[7] - porcentaje Cuota
					            strNewEquipo += g_cuotaini + "|"; 			    //[8] - Monto Cuota

					    
					            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
					            url += 'strcodequipo=' + hidcodigo1;
					            url += '&strdesquipo=' + hidDescripcion1;
					            url += '&strTipoModalidadVenta=' + g_strCodModalidad;
					            url += '&strtabla=' + 'tblPlanesProActEq1';
					            url += '&strMetodo=' + 'LlenarPlanProactivo';
					            url += '&strNroDocumento=' + g_strNroDocumento;
					            url += '&strNewEquipo=' + strNewEquipo;
					            url += '&strTipOperacion=' + g_strIdtipoOperacion;
					            url += '&familia=' + g_stfamilia;
					            url += '&strCFServAdic=' + g_strCFServAdic;
					            url += '&strServAdicional=' + g_strServAdic;//cristhian
					            url += '&intFlagEquipo=' + 1;
					            url += '&strFlujo=' + g_idFlujo;
					            url += '&strComparar=' + "1";

					            self.frames['iframeAuxiliar'].location.replace(url);
					        }
					    }
					},

					//function () { $('#DCargando').fadeOut('fast'); });
                    function () {  });
		        habilitarBoton('btnEvaluar2', 'Comparar', true);		
		    }

		    function FbtnEvaluar3() {
		            habilitarBoton('btnEvaluar3', 'Cargando...', false);
                    cargarImagenEsperando(
					function () { $('#DCargando').fadeIn('fast'); },
					function () {

					    var hidcodigo1 = $('#hidValorEquipo1').val();
					    var hidcodigo2 = $('#hidValorEquipo2').val();
					    var hidDescripcion2 = $('#txtTextoEquipo2').val();
					    var equipo = $('#lblEquipo').text();
					    var equipo1 = $('#lblEquipo1').text();
					    var equipo2 = $('#lblEquipo2').text();
					    var hidPrecio2 = $('#hidListaPrecio2').val();//PROY 30748 F2 FALLA MDE
					    var strNewEquipo = '';


					    if (hidcodigo2 == '') {
					        $('#DCargando').fadeOut('fast');
					        $('#lblEquipo2').text('');
					        alert('Seleccione equipo');
					        $('#txtTextoEquipo2').focus();

					    } else {
					        if (hidDescripcion2 == equipo || hidDescripcion2 == equipo1 ) {
					            $('#DCargando').fadeOut('fast');
					            alert('Seleccione otro equipo, no debe ser el mismo');
					            $('#txtTextoEquipo2').focus();

					            if(hidDescripcion2 != equipo2)
					                $('#tblPlanesProActEq2').find('tbody').remove();
					        }
					        else {
					            $('#tblPlanesProActEq2').find('tbody').remove();
					            $('#lblEquipo2').text(hidDescripcion2);
					            strNewEquipo += g_idFilaProa + ";"; 				//[0] - idFila
					            strNewEquipo += '' + ";"; 				//[1] - idProducto
					            strNewEquipo += hidcodigo2 + ";"; 	    //[2] - idEquipo
					            strNewEquipo += hidDescripcion2 + ";";  //[3] - Equipo
					            strNewEquipo += hidPrecio2.split('_')[3] + ";"; //[4] - Costo PROY 30748 F2 FALLA MDE
					            strNewEquipo += hidPrecio2.split('_')[0] + ";"; //[5] - Precio Venta PROY 30748 F2 FALLA MDE
					            strNewEquipo += g_nrcuotas + ";"; 			    //[6] - nro Cuota
					            strNewEquipo += g_porcIni + ";"; 			    //[7] - porcentaje Cuota
					            strNewEquipo += g_cuotaini + "|"; 			    //[8] - Monto Cuota

					            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
					            url += 'strcodequipo=' + hidcodigo2;
					            url += '&strdesquipo=' + hidDescripcion2;
					            url += '&strTipoModalidadVenta=' + g_strCodModalidad;
					            url += '&strtabla=' + 'tblPlanesProActEq2';
					            url += '&strMetodo=' + 'LlenarPlanProactivo';
					            url += '&strNroDocumento=' + g_strNroDocumento;
					            url += '&strNewEquipo=' + strNewEquipo;
					            url += '&strTipOperacion=' + g_strIdtipoOperacion;
					            url += '&familia=' +g_stfamilia; 
					            url += '&strCFServAdic=' + g_strCFServAdic;
					            url += '&strServAdicional=' + g_strServAdic;//cristhian
					            url += '&intFlagEquipo=' + 1;
					            url += '&strFlujo=' + g_idFlujo;
					            url += '&strComparar=' + "2";
					            self.frames['iframeAuxiliar'].location.replace(url);
					        }
					    }
					},
					//function () { $('#DCargando').fadeOut('fast'); });
                    function () {  });
					habilitarBoton('btnEvaluar3', 'Comparar', true);
			}
		        function cargarImagenEsperando(callfuncion1,callfuncion2,callfuncion3)
			{			
				callfuncion1();
				callfuncion2();
				callfuncion3();
			}
			
			function quitarImagenEsperando()
			{
				$('#DCargando').fadeOut('fast');				
			}

			$(document).on('change', '#tblPlanesProAct tbody tr input', function (e) {
			    e.preventDefault();
	
			    var blnconfirmacion = confirm('¿Desea seleccionar plan? - ' + $(this).val());
			    if (blnconfirmacion == true) {
			        var strCodEquipo = '';
			        var strEquipo = '';
			        var strCargoFijo = '';
			        var strPrecioEquipo = '';
			        var strCodPlan = '';
			        var strPlan = '';
			        var row = $(this).parent().parent().index();
			        var strAutonomia = '';
			        var strCuota = '';
			        var strPorcenIni = ''; 
			        var strTipodecobro = ''; 
			        var strServiciosAdicionalesCompatibles = ''; 
			        var strServiciosAdicionales = '';
			        var strMontoRA = '';
			        var strCostoEquipo = ''; //cristhian
			        var strTopeMonto = ''; //EMMH
			        var strIdListaPrecio = ''; //EMMH
			        var strDesListaPrecio = ''; //EMMH
			        var strCodListaPecio = ''; //PROY 30748 F2 MDE
			        //PROY-140335 RF1 INI
			        var strEjecucionCP = '';
			        var strEjecucionCP_Des = '';
			        var strflagCPPermitida = '';
			        //PROY-140335 RF1 FIN

			        strCodPlan = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(1)').text();
			        strPlan = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(2)').text();
			        strPrecioEquipo = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(6)').text();
			        strCargoFijo = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(10)').text();
			        strAutonomia = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(25)').text();
			        strCuota = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(3)').text();
			        strPorcenIni = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(4)').text();
			        strTipodecobro = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(27)').text();
			        strServiciosAdicionalesCompatibles = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(28)').text();
			        strServiciosAdicionales = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(29)').text();
			        strMontoRA = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(32)').text();
			        strMontoGarantia = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(20)').text();
			        strCostoEquipo = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(30)').text();
			        strTopeMonto = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(31)').text();
			        strIdListaPrecio = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(33)').text();
			        strDesListaPrecio = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(34)').text();
			        strCodListaPecio = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(35)').text(); //PROY 30748 F2 MDE
			        strPorcenIniOsb = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(36)').text(); //PROY 30748 F2 EMMH
			        strEjecucionCP = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(37)').text(); //PROY-140335 RF1

			        var objSeleccion = {
			            tabla: '1',
			            codplan: strCodPlan,
			            Plan: strPlan,
			            CodEquipo: strCodEquipo,
			            Equipo: strEquipo,
			            CargoFijo: strCargoFijo,
			            PrecioEquipo: strPrecioEquipo,
			            Autonomia: strAutonomia,
                        cuota: strCuota, 
                        PorIni: strPorcenIni,
			            CodModalVenta: g_strCodModalidad,
			            Tipodecobro: strTipodecobro,
			            ServiciosAdicionalesCompatibles: strServiciosAdicionalesCompatibles,
			            ServiciosAdicionales: strServiciosAdicionales,
			            MontoRA: strMontoRA,
			            MontoGarantia: strMontoGarantia,
			            CostoEquipo: strCostoEquipo,
			            TopeMonto: strTopeMonto,
                        IdListaPrecio: strIdListaPrecio,
                        DesListaPrecio: strDesListaPrecio,
                        CodListaPrecio: strCodListaPecio, //PROY 30748 F2 MDE
                        PorcenIniOsb: strPorcenIniOsb, //PROY 30748 F2 EMMH
                        //PROY-140335 RF1 INI
                        EjecucionCP:  strEjecucionCP,
                        EjecucionCP_Des: strEjecucionCP_Des,
                        FlagCPPermitida: strflagCPPermitida
                        //PROY-140335 RF1 FIN
			        }
                    var objResponse = new Object();
			        objResponse.objSeleccion = objSeleccion;
			        window.returnValue = objResponse;
			        window.close();
			    } else {
			       //Limpiar
			    }
			});
			
			$(document).on('change','#tblPlanesProActEq1 tbody tr input',function(e){
				e.preventDefault();
				var blnconfirmacion = confirm('¿Desea seleccionar plan? - ' + $(this).val());
				if (blnconfirmacion == true) {
				    var strCodEquipo    = '';
				    var strEquipo       = '';
				    var strCargoFijo    = '';
				    var strPrecioEquipo = '';
				    var strCodPlan = '';
				    var strPlan = '';
				    var strAutonomia = '';
				    var strCuota = ''; 
				    var strPorcenIni = ''; 
				    var strTipodecobro = ''; 
				    var strServiciosAdicionalesCompatibles = ''; 
				    var strServiciosAdicionales = '';
				    var strMontoRA = '';
				    var strCostoEquipo = '';
				    var strTopeMonto = '';
				    var strIdListaPrecio = ''; //EMMH
				    var strDesListaPrecio = ''; //EMMH
			            var strCodListaPecio = ''; //PROY 30748 F2 MDE

				    var row = $(this).parent().parent().index();

				    if ($('#hidValorEquipo1').val() != "") {
				        strCodEquipo = $('#hidValorEquipo1').val();
				        strEquipo    = $('#txtTextoEquipo1').val();
				    }

				    strCodPlan = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(1)').text();
				    strPlan = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(2)').text();
				    strPrecioEquipo = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(6)').text();
				    strCargoFijo = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(10)').text();
				    strAutonomia = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(25)').text();
				    strCuota = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(3)').text(); 
				    strPorcenIni = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(4)').text();
				    strTipodecobro = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(27)').text();
				    strServiciosAdicionalesCompatibles = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(28)').text();
				    strServiciosAdicionales = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(29)').text();
				    strMontoRA = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(32)').text();
				    strMontoGarantia = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(20)').text();
				    strCostoEquipo = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(30)').text();//cristhian
				    strTopeMonto = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(31)').text();
				    strIdListaPrecio = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(33)').text();
				    strDesListaPrecio = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(34)').text();
			            strCodListaPecio = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(35)').text(); //PROY 30748 F2 MDE
			            strPorcenIniOsb = $('#tblPlanesProActEq1 tbody tr:eq(' + row + ') td:eq(36)').text(); //PROY 30748 F2 EMMH

				    var objSeleccion = {
				        tabla: '2',
				        codplan: strCodPlan,
				        Plan: strPlan,
				        CodEquipo: strCodEquipo,
				        Equipo: strEquipo,
				        CargoFijo: strCargoFijo,
				        PrecioEquipo: strPrecioEquipo,
				        Autonomia: strAutonomia,
				        cuota: strCuota,
				        PorIni: strPorcenIni, 
                        CodModalVenta: g_strCodModalidad,
				        Tipodecobro: strTipodecobro,
				        ServiciosAdicionalesCompatibles: strServiciosAdicionalesCompatibles,
				        ServiciosAdicionales: strServiciosAdicionales,
				        MontoRA: strMontoRA,
                        MontoGarantia: strMontoGarantia,
                        CostoEquipo: strCostoEquipo,
                        TopeMonto: strTopeMonto,
                        IdListaPrecio: strIdListaPrecio,
			            DesListaPrecio: strDesListaPrecio,
			            CodListaPrecio: strCodListaPecio, //PROY 30748 F2 MDE
                                    PorcenIniOsb:strPorcenIniOsb //PROY 30748 F2 EMMH
				}
				    var objResponse = new Object();
				    objResponse.objSeleccion = objSeleccion;
				    window.returnValue = objResponse;
				    window.close();
				} else {


				}
			});

			$(document).on('change', '#tblPlanesProActEq2 tbody tr input', function (e) {
			    e.preventDefault();

			    var blnconfirmacion = confirm('¿Desea seleccionar plan? - ' + $(this).val());
			    if (blnconfirmacion == true) {
			        var strCodEquipo = '';
			        var strEquipo = '';
			        var strCodPlan = '';
			        var strPlan = '';
			        var strAutonomia = '';
			        var strCargoFijo = '';
			        var strPrecioEquipo = '';
			        var row = $(this).parent().parent().index();
			        var strCuota = ''; 
			        var strPorcenIni = ''; 
			        var strTipodecobro = ''; 
			        var strServiciosAdicionalesCompatibles = '';
			        var strServiciosAdicionales = '';
			        var strMontoRA = '';
			        var strCostoEquipo = '';
			        var strTopeMonto = '';
			        var strIdListaPrecio = ''; //EMMH
			        var strDesListaPrecio = ''; //EMMH
			        var strCodListaPecio = ''; //PROY 30748 F2 MDE

			        if ($('#hidValorEquipo2').val() != "") {
			            strCodEquipo = $('#hidValorEquipo2').val();
			            strEquipo = $('#txtTextoEquipo2').val();
			        }
			        strCodPlan = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(1)').text();
			        strPlan = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(2)').text();
			        strPrecioEquipo = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(6)').text();
			        strCargoFijo = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(10)').text();
			        strAutonomia = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(25)').text();
			        strCuota = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(3)').text(); 
			        strPorcenIni = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(4)').text();
			        strTipodecobro = $('#tblPlanesProAct tbody tr:eq(' + row + ') td:eq(27)').text();
			        strServiciosAdicionalesCompatibles = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(28)').text();
			        strServiciosAdicionales = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(29)').text();
			        strMontoRA = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(32)').text();
			        strMontoGarantia = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(20)').text();
			        strCostoEquipo = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(30)').text();//cristhian
			        strTopeMonto = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(31)').text();
			        strIdListaPrecio = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(33)').text();
			        strDesListaPrecio = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(34)').text();
			        strCodListaPecio = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(35)').text(); //PROY 30748 F2 MDE
                                strPorcenIniOsb = $('#tblPlanesProActEq2 tbody tr:eq(' + row + ') td:eq(36)').text(); //PROY 30748 F2 EMMH

			        var objSeleccion = {
			            tabla: '3',
			            codplan: strCodPlan,
			            Plan: strPlan,
			            CodEquipo: strCodEquipo,
			            Equipo: strEquipo,
			            CargoFijo: strCargoFijo,
			            PrecioEquipo: strPrecioEquipo,
			            Autonomia: strAutonomia,
			            cuota: strCuota,
			            PorIni: strPorcenIni,
			            CodModalVenta:g_strCodModalidad,
			            Tipodecobro: strTipodecobro,
			            ServiciosAdicionalesCompatibles: strServiciosAdicionalesCompatibles,
			            ServiciosAdicionales: strServiciosAdicionales,
			            MontoRA: strMontoRA,
			            MontoGarantia: strMontoGarantia,
			            CostoEquipo: strCostoEquipo,
			            TopeMonto: strTopeMonto,
			            IdListaPrecio: strIdListaPrecio,
			            DesListaPrecio: strDesListaPrecio,
			            CodListaPrecio: strCodListaPecio, //PROY 30748 F2 MDE
			            PorcenIniOsb: strPorcenIniOsb //PROY 30748 F2 EMMH
			        }
			        var objResponse = new Object();
			        objResponse.objSeleccion = objSeleccion;
			        window.returnValue = objResponse;
			        window.close();
			    } else {
			        
			    }
			});
            function autoSizeIframe() {
            }

		    function inicializarEquipo(idFila) {
		        var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
		        var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
		        var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);

		        if (hidValorEquipo != null) {
		            hidValorEquipo.value = '';
		            txtTextoEquipo.value = 'SELECCIONE...';
		            txtEquipoPrecio.value = '';
		        }
		    }

		    function inicializarPrecioEquipo(idFila) {
		        var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);

		        if (txtEquipoPrecio != null) {
		            txtEquipoPrecio.value = '';
		        }
		    }			
