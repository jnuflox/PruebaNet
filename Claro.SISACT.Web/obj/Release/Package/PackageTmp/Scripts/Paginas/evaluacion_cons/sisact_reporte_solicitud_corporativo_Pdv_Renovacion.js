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
            
            //PROY-14579 LM
            $("#btnExportarBRMSValidaciónCliente").click(function () {
                exportarDetalleBRMSValidaciónCliente();
            });

            //PROY 30748 - INICIO - btnExportarProacBRMS - Empresas
            $("#btnExportarProacBRMS").click(function () {
                exportarDetalleProacBRMS();
            });
            //PROY 30748 - FIN

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
            //INI PROY-31948_Migracion
            $("#btnAprobar").click(function () {
                aprobarSECReno();
            });
            $("#btnRechazarReno").click(function () {
                rechazarSECReno();
            });
            $("#btnCancelarReno").click(function(){
            cancelarSECReno();
            });
            
            //FIN PROY-31948_Migracion
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

                $('#btnGrabar').attr("disabled", true);
                if (getValue('hidnMensajeValue') == 'CANCELADO' && getValue('hidProceso') == 'OK') {
                    redireccionar();
                }
                else {
                    alert(getValue('hidnMensajeValue'));
                    if (getValue('hidProceso') == 'OK') {
                        redireccionar();
                    }
                }
            }
            document.getElementById('lblDeudaEmpresa').innerHTML = getValue('hidDeudaClienteEmpresa'); //PROY-29121
        }

        function iniciarControlesPorta() {

            if (getValue('hidFlagPortabilidad') == 'S') {
                $('#trDocumentosAdjuntos').hide();
                $('#trComentario').hide();
                $('#trBotones').hide();
            }
            else {
                $('#trDatosPortabilidad').hide();
                $('#trComentarioPorta').hide();
                $('#trIfrComentarioPorta').hide();
                $('#trBotonesPorta').hide();
                //PROY-31948_Migracion INI
                if (getValue('hidFlgConsulta') == 'S') {
                    $('#btnAprobar').hide();
                    $('#btnRechazarReno').hide();
                    $('#btnCancelarReno').hide();
                    $('#btnCerrar').show();
               }
                else {
                $('#trBotones').show();
                    $('#btnCreditos').hide(); //PROY-31948_Migracion FIN
                }

                if (getValue('hidflgOrigenPagina') == 'P' && getValue('hidFlgConsulta') != 'S') {
                    //verCheckList();//PROY-31948_Migracion
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
                $('#btnCerrar').hide(); //PROY-31948_Migracion
            }

            if (getValue('hidFlgConsulta') == 'S') {
                //                $('#btnCargarCheckList').hide();//PROY-31948_Migracion
                $('#btnComentarioPorta').hide();
                $('#txtComentarioAlPdv').attr('readonly', true);
                $('#txtComentarioEvaluador').attr('readonly', true);
                //INI PROY-31948_Migracion
                $('#btnAprobar').hide();
                $('#btnRechazarReno').hide();
                $('#btnCancelarReno').hide();
                $('#btnCerrar').show();
                //FIN PROY-31948_Migracion
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
                $('#trNuevosDatosBRMS').show(); //PROY-140579 RU07 NN  
                if (getValue('hdnflagHistorico') != "S") {
                $('#btnDetalleLineaDesactiva').hide();
            }
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
            //PROY-31948 FIN
        }

        function mostrarTabVisible(opcion) {
            $('#tblReporteSEC').hide();
            $('#tblEstados').hide();
            $('#tblHistorico').hide();
            $('#tblEstadosSGA').hide();
            $('#trBotones').show(); //PROY-31948_Migracion
            
            $('#trBotonesPorta').hide();

            switch (opcion) {
                case 'datos':
                    cambiarEstilo('tdReporteSEC', 'tab_activo');
                    cambiarEstilo('tdReporteEstadoHistorico', 'tab_inactivo');
                    cambiarEstilo('tdReporteHistoricoCliente', 'tab_inactivo');
                    cambiarEstilo('tdReporteEstadoHistoricoSGA', 'tab_inactivo');
                    $('#tblReporteSEC').show();

                    if (getValue('hidFlgConsulta') == 'S') {
                         //PROY-31948_Migracion ini 
                        $('#btnAprobar').hide();
                        $('#btnRechazarReno').hide();
                        $('#btnCancelarReno').hide();
                        $('#btnCerrar').show(); 
                         //PROY-31948_Migracion fin
                        
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

                PageMethods.grabarCreditos(getValue('txtNroSEC'), getValue('hidFlgConvergente'), cadenaGarantia, cadenaCostoInstalacion, getValue('hidUsuario'), grabarCreditos_Callback);
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
        window.close();  //PROY-31948_Migracion
        }

        //INI PROY-31948_Migracion
        function aprobarSECReno() {
            if (!confirm('Desea aceptar la SEC')) return false;
            $('#hidProceso').val("A");
            //PROY-31948_Migracion_2 INI
            $('#hidGarantia').val(obtenerCadenaGarantia());
            $('#hidCostoInstalacion').val(obtenerCadenaCostoInstalacion());
            //PROY-31948_Migracion_2 FIN
            frmPrincipal.submit();
        }
        function rechazarSECReno() {
            if (!confirm('Desea rechazar la SEC')) return false;
            $('#hidProceso').val("R");
            //PROY-31948_Migracion_2 INI
            $('#hidGarantia').val(obtenerCadenaGarantia());
            $('#hidCostoInstalacion').val(obtenerCadenaCostoInstalacion());
            //PROY-31948_Migracion_2 FIN
            frmPrincipal.submit();
        }
        function cancelarSECReno() {
        $('#hidProceso').val("C");
        if (!confirm('Desea guardar los cambios')) {
            $('#hdnGuardarAlCancelar').val("NO");
            }
            else {
                $('#hdnGuardarAlCancelar').val("SI");
            }

        frmPrincipal.submit();

        }
        //FIN PROY-31948_Migracion

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
                url = constPaginaPoolEmpresa;
            url += "?cu=" + getValue('hidUsuarioExt');
            window.open(url, '_self', null);
        }

        function verDetalle(nroSEC, idProducto) {
            PageMethods.consultaOperacion(nroSEC, verDetalle_Callback);
        }

        function verDetalle_Callback(objResponse) {

            var arrDatos = objResponse.Cadena.split('|');
            var nroSEC = arrDatos[0];
            var idProducto = arrDatos[1];
            
            if(!objResponse.Error) {
            
            var url = '../evaluacion_cons/sisact_reporte_evaluacion_empresa.aspx?';
                url += 'flgSoloConsulta=S' + '&flgOrigenPagina=' + getValue('hidflgOrigenPagina') + '&nroSEC=' + nroSEC + "&cu=" + getValue('hidUsuarioExt');
                url += '&strIdProd=' + idProducto;

                if (objResponse.Tipo == operacion) {
                    var url = '../evaluacion_cons/sisact_reporte_solicitud_corporativo_Pdv_Renovacion.aspx?';
                    url += 'flgSoloConsulta=S' + '&flgOrigenPagina=' + getValue('hidflgOrigenPagina') + '&nroSEC=' + nroSEC + "&cu=" + getValue('hidUsuarioExt') + "&flagHistorico=S";
                    url += '&strIdProd=' + idProducto;
                }

            abrirVentana(url, "", '950', '540', '_blank', true);
            }
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
            url += '&deudaCliente=' + getValue('hidDeudaClienteEmpresa'); //PROY-29121
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY 30748 - INICIO - exportarDetalleProacBRMS - Empresas
        function exportarDetalleProacBRMS() {
            var url = '../consultas/sisact_pop_consulta_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            url += '&deudaCliente=' + getValue('hidDeudaClienteEmpresa'); //PROY-29121
            url += '&FlagProac=' + '1'; //modificado por emmh antes decia Flag ahora FlagProac
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
        //PROY 30748 - FIN
        function mostrarDetalle(idProducto, idFila, nroSEC, idSol) {
            var url = '../consultas/sisact_consulta_detalle_plan.aspx?';
            url += 'idProducto=' + idProducto + '&idFila=' + idFila + '&idSol=' + idSol + '&nroSEC=' + getValue('txtNroSEC') + '&idSEC=' + nroSEC + '&tipoOperacion=' + getValue('hidTipoOperacion');
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

        //PROY-140579 LM
        function exportarDetalleBRMSValidaciónCliente() {
            var url = '../consultas/sisact_pop_consulta_validacion_cliente_brms.aspx?';
            url += 'nroSEC=' + getValue('txtNroSEC');
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
