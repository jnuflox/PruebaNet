<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_reporte_evaluacion_persona.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.Evaluacion_cons.sisact_reporte_evaluacion_persona" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="Claro.SISACT.Entity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Datos de la Solicitud de Evaluación Persona</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link href="../../Estilos/calendar-blue.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src='<%= ResolveClientUrl("~/Scripts/jquery-1.9.1.js") %>'
        type="text/javascript"></script>
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_es.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendar_setup.js" type="text/javascript"></script>
    <script src="../../Scripts/calendar/calendario_call.js" type="text/javascript"></script>
    <script src="../../Scripts/funciones_creditos.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        var constPaginaVerDocumento = '<%= ConfigurationManager.AppSettings["constPaginaVerDocumento"] %>';
        var constTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';

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
                var perfil = '<%= ConfigurationManager.AppSettings["constOpcionConsultaCreditos"] %>';
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

            if (getValue('hidMensaje') != "") {
                alert(getValue('hidMensaje'));
                if (getValue('hidProceso') == 'OK') {
                    redireccionar();
                }
            }
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
        }

        function iniciarControlesxTipoOperacion() {

            if (getValue('hidTipoOperacion') == '<%= ConfigurationManager.AppSettings["constTipoOperacionRenovacion"] %>') {
                $('#trResumenLinea').hide();
                $('#tdTelefonoRenovar').show();
                $('#txtTelefonoRenovar').show();
                $('#trRenovacion').show();
                $('#tdEditar').hide();
                $('#btnDetalleLineaDesactiva').hide();
            }

            if (getValue('hidFlgCombo') != 'S') {
                $('#idlblCombo').hide();
                $('#idtxtCombo').hide();
            }
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
            self.frames["ifrmExportar_BRMS"].location.replace(url);
        }
         function mostrarDetalle(idProducto, idFila, nroSEC, idSol, strIdProd) {
            var url = '../consultas/sisact_consulta_detalle_plan.aspx?';
            url += 'idProducto=' + idProducto + '&idFila=' + idFila + '&idSol=' + idSol + '&nroSEC=' + getValue('txtNroSEC') + '&idSEC=' + nroSEC + '&tipoOperacion=' + getValue('hidTipoOperacion');
            url += '&strIdProd=' + idProducto;
            abrirVentana(url, "", '850', '300', '_blank', true);
        }

        function f_AnularSot(tipoProducto, nroSec) {
            if (confirm("Está Seguro de Anular la SOT?")) {
                PageMethods.anularSot(tipoProducto, nroSec, callbacks.f_AnularSot);
            }
        }

    </script>
</head>
<body onkeydown="cancelarBackSpace();" style="margin: 0px;">
    <form id="frmPrincipal" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <table cellspacing="1" cellpadding="0" width="100%" border="0">
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
                    <tr>
                        <td class="Header" align="left" height="20">
                            &nbsp;Datos de la SEC
                        </td>
                    </tr>
                    <tr id="trPuntoVenta">
                        <td>
                            <table class="Contenido" cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Arial10B" width="110">
                                        &nbsp;Número de la SEC:
                                    </td>
                                    <td width="100">
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
                                    <td>
                                        <asp:TextBox ID="txtCasoEspecial" runat="server" CssClass="clsInputDisable" Width="200px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trRespuestaDC" style="display: none">
                                    <td class="Arial10B" width="110">
                                        &nbsp;Tipo:
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtTipoRespuestaDC" CssClass="clsInputDisable" Width="335px" ReadOnly="true"
                                            runat="server"></asp:TextBox>
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
                                        <asp:TextBox ID="txtPuntoVenta" runat="server" CssClass="clsInputDisable" Width="165px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="Arial10B" width="120">
                                        Tipo Operación:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTipoOperacion" runat="server" CssClass="clsInputDisable" Width="200px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trMotivo">
                                    <td class="Arial10B" width="110">
                                        &nbsp;Motivo:
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtMotivoIrCreditos" CssClass="clsInputDisable" Width="95%" ReadOnly="True"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trObservacion">
                                    <td class="Arial10B" width="110">
                                        &nbsp;Observación:
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtMotivoCorreccion" CssClass="clsInputDisable" Width="95%" ReadOnly="True"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trDatosCliente">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" align="left" height="20">
                                        &nbsp;Datos del Cliente
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" width="100">
                                                    &nbsp;Tipo Cliente
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTipoCliente" runat="server" CssClass="clsInputDisable" Width="120px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td width="80%">
                                                                <asp:GridView ID="dgCliente" runat="server" BorderColor="#95B7F3" AutoGenerateColumns="false"
                                                                    OnRowDataBound="dgCliente_RowDataBound">
                                                                    <RowStyle CssClass="Arial10B" HorizontalAlign="Center"></RowStyle>
                                                                    <HeaderStyle CssClass="TablaTitulos" HorizontalAlign="Center"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CLIEV_NOMBRE" HeaderText="Nombres" />
                                                                        <asp:BoundField DataField="CLIEV_APE_PAT" HeaderText="Apellido Paterno" />
                                                                        <asp:BoundField DataField="CLIEV_APE_MAT" HeaderText="Apellido Materno" />
                                                                        <asp:BoundField DataField="TDOCV_DESCRIPCION" HeaderText="Tipo de Documento" />
                                                                        <asp:BoundField DataField="CLIEC_NUM_DOC" HeaderText="N&#250;mero de Documento" />
                                                                        <asp:BoundField DataField="CLIED_FEC_NAC" HeaderText="Fecha Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField DataField="CLIED_FEC_NAC_PDV" HeaderText="Fecha Nacimiento PDV" DataFormatString="{0:dd/MM/yyyy}" />
                                                                        <asp:BoundField HeaderText="Edad" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                            <td id="tdEditar" class="Arial10B" width="20%" align="left">
                                                                <asp:CheckBox ID="chkEditarNombre" runat="server" CssClass="Arial10B" Text="Editar Datos del Cliente"
                                                                    Visible="True"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trEditarNombre" style="display: none">
                                                            <td colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td class="Arial10B" width="185px">
                                                                            Nombre(*) :
                                                                        </td>
                                                                        <td class="Arial10B" width="250px">
                                                                            <asp:TextBox onkeypress="validaCaracteresNombres1();" onpaste="return false;" ID="txtNombreEditar"
                                                                                runat="server" CssClass="clsInputEnable" Width="240" MaxLength="40"></asp:TextBox>
                                                                        </td>
                                                                        <td class="Arial10B" width="185px">
                                                                            Apellido Paterno(*) :
                                                                        </td>
                                                                        <td class="Arial10B">
                                                                            <asp:TextBox onkeypress="validaCaracteresNombres1();" onpaste="return false;" ID="txtApePaternoEditar"
                                                                                runat="server" CssClass="clsInputEnable" Width="240" MaxLength="20"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="Arial10B" width="185px">
                                                                            Apellido Materno(*) :
                                                                        </td>
                                                                        <td class="Arial10B" width="250px">
                                                                            <asp:TextBox onkeypress="validaCaracteresNombres1();" onpaste="return false;" ID="txtApeMaternoEditar"
                                                                                runat="server" CssClass="clsInputEnable" Width="240" MaxLength="20"></asp:TextBox>
                                                                        </td>
                                                                        <td class="Arial10B" width="180px">
                                                                            Fecha de Nacimiento(*) :
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFechaNac" onblur="validarFecha(this);" onkeyup="formateafecha(this);"
                                                                                runat="server" CssClass="clsInputEnable" Width="80px"></asp:TextBox>
                                                                            <img alt="" id="btnFechaNac" style="border-right: 0px; border-top: 0px; cursor: pointer;
                                                                                border-bottom: 0px; order-left: 0px" src="../../Imagenes/calendario.gif" border="0" />
                                                                            <script type="text/javascript">
                                                                                Calendar.setup(
                                                                                            {
                                                                                                inputField: "txtFechaNac",      // id of the input field
                                                                                                ifFormat: "%d/%m/%Y",       // format of the input field                                                        
                                                                                                button: "btnFechaNac",   // trigger for the calendar (button ID)
                                                                                                singleClick: true,           // double-click mode
                                                                                                step: 1                // show all years in drop-down boxes (instead of every other year as default)
                                                                                            }
                                                                                        );
                                                                            </script>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="Arial10B" width="185px">
                                                                            Sumatoria de CF líneas Mayores:
                                                                        </td>
                                                                        <td class="Arial10B" width="250">
                                                                            <asp:TextBox ID="txtMontoCFMayor" runat="server" CssClass="clsInputDisable" Width="80px"
                                                                                ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                        <td class="Arial10B" width="185px">
                                                                            Sumatoria de CF líneas Menores:
                                                                        </td>
                                                                        <td class="Arial10B">
                                                                            <asp:TextBox ID="txtMontoCFMenor" runat="server" CssClass="clsInputDisable" Width="80px"
                                                                                ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trResumenLinea">
                                                <td class="Arial10B" width="100">
                                                    &nbsp;Resumen &nbsp;Líneas&nbsp;Móvil
                                                </td>
                                                <td>
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
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td id="idlblCombo" class="Arial10B" width="100px">
                                                    &nbsp;Grupo Producto:
                                                </td>
                                                <td id="idtxtCombo" width="200px">
                                                    <asp:TextBox ID="txtCombo" runat="server" CssClass="clsInputDisable" Width="180px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="100px">
                                                    &nbsp;Modalidad Venta:
                                                </td>
                                                <td width="200px">
                                                    <asp:TextBox ID="txtModalidadVenta" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trRenovacion" style="display: none">
                                                <td id="tdTelefonoRenovar" class="Arial10B" width="150px" style="display: none">
                                                    &nbsp;Número a renovar:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTelefonoRenovar" runat="server" CssClass="clsInputDisable" Width="125px"
                                                        ReadOnly="True" Style="text-align: right; display: none"></asp:TextBox>
                                                </td>
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
                                            <tr>
                                                <td colspan="6">
                                                    <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <div style="overflow: auto; width: 100%; height: 100px; align: rigth">
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
                                                                                    <asp:Label ID="lblAnular" runat="server">
                                                                                        <img id="btnAnular" onclick='javascript:f_AnularSot("<%# DataBinder.Eval(Container.DataItem, "ID_PRODUCTO")%>",  "<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>");'
                                                                                            src="../../Imagenes/rechazar.gif" style="cursor: hand;" alt="Anular"/>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
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
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" colspan="6" align="center">
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
                                                <td class="Arial10B">
                                                    &nbsp;Buro Consultado:
                                                </td>
                                                <td class="Arial10B">
                                                    <asp:TextBox ID="txtBuro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                                <td class="Arial10BRed">
                                                    Rango LC Disponible:
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtRangoLC" ReadOnly="True" Width="120px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trRiesgo">
                                                <td class="Arial10BRed">
                                                    &nbsp;Riesgo Claro:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRiesgoClaro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B">
                                                    Riesgo Buro:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRiesgoBuro" CssClass="clsInputDisable" Width="90px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td class="Arial10BRed">
                                                    LC Central de Riesgo:
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtLCBuro" CssClass="clsInputDisable" Width="80px" ReadOnly="True"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="130">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="150" align="left">
                                                    Promedio Facturado
                                                </td>
                                                <td class="Arial10B" width="130">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="150" align="left">
                                                    Monto aún NO Facturado
                                                </td>
                                                <td width="200">
                                                    &nbsp;
                                                </td>
                                                <td width="100">
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
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
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtLcDisponibleMovil" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
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
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtLcDisponibleInternet" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
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
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtLcDisponibleCable" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
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
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtLcDisponibleTelefonia" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
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
                                                    <asp:TextBox ID="txtLcDisponibleBAM" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
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
                                                    <asp:TextBox ID="txtLcDisponibleClie" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
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
                    <tr id="trDatosPortabilidad">
                        <td>
                            <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" colspan="5" height="20">
                                        &nbsp;Datos de la Solicitud de Portabilidad
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" cellspacing="1" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" width="130">
                                                    &nbsp;Cedente:
                                                </td>
                                                <td class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtCedente" ReadOnly="True" Width="150px" CssClass="clsInputDisable"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td class="Arial10B" width="230">
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" width="230">
                                                    &nbsp;Nro de Folio de Formato de Portabilidad:
                                                </td>
                                                <td class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtNroFolio" ReadOnly="True" Width="80px" CssClass="clsInputDisable"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" width="130">
                                                    &nbsp;Modalidad:
                                                </td>
                                                <td colspan="4" class="Arial10B" width="200">
                                                    <asp:TextBox ID="txtModalidad" ReadOnly="True" Width="150px" CssClass="clsInputDisable"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" colspan="5">
                                                    &nbsp;Documentos Anexos
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <table cellspacing="1" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgPortabilidad" runat="server" Width="50%" AutoGenerateColumns="False"
                                                                    EnableViewState="False">
                                                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                                                    <RowStyle CssClass="Arial10B" HorizontalAlign="Center" />
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" Height="15px" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ARCH_TIPO" HeaderText="Tipo" />
                                                                        <asp:BoundField DataField="ARCH_RUTA" HeaderText="" Visible="false" />
                                                                        <asp:BoundField DataField="ARCH_NOMBRE" HeaderText="Nombre" />
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <img id="imgArchivo" name='imgArchivo' src="../../Imagenes/botones/btn_VerArchivo.gif"
                                                                                    alt="Ver Archivo" onclick='javascript:verArchivo("<%# DataBinder.Eval(Container.DataItem, "ARCH_RUTA")%>");'
                                                                                    style="cursor: hand;" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
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
                            </table>
                        </td>
                    </tr>
                    <tr id="trDatosEvaluador">
                        <td>
                            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="Header" height="20">
                                        &nbsp;Información del Evaluador
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
                                                <td id="tdBotonBRMS">
                                                    <input class="Boton" id="btnExportarBRMS" style="cursor: hand; width: 130px; height: 17px"
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
                                                            <asp:BoundField HeaderText="Tipo Garantía" DataField="GARANTIA_CRED" />
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
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblTitulo" runat="server" Text="Pago de Rentas Adelantadas: "></asp:Label>
                                                    <asp:Label ID="lblPago" runat="server" Font-Bold="True" ForeColor="Black">No</asp:Label>
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
                                            <tr>
                                                <td colspan="6">
                                                    <asp:CheckBox ID="chkReingresoSec" runat="server" Text="NO PERMITIR EL REINGRESO DE SEC BAJO LAS CONDICIONES DE EVALUACIÓN REGISTRADAS &#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;EN ESTA SEC"
                                                        Visible="True" CssClass="Arial10B"></asp:CheckBox>
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
                            <table cellspacing="1" cellpadding="0" width="100%" border="0">
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
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblHistorico" style="overflow: auto" cellspacing="1" cellpadding="0" width="100%"
                    border="0">
                    <tr>
                        <td>
                            <asp:GridView ID="dgHistorico" runat="server" AutoGenerateColumns="False" EnableViewState="False">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" CssClass="TablaTitulos" />
                                <RowStyle BackColor="#E9EBEE" HorizontalAlign="Center" CssClass="Arial10B" />
                                <AlternatingRowStyle BackColor="#DDDEE2" HorizontalAlign="Center" CssClass="Arial10B" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Acción">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                                        <ItemTemplate>
                                            <a href='javascript:verDetalle("<%# DataBinder.Eval(Container.DataItem, "SOLIN_CODIGO")%>", "<%# DataBinder.Eval(Container.DataItem, "PRDC_CODIGO")%>")'>
                                                <img src="../../Imagenes/ico_lupa.gif" border="0" alt='Ver Detalle'></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SOLIN_CODIGO" HeaderText="Nro SEC" />
                                    <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="Nombre/Razón Social" />
                                    <asp:BoundField DataField="ESTAV_DESCRIPCION" HeaderText="Estado" />
                                    <asp:BoundField DataField="SOLID_FEC_REG" HeaderText="Fecha Registro" />
                                    <asp:BoundField DataField="TDOCV_DESCRIPCION" HeaderText="Tipo Doc." />
                                    <asp:BoundField DataField="NUM_DOCU" HeaderText="Nro Doc." />
                                    <asp:BoundField DataField="RIESGO" HeaderText="Riesgo" />
                                    <asp:BoundField DataField="SOLIN_SUM_CAR_CON" HeaderText="CF Evaluado" />
                                    <asp:BoundField DataField="PRDV_DESCRIPCION" HeaderText="Tipo Producto" />
                                    <asp:BoundField DataField="TCESC_DESCRIPCION" HeaderText="Caso Especial" />
                                    <asp:BoundField DataField="CANTIDAD_LINEAS" HeaderText="Cant. Líneas" />
                                    <asp:BoundField DataField="OVENV_DESCRIPCION" HeaderText="Oficina PDV" />
                                    <asp:BoundField DataField="SOLIV_NUM_CON" HeaderText="Num. Acuerdo" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table id="tblEstados" cellspacing="1" cellpadding="0" width="100%" border="0">
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
        <tr>
            <td>
                <table width="90%" border="0">
                    <tr id="trBotones">
                        <td align="center">
                            <input class="Boton" id="btnAprobar" style="width: 160px; height: 18px" type="button"
                                value="Aceptar" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input class="Boton" id="btnRechazar" style="width: 160px; height: 18px" type="button"
                                value="Rechazar" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <input class="Boton" id="btnCancelar" style="width: 160px; height: 18px" type="button"
                                value="Cancelar" />
                        </td>
                    </tr>
                    <tr id="trBotonCerrar">
                        <td align="center">
                            <input class="Boton" id="btnCerrar" style="width: 126px; cursor: hand; height: 18px"
                                type="button" value="Cerrar" />
                        </td>
                    </tr>
                </table>
                <input id="hidTipoDoc" type="hidden" runat="server" />
                <input id="hidNroDoc" type="hidden" runat="server" />
                <input id="hidFlgConsulta" type="hidden" runat="server" />
                <input id="hidflgOrigenPagina" type="hidden" runat="server" />
                <input id="hidProceso" type="hidden" runat="server" />
                <input id="hidUsuarioExt" type="hidden" runat="server" />
                <input id="hidListaPerfiles" type="hidden" runat="server" />
                <input id="hidFlagPortabilidad" type="hidden" runat="server" />
                <input id="hidNombres" type="hidden" runat="server" />
                <input id="hidApePaterno" type="hidden" runat="server" />
                <input id="hidApeMaterno" type="hidden" runat="server" />
                <input id="hidGarantia" type="hidden" runat="server" />
                <input id="hidCostoInstalacion" type="hidden" runat="server" />
                <input id="hidFlgProductoDTH" type="hidden" runat="server" />
                <input id="hidTipoOperacion" type="hidden" runat="server" />
                <input id="hidTiempoInicio" type="hidden" runat="server" />
                <input id="hidMensaje" type="hidden" runat="server" />
                <input id="hidFlgProductoHFC" type="hidden" runat="server" />
                <input id="hidUsuarioRed" type="hidden" runat="server" />
                <input id="hidContFila" type="hidden" runat="server" />
                <input id="hidFlgConvergente" type="hidden" runat="server" />
                <input id="hidFlgCombo" type="hidden" runat="server" />

                <%--//Inicio IDEA-30067--%>
                <input id="hidProductoPortAuto" type="hidden" name="hidProductoPortAuto" runat="server" />
                <%--//Fin IDEA-30067--%>

            </td>
        </tr>
    </table>
    </form>
    <iframe id="ifrmExportar_BRMS" style="width: 0px; height: 0px" />
</body>
</html>
