
var Obligatorio = 0;

//INICIO JVERASTEGUI PROY-32280 IDEA-42248 PICKING FASE 3 DELIVERY
$(document).ready(function () {
    $('#trConsultaStock').show();
    if ($('#hdnFlagValidar').val() == '1') {
        $('#trConsultaStock').hide();
    }
    PosicionBuyBack();// proy-140736 
    //INICIATIVA 920
    PageMethods.LlenadoModalidades(getValue('ddlTipoOperacion'), getValue('hidNTienePortabilidadValues'), LlenadoModalidades_Callback); 

});
//140736 inicio
function PosicionBuyBack() {
    $("#tblFormnBuyBackIphone").css({ top: 350, left: 300, position: 'absolute' });

 
}

function AsignarDatosBuyback(strResultado) {
    var hdbuyback = document.getElementById('hdbuyback');
    hdbuyback.value = strResultado;
}

function CloseBuyBack() {

    document.getElementById('tblFormnBuyBackIphone').style.display = 'none';
    $("#txtcodcuponBuyback").val("");
    $("#txtIMEIBuyback").val("");
    document.getElementById('ddlBuyBackIphone').selectedIndex = '';
    var idFila = self.frames['ifraCondicionesVenta'].getValue('hidFilaSeleccionada');
    self.frames['ifraCondicionesVenta'].asignarMaterial(idFila);
    autoSizeIframe();
  
   
}

function QuitarComasBuyback(idFila) {
    document.getElementById('tblFormnBuyBackIphone').style.display = 'none';
    $("#txtcodcuponBuyback").val("");
    $("#txtIMEIBuyback").val("");
    document.getElementById('ddlBuyBackIphone').selectedIndex = '';
    if (parseInt(idFila) > 0) {
        var buyback = document.getElementById('hdbuyback')
        buyback.value = buyback.value.replace(",", "|");
    }
}

function GrabarBuyBack() {
    var idFila = self.frames['ifraCondicionesVenta'].getValue('hidFilaSeleccionada');
    var ddlBuyBack = document.getElementById('ddlBuyBackIphone');
//    var idFila = document.getElementById('hidLineaActual').value;

    var hdbuyback = document.getElementById('hdbuyback');
    var strmaterial = $('#ddlBuyBackIphone').val()
    var strcupon = document.getElementById('txtcodcuponBuyback').value;
    var strIMEI = document.getElementById('txtIMEIBuyback').value;


    if (ddlBuyBack.value.length == '') {
        ddlBuyBack.focus();
        alert(Key_Sel_Equipo_Buyback);
        return false;
    }

    if (strcupon == '') {
        
        alert(Key_Ingr_Cupon_Buyback);
        return false;
    }

    if (strIMEI == '') {
       
        alert(Key_Ingr_IMEI_Buyback);
        return false;
    }

    if (strcupon.length > parseInt(Key_Max_Length_Cupon)) {

        alert('El cup�n debe tener como m�ximo ' + Key_Max_Length_Cupon + ' d�gitos');
        return false;
    }


    if (strIMEI.length < parseInt(Key_Min_Length_IMEI)) {

        alert('El IMEI debe tener como m�nimo ' + Key_Min_Length_IMEI +' d�gitos');
        return false;
    }

    if (strIMEI.length > parseInt(Key_Max_Length_IMEI)) {
        alert('El IMEI debe tener como m�ximo ' + Key_Max_Length_IMEI + ' d�gitos');
        return false;
    }

    if (strcupon.slice(-15) != strIMEI.slice(-15)) {
        alert(Key_Msj_Error_Cupon_BuyBack);
        return false;
    }

    var valorBuyBackActual = getValue('hdbuyback');
    var arrBuybackActual = valorBuyBackActual.split('|');

    for (var i = 0; i < arrBuybackActual.length; i++) {
        if (arrBuybackActual[i].split(';')[0] > 0) {
            if (arrBuybackActual[i].split(';')[1] == strcupon) {
                alert(Key_Msj_Error_Igual_Cupon) 
                return false;
             }
        }
    }


    for (var i = 0; i < arrBuybackActual.length; i++) {
        if (arrBuybackActual[i].split(';')[0] > 0) {
            if (arrBuybackActual[i].split(';')[2] == strIMEI) {
                alert(Key_Msj_Error_Igual_Imei)
                return false;
            }
        }
    }

    cargarImagenEsperando();
    PageMethods.ValidarBuyback(strIMEI,strcupon,ValidarBuyback_Callback);

   
}

function GuardarValidarBuyback() {

    var idFila = self.frames['ifraCondicionesVenta'].getValue('hidFilaSeleccionada');
    var hdbuyback = document.getElementById('hdbuyback');
    var strmaterial = $('#ddlBuyBackIphone').val();
    var strcupon = document.getElementById('txtcodcuponBuyback').value;
    var strIMEI = document.getElementById('txtIMEIBuyback').value;

    var valorBuyBack = getValue('hdbuyback');
    var valorActual = idFila  + ';' + strcupon + ';' + strIMEI + ';' + strmaterial + '|';
    hdbuyback.value = valorBuyBack + valorActual;
    $('#hdmaterialCanje').val(strmaterial)
    document.getElementById('tblFormnBuyBackIphone').style.display = 'none';
    self.frames['ifraCondicionesVenta'].ContinuarCambiarEquipo1(idFila);
}

function ValidarBuyback_Callback(objResponse) {
    var strcupon = document.getElementById('txtcodcuponBuyback');
    quitarImagenEsperando();
    var idFila = self.frames['ifraCondicionesVenta'].getValue('hidFilaSeleccionada');
    if (objResponse.Tipo == '1') {

        var seleccion = confirm(objResponse.Mensaje);
        if (seleccion) {
            cargarImagenEsperando();
            PageMethods.EliminarBuybackEvalAnt(objResponse.TipoDoc, getValue('txtNroDoc'), EliminarBuybackEvalAnt);
            GuardarValidarBuyback();
            document.getElementById('tblFormnBuyBackIphone').style.display = 'none';
        }
        else {
            $('#txtcodcuponBuyback').val('');
            strcupon.focus();
            return false;
        }     
    }

    else {
        if (objResponse.Tipo == '0') {
            GuardarValidarBuyback();
        }
        else {
   
            alert(objResponse.Mensaje);
            $('#txtcodcuponBuyback').val('');
            strcupon.focus();
            return false;

        }
    }
 

   
}

function EliminarBuybackEvalAnt(objResponse) {
    quitarImagenEsperando();
   
}
//140736 fin
//FIN JVERASTEGUI PROY-32280 IDEA-42248 PICKING FASE 3 DELIVERY

function ConsultarStock() {
    abrirVentana(urlVentStock + "?inicio=0", "", '800', '350', '_blank', false); //PROY-140371 - IDEA-140719
}
//INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
$(document).ready(function () {

    var array = getValue('hdnCorreosHistoricos');
    document.getElementById("txtCorreoElectronico").onkeyup = function (e) {
        array = getValue('hdnCorreosHistoricos');
        if (array != '') {
            if (!e) e = window.event;
            var key = (window.event) ? e.keyCode : e.which;
            switch (key) {
                case 13:
                    arrow(this.id, 3);
                    this.blur();
                    break;
                case 38:
                    arrow(this.id, 1);
                    break;
                case 40:
                    arrow(this.id, 2);
                    break;
                default:
                    var ul = document.getElementById("ul__" + this.id);
                    ul.style.width = this.style.width;
                    var data = "", clase = "selected";
                    var con = 0;
                    var lArray = array.split('|');
                    if (this.value.trim() != "") {
                        for (var i = 0; i < lArray.length; i++) {
                            if (lArray[i].toUpperCase().indexOf(this.value.toUpperCase()) > -1) {
                                if (con != 0) clase = "";
                                data += "<li id='li" + con + "' onmouseover=\"setear('ul__" + this.id + "',this);\" onmouseout=\"quitar(this);\" onclick=\"contenido('" + this.id + "',this)\" class=\"autocomplete " + clase + "\">";
                                data += lArray[i];
                                data += "</li>";
                                con++;
                            }
                        }
                    }
                    ul.innerHTML = data;
                    if (data != "") ul.style.border = "1px solid gray";
                    else ul.style.border = "0px solid gray";
                    break;
            }
        }
    }

    document.getElementById("txtCorreoElectronico").onblur = function () {
        array = getValue('hdnCorreosHistoricos');
        if (array != '') {
            var x = document.activeElement;
            if (x.id != "ul__" + this.id) {
                document.getElementById("ul__" + this.id).style.display = "none";
            }
        }
    }

    document.getElementById("txtCorreoElectronico").onfocus = function () {
        array = getValue('hdnCorreosHistoricos');
        if (array != '') {
            this.onkeyup();
            document.getElementById("ul__" + this.id).style.display = "";
        }
    }

});

function contenido(destino, origen) {
    document.getElementById(destino).value = origen.innerHTML;
    document.getElementById("ul__" + destino).style.display = "none";
}

function setear(origen, element) {
    var list = document.getElementById(origen).getElementsByTagName("li");
    var lList = list.length;
    for (var j = 0; j < lList; j++) {
        list[j].className = "autocomplete";
    }
    element.className += "selected";
}

function quitar(element) {
    element.className = element.className.replace("selected", "");
}

function arrow(id, opc) {
    var list = document.getElementById("ul__" + id).getElementsByTagName("li");
    var lList = list.length;
    var indexLi = 0;
    for (var j = 0; j < lList; j++) {
        if (list[j].className.indexOf("selected") > -1) {
            indexLi = list[j].id.replace("li", "") * 1;
            if (opc == 1) {
                if (indexLi > 0) indexLi--;
            } else if (opc == 2) {
                if (indexLi < (lList - 1)) indexLi++;
            } else {
                document.getElementById("li" + indexLi).onclick();
            }
            break;
        }
    }
    if (opc != 3) setear("ul__" + id, document.getElementById("li" + indexLi));
}

//FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

function inicio() {

    //INI INICIATIVA 941 - IDEA 142525
    var tipoDocumento = getValue('hidnTipoDocumento');
    var nroDocumento = getValue('hidNroDocumento');
    var tipoOferta = getValue('ddlOferta');
    var montoMaximo = getValue('hidnMontoMaximo');
    var nombres = getValue('hidnNombres');
    var fgEnvioLinkFallido = getValue('hidnEnvioLinkFallido');
    var idAfiliacionPrevio = getValue('hidnIdAfiliacionPrevio');
    //FIN INICIATIVA 941 - IDEA 1425251

    //PROY-26963 - GPRD
    var strAdjuntarDocumentoSEC = getValue('hAdjuntarDocumento');
    setValue('hAdjuntarDocumento', '');
    if (strAdjuntarDocumentoSEC != "") {
        var _urlAdjDoc = UrlAdjuntarSustentos + 'llamadoDesde=E&nroSec=' + strAdjuntarDocumentoSEC + '&cu=' + CurrentUser;
        window.showModalDialog(_urlAdjDoc, 'Adjuntar Sustentos', 'dialogHeight:450px; dialogWidth:960px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
    }
    //Fin PROY-26963-F3 - GPRD
    cambiarCanal();
    cambiarTipoDocumento();
    inicializarPerfiles();

    if (getValue('hidPerfilCreditos') != 'S') {
        document.getElementById('tdRiesgoClaro').style.display = 'none';
        document.getElementById('tdTxtRiesgoClaro').style.display = 'none';
    }
    //PROY-140397-MCKINSEY -> INICIO JCC
    if (getValue('hidPerfil_149') == 'S') {
        document.getElementById('trPuntoVenta').style.display = '';
        document.getElementById('ddlCanal').selectedIndex = 0; //PROY-140397-MCKINSEY -> JCC

    }
    else {
        document.getElementById('ddlPuntoVenta').selectedIndex = 1;
    }
    //PROY-140397-MCKINSEY -> FIN JCC
    //DNI Vendedor
    mostrarVendedor();

    if (getValue('hidnValueAccion') == 'Grabar') {

        var nroSEC = getValue('hidNroSEC');
        var mensaje = getValue('hidnMensajeValue');
        var mensajeEnvioLink = getValue('hidnMensajeEnvioLink'); //PROY-140657
        var blnOK = (getValue('hidCodError') != '1');
        var blnAjuntar = (getValue('hidnAdjuntarIngreso') == 'S');
        var strTipoDoc = getValue('hidTipoDocumento');
        var blnIrCreditos = (getValue('hidCreditosxAsesor') == 'S');
        var strEstadoSEC = getValue('hidCodEstadoSEC');
        var blnPortabilidad = (getValue('hidNTienePortabilidadValues') == 'S');
        var blnOkCT = (getValue('hdnOkCT') == '1'); //PROY-140141

        nuevaEvaluacion();
		if (mensajeEnvioLink != null && mensajeEnvioLink != '' && mensajeEnvioLink.length > 0){
        alert(mensajeEnvioLink); //PROY-140657
		}
        alert(mensaje);

        //INI INICIATIVA 941 - IDEA 142525
        if (fgEnvioLinkFallido === "true") {
            var url = '../consultas/sisact_pop_debito_automatico.aspx?';
            url += 'nroDocumento=' + nroDocumento + '&nombres=' + nombres + '&tipoDocumento=' + tipoDocumento + '&montoMaximo=' + montoMaximo + '&tipoOferta=' + tipoOferta + '&envioLinkFallido=' + fgEnvioLinkFallido + '&idAfiliacionPrevio=' + idAfiliacionPrevio; //PROY-140657
            var dialog = 'dialogWidth:830px;dialogHeight:290px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no';
            var retVal = window.showModalDialog(url, 'Registrar Afiliaci�n / Desafiliaci�n Individuales', dialog);
        }


        //FIN INICIATIVA 941 - IDEA 142525
        if (blnOK) {
            if (strTipoDoc == constTipoDocumentoRUC) {
                if (!blnPortabilidad) {
                    if (blnIrCreditos || strEstadoSEC != constEstadoAPR)
                        AdjuntarDocumentos(nroSEC);
                }
            }

            if (blnAjuntar) {
                if (strTipoDoc != constTipoDocumentoRUC)
                    verAdjuntarDocumentos(nroSEC);
            }

            //PROY-140141 :: INICIO
            if (blnOkCT) {
                window.close();
            }
            //PROY-140141 :: FIN

        }

    }

    codServProteccionMovil = getValue('hidCodServProteccionMovil'); //PROY-24724-IDEA-28174 - INICIO
    msgEquipoSinCobertura = getValue('hidMsgEquipoSinCobertura');
    msgEquipoPrecioPrepagoMenor = getValue('hidMsgEquipoPrecioPrepagoMenor');
    montoPrecioPrepago = getValue('hidMontoPrecioPrepago');
    msgErrorProcedurePrecioPrepago = getValue('hidMsgErrorProcedurePrecioPrepago');
    descServProteccionMovil = getValue('hidDescServProteccionMovil'); //PROY-24724-IDEA-28174 - FIN

    //PROY-32129 :: INI
    document.getElementById('hidGraboDatosAlumnos').value = "0";
    //PROY-32129 :: FIN

    if (strHabilitarProactiva == "0") {
        document.getElementById('btnOtrasOpc').style.display = 'none';
    }
    
    setEnabled('txtWhitelist', false, 'clsInputEnabled'); //PROY-140579 RU10 NN

    //INICIATIVA-803 INI
    if (getValue('hidFlagApagadoExcepcionPrecio') == '1' && getValue('hidFlagDelivery') == '1') {
        document.getElementById('tblTiendaVirtual').style.display = '';
    } else {
        document.getElementById('tblTiendaVirtual').style.display = 'none';
    }
    //INICIATIVA-803 FIN
}

function nuevaEvaluacion() {

    //INICIATIVA - 803 - INI
    var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;

    if (blnTiendaVirtual) {
        setValue('txtNroPedidoWeb', '');
        document.getElementById('chkFlagTienda').click();
    }

    //INICIATIVA - 803 - FIN

    //PROY-140245
    PageMethods.LimpiarVariablesColaborador(getValue('txtNroDoc'))
    //FIN PROY-140245
    var strCodigoCampaniaPorta50Dscto = getValue('hidCodigoCampaniaPorta50Dscto');
    var strNroDiasPermitidosOP = getValue('hidNroDiasPermitidosOP');
    var strMsgPermanenciaOP = getValue('hidMsgPermanenciaOP');
    var ConsFlagConsultaPreviaChip = getValue('hidConsFlagConsultaPreviaChip'); //PROY-140223 IDEA-140462
    var consCPCanalVenta = getValue('hidConsCPCanalVenta'); //PROY-140223 IDEA-140462
    var consCPModdalidadVenta = getValue('hidConsCPModVenta'); //PROY-140223 IDEA-140462
    var consCPPuntoVenta = getValue('hidConsCPPuntoVenta'); //PROY-140223 IDEA-140462
    var consOficinaUsuario = getValue('hidOficinaUsuario'); //PROY-140223 IDEA-140462
    inicializarDatosInicial();
    inicializarPanelDatosInicial();
    inicializarPanelDetalleCliente();
    inicializarPanelRepresentante();
    inicializarPanelCondicionVenta();
    inicializarPanelPortabilidad();
    inicializarPanelResultado();
    inicializarPanelComentarios();
    InicializarPanelExepcionPrecio(); //INICIATIVA -803
    inicializarPanelGrabar();
    setValue('hidDatosBRMS', ""); //PROY-29123 Venta en Cuotas
    setValue('hidCodigoCampaniaPorta50Dscto', strCodigoCampaniaPorta50Dscto);
    setValue('hidNroDiasPermitidosOP', strNroDiasPermitidosOP);
    setValue('hidMsgPermanenciaOP', strMsgPermanenciaOP);
    setValue('hidConsFlagConsultaPreviaChip', ConsFlagConsultaPreviaChip); //PROY-140223 IDEA-140462
    setValue('hidConsCPCanalVenta', consCPCanalVenta); //PROY-140223 IDEA-140462
    setValue('hidConsCPModVenta', consCPModdalidadVenta); //PROY-140223 IDEA-140462
    setValue('hidConsCPPuntoVenta', consCPPuntoVenta); //PROY-140223 IDEA-140462
    setValue('hidOficinaUsuario', consOficinaUsuario); //PROY-140223 IDEA-140462
    //PROY-140335 RF1 INI
    setValue('hidEjecucionCPBRMS', '0');
    setValue('hidLineasSinCP', '');
    setValue('hidLineasRec', '');
    setValue('hidFlagCPPermitidaProa', '0');
    //PROY-140335 RF1 FIN

    setValue('hdiRestriccionCampanasFullClaro', ''); //INICIATIVA-1012

    //PROY-32439 MAS INI
    PageMethods.LimpiarVariablesSesionBRMS()
    //PROY-32439 MAS FIN
    //PROY - 140245 VARIABLES GLOBALES
    flagInicCantProd = false;
    strFlagCarrito = 'N';
    cantMovilAct = 0; // almacena la cantidad actual de productos por cliente con caso especial colaborador
    cantMovilMax = 0; //almacena la cantidad maxima de productos por cliente con caso especial colaborador
    cantFijoInalAct = 0;
    cantFijoInalMax = 0;
    cantClaroTvAct = 0;
    cantClaroTvMax = 0;
    cantBamAct = 0;
    cantBamMax = 0;
    cant3PlayAct = 0;
    cant3PlayMax = 0;
    cantInterInalAct = 0;
    cantInterInalMax = 0;
    cantPlayInalAct = 0;
    cantPlayInalMax = 0;
    strCasoEspecialColab = "";
    strMsgAutogestion = "";
    strMsgValidaCantidadLineas = "";
    flagContinuarEvaluacion = "0";
    strMsgServicioValidConteoLineas = "";
    strCantMaxPorProducto = "";
    Obligatorio = 0;
    setValue('CodigoFCP', 1);
     document.getElementById('PanelCarga').style.display = 'none';
     //FIN PROY - 140245

     document.getElementById('lblBeneficio').innerHTML = '';//IDEA-142010
	borrarDatosCAI(); //PROY-140546
}

function inicializarDatosInicial() {
    var arrControles = document.getElementById('frmPrincipal').all;
    var strListaHidden = ",__EVENTTARGET,__EVENTARGUMENT,__VIEWSTATE";
    strListaHidden = strListaHidden + ",hidTiempoInicioReg,hidVerDetalleLinea,hidVerVentaProactiva,hidPerfil_149,hidListaPuntoVenta,hidListaBlackList,hidBLVendedor,hidListaParametro,hidFactorLC"; //EMG - Sobrecarga de datos - Consulta a tabla parametros eliminada - hidListaParametroII
    strListaHidden = strListaHidden + ",hidCanalSap,hidOrgVenta,hidTipoDocVentaSap,hidCentro,hidListaTipoOperacion,hidNroMinPlanesPorta,hidCodCampValidacion,hidCodPlanValidacion,hidCodPlanesValidacionI35,hidNroMinPlanesI35";
    strListaHidden = strListaHidden + ",hidPerfilCreditos,hidListaPerfiles;hidUsuarioRed;hidPlanBase;hidPlanCombo;hidPlanComboRestringido";
    strListaHidden = strListaHidden + ",hidFlagDelivery,hidFlagApagadoExcepcionPrecio"; //INICIATIVA-803
	strListaHidden = strListaHidden + ",hidTipoCobroAnticipadoInstalacion,hidCobroAnticipadoInstalacion,hidFlagAplicaCAI,hidTiempoSecPendientePagoLink,hidTiempoSecPendientePagoLink,hidCodigoPDVTeleventas,hidMAI,hidMsjValidacionSubformularioCAI"; //PROY-140546
    var lstDocsEvaluacion = $("#hidDocsPostpago").val(); //PROY-31636
    for (var i = 0; i < arrControles.length; i++) {
        if (arrControles[i].type == 'hidden' || arrControles[i].tagName == 'HIDDEN') {
            if (strListaHidden.indexOf(arrControles[i].name) < 0)
                arrControles[i].value = '';
        }
    }
    $("#hidDocsPostpago").val(lstDocsEvaluacion); //PROY-31636
}

function inicializarPerfiles() {
    setValue('hidVerDetalleLinea', buscarPerfil(opcionVerDetalleLinea));
    setValue('hidVerVentaProactiva', buscarPerfil(opcionVentaPuertaPuerta));
    setValue('hidPerfilCreditos', buscarPerfil(opcionConsultaCreditos));
}

function inicializarPanelDatosInicial() {
    if (getValue('hidPerfil_149') == 'S') {
        setValue('hidBLVendedor', '');
        document.getElementById('ddlCanal').selectedIndex = 0; //PROY-140397-MCKINSEY -> JCC
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
    //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
    setValue('txtCorreoElectronico', '');
    setValue('txtConfCorreoElectronico', '');
    setValue('hdnEmailFacturaElectronica', '');
    //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

    //INCIATIVA -803
    document.getElementById("chkFlagTienda").checked = false
    document.getElementById('chkFlagTienda').disabled = false
    document.getElementById('txtNroPedidoWeb').style.display = 'none';
    BorrarDatos();
    //FIN-803

}

function inicializarPanelDetalleCliente() {
    setEnabled('txtNombre', true, 'clsInputEnabled');
    setEnabled('txtApePat', true, 'clsInputEnabled');
    setEnabled('txtApeMat', true, 'clsInputEnabled');
    setEnabled('txtRazonSocial', true, 'clsInputEnabled');
    habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);

    //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
    setEnabled('txtCorreoElectronico', true, 'clsInputEnabled');
    setEnabled('txtConfCorreoElectronico', true, 'clsInputEnabled');
    setValue('txtCorreoElectronico', '');
    setValue('txtConfCorreoElectronico', '');
    //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

    setValue('txtNombre', '');
    setValue('txtApePat', '');
    setValue('txtApeMat', '');
    setValue('txtRazonSocial', '');
    setValue('txtTipoContribuyente', '');
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
    document.getElementById('ddlModalidadVenta').value = getValue('hdModalidaDefecto'); //INICIATIVA 920
    document.getElementById('ddlCombo').selectedIndex = 0;

    document.getElementById('txtLCDisponiblexProd').value = '';
    document.getElementById('tdMovil').style.display = '';
    document.getElementById('tdFijo').style.display = '';
    document.getElementById('tdBAM').style.display = '';
    document.getElementById('tdDTH').style.display = '';
    document.getElementById('tdHFC').style.display = '';
    document.getElementById('tdFTTH').style.display = ''; //FTTH - inicializarPanelCondicionVenta()
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

    setEnabled('btnConsultaPrevia', true, 'Boton'); //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

    //PROY-32129 :: INI
    trDatosAlumnoInstitucion.style.display = 'none';
    //PROY-32129 :: FIN
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
    arrEvaluacion = []; // PROY - 30748
}

function inicializarPanelResultado() {
    setValue('txtResultado', '');
    setValue('txtLCDisponible', '');
    setValue('txtRiesgoClaro', '');
    setValue('txtComportamiento', '');
    setValue('txtRangoLC', '');
    setValue('txtTipoGarantia', '');
    setValue('txtImporte', '');

    setValue('txtFormLead', ''); //PROY-140739 Formulario Leads

    setValue('hidnAutonomia', '');
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
    tdFormulariosLeads.style.display = 'none'; //PROY-140739 Formulario Leads
}

//INI PROY-140739 Formulario Leads
function inicializarPanelLeads() {
    var canal = getValue('ddlCanal');
    var pdv = getValue('hidnOficinaValue');
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var blnPortabilidad = document.getElementById('chkPortabilidad').checked;
    var tipoOperacion = "";
    if(blnPortabilidad){
        tipoOperacion = 'P';
    }else{
        tipoOperacion = getValue('ddlTipoOperacion');
    }

    $("#txtFormLead").attr({
        "maxlength": KeyLeadsMaxLength
    });

    if (KeyLeadsCanal.indexOf(canal) > -1 || KeyLeadsPDV.indexOf(pdv) > -1) { //Valida Canal o PDV
        if (KeyLeadsProductosPermitidosPostpago.indexOf(codTipoProductoActual) > -1 && KeyLeadsTopenPostPago.indexOf(tipoOperacion) > -1) {
            tdFormulariosLeads.style.display = '';
        }
    }


}
//FIN PROY-140739 Formulario Leads

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
//INICIATIVA 803 INICIO

function InicializarPanelExepcionPrecio() {
    document.getElementById('chkExcepPrecio').checked = false;

    setValue('txtPrecioExcep', '');
    setValue('txtCuotasTienda', '');
    tdExcepcionPrecios.style.display = 'none';
    tdExcepcionPrecios.style.display = 'none';
    tdExcepcionPrecios.style.display = 'none';
    document.getElementById('lblcuotasTienda').style.display = 'none';
    document.getElementById('txtCuotasTienda').style.display = 'none';
    document.getElementById('txtPrecioExcep').style.display = 'none';
    document.getElementById('chkExcepPrecio').checked = false;
}

//INICIATIVA 803 FIN

function inicializarPanelGrabar() {
    setValue('hidCreditosxAsesor', '');
    setValue('hidArchivosEnvioCreditos', '');
    setValue('hidnCreditosxNombresV', '');
    setValue('hidnAdjuntarIngreso', '');
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
        setEnabled('ddlNacionalidad', true, ''); //PROY-31636
        setEnabled('txtNroDoc', true, 'clsInputEnabled');
        setEnabled('txtNroPedidoWeb', true, 'clsInputEnabled'); //INICIATIVA 803
        setEnabled('txtFechaNac', true, 'clsInputEnabled');
        if (isVisible('trVendedor')) {
            setEnabled('txtDNIVendedor', true, 'clsInputEnabled');
            setEnabled('btnvalidarVendedor', true, 'Boton');
        }
        if (isVisible('chkPortabilidad')) setEnabled('chkPortabilidad', true, '');
        if (isVisible('tblCartaPoder')) setEnabled('tblCartaPoder', true, '');  //PROY-25335 -  Contratación Electronica - Release 0

        // INICIATIVA - 803 - INI
        if (isVisible('chkFlagTienda')) setEnabled('chkFlagTienda', true, '');
        // INICIATIVA - 803 - INI
    }
    else {
        if (isVisible('trPuntoVenta')) {
            setEnabled('ddlCanal', false, '');
            setEnabled('ddlPuntoVenta', false, '');
        }
        setEnabled('ddlTipoDocumento', false, '');
        setEnabled('ddlNacionalidad', false, ''); //PROY-31636
        setEnabled('txtNroDoc', false, 'clsInputDisabled');
        setEnabled('txtFechaNac', false, 'clsInputDisabled');
        setEnabled('txtNroPedidoWeb', false, 'clsInputDisabled'); //INICIATIVA 803
        if (isVisible('trVendedor')) {
            setEnabled('txtDNIVendedor', false, 'clsInputDisabled');
            setEnabled('btnvalidarVendedor', false, '');
        }
        if (isVisible('chkPortabilidad')) setEnabled('chkPortabilidad', false, '');
        if (isVisible('tblCartaPoder')) setEnabled('tblCartaPoder', false, ''); //PROY-25335 -  Contratación Electronica - Release 0

        // INICIATIVA - 803 - INI
        if (isVisible('chkFlagTienda')) setEnabled('chkFlagTienda', false, ''); 
        // INICIATIVA - 803 - FIN

    }
}

function cambiarTipoDocumento() {
    var tipoDoc = getValue('ddlTipoDocumento');
    document.getElementById('txtNroDoc').value = '';
    document.getElementById('txtNroDoc').maxLength = getMaxLengthDocumento(tipoDoc);
    setFocus('txtNroDoc');
    //INI PROY-31636
    var ddlNacionalidad = $('#ddlNacionalidad');
    ddlNacionalidad.val('');
    ddlNacionalidad.css("display", "");
    ddlNacionalidad.attr("disabled", false);
    $("#lblNacionalidad").css("display", "");

    if (tipoDoc == constTipoDocumentoRUC) {
        $("#lblNacionalidad").css("display", "none");
        ddlNacionalidad.css("display", "none");
        document.getElementById('tdLblFechaNac').style.display = 'none';
        document.getElementById('tdTxtFechaNac').style.display = 'none';
    }
    else {
        if (tipoDoc == constTipoDocumentoDNI) {
            if ($("#ddlNacionalidad option[value='154']").length == 0)
                ddlNacionalidad.append('<option value="154">Peru</option>');
            ddlNacionalidad.val('154'); ;
            ddlNacionalidad.attr("disabled", true);
        }
        else if (Key_codDocMigra_Pasaporte_CE != null) {
            if (Key_codDocMigra_Pasaporte_CE.indexOf(tipoDoc) > -1)
                if ($("#ddlNacionalidad option[value='154']").length > 0)
                    $("#ddlNacionalidad option[value='154']").remove();
        }
        //INI PROY-140434
        document.getElementById('tdLblFechaNac').style.display = 'none';
        document.getElementById('tdTxtFechaNac').style.display = 'none';
        //FIN PROY-140434
    }
    //FIN PROY-31636

    //Inicio PROY-25335 -  Contratación Electronica - Release 0
    // if (getValue('ddlTipoDocumento') == constTipoDocumentoDNI && cartaPoder == '1') { //PROY-25335 -  Contratación Electronica - Release 2
    if (cartaPoder == '1') {  //PROY-25335 -  Contratación Electronica - Release 2
        document.getElementById('tblCartaPoder').style.display = '';
    } else {
        document.getElementById('chkCartaPoder').checked = false;
        document.getElementById('hidCartaPoder').value = 'N';
        document.getElementById('tblCartaPoder').style.display = 'none';

    } // Fin PROY-25335 -  Contratación Electronica - Release 0
}

//INI PROY-31636

function validaTxtDoc() {
    var tipoDoc = getValue("ddlTipoDocumento");
    if (Key_codigoDocMigraYPasaporte.indexOf(tipoDoc) > -1)
        validaCaracteres('ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789');
    else
        validaCaracteres('0123456789');
}
//FIN PROY-31636

function cambiarCanal() {
    var strCodCanal = getValue('ddlCanal');
    var ddlPuntoVenta = document.getElementById('ddlPuntoVenta');

    inicializarCombo(ddlPuntoVenta);

    if (strCodCanal != '') {
        setValue('hidnCanalValue', strCodCanal);
        var arrPuntoVenta = document.getElementById('hidListaPuntoVenta').value.split('|');
        for (var i = 0; i < arrPuntoVenta.length; i++) {
            var colPuntoVenta = arrPuntoVenta[i].split(';');
            if (colPuntoVenta[2] == strCodCanal) {
                cargarCombo(ddlPuntoVenta, colPuntoVenta[0], colPuntoVenta[1] + " - " + colPuntoVenta[0]);
            }
        }
    }

    //INI PROY-31636
    var ddlTipoDoc = $("#ddlTipoDocumento");
    var lstDocsEvaluacion = $("#hidDocsPostpago").val().split("|");
    var docsExtPermitidos = (strCodCanal == constCodTipoOficinaCAC) ? Key_tipoDocPermitidoPostCAC :
                                    (strCodCanal == constCodTipoOficinaDAC) ? Key_tipoDocPermitidoPostDAC :
                                    (strCodCanal == constCodTipoOficinaCorner) ? Key_tipoDocPermitidoPostCAD : "";

    if (!strCodCanal)
        ddlTipoDoc.find('option').remove().end().append('<option value="0">SELECCIONE...</option>');

    if (docsExtPermitidos) {
        ddlTipoDoc.find('option').remove().end().append('<option value="0">SELECCIONE...</option>');
        var docsExtra = Key_codigoDocMigratorios.split("|");
        var docsRemove = "";
        for (var i = 0; i < docsExtra.length; i++) {
            if (docsExtPermitidos.indexOf(docsExtra[i]) == -1)
                docsRemove += (docsRemove == "") ? docsExtra[i] : ("|" + docsExtra[i]);
        }

        for (var i = 0; i < lstDocsEvaluacion.length; i++) {
            var documento = lstDocsEvaluacion[i].split(",");
            if (docsRemove.indexOf(documento[0]) == -1) {
                ddlTipoDoc.append($('<option>', {
                    value: documento[0],
                    text: documento[1]
                }));
            }
        }
    }
    //FIN PROY-31636

    cambiarPuntoVenta();

    mostrarCartaPoder(); //PROY-25335 -  Contratación Electronica - Release 0
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
    document.getElementById('hidNTienePortabilidadValues').value = 'N';
    if (chk.checked) {
        document.getElementById('hidNTienePortabilidadValues').value = 'S';
    }
}

//Inicio PROY-25335 -  Contratación Electronica - Release 0
function cambiarCartaPoder(chk) {
    if (chk.checked) {
        document.getElementById('hidCartaPoder').value = 'S';
    } else {
        document.getElementById('hidCartaPoder').value = 'N';
    }
} //Inicio PROY-25335 -  Contratación Electronica - Release 0

//INICIO PROY-140419 Autorizar Portabilidad sin PIN
function cambiarSmsSupervisor(chk) {
    if (chk.checked) {
        document.getElementById('hdnValidaSupervisor').value = 'S';
    } else {
        document.getElementById('hdnValidaSupervisor').value = 'N';
    }
}
//FIN PROY-140419 Autorizar Portabilidad sin PIN

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
    if (getValue('txtDNIVendedor').length != getMaxLengthDocumento(constCodTipoDocumentoDNI)) {
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

    //INI PROY 140736
    $("#txtcodcuponBuyback").attr({
        "maxlength": Key_Max_Length_Cupon
    });

    $("#txtIMEIBuyback").attr({
        "maxlength": Key_Max_Length_IMEI
    });

    $("#txtIMEIBuyback").attr({
        "minlength": Key_Min_Length_IMEI
    });
    //PROY-140736-FIN

    //INICIATIVA - 803 - INI
    var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;

    if (blnTiendaVirtual) {
        setValue('hidFlagTiendaVirtual', '1');

        var txtNroPedidoWeb = getValue("txtNroPedidoWeb");

        if ((txtNroPedidoWeb == null || txtNroPedidoWeb == "") || (txtNroPedidoWeb == 'Ingrese Nro pedido web')) {
            var msjStopperTV = Key_MsjIngresarIdPedido;
            alert(msjStopperTV);
            return false;
        }
       
    }
    //INICIATIVA - 803 - FIN


    //PROY-29123
    setValue('hidDatosBRMS', "");

    document.getElementById('lblMensajeDeudaBloqueo').innerHTML = '';

    if (isVisible('trPuntoVenta')) {
        if (!validarControl('ddlCanal', '', 'Seleccione el Canal.')) return false;
        if (!validarControl('ddlPuntoVenta', '', 'Seleccione el Punto de Venta.')) return false;
    }
    //INI PROY-31636
    var tipoDoc = getValue("ddlTipoDocumento");
    var cantTxtNroDoc = getValue('txtNroDoc').length;
    if (Key_codigoDocMigratorios.indexOf(tipoDoc) > -1 && getValue("hidNTienePortabilidadValues") == "S") {
        alert('No se encuentra habilitado el flujo de portabilidad para este tipo de documento.');
        return false;
    }
    else if (Key_codigoDocMigraYPasaporte.indexOf(tipoDoc) > -1) {
        //if (cantTxtNroDoc < 4 || cantTxtNroDoc > Key_maxLengthDocMigratorios) {
        //INC000003430042 - INICIO
        if (cantTxtNroDoc < parseInt(Key_minLengthDocPass) || cantTxtNroDoc > Key_maxLengthDocMigratorios) {
            //INC000003430042 - FIN
            alert("Ingresar número de documento válido.");
            return false;
        }
    }
    else if (cantTxtNroDoc != getMaxLengthDocumento(tipoDoc)) {
        setFocus('txtNroDoc');
        alert("Ingresar número de documento válido.");
        return false;
    }

    if (getValue("ddlNacionalidad") == '' && tipoDoc != constTipoDocumentoRUC) {
        alert("Debe seleccionar una nacionalidad");
        return false;
    }
    //FIN PROY-31636

    if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
        //INI PROY-140434
        setValue('txtFechaNac', '11111111');
        //FIN PROY-140434
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
            setValue('hidnOficinaValue', filaPuntoVenta[0])
            setValue('hidnOficinaActual', filaPuntoVenta)
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
    if (getValue('hidNTienePortabilidadValues') == 'S')
        self.frames["ifraPortabilidad"].window.location.replace("../frames/sisact_ifr_portabilidad.aspx?strTipoOficina=" + getValue('ddlCanal')); // PROY-26358 - Portabilidad(carga) TIPO OFICINA- Evalenzs

    //Consulta Claro
    PageMethods.consultaClaro(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), getValue('hidnOficinaValue'), getValue('txtFechaNac'), getValue('hidNTienePortabilidadValues'), consultaClaro_Callback); // PROY 32439 AGREGAR  PARAM hidNTienePortabilidadValues
}

//ECM s8 --llamada a ventana lineas 3G

function mostrar_lineas3g() {
    var tipodoc = getValue('ddlTipoDocumento');
    var numdoc = getValue('txtNroDoc');
    PageMethods.AveriguarSiMuestroLineasTecnologia(tipodoc, numdoc, AveriguarSiMuestroLineasTecnologia_callback);
}

function AveriguarSiMuestroLineasTecnologia_callback(objResponse) {
    var tipodoc = getValue('ddlTipoDocumento');
    var numdoc = getValue('txtNroDoc');
    var canal = getValue("ddlCanal");


    var urlLineas = "./sisact_pop_consulta3g.aspx?";
    var datos = "canal=" + canal + "&tipodoc=" + tipodoc + "&numdoc=" + numdoc;
    if (objResponse.Cadena == "SI") {
        var w = 600;
        var h = 200;
        abrirVentana(urlLineas + datos, "", w, h, '_blank', true);
        /*var leftScreen = (screen.width - w) / 2;
        var topScreen = (screen.height - h) / 2;
        var opciones = "directories=no,menubar=no,scrollbars=no,status=no,resizable=no,width=" + w + ",height=" + h + ",left=" + leftScreen + ",top=" + topScreen;
        window.open(url, '_blank', opciones);*/
    } else {
        mostrarSiguientesTr();
    }
}

function mostrarSiguientesTr() {
    trDetalleCliente.style.display = '';
    trConsultarDC.style.display = '';
    if (!vienedeSecReno) return;
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
    vienedeSecReno = false;
}
//fin ecm s8

function consultaClaro_Callback(objResponse) {
    quitarImagenEsperando();

    if (objResponse.Error) {
        if (objResponse.Tipo == 'E') {
            setFocus('txtFechaNac');
            document.getElementById('lblMensajeDeudaBloqueo').innerHTML = objResponse.Mensaje;
            habilitarBoton('btnvalidarClaro', 'Validación Claro', true);
        } else {
            alert(objResponse.Mensaje);
            nuevaEvaluacion();
        }
        return;
    }

  setValue('hidPendienteEnvioBonoFC', objResponse.Obligatorio); //INC000004280198
  setValue('hidEstadoFullClaro', objResponse.EstadoBonoBSCSFC); //INC000004280198

   
    //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
    if (getValue('ddlCanal') == constCodTipoOficinaCAC) {

        if (objResponse.DescripcionError != '' || objResponse.DescripcionError != null) {
            var strEmailFact = objResponse.DescripcionError.split(';')[0];
            var strCorreosHistoricos = objResponse.DescripcionError.split(';')[1];
            setValue('hdnCorreosHistoricos', strCorreosHistoricos);

            if (strEmailFact != '') {
                setValue('txtCorreoElectronico', strEmailFact);
            }
        }

    } else {
        trCorreoElectronico.style.display = 'none';
        trConfCorreoElectronico.style.display = 'none';
        trLista.style.display = 'none';
    }
    //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18


    //PROY-140715- INI ANGEL
    document.getElementById("hidMensajeConfiguracion").value = objResponse.mensaje_contingencia;
    var mensaje = objResponse.mensaje_contingencia;
    if (mensaje != "" && mensaje != null) {
        var arrayMensaje2 = mensaje.split('|')[0];
        var mensaje_contingencia = arrayMensaje2;
        if (mensaje_contingencia != "" && mensaje_contingencia != null) {
            alert(mensaje_contingencia);
        }
    }
    //PROY-140715- INI ANGEL

    habilitarBoton('btnvalidarClaro', 'Validación Claro', false);

    var arrParam = objResponse.Cadena.split('#');

    //DIL::INI::20170910
    var strCanal = getValue('ddlCanal');
    var intCantidadLineasCliente = Number(arrParam[18]);
    var intCantidadLineasPermitidas = Number(keyValidarLineas_CantidadPermitida);
    var strCantidadLineasPermitidas_Mensaje = '';
    var strCantidadLineasPermitidas_MensajeValidacionAdvertencia = keyValidarLineas_MensajeValidacionAdvertencia;
    var strCantidadLineasPermitidas_MensajeValidacionRestrictiva = keyValidarLineas_MensajeValidacionRestrictiva;
    strCantidadLineasPermitidas_MensajeValidacionAdvertencia = strCantidadLineasPermitidas_MensajeValidacionAdvertencia.replace('*', intCantidadLineasPermitidas);
    strCantidadLineasPermitidas_MensajeValidacionRestrictiva = strCantidadLineasPermitidas_MensajeValidacionRestrictiva.replace('*', intCantidadLineasPermitidas);

    if (intCantidadLineasCliente <= intCantidadLineasPermitidas) //Permitido
    {
        blnContinuarProceso = true;
    }
    else //No permitido
    {
        if (strCanal == constCodTipoOficinaCAC) {
            strCantidadLineasPermitidas_Mensaje = strCantidadLineasPermitidas_MensajeValidacionAdvertencia;
            blnContinuarProceso = true;
            alert(strCantidadLineasPermitidas_Mensaje);
        }
        else //DAC y CAD
        {
            strCantidadLineasPermitidas_Mensaje = strCantidadLineasPermitidas_MensajeValidacionRestrictiva;
            blnContinuarProceso = false;
        }
    }

    if (!blnContinuarProceso) {
        alert(strCantidadLineasPermitidas_Mensaje);
        return;
    }
    //DIL::FIN::20170910 

    document.getElementById('lblMensajeDeudaBloqueo').innerHTML = arrParam[0];
    document.getElementById('lblCategoriaCliente').innerHTML = arrParam[1];
    setValue('hidEvaluarSoloFijo', arrParam[2]);

    if (getValue('hidVerDetalleLinea') == 'S') document.getElementById('btnDetalleLinea').style.display = '';

    //INC000003443673 - INI
    var strApePat = (arrParam[3] === null) ? "" : arrParam[3];
    var strApeMat = (arrParam[4] === null) ? "" : arrParam[4];

    if (strApePat != "" || strApeMat != "") {
        var strApePat = Key_ValorSinApellidoPaternoOMaterno.indexOf(strApePat) > -1 ? "." : strApePat;
        var strApeMat = Key_ValorSinApellidoPaternoOMaterno.indexOf(strApeMat) > -1 ? "." : strApeMat;
        setValue('txtApePat', strApePat);
        setValue('txtApeMat', strApeMat);
    } else {
        var strApePat = strApePat == "" ? "." : strApePat;
        var strApeMat = strApeMat == "" ? "." : strApeMat;
        setValue('txtApePat', strApePat);
        setValue('txtApeMat', strApeMat);
    }
    //INC000003443673 - FIN

    setValue('txtNombre', arrParam[5]);
    setValue('txtRazonSocial', arrParam[6]);

    setValue('hidnPlanesActivos', arrParam[8]);
    setValue('hidnPlanesActivoVozDatos', arrParam[9]);
    setValue('hidPlanesDatosVoz', arrParam[10]);

    setValue('hidTipoDocVentaSap', arrParam[11]);
    setValue('hidCanalSap', arrParam[12]);
    setValue('hidOrgVenta', arrParam[13]);
    setValue('hidCentro', arrParam[14]);
    setValue('hidnConsultPrepago', arrParam[15]);
    setValue('hidBlackListCuota', arrParam[16]);
    setValue('hidFechaHoraConsulta', arrParam[17]);
    //PROY-29121-INI
    setValue('hidDeudaCliente', arrParam[19]);
    setValue('hidCumpleReglaA', arrParam[22]);
    setValue('hidCumpleReglaAClienteRRLL', arrParam[22]);
    //PROY-29121-FIN

    if (getValue('hidNTienePortabilidadValues') == 'S') {
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
    

    //PROY-140579 RU10 NN INICIO
    setValue('txtWhitelist', arrParam[23]);
    //PROY-140579 RU10 NN FIN

    //PROY_FULLCLARO_INI.V2
    if (objResponse.CodigoFC == "0" || objResponse.CodigoFC == "1" || objResponse.CodigoFC == "3" || objResponse.CodigoFC == "2" || objResponse.CodigoFC == "7") {
        alert(objResponse.MensajeFC);
        setValue('hdnObligatoriedad', objResponse.ObligatoriedadFC);
        setValue('hdiRestriccionCampanasFullClaro', objResponse.RestriccionCampanasFullClaro);
    }
    //PROY_FULLCLARO_FIN.V2

    //IDEA-142010 INI
    var strMensajeBeneficio = objResponse.BeneficioAdicionalMsg;
    var strLblMensajeBeneficio = objResponse.BeneficioEstado;
    var estadoMensajeBeneficio = objResponse.BeneficioEstado;

    if (strLblMensajeBeneficio == 'SI') {
        document.getElementById('lblBeneficio').innerHTML = strMensajeBeneficio;
    } else {
        document.getElementById('lblBeneficio').innerHTML = '';
    }

    if (strMensajeBeneficio != '') {
        if (estadoMensajeBeneficio != '-1')
        {
            alert(strMensajeBeneficio);
        }        
    }
    //IDEA-142010 INI

    //PROY-140743 - IDEA-141192 - Venta en cuotas | INICIO
    setValue('hidIsClienteClaro', objResponse.isClienteClaro);
    setValue('hidFlagVentaVV', objResponse.flagPermitirVV);
    var blnPortabilidad = document.getElementById('chkPortabilidad').checked;

    if ((objResponse.flagPermitirVV == "0" || objResponse.isClienteClaro == "0") && !blnPortabilidad) {
        alert(Key_MsjNoAplicaCuotas);
    }
    // PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

}

function retornarConsultaSEC(nroSec) {
    var flgTipoDocumentoRUC = (getValue('ddlTipoDocumento') == constTipoDocumentoRUC);
    trDetalleDNI.style.display = (flgTipoDocumentoRUC) ? 'none' : '';
    trDetalleRUC.style.display = (flgTipoDocumentoRUC) ? '' : 'none';


    //FVERGARA  .:Inicio:.  
    //Implementacion del Activador para Consultas 3G    Date: 18/08/2017

    var strSwicht_3G = GV_Swicht_3G;
    if (strSwicht_3G == '1') {
        // Consulta Línea Desactiva
        if (getValue('ddlCanal') == constCodTipoOficinaCAC) {
            var url = "../consultas/sisact_pop_lineas_desactivas.aspx?";
            url += "tipoDocumento=" + getValue('ddlTipoDocumento');
            url += "&nroDocumento=" + getValue('txtNroDoc');
            url += "&flgConsultaEval=S" + "&flgConsultaPopup=N";
            document.getElementById('ifrLineasDesactivas').src = url;
            trLineasDesactivas.style.display = '';
        } /*ECM s8
             else {
                trDetalleCliente.style.display = '';
                trConsultarDC.style.display = '';
            }*/

        // Consulta Línea Prepago
        if (getValue('hidnConsultPrepago') == 'S') {
            var url = '../consultas/sisact_pop_detalle_linea_prepago.aspx?nroDocumento=' + getValue('txtNroDoc');
            window.showModalDialog(url, '', 'dialogHeight:260px; dialogWidth:390px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
        }
        //ECM s8 llamado a lineas 3G
        mostrar_lineas3g();
    }
    else {

        // Consulta Línea Desactiva
        if (getValue('ddlCanal') == constCodTipoOficinaCAC) {
            var url = "../consultas/sisact_pop_lineas_desactivas.aspx?";
            url += "tipoDocumento=" + getValue('ddlTipoDocumento');
            url += "&nroDocumento=" + getValue('txtNroDoc');
            url += "&flgConsultaEval=S" + "&flgConsultaPopup=N";
            document.getElementById('ifrLineasDesactivas').src = url;
            trLineasDesactivas.style.display = '';
        }
        else {
            trDetalleCliente.style.display = '';
            trConsultarDC.style.display = '';
        }

        // Consulta Línea Prepago
        if (getValue('hidnConsultPrepago') == 'S') {
            var url = '../consultas/sisact_pop_detalle_linea_prepago.aspx?nroDocumento=' + getValue('txtNroDoc');
            window.showModalDialog(url, '', 'dialogHeight:260px; dialogWidth:390px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
        }


    }
    //.:Fin:.

}

function consultarDC() {
    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
        if (!validarControl('txtRazonSocial', '', 'Ingresar la razón social del cliente.')) return false;
    } else {
		if ((getValue('ddlTipoDocumento') == constTipoDocumentoCE) || (getValue('ddlTipoDocumento') == constCodTipoDocumentoPAS)) { //INC000002748172
			if (!validarControl('txtNombre', '', 'Ingresar nombres del cliente.')) return false;
			if (!validarControl('txtApePat', '', 'Ingresar apellido paterno del cliente.')) return false;
		}
		else {
        if (!validarControl('txtNombre', '', 'Ingresar nombres del cliente.')) return false;
        if (!validarControl('txtApePat', '', 'Ingresar apellido paterno del cliente.')) return false;
        if (!validarControl('txtApeMat', '', 'Ingresar apellido materno del cliente.')) return false;
    }
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

    //PROY-140141–INICIO
    var strEmail = getValue('txtCorreoElectronico');
    setValue('hdnEmail', strEmail);
    //PROY-140141–FIN
}

//PROY-29123
function consultarClienteBRMS() {
    //variables JSON para el Storage
    var objCuotas = { cuota: [] };
    var objStorageJson;
    var objCuotaJson;

    //Variables para obtener datos del JSON
    var cuotas = 0;
    var monto = 0.0;
    var mensajeCuotas = "";

    //Valida y Crea SessionStorage
    if (getValue('hidDatosBRMS') == "") {

        setValue('hidDatosBRMS', JSON.stringify(objCuotas));
    }


    document.getElementById('lblCuotas').innerHTML = '';
    var nroDocumento = getValue('txtNroDoc');

    if (nroDocumento && nroDocumento != "") {
        objStorageJson = JSON.parse(getValue('hidDatosBRMS'));

        for (var i in objStorageJson.cuota) {
            if (objStorageJson.cuota[i].operacion == getValue('ddlTipoOperacion')) {
                if (objStorageJson.cuota[i].msjbrms == "") {
                    objStorageJson.cuota.splice(i, 1);
                    setValue('hidDatosBRMS', JSON.stringify(objStorageJson));
                }
                else
                    objCuotaJson = objStorageJson.cuota[i];
            }
        }

        if (objCuotaJson) {
            cuotas = parseInt(objCuotaJson.cuotamax);
            mont = parseFloat(objCuotaJson.topemax);
            mensajeCuotas = objCuotaJson.msjbrms;

            if (mensajeCuotas && mensajeCuotas != "" && mensajeCuotas == "SI") {
                document.getElementById('lblCuotas').innerHTML = 'Cuotas: Max. ' + cuotas + ' Tope de Equipo: S/ ' + mont.toLocaleString("es-Mx");
            }
            else {
                document.getElementById('lblCuotas').innerHTML = '';
            }
        }
        else {
            var strDatos = cadenaGeneral();
            PageMethods.ConsultarInformacionClienteBRMS(nroDocumento, getValue('hidNroOperacionDC'), strDatos, getValue('hidProdCuentaFact'), ConsultarInformacionClienteBRMS_Callback);//PROY-140743
        }

    }


}
//PROY-29123
function ConsultarInformacionClienteBRMS_Callback(objResponse) {
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }

    var msj = objResponse.Cadena;
    var arr = new Array();
    var cuotas = 0;
    var monto = 0.0;
    var mensajeCuotas = "";
    var tipOpe = getValue('ddlTipoOperacion');
    var objStorageJson = JSON.parse(getValue('hidDatosBRMS'));
    var objJson;
    arr = msj.split('#');

    cuotas = parseInt(arr[0]);
    mont = parseFloat(arr[1]);
    mensajeCuotas = arr[2];

    objJson = { operacion: tipOpe, cuotamax: cuotas, topemax: mont, msjbrms: mensajeCuotas };

    objStorageJson.cuota.push(objJson);

    setValue('hidDatosBRMS', JSON.stringify(objStorageJson));

    if (mensajeCuotas && mensajeCuotas != "" && mensajeCuotas == "SI") {
        document.getElementById('lblCuotas').innerHTML = 'Cuotas: Max. ' + cuotas + ' Tope de Equipo: S/ ' + mont.toLocaleString("es-Mx");
    }
    else {
        document.getElementById('lblCuotas').innerHTML = '';
    }

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
        if (getValue('txtNroDoc').substring(0, 1) == constRUCInicio) {
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
    document.getElementById('hidnRiesgoDCValue').value = arrdatosDC[3];
    document.getElementById('hidBuroConsultado').value = arrdatosDC[4]; //ADD PROY-20054-IDEA-23849
    document.getElementById('txtTipoContribuyente').value = arrdatosDC[5]; //PROY-32438
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

    document.getElementById('hidNRespuestaDCValue').value = strRespuesta;
    document.getElementById('hidnRiesgoDCValue').value = strRiesgo;
    document.getElementById('hidNroOperacionDC').value = strNroOperacion;
    //gaa20170215
    document.getElementById('hidBuroConsultado').value = arrdatosDC[38];
    //fin gaa20170215
    //Por Defecto Edad 18
    var strEdad = ConstAnioMayorEdad;
    var strFechaNacDC = strFechaNacimiento.substr(0, 10);
    //INI PROY-140434
    setValue('hidFechaNac', strFechaNacDC);
    //FIN PROY-140434

    //INI PROY-31636
    var tipoDocumento = getValue('ddlTipoDocumento');
    if (getValue('hidNRespuestaDCValue') != constRespDataCredTipo7 && tipoDocumento != constCodTipoDocumentoCEX
            && Key_codigoDocMigratorios.indexOf(tipoDocumento) == -1)
        strEdad = calculaEdad(strFechaNacDC, fechaActual);
    //FIN PROY-31636
    if (strRespuesta == constRespDataCredTipo6) //10
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
    else if (strRespuesta == constRespDataCredTipo7) //09
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
    else if (strRespuesta == constRespDataCredTipo1) //13
    {
        //Respuesta Datos del Cliente DC
        document.getElementById('txtApePat').value = strApePaterno;
        document.getElementById('txtApeMat').value = strApeMaterno;
        document.getElementById('txtNombre').value = strNombres;
        habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', false);

        //Campos Editable Carnet Extranjeria
        //INI PROY-31636
        if (Key_codDocMigra_Pasaporte_CE.indexOf(tipoDocumento) > -1) {
            setEnabled('txtNombre', true, 'clsInputEnabled');
            setEnabled('txtApePat', true, 'clsInputEnabled');
            setEnabled('txtApeMat', true, 'clsInputEnabled');
        }
        else {
            setEnabled('txtNombre', false, 'clsInputDisabled');
            setEnabled('txtApePat', false, 'clsInputDisabled');
            setEnabled('txtApeMat', false, 'clsInputDisabled');
        }
        //FIN PROY-31636

        // Consultar LC Disponible x Producto
        consultarLCDisponible();
    }

    //Proy 29123
    consultarClienteBRMS();
}

function mostrarSecPendienteIfr(pstrNroSec) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'strNroSec=' + pstrNroSec;
    url += '&strFlujo=' + obtenerFlujo();
    url += '&strTipoDocumento=' + getValue('ddlTipoDocumento');
    url += '&strNroDoc=' + getValue('txtNroDoc');
    url += '&strOficina=' + getValue('hidnOficinaValue');
    url += '&strRiesgo=' + getValue('hidnRiesgoDCValue');
    url += '&strOrgVenta=' + getValue('hidOrgVenta');
    url += '&strCentro=' + getValue('hidCentro');
    url += '&strTipoDocVentaSap=' + getValue('hidTipoDocVentaSap');
    url += '&strCanalSap=' + getValue('hidCanalSap');
    url += '&strTipoOficina=' + getValue('ddlCanal');
    url += '&strTipoOperacion=' + getValue('ddlTipoOperacion');
    url += '&strMetodo=' + 'MostrarSecPendiente';

    self.frames['iframeAuxiliar'].location.replace(url);
}

/*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
function mostrarSecPendiente(strValor) {
    if (strValor == '' || strValor == null || typeof strValor == 'undefined') {
        if (validarOfertaDefault()) {
            $('#ddlOferta').val(Key_CodigoOfertaDefault);
            cambiarTipoOferta(document.getElementById('ddlOferta'));
        }
    }
    else {
    setValue('hidCadenaSECPendiente', strValor);
    self.frames["ifraCondicionesVenta"].window.location.reload();
}
}
/*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

function asignarTipoOperacion() {
    var strCanal = getValue('ddlCanal');
    var arrTipoOperacion = getValue('hidListaTipoOperacion').split('|');
    var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');
    var strDatos = '';
    for (var i = 0; i < arrTipoOperacion.length; i++) {
        var arrTipo = arrTipoOperacion[i].split(';');
        if (arrTipo[0] == strCanal && arrTipo[1] == getValue('ddlTipoDocumento')) {
            if (getValue('hidNTienePortabilidadValues') == 'S') {
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
    //INI PROY-140434
    setValue('hidLCDisponiblexProd', objResponse.Cadena);
    //FIN PROY-140434
    quitarImagenEsperando();
    if (objResponse.Error) {
        habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
        alert(objResponse.Mensaje);
        return;
    }
    PageMethods.CargarParametrosBoletaElectronica(Boleta_Callback);
}

function Boleta_Callback(objResponse) {
    var alertas = objResponse.split('|');
    //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
    var strEmailFacturacion = getValue('txtCorreoElectronico').toLowerCase();          //RGPFASEII
    var strEmailFacturacionConf = getValue('txtConfCorreoElectronico').toLowerCase();  //RGPFASEII

    if (getValue('ddlCanal') == constCodTipoOficinaCAC) {
        if (strEmailFacturacion == '' && strEmailFacturacionConf == '') {
            if (!confirm(alertas[0])) {
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
                return;
            }
        }
        else if (strEmailFacturacion != '' && strEmailFacturacionConf == '') {
            alert(alertas[1]);
            setValue('txtConfCorreoElectronico', '');
            setValue('hdnEmailFacturaElectronica', '');
            habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
            return;
        }
        else if (strEmailFacturacion == strEmailFacturacionConf && strEmailFacturacion != '') {
            if (Verifica_Correo(strEmailFacturacion)) {
                alert(alertas[2]);
                //setValue('txtCorreoElectronico', '');
                setValue('txtConfCorreoElectronico', '');
                setValue('hdnEmailFacturaElectronica', '');
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
                return;
            } else {
                habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
            }
        }
        else {
            alert(alertas[3]);
            //setValue('txtCorreoElectronico', '');
            setValue('txtConfCorreoElectronico', '');
            setValue('hdnEmailFacturaElectronica', '');
            habilitarBoton('btnConsultaDC', 'Ingresar Condiciones de Venta', true);
            return;
        }

        setEnabled('txtCorreoElectronico', false, 'clsInputEnabled');
        setEnabled('txtConfCorreoElectronico', false, 'clsInputEnabled');
    }
    //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

    //PROY-29121-INI
    if (getValue('ddlTipoDocumento') == '06') {
        trCondicionVenta.style.display = 'none';
    }
    else {
        trCondicionVenta.style.display = '';
    }

    trCondicionVentaDetalle.style.display = '';
    //PROY-29121-FIN

    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
        trRepresentante.style.display = '';
    }

    /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
    // SEC Pendiente
    if (getValue('hidNroSEC') != '' && getValue('hidEvaluarSoloFijo') == '') {
        mostrarSecPendienteIfr(getValue('hidNroSEC'));
        $('#hdsecbuyback').val(getValue('hidNroSEC')); //PROY-140736
    } else {
        if (validarOfertaDefault()) {
            $('#ddlOferta').val(Key_CodigoOfertaDefault);
            cambiarTipoOferta(document.getElementById('ddlOferta'));
        }
    }
    /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

    // Validación Modalidad / Operador Cedente
    if (getValue('hidNTienePortabilidadValues') == 'S') {
        document.getElementById('tdModalidad').style.display = '';
        document.getElementById('tdOperadorCedente').style.display = '';
    }

    //DOBLE BENEFICIO

    var vTipoDocumento = getValue('ddlTipoDocumento');
    var vNumeroDocumento = getValue('txtNroDoc');
    var vCanal = getValue('ddlCanal');
    var vIsPortabilidad = getValue('hidNTienePortabilidadValues');


    PageMethods.consultarLineasAdicionales(vTipoDocumento, vNumeroDocumento, vCanal, vIsPortabilidad, consultarLineasAdicionales_Callback);
    //PageMethods.obtenerLineasCliente(vTipoDocumento, vNumeroDocumento, vCanal, vIsPortabilidad, obtenerLineasCliente_Callback);
}

function obtenerFlujo() {
    var strTienePortabilidad = document.getElementById('hidNTienePortabilidadValues').value;
    var strFlujo = flujoAlta;

    if (strTienePortabilidad == 'S')
        strFlujo = flujoPortabilidad;

    return strFlujo;
}

function cambiarTipoOperacion(ddl) {
    if (getValue('hidCadenaDetalle') != '') {
        if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
            ddl.value = getValue('hidTipoOperacion');
            return;
        }
    }
    //Inicio PROY-25335 -  Contratación Electronica - Release 0
    if (ddl.value != 1 && ddl.value != 2) { // PROY-25335 - Contratacion Electronica - Release 2
        document.getElementById('hidCartaPoder').value = 'N';
        tblCartaPoder.style.display = 'none';
    } else {
        if (cartaPoder == '1') {
            tblCartaPoder.style.display = '';
            document.getElementById('hidCartaPoder').value = document.getElementById('chkCartaPoder').checked == true ? 'S' : 'N';
        }
    } //Fin PROY-25335 -  Contratación Electronica - Release 0

    //PROY-140245

    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);

    if (flagInicCantProd && strCasoEspecialColab.search(strCodCasoEspecial) >= 0 && strCasoEspecialColab != '' && strCasoEspecialColab != null && strCodCasoEspecial != '') {
        restablecerCantProdActCasoEspColab();
    }
    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    //FIN PROY-140245
    setValue('hidTipoOperacion', ddl.value);

    document.getElementById('ddlOferta').selectedIndex = 0;
    document.getElementById('ddlCasoEspecial').selectedIndex = 0;
    document.getElementById('ddlCombo').selectedIndex = 0;
    document.getElementById('ddlModalidadVenta').selected = getValue('hdModalidaDefecto'); // INICIATIVA 920
    inicializarDatosCasoEspecial();
    inicializarPanelCondicionVentaII();

    /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
    if (validarOfertaDefault()) {
        $('#ddlOferta').val(Key_CodigoOfertaDefault);
    }
    /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

    cambiarTipoOferta(document.getElementById('ddlOferta'));

    //Proy 29123
    consultarClienteBRMS();
}

function cambiarTipoOferta(ddl) {
    //Proy-32129 FASE 2 INICIO
    PageMethods.obtenerListaInstituciones(obtenerListaInstituciones_Callback);
    //Proy-32129 FASE 2 FIN
    if (getValue('hidCadenaDetalle') != '') {
        if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
            ddl.value = getValue('hidnTipoOfertaValue');
            return;
        }
    }
    //PROY-140245
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);

    if (flagInicCantProd && strCasoEspecialColab.search(strCodCasoEspecial) >= 0 && strCasoEspecialColab != '' && strCasoEspecialColab != null && strCodCasoEspecial != '') {
        restablecerCantProdActCasoEspColab();
    }

    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    //FIN PROY-140245 
    setValue('hidnTipoOfertaValue', ddl.value);

    document.getElementById('ddlCasoEspecial').selectedIndex = 0;
    document.getElementById('ddlCombo').selectedIndex = 0;

    
    inicializarDatosCasoEspecial();
    inicializarPanelCondicionVentaII();

    //INICIATIVA 920
    PageMethods.LlenadoModalidades(getValue('ddlTipoOperacion'), getValue('hidNTienePortabilidadValues'), LlenadoModalidades_Callback); 

    //PROY-140743 - IDEA-141192 - Venta en cuotas | INICIO
    var blnPortabilidad = document.getElementById('chkPortabilidad').checked;
    var ddlModalidadVenta = document.getElementById('ddlModalidadVenta');
    var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');
    var ddlOferta = document.getElementById('ddlOferta');

    if (blnPortabilidad || Key_FlagGeneralVtaCuotas == "0" || getValue('hidFlagVentaVV') == "0" || getValue('hidIsClienteClaro') == "0") {
        $("#ddlTipoOperacion" + " option[value='25']").remove();
    }

    if (getValue('hidIsClienteClaro') == "1" && getValue('hidFlagVentaVV') == "1") {
        if (ddlTipoOperacion.value == 25 || ddlTipoOperacion.value == '25') {
            ddlModalidadVenta.value = 3;
            ddlModalidadVenta.disabled = true;
            ddlOferta.value = '01';
            ddlOferta.disabled = true;
            setValue('hidnTipoOfertaValue', ddlOferta.value);
        }
    } 

    if (ddlTipoOperacion.value != 25 || ddlTipoOperacion.value != '25') {
        ddlModalidadVenta.disabled = false;
        ddlOferta.disabled = false;
    }
    //PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

    if (ddl.value == '') {
        llenarDatosCombo(document.getElementById('ddlCasoEspecial'), '', true);
        llenarDatosCombo(document.getElementById('ddlCombo'), '', true);
        self.frames["ifraCondicionesVenta"].window.location.reload();
    } else {
        cargarImagenEsperando();
        //INCIATIVA 920 cambio de getValue('hdModalidaDefecto')
        PageMethods.cambiarTipoOferta(getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                              getValue('txtNroDoc'), getValue('hidnOficinaValue'), getValue('hdModalidaDefecto'), cambiarTipoOferta_Callback);    
    }
   

    }
//INICIATIVA 920
function LlenadoModalidades_Callback(objResponse) {
    var ddlModalidadVenta = document.getElementById('ddlModalidadVenta');
    llenarDatosCombo(ddlModalidadVenta, objResponse.Cadena, true);
    setValue('hdModalidaDefecto', objResponse.IdFila); //MODALIDAD POR DEFECTO
    setValue('ddlModalidadVenta', objResponse.IdFila); //MODALIDAD POR DEFECTO
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
    document.getElementById('ddlModalidadVenta').value = getValue('hdModalidaDefecto'); //INICIATIVA 920
    inicializarDatosCasoEspecial();
    inicializarPanelCondicionVentaII();

    cargarImagenEsperando();

    var strCasoEspecial = getValor(ddl.value, 0);
    var strWhiteList = '';

    if (strCasoEspecial != '') strWhiteList = getValor(ddl.value, 1);

    PageMethods.cambiarCasoEspecial(getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                            getValue('txtNroDoc'), strCasoEspecial, strWhiteList, getValue('ddlModalidadVenta'), cambiarCasoEspecial_Callback);
}
//PROY-140245
function validarAutogestion() {
    var strDocCliente = getValue('txtNroDoc');
    PageMethods.validarAutogestionVenta(strDocCliente, validarAutogestionVenta_CallBack);
}
//FIN PROY-140245
function validarAutogestionVenta_CallBack(objResponse) {
    var blnAutogestion = objResponse;
    var dllCasoEspecial = document.getElementById('ddlCasoEspecial');
    if (blnAutogestion) {
        dllCasoEspecial.selectedIndex = 0;
        alert(strMsgAutogestion);
        inicializarDatosCasoEspecial();
        self.frames["ifraCondicionesVenta"].window.location.reload();
        return;
    }
}

function cambiarCasoEspecial_Callback(objResponse) {
    quitarImagenEsperando();
    autoSizeIframe();

    setValue('hidListaTipoProducto', objResponse.LISTA_PRODUCTOS);
    //PROY- 140245
    setValue('hidcasosEspecialesColaborador', objResponse.COD_CASO_ESPECIAL_COLAB);
    strCasoEspecialColab = getValue('hidcasosEspecialesColaborador');

    var strValidarCampEspColab = "0"; //Valida si el caso especial seleccionado es de Colaborador Claro
    // FIN  PROY- 140245
    var blnWhiteList = objResponse.FLAG_WHITELIST;
    var dblCFMaximo = objResponse.CARGO_FIJO_MAX;
    var listaCEPlanBscs = objResponse.PLANES_BSCS;
    var listaCEPlan = objResponse.PLANES_SISACT;
    var listaCEPlanxProd = objResponse.PLANES_X_PRODUCTO;
    var listaCENroPlanxProd = objResponse.NRO_PLANES_X_PRODUCTO;

    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var whiteList = getValor(ddlCasoEspecial.value, 1);

    //PROY- 140245       
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);

    if (strCasoEspecialColab.search(strCodCasoEspecial) < 0 || strCodCasoEspecial == '') {

        flagInicCantProd = false;
        var numeroDocumento = getValue('txtNroDoc').toString();
        PageMethods.LimpiarVariablesColaborador(numeroDocumento);
    }

    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    //FIN PROY-140245

    if (whiteList == 'S') {
        //proy-140245
        var strCodCasoEspColab = getValor(getValue('ddlCasoEspecial'), 0);
        setValue('hidcasoEspecial', strCodCasoEspColab);
        //fin proy-140245
        if (blnWhiteList != 'S') {
            //PROY-32129 :: INI
            var strValidarCampEspUniv = "0";
            var strCodCasoEspUniv = getValor(getValue('ddlCasoEspecial'), 0);

            if (objResponse.COD_CASO_ESPECIAL_UNIV.search(strCodCasoEspUniv) >= 0) {// PROY-32129 FASE 2 INICIO
                //PROY-140245
                strValidarCampEspColab = "2";
                //FIN PROY-140245                     
                if (confirm(objResponse.PREG_CASO_ESPECIAL_UNIV)) {
                    strValidarCampEspUniv = "1";
                } else {
                    strValidarCampEspUniv = "2";
                }
            }

            if ((strValidarCampEspUniv == "0" || strValidarCampEspUniv == "2") || strValidarCampEspColab == "0") {
                if (strValidarCampEspUniv == "0" && objResponse.COD_CASO_ESPECIAL_UNIV.search(strCodCasoEspUniv) > 0) {
                    //PROY-32129 :: FIN
                    alert('El Nro de documento no se encuentra en la lista del caso especial seleccionado.');
                    //PROY-32129 :: INI
                }
                //PROY-32129 :: FIN
                else if (strValidarCampEspUniv == "2") {
                    inicializarDatosCasoEspecial();
                    ddlCasoEspecial.value = '';
                    self.frames["ifraCondicionesVenta"].window.location.reload();
                    return;
                }
                //PROY-32129 :: INI
                if (strValidarCampEspUniv != '1') {
                    alert(objResponse.MSJ_CASO_ESPECIAL_COLAB_NO_ENCONTRADO);
                    ddlCasoEspecial.selectedIndex = 0;
                }
            }
            //PROY-32129 :: FIN
        }
        else if (blnWhiteList == 'S' && strCasoEspecialColab.search(strCodCasoEspColab) >= 0) {
            //PROY- 140245
            if (strCasoEspecialColab.search(strCodCasoEspColab) >= 0 && strCodCasoEspColab != '') {
                strValidarCampEspColab = "1";
                if (strValidarCampEspColab == "1") {
                    strMsgAutogestion = objResponse.MSJ_CASO_ESPECIAL_COLAB_AUTOGESTION;
                    strMsgValidaCantidadLineas = objResponse.MSJ_CASO_ESPECIAL_COLAB_VALID_CANT_PROD;
                    strCantMaxPorProducto = objResponse.CANT_MAX_POR_PROD_CASO_ESP_COLAB;
                    validarAutogestion();
                }
            }
            //END PROY- 140245

        }
        //PROY-32129 :: INI
        else {
            document.getElementById('hidGraboDatosAlumnos').value = "4";
        }
        //PROY-32129 :: FIN
    }

    setValue('hidCasoEspecial', ddlCasoEspecial.value);
    setValue('hidWhitelistCE', blnWhiteList);
    setValue('hidCargoFijoMaxCE', dblCFMaximo);
    setValue('hidlistaCEPlanBscs', listaCEPlanBscs);
    setValue('hidlistaCEPlan', listaCEPlan);
    setValue('hidlistaCEPlanxProd', listaCEPlanxProd);
    setValue('hidlistaCENroPlanxProd', listaCENroPlanxProd);

    self.frames["ifraCondicionesVenta"].window.location.reload();

    //IDEA-142010 INI
    var strMsgLabelBenAdic = objResponse.BENEFICIOADICIONAL;
    document.getElementById('lblBeneficio').innerHTML = strMsgLabelBenAdic;

    //PROY-32129 :: INI
    PageMethods.obtenerListaInstituciones(obtenerListaInstituciones_Callback);
    //PROY-32129 :: FIN
}

function cambiarCombo(ddl) {
    if (getValue('hidCadenaDetalle') != '') {
        if (!confirm('Se perderá la información del carrito de compras ¿Desea Continuar?')) {
            ddl.value = getValue('hidCombo');
            return;
        }
    }
    //PROY -140245 
    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    //FIN PROY -140245 
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
    InicializarPanelExepcionPrecio(); //INICIATIVA-803
    PageMethods.cambiarTipoModalidad(getValue('hidnOficinaValue'), getValue('ddlTipoOperacion'), getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                             getValue('txtNroDoc'), getValor(getValue('ddlCasoEspecial'), 0), getValue('ddlModalidadVenta'), cambiarTipoModalidad_Callback);
}

function cambiarTipoModalidad_Callback(objResponse) {
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }
    //PROY-140245
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCodCasoEspecial = getValor(getValue('ddlCasoEspecial'), 0);
    if (flagInicCantProd && strCasoEspecialColab.search(strCodCasoEspecial) >= 0 && strCasoEspecialColab != '' && strCasoEspecialColab != null && strCodCasoEspecial != '') {
        restablecerCantProdActCasoEspColab();
    }

    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    //FIN PROY-140245

    var arrResultado = objResponse.Cadena.split('¬');
    var listaTipoProducto = arrResultado[0];
    var listaCombo = arrResultado[1];

    setValue('hidListaTipoProducto', listaTipoProducto);

    var ddlCombo = document.getElementById('ddlCombo');
    llenarDatosCombo(ddlCombo, listaCombo, true);

    self.frames["ifraCondicionesVenta"].window.location.reload();
}

// ******************************************************* EVALUAR ******************************************************* //
/////////////////////fdq////////////////////////////
function validarParametrosServRI() {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][validarParametrosServRI()] ", "Entro en la funcion validarParametrosServRI()");
    
    var ifrCondi = self.frames['ifraCondicionesVenta'];

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][validarParametrosServRI()] ifrCondi.document.getElementById('rbtValDeterminado').checked == false && ifrCondi.document.getElementById('rbtValIndeterminado').checked == false", ifrCondi.document.getElementById('rbtValDeterminado').checked == false && ifrCondi.document.getElementById('rbtValIndeterminado').checked == false);        

    if (ifrCondi.document.getElementById('rbtValDeterminado').checked == false && ifrCondi.document.getElementById('rbtValIndeterminado').checked == false) {
        alert('Seleccione e ingrese parametros para Servicio Roaming Internacional');
        return false;
    }

    ifrCondi.document.getElementById('tblRoamingI').style.display = 'none';
    return true;
}

function ServRoamingEsContratado() {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] ", "Entro en la funcion ServRoamingEsContratado()");

    var ifrConVen = self.frames['ifraCondicionesVenta'];

    var lbxsa = ifrConVen.document.getElementById('lbxserviciosagregados1');
    var strcodsa;
    var contsa = lbxsa.options.length;

    for (var i = 0; i < contsa; i++) {
        strcodsa = lbxsa.options[i].value;
        arrcodsa = strcodsa.split('_');

        var codservselected = arrcodsa[3];
        if (codservselected == codservroamingi) {

            PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] codservselected", codservselected);
            PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] codservroamingi", codservroamingi);

            if (!validarParametrosServRI())
                return;

        }

    }

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] hidnPlanServicioValue", ifrConVen.getValue('hidnPlanServicioValue'));    

    //INICIO PROY-30748
    var strPlanServicio = ifrConVen.getValue('hidnPlanServicioValue');
    var arrPlanServicio = strPlanServicio.split('*ID*');
    var arrServicios;
    var codServicio = '';
    var strCodigosServiciosAdicionales = '';


    for (var x = 1; x < arrPlanServicio.length; x++) {
        arrServicios = arrPlanServicio[x].split('|');
        for (var z = 1; z < arrServicios.length; z++) {
            //Almacenamos los códigos de servicios adicionales
            codServicio = arrServicios[z].split(';')[0].split('_')[3];
            if (x == (arrPlanServicio.length - 1)) {
                if (z == arrServicios.length) {
                    strCodigosServiciosAdicionales += codServicio;
                } else {
                    strCodigosServiciosAdicionales += codServicio + '|';
                }
            }
        }
    }

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] hidServiciosAdicionales antes", $("#hidServiciosAdicionales").val());
    
    //FIN PROY-30748				
    //Asignamos la lista de servicios adicionales al hidden
    $("#hidServiciosAdicionales").val(strCodigosServiciosAdicionales);

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][ServRoamingEsContratado()] hidServiciosAdicionales despues", $("#hidServiciosAdicionales").val());    

    return true;

}

var arrEvaluacion = []; //PROY 30748

/////////////////////fdq////////////////////////////
function validarEvaluacion() {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][validarEvaluacion()] ", "Entro en la funcion validarEvaluacion()");

    //PROY-32129 :: INI
    var listCampSelect = "";

    if (document.getElementById('hidGraboDatosAlumnos').value == "1") {
        listCampSelect = ObtenerCombosCampanas()
    }
    //PROY-32129 :: FIN

    if (!ServRoamingEsContratado()) { return; }
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
    if (getValue('hidNTienePortabilidadValues') == 'S') {
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
        alert(constMsjBolsaCompartidaII);
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

    //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
    if (getValue('hidNroDiasCedenteOP') != '') {
        var strCadenaDetalle = ifr.consultarItem('');
        var arrCadenaDetalle = strCadenaDetalle.split(';');
        var strCampania = '';
        var strNroDiasCedenteOP = getValue('hidNroDiasCedenteOP'); // 1: nro de dias <= 90 
        for (i = 0; i < arrCadenaDetalle.length; i++) {
            strCampania = arrCadenaDetalle[15] // Codigo de Campaña seleccionado, debe ser el mismo para ambos items
        }
        var strCampaniaPorta50Dscto = getValue('hidCodigoCampaniaPorta50Dscto');
        var strMsgPermanenciaOP = getValue('hidMsgPermanenciaOP');
        if (strNroDiasCedenteOP == "1") {
            if (strCampaniaPorta50Dscto.indexOf(strCampania) > -1) {
                alert(strMsgPermanenciaOP);
                quitarImagenEsperando(); return false;
            }
        }
    }
    //FIN: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
    quitarImagenEsperando();

    strTieneProteccionMovil = "NO"; //PROY-24724-IDEA-28174 - INICIO
    validarItemsServicioProteccionMovil() //PROY-24724-IDEA-28174 - FIN
    //PROY 30748 INI
    if (getValue('ddlOferta') == '01' && getValue('hidProdMovil') == '1') {
        //PROY-140743 - INI
        var strOper = getValue('ddlTipoOperacion');
        if (strOper == '25')
            document.getElementById('tdbtnOtrasOpc').style.display = 'none';
        else
            document.getElementById('tdbtnOtrasOpc').style.display = '';
        //PROY-140743 - FIN
    }
    else {
        document.getElementById('tdbtnOtrasOpc').style.display = 'none';
    }

    //            var rowNotVisible = 0;
    //            $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr').each(function () {
    //                if ($(this).css('display') == 'none') {
    //                    rowNotVisible++;
    //                }
    //            });

    var cnttablaMovil = $('#ifraCondicionesVenta').contents().find('#hidFilaProa').val();

    var ifrConVenta = self.frames['ifraCondicionesVenta'];
    var tipoProd = ifrConVenta.getValue('hidTipoProductoActual');

    var descampana, desplazo, desplan;
    var fila = 0;
    var cargofijo = 0;
    var arrRenovacion = new Object();
    var lstPlanes = "";

    switch (tipoProd) {
        case 'Movil':
            fila = cnttablaMovil - 1;
            var objEvaluacion = {
                id: fila + 1,
                campana: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(2) select').val(),
                descampana: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(2) select option:selected').text(),
                plazo: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(3) select').val(),
                desplazo: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(3) select option:selected').text(),
                codplan: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(5) select').val(),
                desplan: $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr:eq(' + fila + ') td:eq(5) select option:selected').text(),
                codequipo: $('#ifraCondicionesVenta').contents().find('#hidValorEquipo' + cnttablaMovil).val(),
                desequipo: $('#ifraCondicionesVenta').contents().find('#txtTextoEquipo' + cnttablaMovil).val(),
                codmodalidad: $('#ddlModalidadVenta').val(),
                lstMaterial: $('#ifraCondicionesVenta').contents().find('#hidMaterial' + cnttablaMovil).val()
            }
            var aux = false;

            $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr').each(function () {
                if ($(this).css('display') != 'none') {
                    aux = true
                }
            });

            if (arrEvaluacion.length <= 4) {
                if (arrEvaluacion == null) {
                    arrEvaluacion[0] = objEvaluacion;
                } else {
                    if (aux)
                        arrEvaluacion[arrEvaluacion.length] = objEvaluacion;
                }
            }

            break;
    }
    //PROY 30748 FIN

    //PROY-32129 :: INI
    if (document.getElementById('hidGraboDatosAlumnos').value == "1") {
        PageMethods.ValidarAnularDatosAlumInst(listCampSelect, getValue('ddlTipoDocumento'), getValue('txtNroDoc'), ValidarAnularDatosAlumInst_callback);
    }
    //PROY-32129 :: FIN
    return true;
}

//INI: PROY-140223 IDEA-140462 
function validarEnvioConsultaPrevia() {
    if (getValue('hidConsFlagConsultaPreviaChip') == 1 && (getValue('hidConsCPModVenta').indexOf(getValue('ddlModalidadVenta')) > -1) && (getValue('hidConsCPCanalVenta').indexOf(getValue('ddlCanal')) > -1
            && (getValue('hidConsCPPuntoVenta').indexOf(getValue('hidOficinaUsuario')) == -1))) {
        return true;
    }
    return false;
}
//FIN: PROY-140223 IDEA-140462

function validarItemsServicioProteccionMovil() //PROY-24724-IDEA-28174 - INICIO
{
    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var strCadenaDetalle = ifr.consultarItem('');
    var strCadenaEquipo = cadenaEquiposDetalle(strCadenaDetalle);
    strCadenaEquipo = strCadenaEquipo.substring(0, strCadenaEquipo.length - 1);
    var arrCadenaEquipo = strCadenaEquipo.split('|');
    var strPlanServicio = ifr.getValue('hidnPlanServicioValue');
    var arrPlanServicio = strPlanServicio.split('*ID*');
    var arrServicio;
    var codServicio;
    var concatEquiposConProteccionMovil = "";
    var codItem;

    for (var a = 1; a < arrPlanServicio.length; a++) {
        arrServicio = arrPlanServicio[a].split('|');
        for (var b = 1; b < arrServicio.length; b++) {
            codServicio = arrServicio[b].split(';')[0].split('_')[3];
            if (codServicio == codServProteccionMovil) {
                for (var c = 0; c < arrCadenaEquipo.length; c++) {
                    if (arrServicio[0] == arrCadenaEquipo[c].split(';')[0])
                        concatEquiposConProteccionMovil += arrCadenaEquipo[c].split(';')[0] + ";" + arrCadenaEquipo[c].split(';')[2] + ";" + arrCadenaEquipo[c].split(';')[3] + "|";
                }
            }
        }
    }

    if (concatEquiposConProteccionMovil.length > 0) {
        concatEquiposConProteccionMovil = concatEquiposConProteccionMovil.substring(0, concatEquiposConProteccionMovil.length - 1);
        PageMethods.ObtenerPrecioListaPrepago(getText('ddlOferta'), getValue('ddlTipoDocumento'), getValue('txtNroDoc'), concatEquiposConProteccionMovil, descServProteccionMovil, getValue('hidCodListaPrecioPrepagoMes'), getValue('hidMontoPrecioPrepago'), msgEquipoPrecioPrepagoMenor, msgErrorProcedurePrecioPrepago, callback_ObtenerPrecioListaPrepago);
    } else {

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        if (getValue('hidNTienePortabilidadValues') == 'S' && (codTipoProductoActual == codTipoProductoMovil || codTipoProductoActual == codTipoProductoBAM)) {
            //INI: PROY-140223 IDEA-140462
            if (!(validarEnvioConsultaPrevia())) {
                if (isVisible('tdCarrito')) {
                    var arrMensajesCPCarrito = getValue('hidConsMensajesCPCarrito').split('|');
                    var result = ifr.validarCPLineasPortabilidad();
                    if (result == 0 || result == 1) {
                        if (result == 1) {
                            if (arrMensajesCPCarrito != null && arrMensajesCPCarrito.length > 1) {
                                var lineasRechazadas = ifr.validarCPLineasPortabilidadRechazadas();
                                var mensaje = arrMensajesCPCarrito[0].replace("{0}", lineasRechazadas.lineas);
                                alert(mensaje);
                                ifr.eliminarLineasCPPortabilidad(lineasRechazadas.PkPortabilidad);
                            }
                        }
                    }
                    else {
                        if (arrMensajesCPCarrito != null && arrMensajesCPCarrito.length > 1) {
                            alert(arrMensajesCPCarrito[1]);
                        }
                    }
                }
            }
        }
        //FIN: PROY-140223 IDEA-140462
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        if (getValue('ddlTipoOperacion') == constTipoOperMigracion || getValue('hidNTienePortabilidadValues') == 'S')
            validarNroTelefono(codTipoProductoActual);
        else
            consultaReglasCreditos();
    }
}

function callback_ObtenerPrecioListaPrepago(objResponse) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()]", "Entro a la funcion callback_ObtenerPrecioListaPrepago()");

    if (objResponse != undefined) {
        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()] objResponse ", JSON.stringify(objResponse));
    } else {
        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()] objResponse ", "objResponse es undefined");
    }
    
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }
    else {
        var ifr = self.frames['ifraCondicionesVenta'];
        var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
        var strConcatItemsPopUp = "";
        var strConcatSinCobertura = "";
        var strConcatPrecioPrepagoMenor = "";
        var strConcatErrorProcedurePrecioPrepago = "";
        var strMsgErrorServicioClientePM = "";

        var arrItemsRespuesta = objResponse.Cadena.split('|');
        var arrDetalleItemRespuesta;
        var strConcatPrima = "";
        var strConcatServicioPM = "";
        var codServicio;

        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()] hidnPlanServicioValue ", ifr.getValue('hidnPlanServicioValue'));

        var strPlanServicio = ifr.getValue('hidnPlanServicioValue');
        var arrPlanServicio = strPlanServicio.split('*ID*');
        var arrServicios;
        var strNuevoPlanServicio = "";

        var strPlanServicioNo = ifr.getValue('hidPlanServicioNo');
        var arrPlanServicioNo = strPlanServicioNo.split('*ID*');
        var strNuevoPlanServicioNo = "";

        for (var x = 1; x < arrPlanServicio.length; x++) {
            arrServicios = arrPlanServicio[x].split('|');

            for (var y = 0; y < arrItemsRespuesta.length; y++) {

                if (arrServicios[0] == arrItemsRespuesta[y].split(';')[0]) {
                    arrDetalleItemRespuesta = arrItemsRespuesta[y].split(';');

                    if (arrDetalleItemRespuesta[4] == 'C') {
                        strConcatItemsPopUp += arrDetalleItemRespuesta[2] + ";S/." + arrDetalleItemRespuesta[5] + ";" + arrDetalleItemRespuesta[6] + "|";
                        strConcatPrima += arrDetalleItemRespuesta[0] + ";" + arrDetalleItemRespuesta[5] + ";" + arrDetalleItemRespuesta[6] + ";" +
                                                  arrDetalleItemRespuesta[7] + ";" + arrDetalleItemRespuesta[8] + ";" + arrDetalleItemRespuesta[9] + ";" +
                                                  arrDetalleItemRespuesta[10] + ";" + arrDetalleItemRespuesta[11] + "|";
                        strConcatServicioPM = "";
                        strTieneProteccionMovil = "SI";

                    } else {
                        for (var z = 1; z < arrServicios.length; z++) { //QUITAR PROTECCION MOVIL
                            codServicio = arrServicios[z].split(';')[0].split('_')[3];
                            if (codServicio == codServProteccionMovil) {
                                arrServicios.splice(z, 1);
                                z--;
                            }
                        }
                        strConcatServicioPM = '|' + ifr.strServicioProteccionMovil;

                        if (arrDetalleItemRespuesta[4] == 'N') {
                            strConcatSinCobertura += String.format(msgEquipoSinCobertura, arrDetalleItemRespuesta[2]);
                        } else if (arrDetalleItemRespuesta[4] == 'P') {
                            strConcatPrecioPrepagoMenor += String.format(msgEquipoPrecioPrepagoMenor, arrDetalleItemRespuesta[2], montoPrecioPrepago, "\n");
                        } else if (arrDetalleItemRespuesta[4] == 'EP') {
                            strConcatErrorProcedurePrecioPrepago += String.format(msgErrorProcedurePrecioPrepago, arrDetalleItemRespuesta[2]);
                        } else if (arrDetalleItemRespuesta[4] == 'ES') {
                            strMsgErrorServicioClientePM = msgErrorServicioClientePM;
                        } else {
                            strMsgErrorServicioClientePM = msgErrorServicioClientePM;
                        }
                    }
                    break;
                } else
                    strConcatServicioPM = "";
            }
            strNuevoPlanServicio += "*ID*" + arrServicios.join('|');
            strNuevoPlanServicioNo += "*ID*" + arrPlanServicioNo[x] + strConcatServicioPM;
        }

        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()] strConcatPrecioPrepagoMenor.length > 0 || strConcatSinCobertura.length > 0 || strMsgErrorServicioClientePM.length > 0 || strConcatErrorProcedurePrecioPrepago.length > 0 ", strConcatPrecioPrepagoMenor.length > 0 || strConcatSinCobertura.length > 0 || strMsgErrorServicioClientePM.length > 0 || strConcatErrorProcedurePrecioPrepago.length > 0);
        
        if (strConcatPrecioPrepagoMenor.length > 0 || strConcatSinCobertura.length > 0 || strMsgErrorServicioClientePM.length > 0 || strConcatErrorProcedurePrecioPrepago.length > 0) {
            ifr.setValue('hidnPlanServicioValue', strNuevoPlanServicio);
            ifr.setValue('hidPlanServicioNo', strNuevoPlanServicioNo);
        }

        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][callback_ObtenerPrecioListaPrepago()] strNuevoPlanServicio ", strNuevoPlanServicio);


        if (strConcatPrecioPrepagoMenor.length > 0)
            alert(strConcatPrecioPrepagoMenor);

        if (strConcatSinCobertura.length > 0)
            alert(strConcatSinCobertura);

        if (strConcatErrorProcedurePrecioPrepago.length > 0)
            alert(strConcatErrorProcedurePrecioPrepago);

        if (strMsgErrorServicioClientePM.length > 0)
            alert(strMsgErrorServicioClientePM);

        if (strConcatItemsPopUp.length > 0) {
            strConcatPrima = strConcatPrima.substring(0, strConcatPrima.length - 1);
            strConcatItemsPopUp = strConcatItemsPopUp.substring(0, strConcatItemsPopUp.length - 1);
            ifr.setValue('hidConcatPrima', strConcatPrima);
            ifr.setValue('hidConcatItemsPopUp', strConcatItemsPopUp);
            mostrarPopSeguros(strConcatItemsPopUp);
        }

        if (getValue('ddlTipoOperacion') == constTipoOperMigracion || getValue('hidNTienePortabilidadValues') == 'S') validarNroTelefono(codTipoProductoActual);
        else consultaReglasCreditos();
    }
}

function mostrarPopSeguros(strConcatItemsPopUp) {
    var cantItems = strConcatItemsPopUp.split('|').length;
    var altoVentana = "82";
    if (cantItems == 2) altoVentana = "96";
    if (cantItems == 3) altoVentana = "110";
    strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("['ñ']", 'g'), '=');
    strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("[' ']", 'g'), '_');
    var opciones = "dialogHeight: " + altoVentana + "px; dialogWidth: 500px; edge: raised; center: yes; resizable: no; status: no; scroll: no";
    var url = 'sisact_pop_seguros.aspx?concatSeguros=' + strConcatItemsPopUp;
    window.showModalDialog(url, '', opciones);
} //PROY-24724-IDEA-28174 - FIN

function validarNroTelefono(codTipoProductoActual) {
    var listaTelefono = listaTelefonosEvaluados();

    if (getValue('hidNTienePortabilidadValues') == 'S') {
        if (!validarNroProductosPorta(codTipoProductoActual)) {
            alert('Solo se puede evaluar 1 Tipo de Producto para el flujo de Portabilidad.');
            return;
        }
    }
    var tipoDocumento = $("option:selected", $("#ddlTipoDocumento")).text();

    //PROY-140618-INI
    var strCadenaDetalleMejPor = self.frames['ifraCondicionesVenta'].consultarItem('');
    setValue('hidCadDetalleMejPor', strCadenaDetalleMejPor);
    //PROY-140618-FIN

    /*2016-05-05 CNH*/
    var VarTipoOperacion = getValue('ddlTipoOperacion');
    var VarOferta = getValue('ddlOferta');
    var VarModalidadVenta = getValue('ddlModalidadVenta');

    var VarCodigoCampaign = null;
    if (VarTipoOperacion == constTipoOperMigracion && VarOferta == constCodTipoProductoCON && VarModalidadVenta == constCodModalidadChipSuelto) {
        if (codTipoProductoActual == codTipoProductoMovil || codTipoProductoActual == codTipoProductoBAM) {
            VarCodigoCampaign = GetCodigoCampaigns();
        }
    }
    //INC000001290175-INICIO
    var codTipoDocumento = getValue('ddlTipoDocumento');

    PageMethods.validarNroTelefonoXCliente(codTipoDocumento, tipoDocumento, getValue('txtNroDoc'), listaTelefono, getValue('hidNTienePortabilidadValues'), VarTipoOperacion, VarOferta, VarModalidadVenta, VarCodigoCampaign, validarNroTelefonoXCliente_Callback);
    /*2016-05-05 CNH*/
    //INC000001290175-FIN

    cargarImagenEsperando();
    //PageMethods.validarNroTelefono(getValue('txtNroDoc'), listaTelefono, getValue('hidNTienePortabilidadValues'), getValue('ddlTipoOperacion'), validarNroTelefono_Callback);
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
                //INI PROY-140335           
                var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
                var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + "|" + getValue('ddlModalidadPorta') + "|" + getValue('ddlTipoDocumento') + "|" + getValue('txtNroDoc') + "|" + codTipoProductoActual + "|" + getValue('ddlModalidadVenta');
                
                document.getElementById('hidSecuenceCP').value = '';
                PageMethods.validarNroTelefono(getValue('txtNroDoc'), listaTelefono, getValue('hidNTienePortabilidadValues'), getValue('ddlTipoOperacion'), cadenaConsultaPrevia, validarNroTelefono_Callback);
    }
}

//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
function validarNroTelefono_Callback(objResponse) {
    quitarImagenEsperando();
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }
    //PROY-140335 RF1 INI
    else {

        if (ValidarMejorasPortabilidad()) {

            var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + "|" + getValue('ddlModalidadPorta') + "|" + getValue('ddlTipoDocumento') + "|" + getValue('txtNroDoc');
            var datosPortaCedente
            setValue('hidDatosPorta', cadenaConsultaPrevia);
                var listaTelefono = listaTelefonosEvaluacionCP();
            PageMethods.ValidarRepositorioABDCP(listaTelefono, cadenaConsultaPrevia, consultaReglasCreditos);
            }
            else {

                consultaReglasCreditos();
            }

        }

    //PROY-140335 RF1 INI


}

//PROY-2X1

function ValidarPromocionPortabilidad2x1BRMS_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            ValidarPromocionBeneficioPortaCN_BRMS(); //IDEA-42590
        }
        else {
            quitarImagenEsperando();
            alert(objResponse.DescripcionError);
        }
    }
}

//IDEA-42590
function ValidarPromocionPortabilidad2x1_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            ValidarPromocionBeneficioPortaCN();
        }
        else {
            quitarImagenEsperando();
            setEnabled('btnConsultaPrevia', true, 'Boton');
            alert(objResponse.DescripcionError);
        }
    }
} //PROY-2X1

//IDEA-42590
function ValidarPromocionBeneficioPortaCN() {
    var listaTelefono = listaTelefonosEvaluacionCP();
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual + ";" + document.getElementById('hidNTienePortabilidadValues').value + ";" + document.getElementById('ddlCasoEspecial').value;
    var cadenaDetalleCarrito = obtenerDetalleCarrito();
    PageMethods.ValidarPromocionBeneficioPortaCN("CP", listaTelefono, codTipoProductoActual, cadenaConsultaPrevia, cadenaDetalleCarrito, ValidarPromocionBeneficioPortaCN_CallBack);
}

function ValidarPromocionBeneficioPortaCN_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            var listaTelefono = listaTelefonosEvaluacionCP();
            var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
            var FlagCP_Proa = "0"; //PROY-140335 RF1
            var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual + ";" + getValue('ddlModalidadVenta') + ";" + getValue('ddlCanal') + ";" + getValue('hidOficinaUsuario') + ";" + FlagCP_Proa; //PROY-32089 /PROY-140223 IDEA-140462 /PROY-140335
            PageMethods.RealizarConsultaPrevia(cadenaConsultaPrevia, listaTelefono, RealizarConsultaPrevia_CallBack);
        }
        else {
            quitarImagenEsperando();
            setEnabled('btnConsultaPrevia', true, 'Boton');
            alert(objResponse.DescripcionError);
        }
    }
}

function ValidarPromocionBeneficioPortaCN_BRMS() {
    var listaTelefono = listaTelefonosEvaluacionCP();
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual + ";" + document.getElementById('hidNTienePortabilidadValues').value + ";" + document.getElementById('ddlCasoEspecial').value;
    var cadenaDetalleCarrito = obtenerDetalleCarrito();
    PageMethods.ValidarPromocionBeneficioPortaCN("BRMS", listaTelefono, codTipoProductoActual, cadenaConsultaPrevia, cadenaDetalleCarrito, ValidarPromocionBeneficioPortaCN_BRMS_CallBack);
}

function ValidarPromocionBeneficioPortaCN_BRMS_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            consultaReglasCreditos();
        }
        else {
            quitarImagenEsperando();
            alert(objResponse.DescripcionError);
        }
    }
}


function ValidarPromocionBeneficioPortaCN_SECT() {
    var listaTelefono = listaTelefonosEvaluacionCP();
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual + ";" + document.getElementById('hidNTienePortabilidadValues').value + ";" + document.getElementById('ddlCasoEspecial').value;
    var cadenaDetalleCarrito = obtenerDetalleCarrito();
    PageMethods.ValidarPromocionBeneficioPortaCN("SECT", listaTelefono, codTipoProductoActual, cadenaConsultaPrevia, cadenaDetalleCarrito, ValidarPromocionBeneficioPortaCN_SECT_CallBack);
}

function ValidarPromocionBeneficioPortaCN_SECT_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            grabarSEC();
        }
        else {
            quitarImagenEsperando();
            alert(objResponse.DescripcionError);
            habilitarBoton('btnGrabar', 'Grabar', true);
            return;
        }
    }
}
//IDEA-42590

//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
var timer = null;
var recordCP = 0;
var recordTotalCP = 1;

function RealizarConsultaPrevia_CallBack(objResponse) {

    if (document.getElementById('btnConsultaPrevia').value != "Ver resultado CP") {
        recordCP = 1;
    }
    else {
        recordCP = recordCP + 1;
    }

    if (objResponse == null) {
        setEnabled('btnConsultaPrevia', true, 'Boton');
        quitarImagenEsperando();
    }

    var repeticiones = parseInt(objResponse.Objeto[0]);
    var frecuencia = parseInt(objResponse.Objeto[1]);
    recordTotalCP = repeticiones;
    var mensajeRecpecionCP = objResponse.Mensaje.split('|');

    if (objResponse.CodigoError == "0") {
        if (recordCP == 1) {
            limpiarConsultaPrevia();
            alert(mensajeRecpecionCP[1]);
        }
        timer = setTimeout(function () { RealizarConsultaPreviaCallBack(objResponse); }, frecuencia);
    }
    else if (objResponse.CodigoError == "3") {
        document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
        var result = recuperarConsultaPrevia(objResponse.Cadena);
        quitarImagenEsperando();
        if (result.d.CodigoError == "0") {
            getRespuesta = 0;
            document.getElementById('hidLineasSinCP').value = result.d.LineasSinCP; //PROY-140335 RF1
            actualizarConsultaPrevia(result.d.Objeto);
            //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
            var arrObjetoCP = result.d.Objeto;
            for (i = 0; i < arrObjetoCP.length; i++) {
                var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                var dif = fFechaEnvioCP - fFechaActivacionCP;
                var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                // Validar que los dias en operador cedente sean menores a 90 dias
                //140335 - INICIO
                //var diasPermitidos = parseInt($("#<%= hidNroDiasPermitidosOP.ClientID %>").val());
                var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);
                //140335 - FIN
                if (diasDif <= diasPermitidos) {
                    //140335 - INICIO
                    //$("#<%= hidNroDiasCedenteOP.ClientID %>").val('1');
                    document.getElementById('hidNroDiasCedenteOP').value = '1';
                    //140335 - FIN
                }
            }
            //FIN: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
            document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
        }
        else {
            document.getElementById('btnConsultaPrevia').value = "Ver resultado CP";
            limpiarConsultaPrevia();
            setEnabled('btnConsultaPrevia', true, 'Boton');
            alert(mensajeRecpecionCP[2]);
        }
    }
    else {
        quitarImagenEsperando();
        setEnabled('btnConsultaPrevia', true, 'Boton');
        document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
        document.getElementById('hidEjecucionCPBRMS').value = '0';//PROY-140335 RF1 JCC
        alert(objResponse.DescripcionError);
    }
}


function RealizarConsultaPreviaCallBack(objResponse) {
    var blnSalir = 0;
    if (objResponse.CodigoError == "0" || objResponse.CodigoError == "3") {
        var mensajeRecpecionCP = objResponse.Mensaje.split('|');
        if (objResponse.CodigoError == "0") {
            var repeticiones = parseInt(objResponse.Objeto[0]);
            var frecuencia = parseInt(objResponse.Objeto[1]);
            var getRespuesta = 1;
            document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
            var result = recuperarConsultaPrevia(objResponse.Cadena);
            if (result.d.CodigoError == "0") {
                getRespuesta = 0;
                document.getElementById('hidLineasSinCP').value = result.d.LineasSinCP; //PROY-140335 RF1
                actualizarConsultaPrevia(result.d.Objeto);
                //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                var arrObjetoCP = result.d.Objeto;
                for (i = 0; i < arrObjetoCP.length; i++) {
                    var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                    var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                    var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                    var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                    var dif = fFechaEnvioCP - fFechaActivacionCP;
                    var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                    // Validar que los dias en operador cedente sean menores a 90 dias
                    //140335 - INICIO
                    //var diasPermitidos = parseInt($("#<%= hidNroDiasPermitidosOP.ClientID %>").val());
                    var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);
                    //140335 - FIN
                    if (diasDif <= diasPermitidos) {
                        //140335 - INICIO
                        //$("#<%= hidNroDiasCedenteOP.ClientID %>").val('1');
                        document.getElementById('hidNroDiasCedenteOP').value = '1';
                        //140335 - FIN
                    }
                }
                //FIN: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
                blnSalir = 1;
            }
            //INI: PROY-140335 RF1 EJRC
//            else {
//                actualizarConsultaPrevia(result.d.Objeto);
//                document.getElementById('tdCarrito').style.display = "none";
//                var ifr = self.frames['ifraCondicionesVenta'];
//                ifr.estructuraGrillaPortaCP();
//            }
            //INI: PROY-140335 RF1 EJRC

            quitarImagenEsperando();
            if (getRespuesta == 1 && repeticiones == recordTotalCP) {
                document.getElementById('btnConsultaPrevia').value = "Ver resultado CP";
                setEnabled('btnConsultaPrevia', true, 'Boton');
                limpiarConsultaPrevia();
            }
        }
        else {
            if (objResponse.Cadena != null) {
                quitarImagenEsperando();
                document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
                var result = recuperarConsultaPrevia(objResponse.Cadena);
                if (result.d.CodigoError == "0") {
                    actualizarConsultaPrevia(result.d.Objeto);
                    blnSalir = 1;
                    //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                    var arrObjetoCP = result.d.Objeto;
                    for (i = 0; i < arrObjetoCP.length; i++) {
                        var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                        var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                        var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                        var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                        var dif = fFechaEnvioCP - fFechaActivacionCP;
                        var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                        // Validar que los dias en operador cedente sean menores a 90 dias
                        //140335 - INICIO
                        //var diasPermitidos = parseInt($("#<%= hidNroDiasPermitidosOP.ClientID %>").val());
                        var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);
                        //140335 - FIN
                        if (diasDif <= diasPermitidos) {
                            //140335 - INICIO
                            //$("#<%= hidNroDiasCedenteOP.ClientID %>").val('1');
                            document.getElementById('hidNroDiasCedenteOP').value = '1';
                            //140335 - FIN
                        }
                    }
                    //FIN: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                } else {
                    setEnabled('btnConsultaPrevia', true, 'Boton');
                    document.getElementById('btnConsultaPrevia').value = "Ver resultado CP";
                    limpiarConsultaPrevia();
                    alert(mensajeRecpecionCP[1]);
                }
            }
        }
    }

    if (blnSalir == 1) {
        clearTimeout(timer);
        //PROY-140335 RF1 INI
        var hidlineaPortaCPOK = getValue('hidlineaPortaCPOK');
        if (hidlineaPortaCPOK == '0') {
            consultaReglasCreditos();
        }
        //PROY-140335 RF1 FIN
         
    }
    else if (recordCP == recordTotalCP) {
        clearTimeout(timer);
        //PROY-140335 RF1 INI
        cargarImagenEsperando();
        var MensajeEspera = 'Por favor espere un momento para obtener la respuesta de la consulta previa.';
        alert(MensajeEspera);
        setTimeout(function () {
            var result = recuperarConsultaPrevia(objResponse.Cadena);
            quitarImagenEsperando();
            if (result.d.CodigoError == "0") {
                actualizarConsultaPrevia(result.d.Objeto);
                var hidlineaPortaCPOK = getValue('hidlineaPortaCPOK');
                if (hidlineaPortaCPOK == '0') {
                    consultaReglasCreditos();
                }
            }
        }, 60000);
         
         //PROY-140335 RF1 FIN
    }
    else {
        cargarImagenEsperando();
        setEnabled('btnConsultaPrevia', false, '');
        RealizarConsultaPrevia_CallBack(objResponse);
    }
}

function recuperarConsultaPrevia(strNumeroSecuencialCP) {
    var result = null;
    $.ajax({
        type: "POST",
        url: "sisact_evaluacion_unificada.aspx/RecuperarConsultaPrevia",
        data: "{'strNumeroSecuencialCP':'" + strNumeroSecuencialCP + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (objResponse) {
            result = objResponse;
        },
        error: function (objResponse) {
            result = objResponse;
        }
    });
    return result;
}

function actualizarConsultaPrevia(listaConsultaPrevia) {
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var ifr = self.frames['ifraCondicionesVenta'];
    var tabla = ifr.document.getElementById('tblTabla' + ifr.getValue('hidTipoProductoActual'));
    var cont = tabla.rows.length;
    for (var idFila = 0; idFila < cont; idFila++) {
        var fila = tabla.rows[idFila];
        var idColumn = fila.cells[0].id;
        var linea = fila.cells.namedItem(idColumn + "txtNroTelefono").getElementsByTagName("input")[0].value;
        var registroCP = obtenerRegistroConsultaPrevia(linea, listaConsultaPrevia);
        //PROY-140335 RF1 - EJRC INICIO COMENTADO
        //OBTENER REGISTRO DE LA CONSULTA PREVIA
        fila.cells.namedItem(idColumn + "txtEstadoCP").getElementsByTagName("input")[0].value = (registroCP.descripcionMotivoCP == null || registroCP.descripcionMotivoCP == "") ? registroCP.descripcionEstadoCP : registroCP.descripcionMotivoCP;
        fila.cells.namedItem(idColumn + "txtFlagCPPermitida").getElementsByTagName("input")[0].value = registroCP.flagCPPermitida;
        fila.cells.namedItem(idColumn + "txtIdPortabilidad").getElementsByTagName("input")[0].value = registroCP.idPortabilidad;
        //fila.cells.namedItem(idColumn + "txtFecActivacionCP").getElementsByTagName("input")[0].value = registroCP.fechaActivacionCP;
        //fila.cells.namedItem(idColumn + "txtDeudaCP").getElementsByTagName("input")[0].value = registroCP.deudaCP;
        //PROY-140335 RF1 - EJRC FIN
    }

    var arrMensajesCPCarrito = getValue('hidConsMensajesCPCarrito').split('|');
    var result = ifr.validarCPLineasPortabilidad();
    if (result == 0 || result == 1) {
        document.getElementById('tdCarrito').style.display = '';
        //PROY-140335 RF1 INI
        setValue('hidlineaPortaCPOK', result);  
        if (result == 1) {
            var lineasRechazadas = getValue('hidLineasRec');
            var MensajesCPCarrito = 'Las Líneas {0} no cumplieron con las condiciones para porta, revisar resultado.';
            var mensajeRes = MensajesCPCarrito.replace("{0}", lineasRechazadas);
        ifr.estructuraGrillaPortaCP();
        alert(mensajeRes);
        }

    }
    else {
        if (arrMensajesCPCarrito != null && arrMensajesCPCarrito.length > 1) {
             ifr.estructuraGrillaPortaCP();
            var MensajeRech = 'Consulta Previa rechazada. No se puede continuar con la evaluación.';
            alert(MensajeRech);
            //alert(arrMensajesCPCarrito[1]);
            setValue('hidlineaPortaCPOK', '2');
            //PROY-140335 RF1 FIN
        }
        //PROY-140618-INI
        if (nroRegistroEvaMejPorta == "0") {
            RegistroMejorasPortabilidad("","");
        }
        //PROY-140618-FIN
    }
    setEnabled('btnConsultaPrevia', false, '');
    setEnabled('btnAgregarPlan', false, '');
}
//PROY-140618-INI
function RegistroMejorasPortabilidad(strDatosEvaluacion, strResultado) {

    var strCadenaDetalle = getValue('hidCadDetalleMejPor');
    if (strCadenaDetalle == "") {
        strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    }

    var nroDocumento = document.getElementById('txtNroDoc').value;
    var strCadPlan = cadenaPlanesDetalle(strCadenaDetalle);
    var strCadEquipo = cadenaEquiposDetalle(strCadenaDetalle);
    var strCadDatos = cadenaGeneral();
    var strCadServ = "";
    var strResulEval = getValue('txtResultado');
    if (strResultado != "")
        strResulEval = strResultado;
    var strTipoOperacionMejPor = getValue('ddlTipoOperacion');
    var strTipoProductoMejPor = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var strServicioAgre = self.frames['ifraCondicionesVenta'].getValue('hidnPlanServicioValue');
    var arrServAgre = strServicioAgre.split('|');
    for (var j = 1; j < arrServAgre.length; j++) {
        var arrFilaSerAgre = arrServAgre[j].split(';');
        strCadServ += arrFilaSerAgre[1] + '|';
    }
    if (Key_TipoOperacionPermi.indexOf(strTipoOperacionMejPor) > -1 && Key_TipoProductoPermi.indexOf(strTipoProductoMejPor) > -1) {
        PageMethods.RegistrarEvaluacionMejorasPorta(strDatosEvaluacion, nroDocumento, strCadDatos, strCadServ, strCadPlan, strCadEquipo, strResulEval);
    }
}
//PROY-140618-FIN

function limpiarConsultaPrevia() {
    var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var ifr = self.frames['ifraCondicionesVenta'];
    var tabla = ifr.document.getElementById('tblTabla' + ifr.getValue('hidTipoProductoActual'));
    var cont = tabla.rows.length;
    for (var idFila = 0; idFila < cont; idFila++) {
        var fila = tabla.rows[idFila];
        var idColumn = fila.cells[0].id;
        var linea = fila.cells.namedItem(idColumn + "txtNroTelefono").getElementsByTagName("input")[0].value;
        fila.cells.namedItem(idColumn + "txtEstadoCP").getElementsByTagName("input")[0].value = '';
        fila.cells.namedItem(idColumn + "txtFlagCPPermitida").getElementsByTagName("input")[0].value = '';
        fila.cells.namedItem(idColumn + "txtIdPortabilidad").getElementsByTagName("input")[0].value = 0;
//        fila.cells.namedItem(idColumn + "txtFecActivacionCP").getElementsByTagName("input")[0].value = '';
//        fila.cells.namedItem(idColumn + "txtDeudaCP").getElementsByTagName("input")[0].value = '';
    }
    document.getElementById('hidSecuenceCP').value = '';
}

function obtenerRegistroConsultaPrevia(linea, listregistros) {

    var registrocP = {
        fechaActivacionCP: null,
        descripcionEstadoCP: null,
        descripcionMotivoCP: null,
        flagCPPermitida: null,
        idPortabilidad: null,
        deudaCP: null
    };

    for (var i = 0; i < listregistros.length; i++) {
        if (listregistros[i].numeroLinea != null && listregistros[i].numeroLinea == linea) {
            registrocP.descripcionEstadoCP = listregistros[i].descripcionEstadoCP;
            registrocP.descripcionMotivoCP = listregistros[i].descripcionMotivoCP;
            registrocP.fechaActivacionCP = listregistros[i].fechaActivacionCP;
            registrocP.flagCPPermitida = listregistros[i].flagCPPermitida;
            registrocP.idPortabilidad = listregistros[i].idPortabilidad;
            registrocP.deudaCP = listregistros[i].deudaCP;
            break;
        }
    }
    return registrocP;
}

function realizarConsultaPrevia() {

    //INC000002267567 
    var listaTelefono = listaTelefonosEvaluacionCP();
    var arraytelf1 = listaTelefono.split('|');
    if (arraytelf1[2]) {
        for (var i = 1; i <= arraytelf1.length; i++) {
            for (var j = i + 1; j <= arraytelf1.length; j++) {
                if (arraytelf1[i] == arraytelf1[j]) {
                    alert(consMsjDupLineasCP); //fdq
                    return;
                }
            }
        }
    }


    setEnabled('btnConsultaPrevia', false, '');
    if (!validarEvaluacion()) {
        setEnabled('btnConsultaPrevia', true, 'Boton');
        return;
    }
}

//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
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
    //PROY-140245 
    strFlagCarrito = 'S';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);
    self.frames['ifraCondicionesVenta'].document.getElementById('hidCuotaProa').value = ''; //PROY 30748 F2 MDE
    //INI: PROY-140335 segunda pegada a BRMS RF1
    if (getValue('hidEjecucionCPBRMS') == '1' && ValidarMejorasPortabilidad()) {
        consultaReglasCreditos();
    }
    else if (!validarEvaluacion()) {
        //FIN: PROY-140335 RF1
        return;
    }

            //PROY-140383-INI
            var CodProductosExc = self.frames['ifraCondicionesVenta'].getValue('hidProductosExc');
            var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
            var strServContratados = self.frames['ifraCondicionesVenta'].getValue('hidPlanServicio');
            var strServConExc = self.frames['ifraCondicionesVenta'].getValue('hidServExCont');
            var strServMensCaido = self.frames['ifraCondicionesVenta'].getValue('hidMensServCaido');
            var strServContra = '';
            var strInsServExc = '';
            var strServContExclu = '';
            var strGrupoConTemp = '';
            var strGrupoContEXC = '';

            if (CodProductosExc.indexOf(codTipoProductoActual) > -1) {
                if (strServConExc.length > 0 && strServContratados.length > 0) {                    
                    var arrServContratados = strServContratados.split('*ID*')
                    for (var n = 1; n < arrServContratados.length; n++) {
                        var arrItemPlay = arrServContratados[n].split('|');
                        for (var k = 1; k < arrItemPlay.length; k++) {
                           var arrSerItemDet = arrItemPlay[k].split(';');
                           strServContra += arrSerItemDet[0].split('_')[3] + ',' + arrSerItemDet[0].split('_')[7] + '|';                             
                        }
                    }
                    var arrServContra = strServContra.split('|');
                    for (var o = 0; o < arrServContra.length; o++) {
                        if (strServConExc.indexOf(arrServContra[o].split(',')[0]) >= 0) {
                            if (arrServContra[o].length > 0) {
                                strServContExclu += arrServContra[o] + '|';
                                if (strGrupoConTemp.indexOf(arrServContra[o].split(',')[1]) == -1) {
                                    strGrupoConTemp += arrServContra[o].split(',')[1];
                                } else {
                                    if (strGrupoContEXC.indexOf(arrServContra[o].split(',')[1]) == -1) {
                                        strGrupoContEXC += arrServContra[o].split(',')[1]
                                    }
                                }
                            }
                        }
                    }
                    var arrServContExclu = strServContExclu.split('|');
                    for (var l = 0; l < arrServContExclu.length; l++) {
                        if (strGrupoContEXC.indexOf(arrServContExclu[l].split(',')[1]) >= 0) {
                            strInsServExc += arrServContExclu[l].split(',')[0] + '|';
                        }
                    }
               }
                setValue('hidInsServCon', strInsServExc);
                setValue('hidServicioExcCaidoeval', strServMensCaido);
            }
            //PROY-140383-FIN
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
    //PROY-140335 RF1 INI
    //PageMethods.consultaReglasCreditos(getValue('txtNroDoc'), getValue('hidNroOperacionDC'), strCadenaDatos, strCadenaPlan, strCadenaServicio, strCadenaEquipo, strTieneProteccionMovil, getValue('hidBuroConsultado'), asignarDatosEvaluacion); //PROY-24724-IDEA-28174
    PageMethods.consultaReglasCreditos(getValue('txtNroDoc'), getValue('hidNroOperacionDC'), strCadenaDatos, strCadenaPlan, strCadenaServicio, strCadenaEquipo, strTieneProteccionMovil, getValue('hidBuroConsultado'), getValue('hidProdCuentaFact'), asignarDatosEvaluacionPorta);//PROY-140743
    //PROY-140335 RF1 FIN

}

function consultaReglasCreditosCuotas(idFila, strCadenaDetalle) {
    var strCadenaPlan = cadenaPlanesDetalle(strCadenaDetalle);
    var strCadenaEquipo = cadenaEquiposDetalle(strCadenaDetalle);
    var strCadenaDatos = cadenaGeneral();
    //PROY-140743 - INI
    var txtPromocion = ''
    var strOperacion = document.getElementById('ddlTipoOperacion').value;
    if (strOperacion == '25') {
        txtPromocion = self.frames['ifraCondicionesVenta'].getText('ddlPromocion' + idFila);
    }
    //PROY-140743 - FIN
    cargarImagenEsperando();
    PageMethods.consultaCuota(idFila, getValue('txtNroDoc'), getValue('hidNroOperacionDC'), strCadenaDatos, strCadenaPlan, strCadenaEquipo, strTieneProteccionMovil, getValue('hidBuroConsultado'), txtPromocion, getValue('hidProdCuentaFact'), asignarDatosCuotas); //PROY-24724-IDEA-28174//PROY-140743
}
//PRO-29123 VENTA EN CUOTAS
function asignarDatosCuotas(objResponse) {

    //PROY-140743 - INI
    var strOperacion = document.getElementById('ddlTipoOperacion').value;
    if (objResponse.MensajeErrorBRMS != '' && strOperacion == '25') {
        self.frames['ifraCondicionesVenta'].document.getElementById('tblFormnCuotas').style.display = 'none';
        quitarImagenEsperando();
        alert(objResponse.MensajeErrorBRMS);
        return;
    }
    else {
        self.frames['ifraCondicionesVenta'].document.getElementById('tblFormnCuotas').style.display = 'inline';
    }
    //PROY-140743 - FIN
    
    quitarImagenEsperando();
    if (objResponse.Cadena == '') {
        if (!objResponse.Mensaje || objResponse.Mensaje == '') {
            self.frames['ifraCondicionesVenta'].document.getElementById('tblFormnCuotas').style.display = 'none';
            if (document.getElementById('ddlTipoOperacion').value == '25') {//PROY-140743
                alert(Key_MsjVntaCuotasBRMS);
            } else {
            alert(CM_consMsjNoConfiguracionCuotas);
            }            
            return;
        }
        else {
            var arrDatosMonto = new Array();
            var cuotasMax = 0, montoMax = 0;
            var mensajeBRMS = ""; //Mensaje para mostrar cuotas


            arrDatosMonto = objResponse.Mensaje.split("^");

            cuotasMax = parseInt(arrDatosMonto[0]);
            montoMax = parseFloat(arrDatosMonto[1]);
            mensajeBRMS = arrDatosMonto[2];

            if (mensajeBRMS && mensajeBRMS == "SI") {
                alert('"Cliente no califica a la opcion de cuotas seleccionada"' + '\n' + '"Cliente califica hasta ' + cuotasMax + ' cuotas con un equipo maximo de ' + montoMax.toLocaleString("es-Mx") + ' soles"');
            }
            else if (mensajeBRMS == "NO") {
                //PROY-140743
                if (document.getElementById('ddlTipoOperacion').value == '25') {
                    self.frames['ifraCondicionesVenta'].document.getElementById('tblFormnCuotas').style.display = 'none';
                    alert(Key_MsjVntaCuotasBRMS);
                } else {
                    alert(CM_consMsjNoConfiguracionCuotas);
                }

            }

            self.frames['ifraCondicionesVenta'].document.getElementById('tblFormnCuotas').style.display = 'none';
            return;

        }

    }
    self.frames['ifraCondicionesVenta'].llenarDatosCuota(objResponse.IdFila, objResponse.Cadena);
    //30748 I
    try {
        var arrDatosCuota = new Array();
        var topeMaximoEquipo;
        arrDatosCuota = objResponse.Cadena.split("^");
        topeMaximoEquipo = parseFloat(arrDatosCuota[2]);
        setValue('hidTopeMaximo', topeMaximoEquipo);
    } catch (e) {
        setValue('hidTopeMaximo', '');
    }


    //30748 F
}

function asignarDatosEvaluacion(objResponse) {
    //INI: PROY-140335 RF1
    document.getElementById('tdCarrito').style.display = '';
    document.getElementById('tdConsultaPrevia').style.display = 'none';
    //FIN: PROY-140335 RF1
    document.getElementById('hidResumenCrediticio').value = objResponse.Cadena; //PROY - 30748
    inicializarPanelResultado();
    inicializarPanelLeads(); //PROY-140739
    trComentario.style.display = 'none';
    //PROY-140335 RF1 INI
    var hidFlagCPPermitidaProa = getValue('hidFlagCPPermitidaProa');
    if (hidFlagCPPermitidaProa == 1) {
        habilitarBoton('btnGrabar', 'Grabar', false);
        habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', false);
    }
    else {
    inicializarPanelGrabar();
    }
    //PROY-140335 RF1 FIN
    
// INICIATIVA - 803 - INI
var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;
    var cbocuotas = getValue('ddlModalidadVenta');

if (blnTiendaVirtual) {
    setVisible('tdExcepcionPrecios', true);
}
// INICIATIVA - 803 - FIN

    
    quitarImagenEsperando();
    var montoRA;
    var montoRASeleccionado = getValue('hidMontoRA');
    var strResultadoMejPor = ""; //PROY-140618;

    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        //PROY-140618-INI
        if (nroRegistroEvaMejPorta == "1") {
            nroRegistroEvaMejPorta = "2";
            strResultadoMejPor = "ERROR - ReglaCrediticiaBRMS";
            RegistroMejorasPortabilidad("", strResultadoMejPor);
        }
        //PROY-140618-FIN
        return;
    }

    var arrParam = objResponse.Cadena.split('#');
    strPlanAutonomia = arrParam[0];
    tipoGarantia = arrParam[1];
    strLCDisponible = arrParam[2];
    //30748
    if (getValue('hidMontoRA').length > 0 & getValue('hidMontoRA') != 'undefined') {
        montoRA = getValue('hidMontoRA');
    }
    else {
        montoRA = arrParam[3];
    }
    dblImporte = montoRA;
    //30748
    strPoderes = arrParam[4];
    strTextoLCDisponible = arrParam[5];
    strRiesgoClaro = arrParam[6];
    strComportamiento = arrParam[7];
    strExoneracionRA = arrParam[8];
    totalPlanesProa = arrParam[9]; // PROY 30748
    //PROY-29215 INICIO
    strFormaPago = arrParam[10];
    strNroCuota = arrParam[11];

    document.getElementById('hidFormaPago').value = strFormaPago;
    document.getElementById('hidNroCuotas').value = strNroCuota;
    //PROY-29215 FIN

    strCobroAnticipadoInstalacion = arrParam[15]; //PROY-140546 Cobro Anticipado de Instalacion
    strTipoCobroAnticipadoInstalacion = arrParam[16]; //PROY-140546 Cobro Anticipado de Instalacion

    // Validación Modalidad / Operador Cedente
    if (getValue('hidNTienePortabilidadValues') == 'S') {
        setEnabled('ddlModalidadPorta', false, '');
        setEnabled('ddlOperadorCedente', false, '');
    }

    document.getElementById('hidTipoCobroAnticipadoInstalacion').value = strTipoCobroAnticipadoInstalacion; //PROY-140546
    document.getElementById('hidCobroAnticipadoInstalacion').value = strCobroAnticipadoInstalacion; //PROY-140546

    var ifr = self.frames['ifraCondicionesVenta'];

    if (objResponse.Tipo != 'evalProa') {//PROY 30748
        if (ifr.getValue('hidFlgOrigen') == 'EDIT') {
            ifr.setValue('hidFlgOrigen', '');
        }
        else {
            ifr.guardarItem();
            ifr.agregarCarrito(true);
        }
    }
    //PROY 30748

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
                setValue('txtResultado', constTextoNoAplicaCondiciones);

                /* PROY-140579 RU01 INI RMR */
                strMotivoRestriccion = arrParam[13];
                strMostrarRestriccion = arrParam[14];
                strMostrarRestriccion = strMostrarRestriccion.toUpperCase();

                if (strMostrarRestriccion == 'SI') {
                    trMotivo.style.display = '';
                    document.getElementById('txtMotivo').value = strMotivoRestriccion;
                } else {
                    document.getElementById('txtMotivo').value = '';
                    trMotivo.style.display = 'none';
                }
                /* PROY-140579 RU01 FIN RMR */

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
    document.getElementById('hidnAutonomia').value = strAutonomia;
    document.getElementById('txtTipoGarantia').value = tipoGarantia;
    document.getElementById('txtImporte').value = parseFloat(dblImporte).toFixed(2);
    document.getElementById('txtLCDisponible').value = parseFloat(strLCDisponible).toFixed(2);
    document.getElementById('hidnLCDisponibleValue').value = strLCDisponible;
    document.getElementById('hidPoderes').value = strPoderes;
    document.getElementById('txtRangoLC').value = strTextoLCDisponible;
    document.getElementById('hidnResultadoReglasValues').value = strPlanAutonomia
    document.getElementById('txtRiesgoClaro').value = strRiesgoClaro;
    document.getElementById('txtComportamiento').value = strComportamiento;

    document.getElementById('hidnRiesgoClaroValue').value = strRiesgoClaro;
    document.getElementById('hidnComportamiento').value = strComportamiento;
    document.getElementById('hidnExoneracionRAValues').value = strExoneracionRA;
    document.getElementById('hidTotalPlanProac').value = totalPlanesProa; //PROY - 30748
    // Controles visibles solo para el perfil de Créditos
    if (getValue('hidPerfilCreditos') == 'S') {
        tdLCDisponible.style.display = '';
        tdTxtLCDisponible.style.display = '';
    }

    trResultado.style.display = '';
    trGrabar.style.display = '';
    if (getValue('hidNTienePortabilidadValues') == 'S')
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

    ifr.estructuraGrillaPortabilidad(); //PROY-140335 RF1

    //PROY-29215 INICIO
    ifr.agregarCombo();
    //PROY-29215 FIN

    if (blnTipoDocRUC) {
        if (getValue('hidnAutonomia') == 'S' && blnAutonomiaCE && blnAutonomiaMotivo) {
            trPresentaPoderes.style.display = '';
            trGarantia.style.display = '';
            if (document.getElementById('hidPoderes').value == '1')
                document.getElementById('chkPresentaPoderes').checked = true;

            setValue('txtResultado', constTextoAprobadoAutonomia);
            //IDEA-42590
            trComentario.style.display = '';
        }
        else {
            setValue('txtResultado', constTextoNoAprobadoAutonomia);
            trComentario.style.display = '';
            trGarantia.style.display = 'none';
        }
    }
    else {
        //NO CUMPLE AUTONOMIA: MOTIVO --> Respuesta Tipo 7 y NO tiene excepción DC7
        var blnRespuestaDC7 = (getValue('hidNRespuestaDCValue') == constRespDataCredTipo7);
        var blnAutonomiaDC7 = (!blnRespuestaDC7 || (consultaExcepcionDC7() && blnRespuestaDC7));
        if (!blnAutonomiaDC7)
            setValue('hidCreditosxDC7', 'S');

        if ((blnAutonomiaDC7) && (getValue('hidnAutonomia') == 'S') && (blnAutonomiaCE)) {
            trGarantia.style.display = '';
            //IDEA-42590
            trComentario.style.display = '';
            setValue('txtResultado', constTextoAprobadoAutonomia);
        } else {
            trGarantia.style.display = 'none';
            trComentario.style.display = '';
            setValue('txtResultado', constTextoNoAprobadoAutonomia);
        }
    }

    if (getValue('txtResultado') == constTextoAprobadoAutonomia && dblImporte > 0) {
        document.getElementById('btnDetalleGarantia').style.display = '';
        document.getElementById('btnEnviarCreditos').style.display = '';
    }

    if (ConstflagPlanesProactivos == '0' || getValue('ddlOferta') != '01' || getValue('hidProdMovil') != '1')
        $('#tdbtnOtrasOpc').hide(); //PROY - 30748
    else {
        //PROY-140743 - INI
        var strOper = getValue('ddlTipoOperacion');
        if (strOper == '25')
            $('#tdbtnOtrasOpc').hide();
        else
            $('#tdbtnOtrasOpc').show(); //PROY - 30748
        //PROY-140743 - FIN
        
    }

    //PROY-FULLCLARO.v2-INI
    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var codTipoProdMovil = constTipoProductoMovil; //01
    var codTipoProd3Play = constTipoProductoHFC; //05
    var codTipoProdFTTH = constTipoProductoFTTH; // 09
    var codTipoProd3PlayInalam = constTipoProducto3PlayInalam; //08
    if (codTipoProductoActual == codTipoProdMovil || codTipoProductoActual == codTipoProd3Play || codTipoProductoActual == codTipoProdFTTH || codTipoProductoActual == codTipoProd3PlayInalam) {
        var strPlanesServicios = ObtenerPlanesServicios()
        if (strPlanesServicios != null) {
            evalPlanesServicios(strPlanesServicios)
        }
        else {
            if (getValue('hdnFlagPopupProactiva') == '1') {
                verOtrasOpciones();
            }
        }
    }
    //PROY-FULLCLARO.v2-FIN
    //PROY-140618-INI
    if (nroRegistroEvaMejPorta == "1") {
        nroRegistroEvaMejPorta = "2";
        var strDatosEvaluacion = objResponse.Cadena;
        RegistroMejorasPortabilidad(strDatosEvaluacion,"");
    }
    //PROY-140618-FIN

    //PROY-140546 Cobro Anticipado de Instalacion    
    var strRangoHoras = getValue('hidTiempoSecPendientePagoLink');
    var ifr = self.frames['ifraCondicionesVenta'];
    var CodTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var bEsCanalPermitido = EsValorPermitido( getValue('ddlCanal'), Key_CanalesPermitidosCAI, ",");
    var nCobroAnticipado = parseInt(getValue('hidCobroAnticipadoInstalacion'));

    //(nCobroAnticipado > 0) && SE QUITÓ A SOLICITUD DE ABEL
    
    if (bEsCanalPermitido &&
        (CodTipoProductoActual == codTipoProductoHFC || 
        CodTipoProductoActual == codTipoProd3PlayInalam ||
        CodTipoProductoActual == codTipoProductoFTTH)) {

        trVentanaCobroAnticipadoInstalacion.style.display = 'block';
        llenarCombo(document.getElementById('ddlFranja1'), consTramaDatosFranjaHoraria, true);
        llenarCombo(document.getElementById('ddlFranja2'), consTramaDatosFranjaHoraria, true);
        llenarCombo(document.getElementById('ddlFranja3'), consTramaDatosFranjaHoraria, true);

        if (parseInt(nCobroAnticipado) > 0) { //FALLAS PROY-140546
            llenarCombo(document.getElementById('ddlMedioPago'), Key_CadValorFormaPago, true);
        } else {
            document.getElementById('td7').style.display = 'none';
            document.getElementById('td8').style.display = 'none';
        }

        
    }
    //PROY-140546 Cobro Anticipado de Instalacion

    validarLineasAdicionales();

}

// ******************************************************* EVALUAR ******************************************************* //

// ******************************************************* GRABAR ******************************************************* //
function f_validarGrabar() {
    // Datos Cliente
    if (!validarDatosCliente()) return false;
    // Representante Legal
    if (!validarDatosRRLL()) return false;
    // Planes pendientes de Agregar Carrito
    if (!validarPlanesNoCarrito()) return false;
    // Documentación Portabilidad
    if (getValue('hidNTienePortabilidadValues') == 'S') {
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

    //PROY-140546 Cobro Anticipado de Instalacion
    if (!ValidarCamposCAI()) return false;
    //PROY-140546 Cobro Anticipado de Instalacion
    if (!ModalFullClaro()) {
        return false;
    } else {
        document.getElementById('PanelCarga').style.display = 'none';
    }

    if (!validarLeads()) return false; //PROY-140739 Formulario Leads

    // Grabar Info
    setValue('hidTipoOperacion', getValue('ddlTipoOperacion'));
    setValue('hidCombo', getValue('ddlCombo'));
    setValue('hidComboText', obtenerTextoSeleccionado(document.getElementById('ddlCombo')));
    setValue('hidModalidadVenta', getValue('ddlModalidadVenta'));
    setValue('hidCasoEspecial', getValue('ddlCasoEspecial'));

    setValue('hidDescTipoOperacion', obtenerTextoSeleccionado(document.getElementById('ddlTipoOperacion')));//PROY-140657

    return true;
}

//INI PROY-140739 Formulario Leads
function validarLeads() {
    var tdFormulariosLeads = document.getElementById('tdFormulariosLeads');

	if (tdFormulariosLeads.style.display == '' && KeyLeadsFlag == 'SI')
	{
	    if (getValue('txtFormLead') == '') {
	        alert(KeyLeadsMensajeObligatorio);
		    return false;
	    }
	}
	return true;
}
//FIN PROY-140739 Formulario Leads

function f_grabar() {
    setValue('hidnAdjuntarIngreso', '');
    setValue('hidnCreditosxNombresV', '');
    setValue('hidGrupoPaqueteServerV', '');
    setValue('hidNServicioServerV', '');


    PageMethods.session_hidNServicioServerV("INC000003848031-f_grabar", getValue('hidNServicioServerV')); //INC000003848031
    

    setValue('hidNPromocionServerValue', '');
    setValue('hidEquipoServer', '');
    //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
    var strEmailFacturacion = getValue('txtCorreoElectronico');
    setValue('hdnEmailFacturaElectronica', strEmailFacturacion.toUpperCase());
    //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

    habilitarBoton('btnGrabar', 'PROCESANDO...', false);

    if (!f_validarGrabar()) {
        habilitarBoton('btnGrabar', 'Grabar', true);
        return;
    }

    //PROY-2X1
    if (getValue('hidCampaniaPortabilidad') == "1") {
        var listaTelefono = listaTelefonosEvaluacionCP();
        var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
        var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual;
        var cadenaDetalleCarrito = obtenerDetalleCarrito();
        cargarImagenEsperando();
        PageMethods.ValidarPromocionPortabilidad2x1(getValue('txtNroDoc'), cadenaConsultaPrevia, cadenaDetalleCarrito, getValue('ddlModalidadVenta'), '', ValidarPromocionPortabilidad2x1SEC_CallBack);
    } //PROY-2X1 
    else {
        grabarSEC();
    }

}

//INI PROY-CAMPANA LG

function AgregarCarrito_ValidarComboLG() {

    //PROY-140743 - IDEA-141192 - Venta en cuotas | INI
    var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');
    if ((ddlTipoOperacion.value == 25 || ddlTipoOperacion.value == '25') && (getValue('hidDatosEvalVV') == '' && getValue('hidGuardarLineas') != '1')) {
        alert('Para ejecutar esta accion antes debes guardar los datos de la Linea/Cuenta');
        return;
    }

    if (ddlTipoOperacion.value == 25 || ddlTipoOperacion.value == '25') {
        setEnabled('btnAgregarPlan', false, 'Boton');
    }
    //PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

            nroRegistroEvaMejPorta = "1"; //PROY-140618
            var modcedenteCa = getValue('ddlModalidadPorta') + '_' + getText('ddlModalidadPorta'); //INC000002628010 + 3
            var opecedenCa = getValue('ddlOperadorCedente') + ';' + getText('ddlOperadorCedente'); //INC000002628010 + 3
            var flagPort = getValue('hidNTienePortabilidadValues'); //INC000002628010 + 3
            PageMethods.ValidarModalidadCedente(modcedenteCa, opecedenCa, getValue('txtNroDoc'),flagPort, AgregarCarrito_ValidarModalidadCedente_callback); //INC000002628010 + 3
}


function AgregarCarrito_ValidarModalidadCedente_callback(response) { //INC000002628010 + 3
	if (response.Boleano) {
		if (response.CodigoError != "0") { 
			alert(response.Cadena);
			return;
			
		}
     }

    var oferta = getValue('ddlOferta');
    var operacion = getValue('ddlTipoOperacion');
    var codTipoProdActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    if (oferta == '01' && codTipoProdActual == '01' && operacion != '4') {
        setValue('hdnFlagPopupProactiva', '1');
    } else {
        setValue('hdnFlagPopupProactiva', '0');
    }

    document.getElementById("btnFullClaro2").style.display = "none";  //FullClaro.v2
    //INC000002267567 
    if (validarEnvioConsultaPrevia()) {
        var listaTelefono = listaTelefonosEvaluacionCP();
        var arraytelf1 = listaTelefono.split('|');
        if (arraytelf1[2]) {
            for (var i = 1; i <= arraytelf1.length; i++) {
                for (var j = i + 1; j <= arraytelf1.length; j++) {
                    if (arraytelf1[i] == arraytelf1[j]) {
                        alert(consMsjDupLineasCP);
                        return;
                    }
                }
            }
        }
    }

    //IDEA-142010 INICIO
    var VarTipoOperacion = getValue('ddlTipoOperacion');
    var cantCampanas = self.frames['ifraCondicionesVenta'].obtenerDetalleCarritoCampana(); //obtenerDetalleCarritoPorta();
    setValue('hdiValidacionCantidadCampanas', 'S');
    if (cantCampanas != '') {
        PageMethods.ConsultarCantidadCampanasVigentes(cantCampanas, getValue('txtNroDoc'), VarTipoOperacion, function (objResponse) {
            if (objResponse.Boleano) {
                var strValidacionCantidadCampanas = getValue('hdiValidacionCantidadCampanas');
                if (strValidacionCantidadCampanas == 'S') {
                    setValue('hidCodigoValidador', '');
                    var cadenaDetalleCarrito = obtenerDetalleCarritoPorta();
                    var arrDet = cadenaDetalleCarrito.split('|');
                    var blnOK = false;

                    for (var i = 1; i < arrDet.length; i++) {
                        var detCampana = arrDet[i].split(';')[0];
                        if (keyComboLG_CampanasAutorizadas.indexOf(detCampana) > -1) {
                            blnOK = true;
                            break;
                        } else {
                            blnOK = false;
                        }
                    }

                    if (blnOK) {
                        cargarImagenEsperando();
                        PageMethods.ValidarComboLG(cadenaDetalleCarrito, getValue('txtNroDoc'), getValue('ddlTipoDocumento'), AgregarCarrito_ValidarComboLG_callback);
                    } else {
                        agregarCarrito();
                    }                
                }
            } else {
                alert(objResponse.Mensaje);
            }
        });
    }
}

function AgregarCarrito_ValidarComboLG_callback(response) {
    quitarImagenEsperando();
    if (response != null) {
        if (response.CodigoError != "0") {
            alert(response.Cadena);
            habilitarBoton('btnGrabar', 'Grabar', true);
            return;
        }
        else {
            agregarCarrito();
        }
    }
}
//INC-SMS_PORTA_INI
//PROY-SMSPORTA::INICIO
function F_SMSPortabilidad(operacion) {
    setValue('hdnOperacionGuardar', operacion);
    setValue('hidOferta', getValue('ddlOferta')); //INC000002547199 -fdq1
    setValue('hiModalidadPortTex', getText('ddlModalidadPorta')); //INC000002628010 - (INC000002547199) - fdq1
	setValue('hidOperText', getText('ddlOperadorCedente')); //INC000002628010 + 3
    var oferta = getValue('ddlOferta');
    var canal = getValue('ddlCanal');
    var tipoDocumento = getValue('ddlTipoDocumento');
    var numeroDoc = getValue('txtNroDoc');
    var codTipoProdActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');

    //PROY 140736
     var hdbuyback = document.getElementById('hdbuyback').value;
     var valorActual = "";
     var filasinbuyback = "";
    var final="";
    var EliminarBuyback=false
    var campBuyback = getValue('hidCadenaDetalle').split('|');
    var materialBuyback = '';
       for (var i = 0; i < campBuyback.length; i++) { 
         valorActual = campBuyback[i].split(';')[15] + '|';
          final=final+valorActual
    }


      var codBuyBack = Key_CodCampaniaBuyBack.split('|');
      var final_ = final.split('|');
      var EliminarBuyback = false;
      for (var i = 0; i < campBuyback.length; i++) {
          for (var j = 0; j < final_.length; j++) {
              if (codBuyBack[i] == final_[j]) {
                  EliminarBuyback = true;
                  break;
              }
          }
    }

  
    if (EliminarBuyback) {
        var validarbuybackcu = hdbuyback.split('|');
        if (validarbuybackcu.length- 1 != campBuyback.length - 1) {
            alert(Key_Msj_Error_Fila_BuyBack)
            return false;
        }
    }


    if (operacion == "G") {
        if (!f_validarGrabar()) {
            return;
        }

        //PROY-140715- INI ANGEL
        var mensaje = document.getElementById("hidMensajeConfiguracion").value;
        if (mensaje != "" && mensaje != null) {
            var arrayMensaje2 = mensaje.split('|')[1];
            var mensaje_contingencia = arrayMensaje2;
            if (mensaje_contingencia != '' && mensaje_contingencia != null) {
                alert(mensaje_contingencia);
            }
        }
        //PROY-140715- INI ANGEL

        var blnPortabilidad = document.getElementById('chkPortabilidad').checked
        if (blnPortabilidad) {
            //INICIO PROY-140419 Autorizar Portabilidad sin PIN
            var chkSmsSupervisor = document.getElementById('chkSmsSupervisor').checked
            if (chkSmsSupervisor) {
                VerValidaSupervisorPorta();
            }
            else {
                ValidarSMSPortabilidad(canal, oferta, codTipoProdActual, tipoDocumento, numeroDoc);
            }
            //FIN PROY-140419 Autorizar Portabilidad sin PIN
        }
        else {

                //PROY-140457-DEBITO AUTOMATICO-INI
                var flagDebitoAuto = Key_flagDebitoAuto;
                if (flagDebitoAuto == "1") {
                    validarComboRetail();
                } else {
            ValidarComboLG();
        }
                //PROY-140457-DEBITO AUTOMATICO-FIN
            }
    }
    else {
        if (!f_validarGrabar()) {
            return;
        }

        //PROY-140715- INI ANGEL
        var mensaje = document.getElementById("hidMensajeConfiguracion").value;
        if (mensaje != "" && mensaje != null) {
            var arrayMensaje2 = mensaje.split('|')[1];
            var mensaje_contingencia = arrayMensaje2;
            if (mensaje_contingencia != '' && mensaje_contingencia != null) {
                alert(mensaje_contingencia);
            }
        }
        //PROY-140715- INI ANGEL

        var blnPortabilidad = document.getElementById('chkPortabilidad').checked
        if (blnPortabilidad) {
            //INICIO PROY-140419 Autorizar Portabilidad sin PIN
            var chkSmsSupervisor = document.getElementById('chkSmsSupervisor').checked
            if (chkSmsSupervisor) {
                VerValidaSupervisorPorta();
            }
            else {
                ValidarSMSPortabilidad(canal, oferta, codTipoProdActual, tipoDocumento, numeroDoc);
            }
            //FIN PROY-140419 Autorizar Portabilidad sin PIN
        }
        else {
                    //PROY-140457-DEBITO AUTOMATICO-INI
                    var flagDebitoAuto = Key_flagDebitoAuto;
                    if (flagDebitoAuto == "1") {
                      validarComboRetail();
                    } else {
            f_enviarCreditos();
        }
                    //PROY-140457-DEBITO AUTOMATICO-FIN
                    
                }
            }
    }

function ValidarSMSPortabilidad(canal, oferta, codTipoProdActual, tipoDocumento, numeroDoc) {

    PageMethods.ValidaPDVSMSPortabilidad(canal, oferta, codTipoProdActual, tipoDocumento, numeroDoc, ValidaPDVSMSPortabilidad_callback); //PROY-140585 F2       

}
//PROY-140585 F2 INI
function ValidaPDVSMSPortabilidad_callback(response) {

    if (response != null) {
        var activaPin = response.Boleano;

        if (response.CodigoError == "1")
        { activaPin = true; }

        if (!response.Boleano && response.Mensaje != "") {
            if (confirm(response.Mensaje)) {
                activaPin = true;
            } else {
                activaPin = false;
}
        }

        PageMethods.ValidarMostrarSMSPortabilidad(response.Canal, response.Oferta, response.codTipoProd, response.TipoDoc, response.NumeroDoc, activaPin, ValidarSMSPortabilidad_callback);

    }
}
//PROY-140585 F2 FIN
function ValidarSMSPortabilidad_callback(response) {
    if (response != null) {
        if (response.CodigoError == "1") {
            alert(response.Mensaje);
        }
        else {
            if (response.Boleano) {
                var codPortabilidad = response.IdFila;
                var nroPortabilidad = response.Cadena;
                if (codPortabilidad != "" && nroPortabilidad != "") {
                    verSMSPortabilidad(nroPortabilidad, codPortabilidad);
                } else {
                            //PROY-140457-DEBITO AUTOMATICO-INI
                            var flagDebitoAuto = Key_flagDebitoAuto;
                            if (flagDebitoAuto == "1") {
                                validarComboRetail();
                            } else {
                    if (getValue('hdnOperacionGuardar') == 'G') {
                        ValidarComboLG();
                    } else {
                        f_enviarCreditos();
                    }
                }
                            //PROY-140457-DEBITO AUTOMATICO-FIN
                        }
            }
            else {
                        //PROY-140457-DEBITO AUTOMATICO-INI
                        var flagDebitoAuto = Key_flagDebitoAuto;
                        if (flagDebitoAuto == "1") {
                            validarComboRetail();
                        } else {
                if (getValue('hdnOperacionGuardar') == 'G') {
                    ValidarComboLG();
                } else {
                    f_enviarCreditos();
                }
            }
                        //PROY-140457-DEBITO AUTOMATICO-FIN
                        
                    }
                }
        }
    }

//INICIO PROY-140419 Autorizar Portabilidad sin PIN
function VerValidaSupervisorPorta() {
    var url = '../consultas/sisact_pop_validar_supervisor.aspx'
    var retVal = window.showModalDialog(url, 'Validar Autentificación de Supervisor', 'dialogWidth:455px;dialogHeight:315px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
    if (retVal == "true") {
                //PROY-140457-DEBITO AUTOMATICO-INI
                var flagDebitoAuto = Key_flagDebitoAuto;
                if (flagDebitoAuto == "1") {
                    validarComboRetail();
                } else {
        if (getValue('hdnOperacionGuardar') == 'G') {
            ValidarComboLG();
        } else {
            f_enviarCreditos();
        }
                }
                //PROY-140457-DEBITO AUTOMATICO-FIN
    } else {
        if (retVal != "" && typeof retVal != 'undefined') {
            alert(retVal);
        }
    }
}
//FIN PROY-140419 Autorizar Portabilidad sin PIN

function verSMSPortabilidad(nroPortabilidad, codPortabilidad) {
    var url = '../consultas/sisact_pop_sms_portabilidad.aspx?';
    url += 'nroDocumento=' + getValue('txtNroDoc') + '&tipoDocumento=' + getValue('ddlTipoDocumento') + '&nroPortabilidad=' + nroPortabilidad + '&codPortabilidad=' + codPortabilidad;
    var retVal = window.showModalDialog(url, 'Validación de PIN Claro', 'dialogWidth:600px;dialogHeight:500px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
    if (retVal.split('|')[0] == "true") {
        setValue('hidCodigoValidador', retVal.split('|')[1]);
                //PROY-140457-DEBITO AUTOMATICO-INI
                var flagDebitoAuto = Key_flagDebitoAuto;
                if (flagDebitoAuto == "1") {
                    validarComboRetail();
                } else {
        if (getValue('hdnOperacionGuardar') == 'G') {
            ValidarComboLG();
        } else {
            f_enviarCreditos();
        }
                }
                //PROY-140457-DEBITO AUTOMATICO-FIN
    } else {
        if (retVal != "") {
            alert(retVal);
        }
    }
}
//PROY-SMSPORTA::FIN
//INC-SMS_PORTA_FIN

//PROY-FULLCLARO -INI
//Habilita o deshabilita Boton FullClaro de acuerdo a configuracion (Tipo producto, Tipo documento, Canal)
function activaBotonFullClaro(tipoProducto) {
    var strCodTipoDoc = getValue('hidTipoDocumento');
    var strNumDoc = getValue('txtNroDoc');
    var strCanal = getValue('hidnCanalValue');
    var strTipoProducto = tipoProducto;
    var strTipoOferta = getValue('ddlOferta');
    var strTipoOperacion = getValue('ddlTipoOperacion');
    var flagPorta = getValue('hidNTienePortabilidadValues');
    PageMethods.ValidarFullClaro(strCodTipoDoc, strNumDoc, strCanal, strTipoProducto, strTipoOferta, strTipoOperacion, flagPorta, ValidarFullClaro_callback);
}

// Habilita o deshabilita según la validacion configurable
function ValidarFullClaro_callback(objResponse) {
    document.getElementById('hdnVerificaBotonFC').value = '';
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            document.getElementById('btnFullClaro2').style.display = 'none';
            document.getElementById('hdnVerificaBotonFC').value = '0';
        } else {
            document.getElementById('btnFullClaro2').style.display = 'none';
            document.getElementById('hdnVerificaBotonFC').value = '1';
        }
    } else {
        document.getElementById('btnFullClaro2').style.display = 'none';
        document.getElementById('hdnVerificaBotonFC').value = '1';
    }
}

// Muestra informacion de lineas fijas
$(document).ready(function () {
    $('input[name="radButton"]').on('click', function () {
        if ($(this).val() === 'Fija') {
            $('#selLineaFija').show();
        }
        else {
            $('#selLineaFija').hide();
        }
    });
});

// Muestra informacion de lineas movil
$(document).ready(function () {
    $('input[name="radButton"]').on('click', function () {
        if ($(this).val() === 'Movil') {
            $('#selLineaMovil').show();
        }
        else {
            $('#selLineaMovil').hide();
        }
    })
});


// Activa al Popup al dar click en botón FullClaro
function ModalFullClaro() {
    document.getElementById('PanelCarga').style.display = 'block';

    //INI - INC000002977281
    if (Key_codFlagBFClaroFijaApagado == 0) {
        //Hidden
        $('#r2').prop('disabled', true);
        $('#idR2').css('display', 'none');
        $('#lblHeadStandar').css('display', 'none');
        $('#lblDangerStandar').css('display', 'none');

        //SHOW
        $('#lblHeadOnlyFija').css('display', 'block');
        $('#lblDangerOnlyFija').css('display', 'block');
    }
    // FIN -INC000002977281 
    
    var strCodTipoDoc = getValue('hidTipoDocumento');
    var strNumDoc = getValue('txtNroDoc');
    var strMovilFC = getValue('hdnLineasMovilFC');
    var strFijaFC = getValue('hdnLineasFijaFC');
    var cantMaxMovil = 1;
    var cantMaxFija = 1;
    CrearTablaFullClaro(strMovilFC, "Movil");
    CrearTablaFullClaro(strFijaFC, "Fija");

    var Validapopup = document.getElementById('CodigoFCP').value;
    var respuesta = true; //INC000003048070 
   // var estadoFullClaro = estadoBSCSBonoFC(strCodTipoDoc, strNumDoc); //INC000003048070


    var estadoFullClaro = getValue('hidEstadoFullClaro');



    if (Validapopup == 0) {
    if (Obligatorio == 0) {
                // return false;
                respuesta = false; //INC000003048070 
    } else {

                document.getElementById('PanelCarga').style.display = 'none'; //INC000003048070 
        return true;
    }
//inicio INC000003048070
            if (respuesta == false) {

                 // var estadoFullClaro = estadoBSCSBonoFC(strCodTipoDoc, strNumDoc); //INC000003048070 descomentar

                 if (estadoFullClaro == "2")
                {

                    document.getElementById('PanelCarga').style.display = 'none';
                    alert(getValue('hidConstMsjeConBonoFC'));
        return true;

                }


            //inicio INC000004280198
            if (getValue('hidPendienteEnvioBonoFC') == "true") {
                document.getElementById('PanelCarga').style.display = 'none';
                return true;
            }
            //fin INC000004280198

                return false;
    }
           //fin INC000003048070 
    } else {
        document.getElementById('PanelCarga').style.display = 'none';
        return true;
    }

}
//inicio INC000003048070

function estadoBSCSBonoFC(vTipoDoc,vNumDoc) {

    var lstDocsEvaluacionn = $("#hidDocsPostpagos").val().split("|");
    var vTipoDocBSCS ="";

    if (vTipoDoc == "06" || vTipoDoc == "10" || vTipoDoc == "20") {

        vTipoDocBSCS = 2;
    }
    else {

        for (var i = 0; i < lstDocsEvaluacionn.length; i++) {
            var documento = lstDocsEvaluacionn[i].split(",");
            if (documento[0].indexOf(vTipoDoc) != -1) {
                vTipoDocBSCS = documento[1];
                break;
            }
        }
    }
      
   
    PageMethods.ConsultarCandidatoBono(vTipoDocBSCS, vNumDoc, estadoBonoFC_CallBack);

    
}

   function estadoBonoFC_CallBack(response) {
     
       if (response != null) {

           return response.EstadoBonoBSCSFC;
       }
       else {
              return "";
           }
   }


//fin INC000003048070

//PROY-FULLCLARO.V2_INI
//Valida el maximo de opciones a marcar
function validacionMaximo() {
    var radioFija = document.getElementById("r1").checked;
    var radioMovil = document.getElementById("r2").checked;
    var cantMaxMovil = 10;
    var cantMaxFija = 1;

    if (radioMovil == true) {
        var contadorMovil = 0;
        for (var i = 0; i < cantCheckGeneradosMovil; i++) {
            if (document.getElementById("chckMovil" + i).checked) {
                contadorMovil++;
            }
        }
        if (contadorMovil > cantMaxMovil) {
            for (var i = 0; i < contadorMovil; i++) {
                if (document.getElementById("chckMovil" + i).checked) {
                    document.getElementById("chckMovil" + i).checked = false;
                }
            }
            contadorMovil = 0;
        }
    }
    if (radioFija == true) {
        var contadorFija = 0;
        for (var i = 0; i < cantCheckGeneradosFija; i++) {
            if (document.getElementById("chckFija" + i).checked) {
                contadorFija++;
            }
        }
        if (contadorFija > cantMaxFija) {
            for (var i = 0; i < contadorFija; i++) {
                if (document.getElementById("chckFija" + i).checked) {
                    document.getElementById("chckFija" + i).checked = false;
                }
            }
            contadorFija = 0;
        }
    }
}

// Poblar las tablas del popup
function CrearTablaFullClaro(strDataOpcion, Opcion) {
    var arrCountFC = "";
    arrCountFC = strDataOpcion.split(',');

    if (cantCheckGeneradosMovil > 0 && cantCheckGeneradosFija > 0) {
        $("#tblBodyFija").html("");
        $("#tblBodyMovil").html("");
        cantCheckGeneradosFija = 0;
        cantCheckGeneradosMovil = 0;
    }

    for (var i = 0; i < arrCountFC.length; i++) {
        var strLineasFC = arrCountFC[i].split('|')[2];
        var strPlanFC = arrCountFC[i].split('|')[6];
        var tabTr = document.createElement("tr");
        var tabTd1 = document.createElement("td");
        var tabTd2 = document.createElement("td");
        var tabTd3 = document.createElement("td");
        var tabInput = document.createElement("input");
        tabInput.type = "checkbox";
        tabInput.id = "chck" + Opcion + i;
        tabInput.name = "chck" + Opcion;
        tabInput.onclick = function () { validacionMaximo(this) };
        var txtNode1 = document.createTextNode(strLineasFC);
        var txtNode2 = document.createTextNode(strPlanFC);
        tabTd1.appendChild(txtNode1);
        tabTd2.appendChild(txtNode2);
        tabTd3.appendChild(tabInput);
        tabTr.appendChild(tabTd1);
        tabTr.appendChild(tabTd2);
        tabTr.appendChild(tabTd3);
        document.getElementById('tblBody' + Opcion).appendChild(tabTr);
    }

    if (Opcion == "Movil") {
        cantCheckGeneradosMovil = i;
    }
    if (Opcion == "Fija") {
        cantCheckGeneradosFija = i;
    }
}

function habilitarMarcarTodos() {
    var radioFija = document.getElementById("r1").checked;
    var radioMovil = document.getElementById("r2").checked;

    if (radioFija == true) {
        for (var i = 0; i < cantCheckGeneradosFija; i++) {
            if (document.getElementById("chckFija" + i).checked == true) {
                document.getElementById("chckFija" + i).checked = false;
            }
        }
    }
    if (radioMovil == true) {
        for (var i = 0; i < cantCheckGeneradosMovil; i++) {
            if (document.getElementById("chckMovil" + i).checked == true) {
                document.getElementById("chckMovil" + i).checked = false;
                document.getElementById("chckMovil" + i).disabled = false;
            }
            else {
                document.getElementById("chckMovil" + i).disabled = false;
            }
        }
    }
}

//Envia TMCODE Y SMCODE de planes y servicios para ser validados por reglas de FC.
function evalPlanesServicios(strPlanesServicios) {
    var verificaParamBotonFC = getValue('hdnVerificaBotonFC');
    var strCodTipoProducto = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var hdnCadenaDetalle = getValue('hidCadenaDetalle');
    if (strPlanesServicios != null) {
        if (verificaParamBotonFC == '0') {
            // Invoca PageMethod que valida lineas y planes según reglas de FullClaro mediante servicio.
            PageMethods.ValidarPlanesServicios(strCodTipoProducto, strPlanesServicios, getValue('ddlTipoDocumento'), getValue('txtNroDoc'), hdnCadenaDetalle, evalPlanesServicios_CallBack);
        }
        else {
            if (getValue('hdnFlagPopupProactiva') == '1') {
                verOtrasOpciones();
            }
        }
    }
    else {
        if (getValue('hdnFlagPopupProactiva') == '1') {
            verOtrasOpciones();
        }
    }
}

function evalPlanesServicios_CallBack(objresponse) {
    //Logica para poblar Popup con lineas nuevas que pasen validación FullClaro. Se recorre la cadena respuesta.
    if (objresponse != null) {
        if (objresponse.CodigoFC == "0") {
            if (objresponse.CodigoBotonFC == "0") {
                //Mostrar Popup y poblar Popup: actualiza para nuevas desc_plan y para portas linea.
                var listaProductosFC = objresponse.objDatosClienteFC;
                var listaMoviles = '';
                var listaFijas = '';

                for (i = 0; i < listaProductosFC.length; i++) {
                    if (listaProductosFC[i].linea == null || listaProductosFC[i].linea == '' || listaProductosFC[i].linea == undefined) {
                        listaProductosFC[i].linea = '-';
                    }
                    if (listaProductosFC[i].tipoServicio == "MOVIL") {
                        listaMoviles = listaMoviles + listaProductosFC[i].elegido + '|' + listaProductosFC[i].coId + '|' + listaProductosFC[i].linea + '|' + listaProductosFC[i].tipoServicio
                                                   + '|' + listaProductosFC[i].planPvudb + '|' + listaProductosFC[i].tmCode + '|' + listaProductosFC[i].desTmcode + '|' + listaProductosFC[i].customerId + '|' + listaProductosFC[i].soplnOrden + ',';
                    }
                    if (listaProductosFC[i].tipoServicio == "FIJA") {
                        listaFijas = listaFijas + listaProductosFC[i].elegido + '|' + listaProductosFC[i].coId + '|' + listaProductosFC[i].linea + '|' + listaProductosFC[i].tipoServicio
                                                   + '|' + listaProductosFC[i].planPvudb + '|' + listaProductosFC[i].tmCode + '|' + listaProductosFC[i].desTmcode + '|' + listaProductosFC[i].customerId + '|' + listaProductosFC[i].soplnOrden + ',';
                    }
                }
                listaMoviles = listaMoviles.slice(0, -1);
                listaFijas = listaFijas.slice(0, -1);

                var obligatoriedad = document.getElementById('hdnObligatoriedad').value;

                PageMethods.PintarLogs('INC000004280198 - evalPlanesServicios_CallBack - hdnObligatoriedad: ' + getValue('hdnObligatoriedad') + ' - Antes CodigoFCP: ' + getValue('CodigoFCP'), getValue('txtNroDoc')); // INC000004280198
             
                setValue('CodigoFCP', 0);
               
                if (obligatoriedad == 2) {
                document.getElementById("btnFullClaro2").style.display = "";
                } else {
                    document.getElementById("btnFullClaro2").style.display = "none";
                 }

                PageMethods.PintarLogs('INC000004280198 - evalPlanesServicios_CallBack - Despues CodigoFCP: ' + getValue('CodigoFCP'), getValue('txtNroDoc')); // INC000004280198
					
					
					
                //document.getElementById("btnFullClaro2").style.display = "";
                
                setValue('hdnLineasMovilFC', listaMoviles);
                setValue('hdnLineasFijaFC', listaFijas);

                if (getValue('hdnFlagPopupProactiva') == '1') {
                    verOtrasOpciones();
                }
            }
            else {
                setValue('CodigoFCP', 1);
                document.getElementById("btnFullClaro2").style.display = "none";
                if (getValue('hdnFlagPopupProactiva') == '1') {
                    verOtrasOpciones();
                }
            }
        } else {
            document.getElementById("btnFullClaro2").style.display = "none";
            setValue('CodigoFCP', -1);
            if (getValue('hdnFlagPopupProactiva') == '1') {
                verOtrasOpciones();
            }
        }
    }
    else {
        if (getValue('hdnFlagPopupProactiva') == '1') {
            verOtrasOpciones();
        }
    }
}

// Guarda el valor del beneficio escogido en el popup.
function saveBeneficio() {
    var contador = 0;
    var strMovilFC = getValue('hdnLineasMovilFC');
    var strFijaFC = getValue('hdnLineasFijaFC');
    var arrCountMovilFC = strMovilFC.split(',');
    var arrCountFijaFC = strFijaFC.split(',');
    var msgBeneficioMovil = key_msjBeneficioMovil; //getValue('hdnMsgBeneficioMovil');
    var msgBeneficioFija = key_msjBeneficioFija; //getValue('hdnMsgBeneficioFija');
    var listaFC = '';
    var tipoServicio = '';
    for (var i = 0; i < cantCheckGeneradosMovil; i++) {
        if (document.getElementById("chckMovil" + i).checked == true) {
            contador++;
            listaFC = listaFC + arrCountMovilFC[i].toString() + ',';
            tipoServicio = "Movil";
        }
    }
    for (var i = 0; i < cantCheckGeneradosFija; i++) {
        if (document.getElementById("chckFija" + i).checked == true) {
            contador++;
            listaFC = listaFC + arrCountFijaFC[i].toString() + ',';
            tipoServicio = "Fija";
        }
    }
    if (contador == 0) {
        alert("No se ha seleccionado un Beneficio");
    } else {

        //PROY-140546 Cobro Anticipado de Instalacion        
        var nCantidadProductos = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr').length;
        if (nCantidadProductos > 0) {
            var tipoValidarTipoProducto = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
            if (tipoValidarTipoProducto == codTipoProductoHFC || tipoValidarTipoProducto == codTipoProd3PlayInalam || tipoValidarTipoProducto == codTipoProductoFTTH) {
                var nuevoMAIFullClaro = parseFloat(getValue('hidCaiDescuentoFullClaro'));
                var nMAI = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(1) td:eq(16)').text();                
                if (parseFloat(nMAI) <= parseFloat(nuevoMAIFullClaro)) {
                    nuevoMAIFullClaro = parseFloat('0');
                }
                $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(1) td:eq(16)').text(nuevoMAIFullClaro);
                setValue('hidFlagCaiFullClaro', '1');
                setValue('hidMAI', nMAI);
            }
        }
        //PROY-140546 Cobro Anticipado de Instalacion

        switch (tipoServicio) {
            case "Movil":
                alert(msgBeneficioMovil);
                break;
            case "Fija":
                alert(msgBeneficioFija);
                break;
        }
        //alert("Se ha seleccionado el beneficio FullClaro para " + tipoServicio);
        PageMethods.ObtenerListaBeneficio(listaFC);
        document.getElementById('PanelCarga').style.display = 'none';

        var obligatoriedad = document.getElementById('hdnObligatoriedad').value;

        PageMethods.PintarLogs('INC000004280198 - saveBeneficio - HdnObligatoriedad :' + getValue('hdnObligatoriedad'), getValue('txtNroDoc')); // INC000004280198

        if (obligatoriedad == 2) {
            Obligatorio = 1;
        }
            if (Obligatorio != "1") {

                if (obligatoriedad == 1 || obligatoriedad == 0) {
                    Obligatorio = 1;
                    F_SMSPortabilidad('G');
                }
            } 
        
    }
}

//Cierra el Popup Full Claro
function closeBeneficio() {
    //debugger;
    var obligatoriedad = document.getElementById('hdnObligatoriedad').value;
    if (obligatoriedad != "2") {
        if (obligatoriedad != "1") {
    document.getElementById('PanelCarga').style.display = 'none';
            document.getElementById("btnFullClaro2").style.display = "";
            Obligatorio = 1;
        } else {
            alert("El cliente debe elegir el Beneficio Full Claro");
        }
    } else {
    document.getElementById('PanelCarga').style.display = 'none';
        Obligatorio = 1;
      }
}

//PROY-FULLCLARO.V2_FIN
// PROY-FULLCLARO-FIN

function ValidarComboLG() {
    var cadenaDetalleCarrito = obtenerDetalleCarritoPorta();
    var arrDet = cadenaDetalleCarrito.split('|');
    var blnOK = false;

    for (var i = 1; i < arrDet.length; i++) {
        var detCampana = arrDet[i].split(';')[0];
        if (keyComboLG_CampanasAutorizadas.indexOf(detCampana) > -1) {
            blnOK = true;
            break;
        }
        else {
            blnOK = false;
        }
    }

    if (blnOK) {
        cargarImagenEsperando();
        PageMethods.ValidarComboLG(cadenaDetalleCarrito, getValue('txtNroDoc'), getValue('ddlTipoDocumento'), ValidarComboLG_callback);
    }
    else {
        f_grabar();
    }
}

function ValidarComboLG_callback(response) {
    quitarImagenEsperando();
    if (response != null) {
        if (response.CodigoError != "0") {
            alert(response.Cadena);
            habilitarBoton('btnGrabar', 'Grabar', true);
            return;
        }
        else {
            f_grabar();
        }
    }
}
//FIN PROY-CAMPANA LG

//PROY-2X1
function ValidarPromocionPortabilidad2x1SEC_CallBack(objResponse) {
    if (objResponse != null) {
        if (objResponse.CodigoError == "0") {
            quitarImagenEsperando();
            ValidarPromocionBeneficioPortaCN_SECT(); //IDEA-42590
        }
        else {
            quitarImagenEsperando();
            setEnabled('btnConsultaPrevia', true, 'Boton');
            alert(objResponse.DescripcionError);
        }
    }
} //PROY-2X1       

function grabarSEC() {
    //gaa20151109
    var ifr = self.frames['ifraCondicionesVenta'];
    //PROY-29215 INICIO
    $('#hidFP').val(ifr.obtenerValorCombo("FP"));
    $('#hidCP').val(ifr.obtenerValorCombo("CP"));
    //PROY-29215 FIN


    if (ifr.validarEquipoSinStock() > 0) {
        var strMensaje = constMsjEquipoSinStock;
        if (!confirm(strMensaje)) {
            return;
        }
    }
    //fin gaa20151109

    if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
        // Excepción Respuesta Tipo 7 DataCrédito - Siempre debe mostrarse este mensaje
        if (!confirm("VERIFIQUE LOS NOMBRES Y APELLIDOS DEL CLIENTE. Si son correctos presione Sí, caso contrario la SEC irá a Créditos para validación y corrección respectiva."))
            document.getElementById('hidnCreditosxNombresV').value = 'S';


        //INC000003168568 - G.G.A - INICIO
        if (getValue('hidnCreditosxNombresV') == 'S') {
            var strCredXNomSinAutonomia = 'N';
            setValue('hidnAutonomia', strCredXNomSinAutonomia);
        }
        //INC000003168568 - G.G.A - FIN


        // NO CUMPLE AUTONOMIA
        if ((getValue('hidCreditosxDC7') == 'S') || (getValue('hidnAutonomia') != 'S')) {
            if (!validarSoloPlanesFijo()) {
                if (confirm("SEC no cumple con autonomia, cliente debe ser evaluado por Créditos. ¿Desea adicionalmente adjuntar sustentos de ingreso?"))
                    document.getElementById('hidnAdjuntarIngreso').value = 'S';
            }
        }
    }
    // PROY-26358 - Evalenzs - Inicio -valida si es portabilidad
    if (getValue('hidNTienePortabilidadValues') == 'S')
        f_DeclaracionConocimiento();
    else
        accionGrabar()

    // PROY-26358 - Evalenzs -Fin

}

// PROY-26358 -Evalenzs - Inicio - accionGrabar()

function accionGrabar() {
    if (confirm("Esta Seguro de Grabar la Evaluacion?")) {
        Obligatorio = 0;
        document.getElementById('PanelCarga').style.display = 'none';
        setValue('CodigoFCP', 1);
        //Guardar Info
        setValue('hidNombre', getValue('txtNombre'));
        setValue('hidApePaterno', getValue('txtApePat'));
        setValue('hidApeMaterno', getValue('txtApeMat'));
        setValue('hidRazonSocial', getValue('txtRazonSocial'));
        setValue('hidComentarioPDV', getValue('txtComentarioPDV'));
        setValue('hidnMensajeAutonomiaValue', getValue('txtResultado'));

        //INC000003168568 - G.G.A - INICIO
        if (getValue('hidnCreditosxNombresV') == 'S') {
            setValue('hidnMensajeAutonomiaValue', constTextoNoAprobadoAutonomia);
        }
        //INC000003168568 - G.G.A - FIN


        setValue('hidCodNacionalidad', getValue('ddlNacionalidad')); //PROY-31636
        setValue('hidDesNacionalidad', getText('ddlNacionalidad')); //PROY-31636

        setValue('hidGrupoPaqueteServerV', self.frames['ifraCondicionesVenta'].getValue('hidNGrupoPaqueteValues'));
        setValue('hidNServicioServerV', self.frames['ifraCondicionesVenta'].getValue('hidnPlanServicioValue'));

        PageMethods.MostrarLogs("INC000003848031-accionGrabar" ," hidNServicioServerV : " + getValue('hidNServicioServerV')); //INC000003848031


        PageMethods.session_hidNServicioServerV("INC000003848031- accionGrabar ",  getValue('hidNServicioServerV')); //INC000003848031
   


        setValue('hidNPromocionServerValue', self.frames['ifraCondicionesVenta'].getValue('hidNPromocionValues'));
        setValue('hidEquipoServer', self.frames['ifraCondicionesVenta'].getValue('hidEquiposXfila3Play'));

        //CNH INI
        setValue('hidCodProducto', self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual'));
        setValue('hidDesProducto', self.frames['ifraCondicionesVenta'].getValue('hidTipoProductoActual'));//PROY-140657
        //CNH FIN

        //PROY-140546 Cobro Anticipado de Instalacion        
        var nCantidadProductos = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr').length;
        if (nCantidadProductos > 0) {
            if (getValue('hidCodProducto') == codTipoProductoHFC || getValue('hidCodProducto') == codTipoProd3PlayInalam || getValue('hidCodProducto') == codTipoProductoFTTH) {
                if (getValue('hidFlagCaiFullClaro') == '0') {
                    var nMAI = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(1) td:eq(16)').text(); //VALIDAR RONAL
                    setValue('hidMAI', nMAI);                
                }
            }
        }
        //PROY-140546 Cobro Anticipado de Instalacion

        setValue('hidConcatPrimaServer', self.frames['ifraCondicionesVenta'].getValue('hidConcatPrima')); //PROY-24724-IDEA-28174
        //PROY-26963-F3 - GPRD
        var tipoProdOK = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
        if (getValue('hidNTienePortabilidadValues') == 'S' && getValue('ddlTipoDocumento') == constTipoDocumentoRUC && getValue('hidnAutonomia') == 'N') {
            if (confirm(MensajeAutonomiaAdjuntarSustentos)) {
                setValue('hAdjuntarDocumento', 'S');
            }
        }
        //Fin PROY-26963-F3 - GPRD
        setValue('hidConcatCuotaServer', self.frames['ifraCondicionesVenta'].getValue('hidConcatCuotaIni')); //PROY-30166-IDEA–38863 

        if (getValue('hidNTienePortabilidadValues') == 'S' || getValue('ddlTipoDocumento') == constTipoDocumentoRUC || getValue('hidnAdjuntarIngreso') == 'S') {
            setValue('hidnValueAccion', 'Grabar');
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
// PROY-26358 - Fin _Evalenzs
// PROY-26358 - Inicio - Declaración de Conocimiento - Evalenzs
function f_DeclaracionConocimiento() {
    var url = ''
    url = '../documentos/Sisact_declaracion_conocimiento.aspx?';
    var retVal = window.showModalDialog(url, 'Decl. Conocomiento', 'dialogWidth:705px;dialogHeight:500px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');
    if (retVal != undefined) {
        document.getElementById('hidCadenaItemsCheckP').value = retVal;
        accionGrabar();
    }
    else {
        habilitarBoton('btnGrabar', 'Grabar', true);
        return;
    }
}
//PROY-23568 - Fin - Evalenzs

function f_enviarCreditos() {
    setValue('hidnAdjuntarIngreso', '');
    setValue('hidnCreditosxNombresV', '');
    setValue('hidGrupoPaqueteServerV', '');
    setValue('hidNServicioServerV', '');

  
    PageMethods.session_hidNServicioServerV("INC000003848031-f_enviarCreditos 1",getValue('hidNServicioServerV')); //INC000003848031
    

    setValue('hidNPromocionServerValue', '');
    if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
        //nuevos cambios 03/07/2017
        setValue('hidCodNacionalidad', getValue('ddlNacionalidad')); //PROY-31636
        setValue('hidDesNacionalidad', getText('ddlNacionalidad')); //PROY-31636
    }

    habilitarBoton('btnEnviarCreditos', 'PROCESANDO...', false);

    if (!f_validarGrabar()) {
        habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', true);
        return;
    }

    //gaa20151109
    var ifr = self.frames['ifraCondicionesVenta'];

    //CNH INI
    setValue('hidCodProducto', self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual'));
    setValue('hidDesProducto', self.frames['ifraCondicionesVenta'].getValue('hidTipoProductoActual')); //PROY-140657
    //CNH FIN

    if (ifr.validarEquipoSinStock() > 0) {
        var strMensaje = constMsjEquipoSinStock;
        if (!confirm(strMensaje)) {
            return;
        }
    }
    //fin gaa20151109

    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
        setValue('hidCreditosxAsesor', 'S');
        setValue('hidnMensajeAutonomiaValue', getValue('txtResultado'));

        //Guardar Info
        setValue('hidNombre', getValue('txtNombre'));
        setValue('hidApePaterno', getValue('txtApePat'));
        setValue('hidApeMaterno', getValue('txtApeMat'));
        setValue('hidRazonSocial', getValue('txtRazonSocial'));

        setValue('hidGrupoPaqueteServerV', self.frames['ifraCondicionesVenta'].getValue('hidNGrupoPaqueteValues'));
        setValue('hidNServicioServerV', self.frames['ifraCondicionesVenta'].getValue('hidnPlanServicioValue'));



      
        PageMethods.session_hidNServicioServerV("INC000003848031-f_enviarCreditos 2", getValue('hidNServicioServerV')); //INC000003848031
    

        setValue('hidNPromocionServerValue', self.frames['ifraCondicionesVenta'].getValue('hidNPromocionValues'));
        setValue('hidConcatCuotaServer', self.frames['ifraCondicionesVenta'].getValue('hidConcatCuotaIni')); //PROY-30166-IDEA–38863 
        setValue('hidnValueAccion', 'Grabar');
        frmPrincipal.submit();
    }
    else {
        var url = constPaginaEnviarCreditos;
        url += "?cu=" + getValue('hidUsuarioRed');
        var opciones = "dialogHeight: 350px; dialogWidth: 650px; edge: Raised; center:Yes; help: No; resizable=no; status: No";
        var vRetorno = window.showModalDialog(url, '', opciones);


        PageMethods.MostrarLogs("INC000003848031-f_enviarCreditos"," vRetorno  : " + vRetorno); //INC000003848031
    

        if (vRetorno) {
            setValue('hidCreditosxAsesor', 'S');
            setValue('hidArchivosEnvioCreditos', vRetorno.Archivos);
            setValue('hidComentarioPDV', vRetorno.Comentario);
            setValue('hidnMensajeAutonomiaValue', getValue('txtResultado'));

            //Guardar Info
            setValue('hidNombre', getValue('txtNombre'));
            setValue('hidApePaterno', getValue('txtApePat'));
            setValue('hidApeMaterno', getValue('txtApeMat'));
            setValue('hidRazonSocial', getValue('txtRazonSocial'));

            setValue('hidGrupoPaqueteServerV', self.frames['ifraCondicionesVenta'].getValue('hidNGrupoPaqueteValues'));
            setValue('hidNServicioServerV', self.frames['ifraCondicionesVenta'].getValue('hidnPlanServicioValue'));


            PageMethods.session_hidNServicioServerV("INC000003848031-f_enviarCreditos 3",getValue('hidNServicioServerV')); //INC000003848031
    
    
            setValue('hidNPromocionServerValue', self.frames['ifraCondicionesVenta'].getValue('hidNPromocionValues'));
            setValue('hidConcatCuotaServer', self.frames['ifraCondicionesVenta'].getValue('hidConcatCuotaIni')); //PROY-30166-IDEA–38863 
            if (getValue('hidNTienePortabilidadValues') == 'S' || getValue('hidArchivosEnvioCreditos') != '') {
                setValue('hidnValueAccion', 'Grabar');
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
    var listaTelefono = listaTelefonosEvaluados();
    PageMethods.validarSECRecurrente(getValue('ddlTipoDocumento'), getValue('txtNroDoc'), getValue('ddlOferta'), getValue('ddlCasoEspecial'), cadenaSECRecurrente(), flujo, getValue('ddlTipoOperacion'), listaTelefono, retornarSECRecurrente);
}

function retornarSECRecurrente(objResponse) {
    var valor = objResponse.Cadena;
    var flujo = valor.split('|')[0];
    var nroSEC = valor.split('|')[1];
    var flgReingreso = valor.split('|')[2];
    var flgMig = valor.split('|')[3];

    if (nroSEC == '0') {
        setValue('hidnValueAccion', 'Grabar');
        frmPrincipal.submit();
    }
    else {
        if (flgReingreso != '1') {
            if (flgMig == '1') {
                var str = consMsjErrorSolMigra;
                var res = str.replace("{0}", nroSEC);
                alert(res);
                nuevaEvaluacion();
            } else {
                setValue('hidnValueAccion', 'Grabar');
                frmPrincipal.submit();
            }
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
    var url = constPaginaAdjDocumento;
    url += '?numSolicitud=' + nroSEC;
    abrirVentana(url, '', '800', '600', 'Sustento_de_Ingresos', true);
}

function AdjuntarDocumentos(nroSec) {
    var url = constPaginaAdjDocumentoRuc;
    url += '?nroSec=' + nroSec + '&llamadoDesde=Evaluacion';
    abrirVentana(url, '', '660', '150', 'Acuerdos', false);
}

function verDetalleGarantia() {
    var url = '../consultas/sisact_pop_detalle_garantia.aspx?nroDocumento=' + getValue('txtNroDoc');
    abrirVentana(url, '', '550', '160', '', true);
}

function mostrarTab(tipoProducto) {
    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
    if ((tipoProducto == "BAM" || tipoProducto == "Movil") && getValue('hidNTienePortabilidadValues') == 'S') {
       
       //CAMBIADO POR EL PROY-140335 RF1 INI
        //PROY-140223 IDEA-140462 
//        if (validarEnvioConsultaPrevia()) {
//            document.getElementById('tdCarrito').style.display = '';
//            document.getElementById('tdConsultaPrevia').style.display = 'none';
//        }
//        else {
//            document.getElementById('tdCarrito').style.display = 'none';
//            document.getElementById('tdConsultaPrevia').style.display = '';
//        }
            document.getElementById('tdCarrito').style.display = '';
            document.getElementById('tdConsultaPrevia').style.display = 'none';
        //PROY-140223 IDEA-140462 
        //CAMBIADO POR EL PROY-140335 RF1 INI
        }

    else {
        document.getElementById('tdCarrito').style.display = '';
        document.getElementById('tdConsultaPrevia').style.display = 'none';
    }
    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
    // PROY - 30748 INICIO
    if (tipoProducto == "Movil") {
        document.getElementById('hidProdMovil').value = '1';
    }
    else {
        document.getElementById('hidProdMovil').value = '0';
    }
    // PROY - 30748 FIN
    self.frames['ifraCondicionesVenta'].f_MostrarTab(tipoProducto);
    llenarOperadorCedente();
}

function agregarPlan() {
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCasoEspecial = getValor(ddlCasoEspecial.value, 0);
    var codigoRespServicio = '';
    //PROY-140245
    strFlagCarrito = 'N';
    setValue('hidFlagAgregarCarrito', strFlagCarrito);

    if (strCasoEspecialColab == null) {
        strCasoEspecialColab = '';
    }
    if (flagInicCantProd == false && strCasoEspecialColab.search(strCasoEspecial) > 0 && strCasoEspecial != '') {
        var strNumDoc = getValue('txtNroDoc');
        var strCodTipoDoc = getValue('hidTipoDocumento');

        PageMethods.ObtenerCantidadProductosPorCliente(strCodTipoDoc, strNumDoc, strCasoEspecial, ObtenerCantidadProductosPorCliente_Callback);

    } else {
        //PROY-140245
        validarInsercionFila();
    }
}

//PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
function validacionCampanaBRMS() {
    // Modalidad / Operador Cedente

    //PROY-140743 - IDEA-141192 - Venta en cuotas | INI
    var cantCampanas = self.frames['ifraCondicionesVenta'].obtenerDetalleCarritoCampana();
    var ddlTipoOperacion = document.getElementById('ddlTipoOperacion');

    if (ddlTipoOperacion.value == 25 || ddlTipoOperacion.value == '25') {
        setEnabled('btnAgregarPlan', false, 'Boton');
    }
    //PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

    if (getValue('hidNTienePortabilidadValues') == 'S') {
        if (!validarControl('ddlModalidadPorta', '', 'Seleccione la Modalidad.')) {
            quitarImagenEsperando(); return false;
        }
        if (!validarControl('ddlOperadorCedente', '', 'Seleccione el Operador Cedente.')) {
            quitarImagenEsperando(); return false;
        }
    }

    //IDEA-142010 INICIO
    setValue('hdiValidacionCantidadCampanas', 'N');
    var VarTipoOperacion = getValue('ddlTipoOperacion');
    var cantCampanas = self.frames['ifraCondicionesVenta'].obtenerDetalleCarritoCampana(); //obtenerDetalleCarritoPorta();
    if (cantCampanas != '') {
        PageMethods.ConsultarCantidadCampanasVigentes(cantCampanas, getValue('txtNroDoc'), VarTipoOperacion, function (objResponse) {
            if (!objResponse.Boleano) {
                alert(objResponse.Mensaje);
            }
        });
    }
    //IDEA-142010 INICIO

    var numeroDoc = getValue('txtNroDoc');
    var tipoDocumento = getValue('ddlTipoDocumento');
    var canal = getValue('ddlCanal');
    var mventa = getValue('ddlModalidadVenta');
    var oferta = getValue('ddlOferta');
    var ifr = self.frames['ifraCondicionesVenta'];
    var CodTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var operacion = getValue('ddlTipoOperacion');
    var flagPorta = getValue('hidNTienePortabilidadValues');
	var modcedente = getValue('ddlModalidadPorta') + '_' + getText('ddlModalidadPorta'); //INC000002628010 + 3
    var opeceden = getValue('ddlOperadorCedente') + ';' + getText('ddlOperadorCedente'); //INC000002628010 + 3
    PageMethods.ValidarParametrosCampanaBRMS(numeroDoc, tipoDocumento, canal, mventa, oferta, operacion, CodTipoProductoActual, flagPorta, modcedente, opeceden, function (objResponse) {
        if (objResponse.Boleano) {
            cargarImagenEsperando();
            var strCadenaDatos = cadenaGeneral();
            var strTieneProteccionMovil = 'NO';
            PageMethods.validacionCampanaBRMS(numeroDoc, getValue('hidNroOperacionDC'), strCadenaDatos, strTieneProteccionMovil, getValue('hidBuroConsultado'), CodTipoProductoActual, getValue('hidProdCuentaFact'), validacionCampanaBRMS_Callback);//PROY-140743
        }
        else {
            agregarPlan();
        }
    });
}

function validacionCampanaBRMS_Callback(objResponse) {
    //PROY-140585 F2-INI
    var canal = getValue('ddlCanal');
    var operacion = getValue('ddlTipoOperacion');
    var objStorageJson;
    var cuotas = 0;
    var monto = 0.0;
    var mensajeCuotas = "";
    var objCuotas = { cuota: [] };

    var msj = objResponse.Cadena;
    var arr = new Array();
    var tipOpe = getValue('ddlTipoOperacion');

    var objStorageJson;
    var objJson;
    if (getValue('hidDatosBRMS') == '') {
        objStorageJson = objCuotas;
    } else {
        objStorageJson = JSON.parse(getValue('hidDatosBRMS'));
    }


    //PROY-140585 F2-FIN

    quitarImagenEsperando();
    agregarPlan();

    //PROY-140585 F2-INI
    arr = msj.split('#');

    cuotas = parseInt(arr[0]);
    mont = parseFloat(arr[1]);
    mensajeCuotas = arr[2];
    objJson = { operacion: tipOpe, cuotamax: cuotas, topemax: mont, msjbrms: mensajeCuotas };
    objStorageJson.cuota.push(objJson);
    setValue('hidDatosBRMS', JSON.stringify(objStorageJson));

    for (var i in objStorageJson.cuota) {
        if (objStorageJson.cuota[i].operacion == getValue('ddlTipoOperacion')) {
            if (objStorageJson.cuota[i].msjbrms == "") {
                objStorageJson.cuota.splice(i, 1);
                setValue('hidDatosBRMS', JSON.stringify(objStorageJson));
            }
            else
                objCuotaJson = objStorageJson.cuota[i];
        }
    }
    if (key_CanalPermMsjeBRMSCamp.indexOf(canal) > -1 && key_OperaPermMsjeBRMSCamp.indexOf(operacion) > -1) {
        if (objCuotaJson) {
            cuotas = parseInt(objCuotaJson.cuotamax);
            mont = parseFloat(objCuotaJson.topemax);
            mensajeCuotas = objCuotaJson.msjbrms;
            if (mensajeCuotas && mensajeCuotas != "" && mensajeCuotas == "SI") {

                var str = key_MsjAplicaCuotas;
                str = str.replace("{0}", "\n");
                str = str.replace("{2}", "\n");
                str = str.replace("{1}", cuotas);
                var dato = str.replace("{3}", mont.toLocaleString("es-Mx"));
                alert(dato);
                //alert('Cliente puede aplicar a CUOTAS.' + '\n' + 'CUOTAS: Max. ' + cuotas + '\n' + 'Tope de Equipo: S/ ' + mont.toLocaleString("es-Mx"));
            }
        }
    }
    //PROY-140585 F2 -FIN
}
//PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

function consultaSOTxMigracion_Callback(objResponse) {
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }
    setValue('hidNroSOTMigracion', objResponse.Cadena);
    self.frames['ifraCondicionesVenta'].agregarFila1(true);
}

// INICIO PROY-140245     
function ObtenerCantidadProductosPorCliente_Callback(objResponse) {

    if (objResponse.split('|')[0] == "-1" || objResponse.toString() == "") {
        if (objResponse.split('|')[0] == "-1") {
            alert(objResponse.split('|')[1].toString());
            return;
        }
    }

    //----------------
    var array_productos = objResponse.split('¬');
    var cadenaRespuestaServicio = array_productos[array_productos.length - 1];
    var arrayRespuestaServicio = cadenaRespuestaServicio.split(',');
    var codigoRespServicio = arrayRespuestaServicio[0];
    strMsgServicioValidConteoLineas = arrayRespuestaServicio[1]; // mensaje del servicio;

    if (codigoRespServicio == '0') {
        //-------------------------
        //var array_cantidades;
        //--------------------------
        var arrayCantMaxProductos = strCantMaxPorProducto.split(';');

        for (var i = 0; i < arrayCantMaxProductos.length; i++) {
            var strCodProd = arrayCantMaxProductos[i].split('|')[0];

            switch (strCodProd) {

                case codTipoProductoMovil:
                    cantMovilMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProductoFijo:
                    cantFijoInalMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProductoDTH:
                    cantClaroTvMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProductoBAM:
                    cantBamMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProductoHFC:
                    cant3PlayMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProdInterInalam:
                    cantInterInalMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
                case codTipoProd3PlayInalam:
                    cantPlayInalMax = Number(arrayCantMaxProductos[i].split('|')[1]);
                    break;
            }
        }


        for (var i = 0; i < array_productos.length - 1; i++) {
            strCodProd = array_productos[i].split('|')[0];

            switch (strCodProd) {

                case codTipoProductoMovil:
                    if (Number(cantMovilMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        cantMovilAct = Number(cantMovilMax) - Number(array_productos[i].split('|')[1]);
                        //cantMovilAct = Number(array_cantidades.split('|')[0])
                        setValue('hidcantMovilAct', cantMovilAct);
                        //cantMovilMax = Number(array_cantidades.split('|')[1]);
                    }
                    else {

                        setValue('hidcantMovilAct', cantMovilMax);
                    }
                    break;
                case codTipoProductoFijo:
                    if (Number(cantFijoInalMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        // cantFijoInalAct = Number(array_cantidades.split('|')[0]);
                        cantFijoInalAct = Number(cantFijoInalMax) - Number(array_productos[i].split('|')[1]);
                        setValue('hidcantFijoInalAct', cantFijoInalAct);
                        //cantFijoInalMax = Number(array_cantidades.split('|')[1]);
                    } else {

                        setValue('hidcantFijoInalAct', cantFijoInalMax);
                    }
                    break;
                case codTipoProductoDTH:
                    if (Number(cantClaroTvMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        // cantClaroTvAct = Number(array_cantidades.split('|')[0]);
                        cantClaroTvAct = Number(cantClaroTvMax) - Number(array_productos[i].split('|')[1]);
                        setValue('hidcantClaroTvAct', cantClaroTvAct);
                        //cantClaroTvMax = Number(array_cantidades.split('|')[1]);
                    } else {
                        setValue('hidcantClaroTvAct', cantClaroTvMax);
                    }
                    break;
                case codTipoProductoBAM:
                    if (Number(cantBamMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        //cantBamAct = Number(array_cantidades.split('|')[0]);
                        cantBamAct = Number(cantBamMax) - Number(array_productos[i].split('|')[1]);
                        setValue('hidcantBamAct', cantBamAct);
                        //cantBamMax = Number(array_cantidades.split('|')[1]);
                    } else {
                        setValue('hidcantBamAct', cantBamMax);
                    }
                    break;
                case codTipoProductoHFC:
                    if (Number(cant3PlayMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        //cant3PlayAct = Number(array_cantidades.split('|')[0]);
                        cant3PlayAct = Number(cant3PlayMax) - Number(array_productos[i].split('|')[1]);
                        setValue('hidcant3PlayAct', cant3PlayAct);
                        //cant3PlayMax = Number(array_cantidades.split('|')[1]);
                    } else {
                        setValue('hidcant3PlayAct', cant3PlayMax);
                    }
                    break;
                case codTipoProdInterInalam:
                    if (Number(cantInterInalMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {
                        //cantInterInalAct = Number(array_cantidades.split('|')[0]);
                        cantInterInalAct = Number(cantInterInalMax) - Number(array_productos[i].split('|')[1]);
                        setValue('hidcantInterInalAct', cantInterInalAct);
                        //cantInterInalMax = Number(array_cantidades.split('|')[1]);
                    }
                    else {
                        setValue('hidcantInterInalAct', cantInterInalMax);
                    }
                    break;
                case codTipoProd3PlayInalam:

                    if (Number(cantPlayInalMax) >= Number(array_productos[i].split('|')[1]) && Number(array_productos[i].split('|')[1]) > 0) {

                        //cantPlayInalAct = Number(array_cantidades.split('|')[0]);
                        cantPlayInalAct = Number(cantPlayInalMax) - Number(array_productos[i].split('|')[1]);

                        setValue('hidcantPlayInalAct', cantPlayInalAct);

                        //cantPlayInalMax = Number(array_cantidades.split('|')[1]);
                    } else {
                        setValue('hidcantPlayInalAct', cantPlayInalMax);
                    }
                    break;
            }
        }
        flagInicCantProd = true;
    }

    else {
        flagContinuarEvaluacion = '3';
    }

    validarInsercionFila();
}
// FIN PROY-140245
// INICIO PROY-140245 
function validarInsercionFila() {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][validarInsercionFila()] ", "Entro en la funcion validarInsercionFila()");

    var ifr = self.frames['ifraCondicionesVenta'];
    if (typeof ifr.getValue == 'undefined') {
        return;
    }
    var strCodProd = ifr.getValue('hidCodigoTipoProductoActual');
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    var strCasoEspAct = getValor(ddlCasoEspecial.value, 0);

    if (strCasoEspecialColab.search(strCasoEspAct) > 0 && strCasoEspAct != '' && ddlCasoEspecial != null && flagInicCantProd) {//&& flagContinuarEvaluacion != '3') {

        actualizarCantidadProductosCasoEspColab(strCodProd);
    }

    if (flagContinuarEvaluacion == '1' || flagContinuarEvaluacion == '0' || strCasoEspAct == '') {

        if (!ServRoamingEsContratado()) { return; } //INC000000676006


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
    else {
        if (flagContinuarEvaluacion == '3')
            alert(strMsgServicioValidConteoLineas);
        return;
    }

}

// PROY-140245
function actualizarCantidadProductosCasoEspColab(strCodTipoProd) {
    switch (strCodTipoProd) {
        case codTipoProductoMovil:
            {
                cantMovilAct = Number(getValue('hidcantMovilAct'));

                if (cantMovilAct < cantMovilMax) {
                    cantMovilAct++;
                    setValue('hidcantMovilAct', cantMovilAct);
                    flagContinuarEvaluacion = '1';

                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantMovilAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantMovilMax);
                    alert(strMsgCantProdCasoEspColab);

                }

                break;
            }
        case codTipoProductoFijo:
            {
                cantFijoInalAct = Number(getValue('hidcantFijoInalAct'));


                if (cantFijoInalAct < cantFijoInalMax) {
                    cantFijoInalAct++;
                    setValue('hidcantFijoInalAct', cantFijoInalAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantFijoInalAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantFijoInalMax);
                    alert(strMsgCantProdCasoEspColab);

                }


                break;
            }
        case codTipoProductoDTH:
            {
                cantClaroTvAct = Number(getValue('hidcantClaroTvAct'));


                if (cantClaroTvAct < cantClaroTvMax) {
                    cantClaroTvAct++;
                    setValue('hidcantClaroTvAct', cantClaroTvAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantClaroTvAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantClaroTvMax);
                    alert(strMsgCantProdCasoEspColab);

                }

                break;
            }
        case codTipoProductoBAM:
            {
                cantBamAct = Number(getValue('hidcantBamAct'));


                if (cantBamAct < cantBamMax) {
                    cantBamAct++;
                    setValue('hidcantBamAct', cantBamAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantBamAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantBamMax);
                    alert(strMsgCantProdCasoEspColab);

                }


                break;
            }
        case codTipoProductoHFC:
            {
                cant3PlayAct = Number(getValue('hidcant3PlayAct'));


                if (cant3PlayAct < cant3PlayMax) {
                    cant3PlayAct++;
                    setValue('hidcant3PlayAct', cant3PlayAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cant3PlayAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cant3PlayMax);
                    alert(strMsgCantProdCasoEspColab);

                }


                break;
            }
        case codTipoProdInterInalam:
            {
                cantInterInalAct = Number(getValue('hidcantInterInalAct'));


                if (cantInterInalAct < cantInterInalMax) {
                    cantInterInalAct++;
                    setValue('hidcantInterInalAct', cantInterInalAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantInterInalAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantInterInalMax);
                    alert(strMsgCantProdCasoEspColab);

                }


                break;
            }
        case codTipoProd3PlayInalam:
            {
                cantPlayInalAct = Number(getValue('hidcantPlayInalAct'));


                if (cantPlayInalAct < cantPlayInalMax) {
                    cantPlayInalAct++;
                    setValue('hidcantPlayInalAct', cantPlayInalAct);
                    flagContinuarEvaluacion = '1';
                }
                else {
                    flagContinuarEvaluacion = '2';
                    var strMsgCantProdCasoEspColab = strMsgValidaCantidadLineas;
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('*', cantPlayInalAct);
                    strMsgCantProdCasoEspColab = strMsgCantProdCasoEspColab.replace('#', cantPlayInalMax);
                    alert(strMsgCantProdCasoEspColab);

                }


                break;
            }

    }
}

function restablecerCantProdActCasoEspColab() {
    var ifr = self.frames['ifraCondicionesVenta'];
    var tblMovil = ifr.document.getElementById('tblTablaMovil');
    var cantFilasProdMovil = tblMovil.rows.length;
    var tblFijo = ifr.document.getElementById('tblTablaFijo');
    var cantFilasProdFijo = tblFijo.rows.length;
    var tblInterInal = ifr.document.getElementById('tblTablaInterInalam');
    var cantFilasInterInal = tblInterInal.rows.length;
    var tblBam = ifr.document.getElementById('tblTablaBAM');
    var cantFilasBam = tblBam.rows.length;
    var tblDTH = ifr.document.getElementById('tblTablaDTH');
    var cantFilasDTH = tblDTH.rows.length;
    var tblHFC = ifr.document.getElementById('tblTablaHFC');
    var cantFilasHFC = tblHFC.rows.length;
    var tbl3PlayInalamb = ifr.document.getElementById('tblTabla3PlayInalam');
    var cantFilas3PlayInalamb = tbl3PlayInalamb.rows.length;

    var cantProd = 0;

    if (Number(getValue('hidcantMovilAct')) > 0 && Number(getValue('hidcantMovilAct')) >= cantFilasProdMovil) {
        cantProd = Number(getValue('hidcantMovilAct'));
        cantProd = cantProd - cantFilasProdMovil;
        setValue('hidcantMovilAct', cantProd);
    }

    if (Number(getValue('hidcantFijoInalAct')) > 0 && Number(getValue('hidcantFijoInalAct')) >= cantFilasProdFijo) {
        cantProd = Number(getValue('hidcantFijoInalAct'));
        cantProd = cantProd - cantFilasProdFijo;
        setValue('hidcantFijoInalAct', cantProd);
    }

    if (Number(getValue('hidcantInterInalAct')) > 0 && Number(getValue('hidcantInterInalAct')) >= cantFilasInterInal) {
        cantProd = Number(getValue('hidcantInterInalAct'));
        cantProd = cantProd - cantFilasInterInal;
        setValue('hidcantInterInalAct', cantProd);
    }

    if (Number(getValue('hidcantBamAct')) > 0 && Number(getValue('hidcantBamAct')) >= cantFilasBam) {
        cantProd = Number(getValue('hidcantBamAct'));
        cantProd = cantProd - cantFilasBam;
        setValue('hidcantBamAct', cantProd);
    }

    if (Number(getValue('hidcantClaroTvAct')) > 0 && Number(getValue('hidcantClaroTvAct')) >= cantFilasDTH) {
        cantProd = Number(getValue('hidcantClaroTvAct'));
        cantProd = cantProd - cantFilasDTH;
        setValue('hidcantClaroTvAct', cantProd);
    }

    if (Number(getValue('hidcant3PlayAct')) > 0 && Number(getValue('hidcant3PlayAct')) >= cantFilasHFC) {
        cantProd = Number(getValue('hidcant3PlayAct'));
        cantProd = cantProd - cantFilasHFC;
        setValue('hidcant3PlayAct', cantProd);
    }

    if (Number(getValue('hidcantPlayInalAct')) > 0 && Number(getValue('hidcantPlayInalAct')) >= cantFilas3PlayInalamb) {
        cantProd = Number(getValue('hidcantPlayInalAct'));
        cantProd = cantProd - cantFilas3PlayInalamb;
        setValue('hidcantPlayInalAct', cantProd);
    }

}
//FIN PROY-140245
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
                //INI PROY-31636
                if (getValue('ddlTipoDocumento') == Key_codigoDocPasaporte07 && Key_flagPermitirProductosLTE == "0" && idProducto != codTipoProductoMovil)
                    mostrarTabProducto(idProducto, false);  //FIN PROY-31636
                else if (idProducto == codTipoProdInterInalam) { //PROY-31812 - IDEA-43340 - INICIO
                    if (getValue('ddlTipoOperacion') == "1" && getValue('ddlOferta') == "01" && getValue('hidNTienePortabilidadValues') != 'S') {
                        mostrarTabProducto(idProducto, true);
                    }
                } //PROY-31812 - IDEA-43340 - FIN
                //PROY-140743 - INI
                else if (getValue('ddlTipoOperacion') == "25") {
                    if (idProducto == "01") {
                        mostrarTabProducto(idProducto, true);
                    }
                }
                //PROY-140743 - FIN
                else
                    mostrarTabProducto(idProducto, true);

            }
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
            if ((isVisible('tdMovil') || isVisible('tdBAM')) && getValue('hidNTienePortabilidadValues') == 'S') {
                //CAMBIADO POR PROY-140335 RF1 INI
                //PROY-140223 IDEA-140462 
                //                if (validarEnvioConsultaPrevia()) {
                //                    document.getElementById('tdCarrito').style.display = '';
                //                    document.getElementById('tdConsultaPrevia').style.display = 'none';
                //                }
                //                else {
                //                    document.getElementById('tdCarrito').style.display = 'none';
                //                    document.getElementById('tdConsultaPrevia').style.display = '';
                //                }
                //PROY-140223 IDEA-140462 
                    document.getElementById('tdCarrito').style.display = '';
                    document.getElementById('tdConsultaPrevia').style.display = 'none';
                }
            //CAMBIADO POR PROY-140335 RF1 INI
            else {
                document.getElementById('tdCarrito').style.display = '';
                document.getElementById('tdConsultaPrevia').style.display = 'none';
            }
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
            // PROY - 30748 INICIO
            if (isVisible('tdMovil')) {
                document.getElementById('hidProdMovil').value = '1';
            }
            // PROY - 30748 FIN
            if (isVisible('tdMovil')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('Movil'); return; }
            if (isVisible('tdBAM')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('BAM'); return; }
            if (isVisible('tdDTH')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('DTH'); return; }
            if (isVisible('tdHFC')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('HFC'); return; }
            if (isVisible('tdFijo')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('Fijo'); return; }
            if (isVisible('tdVentaVarios')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('VentaVarios'); return; }
            if (isVisible('td3PlayInalam')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('3PlayInalam'); return; }
            if (isVisible('tdInterInalam')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('InterInalam'); return; } //PROY-31812 - IDEA - 43340

            if (isVisible('tdFTTH')) { self.frames['ifraCondicionesVenta'].f_MostrarTab('FTTH'); return; } //FTTH

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
        var listaVentaCE4Play = constVentaCE4Play;
        var listaPostventaCE4Play = constPostVentaCE4Play;
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
        nroNroMaxPlanes = getParametroGeneral(constNroMaximoPlanesEmpresa);
        nroNroMaxPlanesMovil = getParametroGeneral(constNroMaxPlanesMovilEmpresa);
        nroNroMaxPlanesFijo = getParametroGeneral(constNroMaxPlanesFijoEmpresa);
        nroNroMaxPlanesBAM = getParametroGeneral(constNroMaxPlanesBAMEmpresa);
        nroNroMaxPlanesDTH = getParametroGeneral(constNroMaxPlanesDTHEmpresa);
        nroNroMaxPlanesHFC = getParametroGeneral(constNroMaxPlanesHFCEmpresa);
        nroNroMaxPlanesHFCI = getParametroGeneral(constNroMaxPlanesHFCIEmpresa);
    }
    else {
        nroNroMaxPlanes = getParametroGeneral(constNroMaximoPlanesPersona);
        nroNroMaxPlanesMovil = getParametroGeneral(constNroMaxPlanesMovilPersona);
        nroNroMaxPlanesFijo = getParametroGeneral(constNroMaxPlanesFijoPersona);
        nroNroMaxPlanesBAM = getParametroGeneral(constNroMaxPlanesBAMPersona);
        nroNroMaxPlanesDTH = getParametroGeneral(constNroMaxPlanesDTHPersona);
        nroNroMaxPlanesHFC = getParametroGeneral(constNroMaxPlanesHFCPersona);
        nroNroMaxPlanesHFCI = getParametroGeneral(constNroMaxPlanesHFCIPersona);
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
    var listaCasoEspecial4Play = constVentaCE4Play;
    if (listaCasoEspecial4Play.indexOf('|' + strCodCasoEspecial + '|') > -1) {
        var planesMovil = constPlanMovil4Play;
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
        var planesFijo = constExclusionPlanes4Play;
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
    var topeBloqueoRobo = parseFloat(getParametroGeneral(consTopeBloqueoRobo));
    var topeBloqueoASolicitud = parseFloat(getParametroGeneral(consTopeBloqueoASolicitud));
    var topeLineaDesactivaMorosidad = parseFloat(getParametroGeneral(consTopeLineaDesactivaMorosidad));
    var topeLineaDesactivaMigracion = parseFloat(getParametroGeneral(consTopeLineaDesactivaMigracion));

    if (listaCantMotivoBloqueo != '') {
        var arrListaCantMotivoBloqueo = listaCantMotivoBloqueo.split('|');
        for (var i = 0; i < arrListaCantMotivoBloqueo.length; i++) {
            if (arrListaCantMotivoBloqueo[i] != '') {
                var strMotivo = arrListaCantMotivoBloqueo[i].split(';')[0];
                var nroLineas = arrListaCantMotivoBloqueo[i].split(';')[1];

                if (nroLineas > 0) {
                    if (strMotivo == consMotivoBloqueoRobo) {
                        if (parseFloat(nroLineas) >= parseFloat(topeBloqueoRobo * nroLineasActivas / 100)) {
                            motivosCreditos += '|' + strMotivo;
                            setValue('hidCreditosxLineaDesactiva', 'S');
                            blnAutonomia = false;
                        }
                    }
                    if (strMotivo == consMotivoBloqueoASolicitud) {
                        if (parseFloat(nroLineas) >= parseFloat(topeBloqueoASolicitud * nroLineasActivas / 100)) {
                            motivosCreditos += '|' + strMotivo;
                            setValue('hidCreditosxLineaDesactiva', 'S');
                            blnAutonomia = false;
                        }
                    }
                    if (strMotivo == consMotivoLineaDesactivaMorosidad) {
                        if (parseFloat(nroLineas) >= parseFloat(topeLineaDesactivaMorosidad * nroLineasActivas / 100)) {
                            motivosCreditos += '|' + strMotivo;
                            setValue('hidCreditosxLineaDesactiva', 'S');
                            blnAutonomia = false;
                        }
                    }
                    if (strMotivo == consMotivoLineaDesactivaMigracion) {
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
    var codParametroCanal = consCodGrupoCanalNoErrorTipo7;
    var strCanalExcepcionDC7 = getParametroGeneral(codParametroCanal);

    var blnExcepcionDC7 = (strCanalExcepcionDC7.indexOf(strCanal) > -1);

    var codParametroDoc = consCodGrupoDocNoErrorTipo7;
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
    url += 'tipo=' + 1 + '&strTipoDoc=' + strTipoDocClie + '&strNumDoc=' + strNroDocClie + '&strTelefono=';
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
        setValue('hidnTipoOfertaValue', ddlOferta.value);
        setValue('hidModalidadVenta', ddlModalidadVenta.value);
        document.getElementById('ddlCasoEspecial').selectedIndex = 0;
        setValue('hidCombo1', arrCadena[3]);

        setValue('hidPlazoReno', arrCadena[5]);
        setValue('hidPlanReno', arrCadena[6]);

        inicializarDatosCasoEspecial();
        inicializarPanelCondicionVentaII();

        consultarDataCredito();

        PageMethods.traerDatosReno(getValue('hidnOficinaValue'), arrCadena[0], getValue('ddlOferta'), obtenerFlujo(), getValue('hidEvaluarSoloFijo'), getValue('ddlTipoDocumento'),
                                             getValue('txtNroDoc'), getValor(getValue('ddlCasoEspecial'), 0), getValue('ddlModalidadVenta'), traerDatosReno_Callback);

    }
    else {
        if (getValue('hidNTienePortabilidadValues') == 'S')
            retornarConsultaSEC('');
        else if (getValue('hidBuscarSEC') == 'S')   //ECM s8
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
        //ECM s8
        vienedeSecReno = true;


        //FVERGARA  .:Inicio:.  
        //Implementacion del Activador para Consultas 3G    Date: 18/08/2017

        var strSwicht_3G = GV_Swicht_3G;
        if (strSwicht_3G == '1') {

            mostrar_lineas3g();
        }
        else {

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
        //.:Fin:. 





        /*trDetalleCliente.style.display = '';
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

        self.frames["ifraCondicionesVenta"].window.location.reload(); --fin ecm s8*/
    }
}
//fin gaa20151201

//Inicio PROY-25335 -  Contratación Electronica - Release 0
function mostrarCartaPoder() {
    var canal = getValue('ddlCanal');
    PageMethods.MostrarCheckCartaPoder(canal, MostrarCheckCartaPoder_CallBack);
}

var cartaPoder = '0';

function MostrarCheckCartaPoder_CallBack(response) {
    cartaPoder = response;
    document.getElementById('chkCartaPoder').checked = false;
    document.getElementById('hidCartaPoder').value = 'N';
    if (response == '1') {
        tblCartaPoder.style.display = '';
    } else {
        tblCartaPoder.style.display = 'none';
    }
}
//Fin PROY-25335 -  Contratación Electronica - Release 0
//Inicio Proy - 30748 - verOtrasOpciones
function verOtrasOpciones() {
    setValue('hdnFlagPopupProactiva', '0'); //PROY-140439 BRMS CAMPAÑA NAVIDEÑA

    var TotalPlanProac = getValue('hidTotalPlanProac');
    if (TotalPlanProac == "1") {
        MsgEquipoNoPlanesProac = getValue('hidMsgEquipoNoPlanesProac'); //PROY 30748 F2 FALLA MDE
        alert(MsgEquipoNoPlanesProac); //PROY 30748 F2 FALLA MDE
        return;
    }
    var cnttablaMovil = $('#ifraCondicionesVenta').contents().find('#tblTablaMovil tr').length;
    var ifrConVenta = self.frames['ifraCondicionesVenta'];
    var tipoProd = ifrConVenta.getValue('hidTipoProductoActual');
    var CodOficina = getValue('hidnOficinaValue');
    var CodOficinaActual = getValue('hidnOficinaActual');

    var campana, plazo, precioVenta, nrcuotas, cuotaini, total, desequipo, porcIni;
    var descampana, desplazo;
    var fila = 0;
    var cargofijo = 0;
    var arrEquipo = new Object();
    var idProducto, costo, precioVenta; //PROY-30748-MGAMBINI
    cargarImagenEsperando();
    //PROY-140335 RF1 INICIO EJRC
    var cadenaPorta;
    //PROY-140335 RF1 FIN EJRC

    switch (tipoProd) {
        case 'Movil':
            var hidCadenaDetalle = getValue('hidCadenaDetalle');
            var arrCadenaDetalle = hidCadenaDetalle.split('|');
            var arrCadenaDatos = arrCadenaDetalle[arrCadenaDetalle.length - 2].split(';');
            campana = arrCadenaDatos[15];
            descampana = arrCadenaDatos[16];
            plazo = arrCadenaDatos[2];
            desplazo = arrCadenaDatos[3];
            codplan = arrCadenaDatos[10];
            desplan = arrCadenaDatos[11];
            codequipo = arrCadenaDatos[17];
            desequipo = arrCadenaDatos[18];

            nrcuotas = arrCadenaDatos[28];
            porcIni = arrCadenaDatos[29];
            cuotaini = arrCadenaDatos[31];
            //PROY-30748-INICIO-MGAMBINI
            idProducto = arrCadenaDatos[1];
            costo = arrCadenaDatos[25];
            precioVenta = arrCadenaDatos[26];
            //PROY-30748-FIN-MGAMBINI
            //PROY-140335 RF1 EJRC INI
            cadenaPorta = arrCadenaDatos[30] + '|' + getValue('hidDatosPorta');
            setValue('hidDatosPortaProactivoCP', cadenaPorta);
            //PROY-140335 RF1 EJRC FIN

            codmodalidad = getValue('ddlModalidadVenta');
            lstMaterial = arrEvaluacion[arrEvaluacion.length - 1] == undefined ? '' : arrEvaluacion[arrEvaluacion.length - 1].lstMaterial;


            arrEquipo.lstMaterial = lstMaterial;
            arrEquipo.codequipo = codequipo;
            arrEquipo.CodOficina = CodOficina;
            arrEquipo.CodOficinaActual = CodOficinaActual;

            if (document.getElementById("hidDatosBRMS").value != "") {
                arrEquipo.TopeMaxCuotas = JSON.parse(document.getElementById("hidDatosBRMS").value).cuota[0]['topemax'];
                //EMMH I
                if (getValue('hidTopeMaximo') != '' || getValue('hidTopeMaximo') != '0') {
                    arrEquipo.TopeMaxCuotas = getValue('hidTopeMaximo');
                }
                //EMMH F
            }
            else {
                arrEquipo.TopeMaxCuotas = "0";
            }


            var arrEval = [];
            var aux = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr');
            aux.splice(0, 1);
            $(aux).each(function (i) {
                $(this).children('td').each(function (j, obj) {

                    if (j == 3) {
                        arrEval[i] = { desplan: obj.innerText };
                        return false;
                    }
                });
            });

            arrEquipo.arrEvaluacion = arrEval;
            break;
    }


    var nroDocumento = document.getElementById('txtNroDoc').value;
    var strCFServAdic = document.getElementById('hidCFServAdicionales').value;
    var idtipoopera = getValue('ddlTipoOperacion');
    var familia = document.getElementById('hidFamilia').value;
    var materialCanje = $('#hdmaterialCanje').val(); //PROY-140736 
    var idFilaProa = $('#ifraCondicionesVenta').contents().find('#hidFilaProa').val();


    var strNewEquipo = idFilaProa + ";" + idProducto + ";" + codequipo + ";" + desequipo + ";" + costo + ";" + precioVenta + ";" + nrcuotas + ";" + porcIni + ";" + cuotaini + "|" //PROY-30748-MGAMBINI


    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][verOtrasOpciones()] hidServiciosAdicionales ", $("#hidServiciosAdicionales").val());

    //cristhian
    var strServAdic = $("#hidServiciosAdicionales").val();
    //fin
    var strvariables = '?nroDocumento=' + nroDocumento + '&strServAdic=' + strServAdic + '&strCFServAdic=' + strCFServAdic + '&campana=' + campana + '&descampana=' + descampana + '&plazo=' + plazo + '&desplazo=' + desplazo + '&desequipo=' + desequipo + '&codplan=' + codplan + '&codmodalidad=' + codmodalidad + '&idtipoopera=' + idtipoopera + '&familia=' + familia + '&CodOficina=' + CodOficina + '&idFlujo=' + obtenerFlujo() + '&nrcuotas=' + nrcuotas + '&porcIni=' + porcIni + '&cuotaini=' + cuotaini + '&idFilaProa=' + idFilaProa + '&strNewEquipo=' + strNewEquipo + '&cadenaPorta=' + cadenaPorta + '&materialCanje=' + materialCanje; //PROY-30748-MGAMBINI //PROY-140335 RF1 - EJRC //PROY-140736 

    //PROY-140743
    if (idtipoopera == "25") {
        quitarImagenEsperando(); 
    } else {
    f_PlanesProActivos(strvariables, arrEquipo);
}
}

//PROY-140335 RF1 INI
var timer_Proa = null;
var recordCP_Proa = 0;
var recordTotalCP_Proa = 1;
var First = "0";
//PROY-140335 RF1 FIN

function f_PlanesProActivos(Variables, arrVariables) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_PlanesProActivos()] ", "Entro en la funcion f_PlanesProActivos()");



    var url = '';
    habilitarBoton('btnDetalleLineasBolsa', 'Ver Otras Opciones...', false);
    url = 'sisact_popup_evaluacion_proactiva.aspx' + Variables; // dialogHeight:500px;
    var retVal = window.showModalDialog(url, arrVariables, 'dialogWidth:1300px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no');

    if (retVal != undefined) {
        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_PlanesProActivos()] JSON.stringify(retVal) ", JSON.stringify(retVal));
    } else {
        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_PlanesProActivos()] JSON.stringify(retVal) ", "retVal es undefined");
    }
    
    quitarImagenEsperando();
    MsgEquipoNoPlanesProac = getValue('hidMsgEquipoNoPlanesProac'); //PROY 30748 F2 FALLA MDE
    switch (retVal) {
        case 'ERROR': alert(MsgEquipoNoPlanesProac); //PROY 30748 F2 FALLA MDE
            quitarImagenEsperando();
            return;
        case 'VALIDACION': alert(MsgEquipoNoPlanesProac); //PROY 30748 F2 FALLA MDE
            quitarImagenEsperando();
            return;
    }
    //            if (retVal === 'ERROR') {
    //                alert("Ocurrio un error al consultar los planes proactivos.");
    //                quitarImagenEsperando();
    //                return;
    //            }
    if (retVal != undefined) {

        PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_PlanesProActivos()] retVal.objSeleccion.EjecucionCP ", retVal.objSeleccion.EjecucionCP);

        //PROY-140335 RF1 INI
        if (retVal.objSeleccion.EjecucionCP == "SI") {

            var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
            var hidDatosPortaProactivoCP = getValue('hidDatosPortaProactivoCP');
            var arrDatosPortaProactivoCP = hidDatosPortaProactivoCP.split('|');
            var listaTelefono = arrDatosPortaProactivoCP[0];
            var FlagCP_Proa = "1";
            setValue('hidFlagPortaProactivoCP', FlagCP_Proa);
            var cadenaConsultaPrevia = arrDatosPortaProactivoCP[1] + ";" + arrDatosPortaProactivoCP[2] + ";" + arrDatosPortaProactivoCP[3] + ";" + arrDatosPortaProactivoCP[4] + ";" + codTipoProductoActual
            + ";" + getValue('ddlModalidadVenta') + ";" + getValue('ddlCanal') + ";" + getValue('hidOficinaUsuario') + ";" + FlagCP_Proa;

            PageMethods.RealizarConsultaPrevia(cadenaConsultaPrevia, listaTelefono, RealizarConsultaPreviaPROA_CallBack);
           

            function RealizarConsultaPreviaPROA_CallBack(objResponse) {

                if (First == "0") {
                    recordCP_Proa = 1;
                    First = "1";
                }
                else {
                    recordCP_Proa = recordCP_Proa + 1;
                }

                if (objResponse == null) {
        quitarImagenEsperando();
                }

                var repeticiones = parseInt(objResponse.Objeto[0]);
                var frecuencia = parseInt(objResponse.Objeto[1]);
                recordTotalCP_Proa = repeticiones;
                var mensajeRecpecionCP = objResponse.Mensaje.split('|');


                if (objResponse.CodigoError == "0") {
                    if (recordCP_Proa == 1) {
                        limpiarConsultaPrevia();
                        alert(mensajeRecpecionCP[1]);
                    }
                    timer_Proa = setTimeout(function () { RealizarConsultaPreviaProaCallBack(objResponse, retVal); }, frecuencia);
                }
                else if (objResponse.CodigoError == "3") {
                    document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
                    var result = recuperarConsultaPrevia(objResponse.Cadena);
        quitarImagenEsperando();
                    if (result.d.CodigoError == "0") {

                        retVal = actualizarConsultaPreviaPROA(result.d.Objeto, retVal);
                        
                        var arrObjetoCP = result.d.Objeto;
                        for (i = 0; i < arrObjetoCP.length; i++) {
                            var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                            var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                            var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                            var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                            var dif = fFechaEnvioCP - fFechaActivacionCP;
                            var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                            var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);

                            if (diasDif <= diasPermitidos) {
                                document.getElementById('hidNroDiasCedenteOP').value = '1';
                            }
                        }

                        f_ReemplazarCarrito(retVal);
        //APOYO-30748-INICIO
        var ifr = self.frames['ifraCondicionesVenta'];
        var strModalidadVenta = getValue('ddlModalidadVenta');
        var strTipoProducto = ifr.getValue('hidCodigoTipoProductoActual');
        var strPlan = retVal.objSeleccion.codplan;
        var strCasoEspecial = getValue('ddlCasoEspecial');
        var strFlagServicioRI = getValue('hidFlagRoamingI');

        PageMethods.ObtenerNuevosServiciosAdicionales(strModalidadVenta, strTipoProducto, strPlan, strCasoEspecial, strFlagServicioRI, ObtenerNuevosServiciosAdicionales_Callback);
        //APOYO-30748-FIN
                        habilitarBoton('btnDetalleLineasBolsa', 'Ver Otras Opciones...', true);
                        quitarImagenEsperando();
                    }
                    else {

                        limpiarConsultaPrevia();
                        alert(mensajeRecpecionCP[2]);
                    }
                }
                else {
                    quitarImagenEsperando();
                    alert(objResponse.DescripcionError);

    }
            }


    }
        else {

            f_ReemplazarCarrito(retVal);
            quitarImagenEsperando();
            //APOYO-30748-INICIO
            var ifr = self.frames['ifraCondicionesVenta'];
            var strModalidadVenta = getValue('ddlModalidadVenta');
            var strTipoProducto = ifr.getValue('hidCodigoTipoProductoActual');
            var strPlan = retVal.objSeleccion.codplan;
            var strCasoEspecial = getValue('ddlCasoEspecial');
            var strFlagServicioRI = getValue('hidFlagRoamingI');

            PageMethods.ObtenerNuevosServiciosAdicionales(strModalidadVenta, strTipoProducto, strPlan, strCasoEspecial, strFlagServicioRI, ObtenerNuevosServiciosAdicionales_Callback);
            //APOYO-30748-FIN
    habilitarBoton('btnDetalleLineasBolsa', 'Ver Otras Opciones...', true);
    quitarImagenEsperando();
}
        //PROY-140335 RF1 FIN
    }
  
}

//APOYO-30748-INICIO
function ObtenerNuevosServiciosAdicionales_Callback(objResponse) {
    if (objResponse.Error) {
        alert("Error al cargar Servicios Adicionales");
    }
    else {
        var ifr = self.frames['ifraCondicionesVenta'];
        var arrServiciosAdicionales = objResponse.Cadena.split("¬");
        var cnttablaResumen = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr').length;
        var fila = 0;
        fila = cnttablaResumen - 1;
        var strPlanServicioNo = "*ID*" + fila + arrServiciosAdicionales[0];
        var strPlanServicio = "*ID*" + fila + arrServiciosAdicionales[1];

        setValue('hidPlanServicioNoTemp', strPlanServicioNo);
        setValue('hidPlanServicioTemp', strPlanServicio);
        setValue('hidFlagBtnVerOtrOpc', '1');

    }

}
//APOYO-30748-FIN

function f_EliminarTemporal(idfila) {
    var id;
    //arrEvaluacion.splice(idfila, 1);
    for (var row = 0; row < arrEvaluacion.length; row++) {
        id = arrEvaluacion[row].id;

        if (id == idfila) {
            arrEvaluacion.splice(row, 1);
            break;
        }
    }
}
// PROY - 30748 INICIO

function f_ReemplazarCarrito(response) {

    var objResponse = response;
    var tabla = objResponse.objSeleccion.tabla;
    var codPlan = objResponse.objSeleccion.codplan;
    var plan = objResponse.objSeleccion.Plan.toUpperCase(); //PROY 30748 F2 MDE
    var codEquipo = objResponse.objSeleccion.CodEquipo;
    var costoEquipo = objResponse.objSeleccion.CostoEquipo;
    var equipo = objResponse.objSeleccion.Equipo;
    var cargoFijo = parseFloat(objResponse.objSeleccion.CargoFijo).toFixed(2);
    var cargoFijoDes = parseFloat(objResponse.objSeleccion.CargoFijo).toFixed(2); //PROY 30748 F2 MDE
    var precioEquipo = objResponse.objSeleccion.PrecioEquipo;
    var autonomia = objResponse.objSeleccion.Autonomia;
    var cuota = '00';
    var porCuotaIni = '0'; //PROY 30748 F2 EMMH
    var CodModalVenta = objResponse.objSeleccion.CodModalVenta;
    var cnttablaResumen = $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr').length;
    var fila = 0;
    var strMontoCuota = 0;
    var EquipoCuota = 'NO';
    var Tipodecobro = objResponse.objSeleccion.Tipodecobro;
    //INICIO PROY-30748
    var servicioAdicional = objResponse.objSeleccion.ServiciosAdicionales;
    var strRentaAdelantada = objResponse.objSeleccion.RentaAdelantada;
    var strMontoRA = objResponse.objSeleccion.MontoRA;
    var strMontoGarantia = objResponse.objSeleccion.MontoGarantia;
    var strTopeMonto = objResponse.objSeleccion.TopeMonto;

    objResponse.objSeleccion.PorIni = objResponse.objSeleccion.PorcenIniOsb; //PROY 30748 F2 EMMH

    var strcuota = objResponse.objSeleccion.cuota + '_' + objResponse.objSeleccion.PorIni; //PROY 30748 F2 EMMH
    //emmh I
    var IdListaPrecio = objResponse.objSeleccion.IdListaPrecio;
    var DesListaPrecio = objResponse.objSeleccion.DesListaPrecio;
    var CodListaPrecio = objResponse.objSeleccion.CodListaPrecio; //PROY 30748 F2 MDE
    //emmh f
    var strCadenaGama = precioEquipo + "_" + costoEquipo;
    var fltMontoCuotaIni = 0.0; //PROY-30166-IDEA–38863
    document.getElementById('txtImporte').value = parseFloat(strRentaAdelantada).toFixed(2);

    var ifr = self.frames['ifraCondicionesVenta'];
    ifr.setValue('hidEPSelect', 'True');
    ifr.setValue('hidStrCuota', strcuota);
    ifr.setValue('hidPEquipo', strCadenaGama);
    fila = cnttablaResumen - 1;
    document.getElementById("hidCuotaP").value = objResponse.objSeleccion.cuota;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_ReemplazarCarrito()] hidNuevoServicio antes 1", document.getElementById("hidNuevoServicio").value);

    document.getElementById("hidNuevoServicio").value = servicioAdicional;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_ReemplazarCarrito()] hidNuevoServicio despues 1", document.getElementById("hidNuevoServicio").value);

    document.getElementById("hidMontoRA").value = strMontoRA;


    //PROY 30748 F2 INI MDE
    var serviciosA = servicioAdicional.split(' ');
    var serviciosB;
    for (var i = 0; i < serviciosA.length; i++) {
        serviciosB = serviciosA[i].split('_');
        for (var a = 0; a < serviciosB.length; a++) {
            if (((a + 1) % 2) == 0) {
                cargoFijoDes = (parseFloat(cargoFijo) - (parseFloat(serviciosB[a]))).toFixed(2);
            }
        }
    }
    //PROY 30748 F2 FIN MDE

    //FIN PROY-30748
    /*calculo de monto de cuotas*/
    if (CodModalVenta == '3') {
        //PROY-30166-IDEA–38863 - INICIO
        var varMaxPorcentajeCuotaInicial = (parseFloat(self.frames['ifraCondicionesVenta'].getValue('hidMaxPorcentajeCuotaInicial')) * 100).toFixed(2);
        var varMsjMontoCuotaMayorAPorcentaje = self.frames['ifraCondicionesVenta'].getValue('hidMontoCuotaMayorAPorcentaje');
        var msjActualiza = self.frames['ifraCondicionesVenta'].getValue('hidMsjActualizCuotaInicial');

        var strCadenaDetalle = getValue('hidCadenaDetalle');
        var arrCadenaDetalle = strCadenaDetalle.split('|');
        var contCadena = arrCadenaDetalle.length;
        var ultFila = contCadena - 2;
        var ultArrCadDeta = arrCadenaDetalle[ultFila];
        var arrayDet = ultArrCadDeta.split(';');
        var strCuotaIni = 0.0;
        var strPorCuotaIniEval = 0;
        var strPrecioEquipoEval = 0;
        var strMonto = 0;
        var strCuotaIniPro = 0.0;

        strPrecioEquipoEval = arrayDet[26];
        strPorCuotaIniEval = arrayDet[29];
        porCuotaIni = objResponse.objSeleccion.PorIni; //% BRMS de Proactiva

        PageMethods.ActualizarSessionesProactiva(porCuotaIni, ActualizarSessionesProactiva_CallBack); //PROY-140579 

        if (self.frames['ifraCondicionesVenta'].getValue('hidConcatCuotaIni')) {
            var strConcat = self.frames['ifraCondicionesVenta'].getValue('hidConcatCuotaIni');
            var arrConcat = strConcat.split('|');
            var strcaracterIni = "ID" + fila;

            for (var i = 1; i < arrConcat.length; i++) {
                var variable = arrConcat[i].split('*');
                if (variable[1] == strcaracterIni) {
                    strCuotaIni = variable[2];
                }
            }
        }

        strCuotaIniPro = (Math.round((parseFloat(precioEquipo) * parseFloat(porCuotaIni) / 100))).toFixed(2);

        fltMontoCuotaIni = strCuotaIniPro;
        msjActualiza = msjActualiza.replace("{0}", fltMontoCuotaIni);
        alert(msjActualiza);
        //PROY-30166-IDEA–38863 - FIN

        cuota = objResponse.objSeleccion.cuota;
        porCuotaIni = objResponse.objSeleccion.PorIni;
        strMontoCuota = ((precioEquipo - fltMontoCuotaIni) / cuota).toFixed(2);
        var cargoFijoBase = (parseFloat(cargoFijo) - (precioEquipo - fltMontoCuotaIni) / cuota).toFixed(2); //PROY-30166-IDEA–38863 
        cargoFijo = (parseFloat(cargoFijoBase) + parseFloat((precioEquipo - fltMontoCuotaIni) / cuota)).toFixed(2); //PROY-30166-IDEA–38863 

        EquipoCuota = 'SI';
        //PROY-30166-IDEA–38863 - INI
        var strCadenaCuotaInicialIfrCondicVenta = self.frames['ifraCondicionesVenta'].getValue('hidCuota');
        var arrConcatIfrCondVta = strCadenaCuotaInicialIfrCondicVenta.split('|');
        var strcaracterIniIfr = "ID" + fila;
        for (var i = 1; i < arrConcatIfrCondVta.length; i++) {
            var strvariable = arrConcatIfrCondVta[i].split('*');
            if (strvariable[1] == strcaracterIniIfr) {
                var strCadenaAntigua = arrConcatIfrCondVta[i];
                var arrVarNuevos = strvariable[2].split(';');
                parseFloat(porCuotaIni).toFixed(0);
                var strCadenaNueva = "*ID" + fila + "*" + objResponse.objSeleccion.cuota + "_" + porCuotaIni + ";" + parseFloat(porCuotaIni).toFixed(2) + ";" + fltMontoCuotaIni + ";" + strMontoCuota + "*/ID" + fila + "*"; //PROY 30748 F2 MDE
            } //PROY-30166-IDEA–38863 - FIN
        }
        strCadenaCuotaInicialIfrCondicVenta = strCadenaCuotaInicialIfrCondicVenta.replace(strCadenaAntigua, strCadenaNueva); //PROY-30166-IDEA–38863
        self.frames['ifraCondicionesVenta'].setValue('hidCuota', strCadenaCuotaInicialIfrCondicVenta); //PROY-30166-IDEA–38863
    }

    document.getElementById('txtImporte').value = parseFloat(strRentaAdelantada).toFixed(2);
    document.getElementById("hidCuotaP").value = objResponse.objSeleccion.cuota;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_ReemplazarCarrito()] hidNuevoServicio antes 2", document.getElementById("hidNuevoServicio").value);

    document.getElementById("hidNuevoServicio").value = servicioAdicional;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][f_ReemplazarCarrito()] hidNuevoServicio despues 2", document.getElementById("hidNuevoServicio").value);

    document.getElementById("hidMontoRA").value = strMontoRA;

    var ifr = self.frames['ifraCondicionesVenta'];
    ifr.setValue('hidEPSelect', 'True');
    ifr.setValue('hidStrCuota', strcuota);
    ifr.setValue('hidPEquipo', strCadenaGama);

    //PROY-140743 - INI
    if (document.getElementById('ddlTipoOperacion').value != '25') {
    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(3)').text(plan);
    }
    //PROY-140743 - FIN

    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(6)').text(cargoFijo);

    //            if (tabla != '1') {               
    if (equipo != "") {

        $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(5)').text(equipo);
    }
    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(7)').text(precioEquipo);

    //            }

    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(8)').text(fltMontoCuotaIni);
    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(9)').text(EquipoCuota); //10
    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(10)').text(cuota); //9
    $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(11)').text(strMontoCuota); //10

    //PROY-140335 RF1 INI
    var hidFlagPortaProactivoCP = getValue('hidFlagPortaProactivoCP');
    if (hidFlagPortaProactivoCP == "1") {
        var EjecucionCP_Des = objResponse.objSeleccion.EjecucionCP_Des;
        $('#ifraCondicionesVenta').contents().find('#tbResumenCompras tr:eq(' + fila + ') td:eq(15)').text(EjecucionCP_Des); 
        var FlagCPPermitida = objResponse.objSeleccion.FlagCPPermitida;
        if (FlagCPPermitida == 1) {
            setValue('hidFlagCPPermitidaProa', FlagCPPermitida);
            alert('El estado de consulta Previa es no permitido');
            habilitarBoton('btnGrabar', 'Grabar', false);
            habilitarBoton('btnEnviarCreditos', 'Enviar a Créditos', false);
    }
    }
    //PROY-140335 RF1 FIN

    var strCadenaDetalle = getValue('hidCadenaDetalle');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var contCadena = arrCadenaDetalle.length;
    var ultFila = contCadena - 2;
    var ultArrCadDeta = arrCadenaDetalle[ultFila];
    var arrayDet = ultArrCadDeta.split(';');
    var CFPrevio = arrayDet[20];

    //emmh i LISTA DE PRECIO
    var arrCadPrecio = arrayDet[24].split('_');
    arrCadPrecio[0] = precioEquipo;
    arrCadPrecio[1] = CodListaPrecio; //PROY 30748 F2 MDE
    arrCadPrecio[2] = DesListaPrecio;
    arrCadPrecio[3] = costoEquipo;
    //PROY 30748 F2 MDE INI
    var newGama = '';
    newGama = arrCadPrecio[0] + "_" + arrCadPrecio[1] + "_" + arrCadPrecio[2] + "_" + arrCadPrecio[3];
    ifr.setValue('hidListaPrecio' + fila, newGama);
    //PROY 30748 F2 MDE FIN
    arrayDet[24] = arrCadPrecio.join('_');
    //emmh f
    arrayDet[9] = codPlan + "_" + cargoFijo + "_" + codPlan; //D49_39_D49_1___2017_ strCadPlan_cargoFijo_strCadPlan_
    arrayDet[10] = codPlan;
    arrayDet[11] = plan;
    arrayDet[23] = parseFloat(cargoFijo);  //+ parseFloat(strMontoCuota);
    arrayDet[28] = cuota;
    arrayDet[29] = porCuotaIni;
    arrayDet[20] = parseFloat(parseFloat(cargoFijoDes) - parseFloat(strMontoCuota)); //PROY 30748 F2 MDE
    arrayDet[19] = strTopeMonto;
    //arrayDet[40] = strMontoRA; //PROY-30748
    arrayDet[40] = porCuotaIni; //PROY-30166-IDEA–38863

    if (arrEvaluacion.length != 0) {
        arrEvaluacion[arrEvaluacion.length - 1].codplan = codPlan;
        arrEvaluacion[arrEvaluacion.length - 1].desplan = plan;
        arrEvaluacion[arrEvaluacion.length - 1].cargoFijo = cargoFijo;
    }

    if (tabla != '1') {
        arrayDet[17] = codEquipo;
        arrayDet[18] = equipo;
        arrayDet[25] = costoEquipo;
        arrayDet[26] = precioEquipo;
        // fin

        arrayDet[31] = precioEquipo;
        if (arrEvaluacion.length != 0) {
            arrEvaluacion[arrEvaluacion.length - 1].codEquipo = codEquipo;
            arrEvaluacion[arrEvaluacion.length - 1].desequipo = equipo;
            arrEvaluacion[arrEvaluacion.length - 1].precioEquipo = precioEquipo;
        }
    }
    arrCadenaDetalle[ultFila] = arrayDet.join(';');
    strCadenaDetalle = arrCadenaDetalle.join('|');
    setValue('hidCadenaDetalle', strCadenaDetalle);
    setValue('hidMontoInicialPro', fltMontoCuotaIni); //PROY-30166-IDEA–38863 
    setValue('hidEsProactiva', "SI"); //PROY-30166-IDEA–38863

    var idfila = ultFila;
    var idNroDocumento = getValue('hidNroDocumento');

    var ddlPlan = ifr.document.getElementById('ddlPlan' + fila); //FALLA PROACTIVA
    ifr.editarPlanProactivo(ddlPlan, fila, fila, newGama); //FALLA PROACTIVA

    PageMethods.ActualizarGarantiaProactiva(idfila, idNroDocumento, arrCadenaDetalle[ultFila], autonomia, CFPrevio, Tipodecobro, strMontoGarantia, strMontoRA, f_asignarDatosEvaluacionProac_Callback);
}

function f_asignarDatosEvaluacionProac_Callback(objResponse) {

    var autonomia = objResponse.Cadena;

    var strPrimerCarrito = getValue('hidResumenCrediticio');
    var arrPrimerCarrito = strPrimerCarrito.split('#');
    var strPlanAutonomia = arrPrimerCarrito[0];
    var arrarPlanAutonomia = strPlanAutonomia.split('|');
    var cont = arrarPlanAutonomia.length;
    var ultimo = cont - 1;
    var ultArray = arrarPlanAutonomia[ultimo];
    var arrayDetalle = ultArray.split(';');

    document.getElementById("hidMontoRA").value = objResponse.Mensaje;

    arrayDetalle[0] = ultArray.split(';')[0]; //PROY 30748 F2 FALLA MDE
    arrayDetalle[1] = autonomia;
    arrarPlanAutonomia[ultimo] = arrayDetalle.join(';');
    strPlanAutonomia = arrarPlanAutonomia.join('|');
    arrPrimerCarrito[0] = strPlanAutonomia;
    var strultimo = arrPrimerCarrito.join('#');
    var arrDatosEval = new Object();
    arrDatosEval.Tipo = 'evalProa';
    arrDatosEval.Cadena = strultimo;
    arrDatosEval.Error = false;
    arrDatosEval.Mensaje = null;
    asignarDatosEvaluacion(arrDatosEval);
    var ifr = self.frames['ifraCondicionesVenta'];
    ifr.calcularCFCarrito();
}
// Proy - 30748 - FIN
//PROY-32129 :: INI
function GrabarDatosAlumno() {
    var vTipoDocumento = getValue('ddlTipoDocumento');
    var vNroDocumento = getValue('txtNroDoc');
    var vCodInstitucion = getValue('ddlInstitucion');
    var vCodAlumno = getValue('txtCodAlumno');
    // PROY-32129 FASE 2 INICIO EIQ
    if (vCodInstitucion == "" || vCodAlumno == "") {
        alert('Completar los campos del caso especial');
    }
    else {
        PageMethods.GrabarDatosAlumno(vTipoDocumento, vNroDocumento, vCodInstitucion, vCodAlumno, GrabarDatosAlumno_CallBack);
    }
}

function GrabarDatosAlumno_CallBack(response) {
    //alert(response); EIQ
    document.getElementById('hidGraboDatosAlumnos').value = "1";
    trDatosAlumnoInstitucion.style.display = 'none';
    HabilitarDetalleGrilla();
    //PROY-32129 FASE 2:: INI
    HabilitarImg();
    habilitarBoton('btnAgregarPlan', 'Agregar Item', true);
    //PROY-32129 FASE 2:: FIN
}

function CancelarGrabarAlumno() {
    var nombreCtrlCamp = document.getElementById('hidIdCampSelec').value;
    var ifrGrillaItems = self.frames['ifraCondicionesVenta'];
    var ctrlCampUniv = ifrGrillaItems.document.getElementById(nombreCtrlCamp);

    ctrlCampUniv.selectedIndex = 0;
    trDatosAlumnoInstitucion.style.display = 'none';
    document.getElementById('hidGraboDatosAlumnos').value = "0";
    HabilitarDetalleGrilla();
    //PROY-32129 FASE 2  INICIO
    HabilitarImg();
    habilitarBoton('btnAgregarPlan', 'Agregar Item', true);
    //PROY-32129 FASE 2  FIN
}
// Proy-32129 FASE 2 INICIO EIQ
function HabilitarImg() {
    var ifrGrillaDetalle2 = self.frames['ifraCondicionesVenta'];
    var strnombreGrillaDetalle2 = 'tblTabla' + ifrGrillaDetalle2.document.getElementById('hidTipoProductoActual').value;
    var tblDetalle2 = ifrGrillaDetalle2.document.getElementById(strnombreGrillaDetalle2);
    var controlImg = tblDetalle2.getElementsByTagName("img");
    for (var i = 0; i < controlImg.length; i++) controlImg[i].style.visibility = 'visible';
}
// Proy-32129 FASE 2  FIN EIQ

function ObtenerCombosCampanas() {
    var ifrGrillaDetalle = self.frames['ifraCondicionesVenta'];
    var strnombreGrillaDetalle = 'tblTabla' + ifrGrillaDetalle.document.getElementById('hidTipoProductoActual').value;
    var tblDetalle = ifrGrillaDetalle.document.getElementById(strnombreGrillaDetalle);
    var intCantFila = tblDetalle.rows.length;
    var intIndexFila = 1;
    var strValoresComboCamp = "";
    var FilaTabla;
    for (intIndexFila = 0; intIndexFila < intCantFila; intIndexFila++) {
        FilaTabla = tblDetalle.rows[intIndexFila];
        strValoresComboCamp = strValoresComboCamp + FilaTabla.cells[2].getElementsByTagName("select")[0].value + "|";
    }
    return strValoresComboCamp.substring(0, strValoresComboCamp.length - 1);
}
function ValidarAnularDatosAlumInst_callback(objResponse) {
    if (objResponse != "") {
        alert(objResponse);
    }
}

function obtenerListaInstituciones_Callback(objResponse) {
    var ddlListaInstitucion = document.getElementById('ddlInstitucion');
    llenarDatosCombo(ddlListaInstitucion, objResponse, true);
}

function HabilitarDetalleGrilla() {
    var ifrGrillaDetalle2 = self.frames['ifraCondicionesVenta'];
    var strnombreGrillaDetalle2 = 'tblTabla' + ifrGrillaDetalle2.document.getElementById('hidTipoProductoActual').value;
    var tblDetalle2 = ifrGrillaDetalle2.document.getElementById(strnombreGrillaDetalle2);

    var controlComboBox = tblDetalle2.getElementsByTagName("select");
    for (var i = 0; i < controlComboBox.length; i++)
        controlComboBox[i].disabled = false;
}

//PROY-32129 :: FIN

//Inicio PROY-29121 - INI
function btnValidaRPLL_Click() {
    if (validarDatosRRLL()) {
        if (getValue('hidCumpleReglaA') == 'SI' || getValue('hidCumpleReglaA') == '') {
            f_ValidarRepresentanteLegal();
        }
        else {
            trCondicionVenta.style.display = '';
            if (getValue('hidFlagPrimerClic') == '') {
                setValue('hidFlagPrimerClic', true);
            }
        }
    }
    else {
        return;
    }
}

function f_ValidarRepresentanteLegal() {
    var strCadenaRRLL = '';
    var strDiasEmpresaToleranciaDefecto = '';
    var strDeudaMinima = '';

    if (document.getElementById('frmPrincipal').hidListaRepresentante.value != '') {
        var arrListaRRLL = document.getElementById('frmPrincipal').hidListaRepresentante.value.split("|");
        var i = 0;
        for (i = 0; i < arrListaRRLL.length; i++) {
            var arrRRLL = arrListaRRLL[i].split(";");
            if (arrRRLL.length > 0) {
                strCadenaRRLL += arrRRLL[0] + ";" + arrRRLL[1] + "|";
            }
        }
        if (strCadenaRRLL != '') {
            strCadenaRRLL = strCadenaRRLL.substring(0, strCadenaRRLL.length - 1);
        }
        PageMethods.consultaClaroRRLL(strCadenaRRLL, getValue('hidnOficinaValue'), getValue('txtFechaNac'), consultaClaroRRLL_CallBack);
    }
}

function consultaClaroRRLL_CallBack(objResponse) {

    if (objResponse.Error) {
        alert(objResponse.Mensaje);
        return;
    }


    var arrParam = objResponse.Cadena.split('#');
    var strSituacionRRLL = objResponse.hidCadenaSituacionRRLL;
    var strMensajeRRLL = objResponse.hidMensajeRRLL;

    setValue('hidEvaluarSoloFijo', arrParam[1]);
    setValue('hidCumpleReglaAClienteRRLL', arrParam[4]);

    ifraRepresentante.setearSituacionRRLL(strSituacionRRLL);

    if (strMensajeRRLL != '') {
        alert(strMensajeRRLL);
    }
    trCondicionVenta.style.display = '';
    if (getValue('hidFlagPrimerClic') == '') {
        setValue('hidFlagPrimerClic', true);
    }
}


//FIN PROY-29121 - FIN

//INC000002510501
function cambiarModalidad(valor) {

    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');

    // Condiciones de Venta
    var tabla = ifr.document.getElementById('tblTabla' + ifr.getValue('hidTipoProductoActual'));
    var nrofila = tabla.rows.length;
    if (nrofila != 0) {

        if (valor == 'M') {
            alert('Debe eliminar el item para cambiar la Modalidad Cedente');
        } else {
            alert('Debe eliminar el item para cambiar el Operador Cedente');
        }
        quitarImagenEsperando();
        return false;
    }
}

//PROY-140457-DEBITO AUTOMATICO-INI
        function validarComboRetail() {
            var ifr = self.frames['ifraCondicionesVenta'];
            var hdnCadenaDetalle = getValue('hidCadenaDetalle');
            PageMethods.validarComboRetail(getValue('hidnOficinaActual'), ifr.getValue('hidCodigoTipoProductoActual'), getValue('ddlTipoOperacion'),
            getValue('hidNTienePortabilidadValues'), getValue('ddlModalidadVenta'), hdnCadenaDetalle, validarComboRetail_CallBack);
        }

        function validarComboRetail_CallBack(objResponse) {
            if (objResponse.Boleano) {
                ValidarDebitoAutomatico();
            }
            else {
                alert(objResponse.Cadena);
            }
        }

        function ValidarDebitoAutomatico() {
            var ifr = self.frames['ifraCondicionesVenta'];
            var hdnCadenaDetalle = getValue('hidCadenaDetalle');
            PageMethods.ValidarDebitoAutomatico(getValue('hidnOficinaActual'), ifr.getValue('hidCodigoTipoProductoActual'), getValue('ddlTipoDocumento'), getValue('ddlTipoOperacion'), 
            getValue('hidNTienePortabilidadValues'), getValue('ddlOferta'), getValue('ddlModalidadVenta'), hdnCadenaDetalle, ValidarDebitoAutomatico_CallBack);
        }

        function ValidarDebitoAutomatico_CallBack(objResponse) {
            if (objResponse.Boleano) {
                VerDebitoAutomatico(objResponse.Obligatorio, objResponse.MontoMaximo);
            } else {
                if (getValue('hdnOperacionGuardar') == 'G') {
                    ValidarComboLG();
                } else {
                    f_enviarCreditos();
                }
            }
        }

        function VerDebitoAutomatico(strValor, strMonto) {

            var tipoDocumento = getValue('ddlTipoDocumento');
            var tipoOferta = getValue('ddlOferta');//PROY-140657
            var nombres = '';
            var strEmail = getValue('txtCorreoElectronico');

            if (tipoDocumento != '06') {
                nombres = getValue('txtNombre') + ' ' + getValue('txtApePat') + ' ' + getValue('txtApeMat');
            }
            else {
                nombres = getValue('txtRazonSocial');
            }
            //INI INICIATIVA 941 - IDEA 142525
            setValue('hidnMontoMaximo', strMonto);
            setValue('hidnTipoDocumento', tipoDocumento);
            setValue('hidnNombres', nombres);

            //FIN INICIATIVA 941 - IDEA 142525
            var url = '../consultas/sisact_pop_debito_automatico.aspx?';
            url += 'nroDocumento=' + getValue('txtNroDoc') + '&nombres=' + nombres + '&tipoDocumento=' + tipoDocumento + '&montoMaximo=' + strMonto + '&tipoOferta=' + tipoOferta + "&correoNoti=" + strEmail; //PROY-140657
            var dialog = 'dialogWidth:830px;dialogHeight:290px;Menubar=no;Status=no;Titlebar=no;Toolbar=no;Location=no';
            var retVal = window.showModalDialog(url, 'Registrar Afiliación / Desafiliación Individuales', dialog);
            if (retVal == "") {
                if (getValue('hdnOperacionGuardar') == 'G') {
	                ValidarComboLG();
                } else {
	                f_enviarCreditos();
                }
            }
            else if (typeof retVal == 'undefined' || retVal == "C") {
                //Restriccion de campanas
                if (!(strValor)) {
                    if (getValue('hdnOperacionGuardar') == 'G') {
                        ValidarComboLG();
                    } else {
                        f_enviarCreditos();
                    }
                }
            }
            else if (retVal != "" && typeof retVal != 'undefined' && retVal != 'C') {
                alert(retVal);
            }

        }
        //PROY-140457-DEBITO AUTOMATICO-FIN

/*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
function validarOfertaDefault() {

    var canal = getValue('ddlCanal');
    var tipoOperacion = getValue('ddlTipoOperacion');
    var tipoDocumento = getValue('ddlTipoDocumento');
    var isPortabilidad = getValue('hidNTienePortabilidadValues');

    if (Key_FlagGeneralOfertaDefault == "1") {
        if (Key_CanalPermitidoOfertaDefault.indexOf(canal) > -1 && Key_DocumentosPermitidosOfertaDefault.indexOf(tipoDocumento) > -1) {
            if (isPortabilidad == "S" && Key_IsPortabilidadOfertaDefault == "S") {
                return true;
            }
            else {
                if (Key_OperacionPermitidaOfertaDefault.indexOf(tipoOperacion) > -1) {
                    return true;
                }
            }
        } else {
            return false;
        }
    }
    else {
        return false;
    }
}
/*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

//INI: PROY-140335 RF1
function asignarDatosEvaluacionPorta(objResponse) {

    //PROY-140743 - INI
    var strOperacion = document.getElementById('ddlTipoOperacion').value;
    if (objResponse.MensajeErrorBRMS != '' && strOperacion == '25') {
        alert(objResponse.MensajeErrorBRMS);
        quitarImagenEsperando();
        return;
    }
    //PROY-140743 - FIN

    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    if (getValue('hidNTienePortabilidadValues') == 'S' && (codTipoProductoActual == codTipoProductoMovil || codTipoProductoActual == codTipoProductoBAM)) {
        var arrParam = objResponse.Cadena.split('#');
        var EjecucionCPBRMS = arrParam[12];
        var hidSecuenceCP = document.getElementById('hidSecuenceCP').value;
        var TodoOK = true;
        var LineasNoPermitidas = '';
        if (getValue('hidEjecucionCPBRMS') == '1') {
            var hidLineasRec = getValue('hidLineasRec');
            var listaTelefono = listaTelefonosEvaluacionCP();
            if (hidLineasRec != '') {
                var ArrayLineasRec = hidLineasRec.split(',');
                for (i = 0; i < ArrayLineasRec.length; i++) {
                    var index = listaTelefono.indexOf(ArrayLineasRec[i]);
                    if (index > -1) {
                        TodoOK = false;
                        LineasNoPermitidas =LineasNoPermitidas+ ArrayLineasRec[i]+',';
                    }
                }
            }
            if (TodoOK == true) {
            asignarDatosEvaluacion(objResponse);
            }
            else {
                var LineasNoPermitidas = LineasNoPermitidas.substring(0, LineasNoPermitidas.length - 1);
                var Mensaje = 'Las Líneas {0} tienen un estado de consulta Previa no permitido';
                mensaje = Mensaje.replace("{0}", LineasNoPermitidas);
                alert(mensaje);
                quitarImagenEsperando();

            }
            return;
        }
        if (EjecucionCPBRMS == 'SI') {
            //140335 - INICIO
            document.getElementById('hidEjecucionCPBRMS').value = '1';
           
            var listaTelefono = listaTelefonosEvaluacionCP();
            var codTipoProductoActual = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
            //AGREGAR PARAMEROS ENTRADA 140335 RF1
            var FlagCP_Proa = "0";
            var cadenaConsultaPrevia = getValue('ddlOperadorCedente') + ";" + getValue('ddlModalidadPorta') + ";" + getValue('ddlTipoDocumento') + ";" + getValue('txtNroDoc') + ";" + codTipoProductoActual + ";" + getValue('ddlModalidadVenta') + ";" + getValue('ddlCanal') + ";" + getValue('hidOficinaUsuario') + ";" + FlagCP_Proa; //PROY-32089 /PROY-140223 IDEA-140462 //PROY-140335 RF1
            //FIN 140335 RF1
            var cadenaDetalleCarrito = obtenerDetalleCarrito();
            cargarImagenEsperando();
            if (getValue('hidCampaniaPortabilidad') == "1") {
                PageMethods.ValidarPromocionPortabilidad2x1(getValue('txtNroDoc'), cadenaConsultaPrevia, cadenaDetalleCarrito, getValue('ddlModalidadVenta'), '', ValidarPromocionPortabilidad2x1_CallBack);
            }
            else {
                PageMethods.RealizarConsultaPrevia(cadenaConsultaPrevia, listaTelefono, RealizarConsultaPrevia_CallBack);
            }

            quitarImagenEsperando();
            //PROY-140335 - FIN EJRC
            
        }
        //modificando
        else {
            asignarDatosEvaluacion(objResponse);
        }
    }
    else {
        asignarDatosEvaluacion(objResponse);
    }
}


function ValidarMejorasPortabilidad() {
    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    if (getValue('hidNTienePortabilidadValues') == 'S') {
        if (codTipoProductoActual == codTipoProductoMovil || codTipoProductoActual == codTipoProductoBAM) {
            return true;
        }
    }
    return false;

}


function RealizarConsultaPreviaProaCallBack(objResponse, retVal) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][RealizarConsultaPreviaProaCallBack()] ", "Entro en la funcion RealizarConsultaPreviaProaCallBack()");

    var blnSalir = 0;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][RealizarConsultaPreviaProaCallBack()] objResponse.CodigoError == '0' || objResponse.CodigoError == '3'", objResponse.CodigoError == "0" || objResponse.CodigoError == "3");

    if (objResponse.CodigoError == "0" || objResponse.CodigoError == "3") {
        var mensajeRecpecionCP = objResponse.Mensaje.split('|');
        if (objResponse.CodigoError == "0") {
            var repeticiones = parseInt(objResponse.Objeto[0]);
            var frecuencia = parseInt(objResponse.Objeto[1]);
            var getRespuesta = 1;

            document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
            var result = recuperarConsultaPrevia(objResponse.Cadena);
            if (result.d.CodigoError == "0") {
                getRespuesta = 0;
                retVal = actualizarConsultaPreviaPROA(result.d.Objeto, retVal);

                var arrObjetoCP = result.d.Objeto;
                for (i = 0; i < arrObjetoCP.length; i++) {
                    var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                    var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                    var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                    var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                    var dif = fFechaEnvioCP - fFechaActivacionCP;
                    var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                    var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);
               
                    if (diasDif <= diasPermitidos) {
                      
                        document.getElementById('hidNroDiasCedenteOP').value = '1';
                        
                    }
                }
                
               
                blnSalir = 1;
              
                document.getElementById('hidLineasSinCP').value = result.d.LineasSinCP;
               
            }
            
            else {
                retVal = actualizarConsultaPreviaPROA(result.d.Objeto, retVal);
            }
            
            quitarImagenEsperando();
            if (getRespuesta == 1 && repeticiones == recordTotalCP) {
                
                limpiarConsultaPrevia();
            }
        }
        else {
            if (objResponse.Cadena != null) {
                quitarImagenEsperando();
                document.getElementById('hidSecuenceCP').value = objResponse.Cadena;
                var result = recuperarConsultaPrevia(objResponse.Cadena);
                if (result.d.CodigoError == "0") {
                    retVal = actualizarConsultaPreviaPROA(result.d.Objeto, retVal);
                    blnSalir = 1;
                    //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                    var arrObjetoCP = result.d.Objeto;
                    for (i = 0; i < arrObjetoCP.length; i++) {
                        var afechaActivacionCP = arrObjetoCP[i].fechaActivacionCP.split('/');
                        var afechaEnvioCP = arrObjetoCP[i].fechaEnvioCP.split('/');
                        var fFechaActivacionCP = Date.UTC(afechaActivacionCP[2], afechaActivacionCP[1] - 1, afechaActivacionCP[0]);
                        var fFechaEnvioCP = Date.UTC(afechaEnvioCP[2], afechaEnvioCP[1] - 1, afechaEnvioCP[0]);
                        var dif = fFechaEnvioCP - fFechaActivacionCP;
                        var diasDif = Math.floor(dif / (1000 * 60 * 60 * 24));
                        // Validar que los dias en operador cedente sean menores a 90 dias
                        var diasPermitidos = parseInt(document.getElementById('hidNroDiasPermitidosOP').value);
                        
                        if (diasDif <= diasPermitidos) {
                            document.getElementById('hidNroDiasCedenteOP').value = '1';
                           
                        }
                    }
                   
                } else {
                   
                    limpiarConsultaPrevia();
                    alert(mensajeRecpecionCP[1]);
                }
            }
        }
    }

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][RealizarConsultaPreviaProaCallBack()] blnSalir ", blnSalir);

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + document.getElementById("hidNroDocumento").value + "][sisact_evaluacion_unificada.js][RealizarConsultaPreviaProaCallBack()] JSON.stringify(retVal) ", JSON.stringify(retVal));

    if (blnSalir == 1) {
        clearTimeout(timer_Proa);
        f_ReemplazarCarrito(retVal);
       
        var ifr = self.frames['ifraCondicionesVenta'];
        var strModalidadVenta = getValue('ddlModalidadVenta');
        var strTipoProducto = ifr.getValue('hidCodigoTipoProductoActual');
        var strPlan = retVal.objSeleccion.codplan;
        var strCasoEspecial = getValue('ddlCasoEspecial');
        var strFlagServicioRI = getValue('hidFlagRoamingI');

        PageMethods.ObtenerNuevosServiciosAdicionales(strModalidadVenta, strTipoProducto, strPlan, strCasoEspecial, strFlagServicioRI, ObtenerNuevosServiciosAdicionales_Callback);
        habilitarBoton('btnDetalleLineasBolsa', 'Ver Otras Opciones...', true);
        quitarImagenEsperando();
       
    }
    else if (recordCP_Proa == recordTotalCP_Proa) {
        clearTimeout(timer_Proa);
    }
    else {
        cargarImagenEsperando();
        RealizarConsultaPreviaPROA_CallBack(objResponse);
    }
}

function actualizarConsultaPreviaPROA(ObjlistaConsultaPrevia,retVal) {
    var listaConsultaPrevia = ObjlistaConsultaPrevia;
    var linea = listaConsultaPrevia[0].numeroLinea;
    var registroCP = obtenerRegistroConsultaPrevia(linea, listaConsultaPrevia);
    retVal.objSeleccion.EjecucionCP_Des = (registroCP.descripcionMotivoCP == null || registroCP.descripcionMotivoCP == "") ? registroCP.descripcionEstadoCP : registroCP.descripcionMotivoCP;
    retVal.objSeleccion.FlagCPPermitida = registroCP.flagCPPermitida;
    return retVal;

}

//FIN: PROY-140335 RF1

function ActualizarSessionesProactiva_CallBack(response) {

}

//INICIATIVA - 803 - INI
function f_validarUmbral() {

    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var hidMontoEquipoVenta = getValue('hidMontoEquipoVenta');
    var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;
    var blnExcepPrecio = document.getElementById('chkExcepPrecio').checked;
    var montocuotas = self.frames['ifraCondicionesVenta'].getValue('txtMontoCuotaIni');
    var txtcuotas = document.getElementById('txtCuotasTienda').value;
    var cbocuotas = getValue('ddlModalidadVenta');


    if (blnTiendaVirtual && blnExcepPrecio) {

        var strtxtPrecioExcep = getValue('txtPrecioExcep');

        if (strtxtPrecioExcep != "" && strtxtPrecioExcep != null) {

            if (Key_FlagApagadoValidacionSubsidio == "0") {

                setValue('hidFlagServicio', '1');
                return true;

            }

            var dblPrecioExcepcion = parseFloat(getValue('txtPrecioExcep')); // Monto que ingresaron el txt
            var dblPorcentajeUmbral = parseFloat(Key_PorcentSubsidio); // Monto que ingresaron el txt
            var dblPrecioEquipoVenta = parseFloat(hidMontoEquipoVenta); // Obtener el Precio Total de SISACT
            var precioMinimo = parseFloat(dblPrecioEquipoVenta - (dblPrecioEquipoVenta * dblPorcentajeUmbral).toFixed(2)).toFixed(2); //Precio Minimo para la evaluacion
            var precioMaximo = parseFloat(dblPrecioEquipoVenta + (dblPrecioEquipoVenta * dblPorcentajeUmbral)).toFixed(2); //Precio Maximo para la evaluacion

            if (dblPrecioExcepcion < precioMinimo || dblPrecioExcepcion > precioMaximo) {

                alert(Key_MsjFactorSubsidio);
                return false;
            } else {

                setValue('hidFlagServicio', '1');
                return true;
            }

        } else {

            alert(Key_MsjErrorExcepPrec);
            return false;
        }
    }
    else {
        return true;
    }
}

function validacionInicialGrabarEnviarCreditos(operacion) {
    setValue('hdnOperacionGuardar', operacion);
    var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;
    var blnExcepPrecio = document.getElementById('chkExcepPrecio').checked;

    if (blnTiendaVirtual && blnExcepPrecio) {
        ValidarReglasExcepxionCuotas(operacion);
    }
    else {
        ModalLineasAdicionales();
       // F_SMSPortabilidad(operacion)
    }
}

function ValidarReglasExcepxionCuotas(operacion) {
    if (!f_validarUmbral()) {
        return;
    }
    var txtcuotas = document.getElementById('txtCuotasTienda').value;
    var blnTiendaVirtual = document.getElementById('chkFlagTienda').checked;
    var blnExcepPrecio = document.getElementById('chkExcepPrecio').checked;

    if (blnTiendaVirtual && blnExcepPrecio) {
        var modalidad = getValue('ddlModalidadVenta');
        if (modalidad == '3') {
            if (txtcuotas != "" && txtcuotas != null) {

                var hidMontoEquipoVenta = getValue('hidMontoEquipoVenta');
                setValue('hidCuotaIncialTienda', txtcuotas)
                PageMethods.ValidarReglasExcepxionCuotas(hidMontoEquipoVenta, ValidarReglasExcepxionCuotas_callback);
            }
            else {
                alert('Debe ingresar cuota inicial de tienda virtual');
                return false;
            }
        } else {
            ModalLineasAdicionales();
            //F_SMSPortabilidad(operacion);
        }
    }
    return true;
}

function ValidarReglasExcepxionCuotas_callback(objResponse) {
    var operacion = getValue('hdnOperacionGuardar');
    if (!objResponse.Boleano) {
        alert('El cliente no cumple con las condiciones para una excepcion de precios en modalidad cuotas');
    }
    else {
        ModalLineasAdicionales();
        //F_SMSPortabilidad(operacion);
    }
}

function MostrarTiendaDatosTV(obj) {

    if (obj.checked) {
        setVisible('txtNroPedidoWeb', true);
        document.getElementById("txtNroPedidoWeb").style.Color = 'Gray';
        setValue('txtNroPedidoWeb', 'Ingrese Nro pedido web');

    } else {
        setValue('txtNroPedidoWeb', '');
        setVisible('txtNroPedidoWeb', false);

    }
}


function MostrarBoxSubsidio(obj) {
    var modalidad = getValue('ddlModalidadVenta');
    setValue('txtPrecioExcep', '');
    setValue('txtCuotasTienda', '');
    if (obj.checked) {
        setVisible('txtPrecioExcep', true);
        if (modalidad == '3') {
            setVisible('lblcuotasTienda', true);
            setVisible('txtCuotasTienda', true);
        }

        setFocus('txtPrecioExcep');
    } else {
        setVisible('txtPrecioExcep', false);
        setVisible('lblcuotasTienda', false);
        setVisible('txtCuotasTienda', false);
    }

}

function validaCaracteresTV(){
validaCaracteres('0123456789');
}

function BorrarDatos() {
    document.getElementById("txtNroPedidoWeb").style.Color = 'Black';
    document.getElementById("txtNroPedidoWeb").className = 'clsInputEnabled';
    var flagdisabled = document.getElementById('chkFlagTienda').disabled
    if (flagdisabled == false) {
    setValue('txtNroPedidoWeb', '');
}
}

//INICIATIVA - 803 - FIN

//INICIO PROY-140546
function borrarDatosCAI() {
    trVentanaCobroAnticipadoInstalacion.style.display = 'none';
    document.getElementById('ddlFranja1').selectedIndex = 0;
    document.getElementById('ddlFranja2').selectedIndex = 0;
    document.getElementById('ddlFranja3').selectedIndex = 0;
    document.getElementById('ddlMedioPago').selectedIndex = 0;
    document.getElementById('chkPublicar').checked = false;
    document.getElementById('txtFechaAgendamiento1').value = '';
    document.getElementById('txtFechaAgendamiento2').value = '';
    document.getElementById('txtFechaAgendamiento3').value = '';
    document.getElementById('txtCasillaCorreoiClaro').value = '';
    setValue('hidFlagCaiFullClaro', '0');
}
//FIN PROY-140546

//INICIO INICIATIVA-932
function AgregarItem_ValidarDireccionIFI() {
    if (Key_FlagGeneralCobertura == '1') {
		var ifr = self.frames['ifraCondicionesVenta'];
    	var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');

    	PageMethods.ValidarDireccionIFI(codTipoProductoActual, AgregarItem_ValidarDireccionIFI_Callback);
	}
	else{
		validacionCampanaBRMS();
	}
}

function AgregarItem_ValidarDireccionIFI_Callback(objResponse) {
    if (objResponse.Boleano) {
        setValue('hidRestringirCoberturaIFI', '1');
    }
    else {
        setValue('hidRestringirCoberturaIFI', '0');
    }

    validacionCampanaBRMS();
}

function AgregarCarrito_ValidarCoberturaIFI() {

    if (Key_FlagGeneralCobertura == '1') {
		cargarImagenEsperando();
	    var ifr = self.frames['ifraCondicionesVenta'];
	    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
	
	    if (codTipoProductoActual == codTipoProdInterInalam) {
	
	        if (validarPlanes()) {
	
	            var strCadenaDetalleEval = self.frames['ifraCondicionesVenta'].consultarItem('');
	            var strPlanDetalleEval = self.frames['ifraCondicionesVenta'].obtenerPlanesEvaluados(strCadenaDetalleEval);
	            strPlanDetalleEval = strPlanDetalleEval.substring(0, strPlanDetalleEval.length - 1);
	
	            var vtipoDocumento = getValue('ddlTipoDocumento');
	            var vnroDocumento = getValue('txtNroDoc');
	            var vnombre;
	            var vapepaterno= getValue('txtApePat');
	            var vapematerno = getValue('txtApeMat');
	            var vrazonsocial = getValue('txtRazonSocial');
	
	            if (vtipoDocumento != '06') {
	                vnombre = getValue('txtNombre');
	            }
	            else {
	                vnombre = getValue('txtRazonSocial');
	            }
	
	            PageMethods.ValidarCoberturaIFI(vtipoDocumento, vnroDocumento, vnombre, vapepaterno, vapematerno,strPlanDetalleEval, ValidarCoberturaIFI_Callback);
	        }
	    }
	    else {
	        quitarImagenEsperando();
	        AgregarCarrito_ValidarComboLG();
	
	    }
	}
	else{
		AgregarCarrito_ValidarComboLG();
	}
}

function ValidarCoberturaIFI_Callback(objResponse) {
    quitarImagenEsperando();
    if (objResponse.Error) {
        alert(objResponse.DescripcionError);
        return;
    }
    else {
        alert(objResponse.Mensaje)
        AgregarCarrito_ValidarComboLG();
    }

}

function validarPlanes() {
    var ifr = self.frames['ifraCondicionesVenta'];
    var codTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');

    var tabla = ifr.document.getElementById('tblTabla' + ifr.getValue('hidTipoProductoActual'));
    var nrofila = tabla.rows.length;
    if (nrofila == 0) {
        alert('Debe agregar al menos un plan.');
        quitarImagenEsperando();
        return false;
    }

    for (var i = 0; i < nrofila; i++) {
        fila = tabla.rows[i];

        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

        var ddlPlan = ifr.document.getElementById('ddlPlan' + idFila);

        if (ddlPlan != null) {
            if (ddlPlan.value.length == 0) {
                if (!ddlPlan.disabled) {
                    ddlPlan.focus();
                    alert('Debe seleccionar un plan');
                    return false;
                }
            }
        }
    }

    return true;
}
//FIN INICIATIVA-932
function consultarLineasAdicionales_Callback(objResponse) {
    if (objResponse.EstadoSession) {

        document.getElementById('lblBeneficio').innerHTML = objResponse.Mensaje;
    }
    else {
        return;
    }
}

//function obtenerLineasCliente_Callback(objResponse) {
//    if (objResponse.EstadoSession) {

//        document.getElementById('lblBeneficio').innerHTML = objResponse.Mensaje;
//    }
//    else {
//        return;
//    }
//}

function validarLineasAdicionales() {

    setValue('hidPoblarTabla', 'S');
    setValue('hidLineasAdicionales', '');
    $("#tblBodyLineasAdic").html("");

    var vTipoDocumentoL = getValue('ddlTipoDocumento');
    var vNumeroDocumentoL = getValue('txtNroDoc');
    var vOperacion = getValue('ddlTipoOperacion');
    var vOferta = getValue('ddlOferta');
    var vModalidadVenta = getValue('ddlModalidadVenta');
    var vIsPortabilidadL = getValue('hidNTienePortabilidadValues');

    var vCodTipoProducto = self.frames['ifraCondicionesVenta'].getValue('hidCodigoTipoProductoActual');
    var vhdnCadenaDetalle = getValue('hidCadenaDetalle');

    PageMethods.validarLineasAdicionales(vTipoDocumentoL, vNumeroDocumentoL, vCodTipoProducto, vOperacion, vOferta, vModalidadVenta, vIsPortabilidadL, vhdnCadenaDetalle, validarLineasAdicionales_Callback);
}

function validarLineasAdicionales_Callback(objResponse) {

    if (objResponse.EstadoSession) {

        setValue('hidLineasAdicionales', objResponse.Cadena);
    }
    
}

function ModalLineasAdicionales() {

    var strlstLineasAdicionales = getValue('hidLineasAdicionales');
    

    if (strlstLineasAdicionales != '') {

        document.getElementById('PanelLineasAdicionales').style.display = 'block';

        if (getValue('hidPoblarTabla') != 'N') {
        var arrLstLineasAdicionales = "";

        arrLstLineasAdicionales = strlstLineasAdicionales.split(',');

        for (var i = 0; i < arrLstLineasAdicionales.length; i++) {
            var strLineasAdic = arrLstLineasAdicionales[i].split('|')[0];
            var strPlanLA = arrLstLineasAdicionales[i].split('|')[1];
            var strDescuento = arrLstLineasAdicionales[i].split('|')[2];
            var tabTr = document.createElement("tr");
            var tabTd1 = document.createElement("td");
            var tabTd2 = document.createElement("td");
                tabTd2.style.textAlign = "left";
            var tabTd3 = document.createElement("td");
            var txtNode1 = document.createTextNode(strLineasAdic);
            var txtNode2 = document.createTextNode(strPlanLA);
            var txtNode3 = document.createTextNode(strDescuento);
            tabTd1.appendChild(txtNode1);
            tabTd2.appendChild(txtNode2);
            tabTd3.appendChild(txtNode3);
            tabTr.appendChild(tabTd1);
            tabTr.appendChild(tabTd2);
            tabTr.appendChild(tabTd3);
            document.getElementById('tblBodyLineasAdic').appendChild(tabTr);
        }

            if (i > 0) {
                setValue('hidPoblarTabla', 'N');
            }           
        }
        
    }
    else {
        var vOperacion = getValue('hdnOperacionGuardar');
        F_SMSPortabilidad(vOperacion);
    }
}

function saveLineasAdicionales() {

    document.getElementById('PanelLineasAdicionales').style.display = 'none';
    var vOperacion = getValue('hdnOperacionGuardar');
    F_SMSPortabilidad(vOperacion);

}

//PROY-140743 - INICIO
function validarOperacionVV() {
    var blnPortabilidad = document.getElementById('chkPortabilidad').checked;

    if (blnPortabilidad || Key_FlagGeneralVtaCuotas == "0" || getValue('hidFlagVentaVV') == "0" || getValue('hidIsClienteClaro') == "0") {
        $("#ddlTipoOperacion" + " option[value='25']").remove();
    }
}
//PROY-140743 - FIN