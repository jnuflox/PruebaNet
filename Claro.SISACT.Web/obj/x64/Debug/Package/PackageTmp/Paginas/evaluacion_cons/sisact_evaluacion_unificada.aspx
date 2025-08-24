<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="sisact_evaluacion_unificada.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.evaluacion_cons.sisact_evaluacion_unificada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script src="../../Scripts/funciones_sec.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_evaluacion.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/Lib_FuncValidacion.js"></script>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript">

        //Constantes Tipos Productos
        var codTipoProductoMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var codTipoProductoFijo = '<%= ConfigurationManager.AppSettings["constTipoProductoFijo"] %>';
        var codTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        var codTipoProductoBAM = '<%= ConfigurationManager.AppSettings["constTipoProductoBAM"] %>';
        var codTipoProductoHFC = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';
        var codTipoProductoVentaVarios = '<%= ConfigurationManager.AppSettings["constTipoProductoVentaVarios"] %>';
        var codTipoProd3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';
        //Constantes Opciones Pagina
        var opcionVerDetalleLinea = '<%= ConfigurationManager.AppSettings["ConstVerDetalleLinea"] %>';
        var opcionVentaPuertaPuerta = '<%= ConfigurationManager.AppSettings["PerfilVentaPuertaPuerta"] %>';
        var opcionConsultaCreditos = '<%= ConfigurationManager.AppSettings["constOpcionConsultaCreditos"] %>';
        var constTipoDocumentoDNI = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>';
        var constTipoDocumentoCE = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"] %>';
        var constTipoDocumentoRUC = '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"] %>'; 
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

        var mydate = new Date();
        var fechaActual = mydate.getDate() + "/" + parseInt(mydate.getMonth()) + 1 + "/" + mydate.getFullYear();

        function inicio() {
            cambiarCanal();
            cambiarTipoDocumento();
            inicializarPerfiles();

            if (getValue('hidPerfilCreditos') != 'S') {
                document.getElementById('tdRiesgoClaro').style.display = 'none';
                document.getElementById('tdTxtRiesgoClaro').style.display = 'none';
            }

            if (getValue('hidPerfil_149') == 'S')
                document.getElementById('trPuntoVenta').style.display = '';
            else
                document.getElementById('ddlPuntoVenta').selectedIndex = 1;

            //DNI Vendedor
            mostrarVendedor();

            if (getValue('hidAccion') == 'Grabar') {

                var nroSEC = getValue('hidNroSEC');
                var mensaje = getValue('hidMensaje');
                var blnOK = (getValue('hidCodError') != '1');
                var blnAjuntar = (getValue('hidAdjuntarIngreso') == 'S');
                var strTipoDoc = getValue('hidTipoDocumento');
                var blnIrCreditos = (getValue('hidCreditosxAsesor') == 'S');
                var strEstadoSEC = getValue('hidCodEstadoSEC');
                var blnPortabilidad = (getValue('hidTienePortabilidad') == 'S');

                nuevaEvaluacion();
                alert(mensaje);

                if (blnOK) {
                    if (strTipoDoc == constTipoDocumentoRUC) {
                        if (!blnPortabilidad) {
                            if (blnIrCreditos || strEstadoSEC != '<%= ConfigurationManager.AppSettings["constEstadoAPR"] %>')
                                AdjuntarDocumentos(nroSEC);
                        }
                    }

                    if (blnAjuntar) {
                        if (strTipoDoc != constTipoDocumentoRUC)
                            verAdjuntarDocumentos(nroSEC);
                    }
                }
            }
        }

        function nuevaEvaluacion() {
            inicializarDatosInicial();
            inicializarPanelDatosInicial();
            inicializarPanelDetalleCliente();
            inicializarPanelRepresentante();
            inicializarPanelCondicionVenta();
            inicializarPanelPortabilidad();
            inicializarPanelResultado();
            inicializarPanelComentarios();
            inicializarPanelGrabar();
        }

        function inicializarDatosInicial() {
            var arrControles = document.getElementById('frmPrincipal').all;
            var strListaHidden = ",__EVENTTARGET,__EVENTARGUMENT,__VIEWSTATE";
            strListaHidden = strListaHidden + ",hidTiempoInicioReg,hidVerDetalleLinea,hidVerVentaProactiva,hidPerfil_149,hidListaPuntoVenta,hidListaBlackList,hidBLVendedor,hidListaParametro,hidListaParametroII,hidFactorLC";
            strListaHidden = strListaHidden + ",hidCanalSap,hidOrgVenta,hidTipoDocVentaSap,hidCentro,hidListaTipoOperacion,hidNroMinPlanesPorta,hidCodCampValidacion,hidCodPlanValidacion,hidCodPlanesValidacionI35,hidNroMinPlanesI35";
            strListaHidden = strListaHidden + ",hidPerfilCreditos,hidListaPerfiles;hidUsuarioRed;hidPlanBase;hidPlanCombo;hidPlanComboRestringido";
            for (var i = 0; i < arrControles.length; i++) {
                if (arrControles[i].type == 'hidden' || arrControles[i].tagName == 'HIDDEN') {
                    if (strListaHidden.indexOf(arrControles[i].name) < 0)
                        arrControles[i].value = '';
                }
            }
        }

        function inicializarPerfiles() {
            setValue('hidVerDetalleLinea', buscarPerfil(opcionVerDetalleLinea));
            setValue('hidVerVentaProactiva', buscarPerfil(opcionVentaPuertaPuerta));
            setValue('hidPerfilCreditos', buscarPerfil(opcionConsultaCreditos));
        }

        function inicializarPanelDatosInicial() {
            if (getValue('hidPerfil_149') == 'S') {
                setValue('hidBLVendedor', '');
                document.getElementById('ddlCanal').selectedIndex = 1;
                cambiarCanal();
            }
            setValue('txtNroDoc', '');
            setValue('txtFechaNac', '');

            habilitarControl(true);
            document.getElementById('ddlTipoDocumento').selectedIndex = 0;
            cambiarTipoDocumento();
            mostrarVendedor();
            setEnabled('txtDNIVendedor', true, 'clsInputEnabled');

            document.getElementById('btnDetalleLinea').style.display = 'none';
            document.getElementById('lblMensajeDeudaBloqueo').innerHTML = '';

            document.getElementById('chkPortabilidad').checked = false;
            cambiarPortabilidad(false);

            habilitarBoton('btnvalidarClaro', 'Validación Claro', true);
        }

        function inicializarPanelDetalleCliente() {
            setEnabled('txtNombre', true, 'clsInputEnabled');
            setEnabled('txtApePat', true, 'clsInputEnabled');
            setEnabled('txtApeMat', true, 'clsInputEnabled');
            setEnabled('txtRazonSocial', true, 'clsInputEnabled');
            habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);

            setValue('txtNombre', '');
            setValue('txtApePat', '');
            setValue('txtApeMat', '');
            setValue('txtRazonSocial', '');
            setValue('hidIntentos10', '0');
            document.getElementById('lblCategoriaCliente').innerHTML = '';

            trLineasDesactivas.style.display = 'none';
            document.getElementById('ifrLineasDesactivas').src = "";

            trDetalleCliente.style.display = 'none';
            trConsultarDC.style.display = 'none';
            trDetalleRUC.style.display = 'none';
            trDetalleDNI.style.display = '';
        }

        function inicializarPanelRepresentante() {
            trRepresentante.style.display = 'none'; //self.frames["ifraRepresentante"].window.location.reload();
            document.getElementById('ifraRepresentante').src = "";
        }

        function inicializarPanelCondicionVenta() {
            setEnabled('btnAgregarPlan', true, 'Boton');
            document.getElementById('ddlOferta').selectedIndex = 0;
            document.getElementById('ddlCasoEspecial').selectedIndex = 0;
            document.getElementById('ddlModalidadVenta').selectedIndex = 0;
            document.getElementById('ddlCombo').selectedIndex = 0;

            document.getElementById('txtLCDisponiblexProd').value = '';
            document.getElementById('tdMovil').style.display = '';
            document.getElementById('tdFijo').style.display = '';
            document.getElementById('tdBAM').style.display = '';
            document.getElementById('tdDTH').style.display = '';
            document.getElementById('tdHFC').style.display = '';
            document.getElementById('tdVentaVarios').style.display = '';
            document.getElementById('td3PlayInalam').style.display = '';
            trCondicionVenta.style.display = 'none'; //self.frames["ifraCondicionesVenta"].window.location.reload();
            trCondicionVentaDetalle.style.display = 'none';

            // Validación Modalidad / Operador Cedente
            document.getElementById('tdModalidad').style.display = 'none';
            document.getElementById('tdOperadorCedente').style.display = 'none';
            //gaa20151204
            document.getElementById('ddlOferta').disabled = false;
            document.getElementById('ddlModalidadVenta').disabled = false;
            document.getElementById('ddlCasoEspecial').disabled = false;
            document.getElementById('ddlTipoOperacion').disabled = false;
            document.getElementById('ddlCombo').disabled = false;
            //fin gaa20151204
        }

        function inicializarDatosCasoEspecial() {
            setValue('hidWhitelistCE', '');
            setValue('hidCargoFijoMaxCE', '');
            setValue('hidlistaCEPlanBscs', '');
            setValue('hidlistaCEPlan', '');
            setValue('hidlistaCEPlanxProd', '');
            setValue('hidCasoEspecial', '');
        }

        function inicializarPanelCondicionVentaII() {
            setEnabled('btnAgregarPlan', true, 'Boton');

            trAdjuntoPorta.style.display = 'none';
            inicializarPanelResultado();
            inicializarPanelComentarios();
            inicializarPanelGrabar();
        }

        function inicializarPanelResultado() {
            setValue('txtResultado', '');
            setValue('txtLCDisponible', '');
            setValue('txtRiesgoClaro', '');
            setValue('txtComportamiento', '');
            setValue('txtRangoLC', '');
            setValue('txtTipoGarantia', '');
            setValue('txtImporte', '');

            setValue('hidAutonomia', '');
            setValue('hidCreditosxCE', '');
            setValue('hidCreditosxDC7', '');
            setValue('hidCreditosxReglas', '');

            document.getElementById('chkPresentaPoderes').checked = false;
            document.getElementById('btnDetalleGarantia').style.display = 'none';
            setValue('hidPoderes', '');

            trPresentaPoderes.style.display = 'none';
            trGarantia.style.display = 'none';
            trResultado.style.display = 'none';
            tdLCDisponible.style.display = 'none';
            tdTxtLCDisponible.style.display = 'none';
        }

        function inicializarPanelPortabilidad() {
            trAdjuntoPorta.style.display = 'none';  //self.frames["ifraPortabilidad"].window.location.replace("../frames/sisact_ifr_portabilidad.aspx");
            trAdjuntoPorta.style.display = 'none';

            //Inicio IDEA-30067
            setValue('hidProductoPortAuto', '');
            //Fin IDEA-30067
        }

        function inicializarPanelComentarios() {
            setValue('txtComentarioPDV', '');
            setValue('hidComentarioPDV', '');
            trComentario.style.display = 'none';
        }

        function inicializarPanelGrabar() {
            setValue('hidCreditosxAsesor', '');
            setValue('hidArchivosEnvioCreditos', '');
            setValue('hidCreditosxNombres', '');
            setValue('hidAdjuntarIngreso', '');
            setValue('hidCreditosxLineaDesactiva', '');
            habilitarBoton('btnGrabar', 'Grabar', true);
            document.getElementById('btnEnviarCreditos').style.display = 'none';
            trGrabar.style.display = 'none';
        }

        function habilitarControl(bln) {
            if (bln) {
                if (isVisible('trPuntoVenta')) {
                    setEnabled('ddlCanal', true, '');
                    setEnabled('ddlPuntoVenta', true, '');
                }
                setEnabled('ddlTipoDocumento', true, '');
                setEnabled('txtNroDoc', true, 'clsInputEnabled');
                setEnabled('txtFechaNac', true, 'clsInputEnabled');
                if (isVisible('trVendedor')) {
                    setEnabled('txtDNIVendedor', true, 'clsInputEnabled');
                    setEnabled('btnvalidarVendedor', true, 'Boton');
                }
                if (isVisible('chkPortabilidad')) setEnabled('chkPortabilidad', true, '');
            }
            else {
                if (isVisible('trPuntoVenta')) {
                    setEnabled('ddlCanal', false, '');
                    setEnabled('ddlPuntoVenta', false, '');
                }
                setEnabled('ddlTipoDocumento', false, '');
                setEnabled('txtNroDoc', false, 'clsInputDisabled');
                setEnabled('txtFechaNac', false, 'clsInputDisabled');
                if (isVisible('trVendedor')) {
                    setEnabled('txtDNIVendedor', false, 'clsInputDisabled');
                    setEnabled('btnvalidarVendedor', false, '');
                }
                if (isVisible('chkPortabilidad')) setEnabled('chkPortabilidad', false, '');
            }
        }

        function cambiarTipoDocumento() {
            document.getElementById('txtNroDoc').value = '';
            document.getElementById('txtNroDoc').maxLength = getMaxLengthDocumento(document.getElementById('ddlTipoDocumento').value);
            setFocus('txtNroDoc');

            if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                document.getElementById('tdLblFechaNac').style.display = 'none';
                document.getElementById('tdTxtFechaNac').style.display = 'none';
            }
            else {
                document.getElementById('tdLblFechaNac').style.display = '';
                document.getElementById('tdTxtFechaNac').style.display = '';
            }
        }

        function cambiarCanal() {
            var strCodCanal = getValue('ddlCanal');
            var ddlPuntoVenta = document.getElementById('ddlPuntoVenta');

            inicializarCombo(ddlPuntoVenta);

            if (strCodCanal != '') {
                setValue('hidCanal', strCodCanal);
                var arrPuntoVenta = document.getElementById('hidListaPuntoVenta').value.split('|');
                for (var i = 0; i < arrPuntoVenta.length; i++) {
                    var colPuntoVenta = arrPuntoVenta[i].split(';');
                    if (colPuntoVenta[2] == strCodCanal) {
                        cargarCombo(ddlPuntoVenta, colPuntoVenta[0], colPuntoVenta[1] + " - " + colPuntoVenta[0]);
                    }
                }
            }

            cambiarPuntoVenta();
        }

        function cambiarPuntoVenta() {
            if (getValue('hidPerfil_149') == 'S')
                document.getElementById('hidBLVendedor').value = '';

            var arrPuntoVenta = document.getElementById('hidListaPuntoVenta').value.split('|');
            for (i = 0; i < arrPuntoVenta.length; i++) {
                var filaPuntoVenta = arrPuntoVenta[i].split(';');
                if (filaPuntoVenta[0] == getValue('ddlPuntoVenta')) {
                    if (filaPuntoVenta[4] != 'L') {
                        mostrarVendedor();
                        return false;
                    }
                }
            }
            //DNI Vendedor - Se mantiene funcionalidad anterior
            var ListaBL = getValue('hidListaBlackList');
            if (ListaBL.length > 0) {
                var arrayBL = ListaBL.split('|');
                var SelUsr = getValue('ddlCanal') + "-" + getValue('ddlPuntoVenta').split('-')[0];
                var iExiste = 0;
                if (arrayBL.length > 0) {
                    for (i = 0; i < arrayBL.length; i++) {
                        // Si la columna pdv tiene todos...
                        if (arrayBL[i].split('-')[1] == '0') {
                            // Valido unicamente por el canal...
                            if (arrayBL[i].split('-')[0] == getValue('ddlCanal'))
                                iExiste = iExiste + 1;
                        }
                        else {
                            // Sino valido por el canal y el pdv...
                            if (arrayBL[i] == SelUsr)
                                iExiste = iExiste + 1;
                        }
                    }
                }
            }
            if (iExiste > 0)
                setValue('hidBLVendedor', 'S');

            //DNI Vendedor
            mostrarVendedor();
        }

        function cambiarVendedor() {
            if (isVisible('trVendedor'))
                setEnabled('btnvalidarVendedor', true, 'Boton');
        }

        function cambiarPortabilidad(chk) {
            document.getElementById('hidTienePortabilidad').value = 'N';
            if (chk.checked)
                document.getElementById('hidTienePortabilidad').value = 'S';
        }

        function mostrarVendedor() {
            setValue('hidIdVendedor', '');
            setValue('txtDNIVendedor', '');
            if (getValue('hidBLVendedor') == 'S' && getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
                document.getElementById('trVendedor').style.display = '';
                setEnabled('btnvalidarVendedor', true, 'Boton');
            } else
                document.getElementById('trVendedor').style.display = 'none';
        }

        function validarVendedor() {
            setValue('hidIdVendedor', '');
            if (getValue('txtDNIVendedor').length != getMaxLengthDocumento('<%= ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"] %>')) {
                setFocus('txtDNIVendedor');
                alert("Ingresar número de DNI Vendedor válido.");
                return false;
            }
            PageMethods.consultaDNIVendedor(getValue('txtDNIVendedor'), getValue('ddlPuntoVenta'), consultaDNIVendedor_Callback);
        }

        function consultaDNIVendedor_Callback(objResponse) {
            alert(objResponse.Mensaje);
            if (objResponse.Boleano) {
                setValue('hidIdVendedor', objResponse.Cadena);
                setEnabled('btnvalidarVendedor', false, '');
            }
        }

        function validarClaro() {
            document.getElementById('lblMensajeDeudaBloqueo').innerHTML = '';

            if (isVisible('trPuntoVenta')) {
                if (!validarControl('ddlCanal', '', 'Seleccione el Canal.')) return false;
                if (!validarControl('ddlPuntoVenta', '', 'Seleccione el Punto de Venta.')) return false;
            }
            if (getValue('txtNroDoc').length != getMaxLengthDocumento(getValue('ddlTipoDocumento'))) {
                setFocus('txtNroDoc');
                alert("Ingresar número de documento válido.");
                return false;
            }

            if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
                var txtFechaNac = getValue('txtFechaNac');
                if (txtFechaNac.length == 8) {
                    var dd = txtFechaNac.substr(0, 2);
                    var mm = txtFechaNac.substr(2, 2);
                    var yyyy = txtFechaNac.substr(4, 4);
                    var fecha = dd + '/' + mm + '/' + yyyy;
                    setValue('txtFechaNac', fecha);
                }

                txtFechaNac = getValue('txtFechaNac');
                if (!esFechaValida(txtFechaNac)) {
                    setFocus('txtFechaNac');
                    alert("Ingresar una Fecha de Nacimiento válida en el formato (dd/mm/yyyy)");
                    return false;
                }
            }
            else
                if (!ValidaRUC('document.getElementById("frmPrincipal").txtNroDoc', 'El campo Nro Documento', false)) return false;

            if (isVisible('trVendedor')) {
                if (getValue('txtDNIVendedor').length != getMaxLengthDocumento(constTipoDocumentoDNI)) {
                    setFocus('txtDNIVendedor');
                    alert("Ingresar número de DNI Vendedor válido.");
                    return false;
                }

                if (isEnabled('btnvalidarVendedor')) {
                    setFocus('btnvalidarVendedor');
                    alert("Validar número de DNI Vendedor.");
                    return false;
                }
            }

            //Obtener Datos Punto de Venta
            var arrPuntoVenta = document.getElementById('hidListaPuntoVenta').value.split('|');
            for (var i = 0; i < arrPuntoVenta.length; i++) {
                var filaPuntoVenta = arrPuntoVenta[i].split(';');
                if (filaPuntoVenta[0] == getValue('ddlPuntoVenta')) {
                    setValue('hidOficina', filaPuntoVenta[0])
                    setValue('hidOficinaActual', filaPuntoVenta)
                    break;
                }
            }

            //Guardar Info
            setValue('hidNroDocumento', getValue('txtNroDoc'));
            setValue('hidTipoDocumento', getValue('ddlTipoDocumento'));
            setValue('hidFechaNac', getValue('txtFechaNac'))

            //Deshabilitar Controles
            habilitarControl(false);
            habilitarBoton('btnvalidarClaro', 'Procesando...', false);
            cargarImagenEsperando();

            //Habilitar frames
            self.frames["ifraCondicionesVenta"].window.location.replace("../frames/sisact_ifr_condiciones_venta.aspx");

            //Datos Portabilidad
            if (getValue('hidTienePortabilidad') == 'S')
                self.frames["ifraPortabilidad"].window.location.replace("../frames/sisact_ifr_portabilidad.aspx");

            //Consulta Claro
            PageMethods.consultaClaro(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), getValue('hidOficina'), getValue('txtFechaNac'), consultaClaro_Callback);
        }

        function consultaClaro_Callback(objResponse) {
            quitarImagenEsperando();

            if (objResponse.Error) {
                if (objResponse.Tipo == 'E') {
                    setFocus('txtFechaNac');
                    document.getElementById('lblMensajeDeudaBloqueo').innerHTML = objResponse.Mensaje;
                    habilitarBoton('btnvalidarClaro', 'Validación Claro', true);
                } else {
                    alert(objResponse.Mensaje);
                    inicializarDatosInicial();
                    inicializarPanelDatosInicial();
                }
                return;
            }

            habilitarBoton('btnvalidarClaro', 'Validación Claro', false);

            var arrParam = objResponse.Cadena.split('#');
            document.getElementById('lblMensajeDeudaBloqueo').innerHTML = arrParam[0];
            document.getElementById('lblCategoriaCliente').innerHTML = arrParam[1];
            setValue('hidEvaluarSoloFijo', arrParam[2]);

            if (getValue('hidVerDetalleLinea') == 'S') document.getElementById('btnDetalleLinea').style.display = '';

            setValue('txtApePat', arrParam[3]);
            setValue('txtApeMat', arrParam[4]);
            setValue('txtNombre', arrParam[5]);
            setValue('txtRazonSocial', arrParam[6]);

            setValue('hidPlanesActivos', arrParam[8]);
            setValue('hidPlanesActivoVozDatos', arrParam[9]);
            setValue('hidPlanesDatosVoz', arrParam[10]);

            setValue('hidTipoDocVentaSap', arrParam[11]);
            setValue('hidCanalSap', arrParam[12]);
            setValue('hidOrgVenta', arrParam[13]);
            setValue('hidCentro', arrParam[14]);
            setValue('hidConsultaPrepago', arrParam[15]);
            setValue('hidBlackListCuota', arrParam[16]);
            setValue('hidFechaHoraConsulta', arrParam[17]);

            if (getValue('hidTienePortabilidad') == 'S') {
                //gaa20151214
                //retornarConsultaSEC('');
                validarSecReno();
                //fin gaa20151214
            }
            else {
                if (arrParam[7] == 'S') {
                    //gaa20151201
                    setValue('hidBuscarSEC', 'S')
                    validarSecReno();
                    //buscarSEC();
                    //fin gaa20151201
                }
                else {
                    setValue('hidBuscarSEC', 'N')
                    validarSecReno();
                    //retornarConsultaSEC('');
                }
            }
        }

        function retornarConsultaSEC(nroSec) {
            var flgTipoDocumentoRUC = (getValue('ddlTipoDocumento') == constTipoDocumentoRUC);
            trDetalleDNI.style.display = (flgTipoDocumentoRUC) ? 'none' : '';
            trDetalleRUC.style.display = (flgTipoDocumentoRUC) ? '' : 'none';

            // Consulta Línea Desactiva
            if (getValue('ddlCanal') == '<%= ConfigurationManager.AppSettings["constCodTipoOficinaCAC"] %>') {
                var url = "../consultas/sisact_pop_lineas_desactivas.aspx?";
                url += "tipoDocumento=" + getValue('ddlTipoDocumento');
                url += "&nroDocumento=" + getValue('txtNroDoc');
                url += "&flgConsultaEval=S" + "&flgConsultaPopup=N";
                document.getElementById('ifrLineasDesactivas').src = url;
                trLineasDesactivas.style.display = '';
            } else {
                trDetalleCliente.style.display = '';
                trConsultarDC.style.display = '';
            }

            // Consulta Línea Prepago
            if (getValue('hidConsultaPrepago') == 'S') {
                var url = '../consultas/sisact_pop_detalle_linea_prepago.aspx?nroDocumento=' + getValue('txtNroDoc');
                window.showModalDialog(url, '', 'dialogHeight:260px; dialogWidth:390px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
            }
        }

        function consultarDC() {
            if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                if (!validarControl('txtRazonSocial', '', 'Ingresar la razón social del cliente.')) return false;
            } else {
                if (!validarControl('txtNombre', '', 'Ingresar nombres del cliente.')) return false;
                if (!validarControl('txtApePat', '', 'Ingresar apellido paterno del cliente.')) return false;
                if (!validarControl('txtApeMat', '', 'Ingresar apellido materno del cliente.')) return false;
            }

            //Deshabilitar Controles
            setEnabled('txtNombre', false, 'clsInputDisabled');
            setEnabled('txtApePat', false, 'clsInputDisabled');
            setEnabled('txtApeMat', false, 'clsInputDisabled');
            habilitarBoton('btnConsultaDC', 'Procesando...', false);

            //Mostrar Productos Configurados
            mostrarTabxOferta();

            //Cargar Tipo Operación
            asignarTipoOperacion();

            //Consultar DataCredito
            consultarDataCredito();
        }

        function consultarDataCredito() {
            var tipoDocumento = getValue('ddlTipoDocumento');
            var nroDocumento = getValue('txtNroDoc');
            var txtRazonSocial = ''; // getValue('txtRazonSocial').replace(/\#/g, '').replace(/\%/g, '');
            var txtNombres = getValue('txtNombre').replace(/\#/g, '').replace(/\%/g, '');
            var txtApellidoPaterno = getValue('txtApePat').replace(/\#/g, '').replace(/\%/g, '');
            var txtApellidoMaterno = getValue('txtApeMat').replace(/\#/g, '').replace(/\%/g, '');

            cargarImagenEsperando();

            if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC)
                PageMethods.consultaDatosDataCredito(tipoDocumento, nroDocumento, txtNombres, txtApellidoPaterno, txtApellidoMaterno, getValue('ddlPuntoVenta'), consultaDataCredito_Callback);
            else {
                if (getValue('txtNroDoc').substring(0, 1) == '<%= ConfigurationManager.AppSettings["constRUCInicio"] %>') {
                    txtRazonSocial = '';
                } else {
                    txtRazonSocial = 'NN';
                }
                PageMethods.consultaDatosDataCreditoCorp(tipoDocumento, nroDocumento, txtRazonSocial, consultaDataCreditoCorp_Callback);
            }
        }

        // ********************************************* Consulta DataCredito RUC ********************************************* //
        function consultaDataCreditoCorp_Callback(objResponse) {
            quitarImagenEsperando();

            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                setEnabled('txtRazonSocial', true, 'clsInputEnabled');
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
                return;
            }

            //Guardar Info DC
            var datosDC = objResponse.Cadena;
            var arrdatosDC = datosDC.split('|');
            document.getElementById('txtRazonSocial').value = arrdatosDC[0];
            document.getElementById('hidNroOperacionDC').value = arrdatosDC[1];
            document.getElementById('hidRiesgoDC').value = arrdatosDC[3];
            setEnabled('txtRazonSocial', false, 'clsInputDisabled');

            //Consultar Representante Legal
            self.frames["ifraRepresentante"].window.location.replace("../frames/sisact_ifr_representante_legal.aspx?nroDocumento=" + getValue('txtNroDoc'));

            //Consultar LC Disponible x Producto
            consultarLCDisponible();
        }

        // ********************************************* Consulta DataCredito DNI/CE ********************************************* //
        function consultaDataCredito_Callback(objResponse) {
            quitarImagenEsperando();

            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                setEnabled('txtNombre', true, 'clsInputEnabled');
                setEnabled('txtApePat', true, 'clsInputEnabled');
                setEnabled('txtApeMat', true, 'clsInputEnabled');
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
                return;
            }

            //Guardar Info DC
            var datosDC = objResponse.Cadena;
            var arrdatosDC = datosDC.split(';');
            var strRiesgo = arrdatosDC[30];
            var strRespuesta = arrdatosDC[34];
            var strNroOperacion = arrdatosDC[33];
            var strFechaNacimiento = arrdatosDC[0];
            var strApePaterno = arrdatosDC[11];
            var strApeMaterno = arrdatosDC[10];
            var strNombres = arrdatosDC[9];

            document.getElementById('hidRespuestaDC').value = strRespuesta;
            document.getElementById('hidRiesgoDC').value = strRiesgo;
            document.getElementById('hidNroOperacionDC').value = strNroOperacion;

            //Por Defecto Edad 18
            var strEdad = '<%= ConfigurationManager.AppSettings["ConstAnioMayorEdad"] %>';
            var strFechaNacDC = strFechaNacimiento.substr(0, 10);
            if (getValue('hidRespuestaDC') != '<%= ConfigurationManager.AppSettings["constRespDataCredTipo7"] %>' && getValue('ddlTipoDocumento') != '<%= ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"] %>')
                strEdad = calculaEdad(strFechaNacDC, fechaActual);

            if (strRespuesta == '<%= ConfigurationManager.AppSettings["constRespDataCredTipo6"] %>') //10
            {
                //Contador10
                var contadorDCTipo10 = getValue('hidIntentos10');
                if (contadorDCTipo10 >= 2)
                    nuevaEvaluacion();
                else {
                    if (confirm("Documento de identidad no coincide con el Apellido ingresado. ¿Desea modificar datos?")) {
                        setEnabled('txtNombre', true, 'clsInputEnabled');
                        setEnabled('txtApePat', true, 'clsInputEnabled');
                        setEnabled('txtApeMat', true, 'clsInputEnabled');
                        habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);

                        setValue('hidIntentos10', parseInt(getValue('hidIntentos10'), 10) + 1);
                    }
                    else
                        nuevaEvaluacion();
                }
            }
            else if (strRespuesta == '<%= ConfigurationManager.AppSettings["constRespDataCredTipo7"] %>') //09
            {
                // Excepción Respuesta Tipo 7 DC
                if (!consultaExcepcionDC7()) {
                    if (confirm("SEC irá a Créditos para validación de identidad.")) {
                        setEnabled('txtNombre', false, 'clsInputDisabled');
                        setEnabled('txtApePat', false, 'clsInputDisabled');
                        setEnabled('txtApeMat', false, 'clsInputDisabled');
                        habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', false);

                        // Consultar LC Disponible x Producto
                        consultarLCDisponible();
                    }
                    else
                        nuevaEvaluacion();
                }
                else {
                    setEnabled('txtNombre', false, 'clsInputDisabled');
                    setEnabled('txtApePat', false, 'clsInputDisabled');
                    setEnabled('txtApeMat', false, 'clsInputDisabled');
                    habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', false);

                    // Consultar LC Disponible x Producto
                    consultarLCDisponible();
                }
            }
            else if (strRespuesta == '<%= ConfigurationManager.AppSettings["constRespDataCredTipo1"] %>') //13
            {
                //Respuesta Datos del Cliente DC
                document.getElementById('txtApePat').value = strApePaterno;
                document.getElementById('txtApeMat').value = strApeMaterno;
                document.getElementById('txtNombre').value = strNombres;
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', false);

                //Campos Editable Carnet Extranjeria
                if (getValue('ddlTipoDocumento') == '<%= ConfigurationManager.AppSettings["constTipoDocumentoCEX"] %>') {
                    setEnabled('txtNombre', true, 'clsInputEnabled');
                    setEnabled('txtApePat', true, 'clsInputEnabled');
                    setEnabled('txtApeMat', true, 'clsInputEnabled');
                }
                else {
                    setEnabled('txtNombre', false, 'clsInputDisabled');
                    setEnabled('txtApePat', false, 'clsInputDisabled');
                    setEnabled('txtApeMat', false, 'clsInputDisabled');
                }

                // Consultar LC Disponible x Producto
                consultarLCDisponible();
            }
        }

        function mostrarSecPendienteIfr(pstrNroSec) {
            cargarImagenEsperando();

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'strNroSec=' + pstrNroSec;
            url += '&strFlujo=' + obtenerFlujo();
            url += '&strTipoDocumento=' + getValue('ddlTipoDocumento');
            url += '&strNroDoc=' + getValue('txtNroDoc');
            url += '&strOficina=' + getValue('hidOficina');
            url += '&strRiesgo=' + getValue('hidRiesgoDC');
            url += '&strOrgVenta=' + getValue('hidOrgVenta');
            url += '&strCentro=' + getValue('hidCentro');
            url += '&strTipoDocVentaSap=' + getValue('hidTipoDocVentaSap');
            url += '&strCanalSap=' + getValue('hidCanalSap');
            url += '&strTipoOficina=' + getValue('ddlCanal');
            url += '&strTipoOperacion=' + getValue('ddlTipoOperacion');
            url += '&strMetodo=' + 'MostrarSecPendiente';

            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function mostrarSecPendiente(strValor) {
            setValue('hidCadenaSECPendiente', strValor);
            self.frames["ifraCondicionesVenta"].window.location.reload();
        }

        function asignarTipoOperacion() {
            var strCanal = getValue('ddlCanal');
            var arrTipoOperacion = getValue('hidListaTipoOperacion').split('|');
            var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');
            var strDatos = '';
            for (var i = 0; i < arrTipoOperacion.length; i++) {
                var arrTipo = arrTipoOperacion[i].split(';');
                if (arrTipo[0] == strCanal && arrTipo[1] == getValue('ddlTipoDocumento')) {
                    if (getValue('hidTienePortabilidad') == 'S') {
                        if (arrTipo[2] == constTipoOperAlta)
                            strDatos = strDatos + '|' + arrTipo[2] + ';' + arrTipo[3];
                    }
                    else {
                        strDatos = strDatos + '|' + arrTipo[2] + ';' + arrTipo[3];
                    }
                }
            }
            llenarDatosCombo(ddlTipoOperacion, strDatos, false);
            setValue('hidTipoOperacion', ddlTipoOperacion.value);
			
            if (document.getElementById('divClaroPuntos') != null) {
			if (strCanal != constRoamingCorner) {
                    document.getElementById('divClaroPuntos').style.display = 'inline';
            }
            else {
                document.getElementById('divClaroPuntos').style.display = 'none';
            }
        }
        }

        function consultarLCDisponible() {
            cargarImagenEsperando();
            PageMethods.consultaLCDisponible(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), consultarLCDisponible_Callback);
        }

        function consultarLCDisponible_Callback(objResponse) {
            quitarImagenEsperando();
            if (objResponse.Error) {
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
                alert(objResponse.Mensaje);
                return;
            }

            trCondicionVenta.style.display = '';
            trCondicionVentaDetalle.style.display = '';

            if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                trRepresentante.style.display = '';
            }

            // SEC Pendiente
            if (getValue('hidNroSEC') != '' && getValue('hidEvaluarSoloFijo') == '')
                mostrarSecPendienteIfr(getValue('hidNroSEC'));

            // Validación Modalidad / Operador Cedente
            if (getValue('hidTienePortabilidad') == 'S') {
                document.getElementById('tdModalidad').style.display = '';
                document.getElementById('tdOperadorCedente').style.display = '';
            }
        }

        function obtenerFlujo() {
            var strTienePortabilidad = document.getElementById('hidTienePortabilidad').value;
            var strFlujo = '<%= ConfigurationManager.AppSettings["flujoAlta"] %>';

            if (strTienePortabilidad == 'S')
                strFlujo = '<%= ConfigurationManager.AppSettings["flujoPortabilidad"] %>';

            return strFlujo;
        }

        function cambiarTipoOperacion(ddl) {
            if (getValue('hidCadenaDetalle') != '') {
                if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
                    ddl.value = getValue('hidTipoOperacion');
                    return;
                }
            }
            setValue('hidTipoOperacion', ddl.value);

            document.getElementById('ddlOferta').selectedIndex = 0;
            document.getElementById('ddlCasoEspecial').selectedIndex = 0;
            document.getElementById('ddlCombo').selectedIndex = 0;
            document.getElementById('ddlModalidadVenta').selectedIndex = 0;
            inicializarDatosCasoEspecial();
            inicializarPanelCondicionVentaII();

            cambiarTipoOferta(document.getElementById('ddlOferta'));
        }

        function cambiarTipoOferta(ddl) {
            if (getValue('hidCadenaDetalle') != '') {
                if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
                    ddl.value = getValue('hidTipoOferta');
                    return;
                }
            }
            setValue('hidTipoOferta', ddl.value);

            document.getElementById('ddlCasoEspecial').selectedIndex = 0;
            document.getElementById('ddlCombo').selectedIndex = 0;
            document.getElementById('ddlModalidadVenta').selectedIndex = 0;
            inicializarDatosCasoEspecial();
            inicializarPanelCondicionVentaII();

            if (ddl.value == '') {
                llenarDatosCombo(document.getElementById('ddlCasoEspecial'), '', true);
                llenarDatosCombo(document.getElementById('ddlCombo'), '', true);
                self.frames["ifraCondicionesVenta"].window.location.reload();
            } else {
                cargarImagenEsperando();
                PageMethods.cambiarTipoOferta(getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'), 
                                              getValue('txtNroDoc'), getValue('hidOficina'), getValue('ddlModalidadVenta'), cambiarTipoOferta_Callback);
            }
        }

        function cambiarTipoOferta_Callback(objResponse) {
            quitarImagenEsperando();
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }

            var arrResultado = objResponse.Cadena.split('¬');
            var listaTipoProducto = arrResultado[0];
            var listaCasoEspecial = arrResultado[1];
            var listaCombo = arrResultado[2];

            setValue('hidListaTipoProducto', listaTipoProducto);

            var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
            llenarDatosCombo(ddlCasoEspecial, listaCasoEspecial, true);

            var ddlCombo = document.getElementById('ddlCombo');
            llenarDatosCombo(ddlCombo, listaCombo, true);

            self.frames["ifraCondicionesVenta"].window.location.reload();
        }

        function cambiarCasoEspecial(ddl) {
            if (getValue('hidCadenaDetalle') != '') {
                if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
                    setValue('ddlCasoEspecial', getValue('hidCasoEspecial'));
                    return;
                }
            }

            document.getElementById('ddlCombo').selectedIndex = 0;
            document.getElementById('ddlModalidadVenta').selectedIndex = 0;
            inicializarDatosCasoEspecial();
            inicializarPanelCondicionVentaII();

            cargarImagenEsperando();

            var strCasoEspecial = getValor(ddl.value, 0);
            var strWhiteList = '';

            if (strCasoEspecial != '') strWhiteList = getValor(ddl.value, 1);

            PageMethods.cambiarCasoEspecial(getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                            getValue('txtNroDoc'), strCasoEspecial, strWhiteList, getValue('ddlModalidadVenta'), cambiarCasoEspecial_Callback);
        }

        function cambiarCasoEspecial_Callback(objResponse) {
            quitarImagenEsperando();
            autoSizeIframe();

            setValue('hidListaTipoProducto', objResponse.LISTA_PRODUCTOS);

            var blnWhiteList = objResponse.FLAG_WHITELIST;
            var dblCFMaximo = objResponse.CARGO_FIJO_MAX;
            var listaCEPlanBscs = objResponse.PLANES_BSCS;
            var listaCEPlan = objResponse.PLANES_SISACT;
            var listaCEPlanxProd = objResponse.PLANES_X_PRODUCTO;
            var listaCENroPlanxProd = objResponse.NRO_PLANES_X_PRODUCTO;

            var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
            var whiteList = getValor(ddlCasoEspecial.value, 1);
            if (whiteList == 'S') {
                if (blnWhiteList != 'S') {
                    alert('El Nro de documento no se encuentra en la lista del caso especial seleccionado.');
                    inicializarDatosCasoEspecial();
                    ddlCasoEspecial.value = '';
                    self.frames["ifraCondicionesVenta"].window.location.reload();
                    return;
                }
            }

            setValue('hidCasoEspecial', ddlCasoEspecial.value);
            setValue('hidWhitelistCE', blnWhiteList);
            setValue('hidCargoFijoMaxCE', dblCFMaximo);
            setValue('hidlistaCEPlanBscs', listaCEPlanBscs);
            setValue('hidlistaCEPlan', listaCEPlan);
            setValue('hidlistaCEPlanxProd', listaCEPlanxProd);
            setValue('hidlistaCENroPlanxProd', listaCENroPlanxProd);

            self.frames["ifraCondicionesVenta"].window.location.reload();
        }

        function cambiarCombo(ddl) {
            if (getValue('hidCadenaDetalle') != '') {
                if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
                    ddl.value = getValue('hidCombo');
                    return;
                }
            }
            setValue('hidCombo', ddl.value);

            document.getElementById('ddlCasoEspecial').selectedIndex = 0;
            inicializarDatosCasoEspecial();
            inicializarPanelCondicionVentaII();

            if (ddl.value.length > 0) setEnabled('btnAgregarPlan', false, '');

            self.frames["ifraCondicionesVenta"].window.location.reload();
        }

        function cambiarModalidadVenta(ddl) {
            if (getValue('hidCadenaDetalle') != '') {
                if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
                    ddl.value = getValue('hidModalidadVenta');
                    return;
                }
            }
            setValue('hidModalidadVenta', ddl.value);

            inicializarPanelCondicionVentaII();

            PageMethods.cambiarTipoModalidad(getValue('hidOficina'), getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                             getValue('txtNroDoc'), getValor(getValue('ddlCasoEspecial'), 0), getValue('ddlModalidadVenta'), cambiarTipoModalidad_Callback);
        }

        function cambiarTipoModalidad_Callback(objResponse) {
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }

            var arrResultado = objResponse.Cadena.split('¬');
            var listaTipoProducto = arrResultado[0];
            var listaCombo = arrResultado[1];

            setValue('hidListaTipoProducto', listaTipoProducto);
            
            var ddlCombo = document.getElementById('ddlCombo');
            llenarDatosCombo(ddlCombo, listaCombo, true);

            self.frames["ifraCondicionesVenta"].window.location.reload();
        }

        // ******************************************************* EVALUAR ******************************************************* //
        function validarEvaluacion() {
            cargarImagenEsperando();

            // Datos Cliente
            if (!validarDatosCliente()) {
                quitarImagenEsperando(); return false;
            } 
            
            // Representante Legal
            if (!validarDatosRRLL()) {
                quitarImagenEsperando(); return false;
            }

            var ifr = self.frames['ifraCondicionesVenta'];
            var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');

            // Condiciones de Venta
            var tabla = ifr.document.getElementById('tblTabla' + ifr.getValue('hidTipoProductoActual'));
            var nrofila = tabla.rows.length;
            if (nrofila == 0) {
                alert('Debe agregar al menos un plan.');
                quitarImagenEsperando();
                return false;
            }

            // Modalidad / Operador Cedente
            if (getValue('hidTienePortabilidad') == 'S') {
                if (!validarControl('ddlModalidadPorta', '', 'Seleccione la Modalidad.')) {
                    quitarImagenEsperando(); return false;
                }
                if (!validarControl('ddlOperadorCedente', '', 'Seleccione el Operador Cedente.')) {
                    quitarImagenEsperando(); return false;
                }
            }

            if (!ifr.verificarPlanes()) {
                quitarImagenEsperando(); return false;
            }

            // Cobertura Servicio VOD
            if (codTipoProductoActual == codTipoProductoHFC) {
               if (!ifr.verificarVOD(codTipoProductoActual)) {
                    quitarImagenEsperando(); return false;
                }
            }

            // Tipo Operación Migración
            if (getValue('ddlTipoOperacion') == constTipoOperMigracion) {
                var nroPlanes = 0;
                if ((codTipoProductoActual != codTipoProductoHFC) && (codTipoProductoActual != codTipoProd3PlayInalam)) {
                    nroPlanes = nroPlanesEvaluados('ALL', 'ALL');
                } else {
                    if (codTipoProductoActual == codTipoProd3PlayInalam) {
                        nroPlanes = nroPaqEvaluadosHFCI();
                    } else {
                    nroPlanes = nroPaqEvaluadosHFC();
                }
                    
                }
                if (nroPlanes > 1) {
                    alert('Solo se puede evaluar 1 Plan para el tipo de operación Migración.');
                    quitarImagenEsperando(); return false;
                }
            }

            // Bolsa Compartida - Planes Conexion Plus
            if (!validarBolsaCompartidaII()) {
                alert('<%= ConfigurationManager.AppSettings["constMsjBolsaCompartidaII"] %>');
                quitarImagenEsperando(); return false;
            }

            // Caso Especial
            if (getValue('ddlCasoEspecial') != '') {
                if (!validarCargoFijoMaxCE()) {
                    alert('Cargo fijo evaluado supera el cargo fijo máximo autorizado por Créditos.');
                    quitarImagenEsperando(); return false;
                }

                if (!validarNroPlanesMaxCE('')) {
                    alert('La suma Total de Planes excede al permitido.');
                    quitarImagenEsperando(); return false;
                }

                if (!validarNroPlanesMaxGeneralCE('')) {
                    alert('La suma Total de Planes excede al permitido.');
                    quitarImagenEsperando(); return false;
                }

                if (!validar4Play()) {
                    quitarImagenEsperando(); return false;
                }
            }
            else {
                // Cambio Titularidad DTH
                if (!validarTitularidadDTH()) {
                    alert('Producto por cambio de titularidad se debe evaluar de forma individual.');
                    quitarImagenEsperando(); return false;
                }

                // Planes Máximo x Producto
                if (!nroPlanesEvalxProducto()) {
                    alert('La suma total de planes por tipo de producto excede al maximo permitido por la SEC.');
                    quitarImagenEsperando(); return false;
                }
            }

            quitarImagenEsperando();

            // Validación Nro. Teléfono
            if (getValue('ddlTipoOperacion') == constTipoOperMigracion || getValue('hidTienePortabilidad') == 'S') {
                validarNroTelefono(codTipoProductoActual);
            }
            else {
                consultaReglasCreditos();
            }

            return true;
        }

        function validarNroTelefono(codTipoProductoActual) {
            var listaTelefono = listaTelefonosEvaluados();

            if (getValue('hidTienePortabilidad') == 'S') {
                if (!validarNroProductosPorta(codTipoProductoActual)) {
                    alert('Solo se puede evaluar 1 Tipo de Producto para el flujo de Portabilidad.');
                    return;
                }
            }
            var tipoDocumento = $("option:selected", $("#ddlTipoDocumento")).text();

            PageMethods.validarNroTelefonoXCliente(tipoDocumento, getValue('txtNroDoc'), listaTelefono, getValue('hidTienePortabilidad'), getValue('ddlTipoOperacion'), validarNroTelefonoXCliente_Callback);
            cargarImagenEsperando();
            //PageMethods.validarNroTelefono(getValue('txtNroDoc'), listaTelefono, getValue('hidTienePortabilidad'), getValue('ddlTipoOperacion'), validarNroTelefono_Callback);
        }

        function validarNroTelefonoXCliente_Callback(objResponse) {
            quitarImagenEsperando();
            if (objResponse.Error) {
                var v_cadenaindice = objResponse.Mensaje.split('|');
                var v_mensaje = v_cadenaindice[0];
                var v_indice = v_cadenaindice[1];
                var v_id = document.getElementById('ifraCondicionesVenta');
                var v_name = v_id.contentDocument || v_id.contentWindow.document;
                validaNumeroMigracion = true;
                alert(v_mensaje);
                return;
            }
            else {
                validaNumeroMigracion = false;
                var listaTelefono = listaTelefonosEvaluados();
                PageMethods.validarNroTelefono(getValue('txtNroDoc'), listaTelefono, getValue('hidTienePortabilidad'), getValue('ddlTipoOperacion'), validarNroTelefono_Callback);
            }
        }

        function validarNroTelefono_Callback(objResponse) {
            quitarImagenEsperando();
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }
            else {
                consultaReglasCreditos();
            }
        }

        // Listar Operador Cedente Movil/Fija
        function llenarOperadorCedente() {
            var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');

            var tipoPortabilidad = validarTipoPortabilidad(codTipoProductoActual);
            PageMethods.consultaOperadorCedente(tipoPortabilidad, consultaOperadorCedente_Callback);
        }

        function consultaOperadorCedente_Callback(objResponse) {
            llenarCombo(document.getElementById('ddlOperadorCedente'), objResponse.Cadena, true);
        }

        function agregarCarrito() {
            if (!validarEvaluacion()) {
                return;
            }
        }

        function consultaReglasCreditos() {
            var strCadenaDetalle = '';
            if (self.frames['ifraCondicionesVenta'].getValue('hidFlgOrigen') == 'EDIT')
                strCadenaDetalle = self.frames['ifraCondicionesVenta'].getValue('hidCadenaDetalle');
            else
                strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');

            var strCadenaPlan = cadenaPlanesDetalle(strCadenaDetalle);
            var strCadenaEquipo = cadenaEquiposDetalle(strCadenaDetalle);
            var strCadenaServicio = cadenaServiciosDetalle(strCadenaDetalle);
            var strCadenaDatos = cadenaGeneral();

            cargarImagenEsperando();
            PageMethods.consultaReglasCreditos(getValue('txtNroDoc'), getValue('hidNroOperacionDC'), strCadenaDatos, strCadenaPlan, strCadenaServicio, strCadenaEquipo, asignarDatosEvaluacion);
        }

        function consultaReglasCreditosCuotas(idFila, strCadenaDetalle) {
            var strCadenaPlan = cadenaPlanesDetalle(strCadenaDetalle);
            var strCadenaEquipo = cadenaEquiposDetalle(strCadenaDetalle);
            var strCadenaDatos = cadenaGeneral();

            cargarImagenEsperando();
            PageMethods.consultaCuota(idFila, getValue('txtNroDoc'), getValue('hidNroOperacionDC'), strCadenaDatos, strCadenaPlan, strCadenaEquipo, asignarDatosCuotas);
        }

        function asignarDatosCuotas(objResponse) {
            quitarImagenEsperando();
            if (objResponse.Cadena == '') {
                alert('<%= ConfigurationManager.AppSettings["consMsjNoConfiguracionCuotas"] %>');
                self.frames['ifraCondicionesVenta'].document.getElementById('tblCuotas').style.display = 'none';
                return;
            }
            self.frames['ifraCondicionesVenta'].llenarDatosCuota(objResponse.IdFila, objResponse.Cadena);
        }

        function asignarDatosEvaluacion(objResponse) {

            inicializarPanelResultado();
            trComentario.style.display = 'none';
            inicializarPanelGrabar();
            quitarImagenEsperando();

            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }

            var arrParam = objResponse.Cadena.split('#');
            strPlanAutonomia = arrParam[0];
            tipoGarantia = arrParam[1];
            strLCDisponible = arrParam[2];
            dblImporte = arrParam[3];
            strPoderes = arrParam[4];
            strTextoLCDisponible = arrParam[5];
            strRiesgoClaro = arrParam[6];
            strComportamiento = arrParam[7];
            strExoneracionRA = arrParam[8];

            // Validación Modalidad / Operador Cedente
            if (getValue('hidTienePortabilidad') == 'S') {
                setEnabled('ddlModalidadPorta', false, '');
                setEnabled('ddlOperadorCedente', false, '');
            }

            var ifr = self.frames['ifraCondicionesVenta'];

            if (ifr.getValue('hidFlgOrigen') == 'EDIT') {
                ifr.setValue('hidFlgOrigen', '');
            }
            else {
                ifr.guardarItem();
                ifr.agregarCarrito(true);
            }

            autoSizeIframe();

            var blnTipoDocRUC = (getValue('ddlTipoDocumento') == constTipoDocumentoRUC);
            var strCasoEspecial = document.getElementById('ddlCasoEspecial').value;
            var blnAutonomiaCE = true;

            document.getElementById('txtRangoLC').value = strTextoLCDisponible;
            document.getElementById('txtRiesgoClaro').value = strRiesgoClaro;
            document.getElementById('txtComportamiento').value = strComportamiento;

            // Autonomia
            var strAutonomia = 'S';
            var arrAutonomia = strPlanAutonomia.split('|');
            for (var i = 0; i < arrAutonomia.length; i++) {
                if (arrAutonomia[i] != '') {
                    if (arrAutonomia[i].split(';')[1] == 'NO_CONDICION') {
                        // No puede acceder a las Condiciones de Venta seleccionadas
                        trResultado.style.display = '';
                        tdLCDisponible.style.display = 'none';
                        tdTxtLCDisponible.style.display = 'none';
                        setValue('txtResultado', '<%= ConfigurationManager.AppSettings["constTextoNoAplicaCondiciones"] %>');
                        return;
                    }
                    else if ((arrAutonomia[i].split(';')[1] == 'SIN_REGLAS')) {
                        setValue('hidCreditosxReglas', 'S'); // NO CUMPLE AUTONOMIA: MOTIVO --> No Existe Reglas Configuradas
                        strAutonomia = 'N';
                    }
                    else if ((arrAutonomia[i].split(';')[1] == 'N'))
                        strAutonomia = 'N';
                }
            }

            // Controles Resultado Evaluación
            document.getElementById('hidAutonomia').value = strAutonomia;
            document.getElementById('txtTipoGarantia').value = tipoGarantia;
            document.getElementById('txtImporte').value = parseFloat(dblImporte).toFixed(2);
            document.getElementById('txtLCDisponible').value = parseFloat(strLCDisponible).toFixed(2);
            document.getElementById('hidLCDisponible').value = strLCDisponible;
            document.getElementById('hidPoderes').value = strPoderes;
            document.getElementById('txtRangoLC').value = strTextoLCDisponible;
            document.getElementById('hidResultadoReglas').value = strPlanAutonomia
            document.getElementById('txtRiesgoClaro').value = strRiesgoClaro;
            document.getElementById('txtComportamiento').value = strComportamiento;

            document.getElementById('hidRiesgoClaro').value = strRiesgoClaro;
            document.getElementById('hidComportamiento').value = strComportamiento;
            document.getElementById('hidExoneracionRA').value = strExoneracionRA;

            // Controles visibles solo para el perfil de Créditos
            if (getValue('hidPerfilCreditos') == 'S') {
                tdLCDisponible.style.display = '';
                tdTxtLCDisponible.style.display = '';
            }

            trResultado.style.display = '';
            trGrabar.style.display = '';
            if (getValue('hidTienePortabilidad') == 'S')
                trAdjuntoPorta.style.display = '';

            // NO CUMPLE AUTONOMIA: MOTIVO --> Nro Planes Caso Especial
            if (strCasoEspecial != '') {
                blnAutonomiaCE = validarNroPlanesMaxGeneralCE('1');
                if (blnAutonomiaCE) {
                    blnAutonomiaCE = validarNroPlanesMaxCE('1');
                }

                if (!blnAutonomiaCE)
                    setValue('hidCreditosxCE', 'S');
            }

            // NO CUMPLE AUTONOMIA: MOTIVO --> Motivo Desactivación de Líneas
            var blnAutonomiaMotivo = autonomiaDesactivaLineas();

            // Cambiar Luces Carrito según autonomia
            ifr.cambiarLucesCarrito(strPlanAutonomia);

            if (blnTipoDocRUC) {
                if (getValue('hidAutonomia') == 'S' && blnAutonomiaCE && blnAutonomiaMotivo) {
                    trPresentaPoderes.style.display = '';
                    trGarantia.style.display = '';
                    if (document.getElementById('hidPoderes').value == '1')
                        document.getElementById('chkPresentaPoderes').checked = true;

                    setValue('txtResultado', '<%= ConfigurationManager.AppSettings["constTextoAprobadoAutonomia"] %>');
                }
                else {
                    setValue('txtResultado', '<%= ConfigurationManager.AppSettings["constTextoNoAprobadoAutonomia"] %>');
                    trComentario.style.display = '';
                    trGarantia.style.display = 'none';
                }
            }
            else {
                //NO CUMPLE AUTONOMIA: MOTIVO --> Respuesta Tipo 7 y NO tiene excepción DC7
                var blnRespuestaDC7 = (getValue('hidRespuestaDC') == '<%= ConfigurationManager.AppSettings["constRespDataCredTipo7"] %>');
                var blnAutonomiaDC7 = (!blnRespuestaDC7 || (consultaExcepcionDC7() && blnRespuestaDC7));
                if (!blnAutonomiaDC7)
                    setValue('hidCreditosxDC7', 'S');

                if ((blnAutonomiaDC7) && (getValue('hidAutonomia') == 'S') && (blnAutonomiaCE)) {
                    trGarantia.style.display = '';
                    setValue('txtResultado', '<%= ConfigurationManager.AppSettings["constTextoAprobadoAutonomia"] %>');
                } else {
                    trGarantia.style.display = 'none';
                    trComentario.style.display = '';
                    setValue('txtResultado', '<%= ConfigurationManager.AppSettings["constTextoNoAprobadoAutonomia"] %>');
                }
            }

            if (getValue('txtResultado') == '<%= ConfigurationManager.AppSettings["constTextoAprobadoAutonomia"] %>' && dblImporte > 0) {
                document.getElementById('btnDetalleGarantia').style.display = '';
                document.getElementById('btnEnviarCreditos').style.display = '';
            }
        }

        // ******************************************************* EVALUAR ******************************************************* //

        // ******************************************************* GRABAR ******************************************************* //
        function validarGrabar() {
            // Datos Cliente
            if (!validarDatosCliente()) return false;
            // Representante Legal
            if (!validarDatosRRLL()) return false;
            // Planes pendientes de Agregar Carrito
            if (!validarPlanesNoCarrito()) return false;
            // Documentación Portabilidad
            if (getValue('hidTienePortabilidad') == 'S') {
                if (!validarPortabilidad()) return false;
            }
            // Oferta Corporativa
            if (!validarOfertaCorporativa()) return false;
            // Oferta 4Play
            if (!validar4Play()) return false;
            // Oferta Combo
            if (!validarPlanesCombo()) return false;
            //gaa20151204
            if (!self.frames["ifraCondicionesVenta"].validarPlanesRestringidosCombo()) return false;
            //fin gaa20151204
            // Grabar Info
            setValue('hidTipoOperacion', getValue('ddlTipoOperacion'));
            setValue('hidCombo', getValue('ddlCombo'));
            setValue('hidComboText', obtenerTextoSeleccionado(document.getElementById('ddlCombo')));
            setValue('hidModalidadVenta', getValue('ddlModalidadVenta'));
            setValue('hidCasoEspecial', getValue('ddlCasoEspecial'));

            return true;
        }

        function grabar() {
            setValue('hidAdjuntarIngreso', '');
            setValue('hidCreditosxNombres', '');
            setValue('hidGrupoPaqueteServer', '');
            setValue('hidServicioServer', '');
            setValue('hidPromocionServer', '');
            setValue('hidEquipoServer', '');

            habilitarBoton('btnGrabar', 'PROCESANDO...', false);

            if (!validarGrabar()) {
                habilitarBoton('btnGrabar', 'Grabar', true);
                return;
            }

            //gaa20151109
            var ifr = self.frames['ifraCondicionesVenta'];

            if (ifr.validarEquipoSinStock() > 0) {
                var strMensaje = '<%= ConfigurationManager.AppSettings["constMsjEquipoSinStock"] %>';
                if (!confirm(strMensaje)) {
                    return;
                }
            }
            //fin gaa20151109

            if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
                // Excepción Respuesta Tipo 7 DataCrédito - Siempre debe mostrarse este mensaje
                if (!confirm("VERIFIQUE LOS NOMBRES Y APELLIDOS DEL CLIENTE. Si son correctos presione Sí, caso contrario la SEC irá a Créditos para validación y corrección respectiva."))
                    document.getElementById('hidCreditosxNombres').value = 'S';

                // NO CUMPLE AUTONOMIA
                if ((getValue('hidCreditosxDC7') == 'S') || (getValue('hidAutonomia') != 'S')) {
                    if (!validarSoloPlanesFijo()) {
                        if (confirm("SEC no cumple con autonomia, cliente debe ser evaluado por Créditos. ¿Desea adicionalmente adjuntar sustentos de ingreso?"))
                            document.getElementById('hidAdjuntarIngreso').value = 'S';
                    }
                }
            }

            if (confirm("Esta Seguro de Grabar la Evaluacion?")) {
                //Guardar Info
                setValue('hidNombre', getValue('txtNombre'));
                setValue('hidApePaterno', getValue('txtApePat'));
                setValue('hidApeMaterno', getValue('txtApeMat'));
                setValue('hidRazonSocial', getValue('txtRazonSocial'));
                setValue('hidComentarioPDV', getValue('txtComentarioPDV'));
                setValue('hidMensajeAutonomia', getValue('txtResultado'));

                setValue('hidGrupoPaqueteServer', self.frames['ifraCondicionesVenta'].getValue('hidGrupoPaquete'));
                setValue('hidServicioServer', self.frames['ifraCondicionesVenta'].getValue('hidPlanServicio'));
                setValue('hidPromocionServer', self.frames['ifraCondicionesVenta'].getValue('hidPromocion'));
                setValue('hidEquipoServer', self.frames['ifraCondicionesVenta'].getValue('hidEquiposXfila3Play'));

                if (getValue('hidTienePortabilidad') == 'S' || getValue('ddlTipoDocumento') == constTipoDocumentoRUC || getValue('hidAdjuntarIngreso') == 'S') {
                    setValue('hidAccion', 'Grabar');
                    frmPrincipal.submit();
                }
                else
                    validarSECRecurrente('Grabar');
            }
            else {
                habilitarBoton('btnGrabar', 'Grabar', true);
                return;
            }
        }

        function enviarCreditos() {
            setValue('hidAdjuntarIngreso', '');
            setValue('hidCreditosxNombres', '');
            setValue('hidGrupoPaqueteServer', '');
            setValue('hidServicioServer', '');
            setValue('hidPromocionServer', '');

            habilitarBoton('btnEnviarCreditos', 'PROCESANDO...', false);

            if (!validarGrabar()) {
                habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', true);
                return;
            }

            //gaa20151109
            var ifr = self.frames['ifraCondicionesVenta'];

            if (ifr.validarEquipoSinStock() > 0) {
                var strMensaje = '<%= ConfigurationManager.AppSettings["constMsjEquipoSinStock"] %>';
                if (!confirm(strMensaje)) {
                    return;
                }
            }
            //fin gaa20151109

            if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                setValue('hidCreditosxAsesor', 'S');
                setValue('hidMensajeAutonomia', getValue('txtResultado'));

                //Guardar Info
                setValue('hidNombre', getValue('txtNombre'));
                setValue('hidApePaterno', getValue('txtApePat'));
                setValue('hidApeMaterno', getValue('txtApeMat'));
                setValue('hidRazonSocial', getValue('txtRazonSocial'));

                setValue('hidGrupoPaqueteServer', self.frames['ifraCondicionesVenta'].getValue('hidGrupoPaquete'));
                setValue('hidServicioServer', self.frames['ifraCondicionesVenta'].getValue('hidPlanServicio'));
                setValue('hidPromocionServer', self.frames['ifraCondicionesVenta'].getValue('hidPromocion'));

                setValue('hidAccion', 'Grabar');
                frmPrincipal.submit();
            }
            else {
                var url = '<%= ConfigurationManager.AppSettings["constPaginaEnviarCreditos"] %>';
				url += "?cu=" + getValue('hidUsuarioRed');
                var opciones = "dialogHeight: 350px; dialogWidth: 650px; edge: Raised; center:Yes; help: No; resizable=no; status: No";
                var vRetorno = window.showModalDialog(url, '', opciones);

                if (vRetorno) {
                    setValue('hidCreditosxAsesor', 'S');
                    setValue('hidArchivosEnvioCreditos', vRetorno.Archivos);
                    setValue('hidComentarioPDV', vRetorno.Comentario);
                    setValue('hidMensajeAutonomia', getValue('txtResultado'));

                    //Guardar Info
                    setValue('hidNombre', getValue('txtNombre'));
                    setValue('hidApePaterno', getValue('txtApePat'));
                    setValue('hidApeMaterno', getValue('txtApeMat'));
                    setValue('hidRazonSocial', getValue('txtRazonSocial'));

                    setValue('hidGrupoPaqueteServer', self.frames['ifraCondicionesVenta'].getValue('hidGrupoPaquete'));
                    setValue('hidServicioServer', self.frames['ifraCondicionesVenta'].getValue('hidPlanServicio'));
                    setValue('hidPromocionServer', self.frames['ifraCondicionesVenta'].getValue('hidPromocion'));

                    if (getValue('hidTienePortabilidad') == 'S' || getValue('hidArchivosEnvioCreditos') != '') {
                        setValue('hidAccion', 'Grabar');
                        frmPrincipal.submit();
                    }
                    else
                        validarSECRecurrente('IrCreditos');
                }
                else {
                    setValue('hidCreditosxAsesor', '');
                    setValue('hidArchivosEnvioCreditos', '');
                    setValue('hidComentarioPDV', '');
                    habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', true);
                    return;
                }
            }
        }

        function validarSECRecurrente(flujo) {
            PageMethods.validarSECRecurrente(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), getValue('ddlOferta'), getValue('ddlCasoEspecial'), cadenaSECRecurrente(), flujo, retornarSECRecurrente);
        }

        function retornarSECRecurrente(objResponse) {
            var valor = objResponse.Cadena;
            var flujo = valor.split('|')[0];
            var nroSEC = valor.split('|')[1];
            var flgReingreso = valor.split('|')[2];

            if (nroSEC == '0') {
                setValue('hidAccion', 'Grabar');
                frmPrincipal.submit();
            }
            else {
                if (flgReingreso != '1') {
                    setValue('hidAccion', 'Grabar');
                    frmPrincipal.submit();
                }
                else {
                    alert('Ya existe una SEC reciente con las mismas condiciones, y ésta ya fue rechazada por Créditos');

                    if (flujo == 'Grabar')
                        habilitarBoton('btnGrabar', 'Grabar', true);
                    else {
                        setValue('hidCreditosxAsesor', '');
                        setValue('hidArchivosEnvioCreditos', '');
                        setValue('hidComentarioPDV', '');
                        habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', true);
                    }
                    return;
                }
            }
        }

        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }

        function verDetalleLinea() {
            var url = '../consultas/sisact_pop_detalle_linea.aspx?';
            url += 'nroDocumento=' + getValue('txtNroDoc') + '&tipoDocumento=' + getValue('ddlTipoDocumento');
            abrirVentana(url, "", '900', '540', '_blank', true);
        }

        function buscarSEC() {
            var url = '../consultas/sisact_pop_consulta_sec.aspx?';
            url += 'nroDocumento=' + getValue('txtNroDoc') + '&tipoDocumento=' + getValue('ddlTipoDocumento');
            abrirVentana(url, "", '800', '250', '_blank', true);
        }

        function verAdjuntarDocumentos(nroSEC) {
            var url = '<%= ConfigurationManager.AppSettings["constPaginaAdjDocumento"] %>';
            url += '?numSolicitud=' + nroSEC;
            abrirVentana(url, '', '800', '600', 'Sustento_de_Ingresos', true);
        }

        function AdjuntarDocumentos(nroSec) {
            var url = '<%= ConfigurationManager.AppSettings["constPaginaAdjDocumentoRuc"] %>';
            url += '?nroSec=' + nroSec + '&llamadoDesde=Evaluacion';
            abrirVentana(url, '', '660', '150', 'Acuerdos', false);
        }

        function verDetalleGarantia() {
            var url = '../consultas/sisact_pop_detalle_garantia.aspx?nroDocumento=' + getValue('txtNroDoc');
            abrirVentana(url, '', '550', '160', '', true);
        }

        function mostrarTab(tipoProducto) {
            self.frames['ifraCondicionesVenta'].f_MostrarTab(tipoProducto);
            llenarOperadorCedente();
        }

        function agregarPlan() {
            var ifr = self.frames['ifraCondicionesVenta'];
            if ((ifr.getValue('hidCodigoTipoProductoActual') == codTipoProductoHFC && getValue('ddlTipoOperacion') == constTipoOperMigracion) || (ifr.getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam && getValue('ddlTipoOperacion') == constTipoOperMigracion)) {
                   if (ifr.getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {
                      self.frames['ifraCondicionesVenta'].agregarFila1(true);
                  } else {
                PageMethods.consultaSOTxMigracion(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), consultaSOTxMigracion_Callback);
            }
                
             }
            else {
                self.frames['ifraCondicionesVenta'].agregarFila1(true);
            }
        }

        function consultaSOTxMigracion_Callback(objResponse) {
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }
            setValue('hidNroSOTMigracion', objResponse.Cadena);
            self.frames['ifraCondicionesVenta'].agregarFila1(true);
        }

        function mostrarTabxOferta() {
            mostrarAllTabProducto(false);
            trTabProducto.style.display = '';
            trLCDisponible.style.display = 'none';
            trCarrito.style.display = '';
            self.frames['ifraCondicionesVenta'].document.getElementById('trGrilla').style.display = '';

            if (getValue('ddlOferta') != '') {
                var strTipoProducto = getValue('hidListaTipoProducto');
                var arrTipoProducto = strTipoProducto.split('|');
                if (strTipoProducto != '') {
                    for (var i = 0; i < arrTipoProducto.length; i++) {
                        var idProducto = arrTipoProducto[i].split(';')[0];
                        mostrarTabProducto(idProducto, true);
                    }

                    if (isVisible('tdMovil')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('Movil'); return; }
                    if (isVisible('tdBAM')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('BAM'); return; }
                    if (isVisible('tdDTH')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('DTH'); return; }
                    if (isVisible('tdHFC')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('HFC'); return; }
                    if (isVisible('tdFijo')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('Fijo'); return; }
                    if (isVisible('tdVentaVarios')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('VentaVarios'); return; }
                    if (isVisible('td3PlayInalam')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('3PlayInalam'); return; }
                }
            }

            trTabProducto.style.display = 'none';
            trLCDisponible.style.display = 'none';
            trCarrito.style.display = 'none';
            self.frames['ifraCondicionesVenta'].document.getElementById('trGrilla').style.display = 'none';
        }

        function nroPlanesEvalxProducto() {
            var nroPlanesEval = 0;
            var nroPlanesEvalTotal = 0;

            //Validacion 4Play
            var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);
            if (strCodCasoEspecial != '') {
                strCodCasoEspecial = '|' + strCodCasoEspecial + '|';
                var listaVentaCE4Play = '<%= ConfigurationManager.AppSettings["constVentaCE4Play"] %>';
                var listaPostventaCE4Play = '<%= ConfigurationManager.AppSettings["constPostVentaCE4Play"] %>';
                if (listaVentaCE4Play.indexOf(strCodCasoEspecial) > -1 || listaPostventaCE4Play.indexOf(strCodCasoEspecial) > -1)
                    return true;
            }

            var nroNroMaxPlanes;
            var nroNroMaxPlanesMovil;
            var nroNroMaxPlanesFijo;
            var nroNroMaxPlanesBAM;
            var nroNroMaxPlanesDTH;
            var nroNroMaxPlanesHFC;
            var nroNroMaxPlanesHFCI;

            if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                nroNroMaxPlanes = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaximoPlanesEmpresa"] %>');
                nroNroMaxPlanesMovil = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesMovilEmpresa"] %>');
                nroNroMaxPlanesFijo = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesFijoEmpresa"] %>');
                nroNroMaxPlanesBAM = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesBAMEmpresa"] %>');
                nroNroMaxPlanesDTH = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesDTHEmpresa"] %>');
                nroNroMaxPlanesHFC = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCEmpresa"] %>');
                nroNroMaxPlanesHFCI = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCIEmpresa"] %>');
            }
            else {
                nroNroMaxPlanes = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaximoPlanesPersona"] %>');
                nroNroMaxPlanesMovil = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesMovilPersona"] %>');
                nroNroMaxPlanesFijo = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesFijoPersona"] %>');
                nroNroMaxPlanesBAM = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesBAMPersona"] %>');
                nroNroMaxPlanesDTH = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesDTHPersona"] %>');
                nroNroMaxPlanesHFC = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCPersona"] %>');
                nroNroMaxPlanesHFCI = getParametroGeneral('<%= ConfigurationManager.AppSettings["constNroMaxPlanesHFCIPersona"] %>');
            }

            nroPlanesEval = parseInt(0) + nroPlanesEvaluados('', codTipoProductoFijo);
            nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
            if (nroPlanesEval > parseInt(nroNroMaxPlanesFijo, 10)) return false;

            //Excepción a Conteo de Planes
            if (!((getValue('ddlOferta') == constTipoOfertaBusiness) && (getValue('ddlTipoDocumento') != constTipoDocumentoRUC))) {

                nroPlanesEval = parseInt(0) + nroPlanesEvaluados('', codTipoProductoMovil);
                nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
                if (nroPlanesEval > parseInt(nroNroMaxPlanesMovil, 10)) return false;

                nroPlanesEval = parseInt(0) + nroPlanesEvaluados('', codTipoProductoBAM);
                nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
                if (nroPlanesEval > parseInt(nroNroMaxPlanesBAM, 10)) return false;
            }

            nroPlanesEval = parseInt(0) + nroPlanesEvaluados('', codTipoProductoDTH);
            nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
            if (nroPlanesEval > parseInt(nroNroMaxPlanesDTH, 10)) return false;

            nroPlanesEval = parseInt(0) + nroPaqEvaluadosHFC();
            nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
            if (nroPlanesEval > parseInt(nroNroMaxPlanesHFC, 10)) return false;

            nroPlanesEval = parseInt(0) + nroPaqEvaluadosHFCI();
            nroPlanesEvalTotal += parseInt(nroPlanesEval, 10);
            if (nroPlanesEval > parseInt(nroNroMaxPlanesHFCI, 10)) return false;
           
            if (nroPlanesEvalTotal > parseInt(nroNroMaxPlanes, 10)) return false;

            return true;
        }

        function validar4Play() {
            var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);
            var listaCasoEspecial4Play = '<%= ConfigurationManager.AppSettings["constVentaCE4Play"] %>';
            if (listaCasoEspecial4Play.indexOf('|' + strCodCasoEspecial + '|') > -1) {
                var planesMovil = '<%= ConfigurationManager.AppSettings["constPlanMovil4Play"] %>';
                var arrPlanMovil = planesMovil.split('|');
                var blnAgregado = false;
                for (var i = 0; i < arrPlanMovil.length; i++) {
                    var planMovil = arrPlanMovil[i];
                    if (nroPlanesEvaluados(planMovil, '') > 0) {
                        blnAgregado = true;
                        break;
                    }
                }
                if (!blnAgregado) {
                    alert('Es necesario seleccionar como mínimo 1 Plan Móvil.');
                    return false;
                }
                var planesFijo = '<%= ConfigurationManager.AppSettings["constExclusionPlanes4Play"] %>';
                var arrPlanFijo = planesFijo.split('|');
                blnAgregado = false;
                for (var i = 0; i < arrPlanFijo.length; i++) {
                    var planFijo = arrPlanFijo[i];
                    if (planFijo != '') {
                        var nroPlanes = parseInt(nroPlanesEvaluados(planFijo, ''), 10);
                        if (nroPlanes > 0) {
                            if (blnAgregado || nroPlanes > 1) {
                                alert("Solo puede agregar 1 Paquete Fijo.");
                                return false;
                            }
                            blnAgregado = true;
                        }
                    }
                }
                if (!blnAgregado) {
                    alert('Es necesario seleccionar un Paquete Fijo.');
                    return false;
                }
            }
            return true;
        }

        function autonomiaDesactivaLineas() {
            var blnAutonomia = true;
            var motivosCreditos = "";
            var nroLineasActivas = parseInt(getValue('hidNroLineas'), 10); //PENDIENTE DESA
            var listaCantMotivoBloqueo = getValue('hidCantidadMotivoBloqueo');
            var topeBloqueoRobo = parseFloat(getParametroGeneral('<%= ConfigurationManager.AppSettings["consTopeBloqueoRobo"] %>'));
            var topeBloqueoASolicitud = parseFloat(getParametroGeneral('<%= ConfigurationManager.AppSettings["consTopeBloqueoASolicitud"] %>'));
            var topeLineaDesactivaMorosidad = parseFloat(getParametroGeneral('<%= ConfigurationManager.AppSettings["consTopeLineaDesactivaMorosidad"] %>'));
            var topeLineaDesactivaMigracion = parseFloat(getParametroGeneral('<%= ConfigurationManager.AppSettings["consTopeLineaDesactivaMigracion"] %>'));

            if (listaCantMotivoBloqueo != '') {
                var arrListaCantMotivoBloqueo = listaCantMotivoBloqueo.split('|');
                for (var i = 0; i < arrListaCantMotivoBloqueo.length; i++) {
                    if (arrListaCantMotivoBloqueo[i] != '') {
                        var strMotivo = arrListaCantMotivoBloqueo[i].split(';')[0];
                        var nroLineas = arrListaCantMotivoBloqueo[i].split(';')[1];

                        if (nroLineas > 0) {
                            if (strMotivo == '<%= ConfigurationManager.AppSettings["consMotivoBloqueoRobo"] %>') {
                                if (parseFloat(nroLineas) >= parseFloat(topeBloqueoRobo * nroLineasActivas / 100)) {
                                    motivosCreditos += '|' + strMotivo;
                                    setValue('hidCreditosxLineaDesactiva', 'S');
                                    blnAutonomia = false;
                                }
                            }
                            if (strMotivo == '<%= ConfigurationManager.AppSettings["consMotivoBloqueoASolicitud"] %>') {
                                if (parseFloat(nroLineas) >= parseFloat(topeBloqueoASolicitud * nroLineasActivas / 100)) {
                                    motivosCreditos += '|' + strMotivo;
                                    setValue('hidCreditosxLineaDesactiva', 'S');
                                    blnAutonomia = false;
                                }
                            }
                            if (strMotivo == '<%= ConfigurationManager.AppSettings["consMotivoLineaDesactivaMorosidad"] %>') {
                                if (parseFloat(nroLineas) >= parseFloat(topeLineaDesactivaMorosidad * nroLineasActivas / 100)) {
                                    motivosCreditos += '|' + strMotivo;
                                    setValue('hidCreditosxLineaDesactiva', 'S');
                                    blnAutonomia = false;
                                }
                            }
                            if (strMotivo == '<%= ConfigurationManager.AppSettings["consMotivoLineaDesactivaMigracion"] %>') {
                                if (parseFloat(nroLineas) >= parseFloat(topeLineaDesactivaMigracion * nroLineasActivas / 100)) {
                                    motivosCreditos += '|' + strMotivo;
                                    setValue('hidCreditosxLineaDesactiva', 'S');
                                    blnAutonomia = false;
                                }
                            }
                        }
                    }
                }
            }

            setValue('hidMotivoxLineaDesactiva', motivosCreditos);
            return blnAutonomia;
        }

        function consultaExcepcionDC7() {
            var strCanal = getValue('ddlCanal');
            var codParametroCanal = '<%= ConfigurationManager.AppSettings["COD_GRUPO_CANAL_NO_ERROR_TIPO_7"] %>';
            var strCanalExcepcionDC7 = getParametroGeneral(codParametroCanal);

            var blnExcepcionDC7 = (strCanalExcepcionDC7.indexOf(strCanal) > -1);

            var codParametroDoc = '<%= ConfigurationManager.AppSettings["COD_GRUPO_DOC_NO_ERROR_TIPO_7"] %>';
            var strTipoDocExcepcionDC7 = getParametroGeneral(codParametroDoc);
            var strTipoDocClie = getValue('ddlTipoDocumento');

            if (blnExcepcionDC7 && (strTipoDocExcepcionDC7.indexOf(strTipoDocClie) == -1))
                blnExcepcionDC7 = false;

            return blnExcepcionDC7;
        }


        function abrirModalCC() {           
            var strTipoDocClie = getValue('ddlTipoDocumento');
            var strNroDocClie = getValue('txtNroDoc');
            var url = 'sisact_pop_claropuntos.aspx?';
            url += 'tipo=' + 1 + '&strTipoDoc=' + strTipoDocClie +'&strNumDoc=' + strNroDocClie + '&strTelefono=';
            abrirVentana(url, "", '800', '350', '_blank', true);            
        }

        //gaa20151201
        function validarSecReno() {
            PageMethods.validarSecReno(getValue('txtNroDoc'), validarSecReno_Callback);
        }

        function validarSecReno_Callback(objResponse) {
            var strCadena = objResponse.Cadena;

            if (strCadena != null) {
                var arrCadena = strCadena.split(';');
                var ddlOferta = document.getElementById('ddlOferta');
                var ddlModalidadVenta = document.getElementById('ddlModalidadVenta');
                var ddlCombo = document.getElementById('ddlCombo');

                setValue('hidTipoOperacion1', arrCadena[0]);
                ddlOferta.value = arrCadena[1];
                ddlModalidadVenta.value = arrCadena[2];
                setValue('hidTipoOferta', ddlOferta.value);
                setValue('hidModalidadVenta', ddlModalidadVenta.value);
                document.getElementById('ddlCasoEspecial').selectedIndex = 0;
                setValue('hidCombo1', arrCadena[3]);

                setValue('hidPlazoReno', arrCadena[5]);
                setValue('hidPlanReno', arrCadena[6]);

                inicializarDatosCasoEspecial();
                inicializarPanelCondicionVentaII();

                consultarDataCredito();

                PageMethods.traerDatosReno(getValue('hidOficina'), arrCadena[0], getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                             getValue('txtNroDoc'), getValor(getValue('ddlCasoEspecial'), 0), getValue('ddlModalidadVenta'), traerDatosReno_Callback);

            }
            else {
                if (getValue('hidTienePortabilidad') == 'S')
                    retornarConsultaSEC('');

                if (getValue('hidBuscarSEC') == 'S')
                    buscarSEC();
                else
                    retornarConsultaSEC();
            }
        }

        function traerDatosReno_Callback(objResponse) {
            var strCadena = objResponse.Cadena;

            if (strCadena.length > 0) {
                var arrResultado = objResponse.Cadena.split('¬');
                setValue('hidListaTipoOperacion', arrResultado[0]);
                var listaTipoProducto = arrResultado[1];
                var listaCombo = arrResultado[2];

                asignarTipoOperacion();

                var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');
                ddlTipoOperacion.value = getValue('hidTipoOperacion1');

                setValue('hidListaTipoProducto', listaTipoProducto);

                mostrarTabxOferta();

                var ddlCombo = document.getElementById('ddlCombo');
                llenarDatosCombo(ddlCombo, listaCombo, true);

                ddlCombo.value = getValue('hidCombo1');
                setValue('hidCombo', ddlCombo.value);
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', false);
                if (ddlCombo.value.length > 0) setEnabled('btnAgregarPlan', false, '');

                trDetalleCliente.style.display = '';
                trConsultarDC.style.display = '';
                trCondicionVenta.style.display = '';
                trCondicionVentaDetalle.style.display = '';

                if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
                    trRepresentante.style.display = '';
                }

                document.getElementById('ddlOferta').disabled = true;
                document.getElementById('ddlModalidadVenta').disabled = true;
                document.getElementById('ddlCasoEspecial').disabled = true;
                ddlTipoOperacion.disabled = true;
                ddlCombo.disabled = true;

                self.frames["ifraCondicionesVenta"].window.location.reload();
            }
        }
        //fin gaa20151201
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
							<td class="Arial10B" width="125">&nbsp;Tipo Documento:</td>
							<td width="170"><asp:dropdownlist id="ddlTipoDocumento" runat="server" onChange="cambiarTipoDocumento();mostrarVendedor()"
									Width="130px" CssClass="clsSelectEnableC">
									<asp:ListItem Value="01">DNI</asp:ListItem>
									<asp:ListItem Value="04">CE</asp:ListItem>
									<asp:ListItem Value="06">RUC</asp:ListItem>
								</asp:dropdownlist></td>
							<td class="Arial10B" width="100">Nro. Documento:</td>
							<td width="160"><input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789')" id="txtNroDoc"
									maxLength="11" size="22" />
							</td>
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
							<td colspan="3"><asp:checkbox id="chkPortabilidad" onclick="cambiarPortabilidad(this);" runat="server"></asp:checkbox></td>
							<td align="right" colspan="2"><label class="Arial10BRed" id="lblMensajeDeudaBloqueo"></label><input class="Boton" id="btnDetalleLinea" style="DISPLAY: none; WIDTH: 120px; CURSOR: hand"
									onclick="verDetalleLinea();" type="button" value="Ver Detalle Líneas" />&nbsp;</td>
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
							<td width="275"><input id="txtRazonSocial" class="clsInputEnabled" onkeypress="validaCaracteresNombres();" onpaste="return false;"
									        maxLength="40" size="80" /></td>
							<td colspan="4"></td>
						</tr>
						<tr id="trDetalleDNI">
							<td class="Arial10B" width="125">&nbsp;Nombres:</td>
							<td width="250"><input id="txtNombre" class="clsInputEnabled" onkeypress="validaCaracteresNombres();" onpaste="return false;"
									        maxLength="40" size="35" /></td>
							<td class="Arial10B" width="125">&nbsp;Apellido Paterno:</td>
							<td width="250"><input id="txtApePat" class="clsInputEnabled" onkeypress="validaCaracteresNombres();" onpaste="return false;"
									        maxLength="40" size="45" /></td>
							<td class="Arial10B" width="125">&nbsp;Apellido Materno:</td>
							<td><input id="txtApeMat" class="clsInputEnabled" onkeypress="validaCaracteresNombres();" onpaste="return false;"
								maxLength="40" size="45" /></td>
						</tr>
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
							<td height="70"><iframe class="clsGridRow" id="ifraRepresentante" style="WIDTH: 75%; HEIGHT: 100%"
									src="" frameborder="no" scrolling="yes"></iframe>
							</td>
						</tr>
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
							<td colspan="5"><asp:dropdownlist id="ddlTipoOperacion" runat="server" Width="150px" CssClass="clsSelectEnableC" onchange="cambiarTipoOperacion(this);"></asp:dropdownlist></td>
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
						<tr>
							<td colspan="6"><img height="5" alt="" src="../../Imagenes/spacer.gif" width="5" border="0" /></td>
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
                                    </tr>
                                </table>
                            </td>
                        </tr>
						<tr id="trLCDisponible">
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
                                                onclick="agregarPlan();" type="button" value="Agregar Item" />
                                        </td>
                                        <td class="Arial10B" style="display: none" id="tdModalidad">
                                            &nbsp;&nbsp;&nbsp;Modalidad: &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlModalidadPorta" runat="server" Width="150px" CssClass="clsSelectEnable0">
                                                <asp:ListItem Value="">SELECCIONE...</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Arial10B" style="display: none" id="tdOperadorCedente">
                                            &nbsp;&nbsp;&nbsp;Operador cedente: &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlOperadorCedente" runat="server" Width="200px" CssClass="clsSelectEnable0">
                                                <asp:ListItem Value="">SELECCIONE...</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="Arial10B" valign="middle" align="right">
                                Agregar al Carrito&nbsp;
                                <img style="width: 35px; cursor: hand; height: 35px" alt="Agregar Carrito" onclick="agregarCarrito();"
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
						<tr id="trPresentaPoderes" style="DISPLAY: none">
							<td class="Arial10B" colspan="6"><input id="chkPresentaPoderes" disabled type="checkbox" value="" />
								No Requiere Presentar Poderes
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td><img height="2" alt="" src="../../Imagenes/spacer.gif" /></td>
			</tr>
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
				<td colspan="6"><input class="BotonOptm" id="btnGrabar" style="WIDTH: 150px; CURSOR: hand" onclick="grabar();"
						type="button" value="Grabar" />&nbsp;<input class="BotonOptm" id="btnEnviarCreditos" style="WIDTH: 150px; CURSOR: hand" onclick="enviarCreditos();"
						type="button" value="Enviar a Créditos" /></td>
			</tr>
		</table>
        <iframe id="iframeAuxiliar" frameBorder="no" width="10" height="10"></iframe>
			<!-- --------------------------------------------------------------------------------------------------------------------------->
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
			<input id="hidMensaje" type="hidden" name="hidMensaje" runat="server"/> 
            <input id="hidCodError" type="hidden" name="hidCodError" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS DATACREDITO --------------------------------------------->
			<input id="hidRiesgoDC" type="hidden" name="hidRiesgoDC" runat="server"/>
			<input id="hidNroOperacionDC" type="hidden" name="hidNroOperacionDC" runat="server"/>
			<input id="hidRespuestaDC" type="hidden" name="hidRespuestaDC" runat="server"/>
             <input id="hidAutonomia" type="hidden" name="hidAutonomia" runat="server"/>
			<input id="hidCreditosxNombres" type="hidden" name="hidCreditosxNombres" runat="server"/>
			<input id="hidCreditosxDC7" type="hidden" name="hidCreditosxDC7" runat="server"/>
			<input id="hidAdjuntarIngreso" type="hidden" name="hidAdjuntarIngreso" runat="server"/>
			<input id="hidCreditosxReglas" type="hidden" name="hidCreditosxReglas" runat="server"/>
			<!-- ------------------------------------------------------ PARAMETROS DATACREDITO --------------------------------------------->
			<!-- ------------------------------------------------------ PARAMETROS PORTABILIDAD --------------------------------------------><input id="hidOperadorCedente" type="hidden" name="hidOperadorCedente" runat="server">
			<input id="hidNumeroContacto" type="hidden" name="hidNumeroContacto" runat="server"/>
			<input id="hidNumeroFolio" type="hidden" name="hidNumeroFolio" runat="server"/> 
            <input id="hidModalidad" type="hidden" name="hidModalidad" runat="server"/>
			<input id="hidArchivos" type="hidden" name="hidArchivos" runat="server"/> 
            <input id="hidTienePortabilidad" type="hidden" name="hidTienePortabilidad" runat="server"/>
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
            <input id="hidOficinaActual" type="hidden" name="hidOficinaActual" runat="server"/>
			<input id="hidOficina" type="hidden" name="hidOficina" runat="server"/> 
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
            <input id="hidPlanesActivos" type="hidden" name="hidPlanesActivos" runat="server"/>
			<input id="hidArchivosEnvioCreditos" type="hidden" name="hidArchivosEnvioCreditos" runat="server"/>
			<input id="hidCreditosxAsesor" type="hidden" name="hidCreditosxAsesor" runat="server"/>
			<input id="hidMensajeAutonomia" type="hidden" name="hidMensajeAutonomia" runat="server"/>
			<input id="hidCentro" type="hidden" name="hidCentro" runat="server"/>
             <input id="hidCanalSap" type="hidden" name="hidCanalSap" runat="server"/>
			<input id="hidOrgVenta" type="hidden" name="hidOrgVenta" runat="server"/> 
            <input id="hidTipoDocVentaSap" type="hidden" name="hidTipoDocVentaSap" runat="server"/>
			<input id="hidAccion" type="hidden" name="hidAccion" runat="server"/> 
            <input id="hidTipoDocumento" type="hidden" name="hidTipoDocumento" runat="server"/>
			<!-- ------------------------------------------------------ UNIFICADA --------------------------------------------------------->
			<input id="hidCodEstadoSEC" type="hidden" name="hidCodEstadoSEC" runat="server"/>
			<input id="hidLCDisponible" type="hidden" name="hidLCDisponible" runat="server"/>
			<input id="hidPlanesDatosVoz" type="hidden" name="hidPlanesDatosVoz"/> 
            <input id="hidLCDisponiblexProd" type="hidden" name="hidLCDisponiblexProd" runat="server"/>
			<input id="hidPoderes" type="hidden" name="hidPoderes" runat="server"/> 
            <input id="hidListaTipoProducto" type="hidden" name="hidListaTipoProducto"/>
			<input id="hidCadenaSECPendiente" type="hidden" name="hidCadenaSECPendiente"/> 
            <input id="hidCadenaDetalle" type="hidden" name="hidCadenaDetalle" runat="server"/>
			<input id="hidGrupoPaqueteServer" type="hidden" name="hidGrupoPaqueteServer" runat="server"/>
			<input id="hidServicioServer" type="hidden" name="hidServicioServer" runat="server"/>
			<input id="hidPromocionServer" type="hidden" name="hidPromocionServer" runat="server"/>
			<input id="hidListaTipoOperacion" type="hidden" name="hidListaTipoOperacion" runat="server"/>
			<input id="hidTipoOperacion" type="hidden" name="hidTipoOperacion" runat="server"/>
			<input id="hidTipoOferta" type="hidden" name="hidTipoOferta" runat="server" /> 
			<!--ESALASB-->
            <input id="hidNroMinPlanesPorta" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidNroMinPlanesPorta" runat="server"/> 
                <input id="hidCodCampValidacion" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodCampValidacion" runat="server"/> <input id="hidCodPlanValidacion" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodPlanValidacion" runat="server"/> <input id="hidFlagRoamingI" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1" name="hidFlagRoamingI"
				runat="server"/> <input id="hidCanal" type="hidden" name="hidCanal" runat="server"/>
			<input id="hidCodPlanesValidacionI35" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidCodPlanesValidacionI35" runat="server"/> 
                <input id="hidNroMinPlanesI35" style="WIDTH: 3px; HEIGHT: 22px" type="hidden" size="1"
				name="hidNroMinPlanesI35" runat="server"/> 
                <input id="hidResultadoReglas" type="hidden" name="hidResultadoReglas" runat="server"/>
			<input id="hidPerfilCreditos" type="hidden" name="hidPerfilCreditos" runat="server"/>
			<input id="hidLineasMarcadas" type="hidden" name="hidLineasMarcadas" runat="server"/>
			<input id="hidRiesgoClaro" type="hidden" name="hidRiesgoClaro" runat="server"/> 
            <input id="hidComportamiento" type="hidden" name="hidComportamiento" runat="server"/>
			<input id="hidExoneracionRA" type="hidden" name="hidExoneracionRA" runat="server"/>
			<input id="hidPlanesActivoVozDatos" type="hidden" name="hidPlanesActivoVozDatos" runat="server"/>
			<input id="hidListaPerfiles" type="hidden" name="hidListaPerfiles" runat="server"/>
            <input id="hidCantidadMotivoBloqueo" type="hidden" name="hidCantidadMotivoBloqueo" runat="server"/>
			<input id="hidCreditosxLineaDesactiva" type="hidden" name="hidCreditosxLineaDesactiva"
				runat="server"/> 
                <input id="hidMotivoxLineaDesactiva" type="hidden" name="hidMotivoxLineaDesactiva" runat="server"/>
            <input id="hidUsuarioRed" type="hidden" name="hidUsuarioRed" runat="server"/>
            <input id="hidNroEquipos3PlayMax" type="hidden" name="hidNroEquipos3PlayMax" runat="server" />
            <input id="hidListaComodato" type="hidden" name="hidListaComodato" runat="server" />
            <input id="hidEquipoServer" type="hidden" name="hidEquipoServer" runat="server" />
            <input id="hidConsultaPrepago" type="hidden" name="hidConsultaPrepago" runat="server" />
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
    <input id="hidPlanReno" type="hidden" name="hidPlanReno" />
    <input id="hidPlanComboRestringido" type="hidden" name="hidPlanComboRestringido"
        runat="server" />

  <%--//Inicio IDEA-30067--%>
            <input id="hidProductoPortAuto" type="hidden" name="hidProductoPortAuto" runat="server" />
            <%--//Fin IDEA-30067--%>

    </form>
</body>
</html>