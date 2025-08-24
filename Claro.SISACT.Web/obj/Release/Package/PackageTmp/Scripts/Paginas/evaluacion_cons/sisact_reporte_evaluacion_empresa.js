
        //EMMH F
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

            if (ConstflagPlanesProactivos == '0')
                $('#btnExportarProacBRMS').hide(); //PROY - 30748
            else
                $('#btnExportarProacBRMS').show(); //PROY - 30748
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

            $("#btnCargarCheckList").click(function () {
                verCheckList();
            });

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
            //PROY 30748 - INICIO - btnExportarProacBRMS - Empresas
            $("#btnExportarProacBRMS").click(function () {
                exportarDetalleProacBRMS();
            });
            //PROY 30748 - FIN

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            $("#btnExportarCampana").click(function () {
                exportarDetalleBRMSCampana();
            });
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

            $("#btnObservar").click(function () {
                grabar('O');
            });

            $("#btnSubsanar").click(function () {
                grabar('S');
            });

            $("#btnGrabar").click(function () {
                $('#btnGrabar').attr("disabled", true);
                grabar('A');
            });

            $("#btnRechazar").click(function () {
                rechazar();
            });

            $("#btnLiberar").click(function () {
                liberar();
            });

            $("#btnCancelar").click(function () {
                cancelar();
            });

            $("#btnComentarioPorta").click(function () {
                cargarComentario();
            });

            $("#btnCerrar").click(function () {
                cerrar();
            });

            $("#btnCreditos").click(function () {
                grabarCreditos();
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

            if (getValue('hidFlagPortabilidad') == 'S' && getValue('hidnMensajeValue') != "") {
                
                $('#btnGrabar').attr("disabled", true);

                alert(getValue('hidnMensajeValue'));
                if (getValue('hidProceso') == 'OK') {
                    redireccionar();
                }
            }
            document.getElementById('lblDeudaEmpresa').innerHTML = getValue('hidDeudaClienteEmpresa'); //29121

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            var flagBRMSCamp = getValue('hidFlagBRMSCamp');
            if (flagBRMSCamp == '1') {
                $('#btnExportarCampana').show();
            }
            else {
                $('#btnExportarCampana').hide();
            }
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
            
            //PROY-140579 RU04 RMR INICIO
            if (getValue('hidTipoOperacion') == '3') {
                document.getElementById('btnExportarBRMSValidaciónCliente').style.display = 'block';
            }
            //PROY-140579 RU04 RMR FIN
        }

        function iniciarControlesPorta() {

            if (getValue('hidFlagPortabilidad') == 'S') {
                $('#btnCargarCheckList').hide();
                $('#trDocumentosAdjuntos').hide();
                $('#trComentario').hide();
                $('#trBotones').hide();
            }
            else {
                $('#trDatosPortabilidad').hide();
                $('#trComentarioPorta').hide();
                $('#trIfrComentarioPorta').hide();
                $('#trBotonesPorta').hide();
                $('#trBotones').show();

                if (getValue('hidflgOrigenPagina') == 'P' && getValue('hidFlgConsulta') != 'S') {
                    verCheckList();
                    $('#txtComentarioAlPdv').attr('readonly', true);
                    $('#txtComentarioEvaluador').attr('readonly', true);
                }
                else {
                    $('#btnCreditos').hide();
                }
            }
        }

        function iniciarControlesxOrigenPagina() {

            if (getValue('hidflgOrigenPagina') == 'P') {
                $('#tdReporteEstadoHistoricoSGA').hide();
                $('#tdEstadoVenta').hide();
                $('#tdTxtEstadoVenta').hide();
            }
            else {
                $('#trBotonesPorta').hide();
            }

            if (getValue('hidFlgConsulta') == 'S') {
                $('#btnCargarCheckList').hide();
                $('#btnComentarioPorta').hide();
                $('#txtComentarioAlPdv').attr('readonly', true);
                $('#txtComentarioEvaluador').attr('readonly', true);
            }

            if (getValue('hidFlgCombo') != 'S') {
                $('#idlblCombo').hide();
                $('#idtxtCombo').hide();
            }
        }

        function iniciarControlesxTipoOperacion() {

            if (getValue('hidTipoOperacion') == constTipoOperacionRenovacion) {
                $('#tdTelefonoRenovar').show(); //PROY-31948
                $('#trRenovacion').show();
                $('#btnDetalleLineaDesactiva').hide();
                $('#trNuevosDatosBRMS').show(); //PROY-140579 RU07 NN  
            }

            if (getValue('hidFlgCombo') != 'S') {
                $('#idlblCombo').hide();
                $('#idtxtCombo').hide();
            }

            //PROY-31948 INI
            var opciones = getValue('hidListaPerfiles');
            
            if (opciones.indexOf(perfilCuotasEmp) == -1) {

                $('#tdCantCuotasPendLineaE').hide();
                $('#txtCantCuotasPendLineaE').hide();
                $('#tdMontoPendCuotasLineaE').hide();
                $('#txtMontoPendCuotasLineaE').hide();

                $('#tdSistemaE').hide();
                $('#tdTotalCuotasPendE').hide();
                $('#txtTotalCuotasPendE').hide();
                $('#tdCantLineasCuotaE').hide();
                $('#txtCantLineasCuotaE').hide();
                $('#tdCantMaxCuotasE').hide();
                $('#txtCantMaxCuotasE').hide();
                $('#tdUltimaVentaE').hide();
                $('#tdTotalImportCuotaUltE').hide();
                $('#txtTotalImportCuotaUltE').hide();
                $('#tdCantTotalLineaCuotaUltE').hide();
                $('#txtCantTotalLineaCuotaUltE').hide();
                $('#tdCantMaxCuotasGenUltE').hide();
                $('#txtCantMaxCuotasGenUltE').hide();
            }
            else {

                if (getValue('hidTipoOperacion').toString() != operacion) {
                    $('#tdCantCuotasPendLineaE').hide();
                    $('#txtCantCuotasPendLineaE').hide();
                    $('#tdMontoPendCuotasLineaE').hide();
                    $('#txtMontoPendCuotasLineaE').hide();
                }
            }
            //PROY-31948 FIN
        }

        function mostrarTabVisible(opcion) {
            $('#tblReporteSEC').hide();
            $('#tblEstados').hide();
            $('#tblHistorico').hide();
            $('#tblEstadosSGA').hide();
            $('#trBotones').hide();
            $('#trBotonesPorta').hide();

            switch (opcion) {
                case 'datos':
                    cambiarEstilo('tdReporteSEC', 'tab_activo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_inactivo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_inactivo');
                    $('#tblReporteSEC').show();

                    if (getValue('hidFlgConsulta') == 'S') {
                        $('#trBotones').show();
                    }
                    else {
                        if (getValue('hidFlagPortabilidad') == 'S') {
                            $('#trBotonesPorta').show();
                        }
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

        function validarGrabar(idProceso) {
            if (!validarControl('txtComentarioAlPdv', '', 'Debe ingresar el Comentario.')) return false;

            var msg = '';
            var totalDG;

            if (getValue('hidFlgConvergente') == 'S') {
                totalDG = calcularTotalGarantia2();
            } else {
                totalDG = calcularTotalGarantia();
            }

            var nroRecibos = getValue('hidListaRecibo');

            if (idProceso == 'O') {
                if (parseFloat(totalDG) == 0) {
                    alert('La SEC actual no requiere DG o RA, debe ser ENVIADO PARA SU ACTIVACION');
                    return false;
                }
                if (parseFloat(nroRecibos) > 0) {
                    msg = 'SEC ya cuenta con al menos un DEPOSITO. ¿Desea ENVIAR al Punto de Venta como OBSERVADO para que adjunte recibos adicionales de DG o RA?';
                } else {
                    msg = 'Se ENVIARA al Punto de Venta como OBSERVADO para que adjunte los recibos de DG o RA' + ' ¿Desea Continuar?';
                }
            }
            else if (idProceso == 'S') {
                msg = 'La SEC ENVIARA al Punto de Venta como EN SUBSANACIÓN POR C&A para que se realicen las modificaciones respectivas' + ' ¿Desea Continuar?';
            }
            else if (idProceso == 'A') {
                msg = 'Se ENVIARA A MESA DE PORTABILIDAD.' + ' ¿Desea Continuar?';
            }

            if (!confirm(msg)) return false;

            return true;
        }

        function grabar(idProceso) {

            if (validarGrabar(idProceso)) {

                $('#hidGarantia').val(obtenerCadenaGarantia());
                $('#hidCostoInstalacion').val(obtenerCadenaCostoInstalacion());
                $('#hidProceso').val(idProceso);

                //INICIO PROY-140546
                if (getValue('hidFlgProductoDTH') == 'S' || getValue('hidFlgProductoHFC') == 'S') {
                    $('#hidFormaPago').val(obtenerCadenaFomarPagoCuota("FP"));
                    $('#hidCuota').val(obtenerCadenaFomarPagoCuota("C"));
                    if (getValue('hidFlgProductoHFC') == 'S') {
                        $('#hidMAI').val(obtenerMAI());

                        //FALLAS PROY-140546
                        if ($('#hidCostoInstalacion').val().length > 0 && $('#hidCostoInstalacion').val().indexOf(';') > -1 && parseFloat($('#hidMAI').val()) > 0) {
                            var costoInstala = $('#hidCostoInstalacion').val().split(';')[1].substring(0, $('#hidCostoInstalacion').val().split(';')[1].length - 1);
                            var vMai = $('#hidMAI').val();

                            if (parseFloat(costoInstala) < parseFloat(vMai)) {
                                alert(Key_MensajeMaiMayor);
                                return false;
                            }
                        }
                        //FALLAS PROY-140546                       
                    }
                }
                //FIN PROY-140546

                $('#btnGrabar').attr("disabled", true);

                frmPrincipal.submit();
            } else {
                $('#btnGrabar').attr("disabled", false);
            }
        }

        function validarCreditos() {
            if (!validarGarantia()) return false;
            if (!validarCostoInstalacion()) return false;

            return true;
        }

        function grabarCreditos() {

            if (validarCreditos()) {
                var cadenaGarantia = obtenerCadenaGarantia();
                var cadenaCostoInstalacion = obtenerCadenaCostoInstalacion();
                //PROY-29215 INICIO
                if (getValue('hidFlgProductoDTH') == 'S' || getValue('hidFlgProductoHFC') == 'S') {

                    $('#hidFormaPago').val(obtenerCadenaFomarPagoCuota("FP"));
                    $('#hidCuota').val(obtenerCadenaFomarPagoCuota("C"));

                    //INICIO PROY-140546
                    if (getValue('hidFlgProductoHFC') == 'S') {
                        $('#hidMAI').val(obtenerMAI());

                        //FALLAS PROY-140546
                        if (cadenaCostoInstalacion.length > 0 && cadenaCostoInstalacion.indexOf(';') > -1 && parseFloat($('#hidMAI').val()) > 0) {
                            var costoInstala = cadenaCostoInstalacion.split(';')[1].substring(0, cadenaCostoInstalacion.split(';')[1].length - 1);
                            var vMai = $('#hidMAI').val();

                            if (parseFloat(costoInstala) < parseFloat(vMai)) {
                                alert(Key_MensajeMaiMayor);
                                return false;
                            }
                        }
                        //FALLAS PROY-140546
                    }
                    //FIN PROY-140546
                }
                else{   
                    $('#hidFormaPago').val(obtenerCadenaFomarPagoCuota(""));
                    $('#hidCuota').val(obtenerCadenaFomarPagoCuota(""));
                }
                PageMethods.grabarCreditos(getValue('txtNroSEC'), getValue('hidFlgConvergente'), cadenaGarantia, cadenaCostoInstalacion, getValue('hidUsuario'), getValue('hidFormaPago'), getValue('hidCuota'), getValue('hidFormaPagoActual'), getValue('hidCuotaActual'), getValue('hidAsesor'), getValue('hidCanalSEC'), getValue('hidMAI'), getValue('hidMAIOriginal'), getValue('hidCostoInstalacion'), getValue('hidFlgProductoHFC'), getValue('hidEstadoPa'), grabarCreditos_Callback); //PROY-140546

              //PROY-29215 FIN
            }
        }

        function grabarCreditos_Callback(objResponse) {
            alert(objResponse.Mensaje);
        }

        function rechazar() {
            if (!validarControl('txtComentarioAlPdv', '', 'Debe ingresar el Comentario.')) return false;
            if (!confirm('Se RECHAZARA la Solicitud. ¿Desea Continuar?')) return false;

            $('#hidProceso').val("R");
            frmPrincipal.submit();
        }

        function liberar() {
            if (!confirm('Se LIBERARA la Solicitud. ¿Desea Continuar?')) return false;

            PageMethods.liberar(getValue('txtNroSEC'), getValue('hidNroDoc'), callbacks.liberar);
        }

        function cancelar() {
            if (!confirm('Se PERDERAN los cambios realizados en la Solicitud. ¿Desea Continuar?')) return false;
            redireccionar();
        }

        function cerrar() {
            if (getValue('hidFlgConsulta') != 'S') {
                redireccionar();
            }
            else {
                window.close();
            }
        }

        var callbacks = {

            liberar: function (objResponse) {
                if (objResponse.Boleano) {
                    redireccionar();
                }
                else {
                    alert(objResponse.Mensaje);
                }
            }
        }

        function redireccionar() {
            var url = '';
            if (getValue('hidFlagPortabilidad') == 'S') {
                url = constPaginaPoolEmpresaPorta;
            } else {
                url = constPaginaPoolEmpresa;
            }

            url += "?cu=" + getValue('hidUsuarioExt');
            window.location.href = url;
        }

        function verDetalle(nroSEC, idProducto) {
            var url = '../evaluacion_cons/sisact_reporte_evaluacion_empresa.aspx?';
            url += 'flgSoloConsulta=S' + '&flgOrigenPagina=' + getValue('hidflgOrigenPagina') + '&nroSEC=' + nroSEC + "&cu=" + getValue('hidUsuarioExt');
            url += '&strIdProd=' + idProducto
            abrirVentana(url, "", '950', '540', '_blank', true);
        }

        function verDetalleLinea() {
            var url = '../consultas/sisact_pop_detalle_linea.aspx?';
            url += 'nroDocumento=' + getValue('hidNroDoc') + '&tipoDocumento=06' + '&origen=P';
            abrirVentana(url, "", '900', '540', '_blank', true);
        }

        function verDetalleLineasDesactiva() {
            var url = '../consultas/sisact_pop_lineas_desactivas.aspx?';
            url += 'nroDocumento=' + getValue('hidNroDoc') + '&tipoDocumento=06' + '&origen=P';
            abrirVentana(url, "", '650', '300', '_blank', true);
        }

        function exportarDetalleBRMS() {
            var url = '../consultas/sisact_pop_consulta_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&deudaCliente=' + getValue('hidDeudaClienteEmpresa');//PROY-29121
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-32439 MAS INI
        function exportarDetalleBRMSValidaciónCliente() {
            var url = '../consultas/sisact_pop_consulta_validacion_cliente_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-32439 MAS FIN
        //PROY 30748 - INICIO - exportarDetalleProacBRMS - Empresas
        function exportarDetalleProacBRMS() {
            var url = '../consultas/sisact_pop_consulta_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&FlagProac=' + '1'; //modificado por emmh antes decia Flag ahora FlagProac
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY 30748 - FIN
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        function exportarDetalleBRMSCampana() {
            var url = '../consultas/sisact_pop_consulta_campnavidad_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&deudaCliente=' + getValue('hidDeudaClienteEmpresa');
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
        function mostrarDetalle(idProducto, idFila, nroSEC, idSol) {
            var url = '../consultas/sisact_consulta_detalle_plan.aspx?';
            url += 'idProducto=' + idProducto + '&idFila=' + idFila + '&idSol=' + idSol + '&nroSEC=' + getValue('txtNroSEC') + '&idSEC=' + nroSEC + '&tipoOperacion=' + getValue('hidTipoOperacion');
            url += '&nroDoc=' + getValue('txtNroRUC'); //PROY-140743 - AE
            abrirVentana(url, "", '850', '300', '_blank', true);
        }

        function verCheckList() {
            var url = constPaginaChecklist;
            url += '?codsec=' + getValue('txtNroSEC');
            url += '&pagina_origen=evaluaciones';
            url += '&tipoDespacho=' + ConstCodTipoDespaDescentra;
            url += "&cu=" + getValue('hidUsuarioExt');

            abrirVentana(url, "", '850', '300', '_blank', true);
        }

        function verArchivo(archivo) {
            var url = '../documentos/sisact_pop_ver_documento.aspx?archivo=' + encodeURI(archivo);
            abrirVentana(url, "", '700', '600', '_blank', true);
        }

        function verAcuerdos() {
            var url = constPaginaVerAcuerdos;
            url += "?nroSec=" + getValue('txtNroSEC');
            url += "&cu=" + getValue('hidUsuarioExt');
            abrirVentana(url, "", '700', '600', '_blank', true);
        }

        function cargarComentario() {
            var url = '../consultas/sisact_pop_comentario.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            abrirVentana(url, "", '670', '120', '_blank', true);
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
        function cambiarFormapago(valor, obj) {
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

            var objCuotaOpcion = filaCarrito.cells[nColumnaCuota].getElementsByTagName("option"); //PROY-140546
            var objFP = filaCarrito.cells[nColumnaFormaPago].getElementsByTagName("select")[0]; //PROY-140546
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
                var objCombo = filaCarrito.cells[10].getElementsByTagName("select")[0];//Cuotas            
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

                //INICIO FALLAS PROY-140546
                if (!(parseFloat(costoInstalacion) > 0)) {
                    filaCarrito.cells[11].getElementsByTagName("input")[0].value = 0;
                    filaCarrito.cells[12].getElementsByTagName("input")[0].value = 0;
                }
                //FIN FALLAS PROY-140546
            }            
        }
        //PROY-140546 Fin
