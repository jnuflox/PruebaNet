function cambiarTipoDocumento(tipodoc, flaglimpiar) {
    if (flaglimpiar == '1') {
    setValue('txtNroDocumento', '');
    setFocus('txtNroDocumento');
}
    document.getElementById('txtNroDocumento').maxLength = getMaxLengthTipoDocGenerico(tipodoc);
    
}

function mostrarDetalle(idEval) {
    PageMethods.consultarServicios(idEval, consultarServicios_callback);
}

function validacionDatos() {
    if (getValue('txtNroDocumento') != '' && getValue('ddlTipoDocumentoEvaCli') != '') {
        loadingGeneric();
        return true;
    }
    else {
        alert('Ingrese numero de documento de busqueda');
        return false;
    }
}

function consultarServicios_callback(response) {
    document.getElementById('panelCarga').style.display = 'none';
    document.getElementById('modalpopup_backgroundElement').style.display = 'none';
    if (response != null && response != "") {
        var ddl = document.getElementById('lbxServiciosAgregados1');
        ddl.length = 0;
        var arrDatos = response.split("|");
        if (arrDatos.length > 0) {
            for (var i = 0; i < arrDatos.length; i++) {
               var option = document.createElement('option');
                option.value = i + 1;
                option.text = arrDatos[i];
                ddl.add(option);
            }
        }
        document.getElementById('panelCarga').style.display = 'block';
        document.getElementById('modalpopup_backgroundElement').style.display = 'block';
        cargarPopUp();

    } else {
        alert("No existen servicios adicionales");
    }
}
$(document).ready(function () {
    $("#btnCancelarServEva").click(function(){
        document.getElementById('panelCarga').style.display = 'none';
        document.getElementById('modalpopup_backgroundElement').style.display = 'none';
    })
});

window.onload = function () {
    if (getValue('hidMostrar') == '1') {
        document.getElementById('trCuadroInformativo').style.display = '';
        document.getElementById('trDetalleClienteEva').style.display = '';
        quitLoadingGeneric();
    } else {
        document.getElementById('trCuadroInformativo').style.display = 'none';
        document.getElementById('trDetalleClienteEva').style.display = 'none';
        quitLoadingGeneric();
    }
    var nroDocumento = getValue('txtNroDocumento');

    if (getValue('ddlTipoDocumentoEvaCli') == constTipoDocumentoRUC && nroDocumento.substr(0, 2) == "20") {
        document.getElementById('trDatosDNI').style.display = 'none';
        document.getElementById('trDatosRUC').style.display = '';
        //document.getElementById('trNomApe').style.display = 'none';
    } else {
        document.getElementById('trDatosRUC').style.display = 'none';
		document.getElementById('trDatosDNI').style.display = '';
        //document.getElementById('trNomApe').style.display = '';
    }
	
	cambiarTipoDocumento(getValue('ddlTipoDocumentoEvaCli'), '0');
}

function cargarPopUp() {
    var popup = document.getElementById('modalpopup_backgroundElement');
    popup.style.color = "white";
    popup.style.position = "absolute";
    popup.style.right = "0px";
    popup.style.top = "0px";
    popup.style.zIndex = "9998";
    popup.style.width = screen.width;
    popup.style.height = screen.height;

    var load = document.getElementById("PanelCarga");
    load.style.position = "absolute";
    load.style.right = screen.width / 3;
    load.style.top = screen.height / 3;
    load.style.left = "50%";
    load.style.zIndex = "9999";
}

function LimpiarControles() {
    //    setValue('ddlTipoDocumentoEvaCli', constTipoDocumentoDNI)
    //    setValue('txtNroDocumento', '');
    //    document.getElementById("dgEvaluacion").innerHTML = '';
    //location.reload();
}
