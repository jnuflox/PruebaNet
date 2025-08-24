
        $(document).ready(function () {
            var width = new Array();
            var table;
            var nroFila = getValue('hidContFila');

            if (getValue('hidFlgConsulta') != "S") {
                table = $("table[id*=gvResultadoCredito2]");
            } else {
                table = $("table[id*=gvResultadoCredito3]");
            }

            var alto = 0;
            if (nroFila >= 3) {
                alto = 72;
            }
            else {
                alto = 24 * nroFila;
            }

            table.find("th").each(function (i) {
                width[i] = $(this).width();
            });
            //headerRow = table.find("tr:first");
            headerRow = table.find("tr:lt(2)");
            headerRow.find("th").each(function (i) {
                $(this).width(width[i]);
            });
            var header = table.clone();
            header.empty();
            header.append(headerRow);
            header.css("width", width);
            $("#container").before(header);
            var footer = table.clone();
            footer.empty();
            footer.append(table.find("tr:last"));
            table.find("tr:first td").each(function (i) {
                $(this).width(width[i]);
            });
            footer.find("td").each(function (i) {
                $(this).width(width[i]);
            });
            $("#container").height(alto);
            $("#container").width(table.width() + 20);
            $("#container").append(table);
            $("#container").after(footer);
        });

        /*top.window.moveTo(0, 0);
        if (document.all) {
            top.window.resizeTo(screen.availWidth, screen.availHeight);
        } else if (document.layers || document.getElementById) {
            if (top.window.outerHeight < screen.availHeight || top.window.outerWidth < screen.availWidth) {
                top.window.outerHeight = screen.availHeight;
                top.window.outerWidth = screen.availWidth;
            }
        }*/

        $(document).ready(function () {

            inicio();
            iniciarControlesCreditos();
            iniciarControlesPorta();
            iniciarControlesxOrigenPagina();
            iniciarControlesxTipoOperacion();

            $("#btnDetalleLinea").click(function () {
                verDetalleLinea();
            });

            $("#btnDetalleLineaDesactiva").click(function () {
                verDetalleLineasDesactiva();
            });

            $("#btnExportarBRMS").click(function () {
                exportarDetalleBRMS();
            });
            //PROY-32439 MAS INI
            $("#btnExportarBRMSValidaciónCliente").click(function () {
                exportarDetalleBRMSValidaciónCliente();
            });
            //PROY-32439 MAS FIN
            //PROY 30748 - INICIO - btnExportarProacBRMS - Persona
            $("#btnExportarProacBRMS").click(function () {
                exportarDetalleProacBRMS();
            });
            //PROY 30748 - FIN

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            $("#btnExportarCampana").click(function () {
                exportarDetalleBRMSCampana();
            });
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

            $("#chkEditarNombre").click(function () {
                editarNombre();
            });

            $("#btnAprobar").click(function () {
                aprobarSEC();
            });

            $("#btnRechazar").click(function () {
                rechazarSEC();
            });

            $("#btnCancelar").click(function () {
                liberarSEC();
            });

            $("#btnCerrar").click(function () {
                cerrarVentana();
            });
        });

        function iniciarControlesCreditos() {

            if (getValue('hidflgOrigenPagina') != 'P') {

                var opciones = getValue('hidListaPerfiles');                
                if (opciones.indexOf(perfil) == -1) {

                    $('#tdLcDisponibleMovil').hide();
                    $('#tdLcDisponibleInternet').hide();
                    $('#tdLcDisponibleCable').hide();
                    $('#tdLcDisponibleTelefonia').hide();
                    $('#tdLcDisponibleBAM').hide();
                    $('#tdLCDisponible').hide();
                    $('#tdLcDisponibleClie').hide();

                    $('#txtLcDisponibleMovil').hide();
                    $('#txtLcDisponibleInternet').hide();
                    $('#txtLcDisponibleCable').hide();
                    $('#txtLcDisponibleTelefonia').hide();
                    $('#txtLcDisponibleBAM').hide();
                    $('#txtLCDisponible').hide();
                    $('#txtLcDisponibleClie').hide();

                    $('#trRiesgo').hide();
                    $('#trExoneraRA').hide();
                    $('#trRiesgoBuro').hide();

                    $('#btnDetalleLineaDesactiva').hide();
                    $('#tdBotonBRMS').hide();

                    $('#trComentarioEvaluador').hide();
                }
            }

            $('#tblReporteSEC').show();
            $('#tblHistorico').hide();
            $('#tblEstados').hide();
            $('#tblEstadosSGA').hide();

        }

        function inicio() {

            if (getValue('hidnMensajeValue') != "") {
                alert(getValue('hidnMensajeValue'));
                if (getValue('hidProceso') == 'OK') {
                    redireccionar();
                }
            }
            document.getElementById('lblDeuda').innerHTML = getValue('hidDeudaCliente'); //PROY 29121

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            var flagBRMSCamp = getValue('hidFlagBRMSCamp');
            if (flagBRMSCamp == '1') {
                $('#btnExportarCampana').show();
            }
            else {
                $('#btnExportarCampana').hide();
            }
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
        }

        function iniciarControlesPorta() {

            if (getValue('hidFlagPortabilidad') != 'S') {
                $('#trDatosPortabilidad').hide();
            }
        }

        function iniciarControlesxOrigenPagina() {

            if (getValue('hidflgOrigenPagina') == 'P') {
                if (getValue('txtTipoRespuestaDC') != '')
                    $('#trRespuestaDC').show();

                $('#tdReporteEstadoHistoricoSGA').hide();
                $('#tdEstadoVenta').hide();
                $('#tdTxtEstadoVenta').hide();
            }
            else {
                $('#trMotivo').hide();
                $('#trObservacion').hide();
            }

            if (getValue('hidflgOrigenPagina') == 'P' && getValue('hidFlgConsulta') != 'S') {
                $('#trBotones').show();
                $('#trBotonCerrar').hide();
            }
            else {
                $('#tdEditar').hide();
                $('#trBotones').hide();
                $('#trBotonCerrar').show();
                $('#txtComentarioAlPdv').attr('readonly', false);
                $('#txtComentarioEvaluador').attr('readonly', false);
            }


            if (ConstflagPlanesProactivos == '0') 
                $('#btnExportarProacBRMS').hide(); //PROY - 30748
            else 
                $('#btnExportarProacBRMS').show(); //PROY - 30748
        }

        function iniciarControlesxTipoOperacion() {

            if (getValue('hidTipoOperacion') == constTipoOperacionRenovacion) {
                $('#trResumenLinea').hide();
                $('#tdTelefonoRenovar').show();
                $('#txtTelefonoRenovar').show();
                $('#trRenovacion').show();
                $('#tdEditar').hide();
                $('#btnDetalleLineaDesactiva').hide();
                $('#trNuevosDatosBRMS').show(); //PROY-140579 RU07 NN                 
            }

            if (getValue('hidFlgCombo') != 'S') {
                $('#idlblCombo').hide();
                $('#idtxtCombo').hide();
            }

            //PROY-31948 INI
            var opciones = getValue('hidListaPerfiles');
            
            if (opciones.indexOf(perfilCuotasPer) == -1) 
            {

                $('#tdSistema').hide();
                $('#tdUltimaVenta').hide();
                $('#tdCantCuotasPendLinea').hide();
                $('#txtCantCuotasPendLinea').hide();
                $('#tdMontoPendCuotasLinea').hide();
                $('#txtMontoPendCuotasLinea').hide();
                $('#tdTotalCuotasPend').hide();
                $('#txtTotalCuotasPend').hide();
                $('#tdCantLineasCuota').hide();
                $('#txtCantLineasCuota').hide();
                $('#tdCantMaxCuotas').hide();
                $('#txtCantMaxCuotas').hide();
                $('#tdTotalImportCuotaUlt').hide();
                $('#txtTotalImportCuotaUlt').hide();
                $('#tdCantTotalLineaCuotaUlt').hide();
                $('#txtCantTotalLineaCuotaUlt').hide();
                $('#tdCantMaxCuotasGenUlt').hide();
                $('#txtCantMaxCuotasGenUlt').hide();
        }
            else {

                if (getValue('hidTipoOperacion').toString() != operacion) {
                    $('#tdCantCuotasPendLinea').hide();
                    $('#txtCantCuotasPendLinea').hide();
                    $('#tdMontoPendCuotasLinea').hide();
                    $('#txtMontoPendCuotasLinea').hide();
                }
            }
            //PROY-31948 FIN
        }

        var callbacks = {

            liberarSEC: function (objResponse) {
                if (objResponse.Boleano) {
                    redireccionar();
                }
                else {
                    alert(objResponse.Mensaje);
                }
            },

            rechazarSEC: function (objResponse) {
                alert(objResponse.Mensaje);
                if (constErrorRechazarSEC == objResponse.Mensaje) {
                    $('#btnAprobar').attr("disabled", false);
                    $('#btnRechazar').attr("disabled", false);
                }
                if (objResponse.Boleano) {
                    redireccionar();
                }
            },

            f_AnularSot: function (objResponse) {
                alert(objResponse.Mensaje);
                //                if (objResponse.Boleano) {
                //                    redireccionar();
                //                }
            }
        }

        function editarNombre() {
            var blnEditar = ($("#chkEditarNombre").is(':checked'));
            setVisible('trEditarNombre', blnEditar);

            if (!blnEditar) {
                $('#txtNombreEditar').val("");
                $('#txtApePaternoEditar').val("");
                $('#txtApeMaternoEditar').val("");
                $('#txtFechaNac').val("");
            }
        }

        //        function validarFecha(ctrl) {
        //            var txtFechaNac = getValue('txtFechaNac');
        //            var mydate = new Date();
        //            var fechaActual = mydate.getDate() + "/" + parseInt(mydate.getMonth()) + 1 + "/" + mydate.getFullYear();
        //            if (!esMayorEdad(txtFechaNac, fechaActual)) {
        //                alert('La fecha ingresada no corresponde a una persona mayor de edad.');
        //                setFocus('txtFechaNac');
        //            }
        //        }

        function validarAprobar() {
            if ($("#chkEditarNombre").is(':checked')) {
                if (!validarControl('txtNombreEditar', '', 'Ingresar un nombre válido.')) return false;
                if (!validarControl('txtApePaternoEditar', '', 'Ingresar el apellido paterno válido.')) return false;
                if (!validarControl('txtApeMaternoEditar', '', 'Ingresar el apellido materno válido.')) return false;
                if (!validarControl('txtFechaNac', '', 'Ingresar una fecha de nacimiento válido.')) return false;
            }

            if ($("#chkReingresoSec").is(':checked')) {
                alert("Check para no permitir reingreso, solo se permite con la opción Rechazar.");
                return false;
            }

            if (!validarGarantia()) return false;
            if (!validarCostoInstalacion()) return false;

            return true;
        }

        function aprobarSEC() {
            if (validarAprobar()) {
                if (confirm("Está seguro de enviar la evaluación " + getValue('txtNroSEC') + "?")) {

                    $('#hidGarantia').val(obtenerCadenaGarantia());
                    $('#hidCostoInstalacion').val(obtenerCadenaCostoInstalacion());

                     //PROY-29215 INICIO
                    if (getValue('hidFlgProductoDTH') == 'S' || getValue('hidFlgProductoHFC') == 'S') {

                        //FALLAS PROY-140546
                        if (getValue('hidFlgProductoHFC') == 'S') {
                            $('#hidMAI').val(obtenerMAI());

                            if ($('#hidCostoInstalacion').val().length > 0 && $('#hidCostoInstalacion').val().indexOf(';') > -1 && parseFloat($('#hidMAI').val()) > 0) {
                                var costoInstala = $('#hidCostoInstalacion').val().split(';')[1].substring(0, $('#hidCostoInstalacion').val().split(';')[1].length - 1);
                                var vMai = $('#hidMAI').val();

                                if (parseFloat(costoInstala) < parseFloat(vMai)) {
                                    alert(Key_MensajeMaiMayor);
                                    return false;
                                }
                            }
                        }
                        //FALLAS PROY-140546

                        $('#hidFormaPago').val(obtenerCadenaFomarPagoCuota("FP"));
                        $('#hidCuota').val(obtenerCadenaFomarPagoCuota("C"));
                    }
                    //PROY-29215 FIN

                    $('#btnAprobar').attr("disabled", true);
                    $('#btnRechazar').attr("disabled", true); 

                    frmPrincipal.submit();
                }
            }
        }

        function liberarSEC() {
            PageMethods.liberarSEC(getValue('txtNroSEC'), callbacks.liberarSEC);
        }

        function rechazarSEC() {
            if (confirm("Está Seguro de Rechazar la Evaluación " + getValue('txtNroSEC') + "?")) {

                var strflgReIngreso = (document.getElementById('chkReingresoSec').checked) ? "1" : "";
                var cadenaGarantia = obtenerCadenaGarantia();
                var cadenaCostoInstalacion = obtenerCadenaCostoInstalacion();

                $('#btnAprobar').attr("disabled", true); 
                $('#btnRechazar').attr("disabled", true); 

                PageMethods.rechazarSEC(getValue('txtNroSEC'), getValue('hidFlgConvergente'), cadenaGarantia, cadenaCostoInstalacion, getValue('hidUsuarioRed'), getValue('txtComentarioAlPdv'), getValue('txtComentarioEvaluador'), strflgReIngreso, getValue('hidTiempoInicio'), callbacks.rechazarSEC);
            }
        }

        function mostrarTabVisible(opcion) {
            $('#tblReporteSEC').hide();
            $('#tblEstados').hide();
            $('#tblHistorico').hide();
            $('#tblEstadosSGA').hide();
            $('#trBotones').hide();
            $('#trBotonCerrar').hide();

            switch (opcion) {
                case 'datos':
                    cambiarEstilo('tdReporteSEC', 'tab_activo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_inactivo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_inactivo');
                    $('#tblReporteSEC').show();

                    if (getValue('hidflgOrigenPagina') == 'P' && getValue('hidFlgConsulta') != 'S') {
                        $('#trBotones').show();
                        $('#trBotonCerrar').hide();
                    }
                    else {
                        $('#tdEditar').hide();
                        $('#trBotones').hide();
                        $('#trBotonCerrar').show();
                    }
                    break;

                case 'historico':
                    cambiarEstilo('tdReporteSEC', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_inactivo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_activo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_inactivo');
                    $('#tblHistorico').show();
                    break;

                case 'estado':
                    cambiarEstilo('tdReporteSEC', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_activo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_inactivo');
                    $('#tblEstados').show();
                    break;

                case 'estadoSGA':
                    cambiarEstilo('tdReporteSEC', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_inactivo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_activo');
                    $('#tblEstadosSGA').show();
                    break;
            }
        }

        function cerrarVentana() {
            var ventana = window.self;
            ventana.opener = window.self;
            ventana.close();
        }

        function redireccionar() {
            var url = '../evaluacion_cons/sisact_pool_evaluador_consumer.aspx?';
            url += "cu=" + getValue('hidUsuarioExt');
            window.location.href = url;
        }

        function verArchivo(archivo) {
            var url = '../documentos/sisact_pop_ver_documento.aspx?archivo=' + encodeURI(archivo);
            abrirVentana(url, "", '700', '600', '_blank', true);
        }

        function verDetalle(nroSEC, strIdProd) {
            var url = '../evaluacion_cons/sisact_reporte_evaluacion_persona.aspx?';
            url += 'nroSEC=' + nroSEC + '&flgSoloConsulta=S' + '&flgOrigenPagina=' + getValue('hidflgOrigenPagina');
            url += '&strIdProd=' + strIdProd
            abrirVentana(url, "", '950', '540', '_blank', true);
        }

        function verDetalleLinea() {
            var url = '../consultas/sisact_pop_detalle_linea.aspx?';
            url += 'nroDocumento=' + getValue('hidNroDoc') + '&tipoDocumento=' + getValue('hidTipoDoc') + '&origen=P';
            abrirVentana(url, "", '900', '540', '_blank', true);
        }

        function verDetalleLineasDesactiva() {
            var url = '../consultas/sisact_pop_lineas_desactivas.aspx?';
            url += 'nroDocumento=' + getValue('hidNroDoc') + '&tipoDocumento=' + getValue('hidTipoDoc') + '&origen=P';
            abrirVentana(url, "", '650', '300', '_blank', true);
        }

        function exportarDetalleBRMS() {
            var url = '../consultas/sisact_pop_consulta_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&deudaCliente=' + getValue('hidDeudaCliente');//PROY-29121
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-32439 MAS INI
        function exportarDetalleBRMSValidaciónCliente() {
            var url = '../consultas/sisact_pop_consulta_validacion_cliente_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-32439 MAS FIN
 //PROY 30748 - INICIO 
        function exportarDetalleProacBRMS() {
            var url = '../consultas/sisact_pop_consulta_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&FlagProac'+'=1'
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        function exportarDetalleBRMSCampana() {
            var url = '../consultas/sisact_pop_consulta_campnavidad_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&deudaCliente=' + getValue('hidDeudaCliente');
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
        //PROY 30748 - FIN 
         function mostrarDetalle(idProducto, idFila, nroSEC, idSol, strIdProd) {
            var url = '../consultas/sisact_consulta_detalle_plan.aspx?';
            url += 'idProducto=' + idProducto + '&idFila=' + idFila + '&idSol=' + idSol + '&nroSEC=' + getValue('txtNroSEC') + '&idSEC=' + nroSEC + '&tipoOperacion=' + getValue('hidTipoOperacion');
            url += '&strIdProd=' + idProducto + '&nroDoc=' + getValue('hidNroDoc_Cliente'); //PROY-140743 - AE
            //url += '&strIdProd=' + idProducto;
            abrirVentana(url, "", '850', '300', '_blank', true);
        }

        function f_AnularSot(tipoProducto, nroSec) {
            if (confirm("Está Seguro de Anular la SOT?")) {
                PageMethods.anularSot(tipoProducto, nroSec, callbacks.f_AnularSot);
            }
        }

        function mostrarPopSeguros(strConcatItemsPopUp) { //PROY-24724-IDEA-28174 - INICIO
            var cantItems = strConcatItemsPopUp.split('|').length;
            var altoVentana = "80";
            if (cantItems == 2) altoVentana = "96";
            if (cantItems == 3) altoVentana = "110";
            strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("['ñ']", 'g'), '=');
            strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("[' ']", 'g'), '_');
            var opciones = "dialogHeight: " + altoVentana + "px; dialogWidth: 400px; edge: raised; center: yes; resizable: no; status: no";
            var url = '../evaluacion_cons/sisact_pop_seguros.aspx?concatSeguros=' + strConcatItemsPopUp;
            window.showModalDialog(url, '', opciones);
        } //PROY-24724-IDEA-28174 - FIN
        

        //PROY-29215 - INICIO
        function cambiarFormapago(valor,obj) {
            var tablaResumen = document.getElementById(obj);
            var contResumen = tablaResumen.rows.length;
            var idFila, fila;
            var objCuota
            var valCuota = getValue('hidCuotaparam').split("|");
            var filaCarrito = tablaResumen.rows[2];

			//INICIO PROY-140546
            var nColumnaCuota = 0;
            var nColumnaFormaPago = 0;
            if (filaCarrito.cells.length > 9) {
                nColumnaCuota = 10;
                nColumnaFormaPago = 9;
            }
            else {
                nColumnaCuota = 8;
                nColumnaFormaPago = 7;
            }

            var objFP = filaCarrito.cells[nColumnaFormaPago].getElementsByTagName("select")[0]; //PROY-140546
            var objCuotaOpcion = filaCarrito.cells[nColumnaCuota].getElementsByTagName("option"); //PROY-140546
            //FIN PROY-140546

            var valFP;

            for (var o = 0; o < objFP.length; o++) {
                if (objFP[o].selected) {
                    valFP = objFP[o].text;
                }
            }

            objCuota = filaCarrito.cells[nColumnaCuota].getElementsByTagName("select")[0];//PROY-140546
            var option = document.createElement("option");


            if (valFP.toUpperCase() == "CONTRATA") {

                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in valCuota) {
                    fila = valCuota[cp].split(";");
                    if (fila[1] == "0") {
                        option.text = fila[1];
                        option.value = fila[0];

                        objCuota.add(option);
                    }
                }
                calcularCM(obj); //PROY-140546
            }
            else if (valFP.toUpperCase() == "FACTURACION") {

                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in valCuota) {
                    fila = valCuota[cp].split(";");
                    if (fila[0] != "" && fila[1] != "0") {
                        var option1 = document.createElement("option");

                        option1.text = fila[1];
                        option1.value = fila[0];

                        objCuota.add(option1);
                    }
                }
                calcularCM(obj); //PROY-140546
            }
        }
        //PROY-29215 - FIN

        //PROY-140546 Inicio
        function calcularCM(obj) {
            var tablaResumen = document.getElementById(obj);
            var filaCarrito = tablaResumen.rows[2];

            if (filaCarrito.cells.length > 9) {

                var objCombo = filaCarrito.cells[10].getElementsByTagName("select")[0];
                var costoInstalacion = filaCarrito.cells[8].getElementsByTagName("input")[0].value; //Costo instalacion
                var nMAI = filaCarrito.cells[11].getElementsByTagName("input")[0].value;
                var nuevoCostoAnticipadoInstalacion = 0;
                var nNuevoCostoInstalacion = costoInstalacion - nMAI;

                for (var o = 0; o < objCombo.length; o++) {
                    if (objCombo[o].selected) {
                        var valCombo = objCombo[o].text;
                    }
                }

                var monto = 0;
                if ((costoInstalacion != "" || costoInstalacion == "0" || costoInstalacion != " " || costoInstalacion != 'undefined' || costoInstalacion != undefined) && valCombo != "0") {
                    monto = nNuevoCostoInstalacion / valCombo;
                }
                else {
                    monto;
                }

                filaCarrito.cells[12].getElementsByTagName("input")[0].value = monto;

                //FALLAS PROY-140546
                if (!(parseFloat(costoInstalacion) > 0)) {
                    filaCarrito.cells[11].getElementsByTagName("input")[0].value = 0;
                    filaCarrito.cells[12].getElementsByTagName("input")[0].value = 0;
                }
                //FALLAS PROY-140546
            }            
        }
        //PROY-140546 Fin
